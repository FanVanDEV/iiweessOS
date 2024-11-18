using SharpCompress.Archives.Tar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Models
{
    public class FileSystemModel
    {
        private readonly Dictionary<string, List<string>> _fileSystem = new Dictionary<string, List<string>>();
        private string _currentDirectory = "";

        public void LoadFromTar(string tarPath)
        {
            if (!File.Exists(tarPath))
            {
                throw new FileNotFoundException("Tar file not found: " + tarPath);
            }

            using (var archive = TarArchive.Open(tarPath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (entry.IsDirectory)
                    {
                        _fileSystem[entry.Key] = new List<string>();
                    }
                    else
                    {
                        var directory = Path.GetDirectoryName(entry.Key);
                        if (directory != null)
                        {
                            directory = directory.Replace("\\", "/") + "/";

                            if (!_fileSystem.ContainsKey(directory))
                            {
                                _fileSystem[directory] = new List<string>();
                            }

                            _fileSystem[directory].Add(Path.GetFileName(entry.Key));
                        }
                    }
                }
            }
        }

        public List<string> ListDirectory(string path)
        {
            path = NormalizePath(path);
            return _fileSystem.ContainsKey(path) ? _fileSystem[path] : new List<string> { "Directory not found" };
        }

        public void ChangeDirectory(string path)
        {
            path = NormalizePath(path);
            if (_fileSystem.ContainsKey(path))
            {
                _currentDirectory = path;
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory not found: {path}");
            }
        }

        public string GetCurrentDirectory() => _currentDirectory;

        public void Remove(string target, bool recursive = false, bool force = false)
        {
            target = NormalizePath(target);

            if (!_fileSystem.ContainsKey(target))
            {
                if (force)
                {
                    return;
                }

                throw new FileNotFoundException($"Target not found: {target}");
            }

            if (_fileSystem[target].Count == 0)
            {
                _fileSystem.Remove(target);
            }
            else
            {
                if (!recursive && _fileSystem[target].Count > 0)
                {
                    throw new InvalidOperationException("Directory is not empty. Use recursive flag to delete.");
                }

                if (recursive)
                {
                    DeleteDirectoryRecursive(target);
                }

                _fileSystem.Remove(target);
            }
        }

        private void DeleteDirectoryRecursive(string directory)
        {
            if (!_fileSystem.ContainsKey(directory))
            {
                return;
            }

            foreach (string file in _fileSystem[directory])
            {
                string filePath = directory + file;
                _fileSystem.Remove(filePath);
            }

            var subdirectories = _fileSystem.Keys
                .Where(key => key.StartsWith(directory) && key != directory)
                .ToList();

            foreach (string subdirectory in subdirectories)
            {
                DeleteDirectoryRecursive(subdirectory);
            }

            _fileSystem.Remove(directory);
        }

        private string NormalizePath(string path)
        {
            if (!path.StartsWith("/"))
            {
                path = _currentDirectory + path;
            }

            string[] parts = path.Replace("\\", "/").Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Stack<string> stack = new Stack<string>();

            foreach (string part in parts)
            {
                if (part == ".")
                {
                    continue;
                }
                else if (part == "..")
                {
                    if (stack.Count > 0)
                    {
                        stack.Pop();
                    }
                }
                else
                {
                    stack.Push(part);
                }
            }

            string normalizedPath = "/" + string.Join("/", stack);

            return normalizedPath.EndsWith("/") ? normalizedPath : normalizedPath + "/";
        }
    }
}

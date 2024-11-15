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

        private string NormalizePath(string path)
        {
            if (!path.StartsWith("/"))
            {
                path = _currentDirectory + path;
            }

            return path.Replace("\\", "/").TrimEnd('/') + "/";
        }
    }
}

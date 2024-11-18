using iiweessOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Commands
{
    internal class LsCommand : ICommand
    {
        private FileSystemModel fs;

        public LsCommand(FileSystemModel fs)
        {
            this.fs = fs;
        }

        public string Execute(string[] args)
        {
            List<string> items = null;

            try
            {
                items = this.fs.ListDirectory(args.Length == 0 ? "." : args[0]);
            }
            catch (Exception)
            {
                return $"ls: cannot access '{args[0]}': No such file or directory";
            }

            if (items == null || items.Count == 0)
            {
                return string.Empty;
            }

            var formattedItems = items
                .OrderBy(item => item)
                .Select(item =>
                {
                    if (item.EndsWith("/"))
                    {
                        return item + "/";
                    }

                    var executableExtensions = new[] { ".exe", ".sh", ".bat" };
                    if (executableExtensions.Any(ext => item.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                    {
                        return item + "*";
                    }

                    return item;
                });

            return string.Join("  ", formattedItems).Trim();
        }

        public string GetHelp()
        {
            return "Usage: ls [FILE]...";
        }
    }
}

using iiweessOS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Commands
{
    public class RmCommand : ICommand
    {
        private FileSystemModel fs;

        public RmCommand(FileSystemModel fs)
        {
            this.fs = fs;
        }

        public string Execute(string[] args)
        {
            bool recursive = false;
            bool force = false;
            string targetPath = null;

            foreach (var arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    if (arg.Contains("r"))
                    {
                        recursive = true;
                    }
                    if (arg.Contains("f"))
                    {
                        force = true;
                    }
                }
                else
                {
                    targetPath = arg;
                }
            }

            try
            {
                fs.Remove(targetPath, recursive, force);
                return "";
            }
            catch (FileNotFoundException)
            {
                return "Error: File or directory not found.";
            }
            catch (InvalidOperationException)
            {
                return "Error: Directory is not empty. Use the -r flag to delete recursively.";
            }
        }

        public string GetHelp()
        {
            return "Usage: rm [-r] [-f] <file/directory>\n" +
                   "-r    Recursively remove directories\n" +
                   "-f    Force remove (do not prompt for confirmation)\n" +
                   "You can combine flags like -rf.";
        }
    }
}

using iiweessOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Commands
{
    public class CdCommand : ICommand
    {
        private FileSystemModel fs;

        public CdCommand(FileSystemModel fs)
        {
            this.fs = fs;
        }

        public string Execute(string[] args)
        {
            try
            {
                this.fs.ChangeDirectory(args[0]);
            } catch (Exception)
            {
                if (args.Length > 0)
                {
                    return $"bash: cd: {args[0]}: No such file or directory";
                }
            }

            return string.Empty;
        }

        public string GetHelp()
        {
            return "cd: usage: cd [dir]";
        }
    }
}

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
            throw new NotImplementedException();
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }
    }
}

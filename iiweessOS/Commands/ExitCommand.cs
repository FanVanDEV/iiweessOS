using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Commands
{
    public class ExitCommand : ICommand
    {
        public string Execute(string[] args)
        {
            return "exit";
        }

        public string GetHelp()
        {
            return string.Empty;
        }
    }
}

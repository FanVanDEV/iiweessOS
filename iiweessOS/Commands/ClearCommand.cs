using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Commands
{
    public class ClearCommand : ICommand
    {
        public string Execute(string[] args)
        {
            return "clear";
        }

        public string GetHelp()
        {
            return string.Empty;
        }
    }
}

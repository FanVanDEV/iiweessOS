using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iiweessOS.Commands
{
    public class DateCommand : ICommand
    {
        public string Execute(string[] args)
        {
            string format = "ddd MMM d H:mm:ss yyyy";
            if (args.Length >= 1 && Regex.IsMatch(args[0], @"\+""(.*?)""")) // +"(any)"
            {
                format = args[0].Split('"')[1];
            }

            return DateTime.Now.ToString(format);
        }

        public string GetHelp()
        {
            return "Usage: date ";
        }
    }
}

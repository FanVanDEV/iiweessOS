using iiweessOS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Controllers
{
    public class ShellController
    {
        private readonly CommandFactory _commandFactory;

        public ShellController(CommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public string ExecuteCommand(string input)
        {
            var parts = input.Trim().Split(' ');
            var commandName = parts[0];
            var args = parts.Length > 1 ? parts.Skip(1).ToArray() : new string[0];

            var command = _commandFactory.GetCommand(commandName);

            if (command != null)
            {
                if (args.Length == 1 && (args[0] == "--help" || args[0] == "-h"))
                {
                    return command.GetHelp();
                }

                return command.Execute(args);
            }

            return $"bash: {commandName}: command not found";
        }
    }
}

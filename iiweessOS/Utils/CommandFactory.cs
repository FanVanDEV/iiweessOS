using iiweessOS.Commands;
using iiweessOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Utils
{
    public class CommandFactory
    {
        private readonly Dictionary<string, ICommand> _commands;

        public CommandFactory(FileSystemModel fs)
        {
            _commands = new Dictionary<string, ICommand>
            {
                { "ls", new LsCommand(fs) },
                { "cd", new CdCommand(fs) },
                { "rm", new RmCommand(fs) },
                { "date", new DateCommand() },
                { "cal", new CalCommand() },
                { "exit", new ExitCommand() }
            };
        }

        public ICommand GetCommand(string commandName)
        {
            return _commands.TryGetValue(commandName, out var command) ? command : null;
        }
    }
}

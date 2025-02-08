using SpiderControl.Core.Commands;
using SpiderControl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderControl.Core.Factories
{
    public class CommandFactory : ICommandFactory
    {
        public ICommand CreateCommand(char commandChar)
        {
            return char.ToUpper(commandChar) switch
            {
                'F' => new ForwardCommand(),
                'R' => new RotateRightCommand(),
                'L' => new RotateLeftCommand(),
                _ => throw new ArgumentException($"Invalid command character: {commandChar}")
            };
        }
    }
}

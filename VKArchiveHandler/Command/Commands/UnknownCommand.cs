using Command.Core;
using System;

namespace Command.Commands {
    public class UnknownCommand : ICommand {
        public void Execute(Arguments args) {
            throw new ArgumentException();
        }
    }
}

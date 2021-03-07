using Command.Core;
using System;

namespace Command.Commands {
    public class UnknownCommand : ICommand {
        public event Action<CommandEventArgs> OnLog;
        public event Action<CommandEventArgs> OnErrorLog;

        public void Execute(Arguments args) {
            throw new ArgumentException();
        }
    }
}

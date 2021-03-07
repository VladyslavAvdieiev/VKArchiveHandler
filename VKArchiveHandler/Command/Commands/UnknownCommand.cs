using Command.Core;
using System;

namespace Command.Commands {
    public class UnknownCommand : ICommand {
        public event Action<string> OnLog;
        public event Action<string> OnErrorLog;

        public void Execute(Arguments args) {
            throw new ArgumentException();
        }
    }
}

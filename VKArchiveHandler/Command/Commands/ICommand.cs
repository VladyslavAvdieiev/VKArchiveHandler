using Command.Core;
using System;

namespace Command.Commands {
    public interface ICommand {
        void Execute(Arguments args);
        event Action<string> OnLog;
        event Action<string> OnErrorLog;
    }
}

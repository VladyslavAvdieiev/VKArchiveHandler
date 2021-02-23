using Command.Core;

namespace Command.Commands {
    public interface ICommand {
        void Execute(Arguments args);
    }
}

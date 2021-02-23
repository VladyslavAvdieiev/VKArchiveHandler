namespace Command.Core {
    public class Arguments {
        public string Command { get; }
        public string SourceLocation { get; }

        public Arguments(string command, string location) {
            Command = command;
            SourceLocation = location;
        }
    }
}

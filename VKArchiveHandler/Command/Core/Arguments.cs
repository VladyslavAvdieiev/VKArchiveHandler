namespace Command.Core {
    public class Arguments {
        public string Command { get; }
        public string SourceLocation { get; }
        public string OutputFolder { get; }

        public Arguments(string command, string location, string output = null) {
            Command = command;
            SourceLocation = location;
            OutputFolder = output;
        }
    }
}

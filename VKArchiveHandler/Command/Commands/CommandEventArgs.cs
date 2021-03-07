using System;

namespace Command.Commands {
    public class CommandEventArgs : EventArgs {
        public string Message { get; }
        public string Url { get; }

        public CommandEventArgs(string message, string url) {
            Message = message;
            Url = url;
        }
    }
}

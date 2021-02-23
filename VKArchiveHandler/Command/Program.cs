using Autofac;
using Command.Commands;
using Command.Core;
using System;

namespace Command {
    class Program {
        static void Main(string[] args) {
            var arguments = ResolveArguments(args);
            IContainer container = SetupIoC();

            var iCommand = container.Resolve<ICommand>(new NamedParameter("Command", arguments.Command));
            iCommand.Execute(arguments);
        }

        private static IContainer SetupIoC() {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommandsModule>();
            return builder.Build();
        }

        private static Arguments ResolveArguments(string[] args) {
            if (args.Length != 2)
                throw new ArgumentException();
            var command = args[0];
            var sourceLocation = args[1];
            return new Arguments(command, sourceLocation);
        }
    }
}

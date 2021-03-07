using Autofac;
using Command.Commands;
using Command.Core;
using System;
using System.Collections.Generic;

namespace Command {
    class Program {
        static void Main(string[] args) {
            var arguments = ResolveArguments(args);
            IContainer container = SetupIoC();

            var errorUrls = new List<string>();
            var iCommand = container.Resolve<ICommand>(new NamedParameter("Command", arguments.Command));
            iCommand.OnLog += url => {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"OK:  {url}");
            };
            iCommand.OnErrorLog += url => {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERR: {url}");
            };
            iCommand.OnErrorLog += url => errorUrls.Add(url);

            iCommand.Execute(arguments);

            if (errorUrls.Count > 0) {

            }
            Console.ReadLine();
        }

        private static IContainer SetupIoC() {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommandsModule>();
            return builder.Build();
        }

        private static Arguments ResolveArguments(string[] args) {
            if (args.Length != 3)
                throw new ArgumentException();
            var command = args[0];
            var sourceLocation = args[1];
            var outputFolder = args[2];
            return new Arguments(command, sourceLocation, outputFolder == "" ? null : outputFolder);
        }
    }
}

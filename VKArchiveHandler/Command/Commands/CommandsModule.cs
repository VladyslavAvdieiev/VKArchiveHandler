using Autofac;
using System.Linq;

namespace Command.Commands {
    public class CommandsModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterAssemblyTypes(typeof(CommandsModule).Assembly)
                .Where(t => t.IsAssignableTo<ICommand>());

            builder
                .Register((c, p) => {
                    var foundType = typeof(CommandsModule).Assembly.ExportedTypes.SingleOrDefault(x =>
                        x.Name == p.Named<string>("Command") + "Command");

                    if (foundType != null) {
                        return (ICommand)c.Resolve(foundType);
                    } else {
                        return c.Resolve<UnknownCommand>();
                    }
                });
        }
    }
}

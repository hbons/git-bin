using GitBin.Commands;
using Objector;

namespace GitBin
{
    public static class ApplicationRegistrations
    {
        public static void Register(IBuilder builder)
        {
            builder.RegisterAssembly(typeof (CommandFactory).Assembly)
                .InNamespaceOf<CommandFactory>()
                .AsImplementedInterfaces();

            builder.RegisterAssembly(typeof (CleanCommand).Assembly)
                .InNamespaceOf<CleanCommand>()
                .AsSelf();

            builder.RegisterFactory<ShowUsageCommand>();
            builder.RegisterFactory<string[], CleanCommand>();
            builder.RegisterFactory<string[], SmudgeCommand>();
        }
    }
}
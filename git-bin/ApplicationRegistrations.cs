using GitBin.Commands;
using GitBin.Remotes;
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

            builder.RegisterAssembly(typeof (IRemote).Assembly)
                .InNamespaceOf<IRemote>()
                .AsImplementedInterfaces();

            builder.RegisterAssembly(typeof (CleanCommand).Assembly)
                .InNamespaceOf<CleanCommand>()
                .AsSelf();

            builder.RegisterFactory<ShowUsageCommand>();
            builder.RegisterFactory<VersionCommand>();
            builder.RegisterFactory<string, PrintCommand>();
            builder.RegisterFactory<string[], CleanCommand>();
            builder.RegisterFactory<string[], ClearCommand>();
            builder.RegisterFactory<string[], SmudgeCommand>();
            builder.RegisterFactory<string[], PushCommand>();
            builder.RegisterFactory<string[], StatusCommand>();
        }
    }
}
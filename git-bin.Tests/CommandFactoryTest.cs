using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitBin;
using GitBin.Commands;
using NUnit.Framework;

namespace git_bin.Tests
{
    [TestFixture]
    public class CommandFactoryTest
    {
        [Test]
        public void GetShowUsageCommand_InvokesShowUsageFactory()
        {
            bool wasShowUsageCommandFactoryCalled = false;

            Func<ShowUsageCommand> factory = () =>
                              {
                                  wasShowUsageCommandFactoryCalled = true;
                                  return null;
                              };

            var target = new CommandFactory(factory, null, null, null, null, null);
            target.GetShowUsageCommand();

            Assert.IsTrue(wasShowUsageCommandFactoryCalled);
        }

        [Test]
        public void GetCommand_InvalidCommandArgument_ReturnsShowUsageCommand()
        {
            var showUsageCommand = new ShowUsageCommand();
            Func<ShowUsageCommand> factory = () => showUsageCommand;

            var target = new CommandFactory(factory, null, null, null, null, null);
            var command = target.GetCommand(new []{"invalid"});

            Assert.AreSame(showUsageCommand, command);
        }

        [Test]
        public void GetCommand_NoCommandArgument_ReturnsShowUsageCommand()
        {
            var showUsageCommand = new ShowUsageCommand();
            Func<ShowUsageCommand> factory = () => showUsageCommand;

            var target = new CommandFactory(factory, null, null, null, null, null);
            var command = target.GetCommand(new string[0]);

            Assert.AreSame(showUsageCommand, command);
        }

        [Test]
        public void GetCommand_CleanCommandArgument_ReturnsCleanCommand()
        {
            var cleanCommand = new CleanCommand(null, null, new []{"abc"});
            Func<string[], CleanCommand> factory = x => cleanCommand;

            var target = new CommandFactory(null, null, factory, null, null, null);
            var command = target.GetCommand(new[] { CommandFactory.CleanArgument });

            Assert.AreSame(cleanCommand, command);
        }

        [Test]
        public void GetCommand_CleanCommandArgument_PassesTailOfArgumentArray()
        {
            bool wasInvoked = false;
            var inputArguments = new[] { CommandFactory.CleanArgument, "testing"};

            Func<string[], CleanCommand> factory = x =>
                                                       {
                                                           Assert.AreEqual(1, x.Length);
                                                           Assert.AreEqual("testing", x[0]);
                                                           wasInvoked = true;
                                                           return null;
                                                       };

            var target = new CommandFactory(null, null, factory, null, null, null);
            target.GetCommand(inputArguments);

            Assert.IsTrue(wasInvoked);
        }

        [Test]
        public void GetCommand_SmudgeCommandArgument_ReturnsSmudgeCommand()
        {
            var smudgeCommand = new SmudgeCommand(null, null, new string[0]);
            Func<string[], SmudgeCommand> factory = x => smudgeCommand;

            var target = new CommandFactory(null, null, null, factory, null, null);
            var command = target.GetCommand(new[] { CommandFactory.SmudgeArgument });

            Assert.AreSame(smudgeCommand, command);
        }

        [Test]
        public void GetCommand_PushCommandArgument_ReturnsPushCommand()
        {
            var pushCommand = new PushCommand(null, null, null);
            Func<string[], PushCommand> factory = x => pushCommand;

            var target = new CommandFactory(null, null, null, null, factory, null);
            var command = target.GetCommand(new[] { CommandFactory.PushArgument });

            Assert.AreSame(pushCommand, command);
        }

        [Test]
        public void GetCommand_PushCommandArgument_PassesTailOfArgumentArray()
        {
            bool wasInvoked = false;
            var inputArguments = new[] { CommandFactory.PushArgument, "testing" };

            Func<string[], PushCommand> factory = x =>
            {
                Assert.AreEqual(1, x.Length);
                Assert.AreEqual("testing", x[0]);
                wasInvoked = true;
                return null;
            };

            var target = new CommandFactory(null, null, null, null, factory, null);
            target.GetCommand(inputArguments);

            Assert.IsTrue(wasInvoked);
        }
    }
}

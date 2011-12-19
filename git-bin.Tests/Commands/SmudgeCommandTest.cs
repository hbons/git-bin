using System;
using GitBin.Commands;
using NUnit.Framework;

namespace git_bin.Tests.Commands
{
    [TestFixture]
    public class SmudgeCommandTest
    {
        [Test]
        public void Ctor_NoArguments_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new SmudgeCommand(null, null, new string[0]));
        }

        [Test]
        public void Ctor_HasArguments_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                new SmudgeCommand(null, null, new[] { "haha" }));
        }
    }
}
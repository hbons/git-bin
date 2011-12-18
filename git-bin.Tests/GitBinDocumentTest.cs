using GitBin;
using NUnit.Framework;

namespace git_bin.Tests
{
    [TestFixture]
    public class GitBinDocumentTest
    {
        [Test]
        public void ToString_ProducesValidJson()
        {
            var doc = new GitBinDocument("name");
            doc.RecordChunk("abcde");
            doc.RecordChunk("zyxwv");

            var expected = "{\r\n  \"filename\": \"name\",\r\n  \"chunks\": [\r\n    \"abcde\",\r\n    \"zyxwv\"\r\n  ]\r\n}";

            Assert.AreEqual(expected, doc.ToString());
        }
    }
}
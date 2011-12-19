using System.IO;
using GitBin;
using NUnit.Framework;

namespace git_bin.Tests
{
    [TestFixture]
    public class GitBinDocumentTest
    {
        private string expectedYaml = "Filename: name\r\nChunkHashes:\r\n- abcde\r\n- zyxwv\r\n";

        [Test]
        public void ToYaml_ProducesValidYaml()
        {
            var doc = new GitBinDocument("name");
            doc.RecordChunk("abcde");
            doc.RecordChunk("zyxwv");

            var actualYaml = GitBinDocument.ToYaml(doc);

            Assert.AreEqual(expectedYaml, actualYaml);
        }

        [Test]
        public void FromYaml_DeserializedCorrectly()
        {
            var target = GitBinDocument.FromYaml(new StringReader(expectedYaml));

            Assert.AreEqual("name", target.Filename);
            Assert.AreEqual(2, target.ChunkHashes.Count);
            Assert.AreEqual("abcde", target.ChunkHashes[0]);
            Assert.AreEqual("zyxwv", target.ChunkHashes[1]);
        }
    }
}
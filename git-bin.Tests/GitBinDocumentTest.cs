using System.IO;
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

        [Test]
        public void Ctor_Stream_DeserializedCorrectly()
        {
            var expected = "{\r\n  \"filename\": \"name\",\r\n  \"chunks\": [\r\n    \"abcde\",\r\n    \"zyxwv\"\r\n  ]\r\n}";
            
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            streamWriter.Write(expected);
            streamWriter.Flush();
            stream.Position = 0;

            var target = new GitBinDocument(stream);

            Assert.AreEqual("name", target.Filename);
            Assert.AreEqual(2, target.ChunkHashes.Count);
            Assert.AreEqual("abcde", target.ChunkHashes[0]);
            Assert.AreEqual("zyxwv", target.ChunkHashes[1]);
        }
    }
}
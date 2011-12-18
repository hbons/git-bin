using System.Collections.Generic;

namespace GitBin
{
    public class GitBinDocument
    {
        private readonly string _filename;
        private readonly List<string> _chunkHashes = new List<string>();

        public GitBinDocument(string filename)
        {
            _filename = filename;
        }

        public void RecordChunk(string hash)
        {
            _chunkHashes.Add(hash);
        }

        public override string ToString()
        {
            return "mark is awesome";
        }
    }
}
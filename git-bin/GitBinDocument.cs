using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

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
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;

                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("filename");
                jsonWriter.WriteValue(_filename);

                jsonWriter.WritePropertyName("chunks");
                jsonWriter.WriteStartArray();
                foreach (var hash in _chunkHashes)
                {
                    jsonWriter.WriteValue(hash);
                }
                jsonWriter.WriteEndArray();

                jsonWriter.WriteEndObject();
            }

            return sb.ToString();
        }
    }
}
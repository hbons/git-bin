using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GitBin
{
    public class GitBinDocument
    {
        private const string FilenameName = "filename";
        private const string ChunksHashesName = "chunks";

        public string Filename { get; private set; }
        public List<string> ChunkHashes { get; private set; }

        public GitBinDocument(string filename)
        {
            this.Filename = filename;
            this.ChunkHashes = new List<string>();
        }

        public GitBinDocument(Stream stream)
        {
            this.ChunkHashes = new List<string>();

            var sr = new StreamReader(stream);

            using (var jsonReader = new JsonTextReader(sr))
            {
                jsonReader.CloseInput = false;

                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName)
                    {
                        if ((string)jsonReader.Value == FilenameName)
                        {
                            jsonReader.Read();
                            this.Filename = (string)jsonReader.Value;
                        }
                        else if ((string)jsonReader.Value == ChunksHashesName)
                        {
                            while (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray)
                            {
                                if (jsonReader.TokenType == JsonToken.String)
                                {
                                    RecordChunk((string)jsonReader.Value);
                                }
                            }
                        }
                        else
                        {
                            throw new ಠ_ಠ("Found unknown property in GitBinDocument: [" + jsonReader.Value + ']');
                        }
                    }
                }
            }
        }

        public void RecordChunk(string hash)
        {
            this.ChunkHashes.Add(hash);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;

                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName(FilenameName);
                jsonWriter.WriteValue(Filename);

                jsonWriter.WritePropertyName(ChunksHashesName);
                jsonWriter.WriteStartArray();
                foreach (var hash in this.ChunkHashes)
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
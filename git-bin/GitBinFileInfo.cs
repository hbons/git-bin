namespace GitBin
{
    public class GitBinFileInfo
    {
        public string Name { get; private set; }
        public long Size { get; private set; }

        public GitBinFileInfo(string name, long size)
        {
            this.Name = name;
            this.Size = size;
        }
    }
}
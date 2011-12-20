namespace GitBin.Remotes
{
    public class RemoteFileInfo
    {
        public string Name { get; private set; }
        public long Size { get; private set; }

        public RemoteFileInfo(string name, long size)
        {
            this.Name = name;
            this.Size = size;
        }
    }
}
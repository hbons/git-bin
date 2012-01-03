using System;

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

        public bool Equals(GitBinFileInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && other.Size == Size;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(GitBinFileInfo)) return false;
            return Equals((GitBinFileInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Size.GetHashCode();
            }
        }
    }
}
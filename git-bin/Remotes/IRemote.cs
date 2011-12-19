using System.IO;

namespace GitBin.Remotes
{
    public interface IRemote
    {
        string[] ListFiles();
        Stream DownloadFile(string filename);
        void UploadFile(string filename, Stream fileStream);
        void DownloadFileTo(string fullPath, string filename);
    }
}
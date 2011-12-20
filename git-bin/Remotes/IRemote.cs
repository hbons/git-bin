using System;
using System.IO;

namespace GitBin.Remotes
{
    public interface IRemote
    {
        RemoteFileInfo[] ListFiles();

        void UploadFile(string fullPath, string key);
        void DownloadFile(string fullPath, string key);

        event Action<int> ProgressChanged;
    }
}
using System;
using System.Collections.Generic;
using System.IO;

using Renci.SshNet;
using Renci.SshNet.Sftp;


namespace GitBin.Remotes {

    public class SftpRemote : IRemote {

        public event Action<int> ProgressChanged;

        private readonly IConfigurationProvider configurationProvider;
        private SftpClient client;
        private string remote_path;


        public SftpRemote (IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;

            Uri sftp_url = new Uri ((string) this.configurationProvider.Settings ["sftpurl"]);
            string sftp_key_file_path = (string) this.configurationProvider.Settings ["sftpprivatekeyfilepath"];
            
            this.remote_path = sftp_url.AbsolutePath + "/chunks";

            string user = sftp_url.UserInfo.Split (":".ToCharArray ()) [0];
            string host = sftp_url.Host;
            int port    = sftp_url.Port;

            if (string.IsNullOrEmpty (user))
                user = "storage";

            if (port < 0)
                port = 22;

            string passphrase = "";
            
            if (this.configurationProvider.Settings.ContainsKey ("sftpprivatekeypassphrase"))
                passphrase = (string) this.configurationProvider.Settings ["sftpprivatekeypassphrase"];

            this.client = new SftpClient (host, port, user, new PrivateKeyFile (sftp_key_file_path, passphrase));
            this.client.Connect ();
       
            if (!this.client.Exists (this.remote_path)) {
                string path = "";

                foreach (string part in this.remote_path.Split ("/".ToCharArray ())) {
                    path += "/" + part;

                    if (!this.client.Exists (path))
                        this.client.CreateDirectory (path);  
                }
            }
        }


        public GitBinFileInfo [] ListFiles ()
        {
            if (!this.client.IsConnected)
                this.client.Connect ();

            List<GitBinFileInfo> files = new List<GitBinFileInfo> ();

            foreach (SftpFile file in this.client.ListDirectory (this.remote_path)) {
                if (!file.Name.Equals (".") && !file.Name.Equals (".."))
                    files.Add (new GitBinFileInfo (file.Name, file.Length));
            }

            return files.ToArray ();
        }


        public void UploadFile (string full_path, string key)
        {
            try {
                if (!this.client.IsConnected)
                    this.client.Connect ();

                ProgressChanged (0);

                string target_remote_path = this.remote_path + "/" + key;
                string tmp_remote_path    = target_remote_path + ".tmp";
                
                FileStream stream = new FileStream (full_path, FileMode.Open);
                this.client.UploadFile (stream, tmp_remote_path);
                stream.Close ();
                
                this.client.RenameFile (tmp_remote_path, target_remote_path);
                File.Delete (full_path);

                ProgressChanged (100);
            
            } catch (Exception e) {
                throw new ಠ_ಠ ("Error uploading chunk " + key, e);
            }
        }


        public void DownloadFile (string full_path, string key)
        {
            try {
                if (!this.client.IsConnected)
                    this.client.Connect ();

                ProgressChanged (0);

                string tmp_path = full_path + ".tmp";

                FileStream stream = new FileStream (tmp_path, FileMode.Create);
                this.client.DownloadFile (this.remote_path + "/" + key, stream);
                stream.Close ();

                File.Move (tmp_path, full_path);

                ProgressChanged (100);
            
            } catch (Exception e) {
                throw new ಠ_ಠ ("Error downloading chunk " + key, e);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

using Renci.SshNet;
using Renci.SshNet.Sftp;


namespace GitBin.Remotes {

    public class SSHRemote : IRemote {

        public event Action<int> ProgressChanged;

        private readonly IConfigurationProvider configurationProvider;
        private SftpClient client;

        private string remote_path;


        public SSHRemote (IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;

            Uri ssh_url = new Uri ((string) this.configurationProvider.Settings ["sshurl"]);
            string ssh_private_key_file = (string) this.configurationProvider.Settings ["sshprivatekeyfile"];

            this.remote_path = ssh_url.AbsolutePath + "/chunks";

            string user = ssh_url.UserInfo.Split (":".ToCharArray ()) [0];
            string host = ssh_url.Host;
            int port    = ssh_url.Port;

            if (string.IsNullOrEmpty (user))
                user = "storage";

            if (port < 0)
                port = 22;

            string passphrase = "";

            if (this.configurationProvider.Settings.ContainsKey ("sshprivatekeypassphrase"))
                passphrase = (string) this.configurationProvider.Settings ["sshprivatekeypassphrase"];

            this.client = new SftpClient (host, port, user, new PrivateKeyFile (ssh_private_key_file, passphrase));
            this.client.Connect ();
       }
        
        public GitBinFileInfo [] ListFiles ()
        {
            if (!this.client.IsConnected)
                this.client.Connect ();

            if (!this.client.Exists (this.remote_path)) {
                this.client.CreateDirectory (this.remote_path);
                return new GitBinFileInfo [0];
            }

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

                FileStream stream = new FileStream (full_path, FileMode.Open);
                this.client.UploadFile (stream, this.remote_path + "/" + key);
                stream.Close ();

                ProgressChanged (100);

                File.Delete (full_path);
            
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

                FileStream stream = new FileStream (full_path, FileMode.Create);
                this.client.DownloadFile (this.remote_path + "/" + key, stream);
                stream.Close ();

                ProgressChanged (100);
            
            } catch (Exception e) {
                throw new ಠ_ಠ ("Error downloading chunk " + key, e);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace GitBin.Remotes
{
    public class S3Remote : IRemote
    {
        private const int RequestTimeoutInMinutes = 60;
        private const string InvalidAccessKeyErrorCode = "InvalidAccessKeyId";
        private const string InvalidSecurityErrorCode = "InvalidSecurity";

        private readonly IConfigurationProvider _configurationProvider;
        private AmazonS3 _client;

        public S3Remote(
            IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public GitBinFileInfo[] ListFiles()
        {
            var remoteFiles = new List<GitBinFileInfo>();
            var client = GetClient();

            var listRequest = new ListObjectsRequest();
            listRequest.BucketName = _configurationProvider.S3Bucket;

            ListObjectsResponse listResponse;

            do {
                listResponse = client.ListObjects(listRequest);

                if (listResponse.S3Objects.Any())
                {
                    var keys = listResponse.S3Objects.Select(o => new GitBinFileInfo(o.Key, o.Size));

                    remoteFiles.AddRange(keys);

                    listRequest.Marker = remoteFiles[remoteFiles.Count - 1].Name;
                }
            }
            while (listResponse.IsTruncated);

            return remoteFiles.ToArray();
        }

        public void UploadFile(string fullPath, string key)
        {
            var client = GetClient();

            var putRequest = new PutObjectRequest();
            putRequest.BucketName = _configurationProvider.S3Bucket;
            putRequest.FilePath = fullPath;
            putRequest.Key = key;
            putRequest.Timeout = RequestTimeoutInMinutes*60000;
            putRequest.PutObjectProgressEvent += (s, args) => ReportProgress(args);

            try
            {
                PutObjectResponse putResponse = client.PutObject(putRequest);
                putResponse.Dispose();
            }
            catch(AmazonS3Exception e)
            {
                throw new ಠ_ಠ(GetMessageFromException(e));
            }
        }

        public void DownloadFile(string fullPath, string key)
        {
            var client = GetClient();

            var getRequest = new GetObjectRequest();
            getRequest.BucketName = _configurationProvider.S3Bucket;
            getRequest.Key = key;
            getRequest.Timeout = RequestTimeoutInMinutes*60000;

            try
            {
                using (var getResponse = client.GetObject(getRequest))
                {
                    getResponse.WriteObjectProgressEvent += (s, args) => ReportProgress(args);
                    getResponse.WriteResponseStreamToFile(fullPath);
                }
            }
            catch(AmazonS3Exception e)
            {
                throw new ಠ_ಠ(GetMessageFromException(e));                
            }
        }

        public event Action<int> ProgressChanged;

        private AmazonS3 GetClient()
        {
            if (_client == null)
            {
                _client = AWSClientFactory.CreateAmazonS3Client(
                    _configurationProvider.S3Key,
                    _configurationProvider.S3SecretKey);
            }

            return _client;
        }

        private void ReportProgress(TransferProgressArgs args)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(args.PercentDone);
            }
        }

        private string GetMessageFromException(AmazonS3Exception e)
        {
            if (!string.IsNullOrEmpty(e.ErrorCode) &&
                (e.ErrorCode == InvalidAccessKeyErrorCode ||
                 e.ErrorCode == InvalidSecurityErrorCode))
                return "S3 error: check your access key and secret access key";

            return string.Format("S3 error: code [{0}], message [{1}]", e.ErrorCode, e.Message);
        }
    }
}
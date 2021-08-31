using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HackAPIs.ViewModel;
using HackAPIs.ViewModel.Util;
using Microsoft.Azure.Management.Storage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAPIs.Services.Util
{
    public class BlobStorageService
    {
        private BlobContainerClient blobContainer;
        private BlobClient blobClient;
        public string GetBlob(BlobStorage blobStorage)
        {
            string rtnStr = "";
            StringBuilder strBuf = new StringBuilder();

            blobContainer = new BlobContainerClient(blobStorage.Connection, blobStorage.Container);
            

            blobClient = blobContainer.GetBlobClient(blobStorage.Blob);

            Response<BlobDownloadInfo> download = blobClient.Download();

            var sr = new StreamReader(download.Value.Content);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                strBuf.Append(line);
            }
            

            rtnStr = strBuf.ToString();

            return rtnStr;
        }
    }
}

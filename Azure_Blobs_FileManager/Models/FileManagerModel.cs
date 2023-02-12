using Microsoft.VisualBasic;
using System.IO;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Azure_Blobs_FileManager.Models
{
    public class FileManagerModel
    {
        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=myteststorrage;AccountKey=N0SunFyLmFYMDfdn+wIcClQDLR6AEDTLRHWvqO8tRHEQmOeGHFXqK7p1syO4xlgTlQH5bShqnB11+ASt6k4otQ==;EndpointSuffix=core.windows.net";
        private readonly string _blobName = "homeworkblob";

        public string ConnectionString
        {
            get => _connectionString;
        }

        public string BlobContainerName
        {
            get => _blobName;
        }

        public List<string> GetAllFiles(List<string> list)
        {
            List<string> tempList = new ();
            foreach (var item in list)
            {
                FileInfo fileInfo = new FileInfo(item);

                tempList.Add($"Name: {fileInfo.Name}");
            }
            return tempList;
        }
    }
}

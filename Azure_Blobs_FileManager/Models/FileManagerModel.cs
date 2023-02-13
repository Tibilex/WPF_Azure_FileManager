using Microsoft.VisualBasic;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using Azure_Blobs_FileManager.ViewModels;
using Microsoft.Win32;
using System.ComponentModel;
using Azure.Storage.Blobs;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;

namespace Azure_Blobs_FileManager.Models
{
    public class FileManagerModel : ViewModelBase
    {
        #region Fields

        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=myteststorrage;AccountKey=N0SunFyLmFYMDfdn+wIcClQDLR6AEDTLRHWvqO8tRHEQmOeGHFXqK7p1syO4xlgTlQH5bShqnB11+ASt6k4otQ==;EndpointSuffix=core.windows.net";
        private readonly string _blobName = "homeworkblob";
        private CloudBlobClient _backupBlobClient;
        private CloudBlobContainer _backupContainer;
        private BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;
        private BlobContainerClient _container;
        private CloudBlockBlob _blob;

        private readonly ObservableCollection<string> _files = new ObservableCollection<string>();
        public readonly ReadOnlyObservableCollection<string> _filesCollection;
        private OpenFileDialog _dialog;
        private SaveFileDialog _saveFileDialog;
        private string _requestResult;

        #endregion
        public FileManagerModel()
        {
            _backupBlobClient = CloudStorageAccount.Parse(_connectionString).CreateCloudBlobClient();
            _backupContainer = _backupBlobClient.GetContainerReference(_blobName);
            _blobServiceClient = new (_connectionString);
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient("FileUpload");
            _container = new(_connectionString, _blobName);

            _filesCollection = new ReadOnlyObservableCollection<string>(_files);
            _dialog = new();
            _saveFileDialog = new();
        }

        #region Properties

        public string FileSelectedItem { get; set; }

        public string RequestResult
        {
            get => _requestResult;
            set => _requestResult = value;
        }

        #endregion
        public void GetAllFiles()
        {
            List<string> FileList = _backupContainer.ListBlobs().OfType<CloudBlockBlob>().Select(blob => blob.Name).ToList();
            if (_files.Count > 0)
            {
                _files.Clear();
            }
            foreach (var item in FileList)
            {
                FileInfo fileInfo = new FileInfo(item);

                _files.Add($"{fileInfo.Name}");
            }
            
        }

        public async void UploadFile()
        {
            _dialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            _dialog.ShowDialog();
            if (!String.IsNullOrEmpty(_dialog.FileName))
            {
                BlobClient blob = _container.GetBlobClient(_dialog.SafeFileName);
                FileStream stream = File.OpenRead(_dialog.FileName);
                await blob.UploadAsync(stream);
                _requestResult = "File upload successfully";
            }
        }

        public void DeleteFile()
        {
            _container.GetBlobClient(FileSelectedItem).DeleteIfExists(); ;
            _requestResult = "File delete successfully";
        }

        public void DownloadFile()
        {
            _saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            _saveFileDialog.ShowDialog();
            if (!String.IsNullOrEmpty(_saveFileDialog.FileName))
            {
                _blob = _backupContainer.GetBlockBlobReference(FileSelectedItem);
                _blob.DownloadToFile(_saveFileDialog.FileName, FileMode.Create);
            }
        }
    }
}

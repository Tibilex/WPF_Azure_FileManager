using System.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Azure_Blobs_FileManager.ViewModels;
using Microsoft.Win32;
using Azure.Storage.Blobs;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Linq;
using Path = System.IO.Path;

namespace Azure_Blobs_FileManager.Models
{
    public class FileManagerModel : ViewModelBase
    {
        #region Fields

        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=myteststorrage;AccountKey=N0SunFyLmFYMDfdn+wIcClQDLR6AEDTLRHWvqO8tRHEQmOeGHFXqK7p1syO4xlgTlQH5bShqnB11+ASt6k4otQ==;EndpointSuffix=core.windows.net";
        private readonly string _blobName = "homeworkblob";
        private CloudBlobClient _cloudBlobClient;
        private CloudBlobContainer _cloudContainer;
        private BlobContainerClient _container;
        private CloudBlockBlob? _blockBlob;

        private ObservableCollection<string> _files = new ObservableCollection<string>();
        public ReadOnlyObservableCollection<string> _filesCollection;
        private OpenFileDialog _dialog;
        private SaveFileDialog _saveFileDialog;
        private string? _requestResult;
        private string? _searchInputText;
        private string? _fileText;

        #endregion
        public FileManagerModel()
        {
            _cloudBlobClient = CloudStorageAccount.Parse(_connectionString).CreateCloudBlobClient();
            _cloudContainer = _cloudBlobClient.GetContainerReference(_blobName);
            _container = new(_connectionString, _blobName);

            _filesCollection = new ReadOnlyObservableCollection<string>(_files);
            _dialog = new();
            _saveFileDialog = new();
        }

        #region Properties

        public string? FileSelectedItem { get; set; }

        public string? SearchInputText
        {
            get => _searchInputText;
            set
            {
                _searchInputText = value;
                OnPropertyChanged("SearchInputText");
            }
        }

        public string? FileText
        {
            get => _fileText;
            set
            {
                _fileText = value;
                OnPropertyChanged("FileText");
            }
        }

        #endregion

        public void GetAllFiles()
        {
            List<string> fileList = _cloudContainer.ListBlobs().OfType<CloudBlockBlob>().Select(blob => blob.Name).ToList();
            if (_files.Count > 0)
            {
                _files.Clear();
            }
            foreach (var item in fileList)
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
                await blob.UploadAsync(stream, overwrite: true);
            }
            GetAllFiles();
        }

        public void DeleteFile()
        {
            _container.GetBlobClient(FileSelectedItem).DeleteIfExists(); ;
            GetAllFiles();
        }

        public void DownloadFile()
        {
            _saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            _saveFileDialog.ShowDialog();
            if (!String.IsNullOrEmpty(_saveFileDialog.FileName))
            {
                _blockBlob = _cloudContainer.GetBlockBlobReference(FileSelectedItem);
                _blockBlob.DownloadToFile(_saveFileDialog.FileName, FileMode.Create);
            }
        }

        public void SearchFile()
        {
            if (_searchInputText != "")
            {
                ObservableCollection<string> tmpList = new(_files);
                _files.Clear();
                foreach (string item in tmpList)
                {
                    if (item.StartsWith(_searchInputText))
                    {
                        _files.Add(item);
                    }
                }
            }
        }

        public async void GetFileInnerText()
        {
            if (FileSelectedItem != null)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileSelectedItem);

                _blockBlob = _cloudContainer.GetBlockBlobReference(FileSelectedItem);
                _blockBlob.DownloadToFile(path, FileMode.Create);
                using (StreamReader reader = new StreamReader(path))
                {
                    _fileText = await reader.ReadToEndAsync();
                }
            }
        }
    }
}

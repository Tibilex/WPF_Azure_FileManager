using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Documents;
using Azure.Storage.Blobs;
using Azure_Blobs_FileManager.Commands;
using Azure_Blobs_FileManager.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Azure_Blobs_FileManager.ViewModels
{
    public class FileManagerViewModel : ViewModelBase
    {
        #region Fields

        private FileManagerModel _manager;
        private CloudBlobClient _backupBlobClient;
        private CloudBlobContainer _backupContainer;
        private BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;
        private BlobContainerClient _container;
        private ObservableCollection<string> _blobListFiles;

        #endregion

        #region Properties

        public ObservableCollection<string> BlobFileListCollection
        {
            get => _blobListFiles;
            set
            {
                _blobListFiles = value;
                OnPropertyChanged("BlobFileListCollection");
            }
        }

        #endregion

        #region Commands

        public CommandResult ShowAllButton { get; private set; }

        #endregion


        public FileManagerViewModel()
        {
            _manager = new ();
            _backupBlobClient = CloudStorageAccount.Parse(_manager.ConnectionString).CreateCloudBlobClient();
            _backupContainer = _backupBlobClient.GetContainerReference(_manager.BlobContainerName);
            _blobServiceClient = new (_manager.ConnectionString);
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient("FileUpload");
            _container = new(_manager.ConnectionString, _manager.BlobContainerName);
            _blobListFiles = new();

            CommandsLoad();
        }

        #region Methods

        private void CommandsLoad()
        {
            ShowAllButton = new CommandResult(ShowAllFiles);
        }

        private void ShowAllFiles()
        {
            List<string> listBlobsNames = _backupContainer.ListBlobs().OfType<CloudBlockBlob>().Select(blob => blob.Name).ToList();
            _manager.GetAllFiles(listBlobsNames);
            foreach (string item in _manager.GetAllFiles(listBlobsNames))
            {
                _blobListFiles.Add(item);
            }
        }



        #endregion
    }
}

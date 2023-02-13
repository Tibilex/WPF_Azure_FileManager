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

        #endregion

        #region Properties

        public ReadOnlyObservableCollection<string> BlobFileListCollection
        {
            get
            {
                return _manager._filesCollection;
                OnPropertyChanged("BlobFileListCollection");
            }
        }

        public string ListBoxSelectedItem
        {
            set => _manager.FileSelectedItem = value;
        }

        public string RequestResult
        {
            get => _manager.RequestResult;
            set
            {
                _manager.RequestResult = value;
                OnPropertyChanged("RequestResult");
            }
        }

        #endregion

        #region Commands

        public CommandResult ShowAllButton { get; private set; }
        public CommandResult UploadFileButton { get; private set; }
        public CommandResult DeleteFileButton { get; private set; }
        public CommandResult DownloadFileButton { get; private set; }
        public CommandResult ShowInnerButton { get; private set; }
        public CommandResult SearchButton { get; private set; }

        #endregion


        public FileManagerViewModel()
        {
            _manager = new ();
            CommandsLoad();
        }

        #region Methods

        private void CommandsLoad()
        {
            ShowAllButton = new CommandResult(_manager.GetAllFiles);
            UploadFileButton = new CommandResult(_manager.UploadFile);
            DeleteFileButton = new CommandResult(_manager.DeleteFile);
            DownloadFileButton = new CommandResult(_manager.DownloadFile);
        }

        #endregion
    }
}

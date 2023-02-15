using System;
using System.Collections.ObjectModel;
using Azure_Blobs_FileManager.Commands;
using Azure_Blobs_FileManager.Models;

namespace Azure_Blobs_FileManager.ViewModels
{
    public class FileManagerViewModel : ViewModelBase
    {
        #region Fields

        private FileManagerModel _manager;
        private string _fileText;
        private string _requestResult;

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

        public string? ListBoxSelectedItem
        {
            set => _manager.FileSelectedItem = value;
        }

        public string? RequestResult
        {
            get => _requestResult;
            private set
            {
                _requestResult = value;
                OnPropertyChanged("RequestResult");
            }
        }

        public string? SearchInputText
        {
            get
            {
                if (_manager.SearchInputText == null)
                {
                    SearchInputText = "Search file...";
                }
                return _manager.SearchInputText;
            }
            set
            {
                _manager.SearchInputText = value;
                OnPropertyChanged("SearchInputText");
            }
        }

        public String? FileText
        {
            get => _fileText;
            set
            {
                _fileText = value;
                OnPropertyChanged("FileText");
            }
            
        }
        
        #endregion

        #region Commands

        public CommandResult? ShowAllButton { get; private set; }
        public CommandResult? UploadFileButton { get; private set; }
        public CommandResult? DeleteFileButton { get; private set; }
        public CommandResult? DownloadFileButton { get; private set; }
        public CommandResult? ShowInnerButton { get; private set; }
        public CommandResult? SearchButton { get; private set; }

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
            UploadFileButton = new CommandResult(UploadFile);
            DeleteFileButton = new CommandResult(DeleteFile);
            DownloadFileButton = new CommandResult(_manager.DownloadFile);
            SearchButton = new CommandResult(_manager.SearchFile);
            ShowInnerButton = new CommandResult(ShowFileInnerText);
        }

        public void ShowFileInnerText()
        {
            _manager.GetFileInnerText();
            FileText = _manager.FileText;
        }

        public void UploadFile()
        {
            RequestResult = "";
            _manager.UploadFile();
            RequestResult = "File upload successfully"; 
            
        }

        public void DeleteFile()
        {
            RequestResult = "";
            _manager.DeleteFile();
            RequestResult = "File delete successfully";
        }

        #endregion
    }
}

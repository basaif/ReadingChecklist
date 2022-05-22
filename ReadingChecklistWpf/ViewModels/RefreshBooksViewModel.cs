using FileManagementLibrary;
using ReadingChecklistLogicLibrary;
using ReadingChecklistWpf.ViewModels.Cmds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReadingChecklistWpf.ViewModels
{
    public class RefreshBooksViewModel : ViewModelBase
    {
        private readonly FilesManager _filesManager;
        private readonly BooksDataRefresher _booksDataRefresher;

        private string _locationToGetBooks = "";

        public string LocationToGetBooks
        {
            get { return _locationToGetBooks; }
            set
            {
                _locationToGetBooks = value;
                OnPropertyChanged(nameof(LocationToGetBooks));
            }
        }

        private bool _isRefreshingBooks;

        public bool IsRefreshingBooks
        {
            get { return _isRefreshingBooks; }
            set
            {
                _isRefreshingBooks = value;
                OnPropertyChanged(nameof(IsRefreshingBooks));
            }
        }

        private readonly HomeViewModel _homeViewModel;

        public ICommand OpenSearchForBooksDialogCommand { get; }

        public ICommand? RefreshBookDataCommand { get; set; }

        public RefreshBooksViewModel(HomeViewModel homeViewModel, FilesManager filesManager, BooksDataRefresher booksDataRefresher)
        {
            _homeViewModel = homeViewModel;

            _filesManager = filesManager;
            _booksDataRefresher = booksDataRefresher;

            OpenSearchForBooksDialogCommand = new OpenSearchForBooksDialogCommand(this);

            RefreshBookDataCommand = new RefreshBookDataCommand(this, _booksDataRefresher, _homeViewModel);
        }

        public void SetRefreshBookDataCommand(string location)
        {
            _filesManager.Location = location;

        }
    }
}

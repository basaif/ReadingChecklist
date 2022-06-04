using WpfUi.ViewModels.Cmds;
using System.Windows.Input;
using FileSystemUtilities.Library;
using DomainLogic.Library;

namespace WpfUi.ViewModels
{
	public class RefreshBooksViewModel : ViewModelBase
    {
        private readonly IFoldersFileNamePairs _foldersFileNamePairs;
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

        public RefreshBooksViewModel(HomeViewModel homeViewModel, IFoldersFileNamePairs foldersFileNamePairs, BooksDataRefresher booksDataRefresher)
        {
            _homeViewModel = homeViewModel;

            _foldersFileNamePairs = foldersFileNamePairs;
            _booksDataRefresher = booksDataRefresher;

            OpenSearchForBooksDialogCommand = new OpenSearchForBooksDialogCommand(this);

            RefreshBookDataCommand = new RefreshBookDataCommand(this, _booksDataRefresher, _homeViewModel);
        }

        public void SetRefreshBookDataCommand(string location)
        {
            _foldersFileNamePairs.ChangeLocation(location);

        }
    }
}

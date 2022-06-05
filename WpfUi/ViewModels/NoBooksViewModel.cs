using WpfUi.ViewModels.Cmds;
using System.Windows.Input;
using FileSystemUtilities.Library;
using DomainLogic.Library;

namespace WpfUi.ViewModels
{
	public class NoBooksViewModel : ViewModelBase
    {
        private readonly IFoldersFileNamePairs _foldersFileNamePairs;
        private readonly IBooksDataRefresher _bookDataRefresher;
		private string _locationToGetBooks = "";

        public string LocationToGetBooks
        {
            get { return _locationToGetBooks; }
            set { _locationToGetBooks = value;
                OnPropertyChanged(nameof(LocationToGetBooks));
            }
        }

        private bool _isGettingBooks;

        public bool IsGettingBooks
        {
            get { return _isGettingBooks; }
            set { _isGettingBooks = value;
                OnPropertyChanged(nameof(IsGettingBooks));
            }
        }

        private readonly HomeViewModel _homeViewModel;

        public ICommand OpenSearchForBooksDialogCommand{ get; }

        public ICommand? GenterateBookDataCommand { get; set; }

        public NoBooksViewModel(HomeViewModel homeViewModel, IFoldersFileNamePairs foldersFileNamePairs,
			IBooksDataRefresher bookDataRefresher)
        {
            _homeViewModel = homeViewModel;

            _foldersFileNamePairs = foldersFileNamePairs;
            _bookDataRefresher = bookDataRefresher;
			OpenSearchForBooksDialogCommand = new OpenSearchForBooksDialogCommand(this);

            GenterateBookDataCommand = new GenterateBookDataCommand(this, _bookDataRefresher,
				_homeViewModel);
        }

        public void SetGenterateBookDataCommand(string location)
        {
            _foldersFileNamePairs.ChangeLocation(location);
            
        }

    }
}
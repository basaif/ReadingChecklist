using WpfUi.ViewModels.Cmds;
using System.Windows.Input;
using FileSystemUtilities.Library;
using DomainLogic.Library.Creators;

namespace WpfUi.ViewModels
{
	public class GetBooksViewModel : ViewModelBase
    {
        private readonly IFoldersFileNamePairs _foldersFileNamePairs;
        private readonly IBookTagStructureCreator _bookDataRefresher;
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

        public GetBooksViewModel(HomeViewModel homeViewModel, IFoldersFileNamePairs foldersFileNamePairs,
			IBookTagStructureCreator bookDataRefresher)
        {
            _homeViewModel = homeViewModel;

            _foldersFileNamePairs = foldersFileNamePairs;
            _bookDataRefresher = bookDataRefresher;
			OpenSearchForBooksDialogCommand = new OpenSearchForBooksDialogCommand(this);

            GenterateBookDataCommand = new GetBookDataCommand(this, _bookDataRefresher,
				_homeViewModel);
        }

        public void SetGenterateBookDataCommand(string location)
        {
            _foldersFileNamePairs.ChangeLocation(location);
            
        }

    }
}
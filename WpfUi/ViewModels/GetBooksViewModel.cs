using WpfUi.ViewModels.Cmds;
using System.Windows.Input;
using FileSystemUtilities.Library;
using DomainLogic.Library.Creators;
using WpfUi.Stores;

namespace WpfUi.ViewModels
{
	public class GetBooksViewModel : ViewModelBase
    {
        private readonly IFoldersFileNamePairs _foldersFileNamePairs;
        private readonly IBookTagStructureCreator _bookDataRefresher;
		private readonly BookStore _bookStore;
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

        public ICommand OpenSearchForBooksDialogCommand{ get; }

        public ICommand? GenterateBookDataCommand { get; set; }

        public GetBooksViewModel(IFoldersFileNamePairs foldersFileNamePairs,
			IBookTagStructureCreator bookDataRefresher, BookStore bookStore)
        {

            _foldersFileNamePairs = foldersFileNamePairs;
            _bookDataRefresher = bookDataRefresher;
			_bookStore = bookStore;
			OpenSearchForBooksDialogCommand = new OpenSearchForBooksDialogCommand(this);

            GenterateBookDataCommand = new GetBookDataCommand(this, _bookDataRefresher,
				_bookStore);
        }

        public void SetGenterateBookDataCommand(string location)
        {
            _foldersFileNamePairs.ChangeLocation(location);
            
        }

    }
}
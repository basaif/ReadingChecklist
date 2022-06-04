using FileManagementLibrary;
using ReadingChecklistLogicLibrary;
using ReadingChecklistWpf.ViewModels.Cmds;
using System.Windows.Input;

namespace ReadingChecklistWpf.ViewModels
{
    public class NoBooksViewModel : ViewModelBase
    {
        private readonly IFoldersFileNamePairs _foldersFileNamePairs;
        private readonly BookDataGenerator _bookDataGenerator;

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

        public NoBooksViewModel(HomeViewModel homeViewModel, IFoldersFileNamePairs foldersFileNamePairs, BookDataGenerator bookDataGenerator)
        {
            _homeViewModel = homeViewModel;

            _foldersFileNamePairs = foldersFileNamePairs;
            _bookDataGenerator = bookDataGenerator;

            OpenSearchForBooksDialogCommand = new OpenSearchForBooksDialogCommand(this);

            GenterateBookDataCommand = new GenterateBookDataCommand(this, _bookDataGenerator, _homeViewModel);
        }

        public void SetGenterateBookDataCommand(string location)
        {
            _foldersFileNamePairs.ChangeLocation(location);
            
        }

    }
}
using FileManagementLibrary;
using ReadingChecklistLogicLibrary;
using ReadingChecklistWpf.ViewModels.Cmds;
using System.Windows.Input;

namespace ReadingChecklistWpf.ViewModels
{
    public class NoBooksViewModel : ViewModelBase
    {
        private static readonly FilesManager _filesManager = new("");
        private static readonly TagsCreator _tagsCreator = new();
        private readonly BookDataGenerator _bookDataGenerator = new(_filesManager, _tagsCreator);

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

        public NoBooksViewModel(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
            OpenSearchForBooksDialogCommand = new OpenSearchForBooksDialogCommand(this);

            GenterateBookDataCommand = new GenterateBookDataCommand(this, _bookDataGenerator, _homeViewModel);
        }

        public void SetGenterateBookDataCommand(string location)
        {
            _filesManager.Location = location;
            
        }

    }
}
using DomainLogic.Library.Creators;
using DomainLogic.Library.Services;
using FileSystemUtilities.Library;
using WpfUi.Stores;

namespace WpfUi.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
    {

        private ViewModelBase _homeViewModel;

        public ViewModelBase HomeViewModel
        {
            get { return _homeViewModel; }
            set { _homeViewModel = value; }
        }

        public MainWindowViewModel(IBookDataService bookDataService, BooksStore booksStore,
            IFoldersFileNamePairs foldersFileNamePairs, IBookTagStructureCreator booksDataRefresher)
        {
            _homeViewModel = new HomeViewModel(
				bookDataService, booksStore, foldersFileNamePairs, booksDataRefresher);
        }

    }
}

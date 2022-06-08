using DomainLogic.Library;
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

        public MainWindowViewModel(IBookDataService bookDataService, BookStore booksStore,
            IFoldersFileNamePairs foldersFileNamePairs, IBookTagStructureCreator booksDataRefresher, IBooksUpdater booksUpdater)
        {
            _homeViewModel = new HomeViewModel(
				bookDataService, booksStore, foldersFileNamePairs, booksDataRefresher,
				booksUpdater);
        }

    }
}

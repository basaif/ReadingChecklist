using DomainLogic.Library;
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

        public MainWindowViewModel(IBookDataGetter bookDataGetter, BooksStore booksStore,
            IFoldersFileNamePairs foldersFileNamePairs, IBooksDataRefresher booksDataRefresher)
        {
            _homeViewModel = new HomeViewModel(
				bookDataGetter, booksStore, foldersFileNamePairs, booksDataRefresher);
        }

    }
}

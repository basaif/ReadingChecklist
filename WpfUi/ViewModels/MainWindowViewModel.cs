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

        public MainWindowViewModel(BookDataGetter bookDataGetter, BooksStore booksStore,
            IFoldersFileNamePairs foldersFileNamePairs, BookDataGenerator bookDataGenerator, BooksDataRefresher booksDataRefresher)
        {
            _homeViewModel = new HomeViewModel(
				bookDataGetter, booksStore, foldersFileNamePairs, bookDataGenerator, booksDataRefresher);
        }

    }
}

using FileManagementLibrary;
using ReadingChecklistLogicLibrary;
using ReadingChecklistWpf.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels
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

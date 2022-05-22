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
        private readonly BooksStore _booksStore;
        private readonly FilesManager _filesManager;
        private readonly BookDataGenerator _bookDataGenerator;
        private readonly BooksDataRefresher _booksDataRefresher;

        public ViewModelBase HomeViewModel
        {
            get { return _homeViewModel; }
            set { _homeViewModel = value; }
        }

        public MainWindowViewModel(BookDataGetter bookDataGetter, BooksStore booksStore,
            FilesManager filesManager, BookDataGenerator bookDataGenerator, BooksDataRefresher booksDataRefresher)
        {
            _booksStore = booksStore;
            _filesManager = filesManager;
            _bookDataGenerator = bookDataGenerator;
            _booksDataRefresher = booksDataRefresher;
            _homeViewModel = new HomeViewModel(bookDataGetter, _booksStore, _filesManager, _bookDataGenerator, _booksDataRefresher);
        }

    }
}

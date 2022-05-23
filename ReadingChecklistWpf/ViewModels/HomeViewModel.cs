using FileManagementLibrary;
using ReadingChecklistLogicLibrary;
using ReadingChecklistModels;
using ReadingChecklistWpf.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ReadingChecklistWpf.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly BookDataGetter _bookDataGetter;
        private readonly BooksStore _booksStore;
        private readonly FilesManager _filesManager;
        private readonly BookDataGenerator _bookDataGenerator;
        private readonly BooksDataRefresher _booksDataRefresher;
        private bool _notEnoughBooks;

        public bool NotEnoughBooks
        {
            get { return _notEnoughBooks; }
            set
            {
                _notEnoughBooks = value;
                OnPropertyChanged(nameof(NotEnoughBooks));
                OnPropertyChanged(nameof(EnoughBooks));
            }
        }

        public bool EnoughBooks
        {
            get { return !NotEnoughBooks; }
        }


        private int _numberOfBooks;

        public int NumberOfBooks
        {
            get { return _numberOfBooks; }
            set
            {
                _numberOfBooks = value;
                OnPropertyChanged(nameof(NumberOfBooks));
            }
        }

        private int _numberOfReadBooks;

        public int NumberOfReadBooks
        {
            get { return _numberOfReadBooks; }
            set
            {
                _numberOfReadBooks = value;
                OnPropertyChanged(nameof(NumberOfReadBooks));
            }
        }

        private int _percentageOfReadBooks;

        public int PercentageOfReadBooks
        {
            get { return _percentageOfReadBooks; }
            set
            {
                _percentageOfReadBooks = value;
                OnPropertyChanged(nameof(PercentageOfReadBooks));
            }
        }

        private NoBooksViewModel _noBooks;

        public NoBooksViewModel NoBooks
        {
            get { return _noBooks; }
            set { _noBooks = value; }
        }

        private RefreshBooksViewModel _refreshBooksVM;

        public RefreshBooksViewModel RefreshBooksVM
        {
            get { return _refreshBooksVM; }
            set { _refreshBooksVM = value; }
        }



        ObservableCollection<BookCardViewModel> _bookCards;
        public ObservableCollection<BookCardViewModel> BookCards
        {
            get
            {
                if (_bookCards == null)
                {
                    _bookCards = new ObservableCollection<BookCardViewModel>();
                }
                return _bookCards;
            }

            set
            {
                if (value is not null)
                {
                    _bookCards = value;
                }
                else
                {
                    _bookCards = new ObservableCollection<BookCardViewModel>();
                }
                OnPropertyChanged(nameof(BookCards));
            }
        }


        public ICollectionView BooksCollectionView
        {
            get => _booksCollectionView;
            set
            {
                _booksCollectionView = value;
                OnPropertyChanged(nameof(BooksCollectionView));
            }
        }

        private string _booksFilter = string.Empty;
        private ICollectionView _booksCollectionView;

        public string BooksFilter
        {
            get
            {
                return _booksFilter;
            }
            set
            {
                _booksFilter = value;
                OnPropertyChanged(nameof(BooksFilter));
                BooksCollectionView.Refresh();
            }
        }

        public void SetUpBooksCollectionView()
        {
            BooksCollectionView = CollectionViewSource.GetDefaultView(_bookCards);

            BooksCollectionView.Filter = FilterBooks;
            BooksCollectionView.SortDescriptions.Add(new SortDescription(nameof(BookCardViewModel.BookName),
                ListSortDirection.Ascending));


        }

        public HomeViewModel(BookDataGetter bookDataGetter, BooksStore booksStore,
            FilesManager filesManager, BookDataGenerator bookDataGenerator,
            BooksDataRefresher booksDataRefresher)
        {
            _booksStore = booksStore;
            _filesManager = filesManager;
            _bookDataGenerator = bookDataGenerator;
            _booksDataRefresher = booksDataRefresher;
            _bookDataGetter = bookDataGetter;

            _noBooks = new NoBooksViewModel(this, _filesManager, _bookDataGenerator);
            _refreshBooksVM = new RefreshBooksViewModel(this, _filesManager, _booksDataRefresher);

            _booksStore.BookUpdated += OnBookUpdated;

            if (_bookCards == null)
            {
                _bookCards = new ObservableCollection<BookCardViewModel>();
            }

            SetUpBooksCollectionView();

            AddBooks();

            CalculateNumbers();
        }

        private bool FilterBooks(object obj)
        {
            if (obj is BookCardViewModel bookCardViewModel)
            {
                return bookCardViewModel.BookName.Contains(BooksFilter, StringComparison.InvariantCultureIgnoreCase) ||
                    bookCardViewModel.Tags.Any(x => x.Contains(BooksFilter, StringComparison.InvariantCultureIgnoreCase));
            }

            return false;
        }

        private void OnBookUpdated(BookModel book)
        {
            CalculateNumbers();
        }

        protected override void OnDispose()
        {
            _booksStore.BookUpdated -= OnBookUpdated;

            base.OnDispose();
        }

        public void AddBooks()
        {
            List<BookModel> books = _bookDataGetter.GetAllBooks();

            if (books.Count == 0)
            {
                NotEnoughBooks = true;
            }

            else
            {
                NotEnoughBooks = false;

                foreach (BookModel book in books)
                {
                    BookCards.Add(new BookCardViewModel(book, _booksStore));
                }
            }

        }
        public void RefreshBooks()
        {
            BookCards = new ObservableCollection<BookCardViewModel>();

            SetUpBooksCollectionView();
            AddBooks();
        }

        public void CalculateNumbers()
        {
            NumberOfBooks = _bookCards.Count;
            NumberOfReadBooks = _bookCards.Where(book => book.IsRead == true).Count();
            PercentageOfReadBooks = CalculatePercentage(NumberOfBooks, NumberOfReadBooks);
        }

        public int CalculatePercentage(int total, int part)
        {
            if (total == 0)
            {
                return 0;
            }
            return part * 100 / total;
        }

    }
}

using FileManagementLibrary;
using ReadingChecklistLogicLibrary;
using ReadingChecklistModels;
using ReadingChecklistWpf.Models;
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

        private bool _isShowReadBooks = true;

        public bool IsShowReadBooks
        {
            get { return _isShowReadBooks; }
            set
            {
                _isShowReadBooks = value;
                OnPropertyChanged(nameof(IsShowReadBooks));
                RefreshBooksCollectionView();
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

        private TagListViewModel _tagList;

        public TagListViewModel TagList
        {
            get { return _tagList; }
            set
            {
                _tagList = value;
                OnPropertyChanged(nameof(TagList));
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
                RefreshBooksCollectionView();
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
            _tagList = new TagListViewModel();
            _booksCollectionView = CollectionViewSource.GetDefaultView(_bookCards);

            _booksStore.BookUpdated += OnBookUpdated;

            if (_bookCards == null)
            {
                _bookCards = new ObservableCollection<BookCardViewModel>();
            }

            SetUpBooksCollectionView();

            AddBooks();

            PopulateTagList();

            CalculateNumbers();
        }



        private bool FilterBooks(object obj)
        {
            if (obj is BookCardViewModel bookCardViewModel)
            {
                return IsBookShown(bookCardViewModel);
            }

            return false;
        }

        private bool IsBookShown(BookCardViewModel bookCardViewModel)
        {
            return IsBookShownBasedOnFilterText(bookCardViewModel) && IsBookShownBasedOnTagSelection(bookCardViewModel)
                && IsBookShownBasedOnReadStatus(bookCardViewModel);
        }

        private bool IsBookShownBasedOnReadStatus(BookCardViewModel bookCardViewModel)
        {
            bool isBookShown;
            if (IsShowReadBooks == true)
            {
                isBookShown = true;
            }
            else
            {
                if (bookCardViewModel.IsRead)
                {
                    isBookShown = false;
                }
                else
                {
                    isBookShown = true;
                }
            }
            return isBookShown;
        }

        private bool IsBookShownBasedOnFilterText(BookCardViewModel bookCardViewModel)
        {
            return IsFilterTextInBookName(bookCardViewModel) || IsFilterTextInAnyBookTag(bookCardViewModel);
        }

        private bool IsFilterTextInBookName(BookCardViewModel bookCardViewModel)
        {
            bool output = bookCardViewModel.BookName.Contains(BooksFilter, StringComparison.InvariantCultureIgnoreCase);
            return output;
        }

        private bool IsFilterTextInAnyBookTag(BookCardViewModel bookCardViewModel)
        {
            bool output = bookCardViewModel.Tags.Any(x => x.Contains(BooksFilter, StringComparison.InvariantCultureIgnoreCase));
            return output;
        }

        private bool IsBookShownBasedOnTagSelection(BookCardViewModel bookCardViewModel)
        {
            List<SelectableTagModel> selectedTags = GetSelectedTags();

            if (selectedTags.Count == 0)
            {
                return true;
            }
            else
            {
                return DoesBookHaveAllSelectedTags(bookCardViewModel, selectedTags);
            }
        }

        private bool DoesBookHaveAllSelectedTags(BookCardViewModel bookCardViewModel, List<SelectableTagModel> selectedTags)
        {
            ObservableCollection<string> bookTags = bookCardViewModel.Tags;
            List<string> selectedTagNames = selectedTags.Select(x => x.Tag).ToList();

            bool doesBookHaveAllSelectedTags = selectedTagNames.All(tagName => bookTags.Contains(tagName));

            return doesBookHaveAllSelectedTags;
        }

        private List<SelectableTagModel> GetSelectedTags()
        {
            return TagList.SelectableTags.Where(x => x.IsSelected).ToList();
        }

        public void PopulateTagList()
        {
            ObservableCollection<SelectableTagModel> distinctTagList = new();
            foreach (BookCardViewModel bookCard in BookCards)
            {
                AddBookTagsToDistinctTagList(bookCard, distinctTagList);
            }

            TagList.SelectableTags = GetOrderedTagsByNumberOfBooksDecending(distinctTagList);
        }

        private void AddBookTagsToDistinctTagList(BookCardViewModel bookCard, ObservableCollection<SelectableTagModel> distinctTagList)
        {
            foreach (string tag in bookCard.Tags)
            {
                
                if (IsTagInList(tag, distinctTagList))
                {
                    IncrementNumberOfBooksInTagInList(tag, distinctTagList);
                }
                else
                {
                    AddSelectableTagToDistinctTagList(tag, distinctTagList);
                }
            }
        }

        private bool IsTagInList(string tag, ObservableCollection<SelectableTagModel> tagList)
        {
            return tagList.Any(x => x.Tag == tag);
        }

        private void IncrementNumberOfBooksInTagInList(string tag, ObservableCollection<SelectableTagModel> tagList)
        {
            tagList.First(x => x.Tag == tag).NumberOfBooksInTag += 1;
        }

        private void AddSelectableTagToDistinctTagList(string tag, ObservableCollection<SelectableTagModel> distinctTagList)
        {
            SelectableTagModel selectableTagModel = new();
            selectableTagModel.PropertyChanged += OnSelectedTagChanged;

            selectableTagModel.Tag = tag;
            selectableTagModel.NumberOfBooksInTag = 1;
            distinctTagList.Add(selectableTagModel);
        }

        private ObservableCollection<SelectableTagModel> GetOrderedTagsByNumberOfBooksDecending(ObservableCollection<SelectableTagModel> unorderedTags)
        {
            var orderdTags = unorderedTags.OrderBy(x => x.Tag).OrderByDescending(x => x.NumberOfBooksInTag);
            ObservableCollection<SelectableTagModel> orderedObservableTags = new();
            foreach (SelectableTagModel tag in orderdTags)
            {
                orderedObservableTags.Add(tag);
            }

            return orderedObservableTags;
        }

        private void OnSelectedTagChanged(object? sender, PropertyChangedEventArgs e)
        {
            RefreshBooksCollectionView();
        }

        private void OnBookUpdated(BookModel book)
        {
            RefreshBooksCollectionView();
        }

        private void RefreshBooksCollectionView()
        {
            BooksCollectionView.Refresh();
            CalculateNumbers();
            SetHighlightTextForBooks();
        }

        private void SetHighlightTextForBooks()
        {
            foreach (BookCardViewModel book in BookCards)
            {
                book.HighlightText = BooksFilter;
            }
        }

        protected override void OnDispose()
        {
            _booksStore.BookUpdated -= OnBookUpdated;
            foreach (var item in TagList.SelectableTags)
            {
                item.PropertyChanged -= OnSelectedTagChanged;
            }

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
            PopulateTagList();
        }

        public void CalculateNumbers()
        {
            NumberOfBooks = 0;
            NumberOfReadBooks = 0;
            PercentageOfReadBooks = 0;

            foreach (BookCardViewModel shownBook in BooksCollectionView)
            {
                NumberOfBooks += 1;
                if (shownBook.IsRead)
                {
                    NumberOfReadBooks += 1;
                }
                PercentageOfReadBooks = CalculatePercentage(NumberOfBooks, NumberOfReadBooks);
            }
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

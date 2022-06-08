using DomainLogic.Library.Services;
using DomainLogic.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfUi.Stores;
using Models.Library;
using System.ComponentModel;
using WpfUi.Models;

namespace WpfUi.ViewModels
{
	public class BookListViewModel : ViewModelBase
	{
		private readonly IBookDataService _bookDataService;
		private readonly BookStore _booksStore;
		private readonly IBooksUpdater _booksUpdater;

		public int NumberOfBooks
		{
			get
			{
				return _numberOfBooks;
			}
			set
			{
				_numberOfBooks = value;
				OnPropertyChanged(nameof(NumberOfBooks));
			}
		}
		private int _numberOfBooks;
		public int NumberOfReadBooks
		{
			get
			{
				return _numberOfReadBooks;
			}
			set
			{
				_numberOfReadBooks = value;
				OnPropertyChanged(nameof(NumberOfReadBooks));
			}
		}
		private int _numberOfReadBooks;
		public int PercentageOfReadBooks
		{
			get
			{
				return _percentageOfReadBooks;
			}
			set
			{
				_percentageOfReadBooks = value;
				OnPropertyChanged(nameof(PercentageOfReadBooks));
			}
		}
		private int _percentageOfReadBooks;


		public bool IsShowReadBooks
		{
			get
			{
				return _isShowReadBooks;
			}
			set
			{
				_isShowReadBooks = value;
				OnPropertyChanged(nameof(IsShowReadBooks));
				RefreshBooksCollectionView();
			}
		}
		private bool _isShowReadBooks = true;
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
		private string _booksFilter = string.Empty;

		public TagListViewModel TagList
		{
			get
			{
				return _tagList;
			}
			set
			{
				_tagList = value;
				OnPropertyChanged(nameof(TagList));
			}
		}
		private TagListViewModel _tagList;
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
		private ObservableCollection<BookCardViewModel> _bookCards = new();
		public ListCollectionView BooksCollectionView
		{
			get
			{
				return _booksCollectionView;
			}

			set
			{
				_booksCollectionView = value;
				OnPropertyChanged(nameof(BooksCollectionView));
			}
		}
		private ListCollectionView _booksCollectionView;

		public BookListViewModel(IBookDataService bookDataService,
						   BookStore booksStore,
						   IBooksUpdater booksUpdater)
		{
			_bookDataService = bookDataService;
			_booksStore = booksStore;
			_booksUpdater = booksUpdater;

			_booksCollectionView = new(_bookCards);
			_tagList = new TagListViewModel();
			if (_bookCards == null)
			{
				_bookCards = new ObservableCollection<BookCardViewModel>();
			}
			_booksStore.BooksLoaded += OnBooksLoaded;

			_booksStore.BookUpdated += OnBookUpdated;
			
		}

		private void OnBooksLoaded()
		{
			LoadBookList();
		}

		public void LoadBookList()
		{
			BookCards = new ObservableCollection<BookCardViewModel>();
			SetUpBooksCollectionView();

			LoadBooksData();
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
		private void CalculateNumbers()
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
		public static int CalculatePercentage(int total, int part)
		{
			if (total == 0)
			{
				return 0;
			}
			return part * 100 / total;
		}
		private void SetHighlightTextForBooks()
		{
			foreach (BookCardViewModel book in BookCards)
			{
				book.HighlightText = BooksFilter;
			}
		}
		public void SetUpBooksCollectionView()
		{
			BooksCollectionView = new(_bookCards)
			{
				Filter = FilterBooks
			};
			BooksCollectionView.SortDescriptions.Add(new SortDescription(nameof(BookCardViewModel.BookName),
				ListSortDirection.Ascending));
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
		private List<SelectableTagModel> GetSelectedTags()
		{
			return TagList.SelectableTags.Where(x => x.IsSelected).ToList();
		}
		private static bool DoesBookHaveAllSelectedTags(BookCardViewModel bookCardViewModel, List<SelectableTagModel> selectedTags)
		{
			ObservableCollection<string> bookTags = bookCardViewModel.Tags;
			List<string> selectedTagNames = selectedTags.Select(x => x.Tag).ToList();

			bool doesBookHaveAllSelectedTags = selectedTagNames.All(tagName => bookTags.Contains(tagName));

			return doesBookHaveAllSelectedTags;
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

		private void LoadBooksData()
		{
			//AddBooksAsync().ContinueWith(task =>
			//{
			//	if (task.Exception is null)
			//	{
			//		PopulateTagList();

			//		CalculateNumbers();
			//	}
			//});

			AddBooks();
			PopulateTagList();
			CalculateNumbers();
		}
		private async Task AddBooksAsync()
		{
			await Task.Run(() => AddBooks());
		}
		private void AddBooks()
		{
			List<BookModel> books = _booksStore.Books.ToList();
			foreach (BookModel book in books)
			{
				BookCards.Add(new BookCardViewModel(book, _booksStore, _booksUpdater));
			}
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
		private static bool IsTagInList(string tag, ObservableCollection<SelectableTagModel> tagList)
		{
			return tagList.Any(x => x.Tag == tag);
		}
		private static void IncrementNumberOfBooksInTagInList(string tag, ObservableCollection<SelectableTagModel> tagList)
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
		private void OnSelectedTagChanged(object? sender, PropertyChangedEventArgs e)
		{
			RefreshBooksCollectionView();
		}
		private static ObservableCollection<SelectableTagModel> GetOrderedTagsByNumberOfBooksDecending(ObservableCollection<SelectableTagModel> unorderedTags)
		{
			IOrderedEnumerable<SelectableTagModel>? orderdTags = unorderedTags.OrderBy(x => x.Tag).OrderByDescending(x => x.NumberOfBooksInTag);
			ObservableCollection<SelectableTagModel> orderedObservableTags = new();
			foreach (SelectableTagModel tag in orderdTags)
			{
				orderedObservableTags.Add(tag);
			}

			return orderedObservableTags;
		}

		protected override void OnDispose()
		{
			_booksStore.BookUpdated -= OnBookUpdated;
			foreach (SelectableTagModel? item in TagList.SelectableTags)
			{
				item.PropertyChanged -= OnSelectedTagChanged;
			}

			base.OnDispose();
		}
	}
}

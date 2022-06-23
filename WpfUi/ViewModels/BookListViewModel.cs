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
using WpfUi.Helpers;
using System.Windows.Input;
using WpfUi.ViewModels.Cmds;

namespace WpfUi.ViewModels
{
	public class BookListViewModel : ViewModelBase
	{
		private readonly BookStore _booksStore;

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

		


		public BookListViewModel(BookStore booksStore)
		{
			_booksStore = booksStore;

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
				BookListFilter bookListFilter = new(bookCardViewModel, IsShowReadBooks, BooksFilter, TagList.SelectableTags);
				return bookListFilter.IsBookShown();
			}
			return false;
		}
		private void LoadBooksData()
		{
			AddBooksAsync().ContinueWith(task =>
			{
				PopulateTagList();
				CalculateNumbers();
			});
			
		}
		private async Task AddBooksAsync()
		{
			List<BookModel> books = await Task.Run(() => _booksStore.Books.ToList());
			foreach (BookModel book in books)
			{
				BookCards.Add(new BookCardViewModel(book, _booksStore));
			}
		}
		public void PopulateTagList()
		{
			SelectableTagListFromBookListGenerator selectableTagListFromBookListGenerator = new(
				BookCards, this);
			TagList.SetUpTagList(selectableTagListFromBookListGenerator.GetSelectableTagModels());
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
		public void RefreshBooksCollectionView()
		{
			BooksCollectionView.Refresh();
			CalculateNumbers();
			SetHighlightTextForBooks();
			TagList.RefreshTagList();
		}
		private void SetHighlightTextForBooks()
		{
			foreach (BookCardViewModel book in BookCards)
			{
				book.HighlightText = BooksFilter;
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
		private void OnBookUpdated(BookModel book)
		{
			RefreshBooksCollectionView();
		}

		protected override void OnDispose()
		{
			_booksStore.BooksLoaded -= OnBooksLoaded;
			_booksStore.BookUpdated -= OnBookUpdated;
			base.OnDispose();
		}
	}
}

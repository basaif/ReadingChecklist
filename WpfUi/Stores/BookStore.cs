using DomainLogic.Library;
using DomainLogic.Library.Services;
using Microsoft.Extensions.Hosting;
using Models.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfUi.Stores
{
	public class BookStore
	{
		private readonly IBookDataService _bookDataService;
		private readonly IBookUpdater _bookUpdater;
		private readonly List<BookModel> _books;

		private Lazy<Task> _loadBooksLazy;

		public event Action<BookModel>? BookUpdated;
		public IEnumerable<BookModel> Books
		{
			get
			{
				return _books;
			}
		}

		public event Action? BooksLoaded;

		public BookStore(IBookDataService bookDataService, IBookUpdater bookUpdater)
		{
			_bookDataService = bookDataService;
			_bookUpdater = bookUpdater;
			_books = new List<BookModel>();

			_loadBooksLazy = CreateLoadBooksLazy();
		}

		public async Task LoadBooksAsync()
		{
			await _loadBooksLazy.Value;
		}
		public async Task RefreshBooksAsync()
		{
			_loadBooksLazy = CreateLoadBooksLazy();
			await LoadBooksAsync();
		}

		private Lazy<Task> CreateLoadBooksLazy()
		{
			return new Lazy<Task>(() => InitializeBooksAsync());
		}

		private async Task InitializeBooksAsync()
		{
			IEnumerable<BookModel> books = await _bookDataService.GetExistingBooksAsync();

			_books.Clear();
			_books.AddRange(books);

			BooksLoaded?.Invoke();
		}

		public void UpdateBookReadStatus(BookModel book, bool isRead, DateTime readDate)
		{
			_bookUpdater.ChangeReadStatus(book, isRead, readDate);
			BookUpdated?.Invoke(book);
		}
	}
}

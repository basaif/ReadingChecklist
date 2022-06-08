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

		public BookStore(IBookDataService bookDataService)
		{
			_bookDataService = bookDataService;
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

		public void UpdateBook(BookModel book)
		{
			BookUpdated?.Invoke(book);
		}
	}
}

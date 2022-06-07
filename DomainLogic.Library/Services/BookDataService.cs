using DataAccess.Library;
using DataAccess.Library.ModelDataServices;
using Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLogic.Library.Services
{
	public class BookDataService : IBookDataService
	{
		private readonly ISqliteBookData _sqliteBookData;

		public BookDataService(ISqliteBookData sqliteBookData)
		{
			_sqliteBookData = sqliteBookData;
		}
		public BookModel UpdateBookTags(BookModel book, List<TagModel> tagModels)
		{
			_sqliteBookData.DeleteBookRelationship(book.Id);
			book.Tags = tagModels;
			_sqliteBookData.CreateBookTagRelationship(book);
			return book;
		}
		public BookModel AddNewBook(string bookName, List<TagModel> tagModels)
		{
			BookModel book = new(bookName, false, DateTime.UtcNow, tagModels);
			_sqliteBookData.CreateBook(book);
			return book;
		}

		public void DeleteBooks(List<BookModel> books)
		{
			foreach (BookModel book in books)
			{
				_sqliteBookData.DeleteBook(book);
			}
		}

		public List<BookModel> GetExistingBooks()
		{
			return _sqliteBookData.ReadAllBooks();
		}
	}
}

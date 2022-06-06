using DataAccess.Library;
using Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLogic.Library
{
	public class BookDataService : IBookDataService
	{
		public BookModel UpdateBookTags(BookModel book, List<TagModel> tagModels)
		{
			SqliteDeleter.DeleteBookRelationship(book.Id);
			book.Tags = tagModels;
			SqliteCreater.CreateBookTagRelationship(book);
			return book;
		}
		public BookModel AddNewBook(string bookName, List<TagModel> tagModels)
		{
			BookModel book = new(bookName, false, DateTime.UtcNow, tagModels);
			SqliteCreater.CreateBook(book);
			return book;
		}

		public void DeleteBooks(List<BookModel> books)
		{
			foreach (BookModel book in books)
			{
				SqliteDeleter.DeleteBook(book);
			}
		}

		public List<BookModel> GetExistingBooks()
		{
			return SqliteReader.ReadAllBooks();
		}
	}
}

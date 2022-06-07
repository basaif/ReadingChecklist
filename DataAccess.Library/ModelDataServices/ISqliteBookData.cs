using Models.Library;

namespace DataAccess.Library.ModelDataServices
{
	public interface ISqliteBookData
	{
		void AddBookTags(int bookId, int tagId);
		void CreateBook(BookModel book);
		void CreateBookTagRelationship(BookModel book);
		void DeleteBook(BookModel book);
		void DeleteBookRelationship(int bookId);
		List<BookModel> ReadAllBooks();
		BookModel ReadBookById(int id);
		List<BookModel> ReadBooksByTag(int tagId);
		void UpdateBook(BookModel book);
	}
}
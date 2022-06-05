using DataAccess.Library;
using Models.Library;

namespace DomainLogic.Library
{
	public class BookDataGetter : IBookDataGetter
	{
		public List<BookModel> GetAllBooks()
		{
			return SqliteReader.ReadAllBooks();
		}

	}
}

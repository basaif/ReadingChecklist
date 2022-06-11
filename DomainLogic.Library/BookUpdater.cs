using DataAccess.Library.ModelDataServices;
using DataAccess.Library.SqliteDataAccess;
using Models.Library;

namespace DomainLogic.Library
{
	public class BookUpdater : IBookUpdater
	{
		private readonly ISqliteBookData _sqliteBookData;

		public BookUpdater(ISqliteBookData sqliteBookData)
		{
			_sqliteBookData = sqliteBookData;
		}

		public void ChangeReadStatus(BookModel bookToUpdate, bool isRead, DateTime date)
		{
			bookToUpdate.IsRead = isRead;
			bookToUpdate.DateRead = date;

			UpdateBook(bookToUpdate);
		}

		private void UpdateBook(BookModel bookToUpdate)
		{
			_sqliteBookData.UpdateBook(bookToUpdate);
		}

	}
}

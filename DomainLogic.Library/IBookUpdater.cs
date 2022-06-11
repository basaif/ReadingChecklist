using Models.Library;

namespace DomainLogic.Library
{
	public interface IBookUpdater
	{
		void ChangeReadStatus(BookModel bookToUpdate, bool isRead, DateTime date);
	}
}
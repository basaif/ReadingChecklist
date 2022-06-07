using Models.Library;

namespace DomainLogic.Library
{
	public interface IBooksUpdater
	{
		void ChangeReadStatus(BookModel bookToUpdate, bool isRead, DateTime date);
	}
}
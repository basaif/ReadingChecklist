using Models.Library;

namespace DomainLogic.Library
{
	public interface IBooksDataRefresher
	{
		bool AreTagsInBook(BookModel bookModel, List<TagModel> tagModels);
		BookModel CreateBook(string bookName, List<string> tags);
		void RefreshBooksData();
	}
}
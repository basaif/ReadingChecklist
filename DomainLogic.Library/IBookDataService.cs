using Models.Library;

namespace DomainLogic.Library
{
	public interface IBookDataService
	{
		BookModel AddNewBook(string bookName, List<TagModel> tagModels);
		void DeleteBooks(List<BookModel> books);
		List<BookModel> GetExistingBooks();
		BookModel UpdateBookTags(BookModel book, List<TagModel> tagModels);
	}
}
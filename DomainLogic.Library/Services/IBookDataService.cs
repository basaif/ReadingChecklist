using Models.Library;

namespace DomainLogic.Library.Services
{
	public interface IBookDataService
	{
		BookModel AddNewBook(string bookName, List<TagModel> tagModels);
		void DeleteBooks(List<BookModel> books);
		List<BookModel> GetExistingBooks();
		Task<List<BookModel>> GetExistingBooksAsync();
		BookModel UpdateBookTags(BookModel book, List<TagModel> tagModels);
	}
}
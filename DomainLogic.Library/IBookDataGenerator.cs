using Models.Library;

namespace DomainLogic.Library
{
	public interface IBookDataGenerator
	{
		BookModel CreateBook(string bookName, List<string> tags);
		void GenerateBooksData();
	}
}
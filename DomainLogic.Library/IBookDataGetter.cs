using Models.Library;

namespace DomainLogic.Library
{
	public interface IBookDataGetter
	{
		List<BookModel> GetAllBooks();
	}
}
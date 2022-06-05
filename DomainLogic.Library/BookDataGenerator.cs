using DataAccess.Library;
using FileSystemUtilities.Library;
using Models.Library;

namespace DomainLogic.Library
{
	public class BookDataGenerator : IBookDataGenerator
	{

		private readonly IFoldersFileNamePairs _foldersFileNamePairs;

		private readonly ITagsCreator _tagsCreator;
		private readonly List<BookModel> _allBooks = new();

		public BookDataGenerator(IFoldersFileNamePairs foldersFileNamePairs, ITagsCreator tagsCreator)
		{
			_foldersFileNamePairs = foldersFileNamePairs;
			_tagsCreator = tagsCreator;
		}

		public void GenerateBooksData()
		{
			List<(List<string> Tags, string BookName)> tagsBookPairs = _foldersFileNamePairs.GetAllFoldersFileNamePairsInLocation();

			foreach ((List<string> Tags, string BookName) in tagsBookPairs)
			{
				_tagsCreator.AddTags(Tags);

				_allBooks.Add(CreateBook(BookName, Tags));
			}
		}

		public BookModel CreateBook(string bookName, List<string> tags)
		{
			BookModel book = new(bookName, false, DateTime.UtcNow, _tagsCreator.GetTagModelsFromList(tags));
			SqliteCreater.CreateBook(book);
			return book;
		}
	}
}
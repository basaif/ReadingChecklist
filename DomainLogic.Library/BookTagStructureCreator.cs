using DataAccess.Library;
using FileSystemUtilities.Library;
using Models.Library;

namespace DomainLogic.Library
{
	public class BookTagStructureCreator : IBookTagStructureCreator
	{
		private readonly IFoldersFileNamePairs _foldersFileNamePairs;
		private readonly ITagsCreator _tagsCreator;
		private readonly IBookDataService _bookDataService;

		private readonly List<BookModel> _allNewBooks = new();
		private List<BookModel> _allOldBooks = new();

		public BookTagStructureCreator(IFoldersFileNamePairs foldersFileNamePairs,
			ITagsCreator tagsCreator,
			IBookDataService bookDataService)
		{
			_foldersFileNamePairs = foldersFileNamePairs;
			_tagsCreator = tagsCreator;
			_bookDataService = bookDataService;
		}
		public void LoadBookData()
		{
			ClearBookLists();
			GetOldBooks();
			CreateBookTagStructure();
			_bookDataService.DeleteBooks(GetOldBooksThatAreNotInNewBooks());
			ClearBookLists();
		}
		private void GetOldBooks()
		{
			_allOldBooks = _bookDataService.GetExistingBooks();
		}
		private void ClearBookLists()
		{
			_allOldBooks.Clear();
			_allNewBooks.Clear();
		}

		private void CreateBookTagStructure()
		{
			List<(List<string> Tags, string BookName)> tagsBookPairs =
					_foldersFileNamePairs.GetAllFoldersFileNamePairsInLocation();

			foreach ((List<string> Tags, string BookName) in tagsBookPairs)
			{
				_tagsCreator.AddTags(Tags);

				_allNewBooks.Add(CreateBook(BookName, Tags));
			}
		}
		private BookModel CreateBook(string bookName, List<string> tags)
		{
			List<TagModel> tagModels = _tagsCreator.GetTagModelsFromList(tags);

			if (DoesBookExist(bookName))
			{
				return GetBookWithCorrectTags(bookName, tagModels);
			}
			else
			{
				return _bookDataService.AddNewBook(bookName, tagModels);
			}
		}
		private bool DoesBookExist(string bookName)
		{
			bool doesBookExist = _allOldBooks.Any(x => x.BookName == bookName);
			return doesBookExist;
		}
		private BookModel GetBookWithCorrectTags(string bookName, List<TagModel> tagModels)
		{
			BookModel book = GetExistingBookModel(bookName);
			if (AreTagsInBook(book, tagModels))
			{
				return book;
			}
			else
			{
				return _bookDataService.UpdateBookTags(book, tagModels);
			}
		}
		private BookModel GetExistingBookModel(string bookName)
		{
			BookModel book = _allOldBooks.First(x => x.BookName == bookName);
			return book;
		}
		private static bool AreTagsInBook(BookModel bookModel, List<TagModel> tagModels)
		{
			bool areTagsInBook = bookModel.Tags.All(x => tagModels.Contains(x))
				&& tagModels.All(x => bookModel.Tags.Contains(x));
			return areTagsInBook;
		}
		
		private List<BookModel> GetOldBooksThatAreNotInNewBooks()
		{
			List<BookModel> books = _allOldBooks
				.Where(x => !_allNewBooks.Select(x => x.BookName)
				.Contains(x.BookName)).ToList();
			return books;
		}
		
	}
}

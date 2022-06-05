using DataAccess.Library;
using FileSystemUtilities.Library;
using Models.Library;

namespace DomainLogic.Library
{
	public class BooksDataRefresher : IBooksDataRefresher
	{
		private readonly IFoldersFileNamePairs _foldersFileNamePairs;
		private readonly ITagsCreator _tagsCreator;

		private List<BookModel> _allNewBooks = new();
		private List<BookModel> _allOldBooks = new();

		public BooksDataRefresher(IFoldersFileNamePairs foldersFileNamePairs, ITagsCreator tagsCreator)
		{
			_foldersFileNamePairs = foldersFileNamePairs;
			_tagsCreator = tagsCreator;
		}


		public void RefreshBooksData()
		{
			ClearBookLists();
			GetOldBooks();
			CreateBookListStructure();
			DeleteBooks(GetMissingBooks());
			ClearBookLists();
		}

		private void CreateBookListStructure()
		{
			List<(List<string> Tags, string BookName)> tagsBookPairs =
					_foldersFileNamePairs.GetAllFoldersFileNamePairsInLocation();

			foreach ((List<string> Tags, string BookName) in tagsBookPairs)
			{
				_tagsCreator.AddTags(Tags);

				_allNewBooks.Add(CreateBook(BookName, Tags));
			}
		}

		private void GetOldBooks()
		{
			_allOldBooks = SqliteReader.ReadAllBooks();
		}

		private void ClearBookLists()
		{
			_allOldBooks.Clear();
			_allNewBooks.Clear();
		}

		public BookModel CreateBook(string bookName, List<string> tags)
		{
			List<TagModel> tagModels = _tagsCreator.GetTagModelsFromList(tags);

			if (DoesBookExist(bookName))
			{
				return GetBookWithCorrectTags(bookName, tagModels);
			}
			else
			{
				return AddNewBook(bookName, tagModels);
			}
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
				return UpdateBookTags(book, tagModels);
			}
		}

		private static BookModel AddNewBook(string bookName, List<TagModel> tagModels)
		{
			BookModel book = new(bookName, false, DateTime.UtcNow, tagModels);
			SqliteCreater.CreateBook(book);
			return book;
		}

		private static BookModel UpdateBookTags(BookModel book, List<TagModel> tagModels)
		{
			SqliteDeleter.DeleteBookRelationship(book.Id);
			book.Tags = tagModels;
			SqliteCreater.CreateBookTagRelationship(book);
			return book;
		}

		private bool DoesBookExist(string bookName)
		{
			bool doesBookExist = _allOldBooks.Any(x => x.BookName == bookName);
			return doesBookExist;
		}

		private BookModel GetExistingBookModel(string bookName)
		{
			BookModel book = _allOldBooks.First(x => x.BookName == bookName);
			return book;
		}

		public bool AreTagsInBook(BookModel bookModel, List<TagModel> tagModels)
		{
			bool areTagsInBook = bookModel.Tags.All(x => tagModels.Contains(x)) && tagModels.All(x => bookModel.Tags.Contains(x));
			return areTagsInBook;
		}

		private List<BookModel> GetMissingBooks()
		{
			List<BookModel> books = _allOldBooks.Where(x => !_allNewBooks.Select(x => x.BookName).Contains(x.BookName)).ToList();
			return books;
		}

		private void DeleteBooks(List<BookModel> books)
		{
			foreach (BookModel book in books)
			{
				SqliteDeleter.DeleteBook(book);
			}
		}
	}
}

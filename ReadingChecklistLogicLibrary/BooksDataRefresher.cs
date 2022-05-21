using FileManagementLibrary;
using ReadingChecklistDataAccess;
using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistLogicLibrary
{
    public class BooksDataRefresher
    {
        private readonly FilesManager _filesManager;
        private readonly TagsCreator _tagsCreator;

        public List<BookModel> AllNewBooks { get; set; } = new();
        public List<BookModel> AllOldBooks { get; set; } = new();

        public BooksDataRefresher(FilesManager filesManager, TagsCreator tagsCreator)
        {
            _filesManager = filesManager;
            _tagsCreator = tagsCreator;
        }

        public void RefreshBooksData()
        {
            GetOldBooks();

            List<(List<string> Tags, string BookName)> tagsBookPairs = _filesManager.GetAllFoldersFileNamePairsInLocation();

            foreach (var (Tags, BookName) in tagsBookPairs)
            {
                _tagsCreator.AddTags(Tags);

                AllNewBooks.Add(CreateBook(BookName, Tags));
            }

            DeleteBooks(GetMissingBooks());
        }

        private void GetOldBooks()
        {
            AllOldBooks = SqliteReader.ReadAllBooks();
        }

        public BookModel CreateBook(string bookName, List<string> tags)
        {

            List<TagModel> tagModels = _tagsCreator.GetTagModelsFromList(tags);

            if (DoesBookExist(bookName))
            {
                BookModel book = GetExistingBookModel(bookName);

                if (AreTagsInBook(book, tagModels))
                {
                    return book;
                }
                else
                {
                    SqliteDeleter.DeleteBookRelationship(book.Id);
                    book.Tags = tagModels;
                    SqliteCreater.CreateBookTagRelationship(book);
                    return book;
                }
            }
            else
            {
                BookModel book = new(bookName, false, DateTime.UtcNow, tagModels);
                SqliteCreater.CreateBook(book);
                return book;
            }
           
        }

        private bool DoesBookExist(string bookName)
        {
            return AllOldBooks.Any(x => x.BookName == bookName);
        }

        private BookModel GetExistingBookModel(string bookName)
        {
            return AllOldBooks.First(x => x.BookName == bookName);
        }

        private bool AreTagsInBook(BookModel bookModel, List<TagModel> tagModels)
        {
            return bookModel.Tags.All(x => tagModels.Contains(x)) && tagModels.All(x => bookModel.Tags.Contains(x));
        }

        private List<BookModel> GetMissingBooks()
        {
            return AllOldBooks.Where(x => !AllNewBooks.Select(x => x.BookName).Contains(x.BookName)).ToList();
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

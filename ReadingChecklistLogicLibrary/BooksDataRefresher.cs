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
            AllNewBooks = new();
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
            bool doesBookExist = AllOldBooks.Any(x => x.BookName == bookName);
            return doesBookExist;
        }

        private BookModel GetExistingBookModel(string bookName)
        {
            BookModel book = AllOldBooks.First(x => x.BookName == bookName);
            return book;
        }

        public bool AreTagsInBook(BookModel bookModel, List<TagModel> tagModels)
        {
            bool areTagsInBook = bookModel.Tags.All(x => tagModels.Contains(x)) && tagModels.All(x => bookModel.Tags.Contains(x));
            return areTagsInBook;
        }

        private List<BookModel> GetMissingBooks()
        {
            List<BookModel> books = AllOldBooks.Where(x => !AllNewBooks.Select(x => x.BookName).Contains(x.BookName)).ToList();
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

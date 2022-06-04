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
        private readonly IFoldersFileNamePairs _foldersFileNamePairs;
        private readonly TagsCreator _tagsCreator;

        private List<BookModel> _allNewBooks = new();
        private List<BookModel> _allOldBooks = new();

        public BooksDataRefresher(IFoldersFileNamePairs foldersFileNamePairs, TagsCreator tagsCreator)
        {
            _foldersFileNamePairs = foldersFileNamePairs;
            _tagsCreator = tagsCreator;
        }


        public void RefreshBooksData()
        {
            GetOldBooks();

            List<(List<string> Tags, string BookName)> tagsBookPairs = _foldersFileNamePairs.GetAllFoldersFileNamePairsInLocation();

            foreach (var (Tags, BookName) in tagsBookPairs)
            {
                _tagsCreator.AddTags(Tags);

                _allNewBooks.Add(CreateBook(BookName, Tags));
            }

            DeleteBooks(GetMissingBooks());
            _allNewBooks = new();
        }

        private void GetOldBooks()
        {
            _allOldBooks = SqliteReader.ReadAllBooks();
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

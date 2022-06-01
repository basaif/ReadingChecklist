using FileManagementLibrary;
using ReadingChecklistDataAccess;
using ReadingChecklistModels;

namespace ReadingChecklistLogicLibrary
{
    public class BookDataGenerator
    {

        private readonly FilesManager _filesManager;

        private readonly TagsCreator _tagsCreator;
        private readonly List<BookModel> _allBooks = new();

        public BookDataGenerator(FilesManager filesManager, TagsCreator tagsCreator)
        {
            _filesManager = filesManager;
            _tagsCreator = tagsCreator;
        }

        public void GenerateBooksData()
        {
            List<(List<string> Tags, string BookName)> tagsBookPairs = _filesManager.GetAllFoldersFileNamePairsInLocation();

            foreach (var (Tags, BookName) in tagsBookPairs)
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
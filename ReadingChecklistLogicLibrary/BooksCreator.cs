using ReadingChecklistDataAccess;
using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistLogicLibrary
{
    public class BooksCreator
    {
        private readonly string _bookDirectory;

        private readonly List<TagModel> _allTags = new();

        private readonly List<BookModel> _allBooks = new();

        public BooksCreator(string dir)
        {
            _bookDirectory = dir;
        }


        public void CreateDataFromDirectory()
        {
            if (Directory.Exists(_bookDirectory))
            {
                string[] files = Directory.GetFiles(_bookDirectory, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);

                    List<string> tags = GetTagsFromFilePath(file, fileName);

                    AddTags(tags);

                    BookModel book = new(fileName, false, DateTime.UtcNow, GetTagModels(tags));

                    SqliteCreater.CreateBook(book);

                    _allBooks.Add(book);

                }
            }
            else
            {
                throw new DirectoryNotFoundException();
            }

        }

        public List<string> GetTagsFromFilePath(string filePath, string fileName)
        {
            List<string> tags = new();

            string[] tokens = filePath.Split('\\');
            foreach (string t in tokens)
            {
                if (!(_bookDirectory.Contains(t) || t == fileName))
                {
                    if (!tags.Contains(t))
                    {
                        tags.Add(t);
                    }
                }
            }
            return tags;
        }

        public void AddTags(List<string> tags)
        {
            foreach (string t in tags)
            {
                TagModel tag = new(t);

                bool doesTagExist = _allTags.Any(t => t.TagName == tag.TagName);

                if (!doesTagExist)
                {
                    SqliteCreater.CreateTag(tag);
                    _allTags.Add(tag);
                }
            }
        }

        public List<TagModel> GetTagModels(List<string> tags)
        {
            List<TagModel> tagModels = new();

            tagModels = _allTags.Where(t => tags.Contains(t.TagName)).ToList();

            return tagModels;
        }
    }
}

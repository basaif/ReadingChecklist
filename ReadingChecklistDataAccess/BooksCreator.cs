using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistDataAccess
{
    public class BooksCreator
    {
        public string BookDirectory { get; set; }

        public List<TagModel> AllTags { get; set; } = new();

        public List<BookModel> AllBooks { get; set; } = new();

        public BooksCreator(string dir)
        {
            BookDirectory = dir;
        }

        public void CreateDataFromDirectory()
        {
            if (Directory.Exists(BookDirectory))
            {
                string[] files = Directory.GetFiles(BookDirectory, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);

                    List<string> tags = GetTagsFromFilePath(file, fileName);

                    AddTags(tags);

                    BookModel book = new(fileName, false, DateTime.UtcNow, GetTagModels(tags));

                    SqliteCreater.CreateBook(book);

                    AllBooks.Add(book);

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
            foreach(string t in tokens)
            {
                if (!(BookDirectory.Contains(t) || t == fileName))
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
            foreach(string t in tags)
            {
                TagModel tag = new(t);

                bool doesTagExist = AllTags.Any(t => t.TagName == tag.TagName);

                if (!doesTagExist)
                {
                    SqliteCreater.CreateTag(tag);
                    AllTags.Add(tag);
                }
            }
        }

        public List<TagModel> GetTagModels(List<string> tags)
        {
            List<TagModel> tagModels = new();

            tagModels = AllTags.Where(t => tags.Contains(t.TagName)).ToList();

            return tagModels;
        }
    }
}

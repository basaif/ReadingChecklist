using ReadingChecklistDataAccess;
using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistLogicLibrary
{
    public class TagsCreator
    {
        public List<TagModel> AllTags { get; set; } = new();

        public TagsCreator()
        {
            AllTags = SqliteReader.ReadAllTags();
        }
        public void AddTags(List<string> tags)
        {
            foreach (string t in tags)
            {
                if (!IsTagNameInList(t))
                {
                    AllTags.Add(CreateTag(t));
                }
            }
        }

        private TagModel CreateTag(string tagName)
        {
            TagModel tag = new(tagName);

            SqliteCreater.CreateTag(tag);

            return tag;
        }

        public bool IsTagNameInList(string tagName)
        {
            return AllTags.Any(t => t.TagName == tagName);
        }

        public List<TagModel> GetTagModelsFromList(List<string> tags)
        {
            List<TagModel> tagModels = new();

            tagModels = AllTags.Where(t => tags.Contains(t.TagName)).ToList();

            return tagModels;
        }
    }
}

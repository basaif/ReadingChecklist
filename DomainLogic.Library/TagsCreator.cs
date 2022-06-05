using DataAccess.Library;
using Models.Library;

namespace DomainLogic.Library
{
	public class TagsCreator : ITagsCreator
	{
		private readonly List<TagModel> _allTags = new();

		public TagsCreator()
		{
			_allTags = LoadTags();
		}

		public List<TagModel> LoadTags()
		{
			return SqliteReader.ReadAllTags();
		}

		public void AddTags(List<string> tags)
		{
			foreach (string t in tags)
			{
				if (!IsTagNameInList(t))
				{
					_allTags.Add(CreateTag(t));
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
			return _allTags.Any(t => t.TagName == tagName);
		}

		public List<TagModel> GetTagModelsFromList(List<string> tags)
		{
			List<TagModel> tagModels = new();

			tagModels = _allTags.Where(t => tags.Contains(t.TagName)).ToList();

			return tagModels;
		}
	}
}

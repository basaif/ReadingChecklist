using DataAccess.Library;
using DomainLogic.Library.Services;
using Models.Library;

namespace DomainLogic.Library.Creators
{
	public class TagsCreator : ITagsCreator
	{
		private readonly List<TagModel> _allTags = new();
		private readonly ITagDataService _tagDataService;

		public TagsCreator(ITagDataService tagDataService)
		{
			_tagDataService = tagDataService;
			_allTags = _tagDataService.LoadTags();
		}
		public List<TagModel> GetTagModelsFromList(List<string> tags)
		{
			List<TagModel> tagModels = new();

			tagModels = _allTags.Where(t => tags.Contains(t.TagName)).ToList();

			return tagModels;
		}
		public void AddTags(List<string> tags)
		{
			foreach (string t in tags)
			{
				if (!IsTagNameInList(t))
				{
					_allTags.Add(_tagDataService.CreateTag(t));
				}
			}
		}

		private bool IsTagNameInList(string tagName)
		{
			return _allTags.Any(t => t.TagName == tagName);
		}

	}
}

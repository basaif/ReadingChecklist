using Models.Library;

namespace DomainLogic.Library
{
	public interface ITagDataService
	{
		TagModel CreateTag(string tagName);
		List<TagModel> LoadTags();
	}
}
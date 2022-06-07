using Models.Library;

namespace DomainLogic.Library.Services
{
	public interface ITagDataService
	{
		TagModel CreateTag(string tagName);
		List<TagModel> LoadTags();
	}
}
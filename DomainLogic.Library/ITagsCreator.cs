using Models.Library;

namespace DomainLogic.Library
{
	public interface ITagsCreator
	{
		void AddTags(List<string> tags);
		List<TagModel> GetTagModelsFromList(List<string> tags);
		bool IsTagNameInList(string tagName);
		List<TagModel> LoadTags();
	}
}
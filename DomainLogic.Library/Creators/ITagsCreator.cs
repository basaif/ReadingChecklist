using Models.Library;

namespace DomainLogic.Library.Creators
{
	public interface ITagsCreator
	{
		void AddTags(List<string> tags);
		List<TagModel> GetTagModelsFromList(List<string> tags);
	}
}
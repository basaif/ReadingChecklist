using Models.Library;

namespace DataAccess.Library.ModelDataServices
{
	public interface ISqliteTagData
	{
		void CreateTag(TagModel tag);
		void DeleteTag(TagModel tag);
		void DeleteTagRelationship(int tagId);
		bool IsTagInDatabase(TagModel tag, out int tagId);
		List<TagModel> ReadAllTags();
		TagModel ReadTagById(int id);
		List<TagModel> ReadTagsByBook(int bookId);
		void UpdateTag(TagModel tag);
	}
}
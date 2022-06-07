using Models.Library;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Library.SqliteDataAccess;

namespace DataAccess.Library.ModelDataServices
{
	public class SqliteTagData : ISqliteTagData
	{
		private readonly ISaveData _saveData;
		private readonly IQueryData<TagModel> _queryData;

		public SqliteTagData(ISaveData saveData, IQueryData<TagModel> queryData)
		{
			_saveData = saveData;
			_queryData = queryData;
		}
		public void UpdateTag(TagModel tag)
		{
			DynamicParameters p = new();

			p.Add("@Id", tag.Id);
			p.Add("@TagName", tag.TagName);

			string sql = "Update Tag set TagName = @TagName where Id = @Id";

			_saveData.ExecuteParameters(sql, p);
		}
		public void DeleteTag(TagModel tag)
		{
			DeleteTagRelationship(tag.Id);

			string sql = $"Delete from Tag where id = {tag.Id}";

			_saveData.ExecuteModel(sql, tag);
		}

		public void DeleteTagRelationship(int tagId)
		{
			string sql = $"Delete from Book_Tag where TagId = {tagId}";

			_saveData.ExecuteId(sql, tagId);
		}
		public List<TagModel> ReadAllTags()
		{
			string sql = "select id, TagName from Tag";
			List<TagModel> output = _queryData.GetList(sql);

			return output;
		}

		public List<TagModel> ReadTagsByBook(int bookId)
		{
			string sql = $"select * from Tag where Tag.Id In (select TagId from Book_Tag where BookId = {bookId})";

			List<TagModel> output = _queryData.GetList(sql);

			return output;
		}
		public TagModel ReadTagById(int id)
		{
			string sql = $"select id, TagName from Tag where id = {id}";

			TagModel output = _queryData.GetFirst(sql);

			return output;
		}
		public void CreateTag(TagModel tag)
		{
			DynamicParameters p = new();
			p.Add("@TagName", tag.TagName);

			string sql = "insert into Tag (TagName) values (@TagName); select last_insert_rowid()";

			tag.Id = _saveData.ExecuteParametersReturnId(sql, p);
		}

		public bool IsTagInDatabase(TagModel tag, out int tagId)
		{
			List<TagModel> tags = ReadAllTags();
			foreach (TagModel tagModel in tags)
			{
				if (tagModel.TagName == tag.TagName)
				{
					tagId = tagModel.Id;
					return true;
				}
			}
			tagId = 0;
			return false;
		}
	}
}

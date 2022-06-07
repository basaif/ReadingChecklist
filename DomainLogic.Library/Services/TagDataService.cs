using DataAccess.Library;
using DataAccess.Library.ModelDataServices;
using Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLogic.Library.Services
{
	public class TagDataService : ITagDataService
	{
		private readonly ISqliteTagData _sqliteTagData;

		public TagDataService(ISqliteTagData sqliteTagData)
		{
			_sqliteTagData = sqliteTagData;
		}
		public List<TagModel> LoadTags()
		{
			return _sqliteTagData.ReadAllTags();
		}

		public TagModel CreateTag(string tagName)
		{
			TagModel tag = new(tagName);

			_sqliteTagData.CreateTag(tag);

			return tag;
		}
	}
}

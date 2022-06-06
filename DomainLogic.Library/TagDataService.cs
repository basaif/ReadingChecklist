using DataAccess.Library;
using Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLogic.Library
{
	public class TagDataService : ITagDataService
	{
		public List<TagModel> LoadTags()
		{
			return SqliteReader.ReadAllTags();
		}

		public TagModel CreateTag(string tagName)
		{
			TagModel tag = new(tagName);

			SqliteCreater.CreateTag(tag);

			return tag;
		}
	}
}

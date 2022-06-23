using Models.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUi.Models
{
	public class SelectableTagModel : NotifyModel
	{
		private bool _isSelected;

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				OnPropertyChanged(nameof(IsSelected));
			}
		}
		public string Tag { get; set; } = string.Empty;
		public int NumberOfBooksInTag
		{
			get; set;
		}

		public List<string> RelatedTags { get; private set; } = new();

		public void AddRelatedTagsDistinctly(List<string> tags)
		{
			foreach (string tag in tags)
			{
				if (!RelatedTags.Contains(tag))
				{
					RelatedTags.Add(tag);
				}
			}
		}
	}
}

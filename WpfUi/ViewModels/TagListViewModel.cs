using WpfUi.Models;
using WpfUi.ViewModels.Cmds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using System.Diagnostics;
using WpfUi.Helpers;

namespace WpfUi.ViewModels
{
	public class TagListViewModel : ViewModelBase
	{
		private ObservableCollection<SelectableTagModel> _selectableTags;

		public ObservableCollection<SelectableTagModel> SelectableTags
		{
			get
			{
				if (_selectableTags == null)
				{
					_selectableTags = new ObservableCollection<SelectableTagModel>();
				}
				return _selectableTags;
			}

			set
			{
				if (value is not null)
				{
					_selectableTags = value;
				}
				else
				{
					_selectableTags = new ObservableCollection<SelectableTagModel>();
				}
				OnPropertyChanged(nameof(SelectableTags));
			}
		}

		public ListCollectionView TagsCollectionView
		{
			get
			{
				return _tagsCollectionView;
			}

			set
			{
				_tagsCollectionView = value;
				OnPropertyChanged(nameof(TagsCollectionView));
			}
		}
		private ListCollectionView _tagsCollectionView;

		public ICommand ClearSelectedTagsCommand
		{
			get;
		}

		public TagListViewModel()
		{
			if (_selectableTags == null)
			{
				_selectableTags = new ObservableCollection<SelectableTagModel>();
			}

			_tagsCollectionView = new(SelectableTags);

			ClearSelectedTagsCommand = new ClearSelectedTagsCommand(this);
		}

		public void SetUpTagList(ObservableCollection<SelectableTagModel> tags)
		{
			SelectableTags = tags;

			TagsCollectionView = new(SelectableTags)
			{
				Filter = FilterTags
			};
			TagsCollectionView.SortDescriptions.Add(new SortDescription(nameof(SelectableTagModel.NumberOfBooksInTag),
				ListSortDirection.Descending));
		}

		public void RefreshTagList()
		{
			TagsCollectionView.Refresh();
		}

		private bool FilterTags(object obj)
		{
			bool noTagsSelected = AreNoTagsSelected();

			if (noTagsSelected)
			{
				return true;
			}

			if (obj is SelectableTagModel selectableTagModel)
			{
				List<string> relatedTagsIntersecionOfSelectedTags = GetRelatedTagsIntersecion(GetSelectedTags());
				if (relatedTagsIntersecionOfSelectedTags.Contains(selectableTagModel.Tag))
				{
					return true;
				}
			}
			return false;
		}

		private bool AreNoTagsSelected()
		{
			foreach (SelectableTagModel selectableTag in TagsCollectionView)
			{
				if (selectableTag.IsSelected)
				{
					return false;
				}
			}
			return true;
		}

		private List<SelectableTagModel> GetSelectedTags()
		{
			List<SelectableTagModel> selectedTags = new();

			foreach (SelectableTagModel selectableTag in TagsCollectionView)
			{
				if (selectableTag.IsSelected)
				{
					selectedTags.Add(selectableTag);
				}
			}
			return selectedTags;
		}

		private static List<string> GetRelatedTagsIntersecion(List<SelectableTagModel> selectedTags)
		{
			List<List<string>> relatedTagsForEachTag = new();
			foreach (SelectableTagModel selectedTag in selectedTags)
			{
				relatedTagsForEachTag.Add(selectedTag.RelatedTags);
			}

			if (relatedTagsForEachTag.Count == 1)
			{
				return relatedTagsForEachTag[0];
			}

			return GetIntersectionOfListOfLists(relatedTagsForEachTag);
		}

		private static List<string> GetIntersectionOfListOfLists(List<List<string>> listOfList)
		{
			HashSet<string> intersection = listOfList
															.Skip(1)
															.Aggregate(
															new HashSet<string>(listOfList.First()),
															(h, e) => { h.IntersectWith(e); return h; }
															);

			return intersection.ToList();
		}
	}
}

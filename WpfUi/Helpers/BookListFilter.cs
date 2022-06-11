using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUi.Models;
using WpfUi.ViewModels;

namespace WpfUi.Helpers
{
	public class BookListFilter
	{
		private readonly BookCardViewModel _bookCardViewModel;
		private readonly bool _isShowReadBooks;
		private readonly string _bookFilterText = string.Empty;
		private readonly ObservableCollection<SelectableTagModel> _selectableTagModels;

		public BookListFilter(BookCardViewModel bookCardViewModel, bool isShowReadBooks, string bookFilterText, ObservableCollection<SelectableTagModel> selectableTagModels)
		{
			_bookCardViewModel = bookCardViewModel;
			_isShowReadBooks = isShowReadBooks;
			_bookFilterText = bookFilterText;
			_selectableTagModels = selectableTagModels;
		}

		public bool IsBookShown()
		{
			return IsBookShownBasedOnFilterText() && IsBookShownBasedOnTagSelection()
				&& IsBookShownBasedOnReadStatus();
		}
		private bool IsBookShownBasedOnFilterText()
		{
			return IsFilterTextInBookName() || IsFilterTextInAnyBookTag();
		}
		private bool IsFilterTextInBookName()
		{
			bool output = _bookCardViewModel.BookName.Contains(_bookFilterText, StringComparison.InvariantCultureIgnoreCase);
			return output;
		}
		private bool IsFilterTextInAnyBookTag()
		{
			bool output = _bookCardViewModel.Tags.Any(x => x.Contains(_bookFilterText, StringComparison.InvariantCultureIgnoreCase));
			return output;
		}
		private bool IsBookShownBasedOnTagSelection()
		{
			List<SelectableTagModel> selectedTags = GetSelectedTags();

			if (selectedTags.Count == 0)
			{
				return true;
			}
			else
			{
				return DoesBookHaveAllSelectedTags(selectedTags);
			}
		}
		private List<SelectableTagModel> GetSelectedTags()
		{
			return _selectableTagModels.Where(x => x.IsSelected).ToList();
		}
		private bool DoesBookHaveAllSelectedTags(List<SelectableTagModel> selectedTags)
		{
			ObservableCollection<string> bookTags = _bookCardViewModel.Tags;
			List<string> selectedTagNames = selectedTags.Select(x => x.Tag).ToList();

			bool doesBookHaveAllSelectedTags = selectedTagNames.All(tagName => bookTags.Contains(tagName));

			return doesBookHaveAllSelectedTags;
		}
		private bool IsBookShownBasedOnReadStatus()
		{
			bool isBookShown;
			if (_isShowReadBooks == true)
			{
				isBookShown = true;
			}
			else
			{
				if (_bookCardViewModel.IsRead)
				{
					isBookShown = false;
				}
				else
				{
					isBookShown = true;
				}
			}
			return isBookShown;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUi.Components;
using WpfUi.Models;
using WpfUi.ViewModels;

namespace WpfUi.Helpers
{
	public class SelectableTagListFromBookListGenerator : IDisposable
	{
		private readonly ObservableCollection<BookCardViewModel> _bookCards;
		private readonly BookListViewModel _bookListViewModel;
		private readonly ObservableCollection<SelectableTagModel> _distinctTagList = new();

		public SelectableTagListFromBookListGenerator(ObservableCollection<BookCardViewModel> bookCards, BookListViewModel bookListViewModel)
		{
			_bookCards = bookCards;
			_bookListViewModel = bookListViewModel;
		}

		public ObservableCollection<SelectableTagModel> GetSelectableTagModels()
		{
			
			foreach (BookCardViewModel bookCard in _bookCards)
			{
				AddBookTagsToDistinctTagList(bookCard);
			}

			return GetOrderedTagsByNumberOfBooksDecending();
		}
		private void AddBookTagsToDistinctTagList(BookCardViewModel bookCard)
		{
			foreach (string tag in bookCard.Tags)
			{
				if (IsTagInList(tag))
				{
					IncrementNumberOfBooksInTagInList(tag);
				}
				else
				{
					AddSelectableTagToDistinctTagList(tag);
				}
			}
		}
		private bool IsTagInList(string tag)
		{
			return _distinctTagList.Any(x => x.Tag == tag);
		}
		private void IncrementNumberOfBooksInTagInList(string tag)
		{
			_distinctTagList.First(x => x.Tag == tag).NumberOfBooksInTag += 1;
		}
		private void AddSelectableTagToDistinctTagList(string tag)
		{
			SelectableTagModel selectableTagModel = new();
			selectableTagModel.PropertyChanged += OnSelectedTagChanged;

			selectableTagModel.Tag = tag;
			selectableTagModel.NumberOfBooksInTag = 1;
			_distinctTagList.Add(selectableTagModel);
		}
		private void OnSelectedTagChanged(object? sender, PropertyChangedEventArgs e)
		{
			_bookListViewModel.RefreshBooksCollectionView();
		}
		private ObservableCollection<SelectableTagModel> GetOrderedTagsByNumberOfBooksDecending()
		{
			IOrderedEnumerable<SelectableTagModel>? orderdTags = _distinctTagList.OrderBy(x => x.Tag).OrderByDescending(x => x.NumberOfBooksInTag);
			ObservableCollection<SelectableTagModel> orderedObservableTags = new();
			foreach (SelectableTagModel tag in orderdTags)
			{
				orderedObservableTags.Add(tag);
			}

			return orderedObservableTags;
		}

		public void Dispose()
		{
			foreach (SelectableTagModel? item in _distinctTagList)
			{
				item.PropertyChanged -= OnSelectedTagChanged;
			}
			GC.SuppressFinalize(this);
		}
	}
}

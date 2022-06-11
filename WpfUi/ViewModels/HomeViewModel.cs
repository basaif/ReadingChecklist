using WpfUi.Models;
using WpfUi.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using FileSystemUtilities.Library;
using Models.Library;
using DomainLogic.Library.Creators;
using DomainLogic.Library.Services;
using DomainLogic.Library;
using WpfUi.ViewModels.Cmds;
using System.Windows.Input;

namespace WpfUi.ViewModels
{
	public class HomeViewModel : ViewModelBase
	{
		private readonly BookStore _booksStore;
		private readonly IFoldersFileNamePairs _foldersFileNamePairs;
		private readonly IBookTagStructureCreator _bookTagStructureCreator;

		public bool NotEnoughBooks
		{
			get
			{
				return _notEnoughBooks;
			}
			set
			{
				_notEnoughBooks = value;
				OnPropertyChanged(nameof(NotEnoughBooks));
				OnPropertyChanged(nameof(EnoughBooks));
			}
		}
		private bool _notEnoughBooks = true;

		public bool EnoughBooks
		{
			get
			{
				return !NotEnoughBooks;
			}
		}
		public GetBooksViewModel GetBooksViewModel
		{
			get; set;
		}

		public BookListViewModel BookListViewModel
		{
			get; set;
		}

		public ICommand LoadBooksCommand
		{
			get;
		}

		public HomeViewModel(BookStore booksStore,
			IFoldersFileNamePairs foldersFileNamePairs,
			IBookTagStructureCreator bookTagStructureCreator)
		{
			_booksStore = booksStore;
			_foldersFileNamePairs = foldersFileNamePairs;
			_bookTagStructureCreator = bookTagStructureCreator;

			GetBooksViewModel = new(_foldersFileNamePairs, _bookTagStructureCreator, _booksStore);
			BookListViewModel = new(_booksStore);

			_booksStore.BooksLoaded += OnBooksLoaded;

			LoadBooksCommand = new LoadBooksCommand(_booksStore);
		}

		private void OnBooksLoaded()
		{

			if (!_booksStore.Books.Any())
			{
				NotEnoughBooks = true;
			}
			else
			{
				NotEnoughBooks = false;
			}

		}

		public static HomeViewModel LoadViewModel(BookStore booksStore,
			IFoldersFileNamePairs foldersFileNamePairs,
			IBookTagStructureCreator bookTagStructureCreator)
		{
			HomeViewModel viewModel = new(booksStore,
			 foldersFileNamePairs,
			 bookTagStructureCreator);

			viewModel.LoadBooksCommand.Execute(null);

			return viewModel;
		}


	}
}

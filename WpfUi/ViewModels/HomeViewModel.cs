﻿using WpfUi.Models;
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

namespace WpfUi.ViewModels
{
	public class HomeViewModel : ViewModelBase
	{
		private readonly IBookDataService _bookDataService;
		private readonly BookStore _booksStore;
		private readonly IFoldersFileNamePairs _foldersFileNamePairs;
		private readonly IBookTagStructureCreator _bookTagStructureCreator;
		private readonly IBooksUpdater _booksUpdater;

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
		private bool _notEnoughBooks;

		public bool EnoughBooks
		{
			get
			{
				return !NotEnoughBooks;
			}
		}
		public GetBooksViewModel GetBooksViewModel { get; set; }

		public BookListViewModel BookListViewModel { get; set; }

		public HomeViewModel(IBookDataService bookDataService, BookStore booksStore,
			IFoldersFileNamePairs foldersFileNamePairs,
			IBookTagStructureCreator bookTagStructureCreator,
			IBooksUpdater booksUpdater)
		{
			_bookDataService = bookDataService;
			_booksStore = booksStore;
			_foldersFileNamePairs = foldersFileNamePairs;
			_bookTagStructureCreator = bookTagStructureCreator;
			_booksUpdater = booksUpdater;

			GetBooksViewModel = new(this, _foldersFileNamePairs, _bookTagStructureCreator);
			BookListViewModel = new(_bookDataService, _booksStore, _booksUpdater);

			LoadHomeViewModel();
		}

		public void LoadHomeViewModel()
		{
			_booksStore.LoadBooksAsync().ContinueWith(task =>
			{
				if (task.Exception is null)
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
			});
		}
	}
}

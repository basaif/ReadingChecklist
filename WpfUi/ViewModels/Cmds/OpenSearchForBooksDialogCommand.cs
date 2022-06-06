﻿using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUi.ViewModels.Cmds
{
	public class OpenSearchForBooksDialogCommand : CommandBase
	{
		private readonly GetBooksViewModel _getBooksViewModel;

		public OpenSearchForBooksDialogCommand(GetBooksViewModel getBooksViewModel)
		{
			_getBooksViewModel = getBooksViewModel;
		}

		public override bool CanExecute(object? parameter)
		{

			return !_getBooksViewModel.IsGettingBooks;

		}

		public override void Execute(object? parameter)
		{
			_getBooksViewModel.LocationToGetBooks = ShowFolderBrowserDialog("Select folder tol look for books in");
			_getBooksViewModel.SetGenterateBookDataCommand(_getBooksViewModel.LocationToGetBooks);

		}

		private string ShowFolderBrowserDialog(string description)
		{
			VistaFolderBrowserDialog dialog = new();
			dialog.Description = description;
			dialog.UseDescriptionForTitle = true;

			string selectedFolder = "";

			if ((bool)dialog.ShowDialog(System.Windows.Application.Current.MainWindow))
			{
				selectedFolder = dialog.SelectedPath;
			}

			return selectedFolder;
		}
	}
}

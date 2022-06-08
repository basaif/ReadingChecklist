using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

		private static string ShowFolderBrowserDialog(string description)
		{
			VistaFolderBrowserDialog dialog = new()
			{
				Description = description,
				UseDescriptionForTitle = true
				
			};

			string selectedFolder = string.Empty;

			if (dialog.ShowDialog(Application.Current.MainWindow) is bool hasUserSelectedFolder)
			{
				if (hasUserSelectedFolder)
				{
					selectedFolder = dialog.SelectedPath;
				}
			}

			return selectedFolder;
		}
	}
}

using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels.Cmds
{
    public class OpenSearchForBooksDialogCommand : CommandBase
    {
        private readonly NoBooksViewModel? _noBooksViewModel;
        private readonly RefreshBooksViewModel? _refreshBooksViewModel;

        public OpenSearchForBooksDialogCommand(NoBooksViewModel noBooksViewModel)
        {
            _noBooksViewModel = noBooksViewModel;
        }

        public OpenSearchForBooksDialogCommand(RefreshBooksViewModel refreshBooksViewModel)
        {
            _refreshBooksViewModel = refreshBooksViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            if (_noBooksViewModel is not null)
            {
                return !_noBooksViewModel.IsGettingBooks;
            }
            else if (_refreshBooksViewModel is not null)
            {
                return !_refreshBooksViewModel.IsRefreshingBooks;
            }
            return false;
            
        }

        public override void Execute(object? parameter)
        {
            if (_noBooksViewModel is not null)
            {
                _noBooksViewModel.LocationToGetBooks = ShowFolderBrowserDialog("Select folder tol look for books in");
                _noBooksViewModel.SetGenterateBookDataCommand(_noBooksViewModel.LocationToGetBooks);
            }
            else if (_refreshBooksViewModel is not null)
            {
                _refreshBooksViewModel.LocationToGetBooks = ShowFolderBrowserDialog("Select folder tol look for books in");
                _refreshBooksViewModel.SetRefreshBookDataCommand(_refreshBooksViewModel.LocationToGetBooks);
            }
           
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

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
        private readonly NoBooksViewModel _noBooksViewModel;

        public OpenSearchForBooksDialogCommand(NoBooksViewModel noBooksViewModel)
        {
            _noBooksViewModel = noBooksViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return !_noBooksViewModel.IsGettingBooks; 
        }

        public override void Execute(object? parameter)
        {
            _noBooksViewModel.LocationToGetBooks = ShowFolderBrowserDialog("Select folder tol look for books in");
            _noBooksViewModel.SetGenterateBookDataCommand(_noBooksViewModel.LocationToGetBooks);
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

using ReadingChecklistLogicLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels.Cmds
{
    public class GenterateBookDataCommand : CommandBase
    {
        private readonly NoBooksViewModel _noBooksViewModel;
        private readonly BookDataGenerator _bookDataGenerator;
        private readonly HomeViewModel _homeViewModel;

        public GenterateBookDataCommand(NoBooksViewModel noBooksViewModel,
            BookDataGenerator bookDataGenerator,
            HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
            _noBooksViewModel = noBooksViewModel;
            _bookDataGenerator = bookDataGenerator;
        }

        public override bool CanExecute(object? parameter)
        {
            if(!(string.IsNullOrEmpty(_noBooksViewModel.LocationToGetBooks)
                || string.IsNullOrWhiteSpace(_noBooksViewModel.LocationToGetBooks))
                && !_noBooksViewModel.IsGettingBooks)
            {
                return true;
            }
            return false;
        }

        public async override void Execute(object? parameter)
        {
            _noBooksViewModel.IsGettingBooks = true;
            await GetBooksAsync();
            _homeViewModel.AddBooks();
            _homeViewModel.CalculateNumbers();
            _noBooksViewModel.IsGettingBooks = false;
            _noBooksViewModel.LocationToGetBooks = "";
        }

        private async Task GetBooksAsync()
        {
            await Task.Run(() => _bookDataGenerator.GenerateBooksData());
            
        }
    }
}

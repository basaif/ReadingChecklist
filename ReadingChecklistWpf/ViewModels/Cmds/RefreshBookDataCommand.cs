using ReadingChecklistLogicLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels.Cmds
{
    public class RefreshBookDataCommand : CommandBase
    {
        private readonly RefreshBooksViewModel _refreshBooksViewModel;
        private readonly BooksDataRefresher _booksDataRefresher;
        private readonly HomeViewModel _homeViewModel;

        public RefreshBookDataCommand(RefreshBooksViewModel refreshBooksViewModel,
            BooksDataRefresher booksDataRefresher,
            HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
            _refreshBooksViewModel = refreshBooksViewModel;
            _booksDataRefresher = booksDataRefresher;
        }

        public override bool CanExecute(object? parameter)
        {
            if (!(string.IsNullOrEmpty(_refreshBooksViewModel.LocationToGetBooks)
                || string.IsNullOrWhiteSpace(_refreshBooksViewModel.LocationToGetBooks))
                && !_refreshBooksViewModel.IsRefreshingBooks)
            {
                return true;
            }
            return false;
        }

        public async override void Execute(object? parameter)
        {
            _refreshBooksViewModel.IsRefreshingBooks = true;
            await GetBooksAsync();
            _homeViewModel.RefreshBooks();
            _homeViewModel.CalculateNumbers();
            _refreshBooksViewModel.IsRefreshingBooks = false;
            _refreshBooksViewModel.LocationToGetBooks = "";
        }

        private async Task GetBooksAsync()
        {
            await Task.Run(() => _booksDataRefresher.RefreshBooksData());

        }
    }
}

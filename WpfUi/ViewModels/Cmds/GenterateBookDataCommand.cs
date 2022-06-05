﻿using DomainLogic.Library;
using System.Threading.Tasks;

namespace WpfUi.ViewModels.Cmds
{
	public class GenterateBookDataCommand : CommandBase
    {
        private readonly NoBooksViewModel _noBooksViewModel;
        private readonly IBooksDataRefresher _bookDataRefresher;
		private readonly HomeViewModel _homeViewModel;

        public GenterateBookDataCommand(NoBooksViewModel noBooksViewModel,
            IBooksDataRefresher bookDataRefresher,
            HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
            _noBooksViewModel = noBooksViewModel;
            _bookDataRefresher = bookDataRefresher;
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
            _homeViewModel.LoadBooksData();
            _noBooksViewModel.IsGettingBooks = false;
            _noBooksViewModel.LocationToGetBooks = "";
        }

        private async Task GetBooksAsync()
        {
			await Task.Run(() => _bookDataRefresher.RefreshBooksData());
		}
    }
}

using DomainLogic.Library.Creators;
using System.Threading.Tasks;
using WpfUi.Stores;

namespace WpfUi.ViewModels.Cmds
{
	public class GetBookDataCommand : CommandBase
    {
        private readonly GetBooksViewModel _getBooksViewModel;
        private readonly IBookTagStructureCreator _bookTagStructureCreator;
		private readonly BookStore _bookStore;

		public GetBookDataCommand(GetBooksViewModel getBooksViewModel,
            IBookTagStructureCreator bookTagStructureCreator,
            BookStore bookStore)
        {
            _getBooksViewModel = getBooksViewModel;
            _bookTagStructureCreator = bookTagStructureCreator;
			_bookStore = bookStore;
		}

        public override bool CanExecute(object? parameter)
        {
            if (!(string.IsNullOrEmpty(_getBooksViewModel.LocationToGetBooks)
                || string.IsNullOrWhiteSpace(_getBooksViewModel.LocationToGetBooks))
                && !_getBooksViewModel.IsGettingBooks)
            {
                return true;
            }
            return false;
        }

        public override async void Execute(object? parameter)
        {
            _getBooksViewModel.IsGettingBooks = true;
            await GetBooksAsync();
            _getBooksViewModel.IsGettingBooks = false;
            _getBooksViewModel.LocationToGetBooks = "";
        }

        private async Task GetBooksAsync()
        {
            await Task.Run(() => _bookTagStructureCreator.LoadBookData());
			await _bookStore.RefreshBooksAsync();

        }
    }
}

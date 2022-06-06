using DomainLogic.Library.Creators;
using System.Threading.Tasks;

namespace WpfUi.ViewModels.Cmds
{
	public class GetBookDataCommand : CommandBase
    {
        private readonly GetBooksViewModel _getBooksViewModel;
        private readonly IBookTagStructureCreator _bookTagStructureCreator;
        private readonly HomeViewModel _homeViewModel;

        public GetBookDataCommand(GetBooksViewModel getBooksViewModel,
            IBookTagStructureCreator bookTagStructureCreator,
            HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
            _getBooksViewModel = getBooksViewModel;
            _bookTagStructureCreator = bookTagStructureCreator;
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

        public async override void Execute(object? parameter)
        {
            _getBooksViewModel.IsGettingBooks = true;
            await GetBooksAsync();
            _homeViewModel.LoadBookList();
            _getBooksViewModel.IsGettingBooks = false;
            _getBooksViewModel.LocationToGetBooks = "";
        }

        private async Task GetBooksAsync()
        {
            await Task.Run(() => _bookTagStructureCreator.LoadBookData());

        }
    }
}

using DomainLogic.Library;
using DomainLogic.Library.Creators;
using DomainLogic.Library.Services;
using FileSystemUtilities.Library;
using WpfUi.Stores;

namespace WpfUi.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
    {

        private ViewModelBase _homeViewModel;

        public ViewModelBase HomeViewModel
        {
            get { return _homeViewModel; }
            set { _homeViewModel = value; }
        }

		public MainWindowViewModel(HomeViewModel homeViewModel)
		{
			_homeViewModel = homeViewModel;
		}

    }
}

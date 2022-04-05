using ReadingChecklistLogicLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private ViewModelBase _homeViewModel;

        public ViewModelBase HomeViewModel
        {
            get { return _homeViewModel; }
            set { _homeViewModel = value; }
        }

        public MainWindowViewModel(BookDataGetter bookDataGetter)
        {
            _homeViewModel = new HomeViewModel(bookDataGetter);
        }

    }
}

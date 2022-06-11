using WpfUi.Models;
using WpfUi.ViewModels.Cmds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfUi.ViewModels
{
    public class TagListViewModel : ViewModelBase
    {
		private ObservableCollection<SelectableTagModel> _selectableTags;

        public ObservableCollection<SelectableTagModel> SelectableTags
        {
            get
            {
                if (_selectableTags == null)
                {
                    _selectableTags = new ObservableCollection<SelectableTagModel>();
                }
                return _selectableTags;
            }

            set
            {
                if (value is not null)
                {
                    _selectableTags = value;
                }
                else
                {
                    _selectableTags = new ObservableCollection<SelectableTagModel>();
                }
                OnPropertyChanged(nameof(SelectableTags));
            }
        }

        public ICommand ClearSelectedTagsCommand { get; }

        public TagListViewModel()
        {
            if (_selectableTags == null)
            {
                _selectableTags = new ObservableCollection<SelectableTagModel>();
            }
            ClearSelectedTagsCommand = new ClearSelectedTagsCommand(this);
        }
    }
}

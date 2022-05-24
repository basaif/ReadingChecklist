using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.Models
{
    public class SelectableTagModel : NotifyModel
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                //OnIsSelectedChanged();
            }
        }
        public string Tag { get; set; } = string.Empty;
        public int NumberOfBooksInTag { get; set; }

        //public event EventHandler? IsSelectedChanged;

        //public void OnIsSelectedChanged()
        //{
        //    IsSelectedChanged?.Invoke(this, new EventArgs());
        //}
    }
}

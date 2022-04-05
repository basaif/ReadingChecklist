using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels
{
    public class BookCardViewModel : ViewModelBase
    {
        private string _bookName = "";

        public string BookName
        {
            get { return _bookName; }
            set { _bookName = value;
                OnPropertyChanged(nameof(BookName));
            }
        }

        private bool _isRead;

        public bool IsRead
        {
            get { return _isRead; }
            set { _isRead = value;
                OnPropertyChanged(nameof(IsRead));
            }
        }

        private DateTime _dateRead;

        public DateTime DateRead
        {
            get { return _dateRead; }
            set { _dateRead = value;
                OnPropertyChanged(nameof(DateRead));
            }
        }

        ObservableCollection<string> _tags = new();
        public ObservableCollection<string> Tags
        {
            get
            {
                return _tags;
            }
        }

        public BookCardViewModel(BookModel book)
        {
            CreateBookCard(book);
        }

        private void CreateBookCard(BookModel book)
        {
            foreach (TagModel tag in book.Tags)
            {
                Tags.Add(tag.TagName);
            }
            BookName = book.BookName;
            IsRead = book.IsRead;
            DateRead = book.DateRead;
        }


    }
}

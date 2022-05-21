using ReadingChecklistModels;
using ReadingChecklistWpf.Stores;
using ReadingChecklistWpf.ViewModels.Cmds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private readonly BooksStore _booksStore;

        public ObservableCollection<string> Tags
        {
            get
            {
                return _tags;
            }
        }

        public BookModel Book { get; set; }

        public ICommand ChangeIsReadCommand { get; }

        public BookCardViewModel(BookModel book, BooksStore booksStore)
        {
            Book = book;
            _booksStore = booksStore;
            CreateBookCard(book);
            ChangeIsReadCommand = new ChangeIsReadCommand(this, _booksStore);
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

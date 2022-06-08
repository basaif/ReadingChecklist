using WpfUi.Stores;
using WpfUi.ViewModels.Cmds;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Models.Library;
using DomainLogic.Library;

namespace WpfUi.ViewModels
{
	public class BookCardViewModel : ViewModelBase
    {
        private string _bookName = string.Empty;

        public string BookName
        {
            get { return _bookName; }
            set
            {
                _bookName = value;
                OnPropertyChanged(nameof(BookName));
            }
        }

        private bool _isRead;

        public bool IsRead
        {
            get { return _isRead; }
            set
            {
                _isRead = value;
                OnPropertyChanged(nameof(IsRead));
            }
        }

        private DateTime _dateRead;

        public DateTime DateRead
        {
            get { return _dateRead; }
            set
            {
                _dateRead = value;
                OnPropertyChanged(nameof(DateRead));
            }
        }

		private readonly ObservableCollection<string> _tags = new();
        private readonly BooksStore _booksStore;

        public ObservableCollection<string> Tags
        {
            get
            {
                return _tags;
            }
        }

        public BookModel Book { get; set; }

        private string _highlightText = string.Empty;

        public string HighlightText
        {
            get { return _highlightText; }
            set
            {
                _highlightText = value;
                OnPropertyChanged(nameof(HighlightText));
            }
        }

        public ICommand ChangeIsReadCommand { get; }

        public BookCardViewModel(BookModel book, BooksStore booksStore, IBooksUpdater booksUpdater)
        {
            Book = book;
            _booksStore = booksStore;
            CreateBookCard(book);
            ChangeIsReadCommand = new ChangeIsReadCommand(this, _booksStore, booksUpdater);
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

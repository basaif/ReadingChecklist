using ReadingChecklistLogicLibrary;
using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly BookDataGetter _bookDataGetter;

        private bool _notEnoughBooks;

        public bool NotEnoughBooks
        {
            get { return _notEnoughBooks; }
            set
            {
                _notEnoughBooks = value;
                OnPropertyChanged(nameof(NotEnoughBooks));
            }
        }

        private NoBooksViewModel _noBooks;

        public NoBooksViewModel NoBooks
        {
            get { return _noBooks; }
            set { _noBooks = value; }
        }

        ObservableCollection<BookCardViewModel> _bookCards;
        public ObservableCollection<BookCardViewModel> BookCards
        {
            get
            {
                if (_bookCards == null)
                {
                    _bookCards = new ObservableCollection<BookCardViewModel>();
                }
                return _bookCards;
            }
        }


        public HomeViewModel(BookDataGetter bookDataGetter)
        {
            _noBooks = new NoBooksViewModel(this);

            if (_bookCards == null)
            {
                _bookCards = new ObservableCollection<BookCardViewModel>();
            }

            _bookDataGetter = bookDataGetter;
            AddBooks();


        }

        public void AddBooks()
        {
            List<BookModel> books = _bookDataGetter.GetAllBooks();

            if (books.Count == 0)
            {
                NotEnoughBooks = true;
            }

            else
            {
                NotEnoughBooks = false;
                foreach (BookModel book in books)
                {
                    BookCards.Add(new BookCardViewModel(book));
                }
            }
        }


    }
}

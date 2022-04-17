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

        private int _numberOfBooks;

        public int NumberOfBooks
        {
            get { return _numberOfBooks; }
            set
            {
                _numberOfBooks = value;
                OnPropertyChanged(nameof(NumberOfBooks));
            }
        }

        private int _numberOfReadBooks;

        public int NumberOfReadBooks
        {
            get { return _numberOfReadBooks; }
            set
            {
                _numberOfReadBooks = value;
                OnPropertyChanged(nameof(NumberOfReadBooks));
            }
        }

        private int _percentageOfReadBooks;

        public int PercentageOfReadBooks
        {
            get { return _percentageOfReadBooks; }
            set
            {
                _percentageOfReadBooks = value;
                OnPropertyChanged(nameof(PercentageOfReadBooks));
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

            CalculateNumbers();


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

        public void CalculateNumbers()
        {
            NumberOfBooks = _bookCards.Count;
            NumberOfReadBooks = _bookCards.Where(book => book.IsRead == true).Count();
            PercentageOfReadBooks = CalculatePercentage(NumberOfBooks, NumberOfReadBooks);
        }

        public int CalculatePercentage(int total, int part)
        {
            if (total == 0)
            {
                return 0;
            }
            return part * 100 / total ;
        }

    }
}

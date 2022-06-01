using ReadingChecklistDataAccess;
using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistLogicLibrary
{
    public class BooksUpdater
    {
        private readonly BookModel _book;

        public BooksUpdater(BookModel book)
        {
            _book = book;
        }

        public void ChangeReadStatus(bool isRead, DateTime date)
        {
            _book.IsRead = isRead;
            _book.DateRead = date;

            UpdateBook();
        }

        private void UpdateBook()
        {
            SqliteUpdater.UpdateBook(_book);
        }

    }
}

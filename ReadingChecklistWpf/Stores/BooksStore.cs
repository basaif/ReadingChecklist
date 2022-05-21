using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.Stores
{
    public class BooksStore
    {
        public event Action<BookModel>? BookUpdated;

        public void UpdateBook(BookModel book)
        {
            BookUpdated?.Invoke(book);
        }
    }
}

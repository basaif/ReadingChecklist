using Models.Library;
using System;

namespace WpfUi.Stores
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

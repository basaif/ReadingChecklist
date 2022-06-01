using ReadingChecklistDataAccess;
using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistLogicLibrary
{
    public class BookDataGetter
    {
        public List<BookModel> GetAllBooks()
        {
            return SqliteReader.ReadAllBooks();
        }

    }
}

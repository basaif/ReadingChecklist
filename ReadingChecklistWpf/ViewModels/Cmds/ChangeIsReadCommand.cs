using ReadingChecklistLogicLibrary;
using ReadingChecklistWpf.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels.Cmds
{
    public class ChangeIsReadCommand : CommandBase
    {
        private readonly BookCardViewModel _bookCardViewModel;
        private readonly BooksStore _booksStore;

        public ChangeIsReadCommand(BookCardViewModel bookCardViewModel, BooksStore booksStore)
        {
            _bookCardViewModel = bookCardViewModel;
            _booksStore = booksStore;
        }
        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            BooksUpdater booksUpdater = new(_bookCardViewModel.Book);

            booksUpdater.ChangeReadStatus(_bookCardViewModel.IsRead, _bookCardViewModel.DateRead);

            _booksStore.UpdateBook(_bookCardViewModel.Book);
        }
    }
}

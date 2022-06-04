using WpfUi.Stores;
using System;
using DomainLogic.Library;

namespace WpfUi.ViewModels.Cmds
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

            _bookCardViewModel.DateRead = DateTime.UtcNow;

            booksUpdater.ChangeReadStatus(_bookCardViewModel.IsRead, _bookCardViewModel.DateRead);

            _booksStore.UpdateBook(_bookCardViewModel.Book);
        }
    }
}

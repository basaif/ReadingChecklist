using WpfUi.Stores;
using System;
using DomainLogic.Library;

namespace WpfUi.ViewModels.Cmds
{
	public class ChangeIsReadCommand : CommandBase
    {
        private readonly BookCardViewModel _bookCardViewModel;
        private readonly BookStore _booksStore;

		public ChangeIsReadCommand(BookCardViewModel bookCardViewModel, BookStore booksStore)
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
            _bookCardViewModel.DateRead = DateTime.UtcNow;

            _booksStore.UpdateBookReadStatus(_bookCardViewModel.Book, _bookCardViewModel.IsRead, _bookCardViewModel.DateRead);
        }
    }
}

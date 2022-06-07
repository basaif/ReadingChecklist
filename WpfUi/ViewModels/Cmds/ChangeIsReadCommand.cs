using WpfUi.Stores;
using System;
using DomainLogic.Library;

namespace WpfUi.ViewModels.Cmds
{
	public class ChangeIsReadCommand : CommandBase
    {
        private readonly BookCardViewModel _bookCardViewModel;
        private readonly BooksStore _booksStore;
		private readonly IBooksUpdater _booksUpdater;

		public ChangeIsReadCommand(BookCardViewModel bookCardViewModel, BooksStore booksStore, IBooksUpdater booksUpdater)
        {
            _bookCardViewModel = bookCardViewModel;
            _booksStore = booksStore;
			_booksUpdater = booksUpdater;
		}
        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            _bookCardViewModel.DateRead = DateTime.UtcNow;

            _booksUpdater.ChangeReadStatus(_bookCardViewModel.Book, _bookCardViewModel.IsRead, _bookCardViewModel.DateRead);

            _booksStore.UpdateBook(_bookCardViewModel.Book);
        }
    }
}

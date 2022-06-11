using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUi.Stores;

namespace WpfUi.ViewModels.Cmds
{
	public class LoadBooksCommand : AsyncCommandBase
	{
		private readonly BookStore _bookStore;

		public LoadBooksCommand(BookStore bookStore)
		{
			_bookStore = bookStore;
		}

		protected override async Task ExecuteAsync(object? parameter)
		{
			await _bookStore.LoadBooksAsync();
		}
	}
}

using ReadingChecklistLogicLibrary;
using ReadingChecklistWpf.Stores;
using ReadingChecklistWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static Dapper.SqlMapper;

namespace ReadingChecklistWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            BookDataGetter bookDataGetter = new();
            BooksStore booksStore = new();

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(bookDataGetter, booksStore)
            };

            MainWindow.Show();
        }
    }
}


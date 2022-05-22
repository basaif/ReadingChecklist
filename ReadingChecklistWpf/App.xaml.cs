using FileManagementLibrary;
using ReadingChecklistLogicLibrary;
using ReadingChecklistWpf.Stores;
using ReadingChecklistWpf.ViewModels;
using System.Windows;

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
            FilesManager filesManager = new("");
            TagsCreator tagsCreator = new();
            BookDataGenerator bookDataGenerator = new(filesManager, tagsCreator);
            BooksDataRefresher booksDataRefresher = new(filesManager, tagsCreator);

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(bookDataGetter, booksStore, filesManager, bookDataGenerator, booksDataRefresher)
            };

            MainWindow.Show();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileManagementLibrary;
using Ookii.Dialogs.Wpf;
using ReadingChecklistDataAccess;
using ReadingChecklistLogicLibrary;
using ReadingChecklistModels;

namespace ReadingChecklistWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new();
        private static readonly FilesManager _filesManager = new("");
        private static readonly TagsCreator _tagsCreator = new();
        private readonly BookDataGenerator _bookDataGenerator = new(_filesManager, _tagsCreator); 

        public MainWindow()
        {
            InitializeComponent();
        }

        private string ShowFolderBrowserDialog(string description)
        {
            VistaFolderBrowserDialog dialog = new();
            dialog.Description = description;
            dialog.UseDescriptionForTitle = true;

            string selectedFolder = "";

            if ((bool)dialog.ShowDialog(this))
            {
                selectedFolder = dialog.SelectedPath;
            }

            return selectedFolder;
        }

        private void GetBooksBtn_Click(object sender, RoutedEventArgs e)
        {
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                BooksProgress.Visibility = Visibility.Hidden;

                //Parallel.ForEach(_booksCreater.AllBooks, book => CreateBookCard(book)); 

                foreach(BookModel book in _bookDataGenerator.AllBooks)
                {
                    CreateBookCard(book);
                }

            });
        }

        private void CreateBookCard(BookModel book)
        {
            BookCardUc bookCardUc = new();
            ObservableCollection<string> tags = new();
            foreach (TagModel tag in book.Tags)
            {
                tags.Add(tag.TagName);
            }
            bookCardUc.Tags = tags;
            bookCardUc.BookName = book.BookName;
            bookCardUc.IsRead = book.IsRead;
            bookCardUc.DateRead = book.DateRead;

            BooksPanel.Children.Add(bookCardUc);
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                BooksProgress.Visibility = Visibility.Visible;
            });
            GetBooksMethod();
        }

        private void GetBooksMethod()
        {
            _bookDataGenerator.GenerateBooksData();
        }

        private void LookForBooksBtn_Click(object sender, RoutedEventArgs e)
        {
            WhereToGetBooks.Text = ShowFolderBrowserDialog("Select folder tol look for books in");
        }

        private void WhereToGetBooks_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filesManager.Location = WhereToGetBooks.Text;
        }
    }
}

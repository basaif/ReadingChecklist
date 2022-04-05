using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ReadingChecklistWpf.Components
{
    /// <summary>
    /// Interaction logic for BookCardUc.xaml
    /// </summary>
    public partial class BookCardUc : UserControl
    {
        public string BookName
        {
            get { return (string)GetValue(BookNameProperty); }
            set { SetValue(BookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BookNameProperty =
            DependencyProperty.Register("BookName", typeof(string), typeof(BookCardUc), new PropertyMetadata("Book Name"));



        public DateTime DateRead
        {
            get { return (DateTime)GetValue(DateReadProperty); }
            set { SetValue(DateReadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateRead.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateReadProperty =
            DependencyProperty.Register("DateRead", typeof(DateTime), typeof(BookCardUc), new PropertyMetadata(DateTime.UtcNow));



        public bool IsRead
        {
            get { return (bool)GetValue(IsReadProperty); }
            set { SetValue(IsReadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRead.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadProperty =
            DependencyProperty.Register("IsRead", typeof(bool), typeof(BookCardUc), new PropertyMetadata(false));



        public ObservableCollection<string> Tags
        {
            get { return (ObservableCollection<string>)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tags.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register("Tags", typeof(ObservableCollection<string>), typeof(BookCardUc), new PropertyMetadata(new ObservableCollection<string>() { "Tag", "Another tage"}));



        public BookCardUc()
        {
            InitializeComponent();
        }
    }
}

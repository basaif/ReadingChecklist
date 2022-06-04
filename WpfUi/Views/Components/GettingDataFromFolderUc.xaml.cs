using System;
using System.Collections.Generic;
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

namespace WpfUi.Views.Components
{
    /// <summary>
    /// Interaction logic for GettingDataFromFolderUc.xaml
    /// </summary>
    public partial class GettingDataFromFolderUc : UserControl
    {


        public string Location
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(GettingDataFromFolderUc), new PropertyMetadata(""));



        public ICommand OpenSearchDialogCommand
        {
            get { return (ICommand)GetValue(OpenSearchDialogCommandProperty); }
            set { SetValue(OpenSearchDialogCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenSearchDialogCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenSearchDialogCommandProperty =
            DependencyProperty.Register("OpenSearchDialogCommand", typeof(ICommand), typeof(GettingDataFromFolderUc), new UIPropertyMetadata());



        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(GettingDataFromFolderUc), new PropertyMetadata("Search"));



        public ICommand GetDataCommand
        {
            get { return (ICommand)GetValue(GetDataCommandProperty); }
            set { SetValue(GetDataCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GetDataCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GetDataCommandProperty =
            DependencyProperty.Register("GetDataCommand", typeof(ICommand), typeof(GettingDataFromFolderUc), new UIPropertyMetadata());



        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(GettingDataFromFolderUc), new PropertyMetadata(false));


        public GettingDataFromFolderUc()
        {
            InitializeComponent();
        }
    }
}

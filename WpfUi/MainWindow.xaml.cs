using System.Windows;
using System.Windows.Input;

namespace WpfUi
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        public MainWindow(object dataContext)
        {
            InitializeComponent();
			DataContext = dataContext;
        }

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}

		}
	}
}

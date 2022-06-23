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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfUi.Views.Components
{
	/// <summary>
	/// Interaction logic for WindowTitleBar.xaml
	/// </summary>
	/// 
	[ContentProperty("WindowIcon")]
	public partial class WindowTitleBar : UserControl
	{


		public bool IsResizable
		{
			get
			{
				return (bool)GetValue(IsResizableProperty);
			}
			set
			{
				SetValue(IsResizableProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for IsResizable.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsResizableProperty =
			DependencyProperty.Register("IsResizable", typeof(bool), typeof(WindowTitleBar), new PropertyMetadata(true));



		public bool IsMinimizable
		{
			get
			{
				return (bool)GetValue(IsMinimizableProperty);
			}
			set
			{
				SetValue(IsMinimizableProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for IsMinimizable.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsMinimizableProperty =
			DependencyProperty.Register("IsMinimizable", typeof(bool), typeof(WindowTitleBar), new PropertyMetadata(true));



		public FontFamily TitleFontFamily
		{
			get
			{
				return (FontFamily)GetValue(TitleFontFamilyProperty);
			}
			set
			{
				SetValue(TitleFontFamilyProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for TitleFontFamily.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleFontFamilyProperty =
			DependencyProperty.Register("TitleFontFamily", typeof(FontFamily), typeof(WindowTitleBar),
				new PropertyMetadata(new FontFamily("Arial")));



		public double TitleFontSize
		{
			get
			{
				return (double)GetValue(TitleFontSizeProperty);
			}
			set
			{
				SetValue(TitleFontSizeProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for TitleFontSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleFontSizeProperty =
			DependencyProperty.Register("TitleFontSize", typeof(double), typeof(WindowTitleBar),
				new PropertyMetadata(0.0));



		public string WindowTitle
		{
			get
			{
				return (string)GetValue(WindowTitleProperty);
			}
			set
			{
				SetValue(WindowTitleProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for WindowTitle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WindowTitleProperty =
			DependencyProperty.Register("WindowTitle", typeof(string), typeof(WindowTitleBar),
				new PropertyMetadata("Window Title Bar"));



		public FrameworkElement WindowIcon
		{
			get
			{
				return (FrameworkElement)GetValue(WindowIconProperty);
			}
			set
			{
				SetValue(WindowIconProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for WindowIcon.  This enables animation, styling, binding, etc...
		//public static readonly DependencyProperty WindowIconProperty =
		//    DependencyProperty.Register("WindowIcon", typeof(FrameworkElement), typeof(WindowTitleBar),
		//         new PropertyMetadata(Application.Current.FindResource("DefaultIcon")));

		public static readonly DependencyProperty WindowIconProperty =
			DependencyProperty.Register("WindowIcon", typeof(FrameworkElement), typeof(WindowTitleBar), new UIPropertyMetadata(null));



		public bool CanCloseApp
		{
			get
			{
				return (bool)GetValue(CanCloseAppProperty);
			}
			set
			{
				SetValue(CanCloseAppProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for CanCloseApp.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CanCloseAppProperty =
			DependencyProperty.Register("CanCloseApp", typeof(bool), typeof(WindowTitleBar), new PropertyMetadata(true));

		public WindowTitleBar()
		{
			InitializeComponent();
		}

		private void CloseBtn_Click(object sender, RoutedEventArgs e)
		{
			Window window = Window.GetWindow(this);

			window.Close();

			Keyboard.ClearFocus();

			if (CanCloseApp)
			{
				Application.Current.Shutdown();
			}

		}

		private void ResizeBtn_Click(object sender, RoutedEventArgs e)
		{
			Window window = Window.GetWindow(this);

			if (window.WindowState == WindowState.Maximized)
			{
				window.WindowState = WindowState.Normal;
				ResizeIcon.Icon = FontAwesome5.EFontAwesomeIcon.Regular_WindowMaximize;
			}

			else
			{
				window.WindowState = WindowState.Maximized;
				ResizeIcon.Icon = FontAwesome5.EFontAwesomeIcon.Regular_WindowRestore;

			}

			Keyboard.ClearFocus();
		}

		private void MiniBtn_Click(object sender, RoutedEventArgs e)
		{
			Window window = Window.GetWindow(this);
			// Minimize
			window.WindowState = WindowState.Minimized;
			Keyboard.ClearFocus();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			Window window = Window.GetWindow(this);

			ResizeIcon.Icon = window.WindowState == WindowState.Maximized
				? FontAwesome5.EFontAwesomeIcon.Regular_WindowRestore
				: FontAwesome5.EFontAwesomeIcon.Regular_WindowMaximize;

			Keyboard.ClearFocus();

		}
	}
}

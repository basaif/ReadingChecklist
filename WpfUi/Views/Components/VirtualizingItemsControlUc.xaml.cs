using System;
using System.Collections;
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
using WpfUi.ViewModels;

namespace WpfUi.Views.Components
{
	/// <summary>
	/// Interaction logic for VirtualizingItemsControlUc.xaml
	/// </summary>
	public partial class VirtualizingItemsControlUc : UserControl
	{


		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register(
				nameof(ItemsSource), typeof(IEnumerable), typeof(VirtualizingItemsControlUc));


		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(ItemTemplateProperty);
			}
			set
			{
				SetValue(ItemTemplateProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(VirtualizingItemsControlUc));


		public VirtualizingItemsControlUc()
		{
			InitializeComponent();
		}
	}
}

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
using System.Windows.Shapes;

namespace NavigationApp.Infra
{
	/// <summary>
	/// Interaction logic for SimpleTabWindow.xaml
	/// </summary>
	public partial class SimpleTabWindow : Window
	{
		private InterTabClient _tabClient;

		public SimpleTabWindow()
		{
			InitializeComponent();
			DataContext = this;
			Views = new ObservableCollection<object>();
		}

		public ObservableCollection<object> Views
		{
			get { return (ObservableCollection<object>)GetValue(ViewsProperty); }
			set { SetValue(ViewsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Views.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ViewsProperty =
				DependencyProperty.Register("Views", typeof(ObservableCollection<object>), typeof(SimpleTabWindow), new PropertyMetadata(null));

		public Guid Group
		{
			get { return (Guid)GetValue(GroupProperty); }
			set { SetValue(GroupProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Group.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty GroupProperty =
				DependencyProperty.Register("Group", typeof(Guid), typeof(SimpleTabWindow), new PropertyMetadata(null));

		public object CurrentView
		{
			get { return (object)GetValue(CurrentViewProperty); }
			set { SetValue(CurrentViewProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CurrentView.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurrentViewProperty =
				DependencyProperty.Register("CurrentView", typeof(object), typeof(SimpleTabWindow), new PropertyMetadata(null, (x, y) =>
				{

				}));

		public InterTabClient TabClient
		{
			get
			{
				return this._tabClient;
			}
			set
			{
				this._tabClient = value;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

namespace NavigationApp.ModuleA
{
	/// <summary>
	/// Interaction logic for ViewA.xaml
	/// </summary>
	[Export("ViewA"), PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
	public partial class ViewA : UserControl
	{
		[ImportingConstructor]
		public ViewA(ViewModelA vm)
		{
			InitializeComponent();
			DataContext = vm;
		}
	}
}

using Microsoft.Practices.Prism.Regions;
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

namespace NavigationApp.ModuleB
{
	/// <summary>
	/// Interaction logic for ViewB.xaml
	/// </summary>
	[Export("ViewB"), PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
	public partial class ViewB : UserControl//, IRegionMemberLifetime
	{
		[ImportingConstructor]
		public ViewB(ViewModelB vm)
		{
			InitializeComponent();
			DataContext = vm;
		}

		public bool KeepAlive
		{
			get { return false; }
		}
	}
}

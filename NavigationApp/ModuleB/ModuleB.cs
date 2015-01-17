using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NavigationApp.ModuleB
{
	[ModuleExport("ModuleB", typeof(ModuleB), InitializationMode = InitializationMode.WhenAvailable)]
	public class ModuleB : IModule
	{
		private readonly IRegionManager regionManager;

		[ImportingConstructor]
		public ModuleB(IRegionManager regionManager)
		{
			this.regionManager = regionManager;

		}
		public void Initialize()
		{
			regionManager.RegisterViewWithRegion(RegionNames.MainMenuRegion, typeof(MenuB));
		}
	}

	[Export]
	public class MenuB : MenuItem
	{
		[ImportingConstructor]
		public MenuB(IRegionManager regionManager)
		{
			Header = "Menu B";
			var viewBMenuItem = new MenuItem();
			viewBMenuItem.Header = "View B";
			viewBMenuItem.Command = new DelegateCommand(() =>
			{
				Uri uri = new Uri("ViewB", UriKind.Relative);
				regionManager.RequestNavigate(RegionNames.MainRegion, uri);
			});
			Items.Add(viewBMenuItem);
		}
	}
}

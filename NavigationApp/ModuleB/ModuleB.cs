using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using NavigationApp.Infra;
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
	public class ViewBWorkspaceView : WorkspaceViewBase
	{


	}

	[Export]
	public class MenuB : MenuItem
	{
		private int ii = 0;
		[ImportingConstructor]
		public MenuB(IRegionManager regionManager)
		{
			
			Header = "Menu B";
			var viewBMenuItem = new MenuItem();
			viewBMenuItem.Header = "View B";
			viewBMenuItem.Command = new DelegateCommand(() =>
			{
				Uri uri = new Uri("ViewB", UriKind.Relative);

				IWorkspaceView workspaceView = new ViewBWorkspaceView();
				workspaceView.Name = string.Format("View B ({0})", ++ii);
				NavigationParameters parameters = new NavigationParameters();
				parameters.Add("WorkspaceView", workspaceView);
				regionManager.RequestNavigate(RegionNames.MainRegion, uri, parameters);
			});
			Items.Add(viewBMenuItem);
		}
	}
}

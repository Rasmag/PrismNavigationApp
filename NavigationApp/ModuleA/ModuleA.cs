using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using NavigationApp.Infra;
using NavigationApp.Infra.Navigation;
using NavigationApp.ModuleB;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NavigationApp.ModuleA
{
	[ModuleExport("ModuleA", typeof(ModuleA), InitializationMode = InitializationMode.WhenAvailable)]
	public class ModuleA : IModule
	{
		private readonly IRegionManager regionManager;

		[ImportingConstructor]
		public ModuleA(IRegionManager regionManager)
		{
			this.regionManager = regionManager;

		}
		public void Initialize()
		{
			regionManager.RegisterViewWithRegion(RegionNames.MainMenuRegion, typeof(MenuA));
		}
	}

	public class ViewAWorkspaceView : WorkspaceViewBase
	{

	}

	[Export]
	public class MenuA : MenuItem
	{

		private Guid group1 = Guid.NewGuid();
		private int ii = 0;
		[ImportingConstructor]
		public MenuA(IRegionManager regionManager, IEventAggregator eventAggregator)
		{
			Header = "Menu A";
			var viewAMenuItem = new MenuItem();
			viewAMenuItem.Header = "View A";
			viewAMenuItem.Command = new DelegateCommand(() =>
			{
				Uri uri = new Uri("ViewA", UriKind.Relative);
				NavigationParameters parameters = new NavigationParameters();
				IWorkspaceView workspaceView = new ViewAWorkspaceView();
				workspaceView.Name = string.Format("View A ({0})", ++ii);
				parameters.Add("WorkspaceView", workspaceView);
				regionManager.RequestNavigate(RegionNames.MainRegion, uri, parameters);
			});
			Items.Add(viewAMenuItem);

			var addViewAToExistingGroupMenuItem = new MenuItem();
			addViewAToExistingGroupMenuItem.Header = "Add View to existing group";
			addViewAToExistingGroupMenuItem.Command = new DelegateCommand(() =>
			{
				Uri uri = new Uri("ViewA", UriKind.Relative);
				NavigationParameters parameters = new NavigationParameters();
				IWorkspaceView workspaceView = new ViewAWorkspaceView();
				workspaceView.Name = string.Format("View A ({0})", ++ii);
				workspaceView.WorkspaceViewGroup = new WorkspaceViewGroup();
				workspaceView.WorkspaceViewGroup.Group = group1;
				parameters.Add("WorkspaceView", workspaceView);
				regionManager.RequestNavigate(RegionNames.MainRegion, uri, parameters);
			});
			Items.Add(addViewAToExistingGroupMenuItem);



			var addViewsMenuItem = new MenuItem();
			addViewsMenuItem.Header = "Multi views";
			addViewsMenuItem.Command = new DelegateCommand(() =>
			{

				Workspace workspace = GetAWorkspace();
				eventAggregator.GetEvent<OpenWorkspace>().Publish(workspace);
			});
			Items.Add(addViewsMenuItem);

			var closeCurrentWorkspace = new MenuItem();
			closeCurrentWorkspace.Header = "Close current workspace";
			closeCurrentWorkspace.Command = new DelegateCommand(() =>
			{
				eventAggregator.GetEvent<NavigationApp.Infra.Navigation.CloseCurrentWorkspace>().Publish(null);
			});
			Items.Add(closeCurrentWorkspace);

		}

		private Workspace GetAWorkspace()
		{
			Workspace workspace = new Workspace();
			int g = 0;
			int workspaceItemNumber = 8;
			double totalScreenWidth = 1920 * 2;
			double totalScreenHeight = 900;
			double heightOffset = 0;
			List<IWorkspaceView> workspaceItemTabs = new List<IWorkspaceView>();
			int[] ids = new int[] { 0, 15 };
			for (int i = 0; i < ids.Count(); i++)
			{
				for (int j = 0; j < workspaceItemNumber; j++)
				{
					var group = Guid.NewGuid();
					var workspaceViewGroup = new WorkspaceViewGroup
					{
						Group = group,
						GroupName = string.Format("Group {0}", ++g),
						Top = heightOffset + ((totalScreenHeight / ids.Count()) * i),
						Height = totalScreenHeight / ids.Count(),
						Width = totalScreenWidth / workspaceItemNumber,
						Left = (totalScreenWidth / workspaceItemNumber) * j
					};
					workspace.ViewGroups.Add(workspaceViewGroup);
					for (int idfs = 0; idfs < 2; idfs++)
					{
						workspaceViewGroup.Views.Add(new ViewAWorkspaceView
						{
							Name = string.Format("View A ({0})", ++ii),
							ViewTypeName = "ViewA",
							WorkspaceViewGroup = workspaceViewGroup
						});
						workspaceViewGroup.Views.Add(new ViewBWorkspaceView
						{
							Name = string.Format("View B ({0})", ++ii),
							ViewTypeName = "ViewB",
							WorkspaceViewGroup = workspaceViewGroup
						});
					}
				}
			}
			return workspace;
		}
	}
}

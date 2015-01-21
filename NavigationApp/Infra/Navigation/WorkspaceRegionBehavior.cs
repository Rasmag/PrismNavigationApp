using Dragablz;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NavigationApp.Infra;
using System.ComponentModel.Composition;

namespace NavigationApp.Infra.Navigation
{

	public class GetWorkspace : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<Action<Workspace>>
	{
	}

	public class OpenWorkspace : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<Workspace>
	{

	}

	public class CloseCurrentWorkspace : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<Action>
	{

	}

	[Export]
	public class WorkspaceRegionBehavior : RegionBehavior, IHostAwareRegionBehavior
	{
		private IRegionManager _regionManager;
		public Workspace GetWorkspace()
		{
			var workspace = new Workspace();
			workspace.ViewGroups.AddRange(_windows.Select(a => a.Value.WorkspaceViewGroup));
			return workspace;
		}
		private Workspace _workspace = new Workspace();

		private readonly InterTabClient _tabClient;
		[ImportingConstructor]
		public WorkspaceRegionBehavior(IEventAggregator eventAggregator, IRegionManager regionManager)
		{
			_regionManager = regionManager;
			eventAggregator.GetEvent<GetWorkspace>().Subscribe(callback =>
			{
				if (callback != null)
					callback(GetWorkspace());
			});
			eventAggregator.GetEvent<CloseCurrentWorkspace>().Subscribe(callback =>
			{
				CloseWorkspace();
				if (callback != null)
					callback();
			});
			eventAggregator.GetEvent<OpenWorkspace>().Subscribe(OpenWorkspace);
			_tabClient = new InterTabClient(() =>
			{
				var window = GetNewWindow();
				_windows.Add(window.WorkspaceViewGroup.Group, window);
				if (!_workspace.ViewGroups.Contains(window.WorkspaceViewGroup))
				{
					_workspace.ViewGroups.Add(window.WorkspaceViewGroup);
				}
				return window;
			}, window =>
			{
				_windows.Remove(window.WorkspaceViewGroup.Group);
				_workspace.ViewGroups.Add(window.WorkspaceViewGroup);
			});
		}

		private void OpenWorkspace(Workspace workspace)
		{
			CloseWorkspace();
			_workspace = workspace;
			foreach (var item in _workspace.ViewGroups.ToArray().SelectMany(a => a.Views.ToArray()))
			{
				Uri uri = new Uri(item.ViewTypeName, UriKind.Relative);
				NavigationParameters parameters = new NavigationParameters();
				IWorkspaceView workspaceView = item;
				parameters.Add("WorkspaceView", workspaceView);
				_regionManager.RequestNavigate(RegionNames.MainRegion, uri, parameters);
			}
		}

		private void CloseWorkspace()
		{
			foreach (var window in _windows.Values.ToArray())
			{
				window.Close();
			}
		}

		private SimpleTabWindow GetNewWindow()
		{
			SimpleTabWindow window = new SimpleTabWindow();
			window.WorkspaceViewGroup = new WorkspaceViewGroup();
			//window.Group = window.WorkspaceViewGroup.Group;
			window.TabClient = _tabClient;
			window.Closed += (x, y) =>
			{
				foreach (var existingView in window.Views.ToArray())
				{
					Region.Remove(existingView);
				}
			};
			window.tabablzControl.ClosingItemCallback = dragablzEvent =>
			{
				Region.Remove(dragablzEvent.DragablzItem.DataContext);
			};
			return window;
		}

		private readonly Dictionary<Guid, SimpleTabWindow> _windows = new Dictionary<Guid, SimpleTabWindow>();

		public Dictionary<Guid, SimpleTabWindow> Windows
		{
			get
			{
				return _windows;
			}
		}

		public DependencyObject HostControl
		{
			get;
			set;
		}
		protected override void OnAttach()
		{
			this.Region.Views.CollectionChanged += Views_CollectionChanged;

			this.Region.NavigationService.Navigated += NavigationService_Navigated;
			this.Region.NavigationService.Navigating += NavigationService_Navigating;

		}

		void NavigationService_Navigating(object sender, RegionNavigationEventArgs e)
		{
			var view = sender as UserControl;
			if (view == null)
			{
				throw new Exception("Erreur");
			}
			var param = e.NavigationContext.Parameters["WorkspaceView"];
			if (param == null)
			{
				throw new Exception("Erreur");
			}
			var workspaceView = param as IWorkspaceView;
			if (workspaceView == null)
			{
				throw new Exception("Erreur");
			}
			var workspaceViewmodel = view.DataContext as IWorkspaceViewModel;
			workspaceViewmodel.WorkspaceView = workspaceView;
			workspaceViewmodel.Close = () =>
			{
				Region.Remove(view);
			};
		}

		void NavigationService_Navigated(object sender, RegionNavigationEventArgs e)
		{
			var view = sender as UserControl;
			SimpleTabWindow window = null;
			if (view == null)
			{
				throw new Exception("Erreur");
			}
			var param = e.NavigationContext.Parameters["WorkspaceView"];
			if (param == null)
			{
				throw new Exception("Erreur");
			}
			var workspaceView = param as IWorkspaceView;
			if (workspaceView == null)
			{
				throw new Exception("Erreur");
			}
			var windowWasFound = workspaceView.WorkspaceViewGroup != null && _windows.TryGetValue(workspaceView.WorkspaceViewGroup.Group, out window);
			// 1 workspaceViewGroup est != de nul et pas de win correspondant
			// 2 workspaceViewGroup est != de nul et win correspondant
			// 3 workspaceViewGroup est nul donc new win
			if (workspaceView.WorkspaceViewGroup != null && !windowWasFound)
			{
				window = GetNewWindow();
				window.WorkspaceViewGroup = workspaceView.WorkspaceViewGroup;
				_windows.Add(window.WorkspaceViewGroup.Group, window);
				if (!_workspace.ViewGroups.Contains(window.WorkspaceViewGroup))
				{
					_workspace.ViewGroups.Add(window.WorkspaceViewGroup);
				}
				window.Show();
			}
			else if (workspaceView.WorkspaceViewGroup == null && !windowWasFound)
			{
				window = GetNewWindow();
				workspaceView.WorkspaceViewGroup = window.WorkspaceViewGroup;
				_windows.Add(window.WorkspaceViewGroup.Group, window);
				if (!_workspace.ViewGroups.Contains(window.WorkspaceViewGroup))
				{
					_workspace.ViewGroups.Add(window.WorkspaceViewGroup);
				}
				window.Show();
			}
			else if (workspaceView.WorkspaceViewGroup != null && windowWasFound) //
			{

			}
			window.Views.Add(view);
			window.tabablzControl.SelectedItem = view;
		}

		void Views_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
					//this.OnViewAddedToRegion(e.NewItems[0]);
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
					this.OnViewRemovedFromRegion(e.OldItems[0]);
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
					break;
				default:
					break;
			}
		}

		private void OnViewRemovedFromRegion(object view)
		{
			var workspaceEnabled = (view as FrameworkElement).DataContext as WorkspaceViewBase;
			if (workspaceEnabled != null)
			{
				//workspaceEnabled.OnWorkspaceTabClosed();
			}
			(view as FrameworkElement).DataContext = null;

			var window = _windows.FirstOrDefault(a => a.Value.Views.Contains(view));
			if (window.Value != null)
			{
				if (window.Value.Views.Count == 1)
				{
					if (window.Value.Views.Remove(view))
					{
						window.Value.Close();

						_windows.Remove(window.Value.WorkspaceViewGroup.Group);
						_workspace.ViewGroups.Remove(window.Value.WorkspaceViewGroup);
					}
				}
				else
				{
					window.Value.Views.Remove(view);
				}
			}
		}

		public const string BehaviorKey = "WorkspaceRegionBehavior";


	}
}

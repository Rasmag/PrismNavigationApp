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

namespace NavigationApp.Infra
{
	public static class RegionBehaviors
	{
		public static string GetWorkspaceRegion(DependencyObject obj)
		{
			return (string)obj.GetValue(WorkspaceRegionProperty);
		}

		public static void SetWorkspaceRegion(DependencyObject obj, string value)
		{
			obj.SetValue(WorkspaceRegionProperty, value);
		}

		// Using a DependencyProperty as the backing store for TabWindowsRegion.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WorkspaceRegionProperty =
				DependencyProperty.RegisterAttached("WorkspaceRegion", typeof(string), typeof(RegionBehaviors), new PropertyMetadata(TabWindowsRegionChanged));

		private static void TabWindowsRegionChanged(DependencyObject hostControl, DependencyPropertyChangedEventArgs e)
		{
			if (IsInDesignMode(hostControl))
			{
				return;
			}
			RegisterNewPopupRegion(hostControl, e.NewValue as string);
		}

		public static void RegisterNewPopupRegion(DependencyObject owner, string regionName)
		{
			// Creates a new region and registers it in the default region manager.
			// Another option if you need the complete infrastructure with the default region behaviors
			// is to extend DelayedRegionCreationBehavior overriding the CreateRegion method and create an 
			// instance of it that will be in charge of registering the Region once a RegionManager is
			// set as an attached property in the Visual Tree.
			IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
			if (regionManager != null)
			{
				IRegion region = new AllActiveRegion();

				WorkspaceRegionBehavior behavior;

				behavior = new WorkspaceRegionBehavior();

				behavior.HostControl = owner;

				region.Behaviors.Add(WorkspaceRegionBehavior.BehaviorKey, behavior);

				regionManager.Regions.Add(regionName, region);
			}
		}

		private static bool IsInDesignMode(DependencyObject element)
		{
			// Due to a known issue in Cider, GetIsInDesignMode attached property value is not enough to know if it's in design mode.
			return DesignerProperties.GetIsInDesignMode(element) || Application.Current == null
						 || Application.Current.GetType() == typeof(Application);
		}
	}
	public class WorkspaceRegionBehavior : RegionBehavior, IHostAwareRegionBehavior
	{
		private readonly InterTabClient _tabClient;
		public WorkspaceRegionBehavior()
		{
			_tabClient = new InterTabClient(window =>
			{
				window.Group = Guid.NewGuid();
				window.TabClient = _tabClient;
				_windows.Add(window.Group, window);
			}, window =>
			{
				_windows.Remove(window.Group);
			});
		}
		private readonly Dictionary<Guid, SimpleTabWindow> _windows = new Dictionary<Guid, SimpleTabWindow>();

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
			SimpleTabWindow window;
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
			if (!_windows.TryGetValue(workspaceView.Group, out window))
			{
				window = new SimpleTabWindow();
				workspaceView.Group = workspaceView.Group == new Guid() ? Guid.NewGuid() : workspaceView.Group;
				window.Group = workspaceView.Group;
				window.TabClient = _tabClient;
				window.Closed += (x, y) =>
				{
					foreach (var existingView in window.Views.ToArray())
					{
						Region.Remove(existingView);
					}
				};
				if (workspaceView.WorkspaceViewGroup != null && workspaceView.WorkspaceViewGroup.Width != 0 && workspaceView.WorkspaceViewGroup.Height != 0)
				{
					window.Top = workspaceView.WorkspaceViewGroup.Top;
					window.Left = workspaceView.WorkspaceViewGroup.Left;
					window.Width = workspaceView.WorkspaceViewGroup.Width;
					window.Height = workspaceView.WorkspaceViewGroup.Height;
				}
				window.Show();
				_windows.Add(window.Group, window);
			}
			window.Views.Add(view);
			window.CurrentView = view;
		}

		void NavigationService_Navigated(object sender, RegionNavigationEventArgs e)
		{

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

						_windows.Remove(window.Value.Group);
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

	public class InterTabClient : IInterTabClient
	{
		private readonly Action<SimpleTabWindow> _onTabWindowCreated;
		private readonly Action<SimpleTabWindow> _onTabWindowClosedByDragging;

		public InterTabClient(Action<SimpleTabWindow> onTabWindowCreated, Action<SimpleTabWindow> onTabWindowClosedByDragging)
		{
			_onTabWindowCreated = onTabWindowCreated;
			_onTabWindowClosedByDragging = onTabWindowClosedByDragging;
		}
		public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
		{
			var window = new SimpleTabWindow();
			_onTabWindowCreated(window);
			return new NewTabHost<SimpleTabWindow>(window, window.tabablzControl);
		}

		public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
		{
			_onTabWindowClosedByDragging(window as SimpleTabWindow);
			return TabEmptiedResponse.CloseWindow;
		}
	}
}

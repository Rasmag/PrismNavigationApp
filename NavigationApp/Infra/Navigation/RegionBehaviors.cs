using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using NavigationApp.Infra;

namespace NavigationApp.Infra.Navigation
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

				behavior = ServiceLocator.Current.GetInstance<WorkspaceRegionBehavior>();

				behavior.HostControl = owner;

				region.Behaviors.Add(WorkspaceRegionBehavior.BehaviorKey, behavior);

				regionManager.Regions.Add(regionName, region);
			}
		}

		private static bool IsInDesignMode(DependencyObject element)
		{
			// Due to a known issue in Cider, GetIsInDesignMode attached property value is not enough to know if it's in design mode.
			return DesignerProperties.GetIsInDesignMode(element) || Application.Current == null ||
						 Application.Current.GetType() == typeof(Application);
		}
	}
}
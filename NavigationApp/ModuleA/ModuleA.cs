using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using NavigationApp.Infra.Navigation;
using NavigationApp.Infra.ViewObjects;
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
}

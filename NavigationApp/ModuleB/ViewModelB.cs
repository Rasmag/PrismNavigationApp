using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace NavigationApp.ModuleB
{
	[Export, PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
	public class ViewModelB// : INavigationAware
	{
		~ViewModelB()
		{

		}
		public bool KeepAlive
		{
			get { return false; }
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return false;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			
		}
	}
}

using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using NavigationApp.Infra.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using NavigationApp.Infra.ViewObjects;

namespace NavigationApp.ModuleB
{

	[Export, PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
	public class ViewModelB : BindableBase, INavigationAware, IWorkspaceViewModel
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
		public IWorkspaceView WorkspaceView
		{
			get;
			set;
		}

		public Action Close
		{
			get;
			set;
		}

		public void OnClose()
		{
			
		}
	}
}

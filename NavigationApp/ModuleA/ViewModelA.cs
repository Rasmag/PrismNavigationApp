using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using NavigationApp.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace NavigationApp.ModuleA
{
	[Export, PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
	public class ViewModelA : BindableBase, INavigationAware
	{
		public ViewModelA()
		{

		}
		private string _name;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(() => Name);
			}
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
			var workspaceView = navigationContext.Parameters["WorkspaceView"] as IWorkspaceView;
			Name = workspaceView.Name;
		}
	}
}

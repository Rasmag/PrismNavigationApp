using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using NavigationApp.Infra;
using NavigationApp.Infra.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace NavigationApp.ModuleA
{
	[Export, PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
	public class ViewModelA : BindableBase, INavigationAware, IWorkspaceViewModel
	{
		~ViewModelA()
		{

		}
		public ViewModelA()
		{
			CloseCommand = new DelegateCommand(() => Close());
		}
		public DelegateCommand CloseCommand { get; private set; }

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

		void IWorkspaceViewModel.OnClose()
		{
			throw new NotImplementedException();
		}
	}
}

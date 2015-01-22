using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationApp.Infra.ViewObjects;

namespace NavigationApp.Infra.ViewObjects
{
	public class Workspace
	{
		public Workspace()
		{
			ViewGroups = new List<WorkspaceViewGroup>();
		}
		public string Name { get; set; }
		public List<WorkspaceViewGroup> ViewGroups { get; set; }
	}
}

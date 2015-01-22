using System;
using NavigationApp.Infra;
using NavigationApp.Infra.ViewObjects;

namespace NavigationApp.Infra.Navigation
{
	public interface IWorkspaceViewModel
	{
		Action Close { get; set; }
		void OnClose();
		IWorkspaceView WorkspaceView { get; set; }
	}
}
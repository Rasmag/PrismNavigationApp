using System;
using System.Windows;
using Dragablz;

namespace NavigationApp.Infra
{
	public class InterTabClient : IInterTabClient
	{
		private readonly Func<SimpleTabWindow> _onTabWindowCreated;
		private readonly Action<SimpleTabWindow> _onTabWindowClosedByDragging;

		public InterTabClient(Func<SimpleTabWindow> onTabWindowCreated, Action<SimpleTabWindow> onTabWindowClosedByDragging)
		{
			_onTabWindowCreated = onTabWindowCreated;
			_onTabWindowClosedByDragging = onTabWindowClosedByDragging;
		}

		public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
		{
			var win = _onTabWindowCreated();
			return new NewTabHost<SimpleTabWindow>(win, win.tabablzControl);
		}

		public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
		{
			_onTabWindowClosedByDragging(window as SimpleTabWindow);
			return TabEmptiedResponse.CloseWindow;
		}
	}
}
using Microsoft.Practices.Prism.Mvvm;

namespace NavigationApp.Infra.ViewObjects
{
	public abstract class WorkspaceViewBase : BindableBase, IWorkspaceView
	{
		private string _name;

		public string ViewTypeName { get; set; }

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
				OnPropertyChanged(() => Name);
			}
		}

		public WorkspaceViewGroup WorkspaceViewGroup { get; set; }
	}
}
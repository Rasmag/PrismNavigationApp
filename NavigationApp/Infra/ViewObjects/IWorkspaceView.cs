namespace NavigationApp.Infra.ViewObjects
{
	public interface IWorkspaceView
	{
		string ViewTypeName { get; set; }

		string Name { get; set; }

		WorkspaceViewGroup WorkspaceViewGroup { get; set; }
	}
}
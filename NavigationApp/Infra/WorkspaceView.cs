using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationApp.Infra
{

	public class Workspace
	{

	}

	public class WorkspaceViewGroup
	{
		public WorkspaceViewGroup()
		{
			Views = new List<IWorkspaceView>();
		}
		public Guid Group
		{
			get;
			set;
		}
		public string GroupName { get; set; }
		public double Top { get; set; }
		public double Left { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public IEnumerable<IWorkspaceView> Views { get; private set; }

	}
	
	public interface IWorkspaceView
	{
		string ViewTypeName { get; set; }
		string Name { get; set; }
		Guid Group { get; set; }
		WorkspaceViewGroup WorkspaceViewGroup { get; set; }
	}

	public abstract class WorkspaceViewBase : IWorkspaceView
	{
		public string ViewTypeName { get; set; }
		public string Name { get; set; }
		public Guid Group { get; set; }

		public WorkspaceViewGroup WorkspaceViewGroup { get; set; }
	}

}

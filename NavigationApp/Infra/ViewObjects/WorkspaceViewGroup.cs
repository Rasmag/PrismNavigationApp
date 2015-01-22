using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;

namespace NavigationApp.Infra.ViewObjects
{
	public class WorkspaceViewGroup : BindableBase
	{
		private double _top;

		private double _left;

		private double _width;

		private double _height;

		public WorkspaceViewGroup()
		{
			Views = new List<IWorkspaceView>();
			Height = double.NaN;
			Left = double.NaN;
			Top = double.NaN;
			Width = double.NaN;
			GroupName = "New window";
			Group = Guid.NewGuid();
		}

		public Guid Group { get; set; }

		public string GroupName { get; set; }

		public double Top
		{
			get
			{
				return this._top;
			}
			set
			{
				this._top = value;
				OnPropertyChanged(() => Top);
			}
		}

		public double Left
		{
			get
			{
				return this._left;
			}
			set
			{
				this._left = value;
				OnPropertyChanged(() => Left);
			}
		}

		public double Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = value;
				OnPropertyChanged(() => Width);
			}
		}

		public double Height
		{
			get
			{
				return this._height;
			}
			set
			{
				this._height = value;
				OnPropertyChanged(() => Height);
			}
		}

		public List<IWorkspaceView> Views { get; private set; }
	}
}
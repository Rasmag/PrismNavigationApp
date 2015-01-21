using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NavigationApp
{
	public class Bootstrapper : MefBootstrapper
	{
		protected override System.Windows.DependencyObject CreateShell()
		{
			return this.Container.GetExportedValue<Shell>();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();
			Application.Current.MainWindow = (Window)this.Shell;
			Application.Current.MainWindow.Show();
		}

		protected override void ConfigureAggregateCatalog()
		{
			this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));
			base.ConfigureAggregateCatalog();
		}


	}

}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Runner
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		//protected override void OnStartup(StartupEventArgs e)
		//protected override void OnActivated(EventArgs e)
		protected override void OnLoadCompleted(NavigationEventArgs e)
		{
			//base.OnStartup(e);
			//base.OnActivated(e);
			base.OnLoadCompleted(e);

			//MainWindow.DataContext = new ViewModels.MainWindowViewModel();
		}
	}
}

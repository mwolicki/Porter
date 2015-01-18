using System;
using DockingLibrary;
using PorterApp.ViewModel;

namespace PorterApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public sealed partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel();

			Loaded += OnLoaded;
			WindowDispatcher.SetDockManager(DockManager);
		}

		private void OnLoaded(object sender, EventArgs e)
		{
			DockManager.ParentWindow = this;
		}
	}


	public static class WindowDispatcher
	{
		private static DockManager _dockManager;

		public static void SetDockManager(DockManager dockManager)
		{
			_dockManager = dockManager;
		}

		public static void Show(ManagedContent  w)
		{
			w.DockManager = _dockManager;
			w.Show();
		}
	}

}
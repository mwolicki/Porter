using System.Windows;

namespace PorterApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel();
		}
	}
}
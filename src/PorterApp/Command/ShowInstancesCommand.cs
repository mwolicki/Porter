using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PorterApp.ViewModel;

namespace PorterApp.Command
{
	public class ShowInstancesCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{

			var typeTreeItem = parameter as TypeTreeItem;
			if (typeTreeItem != null)
			{
				var treeItems = typeTreeItem.Instances.Take(200).Select(p => new TreeItem
				{
					Name = p().TypeObjectDescription.Name
				}).ToArray();
				WindowDispatcher.Show(new TypesTreeWindow(new ObservableCollection<TreeItem>(treeItems)));
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}
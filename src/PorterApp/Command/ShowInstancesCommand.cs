using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PorterApp.UserControls;
using PorterApp.ViewModel;

namespace PorterApp.Command
{
	public class ShowInstancesCommand : BaseCommand<TypeTreeItem>
	{
		public override void Execute(TypeTreeItem typeTreeItem)
		{
			var treeItems = typeTreeItem.Instances.Take(200).Select(p => new ReferenceObjectTreeItem
			{
				Name = p().TypeObjectDescription.Name
			}).ToArray();
			WindowDispatcher.Show(new TypesTreeWindow(new TreeViewModel { TreeItems = new ObservableCollection<ITreeItem>(treeItems) }));
		}
	}
}
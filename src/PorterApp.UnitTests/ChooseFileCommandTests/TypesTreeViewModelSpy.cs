using System.Collections.ObjectModel;
using PorterApp.UserControls;
using PorterApp.ViewModel;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	internal sealed class TypesTreeViewModelSpy : ITypesTreeViewModel
	{
		public TypesTreeViewModelSpy()
		{
			TypesTree = new ObservableCollection<ObservableCollection<ITreeItem>>();
		}

		public ObservableCollection<ObservableCollection<ITreeItem>> TypesTree { get; set; }
	}
}
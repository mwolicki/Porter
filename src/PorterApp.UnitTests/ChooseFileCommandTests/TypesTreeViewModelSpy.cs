using System.Collections.ObjectModel;
using PorterApp.ViewModel;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	internal sealed class TypesTreeViewModelSpy : ITypesTreeViewModel
	{
		public TypesTreeViewModelSpy()
		{
			TypesTree = new ObservableCollection<TreeItem>();
		}

		public ObservableCollection<TreeItem> TypesTree { get; set; }
	}
}
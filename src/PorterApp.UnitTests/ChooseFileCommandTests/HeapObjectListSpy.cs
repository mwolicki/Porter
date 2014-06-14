using System.Collections.ObjectModel;
using PorterApp.ViewModel;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	internal sealed class HeapObjectListSpy : IHeapObjectList
	{
		public ObservableCollection<ObjectViewModel> Objects { get; set; }

		public HeapObjectListSpy()
		{
			Objects = new ObservableCollection<ObjectViewModel>();
		}
	}
}
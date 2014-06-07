using System.Collections.ObjectModel;

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
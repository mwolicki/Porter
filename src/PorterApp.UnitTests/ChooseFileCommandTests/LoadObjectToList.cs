using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class LoadObjectToList : ChosenFile
	{
		[Fact]
		public void Execute_SelectedFile_ObjectsLoaded()
		{
			var chooseFileCommand = GetChooseFileCommand();
			chooseFileCommand.Execute(HeapObjectListSpy);
			Assert.True(HeapObjectListSpy.Objects.Any());
		}

		[Fact]
		public void Execute_SelectedFile_ObjectsHaveCorrectNames()
		{
			var expected = new ObservableCollection<ObjectViewModel>(new[]
			{
				new ObjectViewModel { Name = "a", Size = 1 },
				new ObjectViewModel { Name = "b", Size = 2 },
				new ObjectViewModel { Name = "c", Size = 3 }
			});

			var chooseFileCommand = GetChooseFileCommand();
			chooseFileCommand.Execute(HeapObjectListSpy);
			Assert.Equal(expected, HeapObjectListSpy.Objects, new DelegateComparer<ObjectViewModel>((a, b) => a.Name == b.Name));
		}

		[Fact]
		public void Execute_SelectedFile_ObjectsHaveCorrectSize()
		{
			var expected = new ObservableCollection<ObjectViewModel>(new[]
			{
				new ObjectViewModel { Name = "a", Size = 1 },
				new ObjectViewModel { Name = "b", Size = 2 },
				new ObjectViewModel { Name = "c", Size = 3 }
			});

			var chooseFileCommand = GetChooseFileCommand();
			chooseFileCommand.Execute(HeapObjectListSpy);
			Assert.Equal(expected, HeapObjectListSpy.Objects, new DelegateComparer<ObjectViewModel>((a, b) => a.Size == b.Size));
		}

		[Fact]
		public void Execute_SelectedFile_TryLoadObjectFromCorrectFileName()
		{
			const string expectedFileName = "expectedFileName";
			var chooseFileCommand = GetChooseFileCommand(expectedFileName);
			chooseFileCommand.Execute(HeapObjectListSpy);
			Assert.Equal(expectedFileName, ExtendedDebuggerFactorySpy.FilePath);
		}
	}
}
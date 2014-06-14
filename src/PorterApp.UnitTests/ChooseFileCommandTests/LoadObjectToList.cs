using System.Linq;
using PorterApp.ViewModel;
using Xunit;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class LoadObjectToList : ChosenFile
	{
		[Fact]
		public void Execute_SelectedFile_ObjectsLoaded()
		{
			InvokeExecute();
			Assert.True(HeapObjectListSpy.Objects.Any());
		}

		[Fact]
		public void Execute_SelectedFile_ObjectsAreCorrectlyPopulated()
		{
			var comparer = new DelegateComparer<ObjectViewModel>((a, b) => a.Name == b.Name && a.ObjectRef == b.ObjectRef && a.Size == b.Size);

			InvokeExecute();

			Assert.Equal(NotEmptyDebuggerStub.ExpectedObjectViewModels, HeapObjectListSpy.Objects, comparer);
		}

		private void InvokeExecute()
		{
			GetChooseFileCommand().Execute(HeapObjectListSpy);
		}

		[Fact]
		public void Execute_SelectedFile_TryLoadObjectFromCorrectFileName()
		{
			const string expectedFileName = "expectedFileName";
			var chooseFileCommand = GetChooseFileCommand(expectedFileName);
			chooseFileCommand.Execute(HeapObjectListSpy);
			Assert.Equal(expectedFileName, ExtendedDebuggerFactorySpy.FilePath);
		}

		[Fact]
		public void Execute_SelectedFile_ClearList()
		{
			InvokeExecute();
			InvokeExecute();
			Assert.Equal(NotEmptyDebuggerStub.ExpectedObjectViewModels, HeapObjectListSpy.Objects, new DelegateComparer<ObjectViewModel>((a, b) => a.Size == b.Size));
		}
	}
}
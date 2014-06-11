using System.Linq;
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
		public void Execute_SelectedFile_ObjectsHaveCorrectNames()
		{
			InvokeExecute();
			Assert.Equal(NotEmptyDebuggerStub.ExpectedObjectViewModels, HeapObjectListSpy.Objects, new DelegateComparer<ObjectViewModel>((a, b) => a.Name == b.Name));
		}

		[Fact]
		public void Execute_SelectedFile_ObjectsHaveCorrectSize()
		{
			InvokeExecute();
			Assert.Equal(NotEmptyDebuggerStub.ExpectedObjectViewModels, HeapObjectListSpy.Objects, new DelegateComparer<ObjectViewModel>((a, b) => a.Size == b.Size));
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
	}
}
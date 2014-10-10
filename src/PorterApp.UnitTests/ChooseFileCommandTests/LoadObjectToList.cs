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
			Assert.True(TypesTreeViewModelSpy.TypesTree.Any());
		}

		private readonly DelegateComparer<TreeItem> _delegateComparer = new DelegateComparer<TreeItem>((a, b) => a.Name == b.Name && a.Children.Count == b.Children.Count);

		[Fact]
		public void Execute_SelectedFile_ObjectsAreCorrectlyPopulated()
		{
			
			InvokeExecute();
			Assert.Equal(NotEmptyDebuggerStub.ExpectedObjectViewModels, TypesTreeViewModelSpy.TypesTree, _delegateComparer);
		}

		private void InvokeExecute()
		{
			GetChooseFileCommand().Execute(TypesTreeViewModelSpy);
		}

		[Fact]
		public void Execute_SelectedFile_TryLoadObjectFromCorrectFileName()
		{
			const string expectedFileName = "expectedFileName";
			var chooseFileCommand = GetChooseFileCommand(expectedFileName);
			chooseFileCommand.Execute(TypesTreeViewModelSpy);
			Assert.Equal(expectedFileName, ExtendedDebuggerFactorySpy.FilePath);
		}

		[Fact]
		public void Execute_SelectedFile_ClearList()
		{
			InvokeExecute();
			InvokeExecute();
			Assert.Equal(NotEmptyDebuggerStub.ExpectedObjectViewModels, TypesTreeViewModelSpy.TypesTree, _delegateComparer);
		}
	}
}
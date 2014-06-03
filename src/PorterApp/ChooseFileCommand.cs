using Porter;
using System;
using System.Windows.Input;

namespace PorterApp
{
	internal sealed class ChooseFileCommand : ICommand
	{
		private readonly Func<IExtendedDebuggerFactory> _extendedDebugger;
		private readonly Func<IOpenFileDialog> _openDumpFileFactory;

		public ChooseFileCommand(Func<IExtendedDebuggerFactory> debuggerFactory, Func<IOpenFileDialog> openDumpFileFactory)
		{
			_extendedDebugger = debuggerFactory;
			_openDumpFileFactory = openDumpFileFactory;
		}

		public ChooseFileCommand()
		{
			_extendedDebugger = () => new ExtendedDebuggerFactory();
			_openDumpFileFactory = () => new OpenDumpFileDialog();
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
		}

		//public void Execute(object parameter)
		//{
		//	string fileName;
		//	if (_openDumpFileFactory().TryGetFileName(out fileName))
		//	{
		//		LoadHeapObjects(parameter, fileName);
		//	}
		//}

		//private void LoadHeapObjects(object parameter, string fileName)
		//{
		//	using (var extendedDebugger = _extendedDebugger().Create(fileName))
		//	{
		//		var objects = ((IHeapObjectList)parameter).Objects;
		//		objects.Clear();
		//		foreach (var clrData in extendedDebugger.GetClrs())
		//		{
		//			foreach (var obj in clrData.GetHeapObjects())
		//			{
		//				var o = obj();
		//				objects.Add(new ObjectViewModel
		//				{
		//					Name = o.TypeObjectDescription.Name,
		//					Size = o.Size
		//				});
		//			}
		//		}
		//	}
		//}

		public event EventHandler CanExecuteChanged;
	}
}
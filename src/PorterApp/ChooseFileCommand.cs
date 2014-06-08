using Porter;
using Porter.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace PorterApp
{
	internal sealed class ChooseFileCommand : ICommand
	{
		private readonly Func<IExtendedDebuggerFactory> _extendedDebugger;
		private readonly Func<IOpenFileDialog> _openDumpFileFactory;
		private readonly IDumpFileNotFoundAlert _alert;

		public ChooseFileCommand(Func<IExtendedDebuggerFactory> debuggerFactory, Func<IOpenFileDialog> openDumpFileFactory, IDumpFileNotFoundAlert alert)
		{
			_extendedDebugger = debuggerFactory;
			_openDumpFileFactory = openDumpFileFactory;
			_alert = alert;
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
			if (parameter is IHeapObjectList)
			{
				ObservableCollection<ObjectViewModel> objectViewModels = ((IHeapObjectList)parameter).Objects;
				string fileName;
				if (_openDumpFileFactory().TryGetFileName(out fileName))
				{
					try
					{
						IExtendedDebugger extendedDebugger = _extendedDebugger().Create(fileName);
						foreach (var heapObjects in extendedDebugger.GetClrs().SelectMany(p => p.GetHeapObjects()))
						{
							IReferenceObject referenceObject = heapObjects();
							objectViewModels.Add(new ObjectViewModel
							{
								Name = referenceObject.TypeObjectDescription.Name,
								Size = referenceObject.Size
							});
						}
					}
					catch (FileNotFoundException)
					{
						_alert.Display(fileName);
					}
				}
			}
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
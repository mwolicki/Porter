using Porter;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using PorterApp.Extensions;

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
			var list = parameter as IHeapObjectList;
			if (list != null)
			{
				Execute(list);
			}
		}

		private void Execute(IHeapObjectList list)
		{
			ObservableCollection<ObjectViewModel> objectViewModels = list.Objects;

			string fileName;
			if (_openDumpFileFactory().TryGetFileName(out fileName))
			{
				PopulateObjectList(fileName, objectViewModels);
			}
		}

		private void PopulateObjectList(string fileName, ObservableCollection<ObjectViewModel> objectViewModels)
		{
			try
			{
				IExtendedDebugger extendedDebugger = _extendedDebugger().Create(fileName);

				objectViewModels.Clear();

				foreach (var heapObjects in extendedDebugger.GetClrs().SelectMany(p => p.GetHeapObjects()))
				{
					objectViewModels.AddHeapObject(heapObjects);
				}
			}
			catch (FileNotFoundException)
			{
				_alert.Display(fileName);
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}
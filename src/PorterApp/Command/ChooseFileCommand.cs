using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Porter;
using Porter.Models;
using PorterApp.Extensions;
using PorterApp.ViewModel;

namespace PorterApp.Command
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

		private  void Execute(IHeapObjectList list)
		{
			ObservableCollection<TreeItem> objectViewModels = list.TypesTree;

			string fileName;
			if (_openDumpFileFactory().TryGetFileName(out fileName))
			{
				PopulateObjectList(fileName, objectViewModels);
			}
		}

		private async void PopulateObjectList(string fileName, ObservableCollection<TreeItem> objectViewModels)
		{
			objectViewModels.Clear();
			try
			{
				foreach (var typeNode in await SelectMany(fileName))
				{
					objectViewModels.Add(new TypeTreeItem(typeNode));
				}
			}
			catch (FileNotFoundException)
			{
				_alert.Display(fileName);
			}
		}

		public static Task<T> StartSTATask<T>(Func<T> func)
		{
			var tcs = new TaskCompletionSource<T>();
			var thread = new Thread(() =>
			{
				try
				{
					tcs.SetResult(func());
				}
				catch (Exception e)
				{
					tcs.SetException(e);
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			return tcs.Task;
		}

		private Task<ITypeNode[]> SelectMany(string fileName)
		{

			return StartSTATask(() =>
			{
				
				IExtendedDebugger extendedDebugger = _extendedDebugger().Create(fileName);
				return extendedDebugger.GetClrs().SelectMany(p => p.GetTypeHierarchy()).ToArray();
			});
		}

		public event EventHandler CanExecuteChanged;
	}
}
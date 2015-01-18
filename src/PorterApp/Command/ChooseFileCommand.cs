using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Porter;
using Porter.Models;
using PorterApp.ViewModel;

namespace PorterApp.Command
{
	internal sealed class ChooseFileCommand : BaseCommand<ITypesTreeViewModel>
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

		public override void Execute(ITypesTreeViewModel list)
		{
			string fileName;
			if (_openDumpFileFactory().TryGetFileName(out fileName))
			{
				PopulateObjectList(fileName);
			}
		}

		private async void PopulateObjectList(string fileName)
		{
			try
			{
				var treeItems = new ObservableCollection<TreeItem>();
				foreach (var typeNode in SelectMany(fileName))
				{
					treeItems.Add(new TypeTreeItem(typeNode));
				}
				WindowDispatcher.Show(new TypesTreeWindow(treeItems));
			}
			catch (FileNotFoundException)
			{
				_alert.Display(fileName);
			}
		}

		private IEnumerable<ITypeNode> SelectMany(string fileName)
		{
			return _extendedDebugger().Create(fileName).GetClrs().SelectMany(p =>
			{
				var typeNodes1 = p.GetTypeHierarchy();
				return typeNodes1.ToArray();
			});
		}
	}
}
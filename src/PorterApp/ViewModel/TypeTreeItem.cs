using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Porter.Diagnostics.Decorator;
using Porter.Models;
using PorterApp.Command;
using PorterApp.UserControls;

namespace PorterApp.ViewModel
{
	public sealed class TypeTreeItem : TreeItem<TypeTreeItem>
	{

		public ICommand ToRemoveOnlyForTest
		{
			get { return new ShowInstancesCommand(); }
		}

		public ISingleThreadEnumerable<Func<IReferenceObject>> Instances { get; set; }

		private static readonly Lazy<ICollection<ITreeItem>> CachedEmptyLazy = new Lazy<ICollection<ITreeItem>>(() => new ObservableCollection<ITreeItem>());
		private static readonly ICommand ShowInstancesCommand = new ShowInstancesCommand();
		public TypeTreeItem(ITypeNode typeNode)
		{
			Name = typeNode.Name;
			ItemDoubleClick = ShowInstancesCommand;
			var typeLeaf = typeNode as TypeLeaf;
			if (typeLeaf != null)
			{
				Instances = typeLeaf.Instances;
			}
			var typeHierarchy = typeNode as TypeHierarchy;
			LazyChildren = typeHierarchy != null
				? new Lazy<ICollection<ITreeItem>>(() => GetChildrenTreeItems(typeHierarchy))
				: CachedEmptyLazy;
		}

		private static ObservableCollection<ITreeItem> GetChildrenTreeItems(TypeHierarchy typeHierarchy)
		{
			var typeTreeItems = typeHierarchy.Elements.ToArray().Select(e => new TypeTreeItem(e));

			return new ObservableCollection<ITreeItem>(typeTreeItems);
		}

		public ICommand OpenObjectViewCommand
		{
			get { return new OpenObjectViewCommand(); }
		}
	}
}


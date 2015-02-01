using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Porter.Models;
using PorterApp.Command;

namespace PorterApp.ViewModel
{
	public sealed class TypeTreeItem : TreeItem
	{

		public ICommand ToRemoveOnlyForTest
		{
			get { return new ShowInstancesCommand(); }
		}

		public IEnumerable<Func<IReferenceObject>> Instances { get; set; }

		private static readonly Lazy<ICollection<TreeItem>> CachedEmptyLazy = new Lazy<ICollection<TreeItem>>(() => new ObservableCollection<TreeItem>());

		public TypeTreeItem(ITypeNode typeNode)
		{
			Name = typeNode.Name;
			var typeLeaf = typeNode as TypeLeaf;
			if (typeLeaf != null)
			{
				Instances = typeLeaf.Instances;
			}
			var typeHierarchy = typeNode as TypeHierarchy;
			LazyChildren = typeHierarchy != null
				? new Lazy<ICollection<TreeItem>>(() => GetChildrenTreeItems(typeHierarchy))
				: CachedEmptyLazy;
		}

		private static ObservableCollection<TreeItem> GetChildrenTreeItems(TypeHierarchy typeHierarchy)
		{
			var typeTreeItems = typeHierarchy.Elements.ToArray().Select(e => new TypeTreeItem(e));

			return new ObservableCollection<TreeItem>(typeTreeItems);
		}

		public ICommand OpenObjectViewCommand
		{
			get { return new OpenObjectViewCommand(); }
		}
	}
}


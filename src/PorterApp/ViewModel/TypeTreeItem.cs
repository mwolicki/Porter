using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Porter.Models;

namespace PorterApp.ViewModel
{
	public sealed class TypeTreeItem : TreeItem
	{
		private static readonly Lazy<ICollection<TreeItem>> CachedEmptyLazy = new Lazy<ICollection<TreeItem>>(() => new ObservableCollection<TreeItem>());

		public TypeTreeItem(ITypeNode typeNode)
		{
			Name = typeNode.Name;
			var typeHierarchy = typeNode as TypeHierarchy;
			LazyChildren = typeHierarchy != null
				? new Lazy<ICollection<TreeItem>>(() => GetChildrenTreeItems(typeHierarchy))
				: CachedEmptyLazy;
		}

		private static ObservableCollection<TreeItem> GetChildrenTreeItems(TypeHierarchy typeHierarchy)
		{
			return new ObservableCollection<TreeItem>(typeHierarchy.Elements().Select(e => new TypeTreeItem(e)));
		}
	}
}

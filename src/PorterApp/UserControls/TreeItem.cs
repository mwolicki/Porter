using System;
using System.Collections.Generic;
using System.Windows.Input;
using PorterApp.Command;

namespace PorterApp.UserControls
{
	public interface ITreeItem
	{
		string Name { get; set; }
		ICollection<ITreeItem> Children { get; }
		ICommand ItemDoubleClick { get; set; }
	}

	public abstract class TreeItem<T> : ITreeItem where T : class, ITreeItem
	{
		public string Name { get; set; }

		public ICollection<ITreeItem> Children
		{
			get
			{
				var lazy = LazyChildren;
				if(lazy!=null)
					return lazy.Value;
				return null;
			}
		}

		public Lazy<ICollection<ITreeItem>> LazyChildren { private get; set; }

		public ICommand ItemDoubleClick { get; set; }
	}
}
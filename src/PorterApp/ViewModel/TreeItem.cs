using System;
using System.Collections.Generic;

namespace PorterApp.ViewModel
{
	public class TreeItem
	{
		public string Name { get; set; }

		public ICollection<TreeItem> Children
		{
			get { return LazyChildren.Value; }
		}

		public Lazy<ICollection<TreeItem>> LazyChildren {private get; set; }
	}
}
using System;
using System.Collections.Generic;

namespace PorterApp.ViewModel
{
	public class TreeItem
	{
		public string Name { get; set; }

		public ICollection<TreeItem> Children
		{
			get
			{
				var lazy = LazyChildren;
				if(lazy!=null)
					return lazy.Value;
				return null;
			}
		}

		public Lazy<ICollection<TreeItem>> LazyChildren {private get; set; }
	}
}
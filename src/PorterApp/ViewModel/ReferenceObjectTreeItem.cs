using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Porter.Models;

namespace PorterApp.ViewModel
{
	public class ReferenceObjectTreeItem : TreeItem
	{
		public ReferenceObjectTreeItem(Func<IFieldData> value)
		{
			IFieldData fieldData = value();
			var refObj = fieldData.ReferenceObject();

			LazyChildren = new Lazy<ICollection<TreeItem>>(()=> new ObservableCollection<TreeItem>(refObj.Fields.Select(p => new ReferenceObjectTreeItem(p.Value))));

			Name = fieldData.Name + " : " + refObj.TypeObjectDescription.Name + " (" + refObj.Value + ")";
		}
	}
}
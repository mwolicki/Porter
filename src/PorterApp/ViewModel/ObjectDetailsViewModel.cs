using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Porter;
using Porter.Models;

namespace PorterApp.ViewModel
{
	internal sealed class ObjectDetailsViewModel :NotifyPropertyChange
	{
		private ObservableCollection<DataItem> _referencedObjects;
		public IReferenceObject ReferenceObject { get; set; }

		public ObservableCollection<DataItem> ReferencedObjects
		{
			get { return _referencedObjects; }
			
		}

		public ObjectDetailsViewModel(IReferenceObject referenceObject)
		{
			ReferenceObject = referenceObject;

			var dis = new ObservableCollection<DataItem>();

			foreach (var field in referenceObject.Fields)
			{
				dis.Add(new DataItem(field.Value));
			}
			


			_referencedObjects = dis;
		}
	}

	public class DataItem
	{
		private IReferenceObject _referenceObject;

		public DataItem(Func<IReferenceObject> value)
		{
			_referenceObject = value();
			Name = _referenceObject.TypeObjectDescription.Name;
		}

		public string Name
		{
			get;
			set;
		}

		public ICollection<DataItem> Children
		{
			get
			{
				var dis = new ObservableCollection<DataItem>();
				foreach (var field in _referenceObject.Fields)
				{
					dis.Add(new DataItem(field.Value));
				}
				return dis;
			}
			
		}

		public Brush Background
		{
			get;
			set;
		}
	}
}

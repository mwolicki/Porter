using System.Collections.ObjectModel;
using System.Linq;
using Porter.Models;

namespace PorterApp.ViewModel
{
	internal sealed class ObjectDetailsViewModel :NotifyPropertyChange
	{
		private readonly ObservableCollection<ReferenceObjectTreeItem> _referencedObjects;
		public IReferenceObject ReferenceObject { get; set; }

		public ObservableCollection<ReferenceObjectTreeItem> ReferencedObjects
		{
			get { return _referencedObjects; }
		}

		public ObjectDetailsViewModel(IReferenceObject referenceObject)
		{
			ReferenceObject = referenceObject;
			_referencedObjects =
				new ObservableCollection<ReferenceObjectTreeItem>(referenceObject.Fields.Select(p => new ReferenceObjectTreeItem(p.Value)));
		}
	}
}

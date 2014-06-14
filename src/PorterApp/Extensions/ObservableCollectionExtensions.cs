using System;
using System.Collections.ObjectModel;
using Porter.Models;
using PorterApp.ViewModel;

namespace PorterApp.Extensions
{
	internal static class ObservableCollectionExtensions
	{
		public static void AddHeapObject(this ObservableCollection<ObjectViewModel> objectViewModels, Func<IReferenceObject> heapObjects)
		{
			IReferenceObject referenceObject = heapObjects();
			objectViewModels.Add(new ObjectViewModel
			{
				Name = referenceObject.TypeObjectDescription.Name,
				Size = referenceObject.Size,
				ObjectRef = referenceObject.ObjectRef
			});
		}
	}
}
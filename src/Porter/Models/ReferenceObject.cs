using System;

namespace Porter.Models
{
	internal sealed class ReferenceObject : IReferenceObject
	{
		public ulong ObjectRef { get; set; }

		public ITypeDescription TypeObjectDescription { get; set; }

		public IMultiElementDictionary<string, Func<IFieldData>> Fields { get; set; }

		public ulong Size { get; set; }
		public string Value { get; set; }
	}
}
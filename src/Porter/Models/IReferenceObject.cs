using System;

namespace Porter.Models
{
	public interface IReferenceObject
	{
		ulong ObjectRef { get; }

		ITypeDescription TypeObjectDescription { get; }

		IMultiElementDictionary<string, Func<IFieldData>> Fields { get; }

		ulong Size { get; }
		string Value { get; set; }
	}
}
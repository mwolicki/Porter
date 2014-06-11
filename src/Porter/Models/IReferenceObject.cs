using System;

namespace Porter.Models
{
	public interface IReferenceObject
	{
		ulong ObjectRef { get; }

		ITypeDescription TypeObjectDescription { get; }

		IMultiElementDictionary<string, Func<IReferenceObject>> Fields { get; }

		ulong Size { get; }
	}
}
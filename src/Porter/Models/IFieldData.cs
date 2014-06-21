using System;

namespace Porter.Models
{
	public interface IFieldData
	{
		Func<IReferenceObject> ReferenceObject { get; }
		string Name { get; }
	}
}
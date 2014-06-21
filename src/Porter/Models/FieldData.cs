using System;

namespace Porter.Models
{
	internal class FieldData : IFieldData
	{
		public Func<IReferenceObject> ReferenceObject { get; internal set; }
		public string Name { get; internal set; }
	}
}
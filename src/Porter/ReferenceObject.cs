using System;

namespace Porter
{
	public sealed class ReferenceObject
	{
		public ulong Type { get; private set; }

		public TypeDescription TypeObjectDescription { get; private set; }

		public MultiElementDictionary<string, Func<ReferenceObject>> Fields { get; private set; }

		public ReferenceObject(ulong type, TypeDescription typeObjectDescription, MultiElementDictionary<string, Func<ReferenceObject>> objectFields)
		{
			Type = type;
			TypeObjectDescription = typeObjectDescription;
			Fields = objectFields;
		}
	}
}
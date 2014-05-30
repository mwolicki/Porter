using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Porter.Extensions
{
	internal static class ClrTypeExtensions
	{
		public static Func<ReferenceObject> GetReferenceObjectFactory(this ClrType type, ulong objRef)
		{
			return () => new ReferenceObject(objRef, type.GetTypeObjectDescription(), type.GetObjectFieldsReferences(objRef));
		}

		private static TypeDescription GetTypeObjectDescription(this ClrType type)
		{
			IEnumerable<string> methods = type.Methods.Select(m => m.Name);

			var typeObjectDescription = new TypeDescription(type.Name, type.Fields.Select(f => f.Name), methods);
			return typeObjectDescription;
		}

		private static MultiElementDictionary<string, Func<ReferenceObject>> GetObjectFieldsReferences(this ClrType type,
			ulong objRef)
		{
			var objectFields = new MultiElementDictionary<string, Func<ReferenceObject>>();
			foreach (string field in type.Fields.Select(f => f.Name))
			{
				var fieldRef = type.GetFieldByName(field);
				objectFields.Add(field, GetReferenceObjectFactory(fieldRef.Type, fieldRef.GetFieldAddress(objRef)));
			}
			return objectFields;
		}
	}
}
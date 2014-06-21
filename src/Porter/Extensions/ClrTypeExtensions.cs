using Microsoft.Diagnostics.Runtime;
using Porter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Porter.Extensions
{
	internal static class ClrTypeExtensions
	{
		public static Func<IReferenceObject> GetReferenceObjectFactory(this ClrType type, ulong objRef)
		{
			return () => new ReferenceObject
			{
				Size = type.GetSize(objRef),
				Fields = type.GetObjectFieldsReferences(objRef),
				TypeObjectDescription = type.GetTypeObjectDescription(),
				ObjectRef = objRef,
			};
		}

		private static ITypeDescription GetTypeObjectDescription(this ClrType type)
		{
			IEnumerable<string> methods = type.Methods.Select(m => m.Name);

			var typeObjectDescription = new TypeDescription
			{
				Fields = type.Fields.Select(f => f.Name),
				Name = type.Name,
				Methods = methods
			};
			return typeObjectDescription;
		}

		private static IMultiElementDictionary<string, Func<IReferenceObject>> GetObjectFieldsReferences(this ClrType type,
			ulong objRef)
		{
			var objectFields = new MultiElementDictionary<string, Func<IReferenceObject>>();
			foreach (string field in type.Fields.Select(f => f.Name))
			{
				if (!type.IsPrimitive)
				{
					var fieldRef = type.GetFieldByName(field);
					objectFields.Add(field, GetReferenceObjectFactory(fieldRef.Type, fieldRef.GetFieldAddress(objRef)));
				}
			}
			return objectFields;
		}
	}
}
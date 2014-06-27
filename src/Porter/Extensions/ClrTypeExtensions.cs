using System.Diagnostics;
using Microsoft.Diagnostics.Runtime;
using Porter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Porter.Extensions
{
	internal static class ClrTypeExtensions
	{
		public static Func<IReferenceObject> GetReferenceObjectFactory(this ClrType type, ulong objRef, string value = "", bool isInterior = true)
		{
			return () => new ReferenceObject
			{
				Size = type.GetSize(objRef),
				Fields = type.GetObjectFieldsReferences(objRef, isInterior),
				TypeObjectDescription = type.GetTypeObjectDescription(),
				ObjectRef = objRef,
				Value = value
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

		private static IMultiElementDictionary<string, Func<IFieldData>> GetObjectFieldsReferences(this ClrType type,
			ulong objRef, bool isInterior)
		{
			var objectFields = new MultiElementDictionary<string, Func<IFieldData>>();
			if (type.Name == "ConsoleApplication18.Program+Test")
			{
				
			}
			if (!type.IsPrimitive && !type.IsString)
			{
				bool interior = !type.IsObjectReference;

				if (isInterior)
				{
					interior = false;
				}

				foreach (string field in type.Fields.Select(f => f.Name))
				{
					
					var fieldRef = type.GetFieldByName(field);
					string fieldName = field;
					objectFields.Add(field, () =>
					{
						return FieldData(type, objRef, fieldRef, interior, fieldName);
					});
				}
			}
			return objectFields;
		}

		private static IFieldData FieldData(ClrType type, ulong objRef, ClrInstanceField fieldRef, bool interior,
			string fieldName)
		{
			string fieldValue = string.Empty;
			if (fieldRef.HasSimpleValue)
			{
				var f1 = fieldRef.GetFieldValue(objRef, interior);
				if (f1 != null)
				{
					fieldValue = f1.ToString();
				}
			}

			var address = fieldRef.GetFieldAddress(objRef, interior);

			ClrType fieldType = fieldRef.Type;

			if (!fieldRef.Type.IsArray)
			{
				if (fieldRef.IsObjectReference() && fieldRef.ElementType != ClrElementType.String)
				{
					address = (ulong) fieldRef.GetFieldValue(objRef);
					fieldType = type.Heap.GetObjectType(address);
				}
			}


			if (fieldType == null)
			{
				
			}

			return new FieldData
			{
				Name = fieldName,
				ReferenceObject = GetReferenceObjectFactory(fieldType, address, fieldValue, false)
			};
		}
	}
}
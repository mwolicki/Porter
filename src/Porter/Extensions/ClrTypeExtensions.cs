using System.Linq.Expressions;
using Microsoft.Diagnostics.Runtime;
using Porter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Porter.Extensions
{
	internal static class ClrTypeExtensions
	{
		public static Func<IReferenceObject> GetReferenceObjectFactory(this ClrType type, ulong objRef, string value = "")
		{
			return () => new ReferenceObject
			{
				Size = type.GetSize(objRef),
				Fields = type.GetObjectFieldsReferences(objRef),
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
			ulong objRef)
		{
			var objectFields = new MultiElementDictionary<string, Func<IFieldData>>();
			if (type.Name == "ConsoleApplication18.Program+Test")
			{
				
			}

			if (!type.IsPrimitive && !type.IsString)
			{
				foreach (string field in type.Fields.Select(f => f.Name))
				{
					
					var fieldRef = type.GetFieldByName(field);
					string fieldName = field;
					objectFields.Add(field, () =>
					{
						string fieldValue = string.Empty;
						if (fieldRef.HasSimpleValue)
						{
							var f1 = fieldRef.GetFieldValue(objRef, !type.IsObjectReference);
							if (f1 != null)
							{
								fieldValue = f1.ToString();
							}
						}

						var address = fieldRef.GetFieldAddress(objRef, !type.IsObjectReference);

						if (fieldRef.IsObjectReference() && fieldRef.ElementType != ClrElementType.String)
						{
							address = (ulong)fieldRef.GetFieldValue(objRef);
						}

						return new FieldData
						{
							Name = fieldName,
							ReferenceObject = GetReferenceObjectFactory(fieldRef.Type, address, fieldValue)
						};
					});
				}
			}
			return objectFields;
		}
	}
}
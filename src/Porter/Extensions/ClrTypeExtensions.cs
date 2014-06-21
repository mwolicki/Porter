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
		public static Func<IReferenceObject> GetReferenceObjectFactory(this ClrType type, ulong objRef)
		{

			//var t = type.GetFieldByName("X");
			return () =>
			{
				string value;
				if (type.IsPrimitive && type.HasSimpleValue)
					value = type.GetValue(objRef).ToString();
				else value = "";
				return new ReferenceObject
				             {
					             Size = type.GetSize(objRef),
					             Fields = type.GetObjectFieldsReferences(objRef),
					             TypeObjectDescription = type.GetTypeObjectDescription(),
					             ObjectRef = objRef,
					             Value = value
				             };
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
			foreach (string field in type.Fields.Select(f => f.Name))
			{
				if (!type.IsPrimitive)
				{
					var fieldRef = type.GetFieldByName(field);
					string fieldName = field;
					objectFields.Add(field, ()=>
					{
						if (fieldRef.IsPrimitive())
						{
							object fieldValue = fieldRef.GetFieldValue(objRef, false);
						}
						return new FieldData
						{
							Name = fieldName,
							ReferenceObject = GetReferenceObjectFactory(fieldRef.Type, fieldRef.GetFieldAddress(objRef))
						};
					});
				}
			}
			return objectFields;
		}
	}
}
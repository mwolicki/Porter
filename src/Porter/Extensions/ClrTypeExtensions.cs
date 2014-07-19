using Microsoft.Diagnostics.Runtime;
using Porter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Porter.Extensions
{
	internal static class ClrTypeExtensions
	{
		private const int Null = 0;

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
			return !type.IsPrimitive && !type.IsString
				? GetObjectFields(type, objRef, isInterior)
				: new MultiElementDictionary<string, Func<IFieldData>>();
		}

		private static MultiElementDictionary<string, Func<IFieldData>> GetObjectFields(ClrType type, ulong objRef, bool isInterior)
		{
			var objectFields = new MultiElementDictionary<string, Func<IFieldData>>();
			bool interior = !type.IsObjectReference;

			if (isInterior)
			{
				interior = false;
			}

			foreach (var field in type.Fields)
			{
				string fieldName = field.Name;
				ClrInstanceField fieldRef = field;
				objectFields.Add(fieldName, () => GetFieldData(type, objRef, fieldRef, interior, fieldName));
			}

			return objectFields;
		}

		private static IFieldData GetFieldData(ClrType type, ulong objRef, ClrInstanceField fieldRef, bool interior,
			string fieldName)
		{
			ulong address = GetAddress(objRef, fieldRef, interior);
			ClrType fieldType = GetFieldType(type, fieldRef, address);
			string fieldValue = GetFieldValue(objRef, fieldRef, interior, address, fieldType);

			return new FieldData
			{
				Name = fieldName,
				ReferenceObject = GetReferenceObjectFactory(fieldType, address, fieldValue, false)
			};
		}

		private static string GetFieldValue(ulong objRef, ClrInstanceField fieldRef, bool interior, ulong address,
			ClrType fieldType)
		{
			string fieldValue = string.Empty;

			if (fieldRef.HasSimpleValue)
			{
				fieldValue = GetValueTypeValue(objRef, fieldRef, interior);
			}

			if (IsObjectAndNotString(fieldRef) && address != Null && fieldType.HasSimpleValue)
			{
				fieldValue = GetBoxedValueTypeValue(fieldType, address);
			}
			return fieldValue;
		}

		private static ClrType GetFieldType(ClrType type, ClrInstanceField fieldRef, ulong address)
		{
			ClrType fieldType = fieldRef.Type;

			if (address != Null && IsObjectAndNotString(fieldRef))
			{
				fieldType = type.Heap.GetObjectType(address);
			}
			return fieldType;
		}

		private static ulong GetAddress(ulong objRef, ClrInstanceField fieldRef, bool interior)
		{
			var address = fieldRef.GetFieldAddress(objRef, interior);
			if (IsObjectAndNotString(fieldRef))
			{
				address = (ulong)fieldRef.GetFieldValue(objRef);
			}
			return address;
		}

		private static bool IsObjectAndNotString(ClrInstanceField fieldRef)
		{
			return fieldRef.IsObjectReference() && fieldRef.ElementType != ClrElementType.String;
		}

		private static string GetValueTypeValue(ulong objRef, ClrInstanceField fieldRef, bool interior)
		{
			string fieldValue = string.Empty;

			object value = fieldRef.GetFieldValue(objRef, interior);
			if (value != null)
			{
				fieldValue = value.ToString();
			}
			return fieldValue;
		}

		private static string GetBoxedValueTypeValue(ClrType fieldType, ulong address)
		{
			string fieldValue = string.Empty;

			object value = fieldType.GetValue(address);
			if (value != null)
			{
				fieldValue = value.ToString();
			}
			return fieldValue;
		}
	}
}
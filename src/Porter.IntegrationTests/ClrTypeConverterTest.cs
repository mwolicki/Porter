using System.Collections.Generic;
using System.Linq;
using Microsoft.Diagnostics.Runtime;
using Microsoft.Diagnostics.Runtime.Fakes;
using Porter.Diagnostics.Decorator;
using Porter.Extensions;
using Porter.Models;
using Xunit;

namespace Porter.IntegrationTests
{
	public sealed class ClrTypeConverterTest
	{
		private readonly StubClrInstanceField _valueTypeField = new StubClrInstanceField
			{
				NameGet = () => ValueTypeFieldName,
				TypeGet = () => new StubClrType
				{
					FieldsGet = () => new ClrInstanceField[0],
					MethodsGet = () => new List<ClrMethod>(),
				},
				HasSimpleValueGet = () => true,
				GetFieldValueUInt64Boolean = (_, interior) => !interior ? (object)ValueTypeExpectedValue : null,
			};

		private readonly StubClrInstanceField _boxedValueTypeField = new StubClrInstanceField
			{
				NameGet = () => BoxedFieldName,
				TypeGet = () => new StubClrType
				{
					FieldsGet = () => new ClrInstanceField[0],
					MethodsGet = () => new List<ClrMethod>(),
				},
				HasSimpleValueGet = () => true,
				IsObjectReference01 = () => true,
				ElementTypeGet = () => ClrElementType.Object,
				GetFieldValueUInt64 = _ => BoxedObjectAddress,
				GetFieldValueUInt64Boolean = (_,__) => BoxedObjectAddress,

			};

		private readonly StubClrInstanceField _stringField = new StubClrInstanceField
			{
				NameGet = () => StringFieldName,
				TypeGet = () => new StubClrType
				{
					FieldsGet = () => new ClrInstanceField[0],
					MethodsGet = () => new List<ClrMethod>(),
				},
				HasSimpleValueGet = () => true,
				GetFieldValueUInt64Boolean = (_, interior) => !interior ? (object)StringExpectedValue : null,
			};

		private readonly StubClrInstanceField _referenceField = new StubClrInstanceField
		{
			NameGet = () => ReferenceFieldName,
			HasSimpleValueGet = () => false,
			GetFieldAddressUInt64Boolean = (_, interior) => interior ? ReferenceFieldAddress : 0,
			IsObjectReference01 = () => true,
			ElementTypeGet = () => ClrElementType.Object,
			GetFieldValueUInt64 = _ => ReferenceFieldValue
		};
		private readonly StubClrType _type;
		private readonly List<ClrInstanceField> _clrInstanceFields;
		private const string TypeName = "testName";
		private const ulong TypeSize = 14;
		private const string ValueTypeFieldName = "fieldName";
		private const string ValueTypeExpectedValue = "46";
		private const string BoxedValueTypeValue = "64";
		private const string StringExpectedValue = "stringValue";
		private const string StringFieldName = "stringName";
		private const string BoxedFieldName = "boxedName";
		private const string ReferenceFieldName = "objectName";
		private const ulong ReferenceFieldAddress = 34;
		private const ulong ReferenceFieldValue = 56;
		private const ulong BoxedObjectAddress = 68;

		public ClrTypeConverterTest()
		{
			_clrInstanceFields = new List<ClrInstanceField> { _valueTypeField, _stringField, _referenceField, _boxedValueTypeField };
			_type = new StubClrType
			{
				NameGet = () => TypeName,
				GetSizeUInt64 = _ => TypeSize,
				MethodsGet = () => new List<ClrMethod>(),
				FieldsGet = () => _clrInstanceFields,
				HeapGet = () => new StubClrHeap
				{
					GetObjectTypeUInt64 = address =>
					{
						switch (address)
						{
							case ReferenceFieldValue:
								return new StubClrType
								{
									HasSimpleValueGet = () => false,
									FieldsGet = () => new ClrInstanceField[0],
									MethodsGet = () => new List<ClrMethod>(),
								};
							case BoxedObjectAddress:
								return new StubClrType
								{
									HasSimpleValueGet = () => true,
									FieldsGet = () => new ClrInstanceField[0],
									MethodsGet = () => new List<ClrMethod>(),
									GetValueUInt64 = _ => BoxedValueTypeValue
								};
							default:
								return null;
						}
					}
				}
			};
		}

		sealed class StubClrTypeDecorator : IClrTypeDecorator
		{
			private readonly StubClrType _type;

			public StubClrTypeDecorator(StubClrType type)
			{
				_type = type;
			}

			public IClrHeapDecorator Heap
			{
				get { throw new System.NotImplementedException(); }
			}

			public bool IsPrimitive
			{
				get { throw new System.NotImplementedException(); }
			}

			public bool IsString
			{
				get { throw new System.NotImplementedException(); }
			}

			public IEnumerable<ClrMethodDecorator> Methods
			{
				get { throw new System.NotImplementedException(); }
			}

			public bool IsObjectReference
			{
				get { throw new System.NotImplementedException(); }
			}

			public IEnumerable<ClrInstanceFieldDecorator> Fields
			{
				get { throw new System.NotImplementedException(); }
			}

			public string Name
			{
				get { throw new System.NotImplementedException(); }
			}

			public bool HasSimpleValue
			{
				get { throw new System.NotImplementedException(); }
			}

			public ulong GetSize(ulong objRef)
			{
				throw new System.NotImplementedException();
			}

			public object GetValue(ulong address)
			{
				throw new System.NotImplementedException();
			}
		}

		[Fact]
		public void GetTypeName_GivenObjectRef_ReturnCorrectName()
		{
			var result = new StubClrTypeDecorator(_type).GetReferenceObjectFactory(1);
			Assert.Equal(TypeName, result().TypeObjectDescription.Name);
			Assert.Equal(TypeSize, result().Size);
			IFieldData valueTypeFieldData = result().Fields[ValueTypeFieldName]();
			Assert.Equal(ValueTypeFieldName, valueTypeFieldData.Name);
			Assert.Equal(ValueTypeExpectedValue, valueTypeFieldData.ReferenceObject().Value);
			Assert.Equal(StringExpectedValue, result().Fields[StringFieldName]().ReferenceObject().Value);
			Assert.Equal(ReferenceFieldValue, result().Fields[ReferenceFieldName]().ReferenceObject().ObjectRef);
			Assert.Equal(BoxedValueTypeValue, result().Fields[BoxedFieldName]().ReferenceObject().Value);
			Assert.True(result().TypeObjectDescription.Fields.All(p => _clrInstanceFields.Select(k => k.Name).Contains(p)));
		}
	}
}
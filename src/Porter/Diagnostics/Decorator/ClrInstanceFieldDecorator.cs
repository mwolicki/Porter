using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class ClrInstanceFieldDecorator
	{
		private readonly ClrInstanceField _clrInstanceField;
		private readonly ThreadDispatcher _threadDispatcher;

		public ClrInstanceFieldDecorator(ClrInstanceField clrInstanceField, ThreadDispatcher threadDispatcher, IClrHeapDecorator heap)
		{
			_clrInstanceField = clrInstanceField;
			_threadDispatcher = threadDispatcher;

			_threadDispatcher.Process(() =>
			{
				Name = _clrInstanceField.Name;
				HasSimpleValue = _clrInstanceField.HasSimpleValue;
				Type = new ClrTypeDecorator(heap, _threadDispatcher, _clrInstanceField.Type);
				IsObjectAndNotString = _clrInstanceField.IsObjectReference() &&_clrInstanceField.ElementType != ClrElementType.String;
			});
		}

		public string Name { get; private set; }

		public bool HasSimpleValue { get; set; }

		public IClrTypeDecorator Type { get; set; }

		public bool IsObjectAndNotString { get; private set; }

		public object GetFieldValue(ulong objRef, bool interior)
		{
			return _threadDispatcher.Process(() => _clrInstanceField.GetFieldValue(objRef, interior));
		}

		public object GetFieldValue(ulong objRef)
		{
			return _threadDispatcher.Process(() => _clrInstanceField.GetFieldValue(objRef));
		}

		public ulong GetFieldAddress(ulong objRef, bool interior)
		{
			return _threadDispatcher.Process(() => _clrInstanceField.GetFieldAddress(objRef, interior));
		}
	}
}
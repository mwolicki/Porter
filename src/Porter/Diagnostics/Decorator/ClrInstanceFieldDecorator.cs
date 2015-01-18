using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class ClrInstanceFieldDecorator
	{
		private readonly ClrInstanceField _clrInstanceField;
		private readonly ThreadDispatcher _threadDispatcher;
		private readonly IClrHeapDecorator _heap;

		public ClrInstanceFieldDecorator(ClrInstanceField clrInstanceField, ThreadDispatcher threadDispatcher, IClrHeapDecorator heap)
		{
			_clrInstanceField = clrInstanceField;
			_threadDispatcher = threadDispatcher;
			_heap = heap;
		}

		public string Name
		{
			get { return _threadDispatcher.Process(() => _clrInstanceField.Name); }
		}

		public bool HasSimpleValue
		{
			get { return _threadDispatcher.Process(() => _clrInstanceField.HasSimpleValue); }
		}

		public IClrTypeDecorator Type
		{
			get { return _threadDispatcher.Process(() => new ClrTypeDecorator(_heap, _threadDispatcher, _clrInstanceField.Type)); }
		}


		public bool IsObjectAndNotString()
		{
			return
				_threadDispatcher.Process(
					() => _clrInstanceField.IsObjectReference() && _clrInstanceField.ElementType != ClrElementType.String);
		}

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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace Porter.Diagnostics.Decorator
{
	internal class ClrTypeDecorator : IClrTypeDecorator
	{
		private readonly IClrHeapDecorator _clrHeapDecorator;
		private readonly ThreadDispatcher _threadDispatcher;
		private ClrType _clrType;

		public ClrTypeDecorator(IClrHeapDecorator clrHeapDecorator, ThreadDispatcher threadDispatcher, ClrType clrType)
		{
			_clrHeapDecorator = clrHeapDecorator;
			_threadDispatcher = threadDispatcher;
			_clrType = clrType;
		}

		public IClrHeapDecorator Heap
		{
			get { return _clrHeapDecorator; }
		}

		public bool IsPrimitive
		{
			get { return _threadDispatcher.Process(() => _clrType.IsPrimitive); }
		}

		public bool IsString
		{
			get { return _threadDispatcher.Process(() => _clrType.IsString); }
		}

		public IEnumerable<ClrMethodDecorator> Methods
		{
			get { return _threadDispatcher.Process(() => _clrType.Methods.Select(p => new ClrMethodDecorator(p, _threadDispatcher))).Dispatch(_threadDispatcher); }
		}

		public bool IsObjectReference
		{
			get { return _threadDispatcher.Process(() => _clrType.IsObjectReference); }
		}

		public IEnumerable<ClrInstanceFieldDecorator> Fields
		{
			get { return _threadDispatcher.Process(() => _clrType.Fields.Select(p => new ClrInstanceFieldDecorator(p, _threadDispatcher, Heap))).Dispatch(_threadDispatcher); }
		}

		public string Name
		{
			get
			{
				if (_threadDispatcher.IsCurrentThread())
				{
					return _clrType.Name;
				}
				return GetNameUsingDifferentThread();
			}
		}

		private string GetNameUsingDifferentThread()
		{
			return _threadDispatcher.Process(() => _clrType.Name);
		}

		public bool HasSimpleValue
		{
			get { return _threadDispatcher.Process(() => _clrType.HasSimpleValue); }
		}

		public ulong GetSize(ulong objRef)
		{
			return _threadDispatcher.Process(() => _clrType.GetSize(objRef));
		}

		public object GetValue(ulong address)
		{
			return _threadDispatcher.Process(() => _clrType.GetValue(address));
		}
	}
}
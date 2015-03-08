using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Microsoft.Diagnostics.Runtime;
using Porter.Extensions;
using Porter.Models;

namespace Porter.Diagnostics.Decorator
{

	internal class ClrHeapDecorator : IClrHeapDecorator
	{
		private readonly ThreadDispatcher _threadDispatcher;

		internal static class IndexBuilder
		{
			public static MultiElementDictionary2<ulong> Build(ClrHeap heap)
			{

				var result = new MultiElementDictionary2<ulong>();

				foreach (var objRef in heap.EnumerateObjects())
				{
					var type = heap.GetObjectType(objRef);
					result.Add(type.Name, objRef);
				}

				return result;
			}
		}
		
		private readonly ClrHeap _clrHeap;
		private MultiElementDictionary2<ulong> objectIndex;

		public ClrHeapDecorator(ClrRuntime clrRuntime, ThreadDispatcher threadDispatcher)
		{
			_threadDispatcher = threadDispatcher;
			_clrHeap = threadDispatcher.Process(() => clrRuntime.GetHeap());

			

			_threadDispatcher.Process(()=>
			{
				objectIndex = IndexBuilder.Build(_clrHeap);
				GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
			});
		}

		public ThreadDispatcher Dispatcher
		{
			get { return _threadDispatcher; }
		}

		public ISingleThreadEnumerable<Func<IReferenceObject>> GetHeapObjects(string typeName)
		{
			List<ulong> list;
			if (objectIndex.GetDictionary().TryGetValue(typeName, out list) && list.Any())
			{
				var type = _threadDispatcher.Process(() => _clrHeap.GetObjectType(list.First()));
				return list.Select(objRef => new ClrTypeDecorator(this, _threadDispatcher, type).GetReferenceObjectFactory(objRef))
						.Dispatch(_threadDispatcher);
			}
			return Enumerable.Empty<Func<IReferenceObject>>().Dispatch(_threadDispatcher);
		}


		public IEnumerable<ulong> EnumerateObjects()
		{
			return _threadDispatcher.Process(() => _clrHeap.EnumerateObjects()).Dispatch(_threadDispatcher);
		}

		public IClrTypeDecorator GetObjectType(ulong objRef)
		{
			if(_threadDispatcher.IsCurrentThread())
			{
				return new ClrTypeDecorator(this, _threadDispatcher, _clrHeap.GetObjectType(objRef));
			}
			return ClrTypeDecoratorProcess(objRef);
		}

		private IClrTypeDecorator ClrTypeDecoratorProcess(ulong objRef)
		{
			return new ClrTypeDecorator(this, _threadDispatcher, _threadDispatcher.Process(() => _clrHeap.GetObjectType(objRef)));
		}

		public IEnumerable<string> GetTypeNames()
		{
			return objectIndex.GetDictionary().Keys;
		}

		public IEnumerable<IClrTypeDecorator> EnumerateTypes()
		{
			return _threadDispatcher.Process(() => _clrHeap.EnumerateTypes().Select(p => new ClrTypeDecorator(this, _threadDispatcher, p))).Dispatch(_threadDispatcher);
		}
	}
}
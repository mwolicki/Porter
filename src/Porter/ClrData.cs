using Microsoft.Diagnostics.Runtime;
using Porter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Porter
{
	internal sealed class ClrData : IClrData
	{
		private readonly ClrRuntime _clrRuntime;
		private ClrHeap _clrHeap;

		internal ClrData(ClrRuntime clrRuntime)
		{
			_clrRuntime = clrRuntime;
		}

		private ClrHeap ClrHeap
		{
			get { return _clrHeap ?? (_clrHeap = _clrRuntime.GetHeap()); }
		}

		public IEnumerable<Func<ReferenceObject>> GetHeapObjects()
		{
			return from objRef in ClrHeap.EnumerateObjects()
				   let type = ClrHeap.GetObjectType(objRef)
				   select type.GetReferenceObjectFactory(objRef);
		}
	}
}
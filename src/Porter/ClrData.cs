using Microsoft.Diagnostics.Runtime;
using Porter.Extensions;
using Porter.Models;
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

		public IEnumerable<Func<IReferenceObject>> GetHeapObjects()
		{
			return from objRef in ClrHeap.EnumerateObjects()
				   let type = ClrHeap.GetObjectType(objRef)
				   select type.GetReferenceObjectFactory(objRef);
		}

		public IEnumerable<TypeHierarchy> GetTypeHierarchy()
		{
			return from type in ClrHeap.EnumerateTypes()
				let fullName = type.Name.Contains(".") ? type.Name : "(global namespace)." + type.Name
				group type by fullName.Split('.').First()
					into grp
					   select new TypeHierarchy { Namespace = grp.Key, Elements = () => GetTypeHierarchy(grp, 1) };
		}

		private IEnumerable<TypeHierarchy> GetTypeHierarchy(IEnumerable<ClrType> grp, int level)
		{
			return from type in grp
				group type by type.Name.Split('.')[level]
				into grp2
				select new TypeHierarchy {Namespace = grp2.Key, Elements = () => GetTypeHierarchy(grp2, level + 1)};
		}
	}

	public class TypeHierarchy 
	{
		public string Namespace { get; set; }
		public Func<IEnumerable<TypeHierarchy>> Elements { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using Porter.Diagnostics.Decorator;
using Porter.Extensions;
using Porter.Models;

namespace Porter
{
	internal sealed class ClrData : IClrData
	{
		private readonly ClrRuntimeDecorator _clrRuntime;
		private IClrHeapDecorator _clrHeap;

		internal ClrData(ClrRuntimeDecorator clrRuntime)
		{
			_clrRuntime = clrRuntime;
		}

		private IClrHeapDecorator ClrHeap
		{
			get { return _clrHeap ?? (_clrHeap = _clrRuntime.GetHeap()); }
		}

		public IEnumerable<Func<IReferenceObject>> GetHeapObjects()
		{
			return from objRef in ClrHeap.EnumerateObjects()
				   let type = ClrHeap.GetObjectType(objRef)
				   select type.GetReferenceObjectFactory(objRef);
		}

		public IEnumerable<ITypeNode> GetTypeHierarchy()
		{
			return (from name in ClrHeap.GetTypeNames()
					let fullName = name.Contains(".") ? name : "(global namespace)." + name
					group name by fullName.GetSubNamespace(0)
						into grp
						select (ITypeNode)new TypeHierarchy { Name = grp.Key, Elements = GetTypeHierarchy(grp, 1) });
		}

		private IEnumerable<ITypeNode> GetTypeHierarchy(IEnumerable<string> names, uint level)
		{
			var nextLevelElements = new MultiElementDictionary<string, string>();
			foreach (string name in names)
			{
				var type = name;
				if (type.IsLastSubNamespace(level) || type.IsLastSubNamespace(0))
				{
					yield return new TypeLeaf { Name = name, Instances = GetHeapObjects(name) };
				}
				else
				{
					nextLevelElements.Add(name.GetSubNamespace(level), name);
				}
			}

			foreach (var keyValuePair in nextLevelElements.GetDictionary())
			{
				yield return GetTypeHierarchy(level, keyValuePair.Key, keyValuePair.Value);
			}
		}

		private ISingleThreadEnumerable<Func<IReferenceObject>> GetHeapObjects(string typeName)
		{
			return ClrHeap.GetHeapObjects(typeName);
		}

		private TypeHierarchy GetTypeHierarchy(uint level, string name, LinkedList<string> groupedTypesByNamespaceCopy)
		{
			return new TypeHierarchy { Name = name, Elements = GetTypeHierarchy(groupedTypesByNamespaceCopy, level + 1) };
		}
	}

}
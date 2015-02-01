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

		public ISingleThreadEnumerable<ITypeNode> GetTypeHierarchy()
		{
			return (from type in ClrHeap.EnumerateTypes()
						let fullName = type.Name.Contains(".") ? type.Name : "(global namespace)." + type.Name
						group type by fullName.GetSubNamespace(0)
							into grp
						select (ITypeNode)new TypeHierarchy { Name = grp.Key, Elements = GetTypeHierarchy(grp, 1).Dispatch(ClrHeap.Dispatcher) }).Dispatch(ClrHeap.Dispatcher);
		}

		

		private IEnumerable<ITypeNode> GetTypeHierarchy(IEnumerable<IClrTypeDecorator> clrTypes, uint level)
		{
			var nextLevelElements = new MultiElementDictionary<string, IClrTypeDecorator>();
			foreach (IClrTypeDecorator clrType in clrTypes)
			{
				var type = clrType;
				if (type.Name.IsLastSubNamespace(level) || type.Name.IsLastSubNamespace(0))
				{

					yield return new TypeLeaf { Name = type.Name, Instances = GetHeapObjects(type.Name).Dispatch(ClrHeap.Dispatcher) };
				}
				else
				{
					nextLevelElements.Add(type.Name.GetSubNamespace(level), type);
				}
			}

			foreach (var keyValuePair in nextLevelElements.GetDictionary())
			{
				yield return GetTypeHierarchy(level, keyValuePair.Key, keyValuePair.Value);
			}
		}

		private IEnumerable<Func<IReferenceObject>> GetHeapObjects(string typeName)
		{
			return from objRef in ClrHeap.EnumerateObjects()
				let type = ClrHeap.GetObjectType(objRef)
				where type.Name == typeName
				select type.GetReferenceObjectFactory(objRef);
		}

		private TypeHierarchy GetTypeHierarchy(uint level, string name, IEnumerable<IClrTypeDecorator> groupedTypesByNamespaceCopy)
		{
			return new TypeHierarchy { Name = name, Elements = GetTypeHierarchy(groupedTypesByNamespaceCopy, level + 1).Dispatch(ClrHeap.Dispatcher) };
		}
	}

}
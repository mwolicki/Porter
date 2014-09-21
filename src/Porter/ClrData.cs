using System.Runtime.CompilerServices;
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

		public IEnumerable<ITypeNode> GetTypeHierarchy()
		{
			return from type in ClrHeap.EnumerateTypes()
				let fullName = type.Name.Contains(".") ? type.Name : "(global namespace)." + type.Name
				group type by fullName.Split('.').First()
					into grp
					   select new TypeHierarchy { Name = grp.Key, Elements = () => GetTypeHierarchy(grp, 1) };
		}

		private IEnumerable<ITypeNode> GetTypeHierarchy(IEnumerable<ClrType> clrTypes, int level)
		{
			var nextLevelElements = new MultiElementDictionary<string, ClrType>();
			foreach (ClrType clrType in clrTypes)
			{
				if (ContainsCharExpectedNumberTimes(clrType.Name, '.', level))
				{
					yield return new TypeLeaf { Name = clrType.Name };
				}
				else
				{
					var x = clrType.Name.Split('.')[level];
					nextLevelElements.Add(x, clrType);
				}
			}

			foreach (var keyValuePair in nextLevelElements.GetDictionary())
			{
				yield return GetTypeHierarchy(level, keyValuePair.Key, keyValuePair.Value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool ContainsCharExpectedNumberTimes(string s, char c, int number)
		{
			var arr = s.ToCharArray();
			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] == c)
				{
					if (--number == -1)
						return false;
				}
			}
			return number == 0;
		}

		private TypeHierarchy GetTypeHierarchy(int level, string name, IEnumerable<ClrType> groupedTypesByNamespaceCopy)
		{
			return new TypeHierarchy { Name = name, Elements = () => GetTypeHierarchy(groupedTypesByNamespaceCopy, level + 1) };
		}
	}
}
using System.Collections.ObjectModel;
using Porter;
using Porter.Models;
using System;
using System.Collections.Generic;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class NotEmptyDebuggerStub : IExtendedDebugger
	{
		public Architecture Architecture { get; private set; }

		internal static IEnumerable<ObjectViewModel> ExpectedObjectViewModels
		{
			get
			{
				return new[]
				{
					new ObjectViewModel {Name = "a", Size = 1},
					new ObjectViewModel {Name = "b", Size = 2},
					new ObjectViewModel {Name = "c", Size = 3}
				};
			}
		}

		public IEnumerable<IClrData> GetClrs()
		{
			Func<string, ulong, Func<IReferenceObject>> refObj = (name, size) => (() => new ReferenceObject(name, size));
			return new List<IClrData>
			{
				new ClrData(new[] { refObj("a", 1), refObj("b", 2) }),
				new ClrData(new[] { refObj("c", 3) })
			};
		}

		public void Dispose()
		{
		}

		private sealed class ClrData : IClrData
		{
			private readonly IEnumerable<Func<IReferenceObject>> _heapObjects;

			public ClrData(IEnumerable<Func<IReferenceObject>> heapObjects)
			{
				_heapObjects = heapObjects;
			}

			public IEnumerable<Func<IReferenceObject>> GetHeapObjects()
			{
				return _heapObjects;
			}
		}

		private sealed class ReferenceObject : IReferenceObject
		{
			public ulong Type { get; private set; }

			public ITypeDescription TypeObjectDescription { get; private set; }

			public IMultiElementDictionary<string, Func<IReferenceObject>> Fields { get; private set; }

			public ulong Size { get; private set; }

			public ReferenceObject(string typeName, ulong size)
			{
				TypeObjectDescription = new TypeDescription { Name = typeName };
				Size = size;
			}
		}

		private sealed class TypeDescription : ITypeDescription
		{
			public string Name { get; set; }

			public IEnumerable<string> Fields { get; private set; }

			public IEnumerable<string> Methods { get; private set; }
		}
	}
}
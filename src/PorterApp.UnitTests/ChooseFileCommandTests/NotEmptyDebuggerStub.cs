using Porter;
using Porter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PorterApp.ViewModel;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{

	//TODO: FIX after adding ASYNC...
	public sealed class NotEmptyDebuggerStub : IExtendedDebugger
	{
		private static readonly TypeHierarchy GetTypeHierarchy = new TypeHierarchy
		{
			Name = "test",
			//Elements = new[] {new TypeHierarchy()}
		};

		public ArchitectureType ArchitectureType { get; private set; }

		internal static IEnumerable<TreeItem> ExpectedObjectViewModels
		{
			get
			{
				return new[]
				{
					new TypeTreeItem(GetTypeHierarchy), 
				};
			}
		}

		public ArchitectureType Architecture
		{
			get { throw new System.NotImplementedException(); }
		}

		IEnumerable<IClrData> IExtendedDebugger.GetClrs()
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<IClrData> GetClrs()
		{
			return new List<IClrData>
			{
				new ClrData()
			};
		}

		//FIX after adding ASYNC
		private sealed class ClrData : IClrData
		{

			public ISingleThreadEnumerable<ITypeNode> GetTypeHierarchy()
			{
				throw new System.NotImplementedException();
			}

			public Task<ITypeNode[]> GetTypeHierarchyAsync()
			{
				throw new System.NotImplementedException();
			}
		}

		public void Dispose() {}
	}
}
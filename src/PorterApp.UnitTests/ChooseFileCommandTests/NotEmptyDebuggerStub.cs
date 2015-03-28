using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Porter;
using Porter.Diagnostics.Decorator;
using Porter.Models;
using PorterApp.UserControls;
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

		internal static IEnumerable<ITreeItem> ExpectedObjectViewModels
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
			get { throw new NotImplementedException(); }
		}

		IEnumerable<IClrData> IExtendedDebugger.GetClrs()
		{
			throw new NotImplementedException();
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

			public IEnumerable<ITypeNode> GetTypeHierarchy()
			{
				throw new NotImplementedException();
			}

			public Task<ITypeNode[]> GetTypeHierarchyAsync()
			{
				throw new NotImplementedException();
			}
		}

		public void Dispose() {}
	}
}
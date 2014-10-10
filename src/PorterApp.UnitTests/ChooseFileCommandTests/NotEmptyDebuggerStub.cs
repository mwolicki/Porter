using Porter;
using Porter.Models;
using System.Collections.Generic;
using PorterApp.ViewModel;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class NotEmptyDebuggerStub : IExtendedDebugger
	{
		private static readonly TypeHierarchy GetTypeHierarchy = new TypeHierarchy
		{
			Name = "test",
			Elements = () => new[] {new TypeHierarchy()}
		};

		public Architecture Architecture { get; private set; }

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

		public IEnumerable<IClrData> GetClrs()
		{
			return new List<IClrData>
			{
				new ClrData()
			};
		}

		private sealed class ClrData : IClrData
		{

			public IEnumerable<ITypeNode> GetTypeHierarchy()
			{
				yield return NotEmptyDebuggerStub.GetTypeHierarchy;
			}
		}

		public void Dispose() {}
	}
}
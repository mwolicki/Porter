using Porter.Models;
using Porter.Extensions;
using SharpCompress.Archive;
using SharpCompress.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Porter.IntegrationTests
{
	public sealed class SosDllIntegrationTests
	{
		static SosDllIntegrationTests()
		{
			if (!File.Exists(Path.GetTempPath() + _dumpFileName))
			{
				using (IArchive archive = ArchiveFactory.Open("ExampleDumps/ExampleConsoleApp.vshost.7z"))
				{
					archive.WriteToDirectory(Path.GetTempPath(), ExtractOptions.Overwrite);
				}
			}
		}

		public SosDllIntegrationTests()
		{
			_debugger = new ExtendedDebugger(Path.GetTempPath() + _dumpFileName);
			_clrData = _debugger.GetClrs().Single();
		}

		[Fact]
		public void TestGetArchitecture()
		{
			Assert.Equal(Architecture.X86, _debugger.Architecture);
		}

		[Fact]
		public void TestGetAnyHeapObjects()
		{
			Assert.Equal(1882, _clrData.GetHeapObjects().Count());
		}

		[Fact]
		public void TestReferenceToObject_CanNavigateToNextObject()
		{
			var examplePublicClassFactory = _clrData.GetHeapObjects().FirstOrDefault(p => p().TypeObjectDescription.Name.Contains("ExamplePublicClass"));
			Debug.Assert(examplePublicClassFactory != null, "examplePublicClassFactory != null");
			var field = examplePublicClassFactory().Fields["<Name>k__BackingField"];
			Assert.NotNull(field());
		}

		[Fact]
		public void TestGetFirstLevelOfTypeHierarchy()
		{
			var typeHierarchy = _clrData.GetTypeHierarchy().Select(t => t.Name).ToArray();

			Assert.Contains("System", typeHierarchy);
			Assert.Contains("(global namespace)", typeHierarchy);
			Assert.Contains("MS", typeHierarchy);
			Assert.Contains("Microsoft", typeHierarchy);
			Assert.Contains("Windows", typeHierarchy);
			Assert.Contains("ExampleConsoleApp", typeHierarchy);
		}


		[Fact]
		public void TestGet2ndLevelOfTypeHierarchy()
		{
			var typeHierarchy = _clrData.GetTypeHierarchy().Where(p => p.Name == "System")
				.OfType<TypeHierarchy>()
				.SelectMany(p => p.Elements()).Select(p => p.Name).ToArray();

			var typeHierarchy2 = _clrData.GetTypeHierarchy().Where(p => p.Name == "System")
				.OfType<TypeHierarchy>()
				.SelectMany(p => p.Elements()).OfType<TypeHierarchy>().SelectMany(p=>p.Elements()).ToArray();

			


			Assert.Contains("System.Object", typeHierarchy);
			Assert.Contains("System.String", typeHierarchy);

		}

		[Theory]
		[InlineData("asdfd0-#adsf", 0u)]
		[InlineData("er werewtr", 1u)]
		[InlineData("gąśćdsgdfg", 2u)]
		[InlineData("retrt", 3u)]
		[InlineData("vdf迪dfg", 4u)]
		[InlineData("", 5u)]
		[InlineData("", 6u)]
		[InlineData(null, 7u)]
		[InlineData(null, 100u)]
		public void GetTextAfterDot(string expectedText, uint position)
		{
			var text = "asdfd0-#adsf.er werewtr.gąśćdsgdfg.retrt.vdf迪dfg..";
			Assert.Equal(expectedText, text.GetSubNamespace(position));
		}

		[Theory]
		[InlineData(false, 0u)]
		[InlineData(false, 1u)]
		[InlineData(false, 2u)]
		[InlineData(false, 3u)]
		[InlineData(false, 4u)]
		[InlineData(false, 5u)]
		[InlineData(true, 6u)]
		[InlineData(false, 7u)]
		[InlineData(false, 100u)]
		public void Count(bool expected, uint position)
		{
			var text = "asdfd0-#adsf.er werewtr.gąśćdsgdfg.retrt.vdf迪dfg..";
			Assert.Equal(expected, text.IsLastSubNamespace(position));
		}


		private readonly ExtendedDebugger _debugger;

		private readonly IClrData _clrData;
		private const string _dumpFileName = @"ExampleConsoleApp.vshost.dmp";
		private const int iter = 100000;
	}
}
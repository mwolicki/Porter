﻿using SharpCompress.Archive;
using SharpCompress.Common;
using SharpCompress.Reader;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Porter.IntegrationTests
{
	public sealed class SosDllIntegrationTests
	{

		static SosDllIntegrationTests()
		{
			if (!File.Exists(Path.GetTempPath() + _dumpFileName))
			{
				IArchive archive = ArchiveFactory.Open("ExampleDumps/ExampleConsoleApp.vshost.7z");
				archive.WriteToDirectory(Path.GetTempPath() , ExtractOptions.Overwrite);
			}
		}

		public SosDllIntegrationTests()
		{
			

			_debugger = new ExtendedDebugger(Path.GetTempPath() + _dumpFileName);
			_clrData = _debugger.GetClrs().Single();
		}

		private readonly ExtendedDebugger _debugger;

		private readonly IClrData _clrData;
		private static string _dumpFileName = @"ExampleConsoleApp.vshost.dmp";

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
	}
}
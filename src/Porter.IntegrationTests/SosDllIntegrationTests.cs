using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Porter.IntegrationTests
{
	public sealed class SosDllIntegrationTests
	{
		public SosDllIntegrationTests()
		{
			_clrData = _debugger.GetClrs().Single();
		}

		private readonly ExtendedDebugger _debugger = new ExtendedDebugger("ExampleDumps/ExampleConsoleApp.vshost.dmp");

		private readonly IClrData _clrData;

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
			Func<ReferenceObject> examplePublicClassFactory = _clrData.GetHeapObjects().FirstOrDefault(p => p().TypeObjectDescription.Name.Contains("ExamplePublicClass"));
			Debug.Assert(examplePublicClassFactory != null, "examplePublicClassFactory != null");
			var field = examplePublicClassFactory().Fields["<Name>k__BackingField"];
			Assert.NotNull(field());
		}
	}
}
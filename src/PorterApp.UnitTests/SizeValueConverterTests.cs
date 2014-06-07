using Porter;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;

namespace PorterApp.UnitTests
{
	public sealed class SizeValueConverterTests
	{
		[Fact]
		public void Convert_ValueIsNull_ReturnUnknownSize()
		{
			Assert.Equal("(Unknown)", Convert(null));
		}

		[Fact]
		public void Convert_ValueIsNumber_ReturnFormatedString()
		{
			Assert.Equal("1 b", Convert(1));
			Assert.Equal("2 b", Convert(2));
			Assert.Equal("1 B", Convert(1 * 8));
			Assert.Equal("1 KB", Convert(8 * 1024));
			Assert.Equal("1.5 KB", Convert(12 * 1024));
			Assert.Equal("2 KB", Convert(2 * 8 * 1024));
			Assert.Equal("1 MB", Convert(8 * 1024 * 1024));
			Assert.Equal("3 GB", Convert(3L * 8 * 1024 * 1024 * 1024));
		}

		private static object Convert(ulong? value)
		{
			var converter = new SizeValueConverter();

			var convert = converter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);
			return convert;
		}
	}
}
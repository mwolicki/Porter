using System;
using System.Globalization;
using System.Windows.Data;

namespace PorterApp
{
	internal sealed class SizeValueConverter : IValueConverter
	{
		private static readonly string[] SizesPostfix = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is ulong)
			{
				ulong v = (ulong)value;
				if (v < 8)
					return v + " b";

				v /= 8;
				var position = (int)Math.Log(v, 1024);
				var size = v / Math.Pow(1024, position);
				return size + " " + SizesPostfix[position];
			}
			return "(Unknown)";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
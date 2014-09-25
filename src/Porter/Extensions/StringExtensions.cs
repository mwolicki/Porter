﻿using System.Runtime.CompilerServices;

namespace Porter.Extensions
{
	static class StringExtensions
	{
		private const char Separator = '.';

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static unsafe bool ContainsSubNamespace(this string s, uint number)
		{
			var length = s.Length;
			fixed (char* p = s)
				for (int i = 0; i < length && number > 0; i++)
				{
					if (p[i] == Separator)
						--number;
				}
			return number == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static unsafe string GetSubNamespace(this string s, uint position)
		{
			int? startPos = position == 0 ? 0 : default(int?);
			int i = 0;

			fixed (char* p = s)
				for (; i < s.Length; i++)
				{
					if (p[i] == Separator)
					{
						if (position == 1)
						{
							startPos = i + 1;
						}
						else if (position == 0)
						{
							break;
						}
						position--;
					}
				}

			return startPos == null
				? null
				: s.Substring(startPos.Value, i - startPos.Value);
		}
	}
}

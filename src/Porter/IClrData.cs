﻿using Porter.Models;
using System;
using System.Collections.Generic;

namespace Porter
{
	public interface IClrData
	{
		IEnumerable<ITypeNode> GetTypeHierarchy();
	}
}
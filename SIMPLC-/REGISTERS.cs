using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIMPLC_
{
	enum E_REGISTERS
	{
		Unknown = 0,
		A = 4,
		B = 2,
		D = 1,
		X = 16,
		Y = 8
	}
	static class C_REGISTERS
	{
		public const char C_X = 'X';
		public const char C_Y = 'Y';
		public const char C_D = 'D';
		public const char C_A = 'A';
		public const char C_B = 'B';
	}
}

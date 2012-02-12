using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MACHINE.CPU
{
	enum E_REGISTERS
	{
		UNKNOWN = 0,
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

		public static E_REGISTERS F_GET( char r )
		{
			switch ( r )
			{
				case C_REGISTERS.C_A:
					return E_REGISTERS.A;
				case C_REGISTERS.C_B:
					return E_REGISTERS.B;
				case C_REGISTERS.C_D:
					return E_REGISTERS.D;
				case C_REGISTERS.C_X:
					return E_REGISTERS.X;
				case C_REGISTERS.C_Y:
					return E_REGISTERS.Y;
				default:
					return E_REGISTERS.UNKNOWN;
			}
		}
	}
}

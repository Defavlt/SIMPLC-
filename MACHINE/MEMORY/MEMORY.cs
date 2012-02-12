using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MACHINE.MEM
{
	static class MEMORY
	{
		private const ushort _SIZE = 0xFFFF;
		private static byte[] _MAIN;

		public static void Poke( ushort Address, byte Value )
		{
			MEMORY.MAIN[Address] = Value;
		}
		public static byte Peek( ushort Address )
		{
			return MEMORY.MAIN[Address];
		}
		public static bool Validate( ushort Address )
		{
			return (
				Address > 0 &&
				Address < MEMORY.SIZE );
		}

		public static byte[] MAIN
		{
			get
			{
				return MEMORY._MAIN;
			}
			set
			{
				MEMORY._MAIN = value;
			}
		}
		public static ushort SIZE
		{
			get
			{
				return MEMORY._SIZE;
			}
		}
		public static void INIT( )
		{
			MEMORY._MAIN = new byte[MEMORY._SIZE];
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MACHINE
{
	static class CPU
	{
		private static byte REG_A;
		private static byte REG_B;
		private static ushort REG_X;
		private static ushort REG_Y;
		private static ushort REG_D;

		private static ushort StartAddr;
		private static ushort ExecAddr;
		private static ushort InstructionPointer;

		private static void Execute( FileStream stream )
		{
			//Start parsing our .b32-files, thank you :)
			//Use the filestream parameter and a BinaryReader
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DEBUG;
using MACHINE.MEM;

namespace MACHINE.CPU
{
	static class CPU
	{
		private static string MAGIC_HEADER = "SIMPLC-";

		private static byte REG_A;
		private static byte REG_B;
		private static ushort REG_X;
		private static ushort REG_Y;
		private static ushort REG_D;

		private static ushort StartAddr;
		private static ushort ExecAddr;
		private static ushort InstructionPointer;
		private static ushort ProgramLength;

		private static FileStream Source;
		private static BinaryReader Input;

		private static bool Execute( FileStream stream )
		{
			//Start parsing our .b32-files, thank you :)
			//Use the filestream parameter and a BinaryReader
			string header;

			CPU.Source = stream;
			CPU.Input = new BinaryReader(CPU.Source);

			CPU.REG_A = 0;
			CPU.REG_B = 0;
			CPU.REG_D = 0;
			CPU.REG_X = 0;
			CPU.REG_Y = 0;

			CPU.StartAddr = 0;
			CPU.ExecAddr = 0;
			CPU.InstructionPointer = 0;
			CPU.ProgramLength = MEMORY.SIZE;

			header = Encoding.ASCII.GetString(CPU.Input.ReadBytes(3));

			if ( header != CPU.MAGIC_HEADER )
			{
				return false;
			}

			CPU.StartAddr = CPU.Input.ReadUInt16( );
			CPU.ExecAddr = CPU.Input.ReadUInt16( );
			ushort Counter = 0;

			while ( CPU.Input.PeekChar() != -1 )
			{
				CPU.INSERT(CPU.Input.ReadByte( ), (ushort) ( CPU.StartAddr + Counter ));
				Counter++;
			}

			CPU.InstructionPointer = CPU.ExecAddr;
			CPU.ExecuteProgram( );
			return true; //"Success" (of course, it might have been a bad program. Bad stuff goes in, bad stuff comes out)
		}

		private static void ExecuteProgram( )
		{
			while ( CPU.ProgramLength > 0 )
			{
				byte Instruction = MEMORY.Peek(CPU.InstructionPointer);

				CPU.ProgramLength--;

				switch ( Instruction )
				{
					case C_MNEMONIC_VAL.LDA:

						CPU.LDA( );
						continue;

					case C_MNEMONIC_VAL.LDX:

						CPU.LDX( );
						continue;

					case C_MNEMONIC_VAL.STA:

						CPU.STA( );
						continue;

					case C_MNEMONIC_VAL.END:
					default:
						CPU.END( );
						break;
				}
			}
		}

		private static void END( )
		{
			CPU.InstructionPointer++;
		}

		private static void STA( )
		{
			CPU.INSERT(CPU.REG_A, CPU.REG_X);
			CPU.InstructionPointer++;
		}

		private static void LDA( )
		{
			CPU.REG_A = CPU.EXTRACT((ushort) ( CPU.InstructionPointer + 1 ));
			CPU.SetRegisterD( );

			CPU.ProgramLength -= 1;
			CPU.InstructionPointer += 2;
		}

		private static void LDX( )
		{
			CPU.REG_X = (ushort) ( CPU.EXTRACT((ushort) ( CPU.InstructionPointer + 2 )) << 8 );
			CPU.REG_X += CPU.EXTRACT((ushort) ( CPU.InstructionPointer + 1 ));

			CPU.ProgramLength -= 2;
			CPU.InstructionPointer += 3;
		}

		private static void SetRegisterD( )
		{
			CPU.REG_D = (ushort) ( CPU.REG_A << 8 + CPU.REG_B );
		}

		#region Util Methods
		private static byte EXTRACT( ushort MEMLOC )
		{
			if ( CPU.ISBOUND(MEMLOC) )
			{
				return MEMORY.Peek(MEMLOC);
			}
			else
			{
				C_DEBUG.FailedMemoryExtraction(MEMLOC);
				return 0x00;
			}
		}
		private static void INSERT( byte VALUE, ushort MEMLOC )
		{
			if ( CPU.ISBOUND(MEMLOC) )
			{
				MEMORY.Poke(MEMLOC, VALUE);
			}
			else
			{
				
				C_DEBUG.FailedMemoryInsertion(MEMLOC);
				return;
			}
		}
		private static bool ISBOUND( ushort Address )
		{
			return ( Address > 0 && Address < MEMORY.SIZE );
		}
		#endregion
	}
}

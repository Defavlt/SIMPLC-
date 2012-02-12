using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace SIMPLC_
{
	class ASSEMBLER
	{
		private static char[] MAGIC_HEADER = new char[] { 'B', '3', '2' };
		private static ushort MAGIC_START = Convert.ToUInt16("1000", 16);
		private static string MAGIC_FILE_IN = "IN.ASM";
		private static string MAGIC_FILE_OUT = "out.b32";

		//Should it be compile-time or run-time?
		//If compile-time we need to bake assets in project
		private static bool __DEBUG = false;

		#region Private members

		private BinaryWriter output;

		private string Source;
		private ushort AsLength;
		private ushort ExecutionAddress;

		private int CurrentNdx;
		private bool IsEnd;

		private Hashtable LabelTable;

		private char CurrentChr
		{
			get
			{
				return this.Source[this.CurrentNdx];
			}
		}

		#endregion

		public ASSEMBLER(
			string infile, 
			string outfile, 
			string startADDR,
			bool __DEBUG )
		{
			ASSEMBLER.__DEBUG = __DEBUG;
			infile = __DEBUG ? ASSEMBLER.MAGIC_FILE_IN : infile;
			outfile = __DEBUG ? ASSEMBLER.MAGIC_FILE_OUT : infile;
			startADDR = __DEBUG ? "1000" : startADDR;

			this.output = new BinaryWriter(File.Create(outfile));
			this.LabelTable = new Hashtable( );
			this.IsEnd = false;
			this.CurrentNdx = 0;
			this.ExecutionAddress = 0;
			this.Source = null;

			this.AsLength = ASSEMBLER.MAGIC_START;

			using ( StreamReader input = new StreamReader(File.Open(infile, FileMode.Open)) )
			{
				this.Source = input.ReadToEnd( );
				input.Close();
				Console.Write(this.Source);
			}

			this.output.Write(ASSEMBLER.MAGIC_HEADER);
			this.output.Write(this.AsLength);
			this.output.Write((ushort) 0);

			this.Parse( startADDR );

			this.output.Seek(5, SeekOrigin.Begin);
			this.output.Write(this.ExecutionAddress);
			this.output.Close( );
		}

		#region Parsing & Tokenizing

		private void Parse( string startADDR )
		{
			this.CurrentNdx = 0;

			while ( !this.IsEnd )
			{
				this.LabelScan(true);
			}

			this.IsEnd = false;
			this.CurrentNdx = 0;
			this.AsLength = ASSEMBLER.MAGIC_START;

			while ( !this.IsEnd )
			{
				this.LabelScan(false);
			}
		}

		private void LabelScan( bool isLabelScan )
		{
			if ( char.IsLetter(this.CurrentChr) )
			{
				if ( isLabelScan )
				{
					this.LabelTable.Add(
						this.GetLabelName( ),
						this.AsLength);
				}

				while ( this.CurrentChr != '\n' )
				{
					this.CurrentNdx++;
				}

				this.CurrentNdx++;
				return;
			}

			else
			{
				this.EatWhiteSpaces( );
				this.ReadMnemonic(isLabelScan);
			}
		}

		private void ReadMnemonic( bool isLabelScan )
		{
			string mnemonic = null;

			while ( !(char.IsWhiteSpace(this.CurrentChr)) )
			{
				mnemonic += this.CurrentChr;
				this.CurrentNdx++;
			}

			switch ( C_MNEMONICS.F_GET(mnemonic.ToUpper()) )
			{
				case E_MNEMONIC.LDA:
					this.LDA(isLabelScan);
					break;
				case E_MNEMONIC.LDX:
					this.LDX(isLabelScan);
					break;
				case E_MNEMONIC.STA:
					this.STA(isLabelScan);
					break;
				case E_MNEMONIC.END:
					this.END(isLabelScan);
					break;
				case E_MNEMONIC.ADD:
					this.ADD(isLabelScan);
					break;
				default:
					break;
			}

			while ( this.CurrentChr != '\n' )
			{
				this.CurrentNdx++;
			}

			this.CurrentNdx++;
		}

		#endregion

		#region Mnemonics

		/// <summary>
		/// End function and set execution at LABEL
		/// </summary>
		/// <param name="isLabelScan"></param>
		private void END( bool isLabelScan )
		{
			this.IsEnd = true;
			this.DoEnd(isLabelScan);
			this.EatWhiteSpaces( );
			this.ExecutionAddress =
				(ushort) this.LabelTable[this.GetLabelName( )];
			return;
		}

		/// <summary>
		/// Mnemonic: Store register A at location
		/// Interprets mnemonic, value and location
		/// <note>This is first 8 bit of X</note>
		/// </summary>
		/// <param name="isLabelScan"></param>
		private void STA( bool isLabelScan )
		{
			this.EatWhiteSpaces( );

			if ( this.CurrentChr == ',' )
			{
				E_REGISTERS R;
				byte opcode = 0x00;

				this.CurrentNdx++;
				this.EatWhiteSpaces( );

				R = ReadRegister( );

				switch ( R )
				{
					case E_REGISTERS.UNKNOWN:
						break;
					case E_REGISTERS.A:
						break;
					case E_REGISTERS.B:
						break;
					case E_REGISTERS.D:
						break;
					case E_REGISTERS.X:
						opcode = 0x03;
						break;
					case E_REGISTERS.Y:
						break;
					default:
						break;
				}

				this.AsLength += 1;

				if ( !isLabelScan )
				{
					this.output.Write(opcode);
				}
			}
		}

		/// <summary>
		/// Mnemonic: Load Register X
		/// Interprets mnemonic and value
		/// <note>X is 16-bit/2-byte pointer</note>
		/// </summary>
		/// <param name="isLabelScan"></param>
		private void LDX( bool isLabelScan )
		{
			this.EatWhiteSpaces( );

			if ( this.CurrentChr == '#' )
			{
				this.CurrentNdx++;

				ushort val = this.ReadWordValue( );
				this.AsLength += 3;

				if ( !isLabelScan )
				{
					this.output.Write((byte) 0x02);
					this.output.Write(val);
				}
			}
		}

		/// <summary>
		/// Mnemonic: Load register A
		/// Interprets mnemonic and value
		/// </summary>
		/// <param name="isLabelScan"></param>
		private void LDA( bool isLabelScan )
		{
			this.EatWhiteSpaces( );

			if ( this.CurrentChr == '#' )
			{
				this.CurrentNdx++;

				byte val = this.ReadByteValue( );
				this.AsLength += 2;

				if ( !isLabelScan )
				{
					this.output.Write((byte) 0x01);
					this.output.Write(val);
				}
			}
		}

		/// <summary>
		/// Mnemonic: Add value to location
		/// </summary>
		/// <param name="isLabelScan"></param>
		private void ADD( bool isLabelScan )
		{
			this.EatWhiteSpaces( );

			if ( this.CurrentChr == '#' )
			{
				byte val = this.ReadByteValue( );
				this.AsLength += 2;

				if ( !isLabelScan )
				{
					//Not implemented yet, but didn't want to
					//break execution with an exception
					this.output.Write((string) null);
				}
			}
		}

		#endregion

		#region Util methods
		private void DoEnd( bool isLabelScan )
		{
			this.AsLength++;

			if ( !isLabelScan )
			{
				this.output.Write((byte) 0x04);
			}
		}
		private E_REGISTERS ReadRegister( )
		{
			E_REGISTERS R = C_REGISTERS.F_GET(this.CurrentChr);
			this.CurrentNdx++;

			return R;
		}
		private ushort ReadWordValue( )
		{
			ushort val = 0;
			bool isHex = false;
			string sval = null;

			if ( this.CurrentChr == '$' )
			{
				this.CurrentNdx++;
				isHex = true;
			}

			while ( char.IsLetterOrDigit(this.CurrentChr) )
			{
				sval += this.CurrentChr;
				this.CurrentNdx++;
			}

			if ( isHex )
			{
				val = Convert.ToUInt16(sval, 16);
			}

			else
			{
				val = ushort.Parse(sval);
			}

			return val;
		}
		private byte ReadByteValue( )
		{
			byte val = 0;
			bool isHex = false;
			string sval = null;

			if ( this.CurrentChr == '$' )
			{
				this.CurrentNdx++;
				isHex = true;
			}

			while ( char.IsLetterOrDigit(this.CurrentChr) )
			{
				sval += this.CurrentChr;
				this.CurrentNdx++;
			}

			if ( isHex )
			{
				val = Convert.ToByte(sval, 16);
			}

			else
			{
				val = byte.Parse(sval);
			}

			return val;
		}
		private void EatWhiteSpaces( )
		{
			while ( char.IsWhiteSpace(this.CurrentChr) )
			{
				this.CurrentNdx++;
			}
		}
		private string GetLabelName( )
		{
			string lblname = null;

			while ( char.IsLetterOrDigit(this.CurrentChr) )
			{
				if ( this.CurrentChr == ':' )
				{
					this.CurrentNdx++;
					break;
				}

				lblname += this.CurrentChr;
				this.CurrentNdx++;
			}

			return lblname;
		}
		#endregion
	}
}

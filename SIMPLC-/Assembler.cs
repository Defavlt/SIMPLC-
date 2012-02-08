using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace SIMPLC_
{
	class Assembler
	{
		private TextReader fileTextReader;

		private StreamReader input;
		private BinaryWriter output;
		private string fileStart;
		private Hashtable LabelTable;
		private bool isEnd;

		private int CurrentNdx;
		private ushort AsLength;
		private ushort ExecutionAddress;
		private string Source;

		public Assembler( FileStream fileReader,FileStream fileWriter, string fileStart )
		{
			// TODO: Complete member initialization
			Console.WriteLine("Initializing..");
			this.input = new StreamReader(fileReader);
			this.output = new BinaryWriter(fileWriter);

			this.LabelTable = new Hashtable( );
			this.isEnd = false;
			this.CurrentNdx = 0;
			this.AsLength = 0;
			this.ExecutionAddress = 0;

			Console.WriteLine("Starting write..");
			this.Initialize( fileStart );
			Console.WriteLine("Write ended, Sucess!");
		}

		public void Initialize( string startAddress )
		{
			this.AsLength = Convert.ToUInt16(startAddress, 16);

			this.Source = this.input.ReadToEnd( );
			this.input.Close( );

			this.output.Write('B');
			this.output.Write('3');
			this.output.Write('2');
			this.output.Write(this.AsLength);
			this.output.Write((ushort) 0);
			this.Parse();

			this.output.Seek(5, SeekOrigin.Begin);
			this.output.Write(this.ExecutionAddress);
			this.output.Close( );

		}

		private void Parse( )
		{
			this.CurrentNdx = 0;

			while ( !this.isEnd )
			{
				this.LabelScan(true);
			}

			this.isEnd = false;
			this.CurrentNdx = 0;

			while ( !this.isEnd )
			{
				this.LabelScan(false);
			}
		}

		private void LabelScan( bool isLabelScan )
		{
			if ( char.IsLetter(this.Source[this.CurrentNdx]) )
			{
				while ( this.Source[this.CurrentNdx] != '\n' )
				{
					this.CurrentNdx++;
				}

				this.CurrentNdx++;
				return;
			}

			this.EatWhiteSpaces( );
			this.ReadMnemonic( isLabelScan );
		}

		private void ReadMnemonic( bool isLabelScan )
		{
			//Stop shouting at me stupid compiler
			string mnemonic = null;

			while ( !(char.IsWhiteSpace(this.Source[this.CurrentNdx])) )
			{
				mnemonic += this.Source[this.CurrentNdx];
				this.CurrentNdx++;
			}

			mnemonic = mnemonic.ToUpper( );

			switch ( mnemonic )
			{
				case C_MNEMONICS.C_LDA:
					this.InterpretMnemonic(E_MNEMONIC.LDA, isLabelScan);
					break;
				case C_MNEMONICS.C_LDX:
					this.InterpretMnemonic(E_MNEMONIC.LDX, isLabelScan);
					break;
				case C_MNEMONICS.C_STA:
					this.InterpretMnemonic(E_MNEMONIC.STA, isLabelScan);
					break;
				case C_MNEMONICS.C_END:
					this.DoEnd(isLabelScan);
					this.EatWhiteSpaces( );
					this.ExecutionAddress = (ushort) this.LabelTable[( this.GetLabelName( ) )];
					return;
			}

			while ( this.Source[this.CurrentNdx] != '\n' )
			{
				this.CurrentNdx++;
			}

			this.CurrentNdx++;
		}

		private void InterpretMnemonic(E_MNEMONIC MNE, bool isLabelScan)
		{
			switch ( MNE )
			{
				case E_MNEMONIC.LDA:
					this.EatWhiteSpaces( );

					if ( this.Source[this.CurrentNdx] == '#' )
					{
						this.CurrentNdx++;
						byte val = this.ReadByteValue( );
						this.AsLength += 2;

						if ( !isLabelScan )
						{
							this.output.Write((byte) C_MNEMONICS.S_LDX.value);
							this.output.Write(val);
						}
					}
					break;
				case E_MNEMONIC.LDX:
					this.EatWhiteSpaces( );

					if ( this.Source[this.CurrentNdx] == '#' )
					{
						this.CurrentNdx++;
						ushort val = this.ReadWordValue( );
						this.AsLength += 3;

						if ( !isLabelScan )
						{
							this.output.Write((byte) C_MNEMONICS.S_LDX.value);
							this.output.Write(val);
						}
					}
					break;
				case E_MNEMONIC.STA:
					this.EatWhiteSpaces( );

					if ( this.Source[this.CurrentNdx] == ',' )
					{
						E_REGISTERS r;
						byte opcode = 0x00;

						this.CurrentNdx++;
						this.EatWhiteSpaces( );
						r = this.ReadRegister( );

						switch ( r )
						{
							case E_REGISTERS.Unknown:
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

						if ( isLabelScan )
						{
							this.output.Write(opcode);
						}
					}
					break;
				default:
					break;
			}
		}

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
			int index = this.CurrentNdx;
			this.CurrentNdx++;

			switch ( this.Source[index] )
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
					return E_REGISTERS.Unknown;
			}
		}

		private ushort ReadWordValue( )
		{
			ushort val = 0;
			bool isHex = false;
			string s_val = "";

			if ( this.Source[this.CurrentNdx] == '$' )
			{
				this.CurrentNdx++;
				isHex = true;
			}

			while ( char.IsLetterOrDigit(this.Source[this.CurrentNdx]) )
			{
				s_val += this.Source[this.CurrentNdx];
			}

			if ( isHex )
			{
				val = Convert.ToUInt16(s_val, 16);
			}

			else
			{
				val = ushort.Parse(s_val);
			}

			return val;
		}

		private byte ReadByteValue( )
		{
			byte val = 0;
			bool isHex = false;
			string s_val = "";

			if ( this.Source[this.CurrentNdx] == '$' )
			{
				this.CurrentNdx++;
				isHex = true;
			}

			while ( char.IsLetterOrDigit(this.Source[this.CurrentNdx]) )
			{
				s_val += this.Source[this.CurrentNdx];
				this.CurrentNdx++;
			}

			if ( isHex )
			{
				val = Convert.ToByte(s_val, 16);
			}

			else
			{
				val = byte.Parse(s_val);
			}

			return val;

		}

		private void EatWhiteSpaces( )
		{
			while ( char.IsWhiteSpace(this.Source[this.CurrentNdx]) )
			{
				this.CurrentNdx++;
			}
		}

		private string GetLabelName( )
		{
			string label = "";

			while ( char.IsLetterOrDigit(this.Source[this.CurrentNdx]) )
			{
				if ( this.Source[this.CurrentNdx] == ':' )
				{
					this.CurrentNdx++;
					break;
				}

				label += this.Source[this.CurrentNdx];
				this.CurrentNdx++;
			}

			return label.ToUpper( );
		}
	
	}
}


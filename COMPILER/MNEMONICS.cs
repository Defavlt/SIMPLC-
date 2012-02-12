using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIMPLC_
{
	struct S_MNEMONIC
	{
		public string name;
		public ushort value;

		public S_MNEMONIC( string name, ushort value )
		{
			this.name = name;
			this.value = value;
		}
	}

	static class C_MNEMONICS
	{
		public const string C_LDA = "LDA";
		public const string C_LDX = "LDX";
		public const string C_STA = "STA";
		public const string C_END = "END";
		public const string C_ADD = "ADD";
		public const string C_CMPA = "CMPA";
		public const string C_CMPB = "CMPB";
		public const string C_CMPX = "CMPX";
		public const string C_CMPY = "CMPY";
		public const string C_CMPD = "CMPD";
		public const string C_JMP = "JMP";
		public const string C_JEQ = "JEQ";
		public const string C_JNE = "JNE";
		public const string C_JGT = "JGT";
		public const string C_JLT = "JLT";

		public static S_MNEMONIC S_LDA = new S_MNEMONIC("LDA", 0x01);
		public static S_MNEMONIC S_LDX = new S_MNEMONIC("LDX", 0x02);
		public static S_MNEMONIC S_STA = new S_MNEMONIC("STA", 0x03);
		public static S_MNEMONIC S_END = new S_MNEMONIC("END", 0x04);
		public static S_MNEMONIC S_ADD = new S_MNEMONIC("ADD", 0x05);

		public static E_MNEMONIC F_GET( string m )
		{
			switch ( m )
			{
				case C_LDA:
					return E_MNEMONIC.LDA;
				case C_LDX:
					return E_MNEMONIC.LDX;
				case C_STA:
					return E_MNEMONIC.STA;
				case C_END:
					return E_MNEMONIC.END;
				case C_ADD:
					return E_MNEMONIC.ADD;
				case C_CMPA:
					return E_MNEMONIC.CMPA;
				case C_CMPB:
					return E_MNEMONIC.CMPB;
				case C_CMPD:
					return E_MNEMONIC.CMPD;
				case C_CMPX:
					return E_MNEMONIC.CMPX;
				case C_CMPY:
					return E_MNEMONIC.CMPY;
				case C_JEQ:
					return E_MNEMONIC.JEQ;
				case C_JGT:
					return E_MNEMONIC.JGT;
				case C_JLT:
					return E_MNEMONIC.JLT;
				case C_JMP:
					return E_MNEMONIC.JMP;
				case C_JNE:
					return E_MNEMONIC.JNE;
			}

			return E_MNEMONIC.END;
		}
	}

	static class C_MNEMONIC_VAL
	{
		public const byte LDA = 0x01;
		public const byte LDX = 0x02;
		public const byte STA = 0x03;
		public const byte END = 0x04;
		public const byte ADD = 0x05;
		public const byte CMPA = 0x06;
		public const byte CMPB = 0x07;
		public const byte CMPX = 0x08;
		public const byte CMPY = 0x09;
		public const byte CMPD = 0x0A;
		public const byte JMP = 0x0B;
		public const byte JEQ = 0x0C;
		public const byte JNE = 0x0D;
		public const byte JGT = 0x0E;
		public const byte JLT = 0x0F;
	}

	enum E_MNEMONIC
	{
		LDA,
		LDX,
		STA,
		END,
		ADD,
		CMPA,
		CMPB,
		CMPX,
		CMPY,
		CMPD,
		JMP,
		JEQ,
		JNE,
		JGT,
		JLT
	}
}

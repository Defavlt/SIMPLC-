using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MACHINE.CPU
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

		public static S_MNEMONIC S_LDA = new S_MNEMONIC(C_LDA, 0x01);
		public static S_MNEMONIC S_LDX = new S_MNEMONIC(C_LDX, 0x02);
		public static S_MNEMONIC S_STA = new S_MNEMONIC(C_STA, 0x03);
		public static S_MNEMONIC S_END = new S_MNEMONIC(C_END, 0x04);
		public static S_MNEMONIC S_ADD = new S_MNEMONIC(C_ADD, 0x05);

		public static E_MNEMONIC F_GET( string m )
		{
			switch ( m )
			{
				case C_MNEMONICS.C_LDA:
					return E_MNEMONIC.LDA;
				case C_MNEMONICS.C_LDX:
					return E_MNEMONIC.LDX;
				case C_MNEMONICS.C_STA:
					return E_MNEMONIC.STA;
				case C_MNEMONICS.C_END:
					return E_MNEMONIC.END;
				case C_MNEMONICS.C_ADD:
					return E_MNEMONIC.ADD;
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
	}
	enum E_MNEMONIC
	{
		LDA,
		LDX,
		STA,
		END,
		ADD
	}
}

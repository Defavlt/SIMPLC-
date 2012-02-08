﻿using System;
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

		public static S_MNEMONIC S_LDA = new S_MNEMONIC("LDA", 0x01);
		public static S_MNEMONIC S_LDX = new S_MNEMONIC("LDX", 0x02);
		public static S_MNEMONIC S_STA = new S_MNEMONIC("STA", 0x03);
		public static S_MNEMONIC S_END = new S_MNEMONIC("END", 0x04);
	}

	enum E_MNEMONIC
	{
		LDA,
		LDX,
		STA,
		END
	}
}

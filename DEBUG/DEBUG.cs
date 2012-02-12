using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DEBUG
{
	public static class C_DEBUG
	{
		private static StreamWriter StdOut;
		private static string File;

		public static void Failed( string Message )
		{
			StdOut.WriteLine(String.Format("Failed {0}", Message));
		}
		public static void FailedMemoryInsertion( ushort Address )
		{
			if ( StdOut == null )
			{
				StdOut = new StreamWriter(File);
			}

			Failed(String.Format("writing to location {0}", Address.ToString()));
		}

		public static void FailedMemoryExtraction( ushort MEMLOC )
		{
			throw new NotImplementedException( );
		}
	}
}

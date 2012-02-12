using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MACHINE
{
	static class GRAPHICS
	{
		private static ushort _LOCATION = 0xA000;
		private static ushort _SIZE = 0x2000;

		/// <summary>
		/// Clear the screen with the default settings
		/// ( 32, 7 )
		/// </summary>
		public static void INIT( )
		{
			//Clear the screen
			CLEAR( 32, 7 );
		}

		/// <summary>
		/// Clear the screen with the specified character and the specified attribute
		/// <note>Use '32' for blank character</note>
		/// </summary>
		/// <param name="Character">The character to fill the screen with</param>
		/// <param name="Attribute">The attribute (color) to use</param>
		public static void CLEAR( byte Character, byte Attribute )
		{
			for (
						 ushort i = GRAPHICS.LOCATION;
						 i < ( GRAPHICS.LOCATION + GRAPHICS.SIZE );
						 i += 2 )
			{
				//Clear the screen
				MEMORY.MAIN[i] = Character;
				MEMORY.MAIN[i + 1] = Attribute;
			}
		}

		/// <summary>
		/// Poke a value into the specified address
		/// </summary>
		/// <param name="Address">The address where the poke should occur</param>
		/// <param name="Value">The value to poke into the address</param>
		public static void POKE( ushort Address, byte Value )
		{
			ushort MEMLOC;

			MEMLOC = GRAPHICS.TRANSLATE(Address);
			INSERT(Value, MEMLOC);
		}

		/// <summary>
		/// Get a value from the specified address
		/// </summary>
		/// <param name="Address">The address where the desired value is located.</param>
		/// <returns>Returns whatever is at the specified address within the pool.</returns>
		public static byte PEEK( ushort Address )
		{
			ushort MEMLOC;

			MEMLOC = GRAPHICS.TRANSLATE(Address);
			return GRAPHICS.EXTRACT(Address);

		}

		/// <summary>
		/// Exposes the allocated pool of memory for this device.
		/// Operation: INSERT a VALUE into LOCATION
		/// </summary>
		/// <param name="VALUE">The value to insert</param>
		/// <param name="MEMLOC">The location where the value should end up</param>
		private static void INSERT( byte VALUE, ushort MEMLOC )
		{
			if (
				ISBOUND(MEMLOC) )
			{
				return;
			}

			else
			{
				MEMORY.MAIN[MEMLOC] = VALUE;
			}
		}

		/// <summary>
		/// Checks whether the specified Address is within the bounds of the allocated memory of this device.
		/// </summary>
		/// <param name="Address">The address to check wether it is within the bounds (of this device).</param>
		/// <returns>Returns true if it is within the bounds, otherwise false.</returns>
		public static bool ISBOUND( ushort Address )
		{
			return Address < GRAPHICS.LOCATION ||
							Address > GRAPHICS.TRANSLATE(GRAPHICS.SIZE);
		}

		/// <summary>
		/// Extract a value at MEMLOC from the allocated pool for this device.
		/// </summary>
		/// <param name="MEMLOC">The location of the value</param>
		/// <returns>The value contained at the location of MEMLOC</returns>
		private static byte EXTRACT( ushort MEMLOC )
		{
			if ( GRAPHICS.ISBOUND(MEMLOC) )
			{
				return MEMORY.MAIN[MEMLOC];
			}

			else
			{
				return 0x00;
			}
		}

		/// <summary>
		/// Translate an address local (to this device) into an address local (to the main pool of memory)
		/// </summary>
		/// <param name="LocalAddress"></param>
		/// <returns></returns>
		public static ushort TRANSLATE( ushort LocalAddress )
		{
			return (ushort) ( GRAPHICS.LOCATION + LocalAddress );
		}

		/// <summary>
		/// The location of the memory seen from the main pool.
		/// This value is STATIC.
		/// </summary>
		public static ushort LOCATION
		{
			get
			{
				return GRAPHICS._LOCATION;
			}
		}
		/// <summary>
		/// The size of the allocated pool of memory for this device.
		/// This value is STATIC.
		/// </summary>
		public static ushort SIZE
		{
			get
			{
				return GRAPHICS._SIZE;
			}
		}
	}
}

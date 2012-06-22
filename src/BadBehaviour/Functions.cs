using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BadBehaviour
{
	public static class Functions
	{
		private static uint? IP2Uint(string str)
		{
			// Only match IPv4 addresses here to match PHP Bad Behavior.
			IPAddress addr;
			if (!IPAddress.TryParse(str, out addr)) return null;
			if (addr.AddressFamily != AddressFamily.InterNetwork) return null;
			var bytes = addr.GetAddressBytes();
			return ((uint)bytes[0] << 24) + ((uint)bytes[1] << 16) +
				((uint)bytes[2] << 8) + (uint)bytes[3];
		}

		/// <summary>
		///  Tests to see whether the given IP address matches a given CIDR netblock.
		/// </summary>
		/// <param name="addr"></param>
		/// <param name="cidr"></param>
		/// <returns></returns>

		public static bool MatchCidr(string addr, params string[] cidrs)
		{
			uint? uAddr = IP2Uint(addr);
			if (!uAddr.HasValue) return false; // Non-IP4 addresses don't match.

			foreach (var cidr in cidrs) {
				var parts = cidr.Split('/');
				IPAddress cidrAddr;
				uint? uCidr = IP2Uint(parts[0]);
				if (uCidr.HasValue) {
					if (parts.Length == 1 && uAddr == uCidr) return true;
					int bits;
					if (!Int32.TryParse(parts[1], out bits)) continue;
					uint mask = ~((1u << (32 - bits)) - 1);
					if ((uCidr & mask) == (uAddr & mask)) return true;
				}
			}
			return false;
		}
	}
}

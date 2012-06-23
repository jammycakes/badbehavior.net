using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BadBehavior
{
	public static class Functions
	{
		private static uint? ToUint(this IPAddress addr)
		{
			// BB original isn't interested in non-IP4 addresses so neither are we.
			if (addr.AddressFamily != AddressFamily.InterNetwork) return null;
			var bytes = addr.GetAddressBytes();
			return ((uint)bytes[0] << 24) + ((uint)bytes[1] << 16) +
				((uint)bytes[2] << 8) + (uint)bytes[3];
		}

		private static uint? IP2Uint(string str)
		{
			return SafeParseIP(str).ToUint();
		}

		/// <summary>
		///  Safe parsing of IP addresses. Returns null rather than throwing an exception
		///  if an invalid string is given.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>

		public static IPAddress SafeParseIP(string str)
		{
			IPAddress ip;
			return IPAddress.TryParse(str, out ip) ? ip : null;
		}

		/// <summary>
		///  Tests to see whether the given IP address matches a given CIDR netblock.
		/// </summary>
		/// <param name="addr"></param>
		/// <param name="cidrs"></param>
		/// <returns></returns>

		public static bool MatchCidr(this IPAddress addr, params string[] cidrs)
		{
			uint? uAddr = addr.ToUint();
			if (!uAddr.HasValue) return false;
			foreach (var cidr in cidrs) {
				var parts = cidr.Split('/');
				uint? uCidr = IP2Uint(parts[0]);
				if (uCidr.HasValue) {
					if (parts.Length == 1) return uAddr == uCidr;
					int bits;
					if (!Int32.TryParse(parts[1], out bits)) continue;
					uint mask = ~((1u << (32 - bits)) - 1);
					if ((uCidr & mask) == (uAddr & mask)) return true;
				}
			}
			return false;
		}


		/// <summary>
		///  Determines if this is an IP address reserved by RFC 1918.
		/// </summary>
		/// <param name="addr">The IP address.</param>
		/// <returns>true if the address is reserved, otherwise false.</returns>

		public static bool IsRfc1918(this IPAddress addr)
		{
			return addr.MatchCidr("10.0.0.0/8", "172.16.0.0/12", "192.168.0.0/16");
		}
	}
}

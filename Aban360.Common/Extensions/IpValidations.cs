using Aban360.Common.Exceptions;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Aban360.Common.Extensions
{
    public static class IpValidations
    {
        public static void CheckValidIpV4(string ip)
        {
            //check ip structure, if it`s not valid then throw Exception of type "InvalidIpException"

            string pattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            bool result = Regex.IsMatch(ip, pattern);
            if (!result)
            {
                throw new InvalidIpException(ip);
            }
        }


        public static void CheckValidIpV6(string ip)
        {
            //check ip structure, if it`s not valid then throw Exception of type "InvalidIpException"
            if (!(IsValidIPv6(ip)))
            {
                throw new InvalidIpException(ip);
            }
        }


        static bool IsValidIPv6(string ipAddress)
        {
            if (IPAddress.TryParse(ipAddress, out IPAddress address))
            {
                if (ipAddress.Contains("999"))
                {
                    return false;
                }
                return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
            }

            return false;
        }

    }
}

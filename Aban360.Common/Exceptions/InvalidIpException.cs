using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.Common.Exceptions
{
    public class InvalidIpException:Exception
    {
        public InvalidIpException(string ip)
            :base(string.Format(ExceptionLiterals.InvalidIp)) 
        {
            ip.NotNull();
        }
    }
}

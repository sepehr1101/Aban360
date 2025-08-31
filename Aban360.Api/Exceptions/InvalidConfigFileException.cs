using Aban360.Common.Exceptions;

namespace Aban360.Api.Exceptions
{
    public class InvalidConfigFileException:BaseException
    {
        public InvalidConfigFileException(string message)
            :base(message)
        {
            
        }
    }
}

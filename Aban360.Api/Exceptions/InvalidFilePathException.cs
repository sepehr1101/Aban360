using Aban360.Common.Exceptions;

namespace Aban360.Api.Exceptions
{
    public class InvalidFilePathException:BaseException
    {
        public InvalidFilePathException(string message)
            :base(message)
        {

        }
    }
}

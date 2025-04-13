using Aban360.Common.Literals;

namespace Aban360.Common.Exceptions
{
    public class CustomeValidationException:BaseException
    {
        public CustomeValidationException(string messageException):base(ExceptionLiterals.MessageException(messageException))
        {
        }
    }
}

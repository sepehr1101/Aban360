using Aban360.Common.Literals;

namespace Aban360.Common.Exceptions
{
    public class CustomValidationException : BaseException
    {
        public CustomValidationException(string messageException)
            : base(ExceptionLiterals.MessageException(messageException))
        {
        }
    }
}

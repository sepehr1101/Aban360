using Aban360.Common.Exceptions;

namespace Aban360.UserPool.Application.Exceptions
{
    public class ChangePasswordValidationException:BaseException
    {
        public ChangePasswordValidationException(string message)
            :base(message)
        {            
        }
    }
}

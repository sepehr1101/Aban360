using Aban360.Common.Exceptions;
using Aban360.UserPool.Domain.Constants;

namespace Aban360.UserPool.Application.Exceptions
{
    public class InvalidIdException : BaseException
    {
        public InvalidIdException()
            : base(MessageResources.InvalidId)
        {
        }
    }
}

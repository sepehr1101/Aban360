using Aban360.UserPool.Domain.Constants;

namespace Aban360.UserPool.Application.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException()
            : base(MessageResources.InvalidId)
        {
        }
    }
}

using Aban360.ClaimPool.Persistence.Constants.Literals;

namespace Aban360.ClaimPool.Persistence.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException() : base(ExceptionLiterals.InvalidIdentifier)
        {

        }
    }
}

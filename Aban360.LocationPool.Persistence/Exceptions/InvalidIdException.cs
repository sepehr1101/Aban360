using Aban360.LocationPool.Persistence.Constants.Literals;

namespace Aban360.LocationPool.Persistence.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException() : base(ExceptionLiterals.InvalidIdentifier)
        {

        }
    }
}

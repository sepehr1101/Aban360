using Aban360.Common.Db.Constants.Literals;

namespace Aban360.Common.Db.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException() : base(ExceptionLiterals.InvalidIdentifier)
        {

        }
    }
}

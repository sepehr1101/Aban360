using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Exceptions;

namespace Aban360.Common.Db.Exceptions
{
    public class InvalidIdException : BaseException
    {
        public InvalidIdException() : base(ExceptionLiterals.InvalidIdentifier)
        {

        }
    }
}

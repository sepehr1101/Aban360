using Aban360.UserPool.Persistence.Constants.Literals;

namespace Aban360.UserPool.Persistence.Exceptions
{
    public class InvalidIdException:Exception
    {
        public InvalidIdException():base(ExceptionLiterals.InvalidIdentifier)
        {
            
        }
    }
}

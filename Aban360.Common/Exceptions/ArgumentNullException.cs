using Aban360.Common.Literals;

namespace Aban360.Common.Exceptions
{
    public class ArgumentIsNullException:ArgumentException
    {
        public ArgumentIsNullException(string argumentName)
            :base($"{ExceptionLiterals.ArgumentIsNull_1} {argumentName}") 
        {            
        }
    }
}

using Aban360.Common.Exceptions;

namespace Aban360.OldCalcPool.Application.Exceptions
{
    public class InvalidBillParametersException:BaseException
    {
        public InvalidBillParametersException(string message)
            :base(message)
        {
            
        }
    }
}

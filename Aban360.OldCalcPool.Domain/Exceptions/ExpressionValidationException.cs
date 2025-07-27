using Aban360.Common.Exceptions;

namespace Aban360.OldCalcPool.Domain.Exceptions
{
    public class ExpressionValidationException : BaseException
    {
        public ExpressionValidationException(string message) : base(message)
        {

        }
    }
}
namespace Aban360.Common.Exceptions
{
    public sealed class IdempotentOperationInProgressException : BaseException
    {
        public IdempotentOperationInProgressException(string message)
            : base(message)
        {
        }
    }
}

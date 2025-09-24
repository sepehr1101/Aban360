namespace Aban360.Common.Exceptions
{
    public class DuplicateEntityException : BaseException
    {
        public DuplicateEntityException(string message)
            : base(message)
        { }
    }
}

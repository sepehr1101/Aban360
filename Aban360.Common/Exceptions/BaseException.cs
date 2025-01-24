namespace Aban360.Common.Exceptions
{
    public class BaseException:Exception
    {
        private readonly string _message;
        public BaseException(string message):
            base(message)
        {
            _message = message;
        }
    }
}

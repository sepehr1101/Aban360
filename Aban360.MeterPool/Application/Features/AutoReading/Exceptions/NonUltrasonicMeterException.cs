using Aban360.Common.Exceptions;

namespace Aban360.MeterPool.Application.Features.AutoReading.Exceptions
{
    public sealed class NonUltrasonicMeterException : BaseException
    {
        public NonUltrasonicMeterException() : base("نوع کنتور اولتراسونیک نیست")
        {
        }
    }
}

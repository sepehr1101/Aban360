using Aban360.Common.Exceptions;

namespace Aban360.UserPool.Application.Features.SubscriberRequests;

public class SubscriberRequestException(string message) : BaseException(message);

public sealed class SubscriberRequestPayloadException : SubscriberRequestException
{
    public IReadOnlyList<string> Errors { get; }

    public SubscriberRequestPayloadException(IReadOnlyList<string> errors)
        : base("اطلاعات درخواست با قالب تعریف‌شده مطابقت ندارد: " + string.Join("؛ ", errors))
    {
        Errors = errors;
    }
}

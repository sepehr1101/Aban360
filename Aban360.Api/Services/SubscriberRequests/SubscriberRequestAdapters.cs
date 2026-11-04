using Aban360.Common.Db.Services;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.UserPool.Application.Features.SubscriberRequests;
using Aban360.UserPool.Domain.Features.SubscriberRequests;
using Aban360.UserPool.Persistence.Contexts.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Aban360.Api.Services.SubscriberRequests;

public sealed class SubscriberRequestContactResolver(UserPoolContext users, ICommonMemberQueryService members)
    : ISubscriberRequestContactResolver
{
    public async Task<SubscriberRequestContact> Resolve(Guid ownerId, string billId, CancellationToken ct)
    {
        var user = await users.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == ownerId, ct);
        var memberKey = await members.Get(billId, false);
        if (user is null || memberKey is null) throw AccessError();
        var member = await members.Get(memberKey, false);
        if (member is null || string.IsNullOrWhiteSpace(user.Mobile) ||
            Normalize(member.MobileNumber) != Normalize(user.Mobile)) throw AccessError();
        return new(billId, Normalize(member.MobileNumber));
    }

    private static string Normalize(string? mobile)
    {
        var value = mobile?.Trim() ?? "";
        if (value.StartsWith("+98", StringComparison.Ordinal)) value = "0" + value[3..];
        else if (value.StartsWith("0098", StringComparison.Ordinal)) value = "0" + value[4..];
        else if (value.Length == 10 && value[0] == '9') value = "0" + value;
        return value;
    }

    private static SubscriberRequestException AccessError() =>
        new("اشتراک یافت نشد یا شماره همراه حساب شما با شماره ثبت‌شده برای اشتراک مطابقت ندارد.");
}

public sealed class SubscriberRequestOtpSender(ISmsOldHandler sms) : ISubscriberRequestOtpSender
{
    public Task Send(string mobile, string code, Guid challengeId, CancellationToken ct) =>
        sms.Send(mobile, $"کد تأیید درخواست شما: {code}\nاعتبار کد حداکثر ۱۲۰ ثانیه است.",
            challengeId, SmsMessageType.Otp).WaitAsync(TimeSpan.FromSeconds(20), ct);
}

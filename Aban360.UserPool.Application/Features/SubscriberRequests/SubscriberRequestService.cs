using System.Text.Json;
using System.Text.RegularExpressions;
using Aban360.UserPool.Domain.Features.SubscriberRequests;

namespace Aban360.UserPool.Application.Features.SubscriberRequests;

public sealed class SubscriberRequestService : ISubscriberRequestService
{
    private readonly ISubscriberRequestStore _store;
    private readonly ISubscriberRequestContactResolver _contacts;
    private readonly ISubscriberRequestOtpSender _sms;
    private readonly SubscriberRequestPayloadValidator _validator;
    private readonly SubscriberRequestOptions _options;
    private readonly SubscriberRequestOtp _otp;
    private readonly TimeProvider _time;
    private readonly IReadOnlyDictionary<string, ISubscriberRequestHandler> _handlers;
    private DateTime Now => _time.GetUtcNow().UtcDateTime;

    public SubscriberRequestService(ISubscriberRequestStore store, ISubscriberRequestContactResolver contacts,
        ISubscriberRequestOtpSender sms, SubscriberRequestPayloadValidator validator, SubscriberRequestOptions options,
        IEnumerable<ISubscriberRequestHandler> handlers, TimeProvider time)
    {
        if (options.OtpLifetimeSeconds is < 1 or > 120 || options.MaxFailedAttempts is < 1 or > 10 ||
            options.ResendCooldownSeconds < 1 || options.MaxSendsPer15Minutes is < 1 or > 20)
            throw new InvalidOperationException("SubscriberRequests options are outside their supported ranges.");
        _store = store; _contacts = contacts; _sms = sms; _validator = validator;
        _options = options; _otp = new(options); _time = time;
        _handlers = handlers.ToDictionary(x => x.Name, StringComparer.Ordinal);
    }

    public async Task<IReadOnlyList<SubscriberRequestDefinitionDto>> Definitions(CancellationToken ct)
    {
        return (await _store.GetDefinitions(ct)).Where(x => x.IsEnabled).Select(x =>
        {
            using var schema = JsonDocument.Parse(x.PayloadSchema);
            return new SubscriberRequestDefinitionDto(x.RequestCode, x.Version, x.Title, schema.RootElement.Clone());
        }).ToArray();
    }

    public async Task<SubscriberRequestReceipt> Create(SubscriberRequestCreateDto input, Guid ownerId, CancellationToken ct)
    {
        CheckOwner(ownerId);
        if (!Enum.IsDefined(input.RequestCode)) throw new SubscriberRequestException("کد درخواست معتبر نیست.");
        if (input.BillId is null || !Regex.IsMatch(input.BillId, "^[0-9]{1,20}$"))
            throw new SubscriberRequestException("شناسه قبض معتبر نیست.");
        if (input.Payload.ValueKind != JsonValueKind.Object || input.Payload.GetRawText().Length > 65536)
            throw new SubscriberRequestPayloadException(["$: اطلاعات باید یک شیء JSON با حداکثر ۶۴ کیلوبایت متن باشد"]);
        await using var tx = await _store.Begin(ownerId, ct);
        var definition = await tx.GetDefinition(input.RequestCode, ct);
        if (definition is null || !definition.IsEnabled) throw new SubscriberRequestException("این خدمت فعال نیست.");
        _ = ResolveHandler(definition.HandlerName);
        _validator.Validate(definition.PayloadSchema, input.Payload);
        var contact = await _contacts.Resolve(ownerId, input.BillId, ct);
        if (contact.BillId != input.BillId || !Regex.IsMatch(contact.Mobile, "^09[0-9]{9}$"))
            throw new SubscriberRequestException("شماره همراه معتبر برای این اشتراک ثبت نشده است.");
        var draft = new SubscriberRequestDraft
        {
            Id = Guid.NewGuid(), TrackingCode = Guid.NewGuid().ToString("N"), OwnerId = ownerId,
            BillId = contact.BillId, Mobile = contact.Mobile, RequestCode = definition.RequestCode,
            DefinitionVersion = definition.Version, HandlerName = definition.HandlerName,
            PayloadSchema = definition.PayloadSchema, Payload = input.Payload.GetRawText(), CreatedAt = Now
        };
        await tx.LockMobile(draft.Mobile, ct);
        await tx.InsertDraft(draft, ct);
        var challenge = await SendCode(draft, tx, ct);
        await tx.Commit(CancellationToken.None);
        if (challenge.DeliveryFailed) throw DeliveryError(draft.Id);
        return Receipt(draft, challenge);
    }

    public async Task<SubscriberRequestReceipt> Resend(Guid id, Guid ownerId, CancellationToken ct)
    {
        CheckOwner(ownerId);
        await using var tx = await _store.Begin(ownerId, ct);
        var draft = await Load(tx, id, ownerId, ct);
        if (draft.Status != SubscriberRequestStatus.Pending) throw new SubscriberRequestException("ارسال کد برای این وضعیت درخواست مجاز نیست.");
        var contact = await _contacts.Resolve(ownerId, draft.BillId, ct);
        if (contact.Mobile != draft.Mobile) throw new SubscriberRequestException("شماره همراه اشتراک تغییر کرده است؛ درخواست جدید ثبت کنید.");
        var previous = await tx.GetLatestChallenge(id, ct);
        if (previous is not null && Now < previous.CreatedAt.AddSeconds(_options.ResendCooldownSeconds))
            throw new SubscriberRequestException("برای ارسال مجدد کد کمی صبر کنید.");
        var challenge = await SendCode(draft, tx, ct);
        await tx.Commit(CancellationToken.None);
        if (challenge.DeliveryFailed) throw DeliveryError(draft.Id);
        return Receipt(draft, challenge);
    }

    public async Task<SubscriberRequestReceipt> Confirm(Guid id, string code, Guid ownerId, CancellationToken ct)
    {
        CheckOwner(ownerId);
        await using var tx = await _store.Begin(ownerId, ct);
        var draft = await Load(tx, id, ownerId, ct);
        if (draft.Status == SubscriberRequestStatus.Completed) return Receipt(draft, null);
        if (draft.Status == SubscriberRequestStatus.Locked) throw new SubscriberRequestException("تعداد تلاش‌های ناموفق بیش از حد مجاز است؛ درخواست جدید ثبت کنید.");
        if (draft.Status == SubscriberRequestStatus.Pending)
        {
            var challenge = await tx.GetLatestChallenge(id, ct);
            if (challenge is null || challenge.DeliveryFailed || challenge.SentAt is null || challenge.ExpiresAt is null)
                throw new SubscriberRequestException("کد فعالی برای این درخواست وجود ندارد؛ کد جدید دریافت کنید.");
            if (challenge.ConsumedAt is not null || Now >= challenge.ExpiresAt)
                throw new SubscriberRequestException("کد تأیید منقضی شده است؛ کد جدید دریافت کنید.");
            if (!_otp.Verify(id, challenge.Id, code ?? "", challenge.CodeHash))
            {
                draft.FailedAttempts++;
                if (draft.FailedAttempts >= _options.MaxFailedAttempts) draft.Status = SubscriberRequestStatus.Locked;
                await tx.SaveDraft(draft, ct);
                await tx.Commit(ct);
                throw new SubscriberRequestException("کد تأیید صحیح نیست.");
            }
            var contact = await _contacts.Resolve(ownerId, draft.BillId, ct);
            if (contact.Mobile != draft.Mobile) throw new SubscriberRequestException("شماره همراه اشتراک تغییر کرده است؛ درخواست جدید ثبت کنید.");
            var confirmedAt = Now;
            if (confirmedAt >= challenge.ExpiresAt)
                throw new SubscriberRequestException("کد تأیید منقضی شده است؛ کد جدید دریافت کنید.");
            challenge.ConsumedAt = confirmedAt;
            draft.Status = SubscriberRequestStatus.Confirmed;
            draft.ConfirmedAt = confirmedAt;
            await tx.SaveChallenge(challenge, ct);
            await tx.SaveDraft(draft, ct);
            // Persist OTP consumption before running business logic; a failed handler can be retried by the owner.
            await tx.Commit(ct);
        }
        else if (draft.Status != SubscriberRequestStatus.Confirmed)
            throw new SubscriberRequestException("وضعیت درخواست معتبر نیست.");
        else
            await tx.Commit(ct);

        await using var execution = await _store.Begin(ownerId, ct);
        draft = await Load(execution, id, ownerId, ct);
        if (draft.Status == SubscriberRequestStatus.Completed) return Receipt(draft, null);
        if (draft.Status != SubscriberRequestStatus.Confirmed) throw new SubscriberRequestException("درخواست تأیید نشده است.");
        using var payload = JsonDocument.Parse(draft.Payload);
        _validator.Validate(draft.PayloadSchema, payload.RootElement);
        var result = await ResolveHandler(draft.HandlerName).Handle(draft, execution, ct);
        draft.ResultJson = result.GetRawText();
        draft.Status = SubscriberRequestStatus.Completed;
        draft.CompletedAt = Now;
        await execution.SaveDraft(draft, ct);
        await execution.Commit(ct);
        return Receipt(draft, null);
    }

    public async Task<SubscriberRequestReceipt> Get(Guid id, Guid ownerId, CancellationToken ct)
    {
        CheckOwner(ownerId);
        await using var tx = await _store.Begin(ownerId, ct);
        var draft = await Load(tx, id, ownerId, ct);
        return Receipt(draft, await tx.GetLatestChallenge(id, ct));
    }

    private async Task<SubscriberRequestChallenge> SendCode(SubscriberRequestDraft draft, ISubscriberRequestTransaction tx, CancellationToken ct)
    {
        await tx.LockMobile(draft.Mobile, ct);
        if (await tx.CountRecentChallenges(draft.OwnerId, draft.Mobile, Now.AddMinutes(-15), ct) >= _options.MaxSendsPer15Minutes)
            throw new SubscriberRequestException("تعداد ارسال کد بیش از حد مجاز است؛ بعداً تلاش کنید.");
        var challenge = new SubscriberRequestChallenge { Id = Guid.NewGuid(), RequestId = draft.Id, CreatedAt = Now };
        var code = _otp.Generate();
        challenge.CodeHash = _otp.Hash(draft.Id, challenge.Id, code);
        await tx.InsertChallenge(challenge, ct);
        try
        {
            // Once delivery starts, retain the attempt even if the caller disconnects.
            await _sms.Send(draft.Mobile, code, challenge.Id, CancellationToken.None);
            challenge.SentAt = Now;
            challenge.ExpiresAt = challenge.SentAt.Value.AddSeconds(_options.OtpLifetimeSeconds);
        }
        catch (Exception ex) when (ex is HttpRequestException or TimeoutException or TaskCanceledException)
        {
            challenge.DeliveryFailed = true;
        }
        await tx.SaveChallenge(challenge, CancellationToken.None);
        return challenge;
    }

    private ISubscriberRequestHandler ResolveHandler(string name) => _handlers.TryGetValue(name, out var handler)
        ? handler : throw new SubscriberRequestException("پردازشگر این خدمت در سامانه ثبت نشده است.");
    private static SubscriberRequestException DeliveryError(Guid id) =>
        new($"ارسال پیامک ناموفق بود؛ برای درخواست {id} کمی بعد ارسال مجدد کد را انجام دهید.");
    private static void CheckOwner(Guid ownerId)
    {
        if (ownerId == Guid.Empty) throw new SubscriberRequestException("هویت درخواست‌دهنده معتبر نیست.");
    }
    private static async Task<SubscriberRequestDraft> Load(ISubscriberRequestTransaction tx, Guid id, Guid ownerId, CancellationToken ct) =>
        await tx.GetDraft(id, ownerId, ct) ?? throw new SubscriberRequestException("درخواست یافت نشد یا دسترسی به آن مجاز نیست.");
    private static SubscriberRequestReceipt Receipt(SubscriberRequestDraft draft, SubscriberRequestChallenge? challenge)
    {
        JsonElement? result = null;
        if (draft.ResultJson is not null)
        {
            using var document = JsonDocument.Parse(draft.ResultJson);
            result = document.RootElement.Clone();
        }
        DateTime? expiresAt = draft.Status == SubscriberRequestStatus.Pending && challenge?.DeliveryFailed == false && challenge.ExpiresAt.HasValue
            ? DateTime.SpecifyKind(challenge.ExpiresAt.Value, DateTimeKind.Utc) : null;
        return new(draft.Id, draft.TrackingCode, draft.RequestCode, draft.Status,
            expiresAt,
            draft.Mobile[..3] + "*****" + draft.Mobile[^3..], result);
    }
}

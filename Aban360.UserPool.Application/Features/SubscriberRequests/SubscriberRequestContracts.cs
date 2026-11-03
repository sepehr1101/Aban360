using System.Text.Json;
using Aban360.UserPool.Domain.Features.SubscriberRequests;

namespace Aban360.UserPool.Application.Features.SubscriberRequests;

public interface ISubscriberRequestContactResolver
{
    Task<SubscriberRequestContact> Resolve(Guid ownerId, string billId, CancellationToken ct);
}

public interface ISubscriberRequestOtpSender
{
    Task Send(string mobile, string code, Guid challengeId, CancellationToken ct);
}

public interface ISubscriberRequestHandler
{
    string Name { get; }
    Task<JsonElement> Handle(SubscriberRequestDraft request, ISubscriberRequestTransaction transaction, CancellationToken ct);
}

public interface ISubscriberRequestService
{
    Task<IReadOnlyList<SubscriberRequestDefinitionDto>> Definitions(CancellationToken ct);
    Task<SubscriberRequestReceipt> Create(SubscriberRequestCreateDto input, Guid ownerId, CancellationToken ct);
    Task<SubscriberRequestReceipt> Confirm(Guid id, string code, Guid ownerId, CancellationToken ct);
    Task<SubscriberRequestReceipt> Resend(Guid id, Guid ownerId, CancellationToken ct);
    Task<SubscriberRequestReceipt> Get(Guid id, Guid ownerId, CancellationToken ct);
}

public sealed class SubscriberRequestOptions
{
    public string OtpHashKey { get; set; } = "";
    public int OtpLifetimeSeconds { get; set; } = 120;
    public int MaxFailedAttempts { get; set; } = 5;
    public int ResendCooldownSeconds { get; set; } = 30;
    public int MaxSendsPer15Minutes { get; set; } = 5;
}

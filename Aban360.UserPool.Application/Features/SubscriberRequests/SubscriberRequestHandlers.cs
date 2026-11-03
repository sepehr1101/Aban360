using System.Text.Json;
using System.Text.Json.Serialization;
using Aban360.UserPool.Domain.Features.SubscriberRequests;

namespace Aban360.UserPool.Application.Features.SubscriberRequests;

public sealed class RegisterSubscriberRequestHandler : ISubscriberRequestHandler
{
    public string Name => nameof(RegisterSubscriberRequestHandler);

    public async Task<JsonElement> Handle(SubscriberRequestDraft request, ISubscriberRequestTransaction transaction, CancellationToken ct)
    {
        await transaction.RegisterSubmission(request, ct);
        return JsonSerializer.SerializeToElement(new { request.TrackingCode, Registered = true });
    }
}

public abstract class TypedSubscriberRequestHandler<TPayload> : ISubscriberRequestHandler where TPayload : class
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    public abstract string Name { get; }

    public Task<JsonElement> Handle(SubscriberRequestDraft request, ISubscriberRequestTransaction transaction, CancellationToken ct)
    {
        TPayload payload;
        try { payload = JsonSerializer.Deserialize<TPayload>(request.Payload, JsonOptions) ?? throw new JsonException(); }
        catch (JsonException) { throw new SubscriberRequestPayloadException(["$: اطلاعات با مدل ورودی پردازشگر مطابقت ندارد"]); }
        return Handle(payload, request, transaction, ct);
    }

    protected abstract Task<JsonElement> Handle(TPayload payload, SubscriberRequestDraft request,
        ISubscriberRequestTransaction transaction, CancellationToken ct);
}

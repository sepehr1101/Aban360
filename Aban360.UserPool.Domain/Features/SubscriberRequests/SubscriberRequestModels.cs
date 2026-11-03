using System.Text.Json;

namespace Aban360.UserPool.Domain.Features.SubscriberRequests;

public enum SubscriberRequestStatus { Pending = 0, Confirmed = 1, Completed = 2, Locked = 3 }

public sealed class SubscriberRequestDefinition
{
    public SubscriberRequestCode RequestCode { get; set; }
    public int Version { get; set; }
    public string Title { get; set; } = "";
    public string HandlerName { get; set; } = "";
    public string PayloadSchema { get; set; } = "";
    public bool IsEnabled { get; set; }
}

public sealed class SubscriberRequestDraft
{
    public Guid Id { get; set; }
    public string TrackingCode { get; set; } = "";
    public Guid OwnerId { get; set; }
    public string BillId { get; set; } = "";
    public string Mobile { get; set; } = "";
    public SubscriberRequestCode RequestCode { get; set; }
    public int DefinitionVersion { get; set; }
    public string HandlerName { get; set; } = "";
    public string PayloadSchema { get; set; } = "";
    public string Payload { get; set; } = "";
    public SubscriberRequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int FailedAttempts { get; set; }
    public string? ResultJson { get; set; }
}

public sealed class SubscriberRequestChallenge
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public byte[] CodeHash { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? ConsumedAt { get; set; }
    public bool DeliveryFailed { get; set; }
}

public sealed record SubscriberRequestCreateDto(
    SubscriberRequestCode RequestCode, string BillId, JsonElement Payload);
public sealed record SubscriberRequestConfirmDto(string Code);
public sealed record SubscriberRequestReceipt(
    Guid Id, string TrackingCode, SubscriberRequestCode RequestCode,
    SubscriberRequestStatus Status, DateTime? OtpExpiresAt, string MaskedMobile,
    JsonElement? Result);
public sealed record SubscriberRequestDefinitionDto(
    SubscriberRequestCode RequestCode, int Version, string Title, JsonElement PayloadSchema);
public sealed record SubscriberRequestContact(string BillId, string Mobile);

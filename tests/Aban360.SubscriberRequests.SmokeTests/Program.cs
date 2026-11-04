using System.Text.Json;
using System.Text.RegularExpressions;
using Aban360.Common.Exceptions;
using Aban360.UserPool.Application.Features.SubscriberRequests;
using Aban360.UserPool.Domain.Features.SubscriberRequests;

int checks = 0;
void Check(bool condition, string name) { if (!condition) throw new Exception(name); checks++; }
async Task Reject(Func<Task> action, string name)
{
    try { await action(); }
    catch (BaseException) { checks++; return; }
    throw new Exception("Expected rejection: " + name);
}
JsonElement Json(string value) => JsonSerializer.Deserialize<JsonElement>(value);
const string schema = """
    {"type":"object","additionalProperties":false,"properties":{"newMobile":{"type":"string","pattern":"^09[0-9]{9}$"}},"required":["newMobile"]}
    """;
var validator = new SubscriberRequestPayloadValidator();
validator.Validate(schema, Json("""{"newMobile":"09123456789"}"""));
Check(true, "valid schema payload");
foreach (var invalid in new[] { "{}", "null", "[]", """{"newMobile":9123456789}""", """{"newMobile":"abc"}""",
    """{"newMobile":"09123456789","extra":true}""", """{"newMobile":"09123456789","newMobile":"09123456789"}""" })
    await Reject(() => { validator.Validate(schema, Json(invalid)); return Task.CompletedTask; }, "payload mismatch");
await Reject(() => { validator.Validate(schema.Replace("\"pattern\"", "\"unknown\""), Json("{}")); return Task.CompletedTask; }, "unsupported schema fails closed");
await Reject(() => { validator.Validate(schema.Replace("\"pattern\":\"^09[0-9]{9}$\"", "\"minLength\":\"wrong\""), Json("{}")); return Task.CompletedTask; }, "malformed schema");
var nested = """
    {"type":"object","additionalProperties":false,"properties":{"rows":{"type":"array","minItems":1,"maxItems":2,"items":{"type":"object","additionalProperties":false,"properties":{"count":{"type":"integer","minimum":1,"maximum":3},"kind":{"type":"string","enum":["a","b"]}},"required":["count","kind"]}}},"required":["rows"]}
    """;
validator.Validate(nested, Json("""{"rows":[{"count":2,"kind":"a"}]}"""));
foreach (var invalid in new[] { """{"rows":[]}""", """{"rows":[{"count":0,"kind":"a"}]}""",
    """{"rows":[{"count":1.5,"kind":"a"}]}""", """{"rows":[{"count":2,"kind":"c"}]}""" })
    await Reject(() => { validator.Validate(nested, Json(invalid)); return Task.CompletedTask; }, "nested validation");

var owner = Guid.NewGuid();
var store = new MemoryStore(schema);
var clock = new ManualTime();
var sms = new CapturingSms();
var contacts = new Contacts();
var options = new SubscriberRequestOptions { OtpHashKey = new string('k', 32), MaxSendsPer15Minutes = 20 };
var registered = new RegisterSubscriberRequestHandler();
var service = new SubscriberRequestService(store, contacts, sms, validator, options, [registered], clock);
SubscriberRequestCreateDto Input() => new(SubscriberRequestCode.ChangeMobile, "10018315", Json("""{"newMobile":"09123456789"}"""));
await Reject(() => service.Create(Input() with { Payload = Json("{}") }, owner, default), "invalid payload never sends SMS");
Check(sms.Count == 0 && store.Drafts.Count == 0, "no side effects before validation");
var receipt = await service.Create(Input(), owner, default);
var id = receipt.Id;
var code = sms.Code;
Check(receipt.Status == SubscriberRequestStatus.Pending && store.Submissions.Count == 0, "pending has no business effect");
Check(receipt.OtpExpiresAt == clock.GetUtcNow().UtcDateTime.AddSeconds(120), "120 second validity");
Check(store.Challenges.Single().CodeHash.Length == 32, "hash storage");
store.Challenges[0].ExpiresAt = DateTime.SpecifyKind(store.Challenges[0].ExpiresAt!.Value, DateTimeKind.Unspecified);
Check((await service.Get(id, owner, default)).OtpExpiresAt!.Value.Kind == DateTimeKind.Utc, "SQL datetime expiry returned as UTC");
await Reject(() => service.Get(id, Guid.NewGuid(), default), "owner isolation");
await Reject(() => service.Confirm(id, code, Guid.NewGuid(), default), "confirm owner isolation");
await Reject(() => service.Resend(id, owner, default), "resend cooldown");
await Reject(() => service.Confirm(id, "bad", owner, default), "bad code");
Check(store.Drafts[id].FailedAttempts == 1, "failed attempt committed");
clock.Advance(120);
await Reject(() => service.Confirm(id, code, owner, default), "expiry at exactly 120 seconds");
await service.Resend(id, owner, default);
Check(store.Challenges.Count == 2 && store.Drafts[id].FailedAttempts == 1, "resend preserves failed attempts");
var previousChallenge = store.Challenges[0];
var currentChallenge = store.Challenges[1];
var otp = new SubscriberRequestOtp(options);
Check(!otp.Verify(id, currentChallenge.Id, code, previousChallenge.CodeHash), "codes bound to challenge");
Check(!otp.Verify(Guid.NewGuid(), previousChallenge.Id, code, previousChallenge.CodeHash), "codes bound to request");
var confirmed = await service.Confirm(id, sms.Code, owner, default);
Check(confirmed.Status == SubscriberRequestStatus.Completed && store.Submissions.Count == 1, "successful finalization");
await service.Confirm(id, "anything", owner, default);
Check(store.Submissions.Count == 1, "repeated confirmation idempotent");
await Reject(() => service.Resend(id, owner, default), "completed cannot resend");

clock.Advance(901);
var locked = await service.Create(Input(), owner, default);
for (int i = 0; i < 5; i++) await Reject(() => service.Confirm(locked.Id, "invalid", owner, default), "attempt limit");
Check(store.Drafts[locked.Id].Status == SubscriberRequestStatus.Locked, "locked after five failures");
await Reject(() => service.Confirm(locked.Id, sms.Code, owner, default), "correct code cannot unlock");
await Reject(() => service.Resend(locked.Id, owner, default), "locked cannot resend");

clock.Advance(901);
sms.Fail = true;
await Reject(() => service.Create(Input(), owner, default), "SMS failure");
var failed = store.Drafts.Values.OrderByDescending(x => x.CreatedAt).First();
Check(store.Challenges.Last().DeliveryFailed && store.Challenges.Last().SentAt is null, "SMS failure persisted");
await Reject(() => service.Confirm(failed.Id, sms.Code, owner, default), "failed delivery unusable");
sms.Fail = false;
clock.Advance(31);
await service.Resend(failed.Id, owner, default);
Check((await service.Confirm(failed.Id, sms.Code, owner, default)).Status == SubscriberRequestStatus.Completed, "SMS retry recovers");

clock.Advance(901);
var flaky = new FlakyHandler();
store.Definition.HandlerName = flaky.Name;
var retryService = new SubscriberRequestService(store, contacts, sms, validator, options, [flaky], clock);
var retry = await retryService.Create(Input(), owner, default);
await Reject(() => retryService.Confirm(retry.Id, sms.Code, owner, default), "handler failure");
Check(store.Drafts[retry.Id].Status == SubscriberRequestStatus.Confirmed && store.Challenges.Last().ConsumedAt is not null, "verified state survives handler failure");
Check(!store.Submissions.Contains(retry.Id), "failed handler writes rolled back");
flaky.Fail = false;
clock.Advance(121);
Check((await retryService.Confirm(retry.Id, "", owner, default)).Status == SubscriberRequestStatus.Completed, "retry verified request without OTP reuse");

clock.Advance(901);
store.Definition.HandlerName = registered.Name;
var parallel = await service.Create(Input(), owner, default);
var parallelCode = sms.Code;
await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => service.Confirm(parallel.Id, parallelCode, owner, default)));
Check(store.Submissions.Count(x => x == parallel.Id) == 1, "parallel confirmations execute once");
clock.Advance(901);
var snapshot = await service.Create(Input(), owner, default);
store.Definition.HandlerName = "FutureHandler";
store.Definition.PayloadSchema = nested;
Check((await service.Confirm(snapshot.Id, sms.Code, owner, default)).Status == SubscriberRequestStatus.Completed, "draft keeps original handler and schema");
store.Definition.HandlerName = registered.Name;
store.Definition.PayloadSchema = schema;

clock.Advance(901);
var delayed = await service.Create(Input(), owner, default);
contacts.OnResolve = () => clock.Advance(120);
await Reject(() => service.Confirm(delayed.Id, sms.Code, owner, default), "expiry during contact lookup");
contacts.OnResolve = null;

clock.Advance(901);
using var cancellation = new CancellationTokenSource();
sms.OnSend = () => cancellation.Cancel();
var disconnected = await service.Create(Input(), owner, cancellation.Token);
Check(store.Drafts.ContainsKey(disconnected.Id) && !sms.LastToken.CanBeCanceled, "disconnect during SMS preserves rate limit record");
sms.OnSend = null;

var typed = new TypedMobileHandler();
await using (var typedTx = await store.Begin(owner, default))
{
    var draft = new SubscriberRequestDraft { Payload = """{"newMobile":"09123456789"}""" };
    Check((await typed.Handle(draft, typedTx, default)).GetProperty("newMobile").GetString() == "09123456789", "JSON converted to typed DTO");
    draft.Payload = """{"newMobile":123}""";
    await Reject(() => typed.Handle(draft, typedTx, default), "DTO conversion mismatch is BaseException");
    draft.Payload = """{"newMobile":"09123456789","extra":1}""";
    await Reject(() => typed.Handle(draft, typedTx, default), "DTO rejects unmapped property");
}
clock.Advance(901);
contacts.Mobile = "09111111111";
var changedContact = await service.Create(Input(), owner, default);
contacts.Mobile = "09222222222";
await Reject(() => service.Confirm(changedContact.Id, sms.Code, owner, default), "mobile changed after creation");
await Reject(() => service.Resend(changedContact.Id, owner, default), "resend cannot change recipient");

clock.Advance(901);
options.MaxSendsPer15Minutes = 1;
await service.Create(Input(), owner, default);
await Reject(() => service.Create(Input(), owner, default), "send rate limit");
store.Definition.HandlerName = "UnregisteredHandler";
await Reject(() => service.Create(Input(), owner, default), "unknown handler rejected");
store.Definition.IsEnabled = false;
await Reject(() => service.Create(Input(), owner, default), "disabled service rejected");

var sql = File.ReadAllText(Path.Combine(args.Length == 0 ? Directory.GetCurrentDirectory() : args[0],
    "docs/subscriber-requests/sql/001_SubscriberRequests_Atlas.sql"));
var rows = Regex.Matches(sql, @"\((\d{11}),N'([^']+)',N'([^']+)',N'([^']+)'\)");
Check(rows.Count == 28 && Enum.GetValues<SubscriberRequestCode>().Length == 28, "28 CRM services");
foreach (Match row in rows)
{
    var requestCode = (SubscriberRequestCode)long.Parse(row.Groups[1].Value);
    Check(Enum.IsDefined(requestCode), "SQL seed matches enum");
    var jsonSchema = "{\"type\":\"object\",\"additionalProperties\":false,\"properties\":" + row.Groups[3].Value + ",\"required\":" + row.Groups[4].Value + "}";
    try { validator.Validate(jsonSchema, Json("{}")); }
    catch (SubscriberRequestPayloadException) { checks++; }
}
Console.WriteLine($"PASS: {checks} subscriber request checks.");

sealed class ManualTime : TimeProvider
{
    private DateTimeOffset _now = new(2026, 11, 3, 0, 0, 0, TimeSpan.Zero);
    public override DateTimeOffset GetUtcNow() => _now;
    public void Advance(int seconds) => _now = _now.AddSeconds(seconds);
}
sealed class CapturingSms : ISubscriberRequestOtpSender
{
    public string Code = "";
    public int Count;
    public bool Fail;
    public Action? OnSend;
    public CancellationToken LastToken;
    public Task Send(string mobile, string code, Guid id, CancellationToken ct)
    {
        Count++; Code = code; LastToken = ct; OnSend?.Invoke();
        return Fail ? Task.FromException(new HttpRequestException("Unavailable")) : Task.CompletedTask;
    }
}
sealed class Contacts : ISubscriberRequestContactResolver
{
    public string Mobile = "09123456789";
    public Action? OnResolve;
    public Task<SubscriberRequestContact> Resolve(Guid owner, string bill, CancellationToken ct)
    {
        OnResolve?.Invoke();
        return Task.FromResult(new SubscriberRequestContact(bill, Mobile));
    }
}
sealed record MobilePayload(string NewMobile);
sealed class TypedMobileHandler : TypedSubscriberRequestHandler<MobilePayload>
{
    public override string Name => nameof(TypedMobileHandler);
    protected override Task<JsonElement> Handle(MobilePayload payload, SubscriberRequestDraft draft, ISubscriberRequestTransaction tx, CancellationToken ct) =>
        Task.FromResult(JsonSerializer.SerializeToElement(new { newMobile = payload.NewMobile }));
}
sealed class FlakyHandler : ISubscriberRequestHandler
{
    public string Name => nameof(FlakyHandler);
    public bool Fail = true;
    public async Task<JsonElement> Handle(SubscriberRequestDraft draft, ISubscriberRequestTransaction tx, CancellationToken ct)
    {
        await tx.RegisterSubmission(draft, ct);
        if (Fail) throw new SubscriberRequestException("Business failure");
        return JsonSerializer.SerializeToElement(new { registered = true });
    }
}

sealed class MemoryStore(string schema) : ISubscriberRequestStore
{
    public SubscriberRequestDefinition Definition = new() { RequestCode = SubscriberRequestCode.ChangeMobile, Version = 1, HandlerName = nameof(RegisterSubscriberRequestHandler), PayloadSchema = schema, IsEnabled = true };
    public Dictionary<Guid, SubscriberRequestDraft> Drafts = [];
    public List<SubscriberRequestChallenge> Challenges = [];
    public List<Guid> Submissions = [];
    private readonly SemaphoreSlim _gate = new(1);
    public Task<IReadOnlyList<SubscriberRequestDefinition>> GetDefinitions(CancellationToken ct) => Task.FromResult<IReadOnlyList<SubscriberRequestDefinition>>([Definition]);
    public async Task<ISubscriberRequestTransaction> Begin(Guid owner, CancellationToken ct)
    {
        await _gate.WaitAsync(ct);
        return new Transaction(this, _gate);
    }

    private sealed class Transaction : ISubscriberRequestTransaction
    {
        private readonly MemoryStore _store;
        private readonly SemaphoreSlim _gate;
        private bool _closed;
        private readonly Dictionary<Guid, SubscriberRequestDraft> _drafts;
        private readonly List<SubscriberRequestChallenge> _challenges;
        private readonly List<Guid> _submissions;
        private static T Clone<T>(T source) => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source))!;
        public Transaction(MemoryStore store, SemaphoreSlim gate)
        {
            _store = store; _gate = gate;
            _drafts = Clone(store.Drafts); _challenges = Clone(store.Challenges); _submissions = [..store.Submissions];
        }
        public Task<SubscriberRequestDefinition?> GetDefinition(SubscriberRequestCode code, CancellationToken ct) => Task.FromResult<SubscriberRequestDefinition?>(_store.Definition);
        public Task<SubscriberRequestDraft?> GetDraft(Guid id, Guid owner, CancellationToken ct) => Task.FromResult(_drafts.TryGetValue(id, out var draft) && draft.OwnerId == owner ? draft : null);
        public Task InsertDraft(SubscriberRequestDraft draft, CancellationToken ct) { _drafts.Add(draft.Id, draft); return Task.CompletedTask; }
        public Task SaveDraft(SubscriberRequestDraft draft, CancellationToken ct) { _drafts[draft.Id] = draft; return Task.CompletedTask; }
        public Task<SubscriberRequestChallenge?> GetLatestChallenge(Guid id, CancellationToken ct) => Task.FromResult(_challenges.LastOrDefault(x => x.RequestId == id));
        public Task InsertChallenge(SubscriberRequestChallenge challenge, CancellationToken ct) { _challenges.Add(challenge); return Task.CompletedTask; }
        public Task SaveChallenge(SubscriberRequestChallenge challenge, CancellationToken ct) { ct.ThrowIfCancellationRequested(); return Task.CompletedTask; }
        public Task LockMobile(string mobile, CancellationToken ct) => Task.CompletedTask;
        public Task<int> CountRecentChallenges(Guid owner, string mobile, DateTime since, CancellationToken ct) => Task.FromResult(
            _challenges.Count(c => c.CreatedAt >= since && (_drafts[c.RequestId].OwnerId == owner || _drafts[c.RequestId].Mobile == mobile)));
        public Task RegisterSubmission(SubscriberRequestDraft draft, CancellationToken ct) { if (_submissions.Contains(draft.Id)) throw new Exception("Duplicate submission"); _submissions.Add(draft.Id); return Task.CompletedTask; }
        public Task Commit(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (_closed) throw new Exception("Transaction already closed");
            _store.Drafts = _drafts; _store.Challenges = _challenges; _store.Submissions = _submissions;
            _closed = true; _gate.Release(); return Task.CompletedTask;
        }
        public ValueTask DisposeAsync() { if (!_closed) { _closed = true; _gate.Release(); } return ValueTask.CompletedTask; }
    }
}

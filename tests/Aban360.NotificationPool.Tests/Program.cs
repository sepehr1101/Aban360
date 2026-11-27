using System.Net;
using System.Text.Json;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.NotificationPool.Domain.Features.Sms;
using Microsoft.Extensions.DependencyInjection;

Check(typeof(ISmsOldHandler).Assembly.GetName().Name == "Aban360.NotificationPool" &&
    typeof(ISmsOldHandler).Assembly == typeof(SendSmsDataDto).Assembly, "SMS contracts must live in the unified DLL.");

ServiceCollection services = new();
RecordingClientFactory factory = new();
services.AddSingleton<IHttpClientFactory>(factory);
services.Configure<SmsOptions>(options =>
{
    options.ApiKey = "test-api-key";
    options.Sender = "test-sender";
});
services.AddNotificationPoolApplicationInjections();
Check(services.Single(service => service.ServiceType == typeof(ISmsOldHandler)).Lifetime == ServiceLifetime.Scoped,
    "Scrutor must register one scoped SMS handler.");

using ServiceProvider provider = services.BuildServiceProvider();
using IServiceScope scope = provider.CreateScope();
using IServiceScope otherScope = provider.CreateScope();
ISmsOldHandler handler = scope.ServiceProvider.GetRequiredService<ISmsOldHandler>();
Check(ReferenceEquals(handler, scope.ServiceProvider.GetRequiredService<ISmsOldHandler>()) &&
    !ReferenceEquals(handler, otherScope.ServiceProvider.GetRequiredService<ISmsOldHandler>()),
    "SMS handler lifetime changed.");
Check(handler.GetType().Assembly == typeof(ISmsOldHandler).Assembly, "Implementation must live in the unified DLL.");

Guid trackingId = Guid.NewGuid();
const string mobile = "09120000000";
const string message = "تست + پیامک & رایاب";
await handler.SendNew(mobile, message, trackingId);
CapturedRequest current = factory.Requests.Single();
Check(current.Client == HttpClientNames.Sms && current.Method == HttpMethod.Post &&
    current.Uri.AbsolutePath == "/messages" && current.ApiKey == "test-api-key", "New SMS request changed.");
using JsonDocument body = JsonDocument.Parse(current.Body!);
JsonElement root = body.RootElement;
JsonElement sms = root.GetProperty("messages")[0];
Check(root.GetProperty("senderLine").GetString() == "test-sender" &&
    root.GetProperty("messageTypeId").GetByte() == (byte)SmsMessageType.Notification &&
    Guid.TryParseExact(root.GetProperty("clientBatchId").GetString(), "N", out _) &&
    sms.GetProperty("recipient").GetString() == mobile && sms.GetProperty("text").GetString() == message &&
    sms.GetProperty("clientCorrelatedId").GetString() == trackingId.ToString(), "New SMS payload changed.");

await handler.Send(mobile, message, trackingId);
CapturedRequest legacy = factory.Requests[1];
Check(legacy.Client == "172" && legacy.Method == HttpMethod.Get &&
    legacy.Uri.AbsolutePath == "/CRM/Sms/SendManual" &&
    legacy.Uri.Query.Contains("mobile=" + mobile, StringComparison.Ordinal) &&
    legacy.Uri.Query.Contains("text=" + Uri.EscapeDataString(message), StringComparison.Ordinal) &&
    legacy.Uri.Query.Contains("trackingId=" + trackingId, StringComparison.Ordinal), "Legacy SMS request changed.");

await handler.Send(Array.Empty<SendSmsDataDto>());
Check(factory.Requests.Count == 2, "The currently disabled batch overload must make no HTTP request.");
factory.ResponseStatus = HttpStatusCode.BadGateway;
try
{
    await handler.SendNew(mobile, message, trackingId);
    throw new InvalidOperationException("SMS HTTP failures must propagate.");
}
catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.BadGateway)
{
}

Console.WriteLine("PASS: NotificationPool assembly, scoped DI, new and legacy SMS requests, batch behavior and HTTP failures.");

static void Check(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException(message);
}

sealed record CapturedRequest(string Client, HttpMethod Method, Uri Uri, string? ApiKey, string? Body);

sealed class RecordingClientFactory : IHttpClientFactory
{
    public List<CapturedRequest> Requests { get; } = new();
    public HttpStatusCode ResponseStatus { get; set; } = HttpStatusCode.OK;

    public HttpClient CreateClient(string name) => new(new Recorder(this, name))
    {
        BaseAddress = new Uri("https://sms.test/")
    };

    private sealed class Recorder(RecordingClientFactory factory, string name) : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
            string? apiKey = request.Headers.TryGetValues("X-Api-Key", out IEnumerable<string>? values) ? values.Single() : null;
            factory.Requests.Add(new CapturedRequest(name, request.Method, request.RequestUri!, apiKey, body));
            return new HttpResponseMessage(factory.ResponseStatus) { Content = new StringContent("{}") };
        }
    }
}

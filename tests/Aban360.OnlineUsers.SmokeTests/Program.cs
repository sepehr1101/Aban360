using System.Reflection;
using System.Security.Claims;
using Aban360.Api.Extensions;
using Aban360.Api.Hubs.Contracts;
using Aban360.ReportPool.Application.Features.FlatReports.Services;
using Aban360.ReportPool.Application.Features.FlatReports.Validations;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Aban360.Api.Hubs.Implementations;
using Aban360.CommunicationPool.Application.Features.Hubs.Services;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;
using Aban360.CommunicationPool.Persistence.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Aban360.CommunicationPool.Application.Features.Hubs.Handlers;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Queries;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Queries.Contracts;

int checks = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new Exception("FAIL: " + message);
    checks++;
    Console.WriteLine("PASS: " + message);
}

var query = new OnlineConnections();
var type = typeof(IOnlineUserGetHandler).Assembly.GetType(
    "Aban360.CommunicationPool.Application.Features.Hubs.Mappers.OnlineUserGetHandler", true)!;
var handler = (IOnlineUserGetHandler)Activator.CreateInstance(type, query)!;
var userId = Guid.NewGuid();
var otherUserId = Guid.NewGuid();
var now = new DateTime(2026, 11, 5, 12, 0, 0);
OnlineUserGetDto Row(long id, Guid user, string connection, DateTime when) => new()
{
    Id = id, UserId = user, FullName = "نام یکسان", ConnectionId = connection, ConnectDateTime = when
};
query.Rows = new[]
{
    Row(1, userId, "tab-1", now.AddMinutes(-5)),
    Row(2, userId, "tab-2", now),
    Row(3, userId, "tab-2", now),
    Row(4, otherUserId, "other-user", now.AddMinutes(-2))
};

var users = (await handler.Handle(CancellationToken.None)).ToArray();
Check(users.Length == 2, "Multiple tabs produce one row per user ID");
var firstUser = users.Single(user => user.UserId == userId);
Check(firstUser.ConnectionCount == 2 && firstUser.ConnectionIds.SequenceEqual(new[] { "tab-2", "tab-1" }),
    "Connection IDs are distinct and include all tabs");
Check(firstUser.ConnectionId == "tab-2" && firstUser.Id == 3 && firstUser.ConnectDateTime == now,
    "Existing connection fields deterministically represent the most recent connection");
Check(users[1].UserId == otherUserId && users[1].ConnectionCount == 1,
    "Different users with the same name remain separate users");

query.Rows = query.Rows.Where(row => row.ConnectionId != "tab-2").ToArray();
users = (await handler.Handle(CancellationToken.None)).ToArray();
Check(users.Single(user => user.UserId == userId).ConnectionCount == 1,
    "Closing one tab leaves the user online through their remaining connection");
query.Rows = query.Rows.Where(row => row.UserId != userId).ToArray();
Check((await handler.Handle(CancellationToken.None)).All(user => user.UserId != userId),
    "User disappears after their final connection is removed");

query.Rows = Array.Empty<OnlineUserGetDto>();
Check(!(await handler.Handle(CancellationToken.None)).Any(), "Empty connections produce an empty user list");

var registry = new HubConnectionRegistry();
var events = new HubEvents();
NotifyHub Hub(string id) => new(events, registry) { Context = new Caller(id, userId) };
using var firstTab = Hub("tab-a");
using var secondTab = Hub("tab-b");
await firstTab.OnConnectedAsync();
await secondTab.OnConnectedAsync();
Check(events.Connections.Count == 2 && registry.GetConnectionIds().Length == 2,
    "Hub registration keeps both tabs for the same user");
await firstTab.OnDisconnectedAsync(null);
Check(events.Connections.Keys.SequenceEqual(new[] { "tab-b" }) && registry.GetConnectionIds().SequenceEqual(new[] { "tab-b" }),
    "Disconnect removes only the specific tab from audit state and heartbeat tracking");
using var reconnected = Hub("tab-a-reconnected");
await reconnected.OnConnectedAsync();
Check(registry.GetConnectionIds().Order().SequenceEqual(new[] { "tab-a-reconnected", "tab-b" }),
    "Reconnection gets its own connection without removing the other tab");

var presence = new Presence();
var services = new ServiceCollection();
services.AddScoped<IHubPresenceService>(_ => presence);
using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
using var heartbeat = new HubPresenceHeartbeatService(registry, provider.GetRequiredService<IServiceScopeFactory>(),
    NullLogger<HubPresenceHeartbeatService>.Instance);
using var cancellation = new CancellationTokenSource();
await heartbeat.RefreshAsync(cancellation.Token);
Check(presence.Ids.Order().SequenceEqual(new[] { "tab-a-reconnected", "tab-b" }) && presence.Token == cancellation.Token,
    "Heartbeat renews all locally live connection IDs through a scoped shared persistence service");
events.FailClose = true;
try { await secondTab.OnDisconnectedAsync(null); }
catch (InvalidOperationException) { }
await heartbeat.RefreshAsync(CancellationToken.None);
Check(presence.Ids.SequenceEqual(new[] { "tab-a-reconnected" }),
    "Failed database disconnect never keeps the closed tab in future heartbeat renewals");
registry.Remove("tab-a-reconnected");
int calls = presence.Calls;
await heartbeat.RefreshAsync(CancellationToken.None);
Check(presence.Calls == calls, "Empty registry performs no database heartbeat");
Parallel.For(0, 1000, i => registry.Add("parallel-" + i));
Parallel.For(0, 1000, i => registry.Remove("parallel-" + i));
Check(registry.GetConnectionIds().Length == 0, "Concurrent connections and disconnections do not corrupt the registry");

events.FailCreate = true;
using var failedTab = Hub("failed-tab");
try { await failedTab.OnConnectedAsync(); }
catch (InvalidOperationException) { }
Check(!registry.GetConnectionIds().Contains("failed-tab"), "Failed connection registration is not renewed as a live connection");

var sendCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
string? targetUser = null;
ReportCompletionNotification? notification = null;
bool failSend = false;
int sends = 0;
var client = TestProxy.For<INotifyHub>((method, args) =>
{
    if (method.Name != nameof(INotifyHub.InformReportCompletion)) throw new Exception("Unexpected notification");
    notification = (ReportCompletionNotification)args[0]!;
    sends++;
    return failSend ? Task.FromException(new InvalidOperationException("Transport failed")) : sendCompletion.Task;
});
var clients = TestProxy.For<IHubClients<INotifyHub>>((method, args) =>
{
    if (method.Name != "User") throw new Exception("Notification must route to the owner, not a connection or broadcast");
    targetUser = (string)args[0]!;
    return client;
});
var hubContext = TestProxy.For<IHubContext<NotifyHub, INotifyHub>>((method, _) =>
    method.Name == "get_Clients" ? clients : throw new Exception("Unexpected hub operation"));
var notifier = new ReportCompletionNotifier(hubContext, NullLogger<ReportCompletionNotifier>.Instance);
var reportId = Guid.NewGuid();
var notificationTask = notifier.Notify(userId, "گزارش آزمایشی", reportId);
Check(targetUser == userId.ToString() && notification?.ReportId == reportId,
    "Report notification routes to the report owner's user ID across their current connections");
Check(!notificationTask.IsCompleted, "Report notification awaits the SignalR send task");
sendCompletion.SetResult();
await notificationTask;
failSend = true;
await notifier.Notify(userId, "گزارش آزمایشی", reportId);
Check(sends == 2, "Notification transport failure is contained without retrying completed report generation");

var saveCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
CancellationToken saveToken = default;
bool failSave = false;
var saveService = TestProxy.For<IServerReportsUpdateService>((_, args) =>
{
    saveToken = (CancellationToken)args[1]!;
    return failSave ? Task.FromException(new InvalidOperationException("Save failed")) : saveCompletion.Task;
});
var updateType = typeof(IServerReportsService).Assembly.GetTypes().Single(type => type.IsClass && typeof(IServerReportsService).IsAssignableFrom(type));
var updateHandler = (IServerReportsService)updateType.GetConstructors().Single().Invoke(updateType.GetConstructors().Single().GetParameters().Select(parameter =>
{
    if (parameter.ParameterType == typeof(IServerReportsUpdateService)) return (object)saveService;
    if (parameter.ParameterType == typeof(FluentValidation.IValidator<ServerReportsUpdateDto>)) return new ServerReportsUpdateValidator();
    var proxy = (TestProxy)DispatchProxy.Create(parameter.ParameterType, typeof(TestProxy));
    proxy.Callback = (_, _) => throw new Exception("Unexpected report dependency call");
    return (object)proxy;
}).ToArray());
var updateTask = updateHandler.UpdateAsync(new ServerReportsUpdateDto(reportId, "test-report.xlsx", now, true), cancellation.Token);
Check(!updateTask.IsCompleted && saveToken == cancellation.Token,
    "Report completion handler awaits persistence and forwards cancellation");
saveCompletion.SetResult();
await updateTask;
failSave = true;
bool saveFailed = false;
try { await updateHandler.UpdateAsync(new ServerReportsUpdateDto(reportId, "test-report.xlsx", now, true), CancellationToken.None); }
catch (InvalidOperationException) { saveFailed = true; }
Check(saveFailed, "Report persistence errors propagate through Task instead of async void");

var tokenHandler = typeof(ConfigureAuth).GetMethod("HandleMessageReceived", BindingFlags.NonPublic | BindingFlags.Static)!;
async Task<MessageReceivedContext> ReadToken(string path, string queryString, bool authorizationHeader = false)
{
    var context = new DefaultHttpContext();
    context.Request.Path = path;
    context.Request.QueryString = new QueryString(queryString);
    if (authorizationHeader) context.Request.Headers.Authorization = "Bearer header-token";
    var received = new MessageReceivedContext(context,
        new AuthenticationScheme("Bearer", null, typeof(JwtBearerHandler)), new JwtBearerOptions());
    await (Task)tokenHandler.Invoke(null, new object[] { received })!;
    return received;
}
var hubToken = await ReadToken("/v1/notify-hub", "?access_token=query-token");
Check(hubToken.Token == "query-token" && !hubToken.Request.Headers.ContainsKey("Authorization"),
    "Hub query token is read without copying credentials into request headers");
Check((await ReadToken("/v1/notify-hub/negotiate", "?access_token=query-token")).Token == "query-token",
    "Hub negotiation accepts the same query token flow");
Check((await ReadToken("/v1/online-user/get", "?access_token=query-token")).Token == "query-token",
    "Existing REST clients using query token authentication remain compatible");
Check((await ReadToken("/v1/server-reports/download/test", "?access_token=query-token")).Token == "query-token",
    "Existing download URLs using query token authentication remain compatible");
Check((await ReadToken("/v1/notify-hub", "?access_token=query-token", true)).Token is null,
    "Authorization header takes priority over a query token");
Check((await ReadToken("/v1/notify-hub", "?access_token=one&access_token=two")).Token is null,
    "Multiple query token values are rejected");

var apiServices = new ServiceCollection();
apiServices.AddLogging();
apiServices.AddSignalR();
apiServices.AddApiInjections();
using var apiProvider = apiServices.BuildServiceProvider();
using var apiScope = apiProvider.CreateScope();
Check(apiScope.ServiceProvider.GetRequiredService<IReportCompletionNotifier>() is ReportCompletionNotifier,
    "Report completion notifier resolves through the application's existing registration scan");
var persistenceServices = new ServiceCollection();
persistenceServices.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
persistenceServices.AddCommunicationPoolPersistenceInjections();
using var persistenceProvider = persistenceServices.BuildServiceProvider();
using var persistenceScope = persistenceProvider.CreateScope();
Check(persistenceScope.ServiceProvider.GetRequiredService<IHubPresenceService>() is not null,
    "Shared heartbeat persistence resolves through the existing communication registration scan");
Console.WriteLine($"{checks} checks passed.");

sealed class OnlineConnections : IOnlineUsersGetQueryService
{
    public IEnumerable<OnlineUserGetDto> Rows = Array.Empty<OnlineUserGetDto>();
    public Task<IEnumerable<OnlineUserGetDto>> Get() => Task.FromResult(Rows);
}

sealed class HubEvents : IHubEventService
{
    public Dictionary<string, Guid> Connections = new();
    public bool FailClose;
    public bool FailCreate;
    public Task CreateAsync(HubEventCreateDto input, CancellationToken token)
    {
        if (FailCreate) throw new InvalidOperationException("Registration failed");
        Connections.Add(input.ConnectionId, input.UserId);
        return Task.CompletedTask;
    }
    public Task UpdateAsync(HubEventUpdateDto input, CancellationToken token)
    {
        if (FailClose) throw new InvalidOperationException("Database unavailable");
        Connections.Remove(input.ConnectionId);
        return Task.CompletedTask;
    }
}

class TestProxy : DispatchProxy
{
    public Func<MethodInfo, object?[], object?> Callback = null!;
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) => Callback(targetMethod!, args!);
    public static T For<T>(Func<MethodInfo, object?[], object?> callback) where T : class
    {
        var proxy = Create<T, TestProxy>();
        ((TestProxy)(object)proxy).Callback = callback;
        return proxy;
    }
}

sealed class Presence : IHubPresenceService
{
    public string[] Ids = Array.Empty<string>();
    public CancellationToken Token;
    public int Calls;
    public Task Refresh(IReadOnlyCollection<string> connectionIds, CancellationToken token)
    {
        Ids = connectionIds.ToArray();
        Token = token;
        Calls++;
        return Task.CompletedTask;
    }
}

sealed class Caller(string id, Guid userId) : HubCallerContext
{
    public override string ConnectionId => id;
    public override string UserIdentifier => userId.ToString();
    public override ClaimsPrincipal User { get; } = new(new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, userId.ToString()) }));
    public override IDictionary<object, object?> Items { get; } = new Dictionary<object, object?>();
    public override IFeatureCollection Features { get; } = new FeatureCollection();
    public override CancellationToken ConnectionAborted => CancellationToken.None;
    public override void Abort() { }
}

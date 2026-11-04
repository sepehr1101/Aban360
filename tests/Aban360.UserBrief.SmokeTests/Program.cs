using System.Text.Json;
using Aban360.Api.Controllers.V1.UserPool.Auth.Queries;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.Implementation;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var checks = 0;
void Check(bool value, string name)
{
    if (!value) throw new Exception(name);
    checks++;
}
var builder = WebApplication.CreateBuilder();
builder.WebHost.UseTestServer();
var database = Guid.NewGuid().ToString();
builder.Services.AddDbContext<UserPoolContext>(options => options.UseInMemoryDatabase(database));
builder.Services.AddUserPoolPersistenceInjections();
builder.Services.AddScoped<IUserBriefQueryHandler, UserBriefQueryHandler>();
builder.Services.AddSingleton<IUserAllQueryHandler, ExistingAllHandler>();
builder.Services.AddControllers().AddApplicationPart(typeof(UserAllQueryController).Assembly);
await using var app = builder.Build();
app.MapControllers();
var activeId = Guid.NewGuid();
var lockedId = Guid.NewGuid();
var removedId = Guid.NewGuid();
using (var scope = app.Services.CreateScope())
{
    var context = (UserPoolContext)scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    User Make(Guid id, string name) => new()
    {
        Id = id, DisplayName = name, FullName = "Private name", Username = id.ToString(),
        Password = "Private password", Mobile = "Private mobile", Hash = "test", InsertLogInfo = "test"
    };
    var active = Make(activeId, "A");
    var locked = Make(lockedId, "B");
    locked.LockTimespan = DateTime.Now.AddDays(1);
    var removed = Make(removedId, "C");
    removed.ValidTo = DateTime.Now;
    context.Users.AddRange(active, locked, removed);
    await context.SaveChangesAsync();
    context.ChangeTracker.Clear();
    var handler = scope.ServiceProvider.GetRequiredService<IUserBriefQueryHandler>();
    var result = await handler.Handle(default);
    Check(result.Count == 2 && result.Select(x => x.Id).SequenceEqual(new[] { activeId, lockedId }),
        "Brief list excludes removed users and preserves locked users as existing all does");
    Check(!context.ChangeTracker.Entries().Any(), "Lookup does not track user entities");
    using var cancelled = new CancellationTokenSource();
    cancelled.Cancel();
    try { await handler.Handle(cancelled.Token); throw new Exception("Cancellation ignored"); }
    catch (OperationCanceledException) { checks++; }
}
await app.StartAsync();
using var client = app.GetTestClient();
foreach (var method in new[] { HttpMethod.Get, HttpMethod.Post })
{
    using var request = new HttpRequestMessage(method, "/v1/user/all-brief");
    using var response = await client.SendAsync(request);
    Check(response.IsSuccessStatusCode, "GET and POST brief route are available without input");
    using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    Check(json.RootElement.ValueKind == JsonValueKind.Array && json.RootElement.GetArrayLength() == 2,
        "Brief response remains a direct array");
    foreach (var user in json.RootElement.EnumerateArray())
        Check(user.EnumerateObject().Select(p => p.Name).Order().SequenceEqual(new[] { "displayName", "id" }),
            "HTTP output exposes exactly id and displayName");
}
using (var response = await client.GetAsync("/v1/user/all"))
{
    using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    Check(response.IsSuccessStatusCode && json.RootElement[0].GetProperty("mobile").GetString() == "Existing payload",
        "Existing all action still uses its original handler and payload");
}
using (var scope = app.Services.CreateScope())
{
    var context = (UserPoolContext)scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    context.Users.RemoveRange(await context.Users.ToListAsync());
    await context.SaveChangesAsync();
}
using (var response = await client.GetAsync("/v1/user/all-brief"))
    Check(response.IsSuccessStatusCode && await response.Content.ReadAsStringAsync() == "[]", "Empty lookup returns an empty array");
await app.StopAsync();
Console.WriteLine($"PASS: {checks} user brief checks. No SQL Server connection was opened.");

sealed class ExistingAllHandler : IUserAllQueryHandler
{
    public Task<ICollection<UserQueryDto>> Handle(CancellationToken cancellationToken)
        => Task.FromResult<ICollection<UserQueryDto>>(new List<UserQueryDto> { new() { Mobile = "Existing payload" } });
}

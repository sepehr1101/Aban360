using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Aban360.Api.Controllers.V1.UserPool.Ticketing.Commands;
using Aban360.Api.ExceptionHandlers;
using Aban360.UserPool.Application.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.Implementation;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

internal static class ApiChecks
{
    public static async Task Run()
    {
        var checks = 0;
        void Check(bool condition, string name)
        {
            if (!condition) throw new Exception(name);
            checks++;
        }
        async Task<JsonElement> Data(HttpResponseMessage response)
        {
            var text = await response.Content.ReadAsStringAsync();
            Check(response.IsSuccessStatusCode, $"HTTP success: {response.StatusCode} {text}");
            using var document = JsonDocument.Parse(text);
            Check(document.RootElement.GetProperty("success").GetBoolean(), "Standard response envelope");
            return document.RootElement.GetProperty("data").Clone();
        }

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Testing" });
        builder.Logging.ClearProviders();
        builder.WebHost.UseTestServer();
        var databaseName = Guid.NewGuid().ToString();
        builder.Services.AddDbContext<UserPoolContext>(options => options.UseInMemoryDatabase(databaseName));
        builder.Services.AddUserPoolPersistenceInjections();
        builder.Services.AddUserPoolApplicationInjections();
        builder.Services.AddControllers().AddApplicationPart(typeof(TicketCreateController).Assembly);
        builder.Services.AddAuthentication("TicketingSmoke")
            .AddScheme<AuthenticationSchemeOptions, TicketingSmokeAuthenticationHandler>("TicketingSmoke", _ => { });
        builder.Services.AddAuthorization();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        await using var app = builder.Build();
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        var owner = Guid.NewGuid();
        var agent = Guid.NewGuid();
        var stranger = Guid.NewGuid();
        using (var seedScope = app.Services.CreateScope())
        {
            var context = (UserPoolContext)seedScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            context.Users.AddRange(new[] { owner, agent, stranger }.Select(id => new User
            {
                Id = id, FullName = "Test", DisplayName = "Test", Username = id.ToString(),
                Password = "unused", Mobile = "000", Hash = "unused", InsertLogInfo = "test"
            }));
            context.TicketStatuses.AddRange(new[] { "New", "InProgress", "Answered", "Closed" }
                .Select((code, i) => new TicketStatus { Id = (short)(i + 1), Code = code, Title = code }));
            await context.SaveChangesAsync();
        }
        await app.StartAsync();
        using var client = app.GetTestClient();
        Check((await client.GetAsync("/v1/ticket-status/list")).StatusCode == HttpStatusCode.Unauthorized, "Anonymous requests rejected");
        void ActAs(Guid id, bool admin = false)
        {
            client.DefaultRequestHeaders.Remove("X-Test-User");
            client.DefaultRequestHeaders.Remove("X-Test-Admin");
            client.DefaultRequestHeaders.Add("X-Test-User", id.ToString());
            if (admin) client.DefaultRequestHeaders.Add("X-Test-Admin", "true");
        }
        var png = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 1, 2, 3, 4 };
        ByteArrayContent FilePart(string contentType = "image/png")
        {
            var part = new ByteArrayContent(png);
            part.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            return part;
        }

        ActAs(owner);
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent("Pressure"), "Subject");
        form.Add(new StringContent("Please check"), "Body");
        form.Add(FilePart(), "Attachments", "first.png");
        form.Add(FilePart(), "Attachments", "second.png");
        var number = (await Data(await client.PostAsync("/v1/ticket/create", form))).GetString();
        var page = await Data(await client.PostAsJsonAsync("/v1/ticket/list", new { pageNumber = 1, pageSize = 20 }));
        Check(page.GetProperty("totalCount").GetInt32() == 1, "Create controller commits ticket");
        var ticket = page.GetProperty("items")[0];
        var ticketId = ticket.GetProperty("id").GetInt64();
        Check(ticket.GetProperty("ticketNumber").GetString() == number, "Create returns saved ticket number");
        await Data(await client.GetAsync($"/v1/ticket/read/{ticketId}"));
        var history = await Data(await client.PostAsJsonAsync("/v1/ticket-message/list", new { ticketId }));
        var initialId = history.GetProperty("items")[0].GetProperty("id").GetInt64();
        await Data(await client.GetAsync($"/v1/ticket-message/read/{initialId}"));
        var files = await Data(await client.GetAsync($"/v1/ticket-attachment/list/{initialId}"));
        Check(files.GetArrayLength() == 2 && !files[0].TryGetProperty("fileContent", out _), "Multipart files saved and metadata excludes bytes");
        var fileId = files[0].GetProperty("id").GetInt64();
        await Data(await client.GetAsync($"/v1/ticket-attachment/read/{fileId}"));
        using (var content = await client.GetAsync($"/v1/ticket-attachment/content/{fileId}"))
        {
            Check(content.StatusCode == HttpStatusCode.OK && (await content.Content.ReadAsByteArrayAsync()).SequenceEqual(png), "Binary content response");
            Check(content.Content.Headers.ContentType?.MediaType == "image/png", "Correct file MIME type");
            Check(content.Content.Headers.ContentDisposition == null, "Preview does not force download");
            Check(content.Headers.CacheControl?.NoStore == true, "Private file response is not cached");
        }
        using (var rangeRequest = new HttpRequestMessage(HttpMethod.Get, $"/v1/ticket-attachment/content/{fileId}"))
        {
            rangeRequest.Headers.Range = new RangeHeaderValue(0, 3);
            using var response = await client.SendAsync(rangeRequest);
            Check(response.StatusCode == HttpStatusCode.PartialContent && (await response.Content.ReadAsByteArrayAsync()).SequenceEqual(png[..4]), "Range returns requested bytes");
            Check(response.Content.Headers.ContentRange?.Length == png.Length, "Range includes full content length");
        }
        using (var invalidRange = new HttpRequestMessage(HttpMethod.Get, $"/v1/ticket-attachment/content/{fileId}"))
        {
            invalidRange.Headers.Range = new RangeHeaderValue(100, 200);
            Check((await client.SendAsync(invalidRange)).StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable, "Invalid range returns 416");
        }
        using (var download = await client.GetAsync($"/v1/ticket-attachment/content/{fileId}?download=true"))
            Check(download.Content.Headers.ContentDisposition?.DispositionType == "attachment", "Explicit download sets attachment header");

        using var extra = new MultipartFormDataContent();
        extra.Add(new StringContent(initialId.ToString()), "TicketMessageId");
        extra.Add(FilePart(), "File", "extra.png");
        await Data(await client.PostAsync("/v1/ticket-attachment/create", extra));
        using var replacement = new MultipartFormDataContent();
        replacement.Add(new StringContent(fileId.ToString()), "Id");
        replacement.Add(FilePart(), "File", "renamed.png");
        using var replaceRequest = new HttpRequestMessage(HttpMethod.Patch, "/v1/ticket-attachment/update") { Content = replacement };
        await Data(await client.SendAsync(replaceRequest));
        Check((await Data(await client.GetAsync($"/v1/ticket-attachment/read/{fileId}"))).GetProperty("originalFileName").GetString() == "renamed.png", "Multipart PATCH update commits");
        await Data(await client.PostAsJsonAsync("/v1/ticket-message/update", new { id = initialId, body = "Updated" }));

        ActAs(stranger);
        Check((await client.GetAsync($"/v1/ticket-attachment/content/{fileId}")).StatusCode == HttpStatusCode.BadRequest, "Unauthorized file read uses application error response");
        var hidden = await Data(await client.PostAsJsonAsync("/v1/ticket/list", new { createdByUserId = owner }));
        Check(hidden.GetProperty("totalCount").GetInt32() == 0, "HTTP list scoped to current identity");
        ActAs(owner, admin: true);
        await Data(await client.PostAsJsonAsync("/v1/ticket/update", new { id = ticketId, subject = "Pressure", assignedToUserId = agent, statusId = 2 }));
        ActAs(agent);
        using var reply = new MultipartFormDataContent();
        reply.Add(new StringContent(ticketId.ToString()), "TicketId");
        reply.Add(new StringContent("Fixed"), "Body");
        await Data(await client.PostAsync("/v1/ticket-message/create", reply));
        var replyHistory = await Data(await client.PostAsJsonAsync("/v1/ticket-message/list", new { ticketId }));
        var replyId = replyHistory.GetProperty("items")[1].GetProperty("id").GetInt64();
        ActAs(owner);
        var noRating = await Data(await client.GetAsync($"/v1/ticket-rating/read-by-message/{replyId}"));
        Check(noRating.ValueKind == JsonValueKind.Null, "Unrated response has a null payload");
        await Data(await client.PostAsJsonAsync("/v1/ticket-rating/create", new { ticketMessageId = replyId, score = 5 }));
        var rating = await Data(await client.GetAsync($"/v1/ticket-rating/read-by-message/{replyId}"));
        var ratingId = rating.GetProperty("id").GetInt64();
        await Data(await client.PostAsJsonAsync("/v1/ticket-rating/update", new { id = ratingId, score = 4, comment = "Thanks" }));
        Check((await Data(await client.GetAsync($"/v1/ticket-rating/read/{ratingId}"))).GetProperty("score").GetInt32() == 4, "Rating update commits");
        var report = await Data(await client.PostAsJsonAsync("/v1/ticket/report", new { }));
        Check(report.GetProperty("ticketCount").GetInt32() == 1 && report.GetProperty("averageScore").GetDouble() == 4, "Report HTTP result");
        Check((await Data(await client.GetAsync("/v1/ticket-status/list"))).GetArrayLength() == 4, "Status list endpoint");
        await Data(await client.GetAsync("/v1/ticket-status/read/1"));

        using var tooMany = new MultipartFormDataContent();
        tooMany.Add(new StringContent("Too many"), "Subject");
        for (var i = 0; i < 6; i++) tooMany.Add(FilePart(), "Attachments", $"file{i}.png");
        Check((await client.PostAsync("/v1/ticket/create", tooMany)).StatusCode == HttpStatusCode.BadRequest, "Upload count limit enforced");
        using var missingFile = new MultipartFormDataContent();
        missingFile.Add(new StringContent(initialId.ToString()), "TicketMessageId");
        Check((await client.PostAsync("/v1/ticket-attachment/create", missingFile)).StatusCode == HttpStatusCode.BadRequest, "Missing form file rejected by model binding");
        Check((await client.PostAsJsonAsync("/v1/ticket/create", new { subject = "JSON" })).StatusCode == HttpStatusCode.UnsupportedMediaType, "Upload API requires multipart");
        // Dedicated My Tickets endpoint must constrain admins too, and exclude
        // tickets merely assigned to the caller. Other ownership fields are not inputs.
        client.DefaultRequestHeaders.Remove("X-Test-User");
        Check((await client.PostAsJsonAsync("/v1/ticket/my-list", new { })).StatusCode == HttpStatusCode.Unauthorized, "My tickets requires authentication");
        ActAs(stranger);
        using var strangerTicket = new MultipartFormDataContent();
        strangerTicket.Add(new StringContent("Other user's ticket"), "Subject");
        strangerTicket.Add(new StringContent("Private"), "Body");
        await Data(await client.PostAsync("/v1/ticket/create", strangerTicket));
        ActAs(owner);
        using var secondOwnTicket = new MultipartFormDataContent();
        secondOwnTicket.Add(new StringContent("Second own ticket"), "Subject");
        secondOwnTicket.Add(new StringContent("Another request"), "Body");
        await Data(await client.PostAsync("/v1/ticket/create", secondOwnTicket));
        var own = await Data(await client.PostAsJsonAsync("/v1/ticket/my-list", new { }));
        Check(own.GetProperty("totalCount").GetInt32() == 2
            && own.GetProperty("items").EnumerateArray().All(x => x.GetProperty("createdByUserId").GetGuid() == owner), "My tickets includes all own statuses and excludes other users");
        var ownPage = await Data(await client.PostAsJsonAsync("/v1/ticket/my-list", new { pageNumber = 2, pageSize = 1 }));
        Check(ownPage.GetProperty("totalCount").GetInt32() == 2 && ownPage.GetProperty("items").GetArrayLength() == 1, "My tickets pagination");
        var ownNew = await Data(await client.PostAsJsonAsync("/v1/ticket/my-list", new { statusId = 1 }));
        Check(ownNew.GetProperty("totalCount").GetInt32() == 1, "My tickets status filter");
        Check((await client.PostAsJsonAsync("/v1/ticket/my-list", new { pageSize = 101 })).StatusCode == HttpStatusCode.BadRequest, "My tickets shares search validation");
        ActAs(owner, admin: true);
        var adminOwn = await Data(await client.PostAsJsonAsync("/v1/ticket/my-list", new { createdByUserId = stranger, assignedToUserId = agent }));
        Check(adminOwn.GetProperty("totalCount").GetInt32() == 2
            && adminOwn.GetProperty("items").EnumerateArray().All(x => x.GetProperty("createdByUserId").GetGuid() == owner), "My tickets cannot override ownership even for admin");
        ActAs(agent);
        var assignedOnly = await Data(await client.PostAsJsonAsync("/v1/ticket/my-list", new { }));
        Check(assignedOnly.GetProperty("totalCount").GetInt32() == 0, "My tickets excludes assigned tickets created by someone else");
        await app.StopAsync();
        Console.WriteLine($"PASS: {checks} ticketing HTTP checks. No SQL Server connection was opened.");
    }
}

// Test-only authentication. Never registered by the production API.
internal sealed class TicketingSmokeAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Guid.TryParse(Request.Headers["X-Test-User"], out var id))
            return Task.FromResult(AuthenticateResult.NoResult());
        var claims = new List<Claim>
        {
            new(ClaimTypes.UserData, id.ToString()), new(ClaimTypes.Name, id.ToString()), new(ClaimTypes.GivenName, "Test")
        };
        if (Request.Headers["X-Test-Admin"] == "true") claims.Add(new Claim(ClaimTypes.Role, "admin"));
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
    }
}

using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.Implementation;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Extensions;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var checks = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception(name);
    checks++;
}

var services = new ServiceCollection();
var databaseName = Guid.NewGuid().ToString();
services.AddDbContext<UserPoolContext>(options => options.UseInMemoryDatabase(databaseName));
services.AddUserPoolPersistenceInjections();
using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var context = (UserPoolContext)scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
var commands = scope.ServiceProvider.GetRequiredService<ITicketCommandService>();
var messages = scope.ServiceProvider.GetRequiredService<ITicketMessageCommandService>();
var attachments = scope.ServiceProvider.GetRequiredService<ITicketAttachmentCommandService>();
var ratings = scope.ServiceProvider.GetRequiredService<ITicketRatingCommandService>();
var query = scope.ServiceProvider.GetRequiredService<ITicketQueryService>();
var messageQuery = scope.ServiceProvider.GetRequiredService<ITicketMessageQueryService>();
var attachmentQuery = scope.ServiceProvider.GetRequiredService<ITicketAttachmentQueryService>();
var ratingQuery = scope.ServiceProvider.GetRequiredService<ITicketRatingQueryService>();
var statusQuery = scope.ServiceProvider.GetRequiredService<ITicketStatusQueryService>();
var reportQuery = scope.ServiceProvider.GetRequiredService<ITicketReportQueryService>();
Check(true, "All ten persistence contracts resolve through existing registration");

var owner = Guid.NewGuid();
var agent = Guid.NewGuid();
var start = new DateTime(2026, 10, 26, 0, 0, 0, DateTimeKind.Utc);
context.Users.AddRange(new[] { owner, agent }.Select(id => new User
{
    Id = id, FullName = "Test", DisplayName = "Test", Username = id.ToString(),
    Password = "unused", Mobile = "000", Hash = "unused", InsertLogInfo = "test"
}));
context.TicketStatuses.AddRange(
    new TicketStatus { Id = 1, Code = "New", Title = "New" },
    new TicketStatus { Id = 3, Code = "Answered", Title = "Answered" });

var ticket = new Ticket { TicketNumber = "T-1", Subject = "Meter", CreatedByUserId = owner,
    AssignedToUserId = agent, StatusId = 3, CreatedAt = start, UpdatedAt = start };
await commands.Add(ticket);
await commands.Add(new Ticket { TicketNumber = "T-2", Subject = "Other", CreatedByUserId = agent,
    StatusId = 1, CreatedAt = start.AddDays(1), UpdatedAt = start });
using (var otherScope = provider.CreateScope())
{
    var otherContext = (UserPoolContext)otherScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    Check(await otherContext.Tickets.CountAsync() == 0, "Commands do not commit implicitly");
}
await context.SaveChangesAsync();

var first = new TicketMessage { TicketId = ticket.Id, SenderUserId = agent, Body = "First", CreatedAt = start };
var second = new TicketMessage { TicketId = ticket.Id, SenderUserId = agent, Body = "Second", CreatedAt = start };
await messages.Add(first);
await messages.Add(second);
await context.SaveChangesAsync();
await ratings.Add(new TicketRating { TicketMessageId = first.Id, RatedByUserId = owner, Score = 5, CreatedAt = start });
await ratings.Add(new TicketRating { TicketMessageId = second.Id, RatedByUserId = owner, Score = 3, CreatedAt = start });
var file = new TicketAttachment { TicketMessageId = first.Id, FileGuid = Guid.NewGuid(),
    OriginalFileName = "test.png", ContentType = "image/png", SizeBytes = 3,
    FileContent = new byte[] { 1, 2, 3 }, CreatedAt = start };
await attachments.Add(file);
await context.SaveChangesAsync();
context.ChangeTracker.Clear();

var page = await query.Get(new TicketSearchInput { PageSize = 1 });
Check(page.TotalCount == 2 && page.Items.Single().TicketNumber == "T-2", "Paging uses stable newest-first ordering");
Check(!context.ChangeTracker.Entries().Any(), "Ticket lists are not tracked");
var filtered = await query.Get(new TicketSearchInput { CreatedByUserId = owner, AssignedToUserId = agent,
    StatusId = 3, SearchText = "Meter", FromDate = start, ToDateExclusive = start.AddDays(1) });
Check(filtered.TotalCount == 1 && filtered.Items.Single().Id == ticket.Id, "Combined filters and exclusive date boundary");
var history = await messageQuery.GetByTicket(ticket.Id, pageSize: 1);
Check(history.TotalCount == 2 && history.Items.Single().Id == first.Id, "Message paging is stable for equal timestamps");
var info = await attachmentQuery.GetByMessage(first.Id);
Check(info.Single().Id == file.Id && (await attachmentQuery.GetInfo(file.Id))?.SizeBytes == 3, "Attachment metadata queries");
Check((await attachmentQuery.GetContent(file.Id))!.FileContent.SequenceEqual(new byte[] { 1, 2, 3 }), "Explicit file content retrieval");
Check(await attachmentQuery.GetInfo(long.MaxValue) == null, "Missing attachment returns null");
Check((await ratingQuery.GetByMessage(first.Id))?.Score == 5, "Rating lookup by response");
Check((await statusQuery.Get()).Count == 2 && (await statusQuery.Get(1))?.Code == "New", "Status lookups");

var report = await reportQuery.Get(new TicketSearchInput { CreatedByUserId = owner });
Check(report.TicketCount == 1 && report.RatingCount == 2 && report.AverageScore == 4,
    "Multiple rated replies do not multiply ticket count");
Check(report.StatusCounts.Single().Count == 1, "Report status counts");
var unrated = await reportQuery.Get(new TicketSearchInput { CreatedByUserId = agent });
Check(unrated.TicketCount == 1 && unrated.RatingCount == 0 && unrated.AverageScore == null, "Unrated ticket has no average");
var empty = await reportQuery.Get(new TicketSearchInput { CreatedByUserId = Guid.NewGuid() });
Check(empty.TicketCount == 0 && empty.RatingCount == 0 && empty.AverageScore == null, "Empty report");

var tracked = (await query.Get(ticket.Id))!;
tracked.Subject = "Updated";
commands.Update(tracked);
Check(context.Entry(tracked).State == EntityState.Modified, "Command stages scalar update");
Check(!context.ChangeTracker.Entries<User>().Any(x => x.State == EntityState.Modified), "Update does not modify user graph");
await context.SaveChangesAsync();
context.ChangeTracker.Clear();
Check((await query.Get(ticket.Id))?.Subject == "Updated", "Update persists on explicit commit");

try { await query.Get(new TicketSearchInput { PageSize = 101 }); throw new Exception("Missing page size validation"); }
catch (ArgumentOutOfRangeException) { checks++; }
try { await reportQuery.Get(new TicketSearchInput { FromDate = start, ToDateExclusive = start }); throw new Exception("Missing date validation"); }
catch (ArgumentException) { checks++; }

// Validate the actual SQL Server model without connecting to a database.
using var sqlContext = new UserPoolContext(new DbContextOptionsBuilder<UserPoolContext>()
    .UseSqlServer("Server=localhost;Database=TicketingSmokeTest;Integrated Security=true;TrustServerCertificate=true").Options);
foreach (var type in new[] { typeof(Ticket), typeof(TicketMessage), typeof(TicketAttachment), typeof(TicketStatus), typeof(TicketRating) })
{
    var entity = sqlContext.Model.FindEntityType(type)!;
    Check(entity.GetSchema() == "UserPool", $"Schema for {type.Name}");
    Check(entity.GetForeignKeys().All(fk => fk.DeleteBehavior == DeleteBehavior.NoAction), $"No cascading deletes for {type.Name}");
    Check(!entity.GetProperties().Any(p => p.IsShadowProperty()), $"No accidental shadow columns for {type.Name}");
}
Check(sqlContext.Model.FindEntityType(typeof(TicketRating))!.GetForeignKeys()
    .Single(f => f.PrincipalEntityType.ClrType == typeof(TicketMessage)).IsUnique, "One rating per response mapping");
Check(sqlContext.Model.FindEntityType(typeof(TicketStatus))!.FindProperty("Id")!.ValueGenerated == Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never,
    "Status lookup ID is not an identity");
Console.WriteLine($"PASS: {checks} ticketing persistence checks. No SQL Server connection was opened.");
await ApplicationChecks.Run();
await ApiChecks.Run();

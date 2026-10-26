using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.UserPool.Application.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.Implementation;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

internal static class ApplicationChecks
{
    public static async Task Run()
    {
        var checks = 0;
        void Check(bool condition, string name)
        {
            if (!condition) throw new Exception(name);
            checks++;
        }
        async Task Reject(Func<Task> action, string name)
        {
            try { await action(); }
            catch (BaseException) { checks++; return; }
            throw new Exception(name);
        }

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<UserPoolContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddUserPoolPersistenceInjections();
        services.AddUserPoolApplicationInjections();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var sp = scope.ServiceProvider;
        var context = (UserPoolContext)sp.GetRequiredService<IUnitOfWork>();
        var ct = CancellationToken.None;
        var owner = new TicketTestUser(Guid.NewGuid());
        var agent = new TicketTestUser(Guid.NewGuid());
        var stranger = new TicketTestUser(Guid.NewGuid());
        var admin = new TicketTestUser(Guid.NewGuid(), true);
        context.Users.AddRange(new[] { owner, agent, stranger, admin }.Select(user => new User
        {
            Id = user.UserId, FullName = "Test", DisplayName = "Test", Username = user.Username,
            Password = "unused", Mobile = "000", Hash = "unused", InsertLogInfo = "test"
        }));
        context.TicketStatuses.AddRange(new[] { "New", "InProgress", "Answered", "Closed" }
            .Select((code, index) => new TicketStatus { Id = (short)(index + 1), Code = code, Title = code }));
        await context.SaveChangesAsync();
        sp.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
        Check(true, "All ticketing mapper profiles validate");
        var handlerContracts = typeof(ITicketCreateHandler).Assembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace != null && t.Namespace.Contains("Ticketing.Handlers"));
        foreach (var contract in handlerContracts)
            Check(sp.GetRequiredService(contract) != null, $"Handler resolves: {contract.Name}");

        var create = sp.GetRequiredService<ITicketCreateHandler>();
        var update = sp.GetRequiredService<ITicketUpdateHandler>();
        var addMessage = sp.GetRequiredService<ITicketMessageCreateHandler>();
        var editMessage = sp.GetRequiredService<ITicketMessageUpdateHandler>();
        var addFile = sp.GetRequiredService<ITicketAttachmentCreateHandler>();
        var editFile = sp.GetRequiredService<ITicketAttachmentUpdateHandler>();
        var addRating = sp.GetRequiredService<ITicketRatingCreateHandler>();
        var editRating = sp.GetRequiredService<ITicketRatingUpdateHandler>();
        var getTicket = sp.GetRequiredService<ITicketGetSingleHandler>();
        var getList = sp.GetRequiredService<ITicketGetListHandler>();
        var getMessage = sp.GetRequiredService<ITicketMessageGetSingleHandler>();
        var getMessages = sp.GetRequiredService<ITicketMessageGetListHandler>();
        var getFile = sp.GetRequiredService<ITicketAttachmentGetSingleHandler>();
        var getFiles = sp.GetRequiredService<ITicketAttachmentGetListHandler>();
        var getContent = sp.GetRequiredService<ITicketAttachmentContentGetHandler>();
        var getRating = sp.GetRequiredService<ITicketRatingGetSingleHandler>();
        var getRatingByMessage = sp.GetRequiredService<ITicketRatingGetByMessageHandler>();
        var getReport = sp.GetRequiredService<ITicketReportGetHandler>();
        var png = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0 };

        await Reject(() => create.Handle(new TicketCreateDto { Subject = "Empty" }, owner, ct), "Empty initial message rejected");
        await Reject(() => create.Handle(new TicketCreateDto { Subject = "Text", Body = "Hello" }, new TicketTestUser(Guid.Empty), ct), "Anonymous identity rejected");
        await Reject(() => create.Handle(new TicketCreateDto { Subject = "File", Attachments = null! }, owner, ct), "Null attachments rejected");
        var fileValidator = new TicketFileCreateValidator();
        Check(!fileValidator.Validate(new TicketFileCreateDto { OriginalFileName = "photo.png", ContentType = "image/png", FileContent = "script"u8.ToArray() }).IsValid,
            "MIME spoofing with invalid signature rejected");
        Check(!fileValidator.Validate(new TicketFileCreateDto { OriginalFileName = "../photo.png", ContentType = "image/png", FileContent = png }).IsValid,
            "Path-containing file name rejected");

        var number = await create.Handle(new TicketCreateDto
        {
            Subject = "Water pressure", Body = "Please investigate",
            Attachments = new List<TicketFileCreateDto> { new() { OriginalFileName = "photo.png", ContentType = "image/png", FileContent = png } }
        }, owner, ct);
        Check(!await context.Tickets.AsNoTracking().AnyAsync(), "Create handler does not commit implicitly");
        await context.SaveChangesAsync();
        var ticketId = await context.Tickets.Where(x => x.TicketNumber == number).Select(x => x.Id).SingleAsync();
        var initialId = await context.TicketMessages.Where(x => x.TicketId == ticketId).Select(x => x.Id).SingleAsync();
        var fileId = await context.TicketAttachments.Select(x => x.Id).SingleAsync();
        context.ChangeTracker.Clear();
        var ticketDto = await getTicket.Handle(ticketId, owner, ct);
        Check(ticketDto.CreatedByUserId == owner.UserId && ticketDto.StatusId == 1, "Owner identity and initial status controlled by handler");
        Check(await context.TicketMessages.CountAsync() == 1 && await context.TicketAttachments.CountAsync() == 1, "Ticket, initial message and file saved as a graph");
        Check((await getFile.Handle(fileId, owner, ct)).SizeBytes == png.Length, "File size derived from bytes");
        Check((await getContent.Handle(fileId, owner, ct)).FileContent.SequenceEqual(png), "Authorized file content retrieval");
        await Reject(() => getTicket.Handle(ticketId, stranger, ct), "Unrelated user cannot read ticket");
        await Reject(() => getContent.Handle(fileId, stranger, ct), "Unrelated user cannot read file");
        await Reject(() => getMessages.Handle(new TicketMessageSearchInput { TicketId = ticketId }, stranger, ct), "Unrelated user cannot read history");

        var forgedFilter = new TicketSearchInput { CreatedByUserId = owner.UserId };
        Check((await getList.Handle(forgedFilter, stranger, ct)).TotalCount == 0, "List cannot access another owner's tickets");
        Check(forgedFilter.CreatedByUserId == owner.UserId, "Scope does not mutate caller input");
        Check((await getReport.Handle(new TicketSearchInput(), stranger, ct)).TicketCount == 0, "Report scoped to current user");
        Check((await getList.Handle(new TicketSearchInput(), admin, ct)).TotalCount == 1, "Administrator sees ticket queue");

        await update.Handle(new TicketUpdateDto { Id = ticketId, Subject = ticketDto.Subject, AssignedToUserId = agent.UserId, StatusId = 2 }, admin, ct);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        Check((await getList.Handle(new TicketSearchInput { AssignedToUserId = agent.UserId }, agent, ct)).TotalCount == 1, "Agent sees own assigned tickets");
        await Reject(() => update.Handle(new TicketUpdateDto { Id = ticketId, Subject = "Changed", AssignedToUserId = stranger.UserId, StatusId = 2 }, agent, ct), "Agent cannot reassign ticket");
        await Reject(() => addMessage.Handle(new TicketMessageCreateDto { TicketId = ticketId, Body = "Intrusion" }, stranger, ct), "Unrelated user cannot reply");
        await addMessage.Handle(new TicketMessageCreateDto { TicketId = ticketId, Body = "Fixed" }, agent, ct);
        await context.SaveChangesAsync();
        var responseId = await context.TicketMessages.Where(x => x.TicketId == ticketId && x.SenderUserId == agent.UserId).Select(x => x.Id).SingleAsync();
        context.ChangeTracker.Clear();
        Check((await getTicket.Handle(ticketId, owner, ct)).StatusId == 3, "Staff reply changes status to answered");
        Check((await getMessage.Handle(responseId, owner, ct)).Body == "Fixed", "Message DTO returned");
        Check((await getMessages.Handle(new TicketMessageSearchInput { TicketId = ticketId, PageSize = 1 }, owner, ct)).TotalCount == 2,
            "Message list retains total count");
        Check(await getRatingByMessage.Handle(responseId, owner, ct) == null, "Unrated response returns null");
        await Reject(() => addRating.Handle(new TicketRatingCreateDto { TicketMessageId = initialId, Score = 5 }, owner, ct), "Owner cannot rate own message");
        await Reject(() => addRating.Handle(new TicketRatingCreateDto { TicketMessageId = responseId, Score = 5 }, agent, ct), "Staff cannot rate own reply");
        await Reject(() => addRating.Handle(new TicketRatingCreateDto { TicketMessageId = responseId, Score = 0 }, owner, ct), "Out-of-range score rejected");
        await addRating.Handle(new TicketRatingCreateDto { TicketMessageId = responseId, Score = 5 }, owner, ct);
        await context.SaveChangesAsync();
        var ratingId = await context.TicketRatings.Select(x => x.Id).SingleAsync();
        await Reject(() => addRating.Handle(new TicketRatingCreateDto { TicketMessageId = responseId, Score = 4 }, owner, ct), "Duplicate response rating rejected");
        await editRating.Handle(new TicketRatingUpdateDto { Id = ratingId, Score = 4, Comment = "Thanks" }, owner, ct);
        await context.SaveChangesAsync();
        Check((await getRating.Handle(ratingId, owner, ct)).Score == 4, "Owner can update rating");
        await Reject(() => editRating.Handle(new TicketRatingUpdateDto { Id = ratingId, Score = 1 }, agent, ct), "Staff cannot change user's rating");
        var report = await getReport.Handle(new TicketSearchInput(), owner, ct);
        Check(report.TicketCount == 1 && report.RatingCount == 1 && report.AverageScore == 4, "Application report reflects ratings");

        await Reject(() => editMessage.Handle(new TicketMessageUpdateDto { Id = initialId, Body = "Not yours" }, agent, ct), "Message edits limited to sender");
        await Reject(() => editMessage.Handle(new TicketMessageUpdateDto { Id = responseId, Body = " " }, agent, ct), "Edit cannot remove all message content");
        await editMessage.Handle(new TicketMessageUpdateDto { Id = initialId, Body = null }, owner, ct);
        await context.SaveChangesAsync();
        Check((await getMessage.Handle(initialId, owner, ct)).Body == null, "Message with attachment may have no text");
        await addFile.Handle(new TicketAttachmentCreateDto { TicketMessageId = initialId, OriginalFileName = "second.png", ContentType = "image/png", FileContent = png }, owner, ct);
        await context.SaveChangesAsync();
        Check((await getFiles.Handle(initialId, owner, ct)).Count == 2, "Standalone attachment create handler");
        var fileGuid = (await context.TicketAttachments.SingleAsync(x => x.Id == fileId)).FileGuid;
        await editFile.Handle(new TicketAttachmentUpdateDto { Id = fileId, OriginalFileName = "renamed.png", ContentType = "image/png", FileContent = png }, owner, ct);
        await context.SaveChangesAsync();
        Check((await getFile.Handle(fileId, owner, ct)).OriginalFileName == "renamed.png"
            && (await context.TicketAttachments.SingleAsync(x => x.Id == fileId)).FileGuid == fileGuid, "Attachment update preserves FILESTREAM row GUID");

        for (var i = 0; i < 3; i++)
            await addFile.Handle(new TicketAttachmentCreateDto { TicketMessageId = initialId, OriginalFileName = $"pending{i}.png", ContentType = "image/png", FileContent = png }, owner, ct);
        await Reject(() => addFile.Handle(new TicketAttachmentCreateDto { TicketMessageId = initialId, OriginalFileName = "sixth.png", ContentType = "image/png", FileContent = png }, owner, ct),
            "Attachment limit includes pending inserts before commit");
        await context.SaveChangesAsync();
        Check((await getFiles.Handle(initialId, owner, ct)).Count == 5, "At most five files staged in a unit of work");

        await update.Handle(new TicketUpdateDto { Id = ticketId, Subject = ticketDto.Subject, AssignedToUserId = agent.UserId, StatusId = 4 }, agent, ct);
        await context.SaveChangesAsync();
        Check((await getTicket.Handle(ticketId, owner, ct)).ClosedAt != null, "Closing records timestamp");
        await Reject(() => addMessage.Handle(new TicketMessageCreateDto { TicketId = ticketId, Body = "Again" }, owner, ct), "Closed ticket rejects replies");
        await Reject(() => editFile.Handle(new TicketAttachmentUpdateDto { Id = fileId, OriginalFileName = "x.png", ContentType = "image/png", FileContent = png }, owner, ct), "Closed ticket rejects file edits");
        await update.Handle(new TicketUpdateDto { Id = ticketId, Subject = ticketDto.Subject, AssignedToUserId = agent.UserId, StatusId = 2 }, agent, ct);
        await context.SaveChangesAsync();
        Check((await getTicket.Handle(ticketId, owner, ct)).ClosedAt == null, "Reopening clears closed timestamp");

        using var cancelled = new CancellationTokenSource();
        cancelled.Cancel();
        try { await getList.Handle(new TicketSearchInput(), owner, cancelled.Token); throw new Exception("Cancellation ignored"); }
        catch (OperationCanceledException) { checks++; }
        Console.WriteLine($"PASS: {checks} ticketing application checks. No SQL Server connection was opened.");
    }
}

internal sealed class TicketTestUser(Guid id, bool isAdmin = false) : IAppUser
{
    public Guid UserId => id;
    public string Username => id.ToString();
    public string FullName => "Test";
    public ICollection<string> Roles => new List<string>();
    public bool IsAdmin => isAdmin;
}

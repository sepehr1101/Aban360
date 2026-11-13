using System.Reflection;
using Aban360.Common.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Services;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using Microsoft.Extensions.DependencyInjection;

internal static class BoundaryChecks
{
    public static async Task Run(IServiceProvider provider)
    {
        int checks = 0;
        void Check(bool condition, string message) { if (!condition) throw new Exception(message); checks++; }
        T Build<T>(params (Type Type, object Value)[] overrides)
        {
            var type = typeof(T).Assembly.GetTypes().Single(t => t.IsClass && typeof(T).IsAssignableFrom(t));
            var constructor = type.GetConstructors().Single();
            return (T)constructor.Invoke(constructor.GetParameters().Select(p =>
                overrides.FirstOrDefault(x => x.Type == p.ParameterType).Value ?? provider.GetRequiredService(p.ParameterType)).ToArray());
        }
        var user = new TicketTestUser(Guid.NewGuid());
        using var caller = new CancellationTokenSource();
        using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
        foreach (var mode in new[] { "pending", "failure", "cancellation" })
        {
            var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            Ticket? staged = null;
            var commands = BoundaryProxy.Create<ITicketCommandService>((method, args) =>
            {
                Check(method.Name == "Add" && (CancellationToken)args![1]! == caller.Token, "Create forwards caller cancellation to Add");
                staged = (Ticket)args![0]!;
                return mode == "failure" ? Task.FromException(new InvalidOperationException("command-failed"))
                    : mode == "cancellation" ? Task.FromCanceled(cancelled.Token) : gate.Task;
            });
            var service = Build<ITicketService>((typeof(ITicketCommandService), commands));
            var task = service.CreateAsync(new TicketCreateDto { Subject = "Await", Body = "Initial message" }, user, caller.Token);
            Check(staged?.CreatedByUserId == user.UserId && staged.StatusId == 1
                && Guid.TryParseExact(staged.TicketNumber, "N", out _) && staged.Messages.Single().Body == "Initial message",
                "Create stages owner, status, display number and initial message as one graph");
            if (mode == "pending")
            {
                Check(!task.IsCompleted, "Create must await Add");
                gate.SetResult();
                Check(ReferenceEquals(staged, await task), "Create returns the same staged graph");
            }
            else
            {
                try { await task; throw new Exception("Create swallowed command failure/cancellation"); }
                catch (InvalidOperationException ex) when (mode == "failure" && ex.Message == "command-failed") { checks++; }
                catch (OperationCanceledException) when (mode == "cancellation") { checks++; }
            }
        }

        foreach (var mode in new[] { "pending", "denied", "cancellation" })
        {
            var calls = new List<string>();
            var metadataGate = new TaskCompletionSource<TicketAttachmentInfo?>(TaskCreationOptions.RunContinuationsAsynchronously);
            var accessGate = new TaskCompletionSource<Ticket>(TaskCreationOptions.RunContinuationsAsynchronously);
            var accessReached = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var queries = BoundaryProxy.Create<ITicketAttachmentQueryService>((method, args) =>
            {
                Check((long)args![0]! == 11 && (CancellationToken)args[1]! == caller.Token, "Attachment query receives ID and caller token");
                calls.Add(method.Name);
                return method.Name switch
                {
                    "GetInfo" => mode == "pending" ? metadataGate.Task : Task.FromResult<TicketAttachmentInfo?>(new() { Id = 11, TicketMessageId = 12 }),
                    "GetContent" => Task.FromResult<TicketAttachment?>(new() { Id = 11, FileContent = new byte[] { 1, 2, 3 }, OriginalFileName = "file.png", ContentType = "image/png" }),
                    _ => throw new Exception("Unexpected attachment query: " + method.Name)
                };
            });
            var messages = BoundaryProxy.Create<ITicketMessageQueryService>((method, args) =>
            {
                Check(method.Name == "Get" && (long)args![0]! == 12 && (CancellationToken)args[1]! == caller.Token, "Authorize using attachment's message");
                calls.Add("Message");
                return Task.FromResult<TicketMessage?>(new() { Id = 12, TicketId = 13 });
            });
            var access = BoundaryProxy.Create<ITicketAccessService>((method, args) =>
            {
                Check(method.Name == "Get" && (long)args![0]! == 13 && ReferenceEquals(args[1], user)
                    && (CancellationToken)args[2]! == caller.Token, "Authorize ticket using the actual caller");
                calls.Add("Access"); accessReached.SetResult();
                return mode == "denied" ? Task.FromException<Ticket>(new CustomValidationException("access-denied"))
                    : mode == "cancellation" ? Task.FromCanceled<Ticket>(cancelled.Token) : accessGate.Task;
            });
            var service = Build<ITicketAttachmentService>((typeof(ITicketAttachmentQueryService), queries),
                (typeof(ITicketMessageQueryService), messages), (typeof(ITicketAccessService), access));
            var task = service.GetContentAsync(11, user, caller.Token);
            if (mode == "pending")
            {
                Check(!task.IsCompleted && calls.SequenceEqual(new[] { "GetInfo" }), "File read waits for metadata");
                metadataGate.SetResult(new() { Id = 11, TicketMessageId = 12 });
                await accessReached.Task.WaitAsync(TimeSpan.FromSeconds(5));
                Check(!task.IsCompleted && !calls.Contains("GetContent"), "File bytes are not loaded before authorization completes");
                accessGate.SetResult(new() { Id = 13 });
                Check((await task).FileContent.SequenceEqual(new byte[] { 1, 2, 3 })
                    && calls.SequenceEqual(new[] { "GetInfo", "Message", "Access", "GetContent" }), "Authorized file bytes preserve query ordering");
            }
            else
            {
                try { await task; throw new Exception("File read swallowed access rejection/cancellation"); }
                catch (CustomValidationException) when (mode == "denied") { checks++; }
                catch (OperationCanceledException) when (mode == "cancellation") { checks++; }
                Check(!calls.Contains("GetContent"), "Rejected/cancelled authorization must never load file bytes");
            }
        }
        Console.WriteLine($"PASS: {checks} consolidated ticketing boundary checks (pending tasks, failures, cancellation and metadata authorization before file content).");
    }
}

public class BoundaryProxy : DispatchProxy
{
    public Func<MethodInfo, object?[]?, object?> Callback { get; set; } = null!;
    public static T Create<T>(Func<MethodInfo, object?[]?, object?> callback)
    {
        var proxy = DispatchProxy.Create(typeof(T), typeof(BoundaryProxy));
        ((BoundaryProxy)proxy).Callback = callback;
        return (T)proxy;
    }
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Callback(method!, args);
}

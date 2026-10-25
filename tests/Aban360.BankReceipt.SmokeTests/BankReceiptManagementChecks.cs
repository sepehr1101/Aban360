using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Implementations;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;

internal static class BankReceiptManagementChecks
{
    public static async Task<int> Run()
    {
        int checks = 0;
        void Check(bool value, string message) { if (!value) throw new Exception(message); checks++; }
        async Task Reject(Func<Task> action, string message)
        {
            try { await action(); } catch (BaseException) { checks++; return; }
            throw new Exception(message);
        }
        var banks = new BankSource();
        var bankHandler = new BankReceiptBankGetAllHandler(banks);
        var list = (await bankHandler.Handle(default)).ToArray();
        Check(list.Length == 2 && list[0].Id == 1 && list[1].Id == 2, "bank selection sorted");
        Check(list[0].CentralBankCode == "01" && list[0].Identifier == "AAA", "bank codes kept as strings");
        Check(list[0].Id != short.Parse(list[1].CentralBankCode), "internal bank identifier is separate from central code");
        var owner = Guid.NewGuid(); var stranger = Guid.NewGuid();
        var start = new DateTime(2025, 3, 20, 20, 0, 0, DateTimeKind.Utc);
        var store = new Deletions();
        var pending = new FileRecord(1, owner, start, false);
        var committed = new FileRecord(1, owner, start.AddHours(10), true) { Description = "شرح ثابت" };
        var nextDay = new FileRecord(1, owner, start.AddDays(1), false);
        var previousDay = new FileRecord(1, owner, start.AddTicks(-1), false);
        var otherBank = new FileRecord(2, owner, start.AddHours(1), false);
        var otherOwner = new FileRecord(1, stranger, start.AddHours(1), false);
        store.Files.AddRange(new[] { pending, committed, nextDay, previousDay, otherBank, otherOwner });
        var handler = new BankReceiptDeleteHandler(store, banks);
        await Reject(() => handler.Handle(new(0, "14040101"), owner, default), "invalid bank ID");
        await Reject(() => handler.Handle(new(3, "14040101"), owner, default), "unknown bank");
        await Reject(() => handler.Handle(new(1, "14041301"), owner, default), "invalid day");
        await Reject(() => handler.Handle(new(1, "140401"), owner, default), "old date format rejected");
        Check(store.Calls == 0, "invalid input cannot delete");
        var result = await handler.Handle(new(1, "1404/01/01"), owner, default);
        Check(result.DeletedFileCount == 2 && result.CommittedFileCount == 1, "delete only matching bank and upload day");
        Check(pending.Deleted && committed.Deleted && !nextDay.Deleted && !previousDay.Deleted, "day boundaries inclusive and exclusive");
        Check(!otherBank.Deleted && !otherOwner.Deleted, "bank and owner isolation");
        Check(committed.PaymentStillExists && committed.Description == "شرح ثابت" && !committed.DiscrepancyActive,
            "financial history and immutable notes retained");
        var retry = await handler.Handle(new(1, "14040101"), owner, default);
        Check(retry.DeletedFileCount == 0 && retry.CommittedFileCount == 0, "repeated delete is harmless");
        var empty = await handler.Handle(new(1, "14040103"), owner, default);
        Check(empty.DeletedFileCount == 0, "empty day");
        using var cancellation = new CancellationTokenSource(); cancellation.Cancel();
        int calls = store.Calls;
        try { await handler.Handle(new(1, "14040101"), owner, cancellation.Token); throw new Exception("cancellation"); }
        catch (OperationCanceledException) { checks++; }
        Check(store.Calls == calls, "cancelled deletion does not reach persistence");
        return checks;
    }
    private sealed class BankSource : IBankQueryService
    {
        private readonly Bank[] _banks = {
            new() { Id = 2, BankName = "B", CentralBankCode = "18", _3Char = "BBB" },
            new() { Id = 1, BankName = "A", CentralBankCode = "01", _3Char = "AAA" }
        };
        public Task<Bank> Get(short id) => Task.FromResult(_banks.FirstOrDefault(b => b.Id == id) ?? throw new BaseException("bank not found"));
        public Task<ICollection<Bank>> Get() => Task.FromResult<ICollection<Bank>>(_banks);
    }
    private sealed class FileRecord(short bankId, Guid owner, DateTime uploadedAt, bool committed)
    {
        public short BankId { get; } = bankId;
        public Guid Owner { get; } = owner;
        public DateTime UploadedAt { get; } = uploadedAt;
        public bool Committed { get; } = committed;
        public bool Deleted { get; set; }
        public bool PaymentStillExists { get; } = committed;
        public string? Description { get; set; }
        public bool DiscrepancyActive { get; set; } = true;
    }
    private sealed class Deletions : IBankReceiptDeleteRepository
    {
        public List<FileRecord> Files { get; } = new();
        public int Calls { get; private set; }
        public Task<BankReceiptDeleteResultDto> Delete(short bankId, DateTime fromUtc, DateTime toUtcExclusive,
            Guid userId, CancellationToken cancellationToken)
        {
            Calls++;
            var selected = Files.Where(f => f.BankId == bankId && f.Owner == userId && !f.Deleted
                && f.UploadedAt >= fromUtc && f.UploadedAt < toUtcExclusive).ToArray();
            foreach (var file in selected) { file.Deleted = true; file.DiscrepancyActive = false; }
            return Task.FromResult(new BankReceiptDeleteResultDto(selected.Length, selected.Count(f => f.Committed)));
        }
    }
}

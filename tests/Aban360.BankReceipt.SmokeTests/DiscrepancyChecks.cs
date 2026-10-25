using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Implementations;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;

internal static class DiscrepancyChecks
{
    public static async Task<int> Run(Guid owner, Guid importId)
    {
        int checks = 0;
        void Check(bool value, string message) { if (!value) throw new Exception(message); checks++; }
        async Task Reject(Func<Task> action, string message)
        {
            try { await action(); } catch (BaseException) { checks++; return; }
            throw new Exception(message);
        }
        var repository = new Discrepancies(owner, importId);
        var handler = new BankReceiptDiscrepancyHandler(repository);
        BankReceiptDiscrepancyReportInputDto Range() => new() { FromDate = "1404/01/01", ToDate = "14040101" };
        var report = await handler.Report(Range(), owner, default);
        Check(report.TotalCount == 1 && report.Items.Single().CanDescribe, "pending discrepancy report");
        Check(repository.FromUtc == new DateTime(2025, 3, 20, 20, 0, 0, DateTimeKind.Utc)
            && repository.ToUtc == new DateTime(2025, 3, 21, 20, 0, 0, DateTimeKind.Utc), "inclusive local date boundaries converted to UTC");
        Check(report.Items.Single().Reasons[0].Message == "کد شهر شناسه قبض شناسایی نشد.", "Persian city reason");
        await Reject(() => handler.Report(new() { FromDate = "14040102", ToDate = "14040101" }, owner, default), "reversed dates");
        await Reject(() => handler.Report(new() { FromDate = "14041301", ToDate = "14041302" }, owner, default), "invalid Persian dates");
        await Reject(() => handler.Report(new() { FromDate = "1/4/0/4/0/1/0/1", ToDate = "14040101" }, owner, default), "invalid date separators");
        var page = Range(); page.PageSize = 201;
        await Reject(() => handler.Report(page, owner, default), "oversized page");
        page.PageSize = 200; page.PageNumber = int.MaxValue;
        await Reject(() => handler.Report(page, owner, default), "page overflow");
        await Reject(() => handler.Describe(importId, 2, new("  "), owner, default), "empty description");
        await Reject(() => handler.Describe(importId, 2, new(new string('x', 2001)), owner, default), "long description");
        await Reject(() => handler.Describe(importId, 1, new("شرح"), owner, default), "header not a receipt");
        await Reject(() => handler.Describe(importId, 2, new("شرح"), Guid.NewGuid(), default), "description owner isolation");
        Check((await handler.Report(Range(), Guid.NewGuid(), default)).TotalCount == 0, "report owner isolation");
        await handler.Describe(importId, 2, new("  کد شهر نیازمند بررسی است  "), owner, default);
        Check(repository.Item.Description == "کد شهر نیازمند بررسی است" && repository.Item.DescribedBy == owner
            && repository.Item.DescribedAt != null && !repository.Item.CanDescribe, "description recorded once with audit");
        await Reject(() => handler.Describe(importId, 2, new("شرح جایگزین"), owner, default), "description immutable");
        repository.Item.IsActive = false;
        var described = Range(); described.HasDescription = true;
        var history = await handler.Report(described, owner, default);
        Check(history.TotalCount == 1 && history.Items.Single().Description == "کد شهر نیازمند بررسی است", "archived descriptions retained");
        var pending = Range(); pending.HasDescription = false;
        Check((await handler.Report(pending, owner, default)).TotalCount == 0, "description filter");
        var active = Range(); active.IsActive = true;
        Check((await handler.Report(active, owner, default)).TotalCount == 0, "active discrepancy filter");
        // The store's conditional write allows exactly one of two simultaneous submissions.
        var competingStore = new Discrepancies(owner, importId);
        var competingHandler = new BankReceiptDiscrepancyHandler(competingStore);
        async Task<bool> TryWrite(string text)
        {
            try { await competingHandler.Describe(importId, 2, new(text), owner, default); return true; }
            catch (BaseException) { return false; }
        }
        var outcomes = await Task.WhenAll(Task.Run(() => TryWrite("شرح اول")), Task.Run(() => TryWrite("شرح دوم")));
        Check(outcomes.Count(x => x) == 1, "concurrent descriptions have one winner");
        return checks;
    }
    private sealed class Discrepancies(Guid owner, Guid importId) : IBankReceiptDiscrepancyRepository
    {
        public BankReceiptDiscrepancy Item { get; } = new()
        {
            ImportId = importId, LineNumber = 2, UploadedAt = new DateTime(2025, 3, 21, 10, 0, 0, DateTimeKind.Utc), IsActive = true,
            Reasons = new() { new("UnknownCityCode", "Unknown city", 2) }
        };
        public DateTime FromUtc { get; private set; }
        public DateTime ToUtc { get; private set; }
        public Task<BankReceiptDiscrepancyReportDto> Report(DateTime fromUtc, DateTime toUtcExclusive, Guid userId,
            int pageNumber, int pageSize, bool? hasDescription, bool? isActive, CancellationToken cancellationToken)
        {
            FromUtc = fromUtc; ToUtc = toUtcExclusive;
            var found = new[] { Item }.Where(i => userId == owner && i.UploadedAt >= fromUtc && i.UploadedAt < toUtcExclusive
                && (hasDescription == null || (i.Description != null) == hasDescription)
                && (isActive == null || i.IsActive == isActive)).ToArray();
            return Task.FromResult(new BankReceiptDiscrepancyReportDto(found.Length, pageNumber, pageSize,
                found.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray()));
        }
        public Task Describe(Guid id, int lineNumber, string description, Guid userId, CancellationToken cancellationToken)
        {
            lock (Item)
            {
                if (id != Item.ImportId || lineNumber != Item.LineNumber || userId != owner || !Item.CanDescribe)
                    throw new BaseException("immutable or inaccessible");
                Item.Description = description; Item.DescribedBy = userId; Item.DescribedAt = DateTime.UtcNow;
            }
            return Task.CompletedTask;
        }
    }
}

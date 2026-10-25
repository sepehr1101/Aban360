using System.Text;
using System.Text.Json;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Application.Features.BankReceipt.Implementations;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;

int checks = 0;
void Check(bool value, string name) { if (!value) throw new Exception(name); checks++; }
async Task Reject(Func<Task> action, string name)
{
    try { await action(); } catch (BaseException) { checks++; return; }
    throw new Exception(name);
}
string bill = TransactionIdGenerator.GenerateBillId("123456", "091");
string pay = TransactionIdGenerator.GeneratePaymentId(123000, bill);
string row = "0000015914040101" + bill.PadLeft(13, '0') + pay.PadLeft(13, '0') + "123456";
string header = "10911814040101" + "0000000123" + "00000001";
string text = header + "\r\n" + row + "\r\n";
BankReceiptImport Import(string data) => new() { FileName = "TEJ14040101WA001.091", Content = Encoding.ASCII.GetBytes(data) };
var parser = new BankReceiptParser();
var good = Import(text);
parser.Parse(good, "18", "TEJ");
Check(good.Issues.Count == 0 && good.Rows.Single().Issues.Count == 0, "valid 8-digit layout");
Check(good.Header!.Amount == 123000 && good.Rows[0].Amount == 123000, "thousand-rial conversion");
Check(good.Rows[0].BillId == bill && good.Rows[0].PayDate == "14040101", "field offsets and padding");
var badDate = Import(text.Replace("14040101", "14041301")); parser.Parse(badDate, "18", "TEJ");
Check(badDate.Issues.Any(i => i.Code == "SendDate") && badDate.Rows[0].Issues.Any(i => i.Code == "PayDate"), "invalid dates");
var badCount = Import(text.Replace("00000001\r", "00000002\r")); parser.Parse(badCount, "18", "TEJ");
Check(badCount.Issues.Any(i => i.Code == "Count"), "header count mismatch");
var badAmount = Import(text.Replace("0000000123", "0000000124")); parser.Parse(badAmount, "18", "TEJ");
Check(badAmount.Issues.Any(i => i.Code == "Total"), "header sum mismatch");
var duplicate = Import(text + row); parser.Parse(duplicate, "18", "TEJ");
Check(duplicate.Rows[1].Issues.Any(i => i.Code == "DuplicateRow"), "duplicate rows");
var malformed = Import(header + "\n123"); parser.Parse(malformed, "18", "TEJ");
Check(malformed.Issues.Any(i => i.Code == "RowStructure"), "short row");
var old = Import(text.Replace("14040101", "040101")); parser.Parse(old, "18", "TEJ");
Check(old.Issues.Any(i => i.Code == "Header"), "old date format rejected");
var corrupt = Import(text); corrupt.Content[0] = 255; parser.Parse(corrupt, "18", "TEJ");
Check(corrupt.Issues.Any(i => i.Code == "Encoding"), "invalid encoding");
var tampered = Import(text[..^4] + "00\r\n"); parser.Parse(tampered, "19", "MLI");
Check(tampered.Issues.Any(i => i.Code == "Bank"), "bank mismatch");
var wrongChecksum = Import(text.Replace(pay.PadLeft(13, '0'), (pay[..^1] + (pay[^1] == '9' ? '0' : '9')).PadLeft(13, '0')));
parser.Parse(wrongChecksum, "18", "TEJ");
Check(wrongChecksum.Rows[0].Issues.Any(i => i.Code == "PaymentId"), "payment checksum");
var nonWater = Import(text.Replace(bill.PadLeft(13, '0'), (bill[..^2] + "2" + bill[^1]).PadLeft(13, '0')));
parser.Parse(nonWater, "18", "TEJ");
Check(nonWater.Rows[0].Issues.Any(i => i.Code == "NotWater"), "non-water payment flagged");
var repo = new MemoryRepository();
var user = Guid.NewGuid();
var handler = new BankReceiptFlowHandler(repo, new Banks(), parser, Array.Empty<IBankReceiptReconciliationService>());
var id = await handler.Upload(new MemoryStream(Encoding.ASCII.GetBytes(text)), good.FileName, 1, user, "tester", default);
await Reject(() => handler.Confirm(id, user, new(0, true), default), "confirm before validation");
await Reject(() => handler.Reconcile(id, user, default), "reconcile before validation");
await Reject(() => handler.Display(id, Guid.NewGuid(), default), "owner isolation");
var valid = await handler.Validate(id, user, default);
Check(valid.SampleRows.Count == 1 && !valid.CanConfirm, "preview before reconciliation");
var pending = await handler.Reconcile(id, user, default);
Check(pending.InvalidCount == 1 && pending.Rows[0].Issues.Any(i => i.Code == "ReconciliationPending"), "missing adapter blocks");
await Reject(() => handler.Confirm(id, user, new(pending.Version, true), default), "unresolved cannot commit");
handler = new(repo, new Banks(), parser, new[] { new Matches() });
var reconciled = await handler.Reconcile(id, user, default);
await Reject(() => handler.Confirm(id, user, new(reconciled.Version, false), default), "explicit confirmation");
await Reject(() => handler.Confirm(id, user, new(reconciled.Version - 1, true), default), "stale version");
int uploader = await handler.Confirm(id, user, new(reconciled.Version, true), default);
Check(uploader == 42 && repo.Commits == 1, "commit matched payments");
Check(await handler.Confirm(id, user, new(reconciled.Version, true), default) == 42 && repo.Commits == 1, "idempotent confirmation");
await Reject(() => handler.Validate(id, user, default), "committed file immutable");
// Preview returns at most ten distinct rows, even for a larger valid file.
var manyRows = Enumerable.Range(1, 15).Select(n =>
{
    var b = TransactionIdGenerator.GenerateBillId((123456 + n).ToString(), "091");
    var p = TransactionIdGenerator.GeneratePaymentId(123000, b);
    return "0000015914040101" + b.PadLeft(13, '0') + p.PadLeft(13, '0') + "123456";
});
var manyText = "10911814040101" + "0000001845" + "00000015\n" + string.Join("\n", manyRows);
var manyId = await handler.Upload(new MemoryStream(Encoding.ASCII.GetBytes(manyText)), good.FileName, 1, user, "tester", default);
var preview = await handler.Validate(manyId, user, default);
Check(preview.ActualRecordCount == 15 && preview.SampleRows.Count == 10 && preview.SampleRows.Select(r => r.LineNumber).Distinct().Count() == 10, "random preview size");
Check(preview.Issues.Count == 0 && preview.InvalidRecordCount == 0, "multi-row totals");
repo.Duplicates.Add(preview.SampleRows[0].Hash);
var repeats = await handler.Reconcile(manyId, user, default);
Check(repeats.InvalidCount == 1 && repeats.Rows.Any(r => r.Issues.Any(i => i.Code == "AlreadyPaid")), "database duplicate");
await Reject(() => handler.Confirm(manyId, user, new(repeats.Version, true), default), "database duplicate blocks commit");
repo.Duplicates.Clear();
handler = new(repo, new Banks(), parser, new[] { new Matches("OutsideIsfahan") });
var outside = await handler.Reconcile(manyId, user, default);
Check(outside.InvalidCount == 15 && outside.Rows.All(r => r.Issues.Any(i => i.Code == "OutsideIsfahan")), "city mismatches exposed");
await Reject(() => handler.Confirm(manyId, user, new(outside.Version, true), default), "city mismatch blocks commit");
handler = new(repo, new Banks(), parser, new[] { new Matches("UnknownBillId") });
var unknown = await handler.Reconcile(manyId, user, default);
Check(unknown.InvalidCount == 15 && unknown.Rows.All(r => r.Issues.Any(i => i.Code == "UnknownBillId")), "unknown bills exposed");
await Reject(() => handler.Upload(new MemoryStream(), good.FileName, 1, user, "tester", default), "empty file");
Check(unknown.Rows.All(r => r.IsExcluded && r.ExclusionReasons.All(i => i.Message.Any(c => c >= '\u0600' && c <= '\u06ff'))), "excluded rows have Persian reasons");
var malformedId = await handler.Upload(new MemoryStream(Encoding.ASCII.GetBytes(header + "\n123")), good.FileName, 1, user, "tester", default);
await handler.Validate(malformedId, user, default);
var malformedResult = await handler.Reconcile(malformedId, user, default);
Check(malformedResult.Rows.Single().RawData == "123" && malformedResult.Rows[0].IsExcluded, "malformed rows retained with reason");
var subsidiaryId = await handler.Upload(new MemoryStream(Encoding.ASCII.GetBytes(text)), "TEJ14040101WA001.092", 1, user, "tester", default);
var subsidiary = await handler.Validate(subsidiaryId, user, default);
Check(subsidiary.ExcludedRecordCount == 1 && subsidiary.SampleRows[0].ExclusionReasons.Any(i => i.Code == "SubUtilityCode"), "file reasons propagated to receipt");
checks += await DiscrepancyChecks.Run(user, manyId);
checks += await BankReceiptManagementChecks.Run();
Console.WriteLine($"Passed {checks} bank receipt checks.");

sealed class Banks : IBankQueryService
{
    public Task<Bank> Get(short id) => Task.FromResult(new Bank { Id = id, BankName = "Tejarat", CentralBankCode = "18", _3Char = "TEJ" });
    public Task<ICollection<Bank>> Get() => throw new NotImplementedException();
}
sealed class Matches(string? issue = null) : IBankReceiptReconciliationService
{
    public Task<IReadOnlyDictionary<int, BankReceiptMatch>> Reconcile(IReadOnlyCollection<BankReceiptRow> rows, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyDictionary<int, BankReceiptMatch>>(rows.ToDictionary(r => r.LineNumber, r => new BankReceiptMatch(1, 2, issue == null ? Array.Empty<BankReceiptIssue>() : new[] { new BankReceiptIssue(issue, "Mismatch") })));
}
sealed class MemoryRepository : IBankReceiptImportRepository
{
    readonly Dictionary<Guid, string> _items = new();
    public HashSet<string> Duplicates { get; } = new();
    public int Commits { get; private set; }
    public Task Create(BankReceiptImport import, CancellationToken cancellationToken) { _items.Add(import.Id, JsonSerializer.Serialize(import)); return Task.CompletedTask; }
    public Task<BankReceiptImport> Get(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var item = JsonSerializer.Deserialize<BankReceiptImport>(_items[id])!;
        if (item.UserId != userId) throw new BaseException("owner");
        return Task.FromResult(item);
    }
    public async Task Save(BankReceiptImport import, int expectedVersion, CancellationToken cancellationToken)
    {
        var old = await Get(import.Id, import.UserId, cancellationToken);
        if (old.Version != expectedVersion) throw new BaseException("version");
        import.Version = expectedVersion + 1; _items[import.Id] = JsonSerializer.Serialize(import);
    }
    public Task<IReadOnlySet<string>> FindCommittedPayments(IEnumerable<BankReceiptRow> rows, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlySet<string>>(Duplicates);
    public async Task<int> Commit(BankReceiptImport import, int expectedVersion, CancellationToken cancellationToken)
    {
        import.Status = BankReceiptImportStatus.Committed; import.UploaderId = 42;
        await Save(import, expectedVersion, cancellationToken); Commits++; return 42;
    }
}

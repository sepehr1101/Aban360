using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Aban360.Api.ExceptionHandlers;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Implementations;
using Aban360.SystemPool.Domain.Contants;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var checks = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception(name);
    checks++;
}
// Verify the public JSON contract, calendar boundaries, 24-hour time and null handling.
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
foreach (var sample in new[]
{
    (new DateTime(2026, 3, 20, 23, 59, 59), "1404/12/29 23:59:59"),
    (new DateTime(2026, 3, 21, 0, 0, 0), "1405/01/01 00:00:00"),
    (new DateTime(2026, 11, 8, 17, 45, 30, DateTimeKind.Utc), "1405/08/17 17:45:30")
})
{
    var report = new HttpRequestReportOutput { TimeStamp = sample.Item1 };
    var detail = new HttpErrorLogDetailOutput { TimeStamp = sample.Item1 };
    var operation = new OpLogDataDto { RegisterDateTime = sample.Item1 };
    Check(report.TimeStampJalali == sample.Item2 && detail.TimeStampJalali == sample.Item2
        && operation.RegisterDateTimeJalali == sample.Item2,
        $"Expected {sample.Item2}; report={report.TimeStampJalali}, detail={detail.TimeStampJalali}, operation={operation.RegisterDateTimeJalali}");
    using var reportJson = JsonDocument.Parse(JsonSerializer.Serialize(report, jsonOptions));
    using var detailJson = JsonDocument.Parse(JsonSerializer.Serialize(detail, jsonOptions));
    using var operationJson = JsonDocument.Parse(JsonSerializer.Serialize(operation, jsonOptions));
    Check(reportJson.RootElement.GetProperty("timeStampJalali").GetString() == sample.Item2
        && detailJson.RootElement.GetProperty("timeStampJalali").GetString() == sample.Item2
        && operationJson.RootElement.GetProperty("registerDateTimeJalali").GetString() == sample.Item2
        && reportJson.RootElement.GetProperty("timeStamp").GetDateTime() == sample.Item1
        && detailJson.RootElement.GetProperty("timeStamp").GetDateTime() == sample.Item1
        && operationJson.RootElement.GetProperty("registerDateTime").GetDateTime() == sample.Item1,
        "JSON includes correctly suffixed Jalali fields and preserves Gregorian timestamps");
}
Check(new HttpRequestReportOutput().TimeStampJalali is null
    && new HttpErrorLogDetailOutput().TimeStampJalali is null
    && new OpLogDataDto().RegisterDateTimeJalali is null,
    "Missing or unsupported timestamps do not throw during serialization");
foreach (var type in typeof(HttpRequestReportOutput).Assembly.GetTypes()
    .Where(type => type.Namespace == typeof(HttpRequestReportOutput).Namespace)
    .Append(typeof(OpLogDataDto)))
foreach (var property in type.GetProperties().Where(property =>
    property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)))
    Check(type.GetProperty(property.Name + "Jalali")?.PropertyType == typeof(string),
        $"{type.Name}.{property.Name} has a matching Jalali output field");

foreach (var kind in Enum.GetValues<HttpRequestReportKind>())
foreach (var userFilter in new[] { false, true })
foreach (var routeFilter in new[] { false, true })
{
    var sql = HttpRequestReportQueryService.BuildSql(kind, userFilter, routeFilter);
    Check(sql.Contains("AND UserId = @UserId") == userFilter
        && sql.Contains("AND RouteTemplate = @RouteTemplate") == routeFilter,
        "Optional report filters remain parameterized and apply only when supplied");
    var needsUser = kind != HttpRequestReportKind.Summary || userFilter;
    var needsRoute = kind is HttpRequestReportKind.Routes or HttpRequestReportKind.Requests || routeFilter;
    Check(sql.Contains("@key=\"UserId\"") == needsUser
        && sql.Contains("@key=\"RouteTemplate\"") == needsRoute,
        "Report extracts all grouping/filter fields without unrelated XML fields");
    Check(sql.Contains("@key=\"TraceId\"") == (kind == HttpRequestReportKind.Requests),
        "Trace IDs are extracted only for individual requests");
    Check(sql.Contains("TimeStamp >= @from AND TimeStamp < @to")
        && sql.Contains("COUNT_BIG(*) OVER() TotalGroups")
        && sql.Contains("OFFSET @offset ROWS FETCH NEXT @PageSize ROWS ONLY"),
        "Date bounds, full result count and pagination are preserved");
    Check(sql.Contains("u.DisplayName") == (kind != HttpRequestReportKind.Summary)
        && sql.Contains("LEFT JOIN Filtered error ON error.Id = r.ErrorId") == (kind != HttpRequestReportKind.Summary),
        "Non-summary reports enrich existing rows with their error and display name without dropping anonymous requests");
    if (kind != HttpRequestReportKind.Summary)
    {
        Check(sql.Contains("CASE WHEN StatusCode BETWEEN 400 AND 599 THEN Id END")
            && sql.Contains("error.UserId ErrorUserId"),
            "Only IDs accepted by the error-detail API are exposed with their matching error user");
        var displayUser = kind == HttpRequestReportKind.Routes ? "error.UserId" : "r.UserId";
        Check(sql.Contains($"u.Id = TRY_CONVERT(uniqueidentifier, {displayUser})"),
            "Route display name belongs to the representative error; user and request names belong to the row's user");
    }
}
var query = new CaptureQuery();
var handler = new LoggingGetyDateTimeHandler(query);
LoggingInputByStringDto Valid() => new()
{
    FromDate = "1405/01/01", ToDate = "1405/01/02", FromTime = "00:00", ToTime = "23:59"
};
await handler.Handle(Valid(), default);
Check(query.Input!.FromDateTime == new DateTime(2026, 3, 21), "Jalali conversion");
Check(query.Input.LogLevel == LogLevelEnum.None, "Omitted level means all levels");
var localized = Valid() with { FromDate = "۱۴۰۵/۰۱/۰۱", FromTime = "١٢:٣٤" };
using var cancellation = new CancellationTokenSource();
await handler.Handle(localized, cancellation.Token);
Check(query.Input!.FromDateTime == new DateTime(2026, 3, 21, 12, 34, 0)
    && query.Token == cancellation.Token, "Localized digits and cancellation forwarding");
var sameMinute = Valid() with { ToDate = "1405/01/01", ToTime = "00:00" };
await handler.Handle(sameMinute, default);
Check(query.Input!.FromDateTime == query.Input.ToDateTime, "Inclusive single-minute interval preserved");

var invalid = new[]
{
    (Valid() with { FromDate = null! }, "وارد کردن تاریخ شروع الزامی است"),
    (Valid() with { ToDate = "" }, "وارد کردن تاریخ پایان الزامی است"),
    (Valid() with { FromDate = "1405/12/30" }, "تاریخ شروع در تقویم شمسی معتبر نیست"),
    (Valid() with { FromDate = "1405/13/01" }, "تاریخ شروع در تقویم شمسی معتبر نیست"),
    (Valid() with { FromDate = "1405/+1/01" }, "قالب تاریخ شروع معتبر نیست"),
    (Valid() with { ToDate = "2026-11-02" }, "قالب تاریخ پایان معتبر نیست"),
    (Valid() with { FromTime = "25:00" }, "ساعت شروع معتبر نیست"),
    (Valid() with { FromTime = "12:60" }, "HH:mm"),
    (Valid() with { FromTime = "" }, "وارد کردن ساعت شروع الزامی است"),
    (Valid() with { ToTime = null! }, "وارد کردن ساعت پایان الزامی است"),
    (Valid() with { FromTime = "12:30:00" }, "HH:mm"),
    (Valid() with { FromDate = "1405/01/03" }, "شروع نباید بعد از"),
    (Valid() with { LogLevel = (LogLevelEnum)99 }, "LogLevel باید بین 1 و 7 باشد"),
    (Valid() with { LogLevel = (LogLevelEnum)0 }, "برای همهٔ سطح‌ها مقدار 7"),
    (Valid() with { LogLevel = (LogLevelEnum)(-1) }, "سطح لاگ معتبر نیست")
};
var builder = WebApplication.CreateBuilder();
builder.Logging.ClearProviders();
builder.WebHost.UseTestServer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
await using var app = builder.Build();
app.UseExceptionHandler();
app.MapPost("/logs", async (LoggingInputByStringDto input, CancellationToken ct) =>
    await handler.Handle(input, ct));
await app.StartAsync();
using var client = app.GetTestClient();
foreach (var (input, expectedMessage) in invalid)
{
    var before = query.Calls;
    using var response = await client.PostAsJsonAsync("/logs", input);
    Check(response.StatusCode == HttpStatusCode.BadRequest, "Invalid log input returns 400 instead of 500");
    using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    var error = body.RootElement.GetProperty("errors")[0];
    Check(error.GetProperty("code").GetInt32() == 400
        && error.GetProperty("message").GetString()!.Contains(expectedMessage), "400 response explains the specific invalid field");
    Check(before == query.Calls, "Invalid input never reaches database query");
}
await app.StopAsync();

var opLogs = new OpLogQueryService(new ConfigurationBuilder().Build());
foreach (var input in new[]
{
    new OpLogGetDto { FromDateJalali = "bad", ToDateJalali = "1405/01/02" },
    new OpLogGetDto { FromDateJalali = null!, ToDateJalali = "1405/01/02" },
    new OpLogGetDto { FromDateJalali = "1405/01/03", ToDateJalali = "1405/01/02" }
})
{
    try { await opLogs.Get(input); throw new Exception("Invalid operation log dates accepted"); }
    catch (InvalidDateException ex)
    {
        Check(ex.Message.Contains("تاریخ شروع"), "Operation log error identifies invalid date or range");
    }
}
try { await handler.Handle(null!, default); throw new Exception("Null input accepted"); }
catch (BaseException ex) { Check(ex.Message.Contains("ارسال نشده"), "Null log search uses BaseException"); }
try { await opLogs.Get(null!); throw new Exception("Null operation log input accepted"); }
catch (BaseException ex) { Check(ex.Message.Contains("ارسال نشده"), "Null operation log search uses BaseException"); }

// Use an isolated working directory so file checks cannot touch application logs.
var originalDirectory = Directory.GetCurrentDirectory();
var testDirectory = Path.Combine(Path.GetTempPath(), "Aban360-LogViewing-" + Guid.NewGuid());
Directory.CreateDirectory(testDirectory);
try
{
    Directory.SetCurrentDirectory(testDirectory);
    foreach (var folder in new[] { @"AppData\AssessmentLogs", @"AppData\MeterApkLogs" })
    {
        Directory.CreateDirectory(folder);
        var path = Path.Combine(folder, "test.txt");
        File.WriteAllText(path, "test log");
        if (folder.Contains("Assessment"))
            Check(new AssessmentLogGetByFileNameHandler().Handle("test.txt", default).Content == "test log", "Assessment file read");
        else
            Check(new MeterApkLogGetByFileNameHandler().Handle("test.txt", default).Content == "test log", "Meter file read");
        using (var exclusive = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            Check(exclusive.Length > 0, "File read releases handle immediately");
        File.Delete(path);
        Directory.Delete(folder);
    }
}
finally
{
    Directory.SetCurrentDirectory(originalDirectory);
    var appData = Path.Combine(testDirectory, "AppData");
    if (Directory.Exists(appData)) Directory.Delete(appData);
    Directory.Delete(testDirectory);
}
Console.WriteLine($"PASS: {checks} log viewing checks. No database connection was opened.");

sealed class CaptureQuery : ILoggingGetByDateTimeQueryService
{
    public LoggingInputByDateTimeDto? Input;
    public int Calls;
    public CancellationToken Token;
    public Task<IEnumerable<LoggingOutputDto>> Get(LoggingInputByDateTimeDto input, CancellationToken cancellationToken = default)
    {
        Calls++; Input = input; Token = cancellationToken;
        return Task.FromResult<IEnumerable<LoggingOutputDto>>(Array.Empty<LoggingOutputDto>());
    }
}

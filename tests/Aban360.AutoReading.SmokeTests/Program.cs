using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Reflection;
using Aban360.Common.Categories.ApiResponse;
using Aban360.MeterPool.Application.Features.AutoReading.Exceptions;
using Aban360.Api.Controllers.V1.MeterPool.AutoReading.Queries;
using Aban360.Common.Authentication;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Application.Extensions;
using Aban360.MeterPool.Application.Features.AutoReading.Handlers.Queries.Contracts;
using Aban360.MeterPool.Application.Features.AutoReading.Validations;
using Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

const string fixture = """
{"ReadAllReportModelList":[],"UltraSonicReadAllReportModelList":[
 {"Id":1,"GreDateTimeReadAll":"2026-09-19T00:00:33","InstantFlow":-0.01,"PositiveTotalConsumption":83.225,"NegativeTotalConsumption":16.346,"EnvironmentTemperature":"27","Pressure":null},
 {"Id":2,"GreDateTimeReadAll":"2026-09-21T00:00:30","InstantFlow":0.02,"PositiveTotalConsumption":84.694,"NegativeTotalConsumption":16.549,"EnvironmentTemperature":"27","Pressure":null}
]}
""";
var valid = new FlowMeterReportInputDto { FromDate = "1405/06/01", ToDate = "1405/06/30", BillId = "4441707016311" };
var validator = new FlowMeterReportValidator();
int checks = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception("FAIL: " + name);
    checks++;
    Console.WriteLine("PASS: " + name);
}
Check(validator.Validate(valid).IsValid, "Valid Persian date range");
foreach (string invalid in new[] { "1405/07/31", "1405/12/30", "1405/00/01", "1405/6/01", "1405/06/01\n", "" })
    Check(!validator.Validate(valid with { FromDate = invalid }).IsValid, "Reject invalid date: " + invalid.Trim());
Check(!validator.Validate(valid with { FromDate = "1405/06/30", ToDate = "1405/06/01" }).IsValid, "Reject reversed range");
Check(typeof(FlowMeterReportInputDto).GetProperties().Select(x => x.Name).OrderBy(x => x)
    .SequenceEqual(new[] { "BillId", "FromDate", "ToDate" }), "Request exposes exactly three properties");
var action = typeof(FlowMeterReportGetController).GetMethod("Get")!;
Check(action.GetCustomAttribute<HttpPostAttribute>()?.Template == "flow-meter-report"
    && action.GetCustomAttribute<HttpGetAttribute>() is null
    && action.GetParameters()[0].GetCustomAttribute<FromBodyAttribute>() is not null, "Endpoint is POST with JSON body");
string inputJson = JsonSerializer.Serialize(valid, new JsonSerializerOptions(JsonSerializerDefaults.Web));
Check(JsonDocument.Parse(inputJson).RootElement.GetProperty("billId").ValueKind == JsonValueKind.String, "billId is a JSON string");
try
{
    JsonSerializer.Deserialize<FlowMeterReportInputDto>("{\"billId\":4441707016311}", new JsonSerializerOptions(JsonSerializerDefaults.Web));
    throw new Exception("Numeric billId was accepted");
}
catch (JsonException) { Check(true, "Reject numeric billId"); }

var transport = new FakeTransport { Body = fixture };
var factory = new FakeFactory(transport);
var services = new ServiceCollection();
services.AddMeterPoolApplicationInjections();
services.AddSingleton<IHttpClientFactory>(factory);
services.AddSingleton(Options.Create(new AutoReadingOptions { BaseUrl = "https://example.invalid/", FlowMeterReportEndpoint = "FM-Poyak-FlowMeterReport/1.0" }));
using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var handler = scope.ServiceProvider.GetRequiredService<IFlowMeterReportGetHandler>();
var report = await handler.Handle(valid, CancellationToken.None);
Check(report.UltraSonicReadAllReportModelList.Count == 2, "Resolve handler through existing DI and deserialize report");
Check(report.UltraSonicReadAllReportModelList.First().EnvironmentTemperature == 27m, "Read numeric string temperature");
Check(report.UltraSonicReadAllReportModelList.First().InstantFlow == -0.01m, "Preserve negative flow");
Check(report.UltraSonicReadAllReportModelList.First().Pressure is null, "Preserve missing pressure");
Check(factory.Name == HttpClientNames.AutoReading && transport.Method == HttpMethod.Get, "Use named AutoReading GET client");
Check(transport.Uri!.OriginalString.Contains("FromDate=1405%2F06%2F01"), "Encode date query values");
Check(transport.Uri!.OriginalString.Contains("FlowMeterSerial=PYC-929801010342&FlowMeterType=3")
    && !transport.Uri.OriginalString.Contains("billId", StringComparison.OrdinalIgnoreCase), "Handler passes fixed serial and type only to upstream");
int calls = transport.Calls;
foreach (string? billId in new[] { "4441707016312", "", " ", null, "4441707016311 ", "۴۴۴۱۷۰۷۰۱۶۳۱۱", "4441707016311&FlowMeterType=1" })
{
    try { await handler.Handle(valid with { BillId = billId! }, CancellationToken.None); throw new Exception("Unsupported bill was accepted"); }
    catch (NonUltrasonicMeterException exception)
    {
        Check(exception.Message == "نوع کنتور اولتراسونیک نیست" && transport.Calls == calls, "Unsupported bill rejected before upstream: " + (billId ?? "null"));
    }
}
try { await handler.Handle(valid with { ToDate = "bad" }, CancellationToken.None); throw new Exception("Validation was skipped"); }
catch (CustomValidationException) { Check(transport.Calls == calls, "Invalid input never calls upstream"); }

transport.Body = "{\"ReadAllReportModelList\":[{\"Unknown\":42}],\"UltraSonicReadAllReportModelList\":null}";
report = await handler.Handle(valid, CancellationToken.None);
Check(report.ReadAllReportModelList.First().GetProperty("Unknown").GetInt32() == 42 && report.UltraSonicReadAllReportModelList.Count == 0, "Preserve unknown report and normalize null list");
var controller = new FlowMeterReportGetController(handler);
var unsupported = (ObjectResult)await controller.Get(valid with { BillId = "other" }, CancellationToken.None);
Check(unsupported.StatusCode == 400
    && ((ApiResponseEnvelope<object>)unsupported.Value!).Errors.Single().Message == "نوع کنتور اولتراسونیک نیست",
    "Unsupported bill returns HTTP 400 with exact requested message");
Check((await controller.Get(valid with { ToDate = "bad" }, CancellationToken.None) as ObjectResult)?.StatusCode == 400, "Invalid date returns HTTP 400");
transport.Body = "{}";
Check((await controller.Get(valid, CancellationToken.None) as ObjectResult)?.StatusCode == 502, "Missing report fields produce 502");
transport.Body = "not-json";
Check((await controller.Get(valid, CancellationToken.None) as ObjectResult)?.StatusCode == 502, "Malformed JSON produces 502");
transport.Body = "{\"ReadAllReportModelList\":[],\"UltraSonicReadAllReportModelList\":[]}";
Check((await controller.Get(valid, CancellationToken.None) as ObjectResult)?.StatusCode == 200, "Empty report is successful");
transport.Status = HttpStatusCode.Unauthorized;
Check((await controller.Get(valid, CancellationToken.None) as ObjectResult)?.StatusCode == 502, "Upstream error is not an empty report");
transport.Status = HttpStatusCode.OK;
transport.Timeout = true;
Check((await controller.Get(valid, CancellationToken.None) as ObjectResult)?.StatusCode == 504, "Timeout produces 504");
transport.Timeout = false;
using var cts = new CancellationTokenSource();
cts.Cancel();
try { await controller.Get(valid, cts.Token); throw new Exception("Cancellation was swallowed"); }
catch (OperationCanceledException) { Check(true, "Caller cancellation propagates"); }

var tokens = new FakeTokens();
var authTransport = new FakeTransport { Body = fixture, RejectFirst = true };
using var auth = new EsbAuthenticationHandler(tokens) { InnerHandler = authTransport };
using var client = new HttpClient(auth);
using var response = await client.GetAsync("https://example.invalid/report?FromDate=1405%2F06%2F01");
Check(response.IsSuccessStatusCode && authTransport.Calls == 2 && tokens.Invalidations == 1 && authTransport.Authorization == "Bearer refreshed", "ESB handler refreshes token and retries after 401");

if (args.Length > 0)
{
    transport.Body = await File.ReadAllTextAsync(args[0]);
    report = await handler.Handle(valid, CancellationToken.None);
    var readings = report.UltraSonicReadAllReportModelList.OrderBy(x => x.GreDateTimeReadAll).ToArray();
    Check(readings.Length == 18, "User sample: all 18 readings deserialize");
    Check(readings.Last().PositiveTotalConsumption - readings.First().PositiveTotalConsumption == 18.868m, "User sample: precise positive volume delta");
    Check(readings.Last().NegativeTotalConsumption - readings.First().NegativeTotalConsumption == 3.681m, "User sample: precise reverse volume delta");
}
Console.WriteLine($"All {checks} checks passed.");

sealed class FakeFactory(FakeTransport transport) : IHttpClientFactory
{
    public string? Name { get; private set; }
    public HttpClient CreateClient(string name)
    {
        Name = name;
        return new HttpClient(transport, disposeHandler: false) { BaseAddress = new Uri("https://example.invalid/") };
    }
}
sealed class FakeTransport : HttpMessageHandler
{
    public string Body { get; set; } = "{}";
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public bool Timeout { get; set; }
    public bool RejectFirst { get; set; }
    public int Calls { get; private set; }
    public Uri? Uri { get; private set; }
    public HttpMethod? Method { get; private set; }
    public string? Authorization { get; private set; }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (Timeout) throw new TaskCanceledException("Simulated timeout");
        Calls++;
        Uri = request.RequestUri;
        Method = request.Method;
        Authorization = request.Headers.Authorization?.ToString();
        return Task.FromResult(new HttpResponseMessage(RejectFirst && Calls == 1 ? HttpStatusCode.Unauthorized : Status)
        {
            Content = new StringContent(Body, System.Text.Encoding.UTF8, "application/json")
        });
    }
}
sealed class FakeTokens : IEsbTokenProvider
{
    public int Invalidations { get; private set; }
    public Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new AuthenticationHeaderValue("Bearer", Invalidations == 0 ? "initial" : "refreshed"));
    public Task InvalidateAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        Invalidations++;
        return Task.CompletedTask;
    }
}

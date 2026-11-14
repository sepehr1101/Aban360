using System.Reflection;
using System.Text.Json;
using Aban360.Common.Db.Services.Dtos;
using DNTPersianUtils.Core;
using Aban360.Api.Controllers.V1.OldCalcPool.Processing;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Exceptions;
using Aban360.Common.Db.Services;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Hangfire;
using Hangfire.Common;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Validations;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

int checks = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception("FAIL: " + name);
    checks++;
    Console.WriteLine("PASS: " + name);
}

var configuration = new ConfigurationBuilder().Build();
var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddOldCalcPoolPersistenceInjections();
using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
Check(scope.ServiceProvider.GetRequiredService<INowGenerateBillRequestService>() is not null,
    "Request service resolves through existing persistence registration");

var requestService = new Requests();
var handlerType = typeof(INowGenerateBillHandler).Assembly.GetType(
    "Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations.NowGenerateBillHandler", true)!;
var arguments = handlerType.GetConstructors().Single().GetParameters().Select(parameter =>
    parameter.ParameterType == typeof(INowGenerateBillRequestService) ? (object)requestService :
    parameter.ParameterType == typeof(IConfiguration) ? configuration :
    parameter.ParameterType == typeof(IHttpContextAccessor) ? new HttpContextAccessor { HttpContext = new DefaultHttpContext() } :
    parameter.ParameterType.IsInstanceOfType(new NowGenerateBillValidator()) ? new NowGenerateBillValidator() :
    Stub.For(parameter.ParameterType, (_, _) => throw new Exception("Bill processing must not run"))).ToArray();
var handler = (INowGenerateBillHandler)Activator.CreateInstance(handlerType, arguments)!;
var invalid = new NowGenerateBillInputDto { BillId = "", MobileNumber = "", CurrentMeterNumber = 0 };
using var cancellation = new CancellationTokenSource();
var valid = new NowGenerateBillInputDto { BillId = "1234567890123", MobileNumber = "09123456789", CurrentMeterNumber = 100 };

foreach (var code in new[] { NowGenerateBillErrorCodeEnum.MobileDailyLimitReached, NowGenerateBillErrorCodeEnum.BillDailyLimitReached })
{
    requestService.Result = code;
    int calls = requestService.Calls;
    var output = await handler.Handle(invalid, new AppUserManual(), cancellation.Token);
    Check(output.ErrorCode == (int)code && !string.IsNullOrWhiteSpace(output.Message),
        $"{code} returns a message before input validation or bill processing");
    Check(output.Message == (code == NowGenerateBillErrorCodeEnum.MobileDailyLimitReached
        ? "به حداکثر تعداد درخواست در روز رسیده‌اید."
        : "امکان صدور قبض برای این شناسه قبض در امروز وجود ندارد چون به حداکثر دفعات مجاز اعلام رقم کنتور رسیده است."),
        "Returns the specific Persian message for the exceeded limit");
    Check(requestService.Calls == calls + 1 && requestService.Input == invalid && requestService.Token == cancellation.Token,
        "Registers exactly once and passes input and cancellation token");

    int logCalls = 0;
    var jobs = new List<Job>();
    var controllerArgs = typeof(GenerateBillController).GetConstructors().Single().GetParameters().Select(parameter =>
        parameter.ParameterType == typeof(INowGenerateBillHandler) ? (object)handler :
        Stub.For(parameter.ParameterType, (method, values) =>
        {
            if (parameter.ParameterType == typeof(IOpLogCommandService) && method.Name == "Insert")
            {
                logCalls++;
                return Task.CompletedTask;
            }
            if (parameter.ParameterType == typeof(IBackgroundJobClient) && method.Name == "Create")
            {
                jobs.Add((Job)values[0]!);
                return "failure-sms";
            }
            throw new Exception("Unexpected controller dependency: " + method.Name);
        })).ToArray();
    var controller = (GenerateBillController)Activator.CreateInstance(typeof(GenerateBillController), controllerArgs)!;
    controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
    var response = (ObjectResult)await controller.GetInstantly(valid, cancellation.Token);
    var envelope = (ApiResponseEnvelope<NowGenerateBillOutputDto>)response.Value!;
    Check(response.StatusCode == 429 && envelope.Data?.ErrorCode == (int)code,
        $"{code} returns actual HTTP 429 and preserves the specific error code");
    Check(logCalls == 2, "Limit rejection records request and failure operation logs");
    Check(jobs.Count == 1 && jobs[0].Type == typeof(ISmsOldHandler) && jobs[0].Method.Name == "SendNew"
        && (string)jobs[0].Args[0] == valid.MobileNumber
        && (string)jobs[0].Args[1] == string.Format(SmsTemplates.NowGenerateBillFailed, valid.BillId, SmsTemplates.NowGenerateBillDailyLimitReason)
        && (Guid)jobs[0].Args[2] != Guid.Empty,
        "Queues exactly one SMS to the requested mobile with bill ID, reason and tracking ID");
    Check(((string)jobs[0].Args[1]).StartsWith("آبفا استان اصفهان" + Environment.NewLine),
        "Failure SMS begins with the approved organization name");
}

requestService.Result = NowGenerateBillErrorCodeEnum.Success;
int previousCalls = requestService.Calls;
try
{
    await handler.Handle(invalid, new AppUserManual(), CancellationToken.None);
    throw new Exception("Input validation was skipped");
}
catch (CustomValidationException)
{
    Check(requestService.Calls == previousCalls + 1, "Invalid input is registered before existing validation rejects it");
}
var unauthorized = await handler.Handle(valid, new AppUserManual(), CancellationToken.None);
Check(unauthorized.ErrorCode == (int)NowGenerateBillErrorCodeEnum.BadRequest,
    "Requests below the limit still follow existing API key validation");
Check(unauthorized.SmsCategory == NowGenerateBillSmsCategoryEnum.System,
    "Authentication failure is explicitly categorized as a system problem");

var recentBill = (NowGenerateBillOutputDto)handlerType.GetMethod("PreviousInfoValidate", BindingFlags.Instance | BindingFlags.NonPublic)!
    .Invoke(handler, new object[] { valid, new CustomerLastInfoGetDto
    {
        LastMeterNumber = 10,
        LastMeterDateJalali = DateTime.Now.ToShortPersianDateString()
    } })!;
Check(recentBill.ErrorCode == (int)NowGenerateBillErrorCodeEnum.Low && recentBill.SmsCategory == NowGenerateBillSmsCategoryEnum.RecentBill,
    "Recent issuance keeps its API error code and is distinct from low consumption for SMS");

foreach (bool limitReached in new[] { true, false })
{
    int historyCalls = 0;
    int calculationCalls = 0;
    var context = new DefaultHttpContext();
    context.Request.Headers[Aban360.ReportPool.Domain.Base.ReportLiterals.XApiKey] =
        Aban360.ReportPool.Domain.Base.ReportLiterals.NowGenerateBillXApiKey;
    var limitArguments = handlerType.GetConstructors().Single().GetParameters().Select(parameter =>
        parameter.ParameterType == typeof(INowGenerateBillRequestService) ? (object)requestService :
        parameter.ParameterType == typeof(IConfiguration) ? configuration :
        parameter.ParameterType == typeof(IHttpContextAccessor) ? new HttpContextAccessor { HttpContext = context } :
        parameter.ParameterType.IsInstanceOfType(new NowGenerateBillValidator()) ? new NowGenerateBillValidator() :
        Stub.For(parameter.ParameterType, (method, values) =>
        {
            if (parameter.ParameterType == typeof(ICommonMemberQueryService))
            {
                if (method.Name == "Get")
                    return Task.FromResult<ZoneIdAndCustomerNumber?>(new ZoneIdAndCustomerNumber(1, 42));
                if (method.Name == "GetMembersBedBesTavizFlatInfo")
                    return Task.FromResult<CustomerLastInfoGetDto?>(new CustomerLastInfoGetDto
                    {
                        ZoneId = 1, CustomerNumber = 42, LastMeterNumber = 10,
                        LastMeterDateJalali = DateTime.Now.AddDays(-30).ToShortPersianDateString()
                    });
            }
            if (parameter.ParameterType == typeof(IBedBesQueryService)
                && method.Name == "HasConsecutiveSelfClaimBills")
            {
                historyCalls++;
                Check((int)values[0]! == 1 && (int)values[1]! == 42 && (int)values[2]! == 3,
                    "History check receives customer identity and the handler period limit");
                return Task.FromResult(limitReached);
            }
            if (parameter.ParameterType == typeof(IFreeGenerateBillHandler))
            {
                calculationCalls++;
                throw new InvalidOperationException("Calculation reached");
            }
            throw new Exception("Unexpected dependency: " + method.Name);
        })).ToArray();
    var limitHandler = (INowGenerateBillHandler)Activator.CreateInstance(handlerType, limitArguments)!;
    if (limitReached)
    {
        var output = await limitHandler.Handle(valid, new AppUserManual(), CancellationToken.None);
        Check(output.ErrorCode == (int)NowGenerateBillErrorCodeEnum.NotAvailableNow
            && output.Message == "به سقف 3 دوره متوالی صدور قبض خوداظهاری رسیده‌اید.",
            "Consecutive period limit returns the existing error code and the configured limit");
        Check(historyCalls == 1 && calculationCalls == 0,
            "Limit rejection happens before any calculation or issuance");
    }
    else
    {
        try
        {
            await limitHandler.Handle(valid, new AppUserManual(), CancellationToken.None);
            throw new Exception("Expected calculation to be reached");
        }
        catch (InvalidOperationException exception) when (exception.Message == "Calculation reached")
        {
            Check(historyCalls == 1 && calculationCalls == 1,
                "Requests below the consecutive period limit continue to calculation");
        }
    }
}

var categoryCases = new (NowGenerateBillErrorCodeEnum Code, string Detail, NowGenerateBillSmsCategoryEnum? Category, string PublicReason)[]
{
    (NowGenerateBillErrorCodeEnum.MobileDailyLimitReached, "به حداکثر تعداد درخواست در روز رسیده‌اید.", null, SmsTemplates.NowGenerateBillDailyLimitReason),
    (NowGenerateBillErrorCodeEnum.BillDailyLimitReached, "حداکثر دفعات اعلام رقم کنتور", null, SmsTemplates.NowGenerateBillDailyLimitReason),
    (NowGenerateBillErrorCodeEnum.BadRequest, "شناسه قبض معتبر نیست.", null, SmsTemplates.NowGenerateBillSubscriptionReason),
    (NowGenerateBillErrorCodeEnum.BadRequest, "اطلاعات قبض قبلی به درستی یافت نشد.", null, SmsTemplates.NowGenerateBillSubscriptionReason),
    (NowGenerateBillErrorCodeEnum.InvalidDeletionState, "انشعاب جمع‌آوری شده است.", null, SmsTemplates.NowGenerateBillSubscriptionReason),
    (NowGenerateBillErrorCodeEnum.PreNumberHigherThanTodayNumber, "رقم فعلی کمتر از رقم قبلی است.", null, SmsTemplates.NowGenerateBillReadingReviewReason),
    (NowGenerateBillErrorCodeEnum.PreNumberEqualTodayNumber, "رقم فعلی برابر با رقم قبلی است.", null, SmsTemplates.NowGenerateBillReadingReviewReason),
    (NowGenerateBillErrorCodeEnum.High, "میانگین مصرف دوره جاری بیش از 50درصد دوره قبلی است.", null, SmsTemplates.NowGenerateBillReadingReviewReason),
    (NowGenerateBillErrorCodeEnum.Low, "میانگین مصرف دوره جاری کمتر از 50درصد دوره قبلی است.", null, SmsTemplates.NowGenerateBillReadingReviewReason),
    (NowGenerateBillErrorCodeEnum.High, "مبلغ دوره جاری بیش از 5میلیون تومان است.", null, SmsTemplates.NowGenerateBillReadingReviewReason),
    (NowGenerateBillErrorCodeEnum.Low, "مصرف دوره جاری نمی‌تواند کمتر از 2مترمکعب باشد.", null, SmsTemplates.NowGenerateBillReadingReviewReason),
    (NowGenerateBillErrorCodeEnum.Low, recentBill.Message, recentBill.SmsCategory, SmsTemplates.NowGenerateBillRecentBillReason),
    (NowGenerateBillErrorCodeEnum.BadRequest, unauthorized.Message, unauthorized.SmsCategory, SmsTemplates.NowGenerateBillSystemReason),
    (NowGenerateBillErrorCodeEnum.NotAvailableNow, "سیستم قادر به محاسبه نیست", null, SmsTemplates.NowGenerateBillSystemReason)
};
foreach (var item in categoryCases)
{
    var original = new NowGenerateBillOutputDto(item.Detail, (int)item.Code) { SmsCategory = item.Category };
    var outcome = await InvokeController(() => Task.FromResult(original), valid);
    Check(outcome.Jobs.Count == 1 && (string)outcome.Jobs[0].Args[1]
        == string.Format(SmsTemplates.NowGenerateBillFailed, valid.BillId, item.PublicReason),
        "SMS uses the approved category text without the exact cause: " + item.Detail);
    var data = ((ApiResponseEnvelope<NowGenerateBillOutputDto>)outcome.Response.Value!).Data!;
    Check(data.ErrorCode == (int)item.Code && data.Message == item.Detail,
        "API keeps the exact reason and original error code");
}
Check(!JsonDocument.Parse(JsonSerializer.Serialize(recentBill)).RootElement.TryGetProperty("SmsCategory", out _),
    "SMS category metadata is excluded from API JSON");

var unexpectedFailure = await InvokeController(() => throw new Exception("Private database error"), valid);
Check(unexpectedFailure.Jobs.Count == 1 && ((string)unexpectedFailure.Jobs[0].Args[1]).Contains(SmsTemplates.NowGenerateBillSystemReason)
    && !((string)unexpectedFailure.Jobs[0].Args[1]).Contains("Private database error"),
    "Unexpected calculation failure queues a generic SMS without exposing internal details");

var success = await InvokeController(() => Task.FromResult(new NowGenerateBillOutputDto("موفق", 0)), valid);
Check(success.Jobs.Count == 0 && success.SuccessSmsCalls == 1,
    "Successful issuance preserves the existing success SMS without a failure SMS");
var successSmsFailure = await InvokeController(() => Task.FromResult(new NowGenerateBillOutputDto("موفق", 0)), valid,
    failSuccessSms: true);
Check(successSmsFailure.Jobs.Count == 0 && successSmsFailure.SuccessSmsCalls == 1,
    "Success SMS failure never reports an already issued bill as unissued");

foreach (string mobile in new[] { "", "123", "09123456789,09111111111" })
{
    var invalidMobile = await InvokeController(() => Task.FromResult(new NowGenerateBillOutputDto("خطا", 1)), valid with { MobileNumber = mobile });
    Check(invalidMobile.Jobs.Count == 0, "Invalid mobile never queues an SMS: " + mobile);
}
var queueFailure = await InvokeController(() => Task.FromResult(new NowGenerateBillOutputDto("سقف روزانه", 10)), valid,
    failQueue: true);
Check(queueFailure.Response.StatusCode == 429 && queueFailure.Jobs.Count == 1,
    "Queue failure preserves the original API result and does not enqueue a second SMS");
Console.WriteLine($"{checks} checks passed.");

async Task<(ObjectResult Response, List<Job> Jobs, int SuccessSmsCalls)> InvokeController(
    Func<Task<NowGenerateBillOutputDto>> result, NowGenerateBillInputDto input, bool failSuccessSms = false, bool failQueue = false)
{
    var jobs = new List<Job>();
    int successSmsCalls = 0;
    var args = typeof(GenerateBillController).GetConstructors().Single().GetParameters().Select(parameter =>
        Stub.For(parameter.ParameterType, (method, values) =>
        {
            if (parameter.ParameterType == typeof(INowGenerateBillHandler)) return result();
            if (parameter.ParameterType == typeof(IOpLogCommandService)) return Task.CompletedTask;
            if (parameter.ParameterType == typeof(ILatestBillSendSmsHandler))
            {
                successSmsCalls++;
                if (failSuccessSms) throw new Exception("Success SMS failed");
                return Task.CompletedTask;
            }
            if (parameter.ParameterType == typeof(IBackgroundJobClient) && method.Name == "Create")
            {
                jobs.Add((Job)values[0]!);
                if (failQueue) throw new Exception("SMS queue unavailable");
                return "failure-sms";
            }
            throw new Exception("Unexpected dependency: " + method.Name);
        })).ToArray();
    var controller = (GenerateBillController)Activator.CreateInstance(typeof(GenerateBillController), args)!;
    controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
    var response = (ObjectResult)await controller.GetInstantly(input, CancellationToken.None);
    return (response, jobs, successSmsCalls);
}

sealed class Requests : INowGenerateBillRequestService
{
    public NowGenerateBillErrorCodeEnum Result;
    public int Calls;
    public NowGenerateBillInputDto? Input;
    public CancellationToken Token;
    public Task<NowGenerateBillErrorCodeEnum> Register(NowGenerateBillInputDto input, CancellationToken token)
    {
        Calls++;
        Input = input;
        Token = token;
        return Task.FromResult(Result);
    }
}

class Stub : DispatchProxy
{
    public Func<MethodInfo, object?[], object?> Callback = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Callback(method!, args!);
    public static object For(Type contract, Func<MethodInfo, object?[], object?> callback)
    {
        var proxy = Create(contract, typeof(Stub));
        ((Stub)proxy).Callback = callback;
        return proxy;
    }
}

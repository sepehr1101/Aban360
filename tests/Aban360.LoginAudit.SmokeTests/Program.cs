using System.Reflection;
using System.Text.Json;
using Aban360.Api.Controllers.V1.UserPool.Auth.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

int checks = 0;
void Check(bool value, string name)
{
    if (!value) throw new Exception(name);
    checks++;
    Console.WriteLine($"PASS: {name}");
}
var knownUser = new User { Id = Guid.NewGuid(), Username = "reader", FullName = "کاربر آزمایشی" };
var input = new FirstStepLoginInput
{
    Username = "reader", Password = "never-include-this-secret-in-oplog",
    CaptchaInputText = "123", CaptchaToken = "test-token"
};
foreach (User? user in new User?[] { null, knownUser })
{
    var fixture = new LoginFixture(user, validCredentials: false);
    var result = await fixture.Controller.PaceFirstStep(input, default);
    Check(result is BadRequestObjectResult { Value: ApiResponseEnvelope<object> response }
        && !response.Success && response.Errors.Single().Message == MessageResources.UserNotFound,
        "Invalid credentials retain the existing generic login response");
    Check(fixture.Logs.Count == 1, "Exactly one OpLog insert is awaited for rejected credentials");
    var log = fixture.Logs.Single();
    Check(log.Username == input.Username && log.UserId == (user?.Id ?? Guid.Empty)
        && log.FullName == (user?.FullName ?? string.Empty),
        "Audit uses the attempted username and only a resolved user's ID and name");
    Check(log.Message == "ورود ناموفق به سامانه؛ نام کاربری یا رمز عبور نادرست است."
        && !JsonSerializer.Serialize(log).Contains(input.Password)
        && !JsonSerializer.Serialize(log).Contains(input.CaptchaToken),
        "Failure message is useful and OpLog receives no password or captcha token");
    Check(fixture.Calls.All(call => !call.StartsWith("IUserPolicy") && !call.StartsWith("ITokenFactory")
        && !call.StartsWith("IUserTokenCreate") && !call.StartsWith("IUserLoginAdd")),
        "Rejected credentials never proceed to policy, tokens or successful login");
}

var brokenAudit = new LoginFixture(null, validCredentials: false) { FailAudit = true };
Check(await brokenAudit.Controller.PaceFirstStep(input, default) is BadRequestObjectResult
    && brokenAudit.ErrorCount == 1 && brokenAudit.Logs.Count == 1,
    "OpLog database failure is reported to the logger without changing credential rejection");

var badCaptcha = new LoginFixture(null, validCredentials: false) { CaptchaValue = "999" };
Check(await badCaptcha.Controller.PaceFirstStep(input, default) is BadRequestObjectResult
    && badCaptcha.Logs.Count == 0 && !badCaptcha.Calls.Any(call => call.StartsWith("IUserFindByUsernamePassword")),
    "Rejected captcha is not falsely classified as an incorrect username or password");

foreach (bool twoStep in new[] { false, true })
{
    knownUser.HasTwoStepVerification = twoStep;
    var successful = new LoginFixture(knownUser, validCredentials: true);
    var result = await successful.Controller.PaceFirstStep(input, default);
    Check(successful.Logs.Count == 0, "Successful credentials do not produce a failed-login OpLog");
    if (twoStep)
        Check(result is OkObjectResult { Value: ApiResponseEnvelope<FirstStepOutput> response }
            && response.Success && response.SuccessMessage == "second-step"
            && response.Data?.TempCode == "temporary-code",
            "Two-step login still returns its temporary code and second-step response");
    else
        Check(result is OkObjectResult { Value: ApiResponseEnvelope<SecondStepOutput> response }
            && response.Success && response.Data?.AccessToken == "access-token"
            && successful.Calls.Any(call => call.StartsWith("IUserTokenCreateHandler")),
            "Single-step login still creates and returns tokens");
    Check(successful.Calls.Count(call => call == "IUnitOfWork.SaveChangesAsync") == 1,
        "Successful login still saves its existing changes once");
}

var policyRejected = new LoginFixture(knownUser, validCredentials: true) { PolicyAllowed = false };
Check(await policyRejected.Controller.PaceFirstStep(input, default) is BadRequestObjectResult
    && policyRejected.Logs.Count == 0,
    "Policy rejection after valid credentials is not logged as a wrong password");

input.Username = "missing\r\nname";
var multiline = new LoginFixture(null, validCredentials: false);
await multiline.Controller.PaceFirstStep(input, default);
Check(multiline.Logs.Single().Username == "missing  name", "Audit username cannot inject additional log lines");
input.Username = null!;
var missingName = new LoginFixture(null, validCredentials: false);
Check(await missingName.Controller.PaceFirstStep(input, default) is BadRequestObjectResult
    && missingName.Logs.Single().Username == string.Empty,
    "Missing username does not make audit logging throw a null-reference error");
Console.WriteLine($"{checks} checks passed.");

record AuditEntry(string Message, Guid UserId, string Username, string FullName);

sealed class LoginFixture
{
    public LoginController Controller { get; }
    public List<AuditEntry> Logs { get; } = new();
    public List<string> Calls { get; } = new();
    public bool FailAudit { get; set; }
    public bool PolicyAllowed { get; set; } = true;
    public string CaptchaValue { get; set; } = "123";
    public int ErrorCount { get; private set; }

    public LoginFixture(User? user, bool validCredentials)
    {
        var constructor = typeof(LoginController).GetConstructors().Single();
        var arguments = constructor.GetParameters().Select(parameter =>
        {
            Type type = parameter.ParameterType;
            if (type == typeof(IOptions<DNTCaptchaOptions>))
                return (object)Options.Create(new DNTCaptchaOptions());
            return Proxy.For(type, (method, args) =>
            {
                Calls.Add($"{type.Name}.{method.Name}");
                if (type == typeof(IOpLogCommandService))
                {
                    var identity = (IAppUser)args[1]!;
                    Logs.Add(new((string)args[0]!, identity.UserId, identity.Username, identity.FullName));
                    return FailAudit ? Task.FromException(new IOException("Audit database unavailable")) : Task.CompletedTask;
                }
                if (type == typeof(ILogger<LoginController>))
                {
                    if (method.Name == "Log" && (LogLevel)args[0]! == LogLevel.Error) ErrorCount++;
                    return method.ReturnType == typeof(bool) ? true : null;
                }
                return method.Name switch
                {
                    "Decrypt" => CaptchaValue,
                    "Handle" when type.Name == "IUserFindByUsernamePasswordHandler" => Task.FromResult((user, validCredentials)),
                    "Handle" when type.Name == "IUserPolicyQueryHandler" => Task.FromResult((PolicyAllowed ? "" : "Policy denied", PolicyAllowed)),
                    "Handle" when type.Name == "IUserLoginAddHandler" => Task.FromResult(new FirstStepOutput(Guid.NewGuid(), 1, "temporary-code")),
                    "CreateJwtTokensAsync" => Task.FromResult(new JwtTokenData { AccessToken = "access-token", RefreshToken = "refresh-token" }),
                    "Handle" when type.Name == "IUserTokenCreateHandler" => Task.CompletedTask,
                    "SaveChangesAsync" => Task.FromResult(1),
                    _ => throw new InvalidOperationException($"Unexpected dependency call: {type.Name}.{method.Name}")
                };
            });
        }).ToArray();
        Controller = (LoginController)constructor.Invoke(arguments);
    }
}

class Proxy : DispatchProxy
{
    public Func<MethodInfo, object?[], object?> Callback = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Callback(method!, args!);
    public static object For(Type type, Func<MethodInfo, object?[], object?> callback)
    {
        var proxy = (Proxy)Create(type, typeof(Proxy));
        proxy.Callback = callback;
        return proxy;
    }
}

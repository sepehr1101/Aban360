using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services.Dtos;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

int comparisons = 0;
int comparedFields = 0;
foreach (int seed in new[] { 1, 2, 3, 4, 5, 6 })
{
    await Compare("aggregate-bills", seed, fixture => Task.FromResult<object?>(
        fixture.Base.GetBedbes(fixture.Bills.AsEnumerable().Reverse(), fixture.Customer)));
    await Compare("previous-bill-mapping", seed, fixture => Task.FromResult<object?>(
        fixture.Base.GetBedBes(fixture.Base.GetBedbes(fixture.Bills, fixture.Customer), 3, 0, 2, "Test")));
    await Compare("full-zero-calculation", seed, fixture => Task.FromResult<object?>(
        fixture.Base.GetFullNewCalculation(fixture.Base.GetBedbes(fixture.Bills, fixture.Customer), 2, 3, 0, "Test")));

    foreach (decimal wastedAmount in new[] { 0m, 12.5m, 1234.125m })
    {
        await Compare("partial-calculation-waste-" + wastedAmount, seed, fixture => Task.FromResult<object?>(
            fixture.Base.GetNewCalculation(fixture.Tariff, fixture.Base.GetBedbes(fixture.Bills, fixture.Customer),
                2, 3, 8.75f, (long)wastedAmount, 0, "Test")));
        await Compare("difference-waste-" + wastedAmount, seed, fixture =>
        {
            var previous = fixture.Base.GetBedbes(fixture.Bills, fixture.Customer);
            var next = fixture.Base.GetNewCalculation(fixture.Tariff, previous, 2, 3, 8.75f, (long)wastedAmount, 0, "Test");
            return Task.FromResult<object?>(fixture.Base.GetDifferent(previous, next, 0, "Test"));
        });
    }

    foreach (bool partial in new[] { false, true })
    {
        await Compare(partial ? "partial-report-and-input-mutations" : "full-report-and-input-aliasing", seed, async fixture =>
        {
            var previous = fixture.Base.GetBedbes(fixture.Bills, fixture.Customer);
            var original = fixture.Base.GetBedBes(previous, 3, 0, 2, "Test");
            var next = partial
                ? fixture.Base.GetNewCalculation(fixture.Tariff, previous, 2, 3, 0, 0, 0, "Test")
                : fixture.Base.GetFullNewCalculation(previous, 2, 3, 0, "Test");
            var difference = partial ? fixture.Base.GetDifferent(previous, next, 0, "Test") : original;
            var report = await fixture.Base.GetReturn(original, next, difference, fixture.Customer, 3, false,
                partial, new AppUserManual(), "1404/01/01", "1404/04/01");
            return new { report, original, next, difference };
        });
    }

    await Compare("full-handler-preview", seed, async fixture =>
        await ((IReturnBillFullHandler)fixture.Create("ReturnBillFullHandler")).Handle(
            new ReturnBillFullInputDto
            {
                BillId = fixture.Customer.BillId, FromDateJalali = "1404/01/01", ToDateJalali = "1404/04/01",
                ReturnCauseId = 2, IsConfirm = false, MinutesNumber = 98765, Description = "Test"
            }, new AppUserManual(), CancellationToken.None));

    foreach (int cause in new[] { 1, 2, 5, 7, 9 })
    {
        await Compare("partial-handler-preview-cause-" + cause, seed, async fixture =>
            await ((IReturnBillPartialHandler)fixture.Create("ReturnBillPartialHandler")).Handle(
                new ReturnBillPartialInputDto
                {
                    BillId = fixture.Customer.BillId, FromDateJalali = "1404/01/01", ToDateJalali = "1404/04/01",
                    ReturnCauseId = cause, IsConfirm = false, MinutesNumber = 98765, Description = "Test",
                    CalculationType = ReturnedBillCalculationTypeEnum.UserInput, UserInput = 10.25f
                }, new AppUserManual(), CancellationToken.None));
    }

    foreach (ReturnBillManualTypeEnum mode in Enum.GetValues<ReturnBillManualTypeEnum>())
    {
        await Compare("manual-mappings-" + mode, seed, async fixture =>
        {
            object manual = fixture.Create("ReturnBillManualHandler");
            var previous = fixture.Base.GetBedbes(fixture.Bills, fixture.Customer);
            var member = ComparisonFixture.Sample<CustomerLastInfoGetDto>(seed);
            var input = ComparisonFixture.Sample<ReturnBillManualInputDto>(seed);
            input.ReturnType = mode;
            input.IsConfirm = false;
            var repair = await Invoke(manual, "GetRepairDto", previous, member, input, 0, 3, 43210L);
            var autoBack = await Invoke(manual, "GetAutoBackCreate", repair!, "Test");
            var result = await Invoke(manual, "GetResult", repair!, fixture.Customer, "Test");
            return new { repair, autoBack, result };
        });
    }
}

Console.WriteLine($"PASS: {comparisons} frozen-baseline comparisons; {comparedFields} scalar fields compared.");
Console.WriteLine("No production database writes. These tests preserve behavior; they do not certify tariff correctness.");

async Task Compare(string name, int seed, Func<ComparisonFixture, Task<object?>> action)
{
    var oldFixture = new ComparisonFixture(true, seed);
    var currentFixture = new ComparisonFixture(false, seed);
    object? expected = await action(oldFixture);
    object? actual = await action(currentFixture);
    var expectedNode = JsonSerializer.SerializeToNode(new { Result = expected, Calls = oldFixture.Calls });
    var actualNode = JsonSerializer.SerializeToNode(new { Result = actual, Calls = currentFixture.Calls });
    Verify(expectedNode, actualNode, name + "[" + seed + "]");
    comparisons++;
    Console.WriteLine("PASS: " + name + " [" + seed + "]");
}

void Verify(JsonNode? expected, JsonNode? actual, string path)
{
    if (expected is JsonObject expectedObject && actual is JsonObject actualObject)
    {
        if (!expectedObject.Select(pair => pair.Key).ToHashSet().SetEquals(actualObject.Select(pair => pair.Key)))
            throw new Exception("Property set changed: " + path);
        foreach (var pair in expectedObject) Verify(pair.Value, actualObject[pair.Key], path + "." + pair.Key);
        return;
    }
    if (expected is JsonArray expectedArray && actual is JsonArray actualArray)
    {
        if (expectedArray.Count != actualArray.Count) throw new Exception("Array length changed: " + path);
        for (int i = 0; i < expectedArray.Count; i++) Verify(expectedArray[i], actualArray[i], path + "[" + i + "]");
        return;
    }
    comparedFields++;
    if (!JsonNode.DeepEquals(expected, actual))
        throw new Exception($"Behavior changed at {path}: expected {expected}, actual {actual}");
}

static async Task<object?> Invoke(object instance, string methodName, params object[] arguments)
{
    try
    {
        object? result = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(instance, arguments);
        if (result is not Task task) return result;
        await task;
        return task.GetType().GetProperty("Result")?.GetValue(task);
    }
    catch (TargetInvocationException exception) when (exception.InnerException is not null)
    {
        ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
        throw;
    }
}

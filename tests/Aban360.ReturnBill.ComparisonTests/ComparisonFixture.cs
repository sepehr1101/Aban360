using System.Reflection;
using System.Text.Json;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services.Dtos;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

internal sealed class ComparisonFixture
{
    public readonly List<string> Calls = new();
    public readonly CustomerInfoOutputDto Customer;
    public readonly List<BedBesCreateDto> Bills;
    public readonly AbBahaCalculationDetails Tariff;
    public float? PreviousAverage = 12.75f;
    private readonly bool baseline;
    private IReturnBillBaseHandler? baseHandler;

    public ComparisonFixture(bool baseline, int seed)
    {
        this.baseline = baseline;
        Customer = Sample<CustomerInfoOutputDto>(seed);
        Customer.ZoneId = 100;
        Customer.Radif = 321;
        Customer.UsageId = seed % 2 == 0 ? 1 : 5;
        Customer.DomesticUnit = 2;
        Customer.EmptyUnit = 0;
        Customer.HouseholdNumber = 0;
        Bills = new List<BedBesCreateDto>();
        for (int i = 0; i < 3; i++)
        {
            var bill = Sample<BedBesCreateDto>(seed + i);
            bill.Id = i + 1;
            bill.Town = Customer.ZoneId;
            bill.Radif = Customer.Radif;
            bill.PriDate = $"1404/0{i + 1}/01";
            bill.TodayDate = $"1404/0{i + 2}/01";
            bill.DateBed = bill.TodayDate;
            bill.CodVas = 1;
            bill.Del = false;
            Bills.Add(bill);
        }
        Tariff = Sample<AbBahaCalculationDetails>(seed);
        Tariff.Customer = Customer;
        Tariff.MeterInfo = new MeterInfoOutputDto("1404/01/01", "1404/04/01", 100, 180, 1);
        Tariff.Consumption = new[] { 0d, 12.25d, 12.35d, 81.55d }[seed % 4];
        Tariff.Duration = 90;
        Tariff.SumItems = seed % 3 == 0 ? 0 : seed % 3 == 1 ? 123456789.5 : 7654.125;
    }

    public IReturnBillBaseHandler Base => baseHandler ??= (IReturnBillBaseHandler)Create("ReturnBillBaseHandler");

    public object Create(string name)
    {
        var assembly = baseline ? typeof(ComparisonFixture).Assembly : typeof(IReturnBillBaseHandler).Assembly;
        string ns = baseline ? "Aban360.ReturnBill.ComparisonTests.Baseline" :
            "Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations";
        Type type = assembly.GetType(ns + "." + name, true)!;
        var arguments = type.GetConstructors().Single().GetParameters().Select(parameter =>
        {
            Type dependency = parameter.ParameterType;
            if (dependency == typeof(IConfiguration)) return (object)new ConfigurationBuilder().Build();
            if (dependency == typeof(IReturnBillBaseHandler)) return Base;
            if (dependency.Name == "ITaxCalculator")
                return Activator.CreateInstance(dependency.Assembly.GetType(
                    "Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators.TaxCalculator", true)!, true)!;
            return DependencyStub.For(dependency, (method, values) => Resolve(dependency, method, values));
        }).ToArray();
        return Activator.CreateInstance(type, arguments)!;
    }

    private object? Resolve(Type dependency, MethodInfo method, object?[] values)
    {
        Calls.Add(JsonSerializer.Serialize(new
        {
            Dependency = dependency.Name,
            Method = method.Name,
            Arguments = values.Select(value => value is CancellationToken ? (object)"<cancellation-token>" :
                value is IAppUser ? "<user>" : value).ToArray()
        }));
        string name = dependency.Name;
        if (name.StartsWith("IValidator") && method.Name == "ValidateAsync")
            return Task.FromResult(new ValidationResult());
        if (name == "ICustomerInfoDetailQueryService" && method.Name == "GetInfo")
            return Task.FromResult(Customer);
        if (name == "ICommonZoneService" && method.Name == "IsUserInZone")
            return Task.FromResult<IEnumerable<int>>(new[] { Customer.ZoneId });
        if (name == "ICommonMemberQueryService" && method.Name == "Get")
            return Task.FromResult<MemberInfoGetDto?>(new MemberInfoGetDto { DebtAmount = 43210 });
        if (name == "IHBedBesQueryService" && method.Name == "Get") return Task.FromResult(false);
        if (name == "IBillReturnCauseQueryService" && method.Name == "Get")
            return values[0] is bool
                ? Task.FromResult<IEnumerable<NumericDictionary>>(Enumerable.Range(1, 18).Select(id => new NumericDictionary(id, "Test")))
                : Task.FromResult(new BillReturnCauseGetDto { Id = 1, Title = "Test" });
        if (name == "IBedBesQueryService")
        {
            if (method.Name == "Get") return Task.FromResult<IEnumerable<BedBesCreateDto>>(Bills);
            if (method.Name == "GetCountInDateBed") return Task.FromResult(1);
            if (method.Name == "GetAverage") return Task.FromResult(PreviousAverage);
        }
        if (name == "IOldTariffEngine" && method.Name == "Handle") return Task.FromResult(Tariff);
        if (name == "ISQueryService" && method.Name == "Get")
            return Task.FromResult(new SGetDto { Olgo = 11 });
        if (name == "IZaribCQueryService" && method.Name == "GetLatest")
            return Task.FromResult(new ZaribCQueryDto { C = 2000 });
        if (name == "IRepairQueryService" && method.Name == "GetRepairDateValidate")
            return Task.FromResult<Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries.RepairedOutputDto?>(null);
        throw new InvalidOperationException("Unexpected dependency or write during comparison: " + name + "." + method.Name);
    }

    public static T Sample<T>(int seed) where T : new()
    {
        var value = new T();
        int index = 0;
        foreach (var property in typeof(T).GetProperties().Where(p => p.CanWrite && p.GetIndexParameters().Length == 0))
        {
            index++;
            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            decimal amount = seed * 1000m + index * 1.125m;
            object? item = type == typeof(decimal) ? amount :
                type == typeof(double) ? (double)amount :
                type == typeof(float) ? (float)amount :
                type == typeof(int) ? seed * 10 + index :
                type == typeof(long) ? (long)seed * 10 + index :
                type == typeof(short) ? (short)(seed + index) :
                type == typeof(bool) ? index % 2 == 0 :
                type == typeof(string) ? property.Name.Contains("Date") ? "1404/01/01" : property.Name + seed : null;
            if (item is not null) property.SetValue(value, item);
        }
        return value;
    }
}

public class DependencyStub : DispatchProxy
{
    public Func<MethodInfo, object?[], object?> Callback = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Callback(method!, args!);
    public static object For(Type contract, Func<MethodInfo, object?[], object?> callback)
    {
        object proxy = Create(contract, typeof(DependencyStub));
        ((DependencyStub)proxy).Callback = callback;
        return proxy;
    }
}

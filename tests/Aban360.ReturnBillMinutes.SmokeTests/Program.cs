using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

int checks = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    checks++;
    Console.WriteLine("PASS: " + message);
}

var handlerType = typeof(IReturnBillBaseHandler).Assembly.GetType(
    "Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations.ReturnBillBaseHandler", true)!;
var configuration = new ConfigurationBuilder().Build();
var arguments = handlerType.GetConstructors().Single().GetParameters().Select(parameter =>
    parameter.ParameterType == typeof(IConfiguration) ? (object)configuration :
    Stub.For(parameter.ParameterType, (_, _) =>
    {
        if (parameter.ParameterType == typeof(ICommonMemberQueryService))
            return Task.FromResult<MemberInfoGetDto?>(new MemberInfoGetDto());
        if (parameter.ParameterType == typeof(Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts.IBillReturnCauseQueryService))
            return Task.FromResult(new BillReturnCauseGetDto { Title = "Test" });
        if (parameter.ParameterType == typeof(IHBedBesQueryService))
            return Task.FromResult(false);
        throw new Exception("Preview must not allocate numbers or call a write dependency: " + parameter.Name);
    })).ToArray();
var handler = (IReturnBillBaseHandler)Activator.CreateInstance(handlerType, arguments)!;
foreach (bool isPartial in new[] { false, true })
{
    for (int i = 0; i < 3; i++)
    {
        var result = await handler.GetReturn(new AutoBackCreateDto(), new AutoBackCreateDto(),
            new AutoBackCreateDto(), new CustomerInfoOutputDto(), 1, false, isPartial,
            new AppUserManual(), "1405/01/01", "1405/02/01");
        Check(result.ReportHeader.ConfirmNumber is null && result.ReportHeader.InputMinutesNumber is null
            && result.ReportData.PreviousValues.MinutesNumber == 0
            && result.ReportData.CurrentValues.MinutesNumber == 0
            && result.ReportData.ReturnValues.MinutesNumber == 0,
            $"Repeated {(isPartial ? "partial" : "full")} previews need no database connection or allocated number");
    }
}

using var connection = new RecordingConnection();
using var transaction = connection.BeginTransaction();
var service = new ReturnBillMinutesCounterCommandService(connection, transaction);
connection.Number = 123;
Check(await service.GetNext() == 123, "Allocator returns the database-generated number");
Check(ReferenceEquals(connection.ExecutedTransaction, transaction),
    "Allocation uses the caller's transaction");
Check(!connection.Committed, "Allocator leaves transaction commit to the caller");
using (var otherConnection = new RecordingConnection())
{
    try
    {
        _ = new ReturnBillMinutesCounterCommandService(otherConnection, transaction);
        throw new Exception("A transaction from another connection must be rejected");
    }
    catch (ArgumentException)
    {
        Check(true, "Allocator rejects a transaction from a different connection");
    }
}
connection.Number = null;
try
{
    await service.GetNext();
    throw new Exception("Missing counter must fail");
}
catch (InvalidOperationException exception) when (exception.Message.Contains("مقداردهی"))
{
    Check(true, "Missing counter rejects allocation instead of returning zero");
}
if (args.Contains("--sql-integration"))
    await SqlIntegrationChecks.Run(Check);
else
    Console.WriteLine("SQL Server concurrency/rollback checks were not requested.");
Console.WriteLine($"{checks} checks passed.");

class Stub : DispatchProxy
{
    public Func<MethodInfo, object?[], object?> Callback = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Callback(method!, args!);
    public static object For(Type type, Func<MethodInfo, object?[], object?> callback)
    {
        var proxy = Create(type, typeof(Stub));
        ((Stub)proxy).Callback = callback;
        return proxy;
    }
}

sealed class RecordingConnection : DbConnection
{
    public int? Number { get; set; }
    public DbTransaction? ExecutedTransaction;
    public bool Committed;
    [AllowNull]
    public override string ConnectionString { get; set; } = "";
    public override string Database => "Test";
    public override string DataSource => "Test";
    public override string ServerVersion => "1";
    public override ConnectionState State => ConnectionState.Open;
    public override void ChangeDatabase(string databaseName) => throw new NotSupportedException();
    public override void Close() { }
    public override void Open() { }
    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => new RecordingTransaction(this);
    protected override DbCommand CreateDbCommand() => new RecordingCommand(this);
}

sealed class RecordingTransaction(RecordingConnection connection) : DbTransaction
{
    public override IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;
    protected override DbConnection DbConnection => connection;
    public override void Commit() => connection.Committed = true;
    public override void Rollback() { }
}

sealed class RecordingCommand(RecordingConnection connection) : DbCommand
{
    private readonly SqlCommand parameters = new();
    [AllowNull]
    public override string CommandText { get; set; } = "";
    public override int CommandTimeout { get; set; }
    public override CommandType CommandType { get; set; }
    public override bool DesignTimeVisible { get; set; }
    public override UpdateRowSource UpdatedRowSource { get; set; }
    protected override DbConnection? DbConnection { get; set; } = connection;
    protected override DbTransaction? DbTransaction { get; set; }
    protected override DbParameterCollection DbParameterCollection => parameters.Parameters;
    protected override DbParameter CreateDbParameter() => new SqlParameter();
    public override void Cancel() { }
    public override int ExecuteNonQuery() => throw new NotSupportedException();
    public override object? ExecuteScalar() => throw new NotSupportedException();
    public override void Prepare() { }
    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        connection.ExecutedTransaction = DbTransaction;
        var table = new DataTable();
        table.Columns.Add("LastNumber", typeof(int));
        if (connection.Number.HasValue) table.Rows.Add(connection.Number.Value);
        return table.CreateDataReader();
    }
}

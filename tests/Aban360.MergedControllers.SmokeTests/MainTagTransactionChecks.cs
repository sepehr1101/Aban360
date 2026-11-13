using System.Collections;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Aban360.ReportPool.Application.Features.Tagging.Services;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Microsoft.Extensions.Configuration;

static class MainTagTransactionChecks
{
    public static async Task<int> Run()
    {
        int checks = 0;
        void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); checks++; }
        var implementation = typeof(IMainTagGroupService).Assembly.GetTypes().Single(t => t.IsClass && typeof(IMainTagGroupService).IsAssignableFrom(t));
        var constructor = implementation.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single();
        IMainTagGroupService Service(SqlRecording state, Func<Task<IEnumerable<TagGroupDto>>>? query = null)
        {
            object Proxy(Type contract) => ScopeDependencyProxy.For(contract, (method, args) =>
            {
                Check(method.Name == "GetByMainTagGroupId" && (int)args[0]! == 7, "Delete must query related groups using its actual main group ID");
                state.Events.Add("RelatedQuery");
                return query?.Invoke() ?? Task.FromResult<IEnumerable<TagGroupDto>>(new[] { new TagGroupDto { Id = 11 }, new TagGroupDto { Id = 22 } });
            });
            var parameters = constructor.GetParameters();
            return (IMainTagGroupService)constructor.Invoke(parameters.Select(p =>
                p.ParameterType == typeof(IConfiguration) ? (object)new ConfigurationBuilder().Build()
                : p.ParameterType == typeof(Func<IDbConnection>) ? (Func<IDbConnection>)(() => { state.FactoryCalls++; return state.Connection; })
                : Proxy(p.ParameterType)).ToArray());
        }
        Task Execute(IMainTagGroupService service, string action) => action switch
        {
            "create" => service.CreateAsync(new MainTagGroupInsertInputDto { Title = "New title" }),
            "update" => service.UpdateAsync(new MainTagGroupUpdateDto(7, "Changed title")),
            _ => service.DeleteAsync(7)
        };
        foreach (var action in new[] { "create", "update", "delete" })
        {
            int steps = action == "delete" ? 3 : 1;
            var cases = new List<(string Mode, int Step)> { ("success", 0), ("open", 0), ("open-failure", 0), ("begin-failure", 0), ("negative", 1), ("commit-failure", 0) };
            for (int step = 1; step <= steps; step++) foreach (var mode in new[] { "failure", "cancel", "gate" }) cases.Add((mode, step));
            foreach (var test in cases)
            {
                var state = new SqlRecording { Mode = test.Mode, At = test.Step };
                var task = Execute(Service(state), action);
                if (test.Mode == "gate")
                {
                    Check(!task.IsCompleted && !state.Committed && state.Commands.Count == test.Step, action + " committed or advanced before SQL completion");
                    state.Gate.SetResult();
                }
                Exception? error = null; try { await task; } catch (Exception ex) { error = ex; }
                bool success = test.Mode is "success" or "open" or "gate";
                Check(success ? error is null && state.Committed && state.Commands.Count == steps : error is not null && !state.Committed,
                    action + "/" + test.Mode + " changed transaction success/failure behavior: " + error);
                if (test.Mode is "failure" or "cancel" or "negative") Check(state.Commands.Count == test.Step, "SQL failure/cancellation continued to a later command");
                if (test.Mode == "cancel") Check(error is OperationCanceledException, "SQL cancellation must propagate");
                Check(state.ConnectionDisposed && (test.Mode is "open-failure" or "begin-failure" || state.TransactionDisposed), "Connection/transaction must be disposed on every exit");
                Check(state.FactoryCalls == 1 && state.Events.Count(e => e == "Open") == (test.Mode == "open" ? 0 : 1), "Each operation must use one connection and preserve the already-open path");
                if (test.Mode != "open-failure") Check(state.Isolation == IsolationLevel.ReadUncommitted, "Existing transaction isolation changed");
                Check(state.Commands.All(c => c.SameTransaction), "All SQL commands must participate in the same transaction");
                if (!success) continue;
                Check(state.Events.IndexOf("Commit") > state.Events.FindLastIndex(e => e.StartsWith("Execute"))
                    && state.Events.IndexOf("DisposeTransaction") > state.Events.IndexOf("Commit"), "Commit and disposal ordering changed");
                var first = state.Commands[0];
                Check(first.Sql.Contains("CustomerWarehouse.dbo.MainTagGroup"), "First command must address the main group");
                if (action is "create" or "update") Check(Equals(first.Parameters["Title"], action == "create" ? "New title" : "Changed title"), "SQL title parameter changed");
                if (action != "create") Check(Equals(first.Parameters["Id"], 7), "SQL ID parameter changed");
                if (action == "create") Check(first.Parameters["CreateDateTime"] is DateTime created && Math.Abs((created - DateTime.Now).TotalSeconds) < 60, "Create must retain its current timestamp");
                if (action == "delete")
                {
                    Check(state.Events[0] == "RelatedQuery" && state.Commands[1].Sql.Contains("CustomerWarehouse.dbo.TagGroups")
                        && state.Commands[2].Sql.Contains("CustomerWarehouse.dbo.Tags"), "Related query/main group/group/tag deletion order changed");
                    Check(Equals(state.Commands[1].Parameters["Id"], 7), "Child groups must be removed using the main group ID");
                    Check(state.Commands[2].Parameters.Values.Cast<object>().Select(Convert.ToInt32).SequenceEqual(new[] { 11, 22 }), "Dapper IN parameters must use the related group IDs");
                }
            }
        }
        var queryGate = new TaskCompletionSource<IEnumerable<TagGroupDto>>(TaskCreationOptions.RunContinuationsAsynchronously);
        var delayed = new SqlRecording(); var pending = Service(delayed, () => queryGate.Task).DeleteAsync(7);
        Check(!pending.IsCompleted && delayed.FactoryCalls == 0, "Delete must await related groups before opening its transaction");
        queryGate.SetException(new InvalidOperationException("related-query-failed"));
        try { await pending; throw new Exception("Related query failure ignored"); } catch (InvalidOperationException ex) when (ex.Message == "related-query-failed") { checks++; }
        Check(delayed.FactoryCalls == 0 && !delayed.Committed, "Related query failure must not open/commit a transaction");
        Console.WriteLine($"PASS: {checks} MainTagGroup checks using actual Dapper command services and an isolated recording provider; SQL parameters, isolation, ordering, awaits, failures, cancellation and disposal.");
        return checks;
    }
}

sealed class SqlRecording
{
    public string Mode = "success";
    public int At, FactoryCalls;
    public bool Committed, ConnectionDisposed, TransactionDisposed;
    public IsolationLevel Isolation;
    public readonly List<string> Events = [];
    public readonly List<(string Sql, Dictionary<string, object> Parameters, bool SameTransaction)> Commands = [];
    public readonly TaskCompletionSource Gate = new(TaskCreationOptions.RunContinuationsAsynchronously);
    public RecordingConnection Connection;
    public RecordingTransaction? Transaction;
    public SqlRecording() { Connection = new RecordingConnection(this); }
    public async Task<int> Execute(RecordingCommand command)
    {
        Commands.Add((command.CommandText, command.Parameters.Cast<DbParameter>().ToDictionary(p => p.ParameterName.TrimStart('@'), p => p.Value!), ReferenceEquals(command.Transaction, Transaction)));
        int step = Commands.Count; Events.Add("Execute" + step);
        if (step == At && Mode == "gate") await Gate.Task;
        if (step == At && Mode == "failure") throw new InvalidOperationException("sql-step-failed");
        if (step == At && Mode == "cancel") throw new OperationCanceledException();
        return step == At && Mode == "negative" ? -1 : 1;
    }
}

sealed class RecordingConnection(SqlRecording state) : DbConnection
{
    private bool open;
    public override string ConnectionString { get; set; } = "isolated";
    public override string Database => "isolated";
    public override string DataSource => "isolated";
    public override string ServerVersion => "1";
    public override ConnectionState State => open || state.Mode == "open" ? ConnectionState.Open : ConnectionState.Closed;
    public override void Open() { state.Events.Add("Open"); if (state.Mode == "open-failure") throw new InvalidOperationException("open-failed"); open = true; }
    public override void Close() => open = false;
    public override void ChangeDatabase(string databaseName) => throw new NotSupportedException();
    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        state.Isolation = isolationLevel; state.Events.Add("Begin");
        if (state.Mode == "begin-failure") throw new InvalidOperationException("begin-failed");
        return state.Transaction = new RecordingTransaction(state, this);
    }
    protected override DbCommand CreateDbCommand() => new RecordingCommand(state, this);
    protected override void Dispose(bool disposing) { if (disposing) { state.ConnectionDisposed = true; state.Events.Add("DisposeConnection"); } base.Dispose(disposing); }
}

sealed class RecordingTransaction(SqlRecording state, DbConnection connection) : DbTransaction
{
    public override IsolationLevel IsolationLevel => state.Isolation;
    protected override DbConnection DbConnection => connection;
    public override void Commit() { if (state.Mode == "commit-failure") throw new InvalidOperationException("commit-failed"); state.Committed = true; state.Events.Add("Commit"); }
    public override void Rollback() => state.Events.Add("Rollback");
    protected override void Dispose(bool disposing) { if (disposing) { state.TransactionDisposed = true; state.Events.Add("DisposeTransaction"); } base.Dispose(disposing); }
}

sealed class RecordingCommand(SqlRecording state, DbConnection connection) : DbCommand
{
    public override string CommandText { get; set; } = "";
    public override int CommandTimeout { get; set; }
    public override CommandType CommandType { get; set; }
    public override bool DesignTimeVisible { get; set; }
    public override UpdateRowSource UpdatedRowSource { get; set; }
    protected override DbConnection? DbConnection { get; set; } = connection;
    protected override DbTransaction? DbTransaction { get; set; }
    protected override DbParameterCollection DbParameterCollection { get; } = new RecordingParameters();
    public override void Cancel() { }
    public override void Prepare() { }
    public override int ExecuteNonQuery() => throw new Exception("Dapper must execute asynchronously");
    public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken) => state.Execute(this);
    public override object? ExecuteScalar() => throw new NotSupportedException();
    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => throw new NotSupportedException();
    protected override DbParameter CreateDbParameter() => new RecordingParameter();
}

sealed class RecordingParameter : DbParameter
{
    public override DbType DbType { get; set; }
    public override ParameterDirection Direction { get; set; } = ParameterDirection.Input;
    public override bool IsNullable { get; set; }
    public override string ParameterName { get; set; } = "";
    public override string SourceColumn { get; set; } = "";
    public override object? Value { get; set; }
    public override bool SourceColumnNullMapping { get; set; }
    public override int Size { get; set; }
    public override void ResetDbType() { }
}

sealed class RecordingParameters : DbParameterCollection
{
    private readonly List<DbParameter> items = [];
    public override int Count => items.Count;
    public override object SyncRoot => this;
    public override int Add(object value) { items.Add((DbParameter)value); return items.Count - 1; }
    public override void AddRange(Array values) { foreach (var value in values) Add(value!); }
    public override void Clear() => items.Clear();
    public override bool Contains(object value) => items.Contains((DbParameter)value);
    public override bool Contains(string value) => IndexOf(value) >= 0;
    public override void CopyTo(Array array, int index) => ((ICollection)items).CopyTo(array, index);
    public override IEnumerator GetEnumerator() => items.GetEnumerator();
    public override int IndexOf(object value) => items.IndexOf((DbParameter)value);
    public override int IndexOf(string parameterName) => items.FindIndex(p => p.ParameterName == parameterName);
    public override void Insert(int index, object value) => items.Insert(index, (DbParameter)value);
    public override void Remove(object value) => items.Remove((DbParameter)value);
    public override void RemoveAt(int index) => items.RemoveAt(index);
    public override void RemoveAt(string parameterName) => RemoveAt(IndexOf(parameterName));
    protected override DbParameter GetParameter(int index) => items[index];
    protected override DbParameter GetParameter(string parameterName) => items[IndexOf(parameterName)];
    protected override void SetParameter(int index, DbParameter value) => items[index] = value;
    protected override void SetParameter(string parameterName, DbParameter value) { int index = IndexOf(parameterName); if (index < 0) items.Add(value); else items[index] = value; }
}

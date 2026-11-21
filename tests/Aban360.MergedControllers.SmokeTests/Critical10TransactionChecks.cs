using System.Data;
using System.Reflection;
using System.Text.Json;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using Microsoft.Extensions.Configuration;

// Production code retains its concrete SqlConnection base. Test-only copies use a recording
// connection; CriticalServiceStructure checks every token of those copies against the sources.
internal abstract class CriticalRecordingConnectionBase
{
    internal static readonly AsyncLocal<SqlRecording?> Current = new();
    private readonly SqlRecording state;
    protected CriticalRecordingConnectionBase(IConfiguration configuration) => state=Current.Value??throw new InvalidOperationException("Missing isolated SQL recording");
    protected IDbConnection _sqlReportConnection { get { state.FactoryCalls++;return state.Connection; } }
}

static class Critical10TransactionChecks
{
    public static async Task<int> Run(IMapper mapper)
    {
        int checks=0,scenarios=0,methods=0;
        void Check(bool condition,string message){if(!condition)throw new InvalidOperationException(message);checks++;}
        var operations=ApplicationServiceBaseline.CriticalOperations;
        foreach(var operation in operations)
        {
            var old=typeof(Critical10TransactionChecks).Assembly.GetType(operation.Original.Replace("BeforeServiceCritical10.","SqlBeforeCritical10."));
            if(old is null)continue;
            var before=old.GetMethod("Handle")!;
            var current=typeof(Critical10TransactionChecks).Assembly.GetType("SqlAfterCritical10."+operation.Service.Replace(".I"+operation.Feature+"Service","."+operation.Feature+"Service"),true)!;
            var after=current.GetMethod(operation.Method)!;
            bool readsLatestId=operation.Feature=="S"&&operation.Method=="CreateAsync";
            int steps=operation.Feature=="AssessmentOff"?1:readsLatestId?3:2;
            methods++;
            var modes=new List<(string Mode,int At)>{("success",0),("open",0),("open-failure",0),("begin-failure",0),("commit-failure",0)};
            for(int step=1;step<=steps;step++)foreach(string mode in new[]{"failure","cancel","negative","gate"})
                if(!(readsLatestId&&step==1&&mode=="negative"))modes.Add((mode,step));
            foreach(var test in modes)
            {
                var a=await Execute(old,before,test.Mode,test.At);
                var b=await Execute(current,after,test.Mode,test.At);
                string label=operation.Feature+"."+operation.Method+"/"+test.Mode+test.At;
                Check(a.Error==b.Error&&a.Commands==b.Commands&&a.State.Events.SequenceEqual(b.State.Events),"SQL before/after mismatch: "+label+"\n"+a.Commands+"\n"+b.Commands+"\n"+a.Error+"\n"+b.Error);
                Check(a.Dependencies==b.Dependencies,"Validation/query order or arguments changed: "+label);
                bool success=test.Mode is "success" or "open" or "gate";
                Check(success?b.Error is null&&b.State.Committed&&b.State.Commands.Count==steps:b.Error is not null&&!b.State.Committed,"Commit/failure behavior changed: "+label+" "+b.Error);
                Check(b.State.ConnectionDisposed&&(test.Mode is "open-failure" or "begin-failure"||b.State.TransactionDisposed),"Resources not disposed: "+label);
                Check(b.State.FactoryCalls==1,"Connection count changed: "+label);
                if(test.Mode!="open-failure")Check(b.State.Isolation==IsolationLevel.ReadUncommitted,"Isolation changed: "+label);
                Check(b.State.Commands.All(c=>c.SameTransaction),"Commands use different transactions: "+label);
                if(test.At>0&&test.Mode!="gate")Check(b.State.Commands.Count==test.At,"Continued SQL after failure: "+label);
                if(success&&operation.Feature!="AssessmentOff")
                {
                    Check(b.State.Commands[steps-1].Sql.Contains("CustomerWarehouse.dbo.OpLog")&&!b.State.Commands[steps-2].Sql.Contains("CustomerWarehouse.dbo.OpLog"),"Data/log order changed: "+label);
                    Check(b.State.Events.IndexOf("Commit")>b.State.Events.IndexOf("Execute"+steps),"Commit preceded log: "+label);
                    Check(b.State.Commands[steps-1].Parameters.ContainsKey("UserId")&&b.State.Commands[steps-1].Parameters.ContainsKey("Description"),"Audit identity/text missing: "+label);
                    if(readsLatestId)Check(b.State.Commands[0].Sql.TrimStart().StartsWith("Select top 1 Id",StringComparison.OrdinalIgnoreCase),"Latest-ID query ordering changed");
                }
                scenarios++;
            }
        }
        Check(methods==14,"Not all 14 tariff transactions exercised");
        Console.WriteLine($"PASS: {checks} tariff transaction checks; {methods} transaction methods/{scenarios} before/after cases; real Dapper command services with isolated recording connections; SQL/parameters, audit ordering, commit, failures, cancellation and disposal.");
        return checks;

        async Task<(SqlRecording State,string? Error,string Commands,string Dependencies)> Execute(Type type,MethodInfo method,string mode,int at)
        {
            var state=new SqlRecording{Mode=mode,At=at};
            CriticalRecordingConnectionBase.Current.Value=state;
            var probe=new ServiceProbe(mapper,CancellationToken.None){CriticalContext=true};
            var task=ServiceConsolidationChecks.Execute(type,method,probe);
            if(mode=="gate")
            {
                Check(!task.IsCompleted&&!state.Committed&&state.Commands.Count==at,"SQL task advanced before command completion: "+type.Name+"."+method.Name);
                state.Gate.SetResult();
            }
            var outcome=await task;
            CriticalRecordingConnectionBase.Current.Value=null;
            bool generatedOffId=type.Name=="AssessmentOffInsertHandler"||type.Name=="AssessmentOffService"&&method.Name=="CreateAsync";
            object? Value(string name,object? value)
            {
                if(generatedOffId&&name=="Id")
                {
                    Check(value is Guid id&&id!=Guid.Empty,"AssessmentOff must generate a nonempty identifier");
                    return "new-assessment-off-id";
                }
                return value is DateTime dt&&Math.Abs((DateTime.Now-dt).TotalSeconds)<60?"current-audit-timestamp":value;
            }
            var commands=JsonSerializer.Serialize(state.Commands.Select(c=>new{c.Sql,Parameters=c.Parameters.OrderBy(p=>p.Key).ToDictionary(p=>p.Key,p=>Value(p.Key,p.Value)),c.SameTransaction}));
            return(state,outcome.Error,commands,JsonSerializer.Serialize(outcome.Calls));
        }
    }
}

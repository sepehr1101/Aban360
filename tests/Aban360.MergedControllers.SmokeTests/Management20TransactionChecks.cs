using System.Data;
using System.Reflection;
using System.Text.Json;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using Microsoft.Extensions.Configuration;

internal abstract class ManagementRecordingConnectionBase : Aban360.Common.Db.Dapper.AbstractBaseConnection
{
    internal static readonly AsyncLocal<AuxiliarySqlState?> Current=new();
    private readonly AuxiliarySqlState state;
    protected ManagementRecordingConnectionBase(IConfiguration configuration) : base(configuration)=>state=Current.Value??throw new InvalidOperationException("Missing isolated management SQL state");
    protected new IDbConnection _sqlConnection { get { state.Main.FactoryCalls++;return state.Main.Connection; } }
    protected new IDbConnection _sqlReportConnection { get { state.Report.FactoryCalls++;return state.Report.Mode=="fresh-connections"?new RecordingConnection(state.Report):state.Report.Connection; } }
}

static class Management20TransactionChecks
{
    public static async Task<int> Run(IMapper mapper)
    {
        int checks=0,scenarios=0,methods=0;
        void Check(bool ok,string message){if(!ok)throw new InvalidOperationException(message);checks++;}
        foreach(var op in ApplicationServiceBaseline.ManagementOperations)
        {
            var old=typeof(Management20TransactionChecks).Assembly.GetType(op.Original.Replace("BeforeServiceManagement20.","SqlBeforeManagement20."));
            if(old is null)continue;
            int separator=op.Service.LastIndexOf('.');
            var current=typeof(Management20TransactionChecks).Assembly.GetType("SqlAfterManagement20."+op.Service[..(separator+1)]+op.Service[(separator+2)..],true)!;
            var before=old.GetMethod("Handle")!;var after=current.GetMethod(op.Method)!;methods++;
            var initial=await Execute(current,after,"success",0);
            Check(initial.Outcome.Error is null&&initial.State.Report.Committed&&initial.State.Report.Commands.Count>0,"Management SQL success fixture did not reach commit: "+op.Feature+"."+op.Method+" "+initial.Outcome.Error);
            int steps=initial.State.Report.Commands.Count;
            var modes=new List<(string Mode,int At)>{("success",0),("open",0),("open-failure",0),("begin-failure",0),("commit-failure",0)};
            for(int step=1;step<=steps;step++)foreach(string mode in new[]{"failure","cancel","negative","gate"})modes.Add((mode,step));
            foreach(var mode in modes)
            {
                var a=await Execute(old,before,mode.Mode,mode.At);var b=await Execute(current,after,mode.Mode,mode.At);
                string label=op.Feature+"."+op.Method+"/"+mode.Mode+mode.At;
                Check(a.Commands==b.Commands&&a.State.Events.SequenceEqual(b.State.Events)&&Management20Checks.Normalize(a.Outcome,op.Feature)==Management20Checks.Normalize(b.Outcome,op.Feature),"Management SQL mismatch: "+label+"\n"+a.Commands+"\n"+b.Commands+"\n"+JsonSerializer.Serialize(a.Outcome)+"\n"+JsonSerializer.Serialize(b.Outcome));
                Check(a.State.Report.Committed==b.State.Report.Committed&&a.State.Report.ConnectionDisposed==b.State.Report.ConnectionDisposed&&a.State.Report.TransactionDisposed==b.State.Report.TransactionDisposed&&a.State.Report.FactoryCalls==b.State.Report.FactoryCalls,"Management transaction lifecycle changed: "+label);
                Check(b.State.Main.FactoryCalls==0&&b.State.Report.Commands.All(c=>c.SameTransaction),"Management SQL database/transaction changed: "+label);
                if(mode.Mode is "failure" or "cancel")Check(b.Outcome.Error is not null&&!b.State.Report.Committed&&b.State.Report.Commands.Count==mode.At,"SQL continued or committed after failure: "+label);
                if(mode.Mode is "success" or "open" or "gate")
                    Check(b.Outcome.Error is null&&b.State.Report.Committed&&b.State.Report.Commands.Count==steps&&b.State.Report.Isolation==IsolationLevel.ReadUncommitted,"Management SQL outcome/isolation changed: "+label);
                scenarios++;
            }
            if(op.Method=="DuplicateInsertAsync"||op.Feature=="MeterChange")
            {
                var a=await Execute(old,before,"preview",0);var b=await Execute(current,after,"preview",0);
                Check(a.Outcome.Error is null&&b.Outcome.Error is null&&a.State.Report.FactoryCalls==0&&b.State.Report.FactoryCalls==0&&Management20Checks.Normalize(a.Outcome,op.Feature)==Management20Checks.Normalize(b.Outcome,op.Feature),"Preview performed writes or changed result");scenarios++;
            }
            if(op.Feature=="SubscriptionAssignment"&&op.Method=="UpdateAsync")
            {
                var a=await Execute(old,before,"fresh-connections",0);var b=await Execute(current,after,"fresh-connections",0);
                Check(a.Outcome.Error==b.Outcome.Error&&b.Outcome.Error?.Contains("recording-transaction-connection-mismatch")==true&&!b.State.Report.Committed&&a.State.Report.FactoryCalls==3&&b.State.Report.FactoryCalls==3,"Legacy subscription connection mismatch changed or escaped detection");scenarios++;
                Console.WriteLine("KNOWN LEGACY DEFECT: SubscriptionAssignment update requests three fresh report connections for one transaction; strict connection fixture rejects both versions. Shared-connection SQL comparisons do not certify its real database path.");
            }

            async Task<(AuxiliarySqlState State,ServiceConsolidationChecks.Outcome Outcome,string Commands)> Execute(Type type,MethodInfo method,string mode,int at)
            {
                var state=new AuxiliarySqlState();state.Report.Mode=mode;state.Report.At=at;ManagementRecordingConnectionBase.Current.Value=state;
                state.Report.RequireSameConnection=mode=="fresh-connections";
                var probe=Management20Checks.Probe(mapper,CancellationToken.None,op.Feature,op.Method);
                if(mode=="preview")
                {
                    var original=probe.ArgumentFactory!;probe.ArgumentFactory=p=>{var value=original(p);value?.GetType().GetProperty("IsConfirm")?.SetValue(value,false);return value;};
                }
                var pending=ServiceConsolidationChecks.Execute(type,method,probe);
                if(mode=="gate")
                {
                    Check(!pending.IsCompleted&&!state.Report.Committed&&state.Report.Commands.Count==at,"Management SQL await boundary changed: "+op.Feature+"."+op.Method);state.Report.Gate.SetResult();
                }
                var outcome=await pending;ManagementRecordingConnectionBase.Current.Value=null;
                var commands=JsonSerializer.Serialize(state.Report.Commands.Select(c=>new{c.Sql,Parameters=c.Parameters.OrderBy(p=>p.Key).ToDictionary(p=>p.Key,p=>p.Value),c.SameTransaction}));
                // Serialized personnel records contain generated identity/time values too.
                return(state,outcome,Management20Checks.Normalize(commands,op.Feature));
            }
        }
        Check(methods==22,"Management SQL coverage shrank");
        Console.WriteLine($"PASS: {checks} management SQL checks; {methods} operations/{scenarios} before/after scenarios; real Dapper commands with recording connections, exact SQL, parameters, audit order, failure/cancellation/disposal and preview behavior.");
        return checks;
    }
}

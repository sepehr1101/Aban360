using System.Data;
using System.Reflection;
using System.Text.Json;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using Microsoft.Extensions.Configuration;

internal abstract class AuxiliaryRecordingConnectionBase
{
    internal static readonly AsyncLocal<AuxiliarySqlState?> Current=new();
    private readonly AuxiliarySqlState state;
    protected AuxiliaryRecordingConnectionBase(IConfiguration configuration)=>state=Current.Value??throw new InvalidOperationException("Missing isolated auxiliary SQL state");
    protected IDbConnection _sqlConnection { get { state.Main.FactoryCalls++;return state.Main.Connection; } }
    protected IDbConnection _sqlReportConnection { get { state.Report.FactoryCalls++;return state.Report.Connection; } }
}
sealed class AuxiliarySqlState
{
    public readonly SqlRecording Main=new(),Report=new();
    public readonly List<string> Events=[];
    public AuxiliarySqlState(){Main.Observer=e=>Events.Add("main:"+e);Report.Observer=e=>Events.Add("report:"+e);}
}

static class Auxiliary10TransactionChecks
{
    public static async Task<int> Run(IMapper mapper)
    {
        int checks=0,scenarios=0,methods=0;
        void Check(bool ok,string message){if(!ok)throw new InvalidOperationException(message);checks++;}
        foreach(var operation in ApplicationServiceBaseline.AuxiliaryOperations)
        {
            var old=typeof(Auxiliary10TransactionChecks).Assembly.GetType(operation.Original.Replace("BeforeServiceAuxiliary10.","SqlBeforeAuxiliary10."));
            if(old is null)continue;
            var before=old.GetMethod("Handle")!;
            int separator=operation.Service.LastIndexOf('.');
            var current=typeof(Auxiliary10TransactionChecks).Assembly.GetType("SqlAfterAuxiliary10."+operation.Service[..(separator+1)]+operation.Service[(separator+2)..],true)!;
            var after=current.GetMethod(operation.Method)!;methods++;
            bool apk=operation.Feature=="MeterApkFile";
            foreach(bool inactive in apk&&operation.Method=="DeleteAsync"?new[]{false,true}:new[]{false})
            {
                int mainSteps=apk?(operation.Method=="DeleteAsync"?(inactive?1:3):2):0;
                int total=mainSteps+1;
                var modes=new List<(string Mode,int At)>{("success",0),("open",0),("open-failure",0),("begin-failure",0),("commit-failure",0)};
                if(apk)modes.AddRange(new[]{("report-open-failure",0),("report-begin-failure",0),("report-commit-failure",0)});
                for(int step=1;step<=total;step++)foreach(string mode in new[]{"failure","cancel","negative","gate"})modes.Add((mode,step));
                foreach(var mode in modes)
                {
                    var a=await Execute(old,before,mode.Mode,mode.At);
                    var b=await Execute(current,after,mode.Mode,mode.At);
                    string label=operation.Feature+"."+operation.Method+"/"+mode.Mode+mode.At+"/inactive="+inactive;
                    Check(a.Error==b.Error&&a.Commands==b.Commands&&a.State.Events.SequenceEqual(b.State.Events)&&a.Dependencies==b.Dependencies,"Auxiliary SQL before/after mismatch: "+label+"\n"+a.Commands+"\n"+b.Commands+"\n"+a.Error+"\n"+b.Error);
                    foreach(var pair in new[]{(a.State.Main,b.State.Main),(a.State.Report,b.State.Report)})
                        Check(pair.Item1.Committed==pair.Item2.Committed&&pair.Item1.ConnectionDisposed==pair.Item2.ConnectionDisposed&&pair.Item1.TransactionDisposed==pair.Item2.TransactionDisposed&&pair.Item1.FactoryCalls==pair.Item2.FactoryCalls,"Auxiliary commit/disposal behavior changed: "+label);
                    bool success=mode.Mode is "success" or "open" or "gate";
                    Check(success?b.Error is null&&b.State.Report.Committed&&(!apk||b.State.Main.Committed):b.Error is not null,"Unexpected auxiliary SQL outcome: "+label+" "+b.Error);
                    Check(b.State.Report.FactoryCalls==1&&b.State.Main.FactoryCalls==(apk?1:0),"Connection selection changed: "+label);
                    Check(b.State.Report.Commands.Concat(b.State.Main.Commands).All(c=>c.SameTransaction),"Command lost its transaction: "+label);
                    if(mode.Mode is "failure" or "cancel" or "negative")
                        Check(!b.State.Report.Committed&&!b.State.Main.Committed&&b.State.Main.Commands.Count+b.State.Report.Commands.Count==mode.At,"Continued SQL or committed after failed command: "+label);
                    if(success)
                    {
                        Check(b.State.Report.Commands.Count==1&&b.State.Main.Commands.Count==mainSteps,"SQL count changed: "+label);
                        Check(b.State.Report.Isolation==(apk?IsolationLevel.ReadCommitted:IsolationLevel.ReadUncommitted),"Report isolation changed");
                        if(apk)
                        {
                            Check(b.State.Main.Isolation==IsolationLevel.ReadUncommitted,"Main isolation changed");
                            Check(b.State.Events.IndexOf("main:Commit")>b.State.Events.IndexOf("report:Execute1")&&b.State.Events.IndexOf("report:Commit")>b.State.Events.IndexOf("main:Commit"),"APK audit/commit order changed");
                        }
                        else VerifySms(operation.Feature,operation.Method,b.State.Report.Commands.Single());
                    }
                    scenarios++;
                }

                async Task<(AuxiliarySqlState State,string? Error,string Commands,string Dependencies)> Execute(Type type,MethodInfo method,string mode,int at)
                {
                    var state=new AuxiliarySqlState();
                    SqlRecording primary=apk?state.Main:state.Report;
                    if(mode.StartsWith("report-"))state.Report.Mode=mode[7..];
                    else if(at>0)
                    {
                        var target=apk&&at<=mainSteps?state.Main:state.Report;target.Mode=mode;target.At=apk&&at>mainSteps?at-mainSteps:at;
                    }
                    else primary.Mode=mode;
                    if(mode=="open")state.Report.Mode="open";
                    AuxiliaryRecordingConnectionBase.Current.Value=state;
                    var probe=Auxiliary10Checks.Probe(mapper,CancellationToken.None,operation.Feature,operation.Method);
                    if(inactive)probe.Mode="inactive";
                    var task=ServiceConsolidationChecks.Execute(type,method,probe);
                    if(mode=="gate")
                    {
                        Check(!task.IsCompleted&&!state.Main.Committed&&!state.Report.Committed&&state.Main.Commands.Count+state.Report.Commands.Count==at,"SQL await ordering changed: "+type.Name+"."+method.Name);
                        (apk&&at<=mainSteps?state.Main:state.Report).Gate.SetResult();
                    }
                    var outcome=await task;AuxiliaryRecordingConnectionBase.Current.Value=null;
                    object? Value(object? value)=>value is DateTime dt&&Math.Abs((DateTime.Now-dt).TotalSeconds)<60?"current-audit-time":value;
                    object Commands(SqlRecording recording)=>recording.Commands.Select(c=>new{c.Sql,Parameters=c.Parameters.OrderBy(p=>p.Key).ToDictionary(p=>p.Key,p=>Value(p.Value)),c.SameTransaction});
                    return(state,outcome.Error,JsonSerializer.Serialize(new{Main=Commands(state.Main),Report=Commands(state.Report)}),JsonSerializer.Serialize(outcome.Calls));
                }
            }
        }
        Check(methods==9,"Auxiliary SQL coverage shrank");
        Console.WriteLine($"PASS: {checks} auxiliary SQL checks; {methods} operations/{scenarios} before/after scenarios; real Dapper command services, isolated connections, exact parameters and commit/failure/cancellation ordering.");
        return checks;

        void VerifySms(string feature,string method,(string Sql,Dictionary<string,object> Parameters,bool SameTransaction) command)
        {
            object? Get(string name)=>command.Parameters.FirstOrDefault(p=>p.Key.Equals(name,StringComparison.OrdinalIgnoreCase)).Value;
            if(feature=="SmsStateTemplate"&&method=="CreateAsync")
            {
                Check(Equals(Convert.ToInt32(Get("GroupId")),11)&&Equals(Convert.ToInt32(Get("SmsTypeId")),22)&&Equals(Convert.ToInt32(Get("StepOrder")),3)&&Equals(Convert.ToInt32(Get("DueDay")),7),"Template identity/step/delay changed");
                Check(Equals(Get("SmsText"),"متن آزمایشی {BillId}؛ فقط برای مقایسه")&&Equals(Get("InsertBy"),Guid.Parse("22222222-2222-2222-2222-222222222222")),"SMS text or creator changed");
            }
            if(feature=="SmsFlow"&&method=="CreateAsync")
                Check(Convert.ToInt32(Get("FirstFlowId"))==71&&Convert.ToInt32(Get("SmsCount"))==4&&Convert.ToInt32(Get("SmsTemplateId"))==42&&Get("DueDateTime") is DateTime due&&due==new DateTime(2026,11,18),"SMS schedule/count/template changed");
            Check(command.Sql.Contains("Sms",StringComparison.Ordinal),"SMS definition SQL changed table");
        }
    }
}

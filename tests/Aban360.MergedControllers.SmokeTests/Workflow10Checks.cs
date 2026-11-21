using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Aban360.Tests.ServiceBaselines;
using Aban360.Common.Extensions;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

static class Workflow10Checks
{
    public static async Task RunFocused()
    {
        var registrations=new ServiceCollection();registrations.AddLogging();
        Aban360.UserPool.Application.Extensions.ConfigureServices.AddUserPoolApplicationInjections(registrations);
        Aban360.ClaimPool.Application.Extentions.ConfigureServices.AddClaimPoolApplicationInjections(registrations);
        Aban360.PaymentPool.Application.Extensions.ConfigureServices.AddPaymentPoolApplicationInjections(registrations);
        Aban360.MeterPool.Application.Extensions.ConfigureService.AddMeterPoolApplicationInjections(registrations);
        using var provider=registrations.BuildServiceProvider();var mapper=provider.GetRequiredService<IMapper>();
        int checks=0,scenarios=0,signatures=0,sqlCases=0,knownUncommitted=0,knownUnawaited=0;var limits=new List<string>();
        void Check(bool condition,string message){if(!condition)throw new InvalidOperationException(message);checks++;}
        var operations=ApplicationServiceBaseline.WorkflowOperations;
        Check(operations.Length==31&&operations.Select(o=>o.Service).Distinct().Count()==10,"Incomplete workflow selection");
        foreach(var group in operations.GroupBy(o=>o.Service))
        {
            var assembly=Assembly.Load(group.First().Assembly);var contract=assembly.GetType(group.Key,true)!;
            var production=assembly.GetTypes().Single(t=>t.IsClass&&!t.IsAbstract&&contract.IsAssignableFrom(t));
            var registration=registrations.Where(d=>d.ServiceType==contract).ToArray();
            Check(registration.Length==1&&registration[0].ImplementationType==production&&registration[0].Lifetime==ServiceLifetime.Scoped,"Workflow DI mismatch: "+group.Key);
            IServiceCollection local=new ServiceCollection();local.Add(registration[0]);var dependencies=Probe(mapper,CancellationToken.None,group.First().Feature,"");
            foreach(var p in production.GetConstructors().Single().GetParameters())local.AddScoped(p.ParameterType,_=>dependencies.Proxy(p.ParameterType));
            using var container=local.BuildServiceProvider(new ServiceProviderOptions{ValidateScopes=true});using var a=container.CreateScope();using var b=container.CreateScope();
            var instance=a.ServiceProvider.GetRequiredService(contract);
            Check(ReferenceEquals(instance,a.ServiceProvider.GetRequiredService(contract))&&!ReferenceEquals(instance,b.ServiceProvider.GetRequiredService(contract)),"Workflow scope changed");
            foreach(var op in group)
            {
                var original=typeof(Workflow10Checks).Assembly.GetType(op.Original,true)!;
                var oldSql=typeof(Workflow10Checks).Assembly.GetType(op.Original.Replace("BeforeServiceWorkflow10.","SqlBeforeWorkflow10."));
                int dot=op.Service.LastIndexOf('.');
                var newSql=typeof(Workflow10Checks).Assembly.GetType("SqlAfterWorkflow10."+op.Service[..(dot+1)]+op.Service[(dot+2)..]);
                var old=oldSql??original;var current=newSql??production;
                foreach(var before in old.GetMethods(BindingFlags.Instance|BindingFlags.Public|BindingFlags.DeclaredOnly).Where(m=>m.Name==op.OldMethod))
                {
                    var after=current.GetMethod(op.Method,before.GetParameters().Select(p=>p.ParameterType).ToArray())!;
                    Check(after is not null&&before.ReturnType==after.ReturnType,"Workflow signature changed: "+op.Feature+"."+op.Method);signatures++;
                    var success=await Compare("success");
                    if(success.Outcome.Error is not null){limits.Add(op.Feature+"."+op.Method+"/"+before.GetParameters().FirstOrDefault()?.ParameterType.Name+": "+success.Outcome.Error);Console.WriteLine("FIXTURE LIMIT: "+limits.Last());continue;}
                    if(success.State.Report.FactoryCalls>0)
                    {
                        Check(success.State.Report.FactoryCalls==1&&success.State.Main.FactoryCalls==0&&success.State.Report.TransactionDisposed&&success.State.Report.ConnectionDisposed,"Connection selection/disposal changed");
                        Check(success.State.Report.Commands.All(c=>c.SameTransaction),"SQL/bulk command escaped its transaction");
                        Check(success.State.Report.Isolation==(op.Feature is "ManualCalculationRequest" or "RequestRegistration"?System.Data.IsolationLevel.Serializable:System.Data.IsolationLevel.ReadUncommitted),"Transaction isolation changed");
                        if(before.GetParameters().FirstOrDefault()?.ParameterType.Name!="MeterInstallationUpdateInputDto")Check(success.State.Report.Committed,"Successful transaction did not commit");
                    }
                    if(op.Feature=="UserNavigation"&&op.Method=="GetTopbarAsync")Check(JsonNode.Parse(success.Outcome.Result)!["Level1s"]!.AsArray().Count>0,"Topbar fixture did not exercise authorized menu content");
                    if(op.Feature=="RequestRegistration"&&op.Method=="CreateAfterSaleAsync")
                    {knownUnawaited++;Console.WriteLine("VERIFIED CORRECTION: After-sale service validation is awaited; errors and cancellation prevent commit, and delayed validation blocks further work.");}
                    if(op.Feature=="UserAuthentication"&&op.Method=="FindByCredentialsAsync")
                    {
                        Check(JsonNode.Parse(success.Outcome.Result)![1]!.GetValue<bool>(),"Valid credentials were rejected");
                        Check(!JsonNode.Parse((await Compare("wrong-password")).Outcome.Result)![1]!.GetValue<bool>(),"Wrong password was accepted");
                        Check(!JsonNode.Parse((await Compare("unknown-user")).Outcome.Result)![1]!.GetValue<bool>(),"Unknown username was accepted");
                    }
                    if(op.Feature=="UserAuthentication"&&op.Method=="FindLoginAsync")Check((await Compare("wrong-code")).Outcome.Error is not null,"Wrong second-step code accepted");
                    if(op.Feature=="UserAuthentication"&&op.Method=="FindByUsernameAsync")
                    {Check((await Compare("wrong-password")).Outcome.Error is not null,"Wrong password accepted by direct lookup");Check((await Compare("unknown-user")).Outcome.Error is not null,"Unknown user accepted by direct lookup");}
                    if(op.Feature=="RequestRegistration"&&op.Method.StartsWith("Create"))
                    {var scheduled=await Compare("scheduled");Check(scheduled.Outcome.Error is null&&JsonNode.Parse(scheduled.Outcome.Result)![1] is not null&&scheduled.State.Report.Commands.Count>success.State.Report.Commands.Count,"Scheduled assessment branch was not persisted with the request");}
                    if(op.Feature=="Credit"&&op.Method=="CreateByDocumentAsync")
                    {
                        var savedCall=success.Outcome.Calls.Single(c=>c.Contains("IUploaderCommandService.Add("));var saved=JsonNode.Parse(savedCall[(savedCall.IndexOf('(')+1)..^1])!;
                        Check(saved["Amount"]!.GetValue<long>()==3500&&saved["InsertRecordCount"]!.GetValue<int>()==2&&saved["BankId"]!.GetValue<int>()==17,"Bank file count/total/bank changed");
                        foreach(string mode in new[]{"bad-total","bad-count","bad-bank","invoice-missing"})
                        {var rejected=await Compare(mode);Check(rejected.Outcome.Error is not null&&!rejected.Outcome.Calls.Any(c=>c.Contains("IUploaderCommandService.Add(")),"Invalid bank file persisted: "+mode);}
                    }
                    if(op.Feature=="Credit"&&op.Method=="CreateWithoutDocumentAsync")
                    {
                        var call=success.Outcome.Calls.Single(c=>c.Contains("ICreditCommandService.Add("));var credit=JsonNode.Parse(call[(call.IndexOf('(')+1)..^1])!;
                        Check(credit["Amount"]!.GetValue<long>()==1200&&credit["Uploader"]!["Amount"]!.GetValue<long>()==1200&&credit["Uploader"]!["UserId"]!.GetValue<Guid>()==Guid.Parse("22222222-2222-2222-2222-222222222222"),"Credit amount/actor changed");
                    }
                    if(op.Feature=="ReadingList"&&op.Method=="GenerateAsync")
                    {
                        var bulk=success.State.Report.Commands.Where(c=>c.Sql.StartsWith("BULK INSERT ")).ToArray();
                        Check(bulk.Length==3&&Convert.ToInt32(bulk[0].Parameters["row-count"])==2&&Convert.ToInt32(bulk[1].Parameters["row-count"])==2&&Convert.ToInt32(bulk[2].Parameters["row-count"])==4,"Bulk destinations/row coverage changed");
                        Check(Equals(bulk[1].Parameters["row0.BlockId"],bulk[0].Parameters["row0.Id"])&&Equals(bulk[2].Parameters["row0.TrackingId"],bulk[1].Parameters["row0.Id"]),"Reading block/tracking/detail identities diverged");
                    }
                    if(op.Feature=="CustomerMaintenance"&&before.GetParameters().FirstOrDefault()?.ParameterType.Name=="MeterInstallationUpdateInputDto")
                    {
                        Check(success.State.Report.Commands.Count>0&&!success.State.Report.Committed&&success.State.Report.TransactionDisposed&&success.State.Report.ConnectionDisposed,"Known uncommitted meter installation update changed");knownUncommitted++;
                        Console.WriteLine("KNOWN LEGACY DEFECT: Meter installation date update performs commands but Commit is commented out in both versions; unchanged behavior is verified, not certified as a successful persisted update.");
                    }
                    if(success.Outcome.Calls.Any(c=>c.Contains("FluentValidation.IValidator")))Check((await Compare("invalid-validation")).Outcome.Error is not null,"Invalid input continued");
                    await Compare("empty");await Compare("missing");
                    for(int step=1;step<=success.Outcome.Calls.Length;step++)
                    {await Compare("success",fail:step);if(success.Outcome.AsyncSteps.Contains(step))await Compare("success",cancel:step);}
                    foreach(int step in success.Outcome.AsyncSteps)await Compare("success",gate:step);
                    int commands=success.State.Report.Commands.Count;
                    if(success.State.Report.FactoryCalls>0)
                    {
                        foreach(string mode in new[]{"open","open-failure","begin-failure","commit-failure"})await Compare("success",sqlMode:mode);
                        for(int step=1;step<=commands;step++)foreach(string mode in new[]{"failure","cancel","negative","gate"})await Compare("success",sqlMode:mode,sqlAt:step);
                    }
                    async Task<Run> Compare(string mode,int fail=0,int cancel=0,int gate=0,string sqlMode="success",int sqlAt=0)
                    {
                        using var cts=new CancellationTokenSource();if(cancel>0)cts.Cancel();
                        var x=await Execute(old,before,mode,fail,cancel,gate,sqlMode,sqlAt,cts.Token);
                        var y=await Execute(current,after!,mode,fail,cancel,gate,sqlMode,sqlAt,cts.Token);
                        string label=op.Feature+"."+op.Method+"/"+before.GetParameters().FirstOrDefault()?.ParameterType.Name+"/"+mode+"/"+fail+cancel+gate+sqlMode+sqlAt;
                        string cx=Canonical(x,op.Feature),cy=Canonical(y,op.Feature);
                        if(cx!=cy)
                        {
                            int first=Enumerable.Range(0,Math.Min(cx.Length,cy.Length)).FirstOrDefault(i=>cx[i]!=cy[i]);
                            Console.WriteLine("CANONICAL DIFFERENCE: "+label+" at "+first+" BEFORE "+cx.Substring(Math.Max(0,first-60),Math.Min(180,cx.Length-Math.Max(0,first-60)))+" AFTER "+cy.Substring(Math.Max(0,first-60),Math.Min(180,cy.Length-Math.Max(0,first-60))));
                        }
                        Check(cx==cy,"Workflow before/after mismatch: "+label+"\nBEFORE "+JsonSerializer.Serialize(x)+"\nAFTER "+JsonSerializer.Serialize(y));scenarios++;
                        if((fail>0||cancel>0)&&op.Method=="CreateAfterSaleAsync"&&y.Outcome.AsyncSteps.Contains(Math.Max(fail,cancel))&&y.Outcome.Calls[Math.Max(fail,cancel)-1].Contains(".IMoshtrakService.GetServicesSelected("))
                            Check(x.Outcome.Error is not null&&y.Outcome.Error is not null&&!x.State.Report.Committed&&!y.State.Report.Committed,"Failed after-sale validation committed");
                        if(sqlMode is "failure" or "cancel")Check(y.Outcome.Error is not null&&!y.State.Report.Committed,"Failed SQL committed: "+label);
                        if(sqlMode!="success")sqlCases++;
                        return y;
                    }
                    async Task<Run> Execute(Type type,MethodInfo method,string mode,int fail,int cancel,int gate,string sqlMode,int sqlAt,CancellationToken token)
                    {
                        var state=new AuxiliarySqlState();state.Report.Mode=sqlMode;state.Report.At=sqlAt;
                        WorkflowRecordingConnectionBase.Current.Value=state;
                        var probe=Probe(mapper,token,op.Feature,op.Method);probe.Mode=mode;probe.FailAt=fail;probe.CancelAt=cancel;probe.GateAt=gate;
                        var pending=ServiceConsolidationChecks.Execute(type,method,probe);
                        if(gate>0)
                        {
                            bool knownMissingAwait=op.Method=="CreateAfterSaleAsync"&&probe.Calls[gate-1].Contains(".IMoshtrakService.GetServicesSelected(");
                            Check(!pending.IsCompleted&&probe.Calls.Count==gate,"Dependency await behavior changed: "+op.Feature+"."+op.Method+"/"+gate);
                            probe.Gate.SetResult(true);
                        }
                        if(sqlMode=="gate"){Check(!pending.IsCompleted&&!state.Report.Committed&&state.Report.Commands.Count==sqlAt,"SQL was not awaited: "+op.Feature+"."+op.Method);state.Report.Gate.SetResult();}
                        var outcome=await pending;WorkflowRecordingConnectionBase.Current.Value=null;
                        if(op.Feature=="UserAuthentication"&&op.Method=="AddLoginAsync"&&outcome.Error is null)
                        {
                            var saved=probe.Tracked.OfType<Aban360.UserPool.Domain.Features.Auth.Entities.UserLogin>().Single();var response=JsonNode.Parse(outcome.Result)!;
                            Check(response["TempCode"]!.GetValue<string>()==saved.TwoStepCode&&response["Id"]!.GetValue<Guid>()==saved.Id&&response["FromMin"]!.GetValue<int>()==120,"Login response differs from persisted code/identity/expiration");
                        }
                        if(op.Feature=="RequestRegistration"&&op.Method.StartsWith("Create")&&outcome.Error is null)
                        {
                            var response=JsonNode.Parse(outcome.Result)!;int number=response[0]!["TrackNumber"]!.GetValue<int>();
                            Check(number>=170&&number<=179&&response[0]!["StringTrackNumber"]!.GetValue<string>()==number.ToString().PadLeft(11,'0'),"Generated request track number lost its database seed/random suffix/format");
                            Check(state.Report.Commands.SelectMany(c=>c.Parameters).Where(p=>p.Key=="TrackNumber").All(p=>Convert.ToInt32(p.Value)==number),"Request track number changed across SQL commands");
                        }
                        string sql=JsonSerializer.Serialize(state.Report.Commands.Select(c=>new{c.Sql,Parameters=c.Parameters.OrderBy(p=>p.Key).ToDictionary(p=>p.Key,p=>p.Value),c.SameTransaction}));
                        return new Run(outcome,state,sql);
                    }
                }
            }
        }
        Check(signatures==43,"Workflow signature coverage changed: "+signatures);
        Check(limits.Count==0,"Workflow success fixtures incomplete: "+string.Join("; ",limits));
        Check(knownUncommitted==1,"Known uncommitted transaction coverage changed");
        Check(knownUnawaited==1,"Known unawaited validation coverage changed");
        Console.WriteLine($"PASS: {checks} workflow checks; 10 scoped services, {signatures} signatures, {scenarios} before/after scenarios, {sqlCases} SQL fault/await scenarios. No real database, SMS or background job executed.");
    }
    internal record Run(ServiceConsolidationChecks.Outcome Outcome,AuxiliarySqlState State,string Sql);
    static string Canonical(Run run,string feature)
    {
        string text=JsonSerializer.Serialize(new{run.Outcome,run.Sql,run.State.Events,run.State.Main.FactoryCalls,ReportFactoryCalls=run.State.Report.FactoryCalls,run.State.Report.Committed,run.State.Report.ConnectionDisposed,run.State.Report.TransactionDisposed,run.State.Report.Isolation});
        var ids=new Dictionary<string,string>();
        text=Regex.Replace(text,@"[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-4[a-fA-F0-9]{3}-[89abAB][a-fA-F0-9]{3}-[a-fA-F0-9]{12}",m=>{if(!ids.ContainsKey(m.Value))ids[m.Value]="generated-id-"+ids.Count;return ids[m.Value];});
        text=Regex.Replace(text,@"(TwoStepCode\\u0022:\\u0022|TempCode\\u0022:\\u0022)(\d{4})(\\u0022)","$1generated-code$3");
        // The real T0 service appends one random digit to the isolated provider's seed 17.
        // The range and consistency across response and commands are asserted separately.
        if(feature=="RequestRegistration")
        {
            text=Regex.Replace(text,@"0000000017\d(?!\d)","generated-request-string-track");
            text=Regex.Replace(text,@"(?<!\d)17\d(?!\d)","generated-request-track");
        }
        text=Regex.Replace(text,@"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:(?:Z|[+-]\d{2}:\d{2})|(?:\\+u002B\d{2}:\d{2}))?",m=>
        {var raw=Regex.Replace(m.Value,@"\\+u002B","+");return DateTime.TryParse(raw,out var date)&&Math.Abs((date-DateTime.Now).TotalMinutes)<125?((date-DateTime.Now).TotalMinutes>110?"login-expiry-plus-120-minutes":"current-generated-time"):m.Value;});
        return text;
    }
    internal static ServiceProbe Probe(IMapper mapper,CancellationToken token,string feature,string operation)
    {
        var state=Management20Checks.Probe(mapper,token,feature,operation);
        state.ResultSnapshotTransform=value=>value is ITuple tuple?Enumerable.Range(0,tuple.Length).Select(i=>tuple[i]).ToArray():value;
        var argument=state.ArgumentFactory!;
        state.ArgumentFactory=p=>
        {
            if(p.ParameterType==typeof(string)&&p.Name=="plainPassword")return "sample-Password";
            var value=argument(p);
            if(value is Aban360.UserPool.Domain.Features.Auth.Dto.Commands.SecondStepLoginInput login)login.ConfirmCode=state.Mode=="wrong-code"?"0000":"2468";
            if(feature=="CustomerMaintenance"&&value is not null)
            {
                foreach(var prop in value.GetType().GetProperties().Where(p=>p.CanWrite))
                {
                    if(prop.PropertyType==typeof(string)&&prop.Name.EndsWith("DateJalali"))prop.SetValue(value,"1405/08/27");
                    if(prop.PropertyType==typeof(int)&&prop.Name is "EmptyUnit" or "CommercialUnit" or "CommertialUnit" or "OtherUnit")prop.SetValue(value,0);
                    if(prop.PropertyType==typeof(int)&&prop.Name is "DomesticUnit" or "UsageSellId" or "UsageConsumptionId")prop.SetValue(value,1);
                    if(prop.Name=="BranchTypId")prop.SetValue(value,6);
                }
            }
            if(value is Aban360.ClaimPool.Domain.Features.Request.Dto.Commands.MotherChildRequestInputDto child)child.ImprovementOverall=16;
            return value;
        };
        var transform=state.ResultTransform!;
        state.ResultTransform=(contract,method,args,value)=>
        {
            value=transform(contract,method,args,value);
            if(feature=="RequestRegistration"&&value is Aban360.Common.BaseEntities.MemberInfoGetDto requestMember)requestMember.MainSiphon="100";
            if(feature=="RequestRegistration"&&method.Name=="GetWithoutResultInDate")return state.Mode=="scheduled"?0:17;
            if(feature=="MotherChildRequest"&&operation is "SetMotherChildAsync" or "SetCommonSiphonAsync"&&contract.Name=="IMotherQueryService"&&method.Name=="Get")return null;
            if(feature=="CustomerMaintenance"&&contract.Name=="ICommonMemberQueryService"&&method.Name=="GetFromClient")return ServiceConsolidationChecks.Sample(method.ReturnType.GetGenericArguments()[0],empty:true);
            if(feature=="CustomerMaintenance"&&value?.GetType().Name=="MemberInfoGetDto")
            {
                foreach(var prop in value.GetType().GetProperties().Where(p=>p.CanWrite&&p.PropertyType==typeof(int)))
                {
                    if(prop.Name is "UsageId" or "DomesticUnit")prop.SetValue(value,1);
                    if(prop.Name is "UseStateId" or "CommercialUnit" or "OtherUnit")prop.SetValue(value,0);
                }
            }
            if(feature=="ReadingList"&&method.Name=="GetUnComplete")return ServiceConsolidationChecks.Sample(method.ReturnType.GetGenericArguments()[0],empty:true);
            void ReadingItem(object? item)
            {
                if(item is Aban360.UserPool.Domain.Features.Auth.Dto.Queries.UserQueryDto reader)reader.Username="17";
                if(item is Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries.TrackingGetDto tracking)
                {tracking.CounterReaderId=Guid.Parse("22222222-2222-2222-2222-222222222222");tracking.FlowStepId=(int)(operation=="FinishAsync"?Aban360.MeterPool.Domain.Constants.ReadingFlowStepEnum.StartReading:Aban360.MeterPool.Domain.Constants.ReadingFlowStepEnum.DownloadByReader);}
                if(item is Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries.DetailGetDto detail)detail.ExcludeBy=null;
            }
            if(feature=="ReadingList")
            {
                ReadingItem(value);
                if(value is IEnumerable list&&value is not string)foreach(var item in list)ReadingItem(item);
                if(operation=="OffloadAsync"&&value is IList listItems&&listItems.Count>1&&listItems[0] is Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries.TrackingGetDto)
                    while(listItems.Count>1)listItems.RemoveAt(listItems.Count-1);
            }
            if(value is Aban360.UserPool.Domain.Features.Auth.Entities.User user)
            {
                if(state.Mode=="unknown-user")return null;
                user.Password=SecurityOperations.GetSha512Hash(state.Mode=="wrong-password"?"other-password":"sample-Password").GetAwaiter().GetResult();
                user.RemoveLogInfo=null;
            }
            if(value is Aban360.UserPool.Domain.Features.Auth.Entities.UserLogin userLogin)userLogin.TwoStepCode="2468";
            if(value is IEnumerable values&&value is not string)
                foreach(var item in values)if(item is Aban360.UserPool.Domain.Features.Auth.Entities.UserClaim claim)claim.ClaimValue="17";
            if(contract.Name=="IDocumentGetContentTypeAddhoc")
            {
                string header=(state.Mode=="bad-bank"?"18":"17")+(state.Mode=="bad-total"?"00003501":"00003500")+(state.Mode=="bad-count"?"003":"002");
                return System.Text.Encoding.UTF8.GetBytes(header+"\r\n1111111111111\r\n2222222222222");
            }
            if(contract.Name=="IBankFileStructureQueryService")
            {
                return new List<Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities.BankFileStructure>{
                    new(){BankStructureItemId=Aban360.PaymentPool.Domain.Constansts.BankStructureItemEnum.PaymentId,FromIndex=0,StringLenght=13},
                    new(){BankStructureItemId=Aban360.PaymentPool.Domain.Constansts.BankStructureItemEnum.BankCode,FromIndex=0,StringLenght=2},
                    new(){BankStructureItemId=Aban360.PaymentPool.Domain.Constansts.BankStructureItemEnum.TotalPrice,FromIndex=2,StringLenght=8},
                    new(){BankStructureItemId=Aban360.PaymentPool.Domain.Constansts.BankStructureItemEnum.RecordNO,FromIndex=10,StringLenght=3}};
            }
            if(contract.Name=="IInvoiceInstallmentGetByPaymentIdAddhoc")
            {
                if(state.Mode=="invoice-missing")return null;
                value!.GetType().GetProperty("Amount")!.SetValue(value,Convert.ChangeType(args[0]?.ToString()=="2222222222222"?2300:1200,Nullable.GetUnderlyingType(value.GetType().GetProperty("Amount")!.PropertyType)??value.GetType().GetProperty("Amount")!.PropertyType));
                value.GetType().GetProperty("PaymentId")?.SetValue(value,args[0]);
            }
            return value;
        };
        var synchronous=state.SynchronousResultTransform!;
        state.SynchronousResultTransform=(contract,method,args,value)=>
        {
            value=synchronous(contract,method,args,value);
            if(method.ReturnType.IsGenericType&&method.ReturnType.GetGenericTypeDefinition()==typeof(IQueryable<>))
            {
                if(feature=="UserNavigation"&&value is IEnumerable claims)
                    foreach(var item in claims)if(item is Aban360.UserPool.Domain.Features.Auth.Entities.UserClaim claim)
                    {claim.ClaimTypeId=Aban360.UserPool.Persistence.Constants.Enums.ClaimType.Endpoint;claim.ClaimValue="sample-AuthValue";claim.UserId=Guid.Parse("22222222-2222-2222-2222-222222222222");claim.ValidTo=null;claim.RemoveGroupId=null;}
                return Activator.CreateInstance(typeof(ManagementAsyncEnumerable<>).MakeGenericType(method.ReturnType.GetGenericArguments()),value);
            }
            return value;
        };
        state.CallObserver=(contract,method,args)=>
        {
            if(state.Calls.Count>200)throw new InvalidOperationException("workflow-fixture-loop-guard");
            if(operation=="AddLoginAsync"&&args.FirstOrDefault() is Aban360.UserPool.Domain.Features.Auth.Entities.UserLogin login)
            {
                state.Tracked.Add(login);
                if(login.Id==Guid.Empty||!int.TryParse(login.TwoStepCode,out int code)||code<1000||code>=9999||Math.Abs((login.TwoStepExpireDateTime!.Value-DateTime.Now).TotalMinutes-120)>0.1)
                    throw new InvalidOperationException("Login identity/code/expiry changed");
            }
        };
        return state;
    }
}

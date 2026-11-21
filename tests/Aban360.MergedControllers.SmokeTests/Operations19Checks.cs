using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Aban360.Common.ApplicationUser;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

static class Operations19Checks
{
    internal record Operation(string Feature,string Assembly,string Original,string OldContract,string Service,string Output,Dictionary<string,string> PublicMethods);
    record Run(ServiceConsolidationChecks.Outcome Outcome,AuxiliarySqlState State);
    public static async Task RunFocused()
    {
        var services=new ServiceCollection();services.AddLogging();
        Aban360.CalculationPool.Application.Extensions.ConfigureServices.AddCalculationPoolApplicationInjections(services);
        Aban360.OldCalcPool.Application.Extentions.ConfigureServices.AddOldCalcPoolApplicationInjections(services);
        Aban360.ClaimPool.Application.Extentions.ConfigureServices.AddClaimPoolApplicationInjections(services);
        Aban360.PaymentPool.Application.Extensions.ConfigureServices.AddPaymentPoolApplicationInjections(services);
        Aban360.BlobPool.Application.Extenstions.ConfigureServices.AddBlobPoolApplicationInjections(services);
        using var provider=services.BuildServiceProvider();var mapper=provider.GetRequiredService<IMapper>();
        var operations=JsonSerializer.Deserialize<Operation[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"ServiceConsolidationOperations19.json")))!;
        // The two file operations now live inside the unified CollectBills workflow.
        // Verify that workflow (including real ZIP output and legacy job activation)
        // directly, rather than replacing its internal CreateZip call with an old service mock.
        if(operations.Length!=70 || operations.Count(o=>o.Feature=="CollectBillsFiles")!=2) throw new Exception("Historical operation selection changed");
        await CollectBillsRuntimeChecks.Run();
        operations=operations.Where(o=>o.Feature!="CollectBillsFiles").ToArray();
        var test=typeof(Operations19Checks).Assembly;var originals=operations.ToDictionary(o=>o.OldContract,o=>test.GetType(o.Original.Replace("BeforeServiceOperations19.","SqlBeforeOperations19."))??test.GetType(o.Original,true)!);
        var current=operations.GroupBy(o=>o.Service).ToDictionary(g=>g.Key,g=>
        {
            var contract=Assembly.Load(g.First().Assembly).GetType(g.Key,true)!;var production=contract.Assembly.GetTypes().Single(t=>t.IsClass&&!t.IsAbstract&&contract.IsAssignableFrom(t));
            return test.GetType("SqlAfterOperations19."+production.FullName)??production;
        });
        int checks=0,signatures=0,scenarios=0,successful=0,sqlCases=0;var coverage=new List<string>();
        void Check(bool ok,string message){if(!ok)throw new InvalidOperationException(message);checks++;}
        Check(operations.Length==68&&current.Count==18,"Approved selection mismatch");
        // Merging entry points can combine previously separate constructor dependencies.
        // Walk production implementations too: mocked DI resolution alone would hide a new cycle.
        var productionTypes=Directory.GetFiles(AppContext.BaseDirectory,"Aban*.dll")
            .Where(path=>Path.GetFileNameWithoutExtension(path)!=test.GetName().Name)
            .Select(Assembly.LoadFrom).SelectMany(a=>a.GetTypes())
            .Where(t=>t.IsClass&&!t.IsAbstract&&!t.ContainsGenericParameters).ToArray();
        var implementations=productionTypes.SelectMany(t=>t.GetInterfaces().Select(i=>(Contract:i,Implementation:t)))
            .ToLookup(pair=>pair.Contract,pair=>pair.Implementation);
        var visited=new HashSet<Type>();var resolving=new List<Type>();
        void VisitDependencies(Type type)
        {
            if(type.IsGenericType&&type.GetGenericTypeDefinition()==typeof(IEnumerable<>))
            {foreach(var implementation in implementations[type.GetGenericArguments()[0]])VisitDependencies(implementation);return;}
            if(type.Namespace?.StartsWith("Aban",StringComparison.Ordinal)!=true)return;
            if(type.IsInterface)
            {
                var candidates=implementations[type].ToArray();var matching=candidates.Where(t=>t.Name==type.Name[1..]).ToArray();
                foreach(var candidate in matching.Length>0?matching:candidates.Length==1?candidates:Array.Empty<Type>())VisitDependencies(candidate);
                return;
            }
            if(resolving.Contains(type))throw new InvalidOperationException("Production constructor dependency cycle: "+string.Join(" -> ",resolving.Append(type).Select(t=>t.FullName)));
            if(!visited.Add(type))return;
            resolving.Add(type);
            var constructor=type.GetConstructors().OrderByDescending(c=>c.GetParameters().Length).FirstOrDefault();
            if(constructor is not null)foreach(var parameter in constructor.GetParameters())VisitDependencies(parameter.ParameterType);
            resolving.RemoveAt(resolving.Count-1);
        }
        foreach(var group in operations.GroupBy(o=>o.Service))
        {
            var contract=Assembly.Load(group.First().Assembly).GetType(group.Key,true)!;
            VisitDependencies(services.Single(d=>d.ServiceType==contract).ImplementationType!);
            Check(resolving.Count==0,"Incomplete constructor dependency traversal");
        }
        // Discover effective MVC routes after moving the bank prefix to the class.
        // This checks the actual composed template and verb, independently of the syntax comparison.
        var mvc=new ServiceCollection();mvc.AddLogging();
        mvc.AddControllers().ConfigureApplicationPartManager(m=>{m.ApplicationParts.Clear();m.ApplicationParts.Add(new AssemblyPart(typeof(Aban360.Api.Controllers.V1.BaseController).Assembly));});
        using(var discovery=mvc.BuildServiceProvider())
        {
            var actions=discovery.GetRequiredService<IActionDescriptorCollectionProvider>().ActionDescriptors.Items.OfType<ControllerActionDescriptor>().ToArray();
            var bank=typeof(Aban360.Api.Controllers.V1.PaymentPool.BankReceipt.BankReceiptController);
            var expected=new[]{("Delete","POST","delete-by-bank-date"),("List","GET","files"),("Upload","POST","upload"),("Validate","POST","{id:guid}/validate"),("Display","GET","{id:guid}/display"),("Reconcile","POST","{id:guid}/reconcile"),("Confirm","POST","{id:guid}/confirm")};
            Check(actions.Count(a=>a.ControllerTypeInfo.AsType()==bank)==expected.Length,"Bank endpoint count changed");
            foreach(var item in expected)
            {
                var action=actions.Single(a=>a.ControllerTypeInfo.AsType()==bank&&a.ActionName==item.Item1);
                string route="v1/bank-receipt/"+item.Item3;
                Check(action.AttributeRouteInfo?.Template==route,"Bank route composition changed: "+item.Item1);
                Check(action.ActionConstraints!.OfType<HttpMethodActionConstraint>().SelectMany(c=>c.HttpMethods).SequenceEqual(new[]{item.Item2}),"Bank HTTP verb changed: "+item.Item1);
                Check(actions.Count(a=>a.AttributeRouteInfo?.Template==route&&a.ActionConstraints!.OfType<HttpMethodActionConstraint>().Any(c=>c.HttpMethods.Contains(item.Item2)))==1,"Ambiguous bank endpoint: "+route);
            }
        }
        foreach(var group in operations.GroupBy(o=>o.Service))
        {
            var contract=Assembly.Load(group.First().Assembly).GetType(group.Key,true)!;var registration=services.Where(d=>d.ServiceType==contract).ToArray();
            Check(registration.Length==1&&registration[0].Lifetime==ServiceLifetime.Scoped,"Service registration mismatch: "+group.Key);
            var production=registration[0].ImplementationType!;Check(production.Name==contract.Name[1..],"Service implementation mismatch");
            var probe=Probe(mapper,CancellationToken.None,group.First(),"",false,"");IServiceCollection local=new ServiceCollection();local.Add(registration[0]);
            foreach(var p in production.GetConstructors().Single().GetParameters())local.AddScoped(p.ParameterType,_=>probe.Proxy(p.ParameterType));
            using var scoped=local.BuildServiceProvider(new ServiceProviderOptions{ValidateScopes=true});using var a=scoped.CreateScope();using var b=scoped.CreateScope();
            Check(ReferenceEquals(a.ServiceProvider.GetRequiredService(contract),a.ServiceProvider.GetRequiredService(contract))&&!ReferenceEquals(a.ServiceProvider.GetRequiredService(contract),b.ServiceProvider.GetRequiredService(contract)),"Scope changed");
        }
        foreach(var op in operations)
        {
            var beforeType=originals[op.OldContract];var afterType=current[op.Service];
            foreach(var before in beforeType.GetMethods(BindingFlags.Instance|BindingFlags.Public|BindingFlags.DeclaredOnly).Where(m=>op.PublicMethods.ContainsKey(m.Name)))
            {
                string name=op.PublicMethods[before.Name];var after=afterType.GetMethod(name,before.GetParameters().Select(p=>p.ParameterType).ToArray());
                Check(after is not null&&before.ReturnType==after.ReturnType,"Public signature changed: "+op.Feature+"."+before.Name);signatures++;
                var success=await Compare("success");
                string handler=op.Original.Split('.').Last();
                string? expectedGuard=handler switch
                {
                    "ServiceLinkReturnRemoveHandler"=>"Aban360.Common.Exceptions.InvalidTrackingException:",
                    "CalculationConfirmationHandler" or "MeterReadingCalculationConfirmThenSendSmsHandler"=>"Aban360.Common.Exceptions.ReadingException:",
                    "MeterFlowValidationGetHandler" when before.GetParameters().Length==3=>"Aban360.Common.Exceptions.ReadingException:",
                    "FreeGenerateBillHandler"=>"Aban360.Common.Exceptions.InvalidBillCommandException:",
                    _=>null
                };
                Check(expectedGuard is null?success.Outcome.Error is null:success.Outcome.Error?.StartsWith(expectedGuard)==true,"Seeded success/explicit rejection coverage changed: "+handler+" / "+success.Outcome.Error);
                coverage.Add(op.Feature+"."+before.Name+"("+string.Join(',',before.GetParameters().Select(p=>p.ParameterType.Name))+"): "+(success.Outcome.Error??"SUCCESS"));
                if(success.Outcome.Error is null)successful++;
                if(op.Feature=="ServiceLinkPayment"&&before.GetParameters().FirstOrDefault()?.ParameterType.Name=="ServiceLinkRegisterManualInputDto")
                {
                    Check(success.Outcome.Error is null&&success.State.Report.Committed,"Manual payment fixture did not persist");
                    Check(success.State.Report.Commands.Any(c=>c.Parameters.TryGetValue("Pard",out var amount)&&Convert.ToInt64(amount)==1234000),"Legacy payment rounding changed");
                    Check(success.State.Report.Commands.Any(c=>c.Parameters.TryGetValue("Amount",out var amount)&&Convert.ToInt64(amount)==1234000),"Warehouse amount differs from posted payment");
                }
                if(op.Feature=="OtherExpensesItems"&&before.GetParameters().FirstOrDefault()?.ParameterType.Name=="OtherExpensesItemsInsertInputDto")
                    Check(success.Outcome.Error is null&&success.State.Main.Committed&&success.State.Main.Commands.Any(c=>c.Parameters.TryGetValue("Amount",out var amount)&&Convert.ToInt64(amount)==1234567&&c.Parameters.TryGetValue("InsertBy",out var actor)&&actor is Guid id&&id==Guid.Parse("22222222-2222-2222-2222-222222222222")),"Expense amount/actor/commit changed");
                if(op.Feature=="BankReceipt"&&before.Name is "Validate" or "Display")
                {
                    var result=JsonNode.Parse(success.Outcome.Result)!;
                    Check(success.Outcome.Error is null&&result["ActualAmount"]!.GetValue<long>()==3500&&result["ActualRecordCount"]!.GetValue<int>()==2,"Bank preview total/count changed");
                }
                if(op.Feature=="TankerWaterOperations"&&before.GetParameters().FirstOrDefault()?.ParameterType.Name=="TankerWaterCalculationInputDto")
                {
                    var result=JsonNode.Parse(success.Outcome.Result)!;decimal water=result["Water"]!.GetValue<decimal>(),budget=result["Budget"]!.GetValue<decimal>(),hot=result["HotSeason"]!.GetValue<decimal>();
                    Check(success.Outcome.Error is null&&water==83521m&&budget==34000m&&result["Tax"]!.GetValue<decimal>()==(water+budget+hot)*0.1m,"Tanker formula/budget/tax changed");
                    Check(result["Final"]!.GetValue<decimal>()==water+budget+hot+result["Delivery"]!.GetValue<decimal>()+result["Tax"]!.GetValue<decimal>(),"Tanker final amount changed");
                }
                await Compare("empty");await Compare("invalid-validation");await Compare("null-query-result");
                foreach(int step in success.Outcome.AsyncSteps)
                {
                    await Compare("success",fail:step);await Compare("success",cancel:step);await Compare("success",gate:step);
                }
                foreach(var sql in new[]{("main",success.State.Main),("report",success.State.Report)})
                {
                    if(sql.Item2.Commands.Count==0)continue;
                    foreach(string mode in new[]{"open-failure","begin-failure","commit-failure"})await Compare("success",sqlMode:mode,sqlDb:sql.Item1);
                    for(int i=1;i<=sql.Item2.Commands.Count;i++)foreach(string mode in new[]{"failure","cancel","negative","gate"}){await Compare("success",sqlMode:mode,sqlAt:i,sqlDb:sql.Item1);sqlCases++;}
                }
                async Task<Run> Compare(string mode,int fail=0,int cancel=0,int gate=0,string sqlMode="success",int sqlAt=0,string sqlDb="report")
                {
                    using var cts=new CancellationTokenSource();if(cancel>0)cts.Cancel();
                    string label=op.Feature+"."+before.Name+"/"+mode+"/"+fail+"/"+cancel+"/"+gate+"/"+sqlDb+"/"+sqlMode+"/"+sqlAt;var x=await Execute(true);var y=await Execute(false);
                    Check(Canonical(x)==Canonical(y),"Operations before/after mismatch: "+label+"\nBEFORE "+Canonical(x)+"\nAFTER "+Canonical(y));scenarios++;
                    return y;
                    async Task<Run> Execute(bool old)
                    {
                        var state=new AuxiliarySqlState();var recording=sqlDb=="main"?state.Main:state.Report;recording.Mode=sqlMode;recording.At=sqlAt;WorkflowRecordingConnectionBase.Current.Value=state;
                        var probe=Probe(mapper,cts.Token,op,before.Name,old,mode);probe.FailAt=fail;probe.CancelAt=cancel;probe.GateAt=gate;
                        var instances=new Dictionary<Type,object>();var outerFactory=probe.DependencyFactory;
                        probe.DependencyFactory=type=>
                        {
                            Type? implementation=old?originals.GetValueOrDefault(type.FullName!):current.GetValueOrDefault(type.FullName!);
                            if(implementation is null)return outerFactory?.Invoke(type);
                            if(!instances.TryGetValue(type,out var instance)){instance=implementation.GetConstructors().Single().Invoke(implementation.GetConstructors().Single().GetParameters().Select(p=>probe.Proxy(p.ParameterType)).ToArray());instances[type]=instance;}return instance;
                        };
                        var pending=ServiceConsolidationChecks.Execute(old?beforeType:afterType,old?before:after!,probe);
                        if(gate>0){Check(!pending.IsCompleted&&probe.Calls.Count==gate,"Dependency is no longer awaited: "+label);probe.Gate.SetResult(true);}
                        if(sqlMode=="gate"){Check(!pending.IsCompleted&&recording.Commands.Count==sqlAt,"SQL is no longer awaited: "+label);recording.Gate.SetResult();}
                        var outcome=await pending;WorkflowRecordingConnectionBase.Current.Value=null;
                        if(sqlMode is "failure" or "cancel")
                        {
                            int failed=recording.Events.FindIndex(e=>e=="Execute"+sqlAt);
                            Check(outcome.Error is not null&&failed>=0&&!recording.Events.Skip(failed+1).Contains("Commit"),"Failed SQL committed: "+label);
                            if(op.Original.EndsWith("MeterReadingCalculationConfirmThenSendSmsHandler"))Check(!outcome.Calls.Any(c=>c.Contains(".IMeterReadingDetailSendSmsHandler.Handle(")),"SMS followed a failed confirmation");
                        }
                        return new Run(outcome,state);
                    }
                }
            }
        }
        File.WriteAllLines(Path.Combine(AppContext.BaseDirectory,"Operations19Coverage.txt"),coverage);
        // The former two CollectBills entry points are verified by the unified
        // workflow checks above; all other historical signatures remain covered.
        Check(signatures==79,"Missing overload/public method coverage: "+signatures);
        Check(successful==74,"Successful fixture coverage regressed: "+successful);
        Console.WriteLine($"PASS: {checks} operations checks; 18 historical scoped services plus unified CollectBills, {signatures} historical signatures ({successful} successful seeded paths), {scenarios} before/after scenarios, {sqlCases} SQL fault/await scenarios. All persistence, SMS, jobs and OpenKM boundaries isolated.");
    }
    static ServiceProbe Probe(IMapper mapper,CancellationToken token,Operation op,string method,bool old,string mode)
    {
        var probe=new ServiceProbe(mapper,token){CriticalContext=true,Mode=mode};
        probe.DependencyFactory=type=>
        {
            if(type.IsGenericType&&type.GetGenericTypeDefinition()==typeof(IEnumerable<>))return Array.CreateInstance(type.GetGenericArguments()[0],0);
            if(type.IsGenericType&&type.GetGenericTypeDefinition()==typeof(Microsoft.Extensions.Options.IOptions<>))return Activator.CreateInstance(typeof(Microsoft.Extensions.Options.OptionsWrapper<>).MakeGenericType(type.GetGenericArguments()[0]),ServiceConsolidationChecks.Sample(type.GetGenericArguments()[0]));
            if(type.IsGenericType&&type.GetGenericTypeDefinition()==typeof(Microsoft.Extensions.Logging.ILogger<>))return Activator.CreateInstance(typeof(Microsoft.Extensions.Logging.Abstractions.NullLogger<>).MakeGenericType(type.GetGenericArguments()[0]));
            return null;
        };
        probe.ResultSnapshotTransform=value=>value is ITuple tuple?Enumerable.Range(0,tuple.Length).Select(i=>tuple[i]).ToArray():value;
        probe.ArgumentFactory=p=>
        {
            if(p.ParameterType==typeof(Stream))return new MemoryStream(new byte[]{1,2,3,4,5});
            if(p.ParameterType==typeof(bool)&&p.Name=="isThumbnail")return false;
            if(p.ParameterType==typeof(IAppUser))return new AppUser(new ClaimsPrincipal(new ClaimsIdentity(new[]{new Claim(ClaimTypes.UserData,"22222222-2222-2222-2222-222222222222"),new Claim(ClaimTypes.Name,"17"),new Claim(ClaimTypes.GivenName,"Reviewer")},"test")));
            if(p.ParameterType==typeof(string))return p.Name?.Contains("date",StringComparison.OrdinalIgnoreCase)==true?"1405/01/02":p.Name=="billId"?"1234567890123":p.Name=="fileName"?"receipt.txt":"sample-"+p.Name;
            var result=ServiceConsolidationChecks.Sample(p.ParameterType,tag:p.Name??"input");Seed(result);return result;
        };
        probe.ResultTransform=(contract,mi,args,result)=>
        {
            if(contract.Name=="IBankReceiptImportRepository"&&mi.Name=="Get")return Import(method);
            if(contract.Name=="IBankReceiptImportRepository"&&mi.Name=="FindCommittedPayments")return new HashSet<string>();
            if(contract.Name=="IVariabService"&&mi.Name=="GetDateCheck")return "1300/01/01";
            if(result is Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries.MeterFlowValidationDto flow)
            {
                flow.RemovedDateTime=null;
                flow.MeterFlowStepId=op.Original.EndsWith("CalculationConfirmationHandler")||op.Original.EndsWith("MeterReadingCalculationConfirmThenSendSmsHandler")?Aban360.CalculationPool.Domain.Constants.MeterFlowStepEnum.ConsumptionChecked:Aban360.CalculationPool.Domain.Constants.MeterFlowStepEnum.Calculated;
            }
            if(op.Original.EndsWith("SwapRequestTypeHandler")&&result is Aban360.ClaimPool.Domain.Features.Request.Dto.Queries.TrackingOutputDto tracking)tracking.StatusId=(int)Aban360.ClaimPool.Domain.Constants.TrackingStatusEnum.RequestRegistered;
            Seed(result);return result;
        };
        probe.CallObserver=(contract,mi,args)=>
        {
            if(probe.Calls.Count>200)throw new InvalidOperationException("Unexpected loop in isolated operations fixture");
            if(contract.Name=="IBankReceiptParser"&&mi.Name=="Parse"){var import=(BankReceiptImport)args[0]!;import.Rows=Import(method).Rows;import.Header=Import(method).Header;import.DataRowCount=2;import.Issues=[];}
        };
        return probe;
    }
    static BankReceiptImport Import(string method)=>new(){Id=Guid.Parse("22222222-2222-2222-2222-222222222222"),UserId=Guid.Parse("22222222-2222-2222-2222-222222222222"),BankId=17,BankName="Bank",Username="17",FileName="receipt.txt",CreatedAt=new DateTime(2020,1,2),Version=17,DataRowCount=2,Status=method=="Confirm"?BankReceiptImportStatus.Reconciled:method=="Reconcile"?BankReceiptImportStatus.Validated:BankReceiptImportStatus.Uploaded,Header=new(){Amount=3500,RecordCount=2},Rows=[new(){LineNumber=2,Amount=1200,InvoiceId=10,InvoiceInstallmentId=11,Hash="first"},new(){LineNumber=3,Amount=2300,InvoiceId=20,InvoiceInstallmentId=21,Hash="second"}]};
    static void Seed(object? value,int depth=0)
    {
        if(value is null||depth>2||value is string)return;
        if(value is IEnumerable sequence){foreach(var item in sequence)Seed(item,depth+1);return;}
        if(value.GetType().Namespace?.StartsWith("Aban360.")!=true)return;
        foreach(var p in value.GetType().GetProperties().Where(p=>p.CanWrite&&p.GetIndexParameters().Length==0))
        {
            if(p.Name is "RemovedDateTime" or "RemovedByUserId"&&(!p.PropertyType.IsValueType||Nullable.GetUnderlyingType(p.PropertyType) is not null)){p.SetValue(value,null);continue;}
            if(p.PropertyType==typeof(string))
            {
                if(p.Name=="WaterFormula")p.SetValue(value,"M*SS*C*K");
                else if(p.Name is "MainSiphon" or "BankBranchCode" or "Username")p.SetValue(value,"100");
                else if(p.Name is "BankDateJalali" or "PayDateJalali")
                {var calendar=new System.Globalization.PersianCalendar();var date=DateTime.Now.AddDays(-10);p.SetValue(value,$"{calendar.GetYear(date):0000}/{calendar.GetMonth(date):00}/{calendar.GetDayOfMonth(date):00}");}
                else if(p.Name is "PreviousDateJalali" or "LastMeterDateJalali" or "DateBed")p.SetValue(value,"1404/12/01");
                else if(p.Name.Contains("Date")||p.Name=="UploadDate")p.SetValue(value,"1405/01/02");
                else if(p.Name=="BillId")p.SetValue(value,"1234567890123");
                else if(p.Name=="File")p.SetValue(value,Convert.ToBase64String([1,2,3,4]));
            }
            else if(p.Name=="IsRegistered"&&p.PropertyType==typeof(bool))p.SetValue(value,false);
            else if(p.Name=="RemovedByUserId"&&(Nullable.GetUnderlyingType(p.PropertyType) is not null))p.SetValue(value,null);
            else if(p.Name=="Amount"&&(p.PropertyType==typeof(decimal)||p.PropertyType==typeof(long)||p.PropertyType==typeof(int)))p.SetValue(value,Convert.ChangeType(1234567,p.PropertyType));
        }
    }
    static string Canonical(Run run)
    {
        string text=JsonSerializer.Serialize(new{run.Outcome,Main=State(run.State.Main),Report=State(run.State.Report),run.State.Events});var ids=new Dictionary<string,string>();
        text=Regex.Replace(text,@"[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-4[a-fA-F0-9]{3}-[89abAB][a-fA-F0-9]{3}-[a-fA-F0-9]{12}",m=>{if(!ids.ContainsKey(m.Value))ids[m.Value]="generated-id-"+ids.Count;return ids[m.Value];});
        text=Regex.Replace(text,@"\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(?:\.\d+)?(?:Z|[+-]\d\d:\d\d)?",m=>DateTime.TryParse(m.Value,out var date)&&Math.Abs((date.ToUniversalTime()-DateTime.UtcNow).TotalSeconds)<120?"current-time":m.Value);
        text=Regex.Replace(text,@"\d{4}-\d\d-\d\d-\d\d-\d\d-\d\d",m=>DateTime.TryParseExact(m.Value,"yyyy-MM-dd-HH-mm-ss",System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None,out var date)&&Math.Abs((date-DateTime.Now).TotalSeconds)<120?"current-filename-time":m.Value);
        return text;
        static object State(SqlRecording s)=>new{s.FactoryCalls,s.Committed,s.ConnectionDisposed,s.TransactionDisposed,s.Isolation,Commands=s.Commands.Select(c=>new{c.Sql,Parameters=c.Parameters.OrderBy(p=>p.Key).ToDictionary(p=>p.Key,p=>p.Value),c.SameTransaction})};
    }
}

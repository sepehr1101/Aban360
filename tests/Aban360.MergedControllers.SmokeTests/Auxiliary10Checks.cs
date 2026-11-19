using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

static class Auxiliary10Checks
{
    public static async Task RunFocused()
    {
        var services=new ServiceCollection();services.AddLogging();
        Aban360.UserPool.Application.Extensions.ConfigureServices.AddUserPoolApplicationInjections(services);
        Aban360.SystemPool.Application.Extenstions.ConfigureServices.AddSystemPoolApplicationInjections(services);
        Aban360.BlobPool.Application.Extenstions.ConfigureServices.AddBlobPoolApplicationInjections(services);
        Aban360.MeterPool.Application.Extensions.ConfigureService.AddMeterPoolApplicationInjections(services);
        Aban360.CalculationPool.Application.Extensions.ConfigureServices.AddCalculationPoolApplicationInjections(services);
        using var provider=services.BuildServiceProvider();var mapper=provider.GetRequiredService<IMapper>();
        int checks=await Run(mapper,services);
        checks+=await Auxiliary10TransactionChecks.Run(mapper);
        // Shared recording-provider helpers were extended to support this batch.
        checks+=await MainTagTransactionChecks.Run();
        Console.WriteLine($"PASS: {checks} auxiliary consolidation checks; no SMS provider, real SQL connection or background job invoked.");
    }

    static async Task<int> Run(IMapper mapper,IServiceCollection registrations)
    {
        int checks=0,scenarios=0,signatures=0,knownLegacyMappings=0;
        void Check(bool ok,string message){if(!ok)throw new InvalidOperationException(message);checks++;}
        var operations=ApplicationServiceBaseline.AuxiliaryOperations;
        Check(operations.Length==38&&operations.Select(o=>o.Service).Distinct().Count()==10,"Incomplete auxiliary selection");
        string previous=Environment.CurrentDirectory;
        string scratch=Path.Combine(Path.GetTempPath(),"aban360-auxiliary-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        try
        {
            Environment.CurrentDirectory=scratch;
            foreach(var group in operations.GroupBy(o=>o.Service))
            {
                var assembly=Assembly.Load(group.First().Assembly);var contract=assembly.GetType(group.Key,true)!;
                var current=assembly.GetTypes().Single(t=>t.IsClass&&!t.IsAbstract&&contract.IsAssignableFrom(t));
                var registration=registrations.Where(d=>d.ServiceType==contract).ToArray();
                Check(registration.Length==1&&registration[0].ImplementationType==current&&registration[0].Lifetime==ServiceLifetime.Scoped,"Auxiliary DI mismatch: "+group.Key);
                IServiceCollection local=new ServiceCollection();local.Add(registration[0]);var dependencies=Probe(mapper,CancellationToken.None,group.First().Feature,"");
                foreach(var p in current.GetConstructors().Single().GetParameters())local.AddScoped(p.ParameterType,_=>dependencies.Proxy(p.ParameterType));
                using var provider=local.BuildServiceProvider(new ServiceProviderOptions{ValidateScopes=true});
                using var scopeA=provider.CreateScope();using var scopeB=provider.CreateScope();
                var instance=scopeA.ServiceProvider.GetRequiredService(contract);
                Check(ReferenceEquals(instance,scopeA.ServiceProvider.GetRequiredService(contract))&&!ReferenceEquals(instance,scopeB.ServiceProvider.GetRequiredService(contract)),"Auxiliary scoped lifetime changed");
                foreach(var operation in group)
                {
                    var old=typeof(Auxiliary10Checks).Assembly.GetType(operation.Original,true)!;
                    foreach(var before in old.GetMethods(BindingFlags.Instance|BindingFlags.Public|BindingFlags.DeclaredOnly).Where(m=>m.Name==operation.OldMethod))
                    {
                        var after=current.GetMethod(operation.Method,before.GetParameters().Select(p=>p.ParameterType).ToArray())!;
                        Check(after is not null&&after.ReturnType==before.ReturnType,"Auxiliary signature changed: "+operation.Feature+"."+operation.Method);signatures++;
                        bool sql=old.BaseType?.Name=="AbstractBaseConnection"&&before.ReturnType==typeof(Task);
                        if(operation.Feature is "MeterApkLog" or "AssessmentLog")
                        {
                            string folder=operation.Feature=="MeterApkLog"?@"AppData\MeterApkLogs":@"AppData\AssessmentLogs";
                            // Each operation gets its own isolated filesystem fixture.
                            if(Directory.Exists(folder))Directory.Delete(folder,true);
                            var missing=await Compare("success");Check(missing.Error is not null,"Missing log path must retain its error");
                            Directory.CreateDirectory(folder);
                            File.WriteAllText(Path.Combine(folder,"sample.txt"),"متن نمونهٔ لاگ\nline 2",System.Text.Encoding.UTF8);
                            File.SetLastWriteTime(Path.Combine(folder,"sample.txt"),new DateTime(2020,2,3,4,5,6));
                            var ok=await Compare("success");Check(ok.Error is null,"Log fixture failed: "+ok.Error);
                            if(operation.Method=="SaveAsync")Check(File.ReadAllBytes(Path.Combine(folder,"sample.txt")).SequenceEqual(new byte[]{1,2,3,4}),"Log upload changed bytes");
                        }
                        else if(sql)
                        {
                            bool validates=old.GetConstructors().Single().GetParameters().Any(p=>p.ParameterType.IsGenericType&&p.ParameterType.GetGenericTypeDefinition()==typeof(FluentValidation.IValidator<>));
                            if(validates)Check((await Compare("invalid-validation")).Error?.Contains("CustomValidationException")==true,"Invalid SMS input passed validation");
                            if(operation.Feature is "SmsType" or "SmsStateGroup")Check((await Compare("duplicate")).Error?.Contains("ReadingException")==true,"Duplicate SMS definition accepted");
                            if(operation.Feature=="SmsFlow"&&operation.Method=="CreateAsync")Check((await Compare("invalid-date")).Error is not null,"Invalid scheduled date accepted");
                            await Compare("success",fail:1);await Compare("success",cancel:1);
                        }
                        else
                        {
                            var success=await Compare("success");
                            bool knownMapping=operation.Feature=="Document"&&operation.Method=="UpdateAsync";
                            Check(knownMapping?success.Error?.StartsWith("AutoMapper.AutoMapperMappingException:")==true&&success.Error.Contains("DocumentUpdateDto"):success.Error is null,"Auxiliary success fixture failed: "+operation.Feature+"."+operation.Method+" "+success.Error);
                            if(knownMapping)knownLegacyMappings++;
                            await Compare("empty");
                            if(operation.Feature=="MeterApkFile")await Compare("missing");
                            if(success.Calls.Any(c=>c.Contains("FluentValidation.IValidator")))await Compare("invalid-validation");
                            for(int step=1;step<=success.Calls.Length;step++)
                            {
                                await Compare("success",fail:step);
                                if(success.AsyncSteps.Contains(step))await Compare("success",cancel:step);
                            }
                            foreach(int step in success.AsyncSteps)
                            {
                                using var cts=new CancellationTokenSource();var a=Probe(mapper,cts.Token,operation.Feature,operation.Method);var b=Probe(mapper,cts.Token,operation.Feature,operation.Method);
                                a.GateAt=b.GateAt=step;
                                var x=ServiceConsolidationChecks.Execute(old,before,a);var y=ServiceConsolidationChecks.Execute(current,after!,b);
                                Check(!x.IsCompleted&&!y.IsCompleted&&a.Calls.Count==b.Calls.Count,"Auxiliary await boundary changed");a.Gate.SetResult(true);b.Gate.SetResult(true);Equal(await x,await y,"delayed");
                            }
                        }
                        async Task<ServiceConsolidationChecks.Outcome> Compare(string mode,int fail=0,int cancel=0)
                        {
                            using var cts=new CancellationTokenSource();if(cancel>0)cts.Cancel();
                            var a=Probe(mapper,cts.Token,operation.Feature,operation.Method);var b=Probe(mapper,cts.Token,operation.Feature,operation.Method);
                            a.Mode=b.Mode=mode;a.FailAt=b.FailAt=fail;a.CancelAt=b.CancelAt=cancel;
                            var x=await ServiceConsolidationChecks.Execute(old,before,a);
                            var y=await ServiceConsolidationChecks.Execute(current,after!,b);
                            Equal(x,y,mode+fail+cancel);return y;
                        }
                        void Equal(ServiceConsolidationChecks.Outcome a,ServiceConsolidationChecks.Outcome b,string mode)
                        {
                            Check(Normalize(a,operation.Feature,operation.Method)==Normalize(b,operation.Feature,operation.Method),"Auxiliary before/after mismatch: "+operation.Feature+"."+operation.Method+"/"+mode+"\n"+JsonSerializer.Serialize(a)+"\n"+JsonSerializer.Serialize(b));scenarios++;
                        }
                    }
                }
            }
        }
        finally
        {
            Environment.CurrentDirectory=previous;
            if(!Path.GetFullPath(scratch).StartsWith(Path.GetFullPath(Path.GetTempPath()),StringComparison.OrdinalIgnoreCase))throw new Exception("Unexpected scratch location");
            Directory.Delete(scratch,true);
        }
        Check(signatures==38,"Auxiliary operation coverage shrank");
        Check(knownLegacyMappings==1,"Known legacy Document update mapping failure changed");
        Console.WriteLine("KNOWN LEGACY DEFECT: DocumentUpdateDto -> Document mapping is absent in both versions; this is an expected failure comparison, not a successful update.");
        Console.WriteLine($"PASS: {checks} auxiliary service checks; 10 DI scopes, {signatures} operations, {scenarios} before/after scenarios; file bytes, errors, validation and async ordering.");
        return checks;
    }

    internal static ServiceProbe Probe(IMapper mapper,CancellationToken token,string feature,string operation)
    {
        var state=new ServiceProbe(mapper,token){CriticalContext=true};
        state.CallObserver=(_,_,args)=>
        {
            foreach(var doc in args.OfType<Aban360.BlobPool.Domain.Features.Taxonomy.Entities.Document>())
                if(!state.Tracked.Contains(doc))state.Tracked.Add(doc);
        };
        IFormFile File()=>new AuxiliaryFormFile(state,feature is "MeterApkLog" or "AssessmentLog"?"sample.txt":feature=="MeterApkFile"?"sample.apk":"sample-document.pdf");
        state.ArgumentFactory=p=>
        {
            if(p.ParameterType==typeof(IFormFile))return File();
            if(p.ParameterType==typeof(string))return p.Name=="fileName"?"sample.txt":p.Name=="version"?"1.2.3":"عنوان آزمایشی";
            var result=ServiceConsolidationChecks.Sample(p.ParameterType,tag:p.Name??"input");
            if(result is not null)
                foreach(var prop in result.GetType().GetProperties().Where(v=>v.SetMethod?.IsPublic==true&&v.GetIndexParameters().Length==0))
                {
                    if(prop.PropertyType==typeof(IFormFile))prop.SetValue(result,File());
                    if(prop.Name=="DueDateJalali")prop.SetValue(result,state.Mode=="invalid-date"?"invalid":"1405/08/27");
                    if(prop.Name=="SmsText")prop.SetValue(result,"متن آزمایشی {BillId}؛ فقط برای مقایسه");
                    var number=prop.Name switch{"GroupId"=>11,"SmsTypeId"=>22,"StepOrder"=>3,"DueDay"=>7,"FirstFlowId"=>71,"SmsCount"=>4,"SmsTemplateId"=>42,_=>0};
                    if(number>0)prop.SetValue(result,Convert.ChangeType(number,Nullable.GetUnderlyingType(prop.PropertyType)??prop.PropertyType));
                }
            return result;
        };
        state.ResultTransform=(contract,method,args,result)=>
        {
            if(feature=="MeterApkFile"&&state.Mode=="inactive"&&result is not null)
                result.GetType().GetProperty("IsActive")?.SetValue(result,false);
            if(feature is "SmsType" or "SmsStateGroup"&&method.Name=="Get"&&args.FirstOrDefault() is string)return state.Mode=="duplicate"?result:null;
            if(feature=="MeterApkFile"&&operation=="CreateAsync"&&method.Name=="GetValid"&&args.FirstOrDefault() is string)return state.Mode=="duplicate"?result:null;
            if(state.Mode=="missing"&&result is not null&&!result.GetType().IsValueType&&result is not string&&result is not IEnumerable)return null;
            return result;
        };
        return state;
    }

    static string Normalize(ServiceConsolidationChecks.Outcome outcome,string feature,string operation)
    {
        if(feature=="ServerInfo"&&outcome.Error is null)
        {
            var live=JsonNode.Parse(outcome.Result)!;
            if(operation=="GetOsInfo")
            {
                foreach(string key in new[]{"SystemDateTime","ElapsedDateTime"})
                {
                    if(string.IsNullOrWhiteSpace(live[key]?.GetValue<string>()))throw new Exception("Missing live OS information");
                    live[key]="current-os-"+key;
                }
            }
            else
                foreach(var drive in live.AsArray())
                    foreach(string key in new[]{"FreeSpaceGb","UsedSpaceGb","AvaialableSpaceGb","ReservedSpaceMb","FreePercent","UsedPercent"})
                    {
                        if(drive?[key] is not JsonValue v||!v.TryGetValue<double>(out var number)||!double.IsFinite(number)||number<0)throw new Exception("Invalid live disk information");
                        drive[key]="live-disk-"+key;
                    }
            outcome=outcome with{Result=live.ToJsonString()};
        }
        if(feature=="Document"&&operation is "CreateAsync" or "UpdateAsync")
        {
            var replacements=new List<(string Value,string Marker)>();
            var nodes=JsonNode.Parse(outcome.Mutations)!.AsArray().Where(n=>n is JsonObject o&&o.ContainsKey("FileRowId")
                &&DateTime.TryParse(o["CreatedDateTime"]?.GetValue<string>(),out var timestamp)&&Math.Abs((DateTime.Now-timestamp).TotalSeconds)<60);
            foreach(var doc in nodes)
                foreach(string key in new[]{"Id","FileRowId","CreatedDateTime"})
                    if((key!="Id"||operation=="CreateAsync")&&doc![key] is JsonValue value&&value.TryGetValue<string>(out var text))
                    {
                        if(key=="CreatedDateTime"&&DateTime.TryParse(text,out var stamp)&&Math.Abs((DateTime.Now-stamp).TotalSeconds)>60)continue;
                        if(key!="CreatedDateTime"&&(!Guid.TryParse(text,out var guid)||guid==Guid.Empty))throw new Exception("Missing generated document identity");
                        replacements.Add((text,"generated-document-"+key));
                    }
            string Clean(string text)
            {
                foreach(var replacement in replacements)
                {
                    text=text.Replace(replacement.Value,replacement.Marker,StringComparison.Ordinal);
                    text=text.Replace(JsonSerializer.Serialize(replacement.Value)[1..^1],replacement.Marker,StringComparison.Ordinal);
                }
                return text;
            }
            outcome=outcome with{Result=Clean(outcome.Result),Calls=outcome.Calls.Select(Clean).ToArray(),Mutations=Clean(outcome.Mutations)};
        }
        return JsonSerializer.Serialize(outcome);
    }
}

sealed class AuxiliaryFormFile(ServiceProbe probe,string fileName):IFormFile
{
    public string ContentType=>"application/octet-stream";public string ContentDisposition=>"";public IHeaderDictionary Headers{get;}=new HeaderDictionary();public long Length=>4;public string Name=>"file";public string FileName=>fileName;
    public void CopyTo(Stream target)=>target.Write(new byte[]{1,2,3,4});
    public Task CopyToAsync(Stream target,CancellationToken cancellationToken=default)=>probe.CopyFileAsync(target,cancellationToken);
    public Stream OpenReadStream()=>new MemoryStream(new byte[]{1,2,3,4});
}

using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

static class Management20Checks
{
    public static async Task RunFocused()
    {
        var registrations=new ServiceCollection();registrations.AddLogging();
        Aban360.UserPool.Application.Extensions.ConfigureServices.AddUserPoolApplicationInjections(registrations);
        Aban360.ClaimPool.Application.Extentions.ConfigureServices.AddClaimPoolApplicationInjections(registrations);
        Aban360.OldCalcPool.Application.Extentions.ConfigureServices.AddOldCalcPoolApplicationInjections(registrations);
        using var provider=registrations.BuildServiceProvider();var mapper=provider.GetRequiredService<IMapper>();
        int checks=0,scenarios=0,signatures=0;var legacy=new List<string>();
        void Check(bool ok,string message){if(!ok)throw new InvalidOperationException(message);checks++;}
        var operations=ApplicationServiceBaseline.ManagementOperations;
        Check(operations.Length==75&&operations.Select(o=>o.Service).Distinct().Count()==20,"Incomplete management selection");
        foreach(var group in operations.GroupBy(o=>o.Service))
        {
            var assembly=Assembly.Load(group.First().Assembly);var contract=assembly.GetType(group.Key,true)!;
            var current=assembly.GetTypes().Single(t=>t.IsClass&&!t.IsAbstract&&contract.IsAssignableFrom(t));
            var registration=registrations.Where(d=>d.ServiceType==contract).ToArray();
            Check(registration.Length==1&&registration[0].ImplementationType==current&&registration[0].Lifetime==ServiceLifetime.Scoped,"Management DI mismatch: "+group.Key);
            IServiceCollection local=new ServiceCollection();local.Add(registration[0]);var deps=Probe(mapper,CancellationToken.None,group.First().Feature,"");
            foreach(var p in current.GetConstructors().Single().GetParameters())local.AddScoped(p.ParameterType,_=>deps.Proxy(p.ParameterType));
            using var container=local.BuildServiceProvider(new ServiceProviderOptions{ValidateScopes=true});
            using var scopeA=container.CreateScope();using var scopeB=container.CreateScope();
            var instance=scopeA.ServiceProvider.GetRequiredService(contract);
            Check(ReferenceEquals(instance,scopeA.ServiceProvider.GetRequiredService(contract))&&!ReferenceEquals(instance,scopeB.ServiceProvider.GetRequiredService(contract)),"Scoped service lifetime changed");
            foreach(var op in group)
            {
                var old=typeof(Management20Checks).Assembly.GetType(op.Original,true)!;
                foreach(var before in old.GetMethods(BindingFlags.Instance|BindingFlags.Public|BindingFlags.DeclaredOnly).Where(m=>m.Name==op.OldMethod||m.Name=="HandleByDictionary"))
                {
                    string methodName=before.Name=="HandleByDictionary"?"GetAllDictionaryAsync":op.Method;
                    var after=current.GetMethod(methodName,before.GetParameters().Select(p=>p.ParameterType).ToArray())!;
                    Check(after is not null&&before.ReturnType==after.ReturnType,"Management signature changed: "+op.Feature+"."+op.Method);signatures++;
                    bool sql=old.BaseType?.Name=="AbstractBaseConnection";
                    // Valid transactional paths execute through the recording provider below.
                    if(sql)
                    {
                        if(old.GetConstructors().Single().GetParameters().Any(p=>p.ParameterType.IsGenericType&&p.ParameterType.GetGenericTypeDefinition()==typeof(FluentValidation.IValidator<>)))
                            Check((await Compare("invalid-validation")).Error is not null,"Invalid input reached SQL");
                        await Compare("success",fail:1);await Compare("success",cancel:1);
                        if(op.Method is "InsertAsync"&&op.Feature is "UsageGroup1" or "UsageGroup2")await Compare("duplicate");
                        continue;
                    }
                    var success=await Compare("success");
                    if(success.Error is not null){legacy.Add(op.Feature+"."+op.Method+": "+success.Error);Console.WriteLine("BASELINE FIXTURE LIMIT: "+legacy.Last());}
                    if(op.Feature=="UserPassword")
                    {
                        string hash=await Aban360.Common.Extensions.SecurityOperations.GetSha512Hash(op.Method=="ChangePasswordAsync"?"sample-Password":"sample-Mobile");
                        Check(success.Error is null&&success.Mutations.Contains(JsonSerializer.Serialize(hash)[1..^1]),"Password hashing/input changed");
                    }
                    if(op.Feature=="UserToken"&&op.Method=="CreateAsync")
                    {
                        Check(success.Error is null,"Token creation fixture failed: "+success.Error);
                        string call=success.Calls.Single(c=>c.StartsWith("Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts.ITokenStoreCommandService.Add("));
                        var token=JsonNode.Parse(call[(call.IndexOf('(')+1)..^1])!;
                        Check(token["AccessTokenHash"]!.GetValue<string>()==await Aban360.Common.Extensions.SecurityOperations.GetSha256Hash("sample-AccessToken")&&token["RefreshTokenIdHash"]!.GetValue<string>()==await Aban360.Common.Extensions.SecurityOperations.GetSha256Hash("sample-RefreshTokenSerial"),"Token hashes changed");
                        Check(Math.Abs((DateTime.Parse(token["AccessTokenExpiresDateTime"]!.GetValue<string>())-DateTime.Now).TotalMinutes-13)<0.1&&Math.Abs((DateTime.Parse(token["RefreshTokenExpiresDateTime"]!.GetValue<string>())-DateTime.Now).TotalMinutes-47)<0.1,"Token expiration settings changed");
                        await Compare("blank-refresh-source");
                    }
                    if(op.Feature=="UserToken"&&op.Method=="FindByRefreshAsync")
                        Check((await Compare("invalid-refresh")).Calls.Length==1,"Invalid refresh token reached persistence");
                    await Compare("empty");await Compare("missing");
                    if(success.Calls.Any(c=>c.Contains("FluentValidation.IValidator")))await Compare("invalid-validation");
                    for(int step=1;step<=success.Calls.Length;step++)
                    {
                        await Compare("success",fail:step);
                        if(success.AsyncSteps.Contains(step))await Compare("success",cancel:step);
                    }
                    foreach(int step in success.AsyncSteps)
                    {
                        using var cts=new CancellationTokenSource();var a=Probe(mapper,cts.Token,op.Feature,op.Method);var b=Probe(mapper,cts.Token,op.Feature,op.Method);
                        a.GateAt=b.GateAt=step;
                        var x=ServiceConsolidationChecks.Execute(old,before,a);var y=ServiceConsolidationChecks.Execute(current,after!,b);
                        Check(!x.IsCompleted&&!y.IsCompleted&&a.Calls.Count==b.Calls.Count,"Management await boundary changed: "+op.Feature+"."+op.Method);
                        a.Gate.SetResult(true);b.Gate.SetResult(true);Equal(await x,await y,"delayed");
                    }
                    async Task<ServiceConsolidationChecks.Outcome> Compare(string mode,int fail=0,int cancel=0)
                    {
                        using var cts=new CancellationTokenSource();if(cancel>0)cts.Cancel();
                        var a=Probe(mapper,cts.Token,op.Feature,op.Method);var b=Probe(mapper,cts.Token,op.Feature,op.Method);
                        a.Mode=b.Mode=mode;a.FailAt=b.FailAt=fail;a.CancelAt=b.CancelAt=cancel;
                        var x=await ServiceConsolidationChecks.Execute(old,before,a);var y=await ServiceConsolidationChecks.Execute(current,after!,b);
                        Equal(x,y,mode+fail+cancel);return y;
                    }
                    void Equal(ServiceConsolidationChecks.Outcome a,ServiceConsolidationChecks.Outcome b,string mode)
                    {
                        Check(Normalize(a,op.Feature)==Normalize(b,op.Feature),"Management behavior changed: "+op.Feature+"."+op.Method+"/"+mode+"\n"+JsonSerializer.Serialize(a)+"\n"+JsonSerializer.Serialize(b));scenarios++;
                    }
                }
            }
        }
        Check(signatures==77,"Management operation coverage shrank");
        Check(legacy.Count==0,"A successful operation is still covered only as a fixture failure: "+string.Join("; ",legacy));
        checks+=await Management20TransactionChecks.Run(mapper);
        Console.WriteLine($"PASS: {checks} management checks; 20 scoped services, {signatures} operations, {scenarios} dependency scenarios; no real SQL or SMS invoked. Fixture limits: {legacy.Count}.");
    }

    internal static ServiceProbe Probe(IMapper mapper,CancellationToken token,string feature,string operation)
    {
        var state=new ServiceProbe(mapper,token){CriticalContext=true};
        state.ArgumentFactory=p=>
        {
            if(p.ParameterType==typeof(Gridify.GridifyQuery))return new Gridify.GridifyQuery{Page=1,PageSize=10};
            if(p.ParameterType==typeof(Aban360.UserPool.Domain.Features.Auth.Dto.Queries.JwtTokenData))
                return new Aban360.UserPool.Domain.Features.Auth.Dto.Queries.JwtTokenData{UserId=Guid.Parse("22222222-2222-2222-2222-222222222222"),AccessToken="sample-AccessToken",RefreshToken="sample-RefreshToken",RefreshTokenSerial="sample-RefreshTokenSerial",Claims=[]};
            var sample=ServiceConsolidationChecks.Sample(p.ParameterType,0,p.Name??"input");
            if(state.Mode=="blank-refresh-source"&&p.Name=="refreshTokenSourceSerial")return "";
            if(sample is not null)
            {
                var date=sample.GetType().GetProperty("MeterChangeDateJalali");
                if(date?.CanWrite==true)
                {
                    var calendar=new System.Globalization.PersianCalendar();var now=DateTime.Now;
                    date.SetValue(sample,$"{calendar.GetYear(now):0000}/{calendar.GetMonth(now):00}/{calendar.GetDayOfMonth(now):00}");
                }
            }
            return sample;
        };
        state.ResultTransform=(contract,method,args,result)=>
        {
            if(method.Name=="Exists")return state.Mode=="duplicate";
            if(feature is "UsageGroup1" or "UsageGroup2"&&operation=="InsertAsync"&&method.Name=="Get"&&args.FirstOrDefault() is string)return state.Mode=="duplicate"?result:null;
            if(contract.Name=="IZoneCountQueryAddhoc"&&method.Name=="GetCount")return ((IEnumerable)args[0]!).Cast<object>().Count();
            if(feature is "UsageGroup1" or "UsageGroup2"&&operation=="RemoveAsync"&&result is IList children&&children.Count>1)
                while(children.Count>1)children.RemoveAt(children.Count-1);
            void Personnel(object? value)
            {
                if(value is Aban360.ClaimPool.Domain.Features.Land.Dto.Queries.ConCompanyPersonnelGetDto company)
                    company.ConCompanyPersonnel=JsonSerializer.Serialize(new[]{
                        new Aban360.ClaimPool.Domain.Features.Land.Dto.Queries.ConCompanyPersonnelDetailOutputDto{Id=Guid.Parse("22222222-2222-2222-2222-222222222222"),FullName="Active member",MobileNumber="09120000001",PersonnelCode="A",RemovedBy=null},
                        new Aban360.ClaimPool.Domain.Features.Land.Dto.Queries.ConCompanyPersonnelDetailOutputDto{Id=Guid.Parse("33333333-3333-3333-3333-333333333333"),FullName="Removed member",MobileNumber="09120000002",PersonnelCode="B",RemovedBy=Guid.Parse("22222222-2222-2222-2222-222222222222")}});
            }
            Personnel(result);
            if(feature=="SubscriptionAssignment"&&result?.GetType().Name=="CustomerLocationDto")
            {result.GetType().GetProperty("X")!.SetValue(result,"35.7");result.GetType().GetProperty("Y")!.SetValue(result,"51.4");}
            if(result is IEnumerable sequence&&result is not string)
                foreach(var item in sequence)
                {
                    Personnel(item);
                    if(item is Aban360.UserPool.Domain.Features.Auth.Entities.Role role)role.DefaultClaims="[17]";
                    if(item is Aban360.UserPool.Domain.Features.Auth.Dto.Queries.UserQueryDto userInfo)userInfo.DefaultZoneId="17";
                }
            if(result is Aban360.UserPool.Domain.Features.Auth.Entities.User user)
                foreach(var claim in user.UserClaims)claim.ClaimValue="17";
            if(result is Aban360.ClaimPool.Domain.Features.Request.Dto.Queries.TrackingOutputDto tracking)tracking.StatusId=0;
            if(result is IEnumerable items&&result is not string)
                foreach(var item in items)
                {
                    var registered=item?.GetType().GetProperty("IsRegistered");if(registered?.CanWrite==true)registered.SetValue(item,false);
                }
            return result;
        };
        state.SynchronousResultTransform=(contract,method,args,result)=>
        {
            if(contract.Name=="IUserQueryService"&&method.Name=="GetQuery")return new ManagementAsyncEnumerable<Aban360.UserPool.Domain.Features.Auth.Entities.User>(((IEnumerable)result!).Cast<Aban360.UserPool.Domain.Features.Auth.Entities.User>());
            if(contract.Name=="ITokenFactoryService"&&method.Name=="GetRefreshTokenSerial")return state.Mode=="invalid-refresh"?"":"sample-refresh-serial";
            if(result is Aban360.UserPool.Domain.Features.Auth.Dto.Queries.BearerTokenOptions options)
            {options.AccessTokenExpirationMinutes=13;options.RefreshTokenExpirationMinutes=47;}
            return result;
        };
        return state;
    }

    // Only wall-clock values and identifiers generated by these exact legacy operations
    // are normalized. Input IDs, actor IDs, dates from callers and SQL remain exact.
    internal static string Normalize(object outcome,string feature)
    {
        string text=JsonSerializer.Serialize(outcome);
        var now=DateTime.Now;
        text=Regex.Replace(text,@"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|[+-]\d{2}:\d{2})?",m=>
            DateTime.TryParse(m.Value,out var dt)&&Math.Abs((dt-now).TotalMinutes)<60?"current-generated-time":m.Value);
        if(feature is "UserManagement" or "ConCompanyPersonnel")
            text=Regex.Replace(text,@"[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-4[a-fA-F0-9]{3}-[89abAB][a-fA-F0-9]{3}-[a-fA-F0-9]{12}","generated-operation-group");
        if(feature=="UserManagement")
            text=Regex.Replace(text,@"(SerialNumber\\u0022:\\u0022)[a-fA-F0-9]{12}4[a-fA-F0-9]{3}[89abAB][a-fA-F0-9]{15}(\\u0022)","$1generated-user-serial$2");
        if(feature=="RequestSubscription")
            text=Regex.Replace(text,@"(TrackNumber\\u0022:\\u0022)(\d+)(\\u0022)",m=>
            {
                if(!int.TryParse(m.Groups[2].Value,out int number)||number<1||number>1000000||number.ToString("D6")!=m.Groups[2].Value)
                    throw new InvalidOperationException("Generated request tracking number lost its range/format");
                return m.Groups[1].Value+"generated-request-track"+m.Groups[3].Value;
            });
        return text;
    }
}


using System.Reflection;
using System.Text.Json;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

static class Critical10Checks
{
    public static async Task RunFocused()
    {
        var registrations=new ServiceCollection();registrations.AddLogging();
        Aban360.UserPool.Application.Extensions.ConfigureServices.AddUserPoolApplicationInjections(registrations);
        Aban360.SystemPool.Application.Extenstions.ConfigureServices.AddSystemPoolApplicationInjections(registrations);
        Aban360.OldCalcPool.Application.Extentions.ConfigureServices.AddOldCalcPoolApplicationInjections(registrations);
        using var provider=registrations.BuildServiceProvider();
        var mapper=provider.GetRequiredService<IMapper>();
        int checks=await Run(mapper,registrations);
        checks+=await Critical10TransactionChecks.Run(mapper);
        Console.WriteLine($"PASS: {checks} focused critical consolidation checks; no production connections or external services used.");
    }
    public static async Task<int> Run(IMapper mapper, IServiceCollection registrations)
    {
        int checks=0,scenarios=0,signatures=0,sqlGuards=0;
        void Check(bool ok,string message){if(!ok)throw new InvalidOperationException(message);checks++;}
        var operations=ApplicationServiceBaseline.CriticalOperations;
        Check(operations.Length==49&&operations.Select(o=>o.Service).Distinct().Count()==10,"Critical selection incomplete");
        foreach(var group in operations.GroupBy(o=>o.Service))
        {
            var assembly=Assembly.Load(group.First().Assembly);
            var contract=assembly.GetType(group.Key,true)!;
            var current=assembly.GetTypes().Single(t=>t.IsClass&&!t.IsAbstract&&contract.IsAssignableFrom(t));
            var registration=registrations.Where(d=>d.ServiceType==contract).ToArray();
            Check(registration.Length==1&&registration[0].ImplementationType==current&&registration[0].Lifetime==ServiceLifetime.Scoped,"Critical DI changed: "+group.Key);
            IServiceCollection local=new ServiceCollection();local.Add(registration[0]);
            var dependencies=State(CancellationToken.None);
            foreach(var parameter in current.GetConstructors().Single().GetParameters())
                local.AddScoped(parameter.ParameterType,_=>dependencies.Proxy(parameter.ParameterType));
            using var provider=local.BuildServiceProvider(new ServiceProviderOptions{ValidateScopes=true});
            using var scopeA=provider.CreateScope();using var scopeB=provider.CreateScope();
            var instance=scopeA.ServiceProvider.GetRequiredService(contract);
            Check(ReferenceEquals(instance,scopeA.ServiceProvider.GetRequiredService(contract))&&!ReferenceEquals(instance,scopeB.ServiceProvider.GetRequiredService(contract)),"Critical scope lifetime changed: "+group.Key);
            foreach(var operation in group)
            {
                var old=typeof(Critical10Checks).Assembly.GetType(operation.Original,true)!;
                foreach(var before in old.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly).Where(m=>m.Name==operation.OldMethod))
                {
                    var after=current.GetMethod(operation.Method,before.GetParameters().Select(p=>p.ParameterType).ToArray())!;
                    Check(after is not null&&after.ReturnType==before.ReturnType,"Critical signature changed: "+operation.Feature+"."+operation.Method);
                    signatures++;
                    bool sql=before.ReturnType==typeof(Task)&&old.BaseType?.Name=="AbstractBaseConnection";
                    if(sql)
                    {
                        // Valid inputs are exercised by the isolated transaction harness, never a real connection.
                        bool validates=old.GetConstructors().Single().GetParameters().Any(p=>p.ParameterType.IsGenericType&&p.ParameterType.GetGenericTypeDefinition()==typeof(FluentValidation.IValidator<>));
                        if(validates)
                        {
                            var invalid=await Compare("invalid-validation");
                            Check(invalid.Error?.Contains("CustomValidationException")==true&&invalid.Calls.Length==1,"Invalid tariff input reached persistence: "+operation.Feature);
                        }
                        await Compare("success",fail:1);
                        await Compare("success",cancel:1);
                        sqlGuards++;
                    }
                    else
                    {
                        var success=await Compare("success");
                        Check(success.Error is null,"Success fixture failed: "+operation.Feature+"."+operation.Method+" "+success.Error);
                        await Compare("empty");
                        if(operation.Feature=="Role"&&operation.Method=="DeleteAsync")
                            Check((await Compare("protected-role")).Error?.Contains("BaseException")==true,"Existing protected role rejection changed");
                        if(success.Calls.Any(c=>c.Contains("FluentValidation.IValidator")))await Compare("invalid-validation");
                        for(int step=1;step<=success.Calls.Length;step++)
                        {
                            await Compare("success",fail:step);
                            if(success.AsyncSteps.Contains(step))await Compare("success",cancel:step);
                        }
                        foreach(int step in success.AsyncSteps)
                        {
                            using var cancel=new CancellationTokenSource();
                            var a=State(cancel.Token);a.GateAt=step;
                            var b=State(cancel.Token);b.GateAt=step;
                            var pendingA=ServiceConsolidationChecks.Execute(old,before,a);
                            var pendingB=ServiceConsolidationChecks.Execute(current,after!,b);
                            Check(!pendingA.IsCompleted&&!pendingB.IsCompleted&&a.Calls.Count==b.Calls.Count,"Await boundary changed: "+operation.Feature+"."+operation.Method);
                            a.Gate.SetResult(true);b.Gate.SetResult(true);
                            Equal(await pendingA,await pendingB,"delayed completion");
                        }
                    }
                    async Task<ServiceConsolidationChecks.Outcome> Compare(string mode,int fail=0,int cancel=0)
                    {
                        using var cts=new CancellationTokenSource();if(cancel>0)cts.Cancel();
                        var a=State(cts.Token);var b=State(cts.Token);
                        a.Mode=b.Mode=mode;a.FailAt=b.FailAt=fail;a.CancelAt=b.CancelAt=cancel;
                        var x=await ServiceConsolidationChecks.Execute(old,before,a);
                        var y=await ServiceConsolidationChecks.Execute(current,after!,b);
                        Equal(x,y,mode+"/"+fail+"/"+cancel);return y;
                    }
                    void Equal(ServiceConsolidationChecks.Outcome a,ServiceConsolidationChecks.Outcome b,string mode)
                    {
                        Check(JsonSerializer.Serialize(a)==JsonSerializer.Serialize(b),"Critical before/after mismatch: "+operation.Feature+"."+operation.Method+"/"+mode+"\n"+JsonSerializer.Serialize(a)+"\n"+JsonSerializer.Serialize(b));scenarios++;
                    }
                }
            }
        }
        Check(signatures==52&&sqlGuards==14,"Critical overload/SQL coverage changed");
        Console.WriteLine($"PASS: {checks} critical checks; 10 DI registrations, {signatures} method signatures, {scenarios} before/after scenarios; {sqlGuards} SQL input/failure guards; no real database access.");
        return checks;
        ServiceProbe State(CancellationToken token)=>new(mapper,token){CriticalContext=true};
    }
}

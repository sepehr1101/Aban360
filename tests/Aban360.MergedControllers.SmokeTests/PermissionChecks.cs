using System.Reflection;
using System.Security.Claims;
using Aban360.Api.Filters;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

static class PermissionChecks
{
    public static async Task<int> Run(Contract[] contracts, ControllerActionDescriptor[] actions)
    {
        int checks = 0;
        foreach (var expected in contracts.Where(c => c.Project == "Api" && c.Permission != null))
        {
            var action = actions.Single(a => a.AttributeRouteInfo?.Template == expected.Route && a.ActionName == expected.Action);
            var factory = action.FilterDescriptors.Select(f => f.Filter).OfType<TypeFilterAttribute>()
                .Single(f => f.ImplementationType == typeof(EndpointAuthorizationFilter));
            foreach (bool allowed in new[] { false, true })
            {
                var query = DispatchProxy.Create<IUserClaimQueryService, ClaimProbe>();
                var probe = (ClaimProbe)(object)query; probe.Allowed = allowed;
                using var services = new ServiceCollection().AddSingleton(query).BuildServiceProvider();
                var filter = (IAsyncAuthorizationFilter)factory.CreateInstance(services);
                var http = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, "22222222-2222-2222-2222-222222222222") }, "regression"))
                };
                http.SetEndpoint(new Endpoint(_ => Task.CompletedTask, new EndpointMetadataCollection(action.EndpointMetadata.Concat(new object[] { action })), expected.Route));
                var context = new AuthorizationFilterContext(new ActionContext(http, new RouteData(), action), new List<IFilterMetadata>());
                await filter.OnAuthorizationAsync(context);
                if (expected.Anonymous)
                {
                    if (probe.Calls != 0 || context.Result != null) throw new Exception("Anonymous permission behavior changed: " + expected.Route);
                    checks++;
                }
                else
                {
                    if (probe.Calls != 1 || probe.Key != expected.Permission || probe.UserId != Guid.Parse("22222222-2222-2222-2222-222222222222") || probe.ClaimType != "Endpoint")
                        throw new Exception("Permission lookup changed: " + expected.Route);
                    if (allowed ? context.Result != null : context.Result is not ObjectResult { StatusCode: 403 })
                        throw new Exception("Permission allow/deny changed: " + expected.Route);
                    checks += 2;
                }
            }
        }
        Console.WriteLine($"PASS: {checks} permission checks using the production authorization filter and isolated claim queries.");
        return checks;
    }
}
public class ClaimProbe : DispatchProxy
{
    public bool Allowed; public int Calls; public Guid UserId; public string? ClaimType; public string? Key;
    protected override object? Invoke(MethodInfo? method, object?[]? values)
    {
        if (method?.Name != "HasValue") throw new Exception("Unexpected claim service call: " + method?.Name);
        Calls++; UserId = (Guid)values![0]!; ClaimType = values[1]!.ToString(); Key = (string)values[2]!;
        return Task.FromResult(Allowed);
    }
}

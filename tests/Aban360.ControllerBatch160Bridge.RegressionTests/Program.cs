using System.Reflection;
using System.Text.Json;
using Aban360.BrdigeApi.Controllers.V1;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
var groups=JsonSerializer.Deserialize<Group[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"Groups.json")))!;
var api=typeof(BaseController).Assembly;
var originals=Assembly.GetExecutingAssembly().GetTypes().Where(t=>t.Namespace?.Contains(".BeforeBatch160Bridge.")==true&&typeof(ControllerBase).IsAssignableFrom(t)).ToArray();
var current=groups.Select(g=>api.GetType(g.Controller,true)!).ToArray();
ControllerActionDescriptor[] Discover(Type[] types){var s=new ServiceCollection();s.AddLogging();s.AddControllers().ConfigureApplicationPartManager(m=>{m.ApplicationParts.Clear();foreach(var a in types.Select(t=>t.Assembly).Distinct())m.ApplicationParts.Add(new AssemblyPart(a));m.FeatureProviders.Add(new SelectTypes(types));});using var p=s.BuildServiceProvider();return p.GetRequiredService<IActionDescriptorCollectionProvider>().ActionDescriptors.Items.OfType<ControllerActionDescriptor>().ToArray();}
var old=Discover(originals);var now=Discover(current);var all=Discover(api.GetTypes().Where(t=>!t.IsAbstract&&typeof(ControllerBase).IsAssignableFrom(t)).ToArray());int checks=0;
void Check(bool b,string msg){if(!b)throw new Exception(msg);checks++;}
string[] Verbs(ControllerActionDescriptor a)=>a.ActionConstraints?.OfType<HttpMethodActionConstraint>().SelectMany(c=>c.HttpMethods).Distinct().Order().ToArray()??[];
string Key(ControllerActionDescriptor a)=>a.AttributeRouteInfo?.Template+"|"+string.Join(",",Verbs(a));
Check(groups.Length==6,"Expected selected groups");Check(old.Length==now.Length,"Action count changed");
foreach(var a in old){var group=groups.Single(g=>g.Originals.Contains(a.ControllerTypeInfo.FullName!.Replace(".BeforeBatch160Bridge.",".")));var b=now.Single(n=>n.ControllerTypeInfo.FullName==group.Controller&&Key(n)==Key(a));Check(a.ActionName==b.ActionName,"Action name "+Key(a));
 string Param(ParameterInfo p)=>p.ParameterType.FullName+" "+p.Name+" "+p.IsOptional+" "+p.DefaultValue+" "+string.Join("|",p.GetCustomAttributes().Where(x=>x.GetType().Namespace!="System.Runtime.CompilerServices").Select(x=>x.GetType().FullName).Order());
 Check(a.MethodInfo.GetParameters().Select(Param).SequenceEqual(b.MethodInfo.GetParameters().Select(Param)),"Parameters "+Key(a));Check(a.MethodInfo.ReturnType==b.MethodInfo.ReturnType,"Return type "+Key(a)); string[] Metadata(MethodInfo m)=>m.GetCustomAttributesData().Where(c=>c.AttributeType.Namespace!="System.Runtime.CompilerServices" && c.AttributeType!=typeof(ActionNameAttribute) && c.AttributeType!=typeof(RouteAttribute) && !typeof(Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute).IsAssignableFrom(c.AttributeType)).Select(c=>c.AttributeType.FullName+":"+string.Join("|",c.ConstructorArguments)+":"+string.Join("|",c.NamedArguments.OrderBy(n=>n.MemberName))).Order().ToArray();
 Check(Metadata(a.MethodInfo).SequenceEqual(Metadata(b.MethodInfo)),"Action filters, authorization policy or request limits changed "+Key(a));
 string[] Responses(MethodInfo m)=>m.GetCustomAttributes<ProducesResponseTypeAttribute>().Select(r=>r.Type.FullName+":"+r.StatusCode).Order().ToArray();Check(Responses(a.MethodInfo).SequenceEqual(Responses(b.MethodInfo)),"Response "+Key(a));
 bool Attr<T>(ControllerActionDescriptor d)where T:Attribute=>d.MethodInfo.IsDefined(typeof(T),true)||d.ControllerTypeInfo.IsDefined(typeof(T),true);
 Check(Attr<AuthorizeAttribute>(a)==Attr<AuthorizeAttribute>(b)&&Attr<AllowAnonymousAttribute>(a)==Attr<AllowAnonymousAttribute>(b),"Authorization "+Key(a));
 Check(a.MethodInfo.GetCustomAttribute<ConsumesAttribute>()?.ContentTypes.SequenceEqual(b.MethodInfo.GetCustomAttribute<ConsumesAttribute>()?.ContentTypes??[])??b.MethodInfo.GetCustomAttribute<ConsumesAttribute>()==null,"Consumes "+Key(a));
 foreach(var verb in Verbs(a))Check(all.Count(n=>string.Equals(n.AttributeRouteInfo?.Template,a.AttributeRouteInfo?.Template,StringComparison.OrdinalIgnoreCase)&&Verbs(n).Contains(verb))==1,"Ambiguous route "+Key(a));
}
foreach(var t in current){var c=t.GetConstructors().Single();var obj=c.Invoke(c.GetParameters().Select(p=>DispatchProxy.Create(p.ParameterType,typeof(NoCalls))).ToArray());Check(obj!=null,"Construction "+t.Name);}
Console.WriteLine($"PASS: {checks} checks; {groups.Length} controllers; {old.Length} actions; {old.Sum(a=>Verbs(a).Length)} HTTP/route combinations; no database access.");
public record Group(string Controller,string[] Originals);
public class NoCalls:DispatchProxy{protected override object? Invoke(MethodInfo? m,object?[]? args)=>throw new Exception("Unexpected dependency call "+m?.Name);}
class SelectTypes(Type[] types):IApplicationFeatureProvider<ControllerFeature>{public void PopulateFeature(IEnumerable<ApplicationPart> parts,ControllerFeature f){for(int i=f.Controllers.Count-1;i>=0;i--)if(!types.Contains(f.Controllers[i].AsType()))f.Controllers.RemoveAt(i);}}







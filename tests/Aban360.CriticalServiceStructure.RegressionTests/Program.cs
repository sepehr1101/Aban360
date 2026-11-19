using System.Text.Json;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// Compare executable syntax, including SQL/strings/numeric literals, against frozen sources.
// Only the explicitly recorded field/helper/entry-point names may differ.
var root=new DirectoryInfo(AppContext.BaseDirectory);
while(root is not null&&!File.Exists(Path.Combine(root.FullName,"Aban360.sln"))) root=root.Parent;
if(root is null) throw new Exception("Repository root not found");
Directory.SetCurrentDirectory(root.FullName);
if(args.Contains("--operations19")){Operations19Checks.Run(args.Contains("--verify-head"));return;}
const string fixtures="tests/Aban360.MergedControllers.SmokeTests/";
bool workflow=args.Contains("--workflow10");
bool management=args.Contains("--management20");
bool auxiliary=args.Contains("--auxiliary10");
string batch=workflow?"Workflow10":management?"Management20":auxiliary?"Auxiliary10":"Critical10";
var operations=JsonSerializer.Deserialize<Operation[]>(File.ReadAllText(fixtures+"ServiceConsolidation"+batch+".json"))!;
int checks=0,methods=0,transactions=0,consumerMethods=0;
void Check(bool condition,string message){if(!condition) throw new Exception(message);checks++;}
CompilationUnitSyntax Read(string path)=>CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetCompilationUnitRoot();
string Tokens(SyntaxNode node)=>string.Join("|",node.DescendantTokens().Select(t=>t.RawKind+":"+t.Text));
string GitHead(string path)
{
    var start=new ProcessStartInfo("git"){RedirectStandardOutput=true,RedirectStandardError=true,UseShellExecute=false,StandardOutputEncoding=System.Text.Encoding.UTF8,StandardErrorEncoding=System.Text.Encoding.UTF8};
    start.ArgumentList.Add("show");start.ArgumentList.Add("HEAD:"+path.Replace('\\','/'));
    using var process=Process.Start(start)!;
    string output=process.StandardOutput.ReadToEnd(),error=process.StandardError.ReadToEnd();process.WaitForExit();
    Check(process.ExitCode==0,"Cannot verify Git baseline: "+path+" "+error);
    return output;
}
Check(operations.Length==(workflow?31:management?75:auxiliary?38:49)&&operations.Select(o=>o.Service).Distinct().Count()==(management?20:10),"Incomplete frozen selection");
foreach(var group in operations.GroupBy(o=>o.Output))
{
    var after=Read(group.Key).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
    foreach(var op in group)
    {
        var before=Read(fixtures+"OriginalHandlers"+batch+"/"+Path.GetFileName(op.Source)).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
        if(args.Contains("--verify-head"))
        {
            var committed=CSharpSyntaxTree.ParseText(GitHead(op.Source)).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            Check(Tokens(committed).Replace("\r\n","\n")==Tokens(before).Replace("\r\n","\n"),"Frozen handler differs from committed source: "+op.Source);
        }
        var rename=new Rename(op.Names);
        foreach(var method in before.Members.OfType<MethodDeclarationSyntax>())
        {
            var expected=(MethodDeclarationSyntax)rename.Visit(method)!;
            if(workflow&&op.Source.EndsWith("/RequestAfterSaleHandler.cs"))expected=(MethodDeclarationSyntax)new ApprovedAfterSaleAwait().Visit(expected)!;
            var actual=after.Members.OfType<MethodDeclarationSyntax>().Single(m=>m.Identifier.ValueText==expected.Identifier.ValueText&&Tokens(m.ParameterList)==Tokens(expected.ParameterList));
            Check(Tokens(expected)==Tokens(actual),"Executable syntax changed: "+op.Feature+"."+method.Identifier);
            methods++;
            if(method.DescendantNodes().OfType<InvocationExpressionSyntax>().Any(i=>i.Expression.ToString().EndsWith(".BeginTransaction"))) transactions++;
        }
        foreach(var field in before.Members.OfType<FieldDeclarationSyntax>())
        {
            var expected=(FieldDeclarationSyntax)rename.Visit(field)!;
            var name=expected.Declaration.Variables.Single().Identifier.ValueText;
            var actual=after.Members.OfType<FieldDeclarationSyntax>().Single(f=>f.Declaration.Variables.Single().Identifier.ValueText==name);
            Check(Tokens(expected)==Tokens(actual),"Dependency type/initializer changed: "+op.Feature+"."+name);
        }
        foreach(var nested in before.Members.OfType<BaseTypeDeclarationSyntax>())
            Check(after.Members.OfType<BaseTypeDeclarationSyntax>().Any(n=>Tokens(n)==Tokens(new Rename(op.Names).Visit(nested)!)),"Nested type changed: "+op.Original);
        var oldCtor=before.Members.OfType<ConstructorDeclarationSyntax>().SingleOrDefault();
        var newCtor=after.Members.OfType<ConstructorDeclarationSyntax>().Single();
        var ctorNames=new Dictionary<string,string>(op.Names);
        foreach(var p in oldCtor?.ParameterList.Parameters??default)
            ctorNames[p.Identifier.ValueText]=newCtor.ParameterList.Parameters.Single(n=>n.Type!.ToString()==p.Type!.ToString()).Identifier.ValueText;
        foreach(var statement in oldCtor?.Body!.Statements??default)
            Check(newCtor.Body!.Statements.Any(s=>Tokens(s)==Tokens(new Rename(ctorNames).Visit(statement)!)),"Constructor assignment/guard lost: "+op.Original+" "+statement);
        if(oldCtor?.Initializer is not null) Check(Tokens(new Rename(ctorNames).Visit(oldCtor.Initializer)!)==Tokens(newCtor.Initializer!),"Base configuration changed: "+op.Feature);
        Check(!File.Exists(op.Source),"Legacy handler still in production: "+op.Source);
        if(management||workflow)Check(op.ContractSource is not null&&!File.Exists(op.ContractSource),"Legacy contract still in production: "+op.ContractSource);
    }
    Check(after.Members.OfType<MethodDeclarationSyntax>().Count()==group.Sum(o=>Read(fixtures+"OriginalHandlers"+batch+"/"+Path.GetFileName(o.Source)).DescendantNodes().OfType<MethodDeclarationSyntax>().Count()),"Extra/unreviewed service method: "+group.Key);
}
// A frozen consumer may predate later approved batches (e.g. Login's token service).
// Resolve all recorded renames while keeping this batch's selected bodies/counts fixed.
var consumerOperations=new[]{"Critical10","Auxiliary10","Management20","Workflow10","Operations19"}
    .SelectMany(name=>JsonSerializer.Deserialize<Operation[]>(File.ReadAllText(fixtures+"ServiceConsolidation"+name+".json"))!).ToArray();
var byContract=consumerOperations.ToDictionary(o=>o.OldContract.Split('.').Last());
bool chainedConsumer=false;
bool IsSelected(string type,ClassDeclarationSyntax cls)
{
    if(!byContract.TryGetValue(type.Split('.').Last(),out var operation))return false;
    if(chainedConsumer&&operation.Original.StartsWith("BeforeServiceOperations19."))return false;
    string ns=operation.OldContract[..operation.OldContract.LastIndexOf('.')];
    var usings=cls.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
    return usings.Any(u=>u.Name?.ToString().Replace("global::","")==ns&&(u.Alias is null&&!type.Contains('.')||u.Alias is not null&&type.StartsWith(u.Alias.Name+".",StringComparison.Ordinal)));
}
foreach(var file in Directory.GetFiles(fixtures+"OriginalConsumers"+batch,"*.cs"))
{
    var before=Read(file).DescendantNodes().OfType<ClassDeclarationSyntax>().First();
    var ns=before.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().First().Name.ToString().Replace("BeforeConsumers"+batch+".","");
    var project=Directory.GetDirectories(".","Aban*").Select(Path.GetFileName)
        .Where(p=>p is not null&&ns.StartsWith(p+".",StringComparison.Ordinal)).OrderByDescending(p=>p!.Length).First()!;
    var candidates=Directory.GetFiles(project,before.Identifier+".cs",SearchOption.AllDirectories).Where(p=>!p.Contains("\\obj\\")).ToArray();
    chainedConsumer=candidates.Length==0;
    // A later batch may have merged this consumer too. Its frozen pre-batch class
    // completes the proof: prior consumer -> frozen handler -> current service.
    var after=chainedConsumer
        ?Read(fixtures+"OriginalHandlersOperations19/"+before.Identifier+".cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single()
        :candidates.Select(Read).SelectMany(t=>t.DescendantNodes().OfType<ClassDeclarationSyntax>()).Single(c=>c.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().First().Name.ToString()==ns);
    var names=new Dictionary<string,string>();var calls=new Dictionary<string,string>();
    foreach(var f in before.Members.OfType<FieldDeclarationSyntax>())
        if(IsSelected(f.Declaration.Type.ToString(),before)&&byContract.TryGetValue(f.Declaration.Type.ToString().Split('.').Last(),out var op))
        {
            var contract=op.Service.Split('.').Last();var field=after.Members.OfType<FieldDeclarationSyntax>().Single(n=>n.Declaration.Type.ToString()==contract).Declaration.Variables.Single().Identifier.ValueText;
            foreach(var v in f.Declaration.Variables){names[v.Identifier.ValueText]=field;calls[v.Identifier.ValueText]=op.Method;if(op.Names.TryGetValue("HandleByDictionary",out var extra))calls[v.Identifier.ValueText+".HandleByDictionary"]=extra;}
        }
    var oldConstructor=before.Members.OfType<ConstructorDeclarationSyntax>().Single();
    var newConstructor=after.Members.OfType<ConstructorDeclarationSyntax>().Single();
    var constructorNames=new Dictionary<string,string>(names);
    foreach(var parameter in oldConstructor.ParameterList.Parameters)
    {
        string type=parameter.Type!.ToString();
        if(IsSelected(type,before)&&byContract.TryGetValue(type.Split('.').Last(),out var op))type=op.Service.Split('.').Last();
        var matches=newConstructor.ParameterList.Parameters.Where(p=>p.Type!.ToString()==type).ToArray();
        // Captcha retains two independently named UnitOfWork parameters from the old controller.
        var target=matches.Length==1?matches[0]:matches.Single(p=>p.Identifier.ValueText==parameter.Identifier.ValueText);
        constructorNames[parameter.Identifier.ValueText]=target.Identifier.ValueText;
    }
    var expectedConstructorStatements=oldConstructor.Body!.Statements.Select(s=>new Rename(constructorNames).Visit(s)!).Select(Tokens).Distinct().ToArray();
    Check(expectedConstructorStatements.SequenceEqual(newConstructor.Body!.Statements.Select(Tokens).Distinct()),"Consumer constructor assignments/guards/order changed: "+ns+"."+before.Identifier);
    var actualFieldNames=after.Members.OfType<FieldDeclarationSyntax>().SelectMany(f=>f.Declaration.Variables.Select(v=>v.Identifier.ValueText)).ToHashSet();
    var expectedFieldNames=before.Members.OfType<FieldDeclarationSyntax>().SelectMany(f=>f.Declaration.Variables.Select(v=>names.GetValueOrDefault(v.Identifier.ValueText,v.Identifier.ValueText))).ToHashSet();
    Check(actualFieldNames.SetEquals(expectedFieldNames),"Consumer dependency lost or added: "+ns+"."+before.Identifier);
    if(args.Contains("--verify-head")&&!management&&!workflow)
    {
        var currentPath=candidates.Single(p=>Read(p).DescendantNodes().OfType<ClassDeclarationSyntax>().Any(c=>c.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().First().Name.ToString()==ns));
        var committed=CSharpSyntaxTree.ParseText(GitHead(currentPath)).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
        Check(Tokens(committed)==Tokens(before),"Frozen consumer differs from committed source: "+currentPath);
    }
    string? approvedRoutePrefix=(ns,before.Identifier.ValueText) switch
    {
        ("Aban360.Api.Controllers.V1.OldCalcPool.Rule","ZaribCController")=>"v1/zarib-c",
        ("Aban360.Api.Controllers.V1.OldCalcPool.Rule","ZaribController")=>"v1/zarib",
        ("Aban360.Api.Controllers.V1.SystemPool.UserGuide","FaqController")=>"v1/faq",
        ("Aban360.Api.Controllers.V1.UserPool.Auth","CaptchaController")=>"v1/captcha",
        ("Aban360.Api.Controllers.V1.ClaimPool.Draft","RequestSubscriptionController") when management=>"v1/request-subscription",
        ("Aban360.Api.Controllers.V1.ClaimPool.People.Commands","CustomerInfoController") when management=>"v1/customer-info",
        ("Aban360.Api.Controllers.V1.MeterPool.Management","CounterStateController") when management=>"v1/counter-state",
        ("Aban360.BrdigeApi.Controllers.V1","ServiceLinkController") when management=>"v1/service-link",
        _=>null
    };
    if(workflow)approvedRoutePrefix=JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText(fixtures+"Workflow10RouteStyles.json"))!.GetValueOrDefault(ns+"."+before.Identifier.ValueText);
    var expectedAttributes=before.AttributeLists;
    if(approvedRoutePrefix is not null)
    {
        var argument=SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,SyntaxFactory.Literal(approvedRoutePrefix)));
        var route=SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Route"))
            .WithArgumentList(SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(argument)));
        expectedAttributes=expectedAttributes.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(route)));
    }
    Check(string.Join("|",expectedAttributes.Select(Tokens))==string.Join("|",after.AttributeLists.Select(Tokens)),"Controller metadata changed: "+before.Identifier);
    foreach(var method in before.Members.OfType<MethodDeclarationSyntax>())
    {
        var expected=new Rename(names).Visit(new CallRename(calls).Visit(method))!;
        if(approvedRoutePrefix is not null)expected=new RelativeRouteStyle(approvedRoutePrefix).Visit(expected)!;
        var actual=after.Members.OfType<MethodDeclarationSyntax>().Single(m=>m.Identifier.ValueText==method.Identifier.ValueText&&Tokens(m.ParameterList)==Tokens(method.ParameterList));
        Check(Tokens(expected)==Tokens(actual),"Consumer method/route/permission changed: "+ns+"."+before.Identifier+"."+method.Identifier);
        consumerMethods++;
    }
    Check(before.Members.OfType<MethodDeclarationSyntax>().Count()==after.Members.OfType<MethodDeclarationSyntax>().Count(),"Consumer method added/lost");
    if(management||workflow)
    {
        var routes=after.DescendantNodes().OfType<AttributeSyntax>().Where(a=>a.Name.ToString()=="Route").SelectMany(a=>a.DescendantNodes().OfType<LiteralExpressionSyntax>()).Select(a=>a.Token.ValueText).ToArray();
        Check(!routes.Any(r=>r.Contains("excel",StringComparison.OrdinalIgnoreCase)||r.Contains("sti",StringComparison.OrdinalIgnoreCase)),"Excluded report route entered the batch: "+ns+"."+before.Identifier);
        Check(!routes.Any(r=>r.StartsWith("~/",StringComparison.Ordinal)),"Absolute route style remains: "+ns+"."+before.Identifier);
    }
}
if(management)
{
    var hashes=JsonSerializer.Deserialize<BaselineHash[]>(File.ReadAllText(fixtures+"Management20BaselineHashes.json"))!;
    foreach(var entry in hashes)Check(Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(entry.File))).Equals(entry.Sha256,StringComparison.OrdinalIgnoreCase),"Immediate pre-batch consumer snapshot changed: "+entry.File);
    string basePath="Aban360.UserPool.Application/Common/Base/UserBaseCreateOrUpdateService.cs";
    Check(Tokens(Read(basePath).DescendantNodes().OfType<ClassDeclarationSyntax>().Single())==Tokens(Read(fixtures+"SupportManagement20/UserBaseCreateOrUpdateService.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single()),"User base logic changed");
    // Estate's source file also contains an unselected report controller.
    var oldInfo=Read(fixtures+"OriginalConsumersManagement20/Aban360.Api_Controllers_V1_ClaimPool_Land_EstateController.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c=>c.Identifier.ValueText=="EstateInfoController");
    var newInfo=Read("Aban360.Api/Controllers/V1/ClaimPool/Land/EstateController.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c=>c.Identifier.ValueText=="EstateInfoController");
    Check(Tokens(oldInfo)==Tokens(newInfo),"Unselected EstateInfo report controller changed");
}
if(workflow)
{
    var hashes=JsonSerializer.Deserialize<BaselineHash[]>(File.ReadAllText(fixtures+"Workflow10BaselineHashes.json"))!;
    foreach(var entry in hashes)Check(Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(entry.File))).Equals(entry.Sha256,StringComparison.OrdinalIgnoreCase),"Immediate workflow baseline changed: "+entry.File);
    Check(Tokens(Read("Aban360.UserPool.Application/Common/Base/UserBaseCreateOrUpdateService.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single())==Tokens(Read(fixtures+"SupportManagement20/UserBaseCreateOrUpdateService.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single()),"Shared user base changed");
    var bulkFixtures=JsonSerializer.Deserialize<SqlFixture[]>(File.ReadAllText(fixtures+"WorkflowBulkFixtures.json"))!;
    Check(bulkFixtures.Length==3,"Bulk fixture coverage changed");
    foreach(var bulk in bulkFixtures)
    {
        var source=Read(bulk.Source).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
        var copy=Read(bulk.Fixture).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
        Check(Tokens(new WorkflowBulkBoundary().Visit(source)!)==Tokens(copy),"Bulk fixture changed schema, mapping or command logic: "+bulk.Source);
    }
    Check(Tokens(Read("Aban360.PaymentPool.Application/Exceptions/ExceptionLiterals.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single())==Tokens(Read(fixtures+"SupportWorkflow10/ExceptionLiterals.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single()),"Payment exception constants drifted");
}
var sqlFixtures=JsonSerializer.Deserialize<SqlFixture[]>(File.ReadAllText(fixtures+"SqlFixtures"+batch+".json"))!;
Check(sqlFixtures.Length==(workflow?19:management?30:auxiliary?14:20),"Missing SQL recording fixtures");
if(!auxiliary&&!management&&!workflow)Check(Tokens(Read("Aban360.UserPool.Application/Features/AccessTree/Factories/AccessTreeFactory.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single())==
    Tokens(Read(fixtures+"SupportCritical10/AccessTreeFactory.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single()),"Frozen internal access-tree factory drifted");
foreach(var fixture in sqlFixtures)
{
    var source=Read(fixture.Source).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
    if(workflow&&fixture.Source.EndsWith("/OriginalHandlersWorkflow10/RequestAfterSaleHandler.cs"))source=(ClassDeclarationSyntax)new ApprovedAfterSaleAwait().Visit(source)!;
    var copy=Read(fixture.Fixture).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
    // The connection base is the sole executable substitution. All methods, constructors,
    // field types and initializers must remain byte-for-token identical.
    Check(Tokens(source.WithBaseList(null))==Tokens(copy.WithBaseList(null)),"SQL test fixture drift: "+fixture.Fixture);
    Check(copy.BaseList!.Types[0].Type.ToString()=="global::"+(workflow?"Workflow":management?"Management":auxiliary?"Auxiliary":"Critical")+"RecordingConnectionBase","Unexpected SQL harness seam");
}
Console.WriteLine($"PASS: {checks} structural checks; {methods} complete service methods, {transactions} SQL transaction bodies and {consumerMethods} consumer methods retain their executable syntax, literals, guards and metadata; {sqlFixtures.Length} SQL fixtures match their sources.");

class Rename(Dictionary<string,string> names):CSharpSyntaxRewriter
{
    public override SyntaxToken VisitToken(SyntaxToken t)
    {
        if(t.Parent is IdentifierNameSyntax id && id.Parent is MemberAccessExpressionSyntax member && member.Name==id && member.Expression.ToString()!="this" && !t.ValueText.StartsWith('_')) return base.VisitToken(t);
        return t.IsKind(SyntaxKind.IdentifierToken)&&names.TryGetValue(t.ValueText,out var name)?SyntaxFactory.Identifier(t.LeadingTrivia,name,t.TrailingTrivia):base.VisitToken(t);
    }
}
class CallRename(Dictionary<string,string> calls):CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax n)=>calls.TryGetValue(n.Expression+"."+n.Name.Identifier.ValueText,out var extra)?n.WithName(SyntaxFactory.IdentifierName(extra)):n.Name.Identifier.ValueText=="Handle"&&calls.TryGetValue(n.Expression.ToString(),out var method)?n.WithName(SyntaxFactory.IdentifierName(method)):base.VisitMemberAccessExpression(n);
}
// Preserve the frozen baseline while accepting the explicitly requested route declaration style.
class RelativeRouteStyle(string prefix):CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitAttribute(AttributeSyntax node)
    {
        if(node.Name.ToString()=="Route"&&node.ArgumentList?.Arguments.SingleOrDefault()?.Expression is LiteralExpressionSyntax literal
            &&literal.Token.ValueText.StartsWith("~/"+prefix+"/",StringComparison.Ordinal))
            return node.WithArgumentList(SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal(literal.Token.ValueText[(prefix.Length+3)..]))))));
        return base.VisitAttribute(node);
    }
}
record Operation(string Feature,string Original,string OldContract,string Service,string Method,string Source,string Output,Dictionary<string,string> Names,string? ContractSource=null);
record SqlFixture(string Source,string Fixture,bool Before);

record BaselineHash(string File,string Sha256);

// The user corrected this helper to async; the caller now awaits its result too.
// Keep the original snapshot frozen and allow only these three explicit async changes.
class ApprovedAfterSaleAwait:CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var result=(MethodDeclarationSyntax)base.VisitMethodDeclaration(node)!;
        if(node.Identifier.ValueText is "GetMoshtrackCreateDto" or "CreateAfterSaleGetMoshtrackCreateDto")
            result=result.WithReturnType(SyntaxFactory.ParseTypeName("Task<MoshtrakCreateDto>")).WithModifiers(result.Modifiers.Add(SyntaxFactory.Token(SyntaxKind.AsyncKeyword)));
        return result;
    }
    public override SyntaxNode? VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if(node.Expression.ToString() is "GetMoshtrackCreateDto" or "CreateAfterSaleGetMoshtrackCreateDto" or "SelectedServiceValidate" or "CreateAfterSaleSelectedServiceValidate")
            return SyntaxFactory.AwaitExpression(node);
        return base.VisitInvocationExpression(node);
    }
}

// Sole test substitution: record SqlBulkCopy's real table/mapping/row inputs.
class WorkflowBulkBoundary:CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        if(node.Type.ToString()!="SqlBulkCopy")return base.VisitObjectCreationExpression(node);
        var args=node.ArgumentList!.Arguments;
        args=args.Replace(args[0],args[0].WithExpression(((CastExpressionSyntax)args[0].Expression).WithType(SyntaxFactory.ParseTypeName("IDbConnection"))));
        args=args.Replace(args[2],args[2].WithExpression(((CastExpressionSyntax)args[2].Expression).WithType(SyntaxFactory.ParseTypeName("IDbTransaction"))));
        return node.WithType(SyntaxFactory.ParseTypeName("global::WorkflowRecordingBulkCopy")).WithArgumentList(node.ArgumentList.WithArguments(args));
    }
}



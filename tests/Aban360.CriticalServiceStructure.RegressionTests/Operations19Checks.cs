using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

static class Operations19Checks
{
    const string Fx="tests/Aban360.MergedControllers.SmokeTests/";
    public static void Run(bool verifyHead=false)
    {
        var ops=JsonSerializer.Deserialize<Ops19Operation[]>(File.ReadAllText(Fx+"ServiceConsolidationOperations19.json"))!;
        var contracts=ops.ToDictionary(o=>o.OldContract.Split('.').Last());int checks=0,methods=0,transactions=0,consumerMethods=0;
        void Check(bool ok,string message){if(!ok)throw new Exception(message);checks++;}
        Check(ops.Length==70&&ops.Select(o=>o.Service).Distinct().Count()==19,"Incorrect approved batch");
        Check(ops.All(o=>o.Feature!="Tariff"),"Tariff was selected");
        var guards=JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText(Fx+"Operations19ProtectedHashes.json"))!;
        foreach(var entry in guards)Check(File.Exists(entry.Key)&&Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(entry.Key)))==entry.Value,"Protected source changed: "+entry.Key);
        foreach(var group in ops.GroupBy(o=>o.Output))
        {
            var current=Read(group.Key).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            var ctor=current.Members.OfType<ConstructorDeclarationSyntax>().Single();
            Check(ctor.ParameterList.Parameters.Select(p=>p.Type!.ToString()).Distinct().Count()==ctor.ParameterList.Parameters.Count,"Duplicate dependency: "+group.Key);
            Check(!ctor.ParameterList.Parameters.Any(p=>p.Type!.ToString()==group.First().Service.Split('.').Last()),"Self injection: "+group.Key);
            var expectedStatements=new List<string>();int expectedMethods=0;
            foreach(var op in group)
            {
                var old=Read(Fx+"OriginalHandlersOperations19/"+Path.GetFileName(op.Source)).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                if(verifyHead)
                {
                    var start=new ProcessStartInfo("git"){RedirectStandardOutput=true,RedirectStandardError=true,UseShellExecute=false,StandardOutputEncoding=Encoding.UTF8,StandardErrorEncoding=Encoding.UTF8};
                    start.ArgumentList.Add("show");start.ArgumentList.Add("HEAD:"+op.Source);
                    using var process=Process.Start(start)!;string source=process.StandardOutput.ReadToEnd(),error=process.StandardError.ReadToEnd();process.WaitForExit();
                    Check(process.ExitCode==0,"Cannot read original handler from Git: "+op.Source+" / "+error);
                    var committed=CSharpSyntaxTree.ParseText(source).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                    committed=ApplyPriorConsumerMerges(committed,old);
                    // Git stores LF while the Windows checkout has CRLF, including in SQL literals.
                    // Only this Git-to-snapshot comparison normalizes line endings.
                    string committedTokens=Tokens(committed).Replace("\r\n","\n"),frozenTokens=Tokens(old).Replace("\r\n","\n");
                    Check(committedTokens==frozenTokens,"Frozen handler differs from Git: "+op.Source);
                }
                var fieldTypes=old.Members.OfType<FieldDeclarationSyntax>().ToDictionary(f=>f.Declaration.Variables.Single().Identifier.ValueText,f=>f.Declaration.Type.ToString());
                var calls=fieldTypes.Where(p=>contracts.ContainsKey(p.Value)).ToDictionary(p=>p.Key,p=>contracts[p.Value].PublicMethods);
                var types=fieldTypes.Values.Where(contracts.ContainsKey).Distinct().ToDictionary(t=>t,t=>contracts[t].Service.Split('.').Last());
                var rewrite=new Ops19Rewrite(op.Names,calls,types);
                foreach(var method in old.Members.OfType<MethodDeclarationSyntax>())
                {
                    var expected=(MethodDeclarationSyntax)rewrite.Visit(method)!;
                    var actual=current.Members.OfType<MethodDeclarationSyntax>().Single(m=>m.Identifier.ValueText==expected.Identifier.ValueText&&Tokens(m.ParameterList)==Tokens(expected.ParameterList));
                    Check(Tokens(expected)==Tokens(actual),"Method/SQL/literal/await changed: "+op.Feature+"."+method.Identifier);methods++;expectedMethods++;
                    transactions+=method.DescendantNodes().OfType<InvocationExpressionSyntax>().Count(i=>i.Expression.ToString().EndsWith(".BeginTransaction"));
                }
                foreach(var field in old.Members.OfType<FieldDeclarationSyntax>())
                {
                    if(op.Self.ContainsKey(field.Declaration.Variables.Single().Identifier.ValueText))continue;
                    var expected=(FieldDeclarationSyntax)rewrite.Visit(field)!;
                    var actual=current.Members.OfType<FieldDeclarationSyntax>().Single(f=>f.Declaration.Variables.Single().Identifier.ValueText==expected.Declaration.Variables.Single().Identifier.ValueText);
                    Check(Tokens(expected)==Tokens(actual),"Field/type/initializer changed: "+op.Original+"."+field.Declaration.Variables.Single().Identifier);
                }
                foreach(var nested in old.Members.OfType<BaseTypeDeclarationSyntax>())Check(current.Members.OfType<BaseTypeDeclarationSyntax>().Any(n=>Tokens(n)==Tokens(rewrite.Visit(nested)!)),"Nested type changed");
                var oldCtor=old.Members.OfType<ConstructorDeclarationSyntax>().SingleOrDefault();var names=new Dictionary<string,string>(op.Names);var selfParams=new HashSet<string>();
                foreach(var p in oldCtor?.ParameterList.Parameters??default)
                {
                    string type=new Ops19Rewrite(op.Names,new(),types).Visit(p.Type!)!.ToString();if(contracts.TryGetValue(p.Type!.ToString(),out var dep))
                    {
                        if(dep.Service==op.Service){selfParams.Add(p.Identifier.ValueText);continue;}type=dep.Service.Split('.').Last();
                    }
                    names[p.Identifier.ValueText]=ctor.ParameterList.Parameters.Single(n=>n.Type!.ToString()==type).Identifier.ValueText;
                }
                var body=oldCtor?.Body??(oldCtor?.ExpressionBody is null?null:SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(oldCtor.ExpressionBody.Expression)));
                foreach(var statement in body?.Statements??default)
                {
                    if(statement.DescendantNodes().OfType<IdentifierNameSyntax>().Any(n=>op.Self.ContainsKey(n.Identifier.ValueText)||selfParams.Contains(n.Identifier.ValueText)))continue;
                    string expected=Tokens(new Ops19Rewrite(names,calls,types).Visit(statement)!);Check(ctor.Body!.Statements.Any(s=>Tokens(s)==expected),"Constructor guard/assignment lost: "+op.Original+" / "+statement);expectedStatements.Add(expected);
                }
                if(oldCtor?.Initializer is not null)Check(Tokens(new Ops19Rewrite(names,calls,types).Visit(oldCtor.Initializer)!)==Tokens(ctor.Initializer!),"Base constructor changed");
                Check(!File.Exists(op.Source)&&!File.Exists(op.ContractSource),"Selected handler/contract was not removed: "+op.Source);
            }
            Check(expectedMethods==current.Members.OfType<MethodDeclarationSyntax>().Count(),"Unreviewed service method: "+group.Key);
            Check(expectedStatements.Distinct().SequenceEqual(ctor.Body!.Statements.Select(Tokens).Distinct()),"Constructor guard/assignment order changed: "+group.Key);
        }
        var styles=JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText(Fx+"Operations19RouteStyles.json"))!;
        foreach(var file in JsonSerializer.Deserialize<Ops19Consumer[]>(File.ReadAllText(Fx+"Operations19Consumers.json"))!)
        {
            var oldTree=Read(Fx+"OriginalConsumersOperations19/"+file.Source.Replace('/','_'));var newTree=Read(file.Source);
            if(file.Source.EndsWith("/MeterReadingDetailSendSmsHandler.cs",StringComparison.Ordinal))
            {
                var original=oldTree.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                Check(!original.Members.OfType<MethodDeclarationSyntax>().SelectMany(m=>m.DescendantNodes().OfType<IdentifierNameSyntax>()).Any(n=>n.Identifier.ValueText=="_calcConfirmationHandler"),"Removed SMS dependency was actually used");
                var field=original.Members.OfType<FieldDeclarationSyntax>().Single(f=>f.Declaration.Type.ToString()=="ICalculationConfirmationHandler");
                var constructor=original.Members.OfType<ConstructorDeclarationSyntax>().Single();
                var parameter=constructor.ParameterList.Parameters.Single(p=>p.Type!.ToString()=="ICalculationConfirmationHandler");
                var removable=constructor.Body!.Statements.Where(s=>s.DescendantNodes().OfType<IdentifierNameSyntax>().Any(n=>n.Identifier.ValueText is "_calcConfirmationHandler" or "calcConfirmationHandler")).ToArray();
                Check(removable.Length==2,"Unexpected unused SMS constructor dependency shape");
                var updated=constructor.WithParameterList(constructor.ParameterList.WithParameters(constructor.ParameterList.Parameters.Remove(parameter))).WithBody(constructor.Body.WithStatements(SyntaxFactory.List(constructor.Body.Statements.Except(removable))));
                var members=original.Members.Where(m=>m.Span!=field.Span).Select(m=>m is ConstructorDeclarationSyntax?updated:m);
                oldTree=oldTree.ReplaceNode(original,original.WithMembers(SyntaxFactory.List(members)));
            }
            var applicable=contracts.Where(p=>oldTree.DescendantNodes().OfType<UsingDirectiveSyntax>().Any(u=>u.Alias is null&&u.Name?.ToString()==p.Value.OldContract[..p.Value.OldContract.LastIndexOf('.')])).ToDictionary(p=>p.Key,p=>p.Value);
            foreach(var old in oldTree.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var current=newTree.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c=>c.Identifier.ValueText==old.Identifier.ValueText);
                var names=new Dictionary<string,string>();var calls=new Dictionary<string,Dictionary<string,string>>();var types=applicable.ToDictionary(p=>p.Key,p=>p.Value.Service.Split('.').Last());
                foreach(var field in old.Members.OfType<FieldDeclarationSyntax>())if(applicable.TryGetValue(field.Declaration.Type.ToString(),out var op))
                {
                    var matching=current.Members.OfType<FieldDeclarationSyntax>().Where(f=>f.Declaration.Type.ToString()==op.Service.Split('.').Last()).ToArray();
                    Check(matching.Length==1,"Consumer service binding missing: "+file.Source+" / "+field.Declaration.Type+" => "+op.Service);
                    var actual=matching.Single();
                    foreach(var variable in field.Declaration.Variables){names[variable.Identifier.ValueText]=actual.Declaration.Variables.Single().Identifier.ValueText;calls[variable.Identifier.ValueText]=op.PublicMethods;}
                }
                string full=old.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().First().Name.ToString().Replace("BeforeConsumersOperations19.","")+"."+old.Identifier.ValueText;
                var rewrite=new Ops19Rewrite(names,calls,types);
                foreach(var field in old.Members.OfType<FieldDeclarationSyntax>())
                {
                    var expected=(FieldDeclarationSyntax)rewrite.Visit(field)!;var actual=current.Members.OfType<FieldDeclarationSyntax>().Single(f=>f.Declaration.Variables.Single().Identifier.ValueText==expected.Declaration.Variables.Single().Identifier.ValueText);
                    Check(Tokens(expected)==Tokens(actual),"Consumer field type/initializer changed: "+full+"."+field.Declaration.Variables.Single().Identifier);
                }
                foreach(var method in old.Members.OfType<MethodDeclarationSyntax>())
                {
                    var expected=(MethodDeclarationSyntax)rewrite.Visit(method)!;if(styles.TryGetValue(full,out var prefix))expected=(MethodDeclarationSyntax)new Ops19Route(prefix).Visit(expected)!;
                    var actual=current.Members.OfType<MethodDeclarationSyntax>().Single(m=>m.Identifier.ValueText==method.Identifier.ValueText&&Tokens(m.ParameterList)==Tokens(expected.ParameterList));
                    Check(Tokens(expected)==Tokens(actual),"Consumer logic/HTTP metadata changed: "+full+"."+method.Identifier);consumerMethods++;
                }
                Check(old.Members.OfType<MethodDeclarationSyntax>().Count()==current.Members.OfType<MethodDeclarationSyntax>().Count(),"Consumer method count changed");
                var expectedAttributes=old.AttributeLists;if(styles.TryGetValue(full,out var route))expectedAttributes=expectedAttributes.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Route")).WithArgumentList(SyntaxFactory.ParseAttributeArgumentList("(\""+route+"\")")))));
                Check(string.Join("|",expectedAttributes.Select(Tokens))==string.Join("|",current.AttributeLists.Select(Tokens)),"Controller permission/route metadata changed: "+full);
                var beforeCtor=old.Members.OfType<ConstructorDeclarationSyntax>().SingleOrDefault();var afterCtor=current.Members.OfType<ConstructorDeclarationSyntax>().SingleOrDefault();
                if(beforeCtor is not null)
                {
                    var ctorNames=new Dictionary<string,string>(names);
                    foreach(var p in beforeCtor.ParameterList.Parameters){string type=types.GetValueOrDefault(p.Type!.ToString(),p.Type.ToString());ctorNames[p.Identifier.ValueText]=afterCtor!.ParameterList.Parameters.Single(n=>n.Type!.ToString()==type).Identifier.ValueText;}
                    var body=beforeCtor.Body??SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(beforeCtor.ExpressionBody!.Expression));
                    Check(body.Statements.Select(s=>Tokens(new Ops19Rewrite(ctorNames,calls,types).Visit(s)!)).Distinct().SequenceEqual(afterCtor!.Body!.Statements.Select(Tokens).Distinct()),"Consumer ctor guards/order changed: "+full);
                    if(beforeCtor.Initializer is not null)Check(Tokens(new Ops19Rewrite(ctorNames,calls,types).Visit(beforeCtor.Initializer)!)==Tokens(afterCtor.Initializer!),"Consumer base constructor changed");
                }
                Check(old.Members.OfType<FieldDeclarationSyntax>().SelectMany(f=>f.Declaration.Variables.Select(v=>names.GetValueOrDefault(v.Identifier.ValueText,v.Identifier.ValueText))).ToHashSet().SetEquals(current.Members.OfType<FieldDeclarationSyntax>().SelectMany(f=>f.Declaration.Variables.Select(v=>v.Identifier.ValueText))),"Consumer dependency lost");
                if(file.Source.Contains("/Controllers/"))Check(!current.DescendantNodes().OfType<AttributeSyntax>().Where(a=>a.Name.ToString()=="Route"||a.Name.ToString().StartsWith("Http")).SelectMany(a=>a.DescendantNodes().OfType<LiteralExpressionSyntax>()).Any(l=>l.Token.ValueText.StartsWith("~/")),"Absolute route remains: "+full);
            }
        }
        var sqlFixtures=JsonSerializer.Deserialize<Ops19SqlFixture[]>(File.ReadAllText(Fx+"SqlFixturesOperations19.json"))!;
        Check(sqlFixtures.Length==32,"SQL fixture coverage changed");
        foreach(var fixture in sqlFixtures)
        {
            var source=Read(fixture.Source).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();var copy=Read(fixture.Fixture).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            Check(Tokens(source.WithBaseList(null))==Tokens(copy.WithBaseList(null)),"SQL fixture logic differs from source: "+fixture.Fixture);
            Check(copy.BaseList!.Types[0].Type.ToString()=="global::WorkflowRecordingConnectionBase","Unexpected SQL substitution");
        }
        foreach(var fixture in JsonSerializer.Deserialize<Ops19SqlFixture[]>(File.ReadAllText(Fx+"Operations19Support.json"))!)Check(Tokens(Read(fixture.Source).DescendantNodes().OfType<ClassDeclarationSyntax>().Single())==Tokens(Read(fixture.Fixture).DescendantNodes().OfType<ClassDeclarationSyntax>().Single()),"Internal helper differs from production: "+fixture.Source);
        var bulkFixtures=JsonSerializer.Deserialize<Ops19SqlFixture[]>(File.ReadAllText(Fx+"Operations19BulkFixtures.json"))!;Check(bulkFixtures.Length==6,"Missing bulk command fixtures");
        foreach(var fixture in bulkFixtures)Check(Tokens(new Ops19SqlNativeBoundary().Visit(Read(fixture.Source).DescendantNodes().OfType<ClassDeclarationSyntax>().Single())!)==Tokens(Read(fixture.Fixture).DescendantNodes().OfType<ClassDeclarationSyntax>().Single()),"Bulk schema/SQL/mapping changed: "+fixture.Source);
        Console.WriteLine($"PASS: {checks} operations checks; 70 handlers consolidated into 19 services; {methods} complete methods, {transactions} SQL transaction bodies, {consumerMethods} consumer methods; {guards.Count} protected files unchanged; 32 SQL fixtures match production/frozen sources.");
    }
    static CompilationUnitSyntax Read(string p)=>CSharpSyntaxTree.ParseText(File.ReadAllText(p)).GetCompilationUnitRoot();
    // HEAD predates earlier approved batches too. Apply only their recorded dependency
    // type/field/parameter/call renames before authenticating this batch's frozen source.
    static ClassDeclarationSyntax ApplyPriorConsumerMerges(ClassDeclarationSyntax committed,ClassDeclarationSyntax frozen)
    {
        var prior=new[]{"Critical10","Auxiliary10","Management20","Workflow10"}.SelectMany(batch=>JsonSerializer.Deserialize<Operation[]>(File.ReadAllText(Fx+"ServiceConsolidation"+batch+".json"))!).ToArray();
        var names=new Dictionary<string,string>();var types=new Dictionary<string,string>();var calls=new Dictionary<string,Dictionary<string,string>>();
        var imports=committed.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>().Where(u=>u.Alias is null).Select(u=>u.Name!.ToString()).ToHashSet();
        imports.Add(committed.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().First().Name.ToString());
        foreach(var field in committed.Members.OfType<FieldDeclarationSyntax>())
        {
            string oldType=field.Declaration.Type.ToString();
            var operation=prior.SingleOrDefault(o=>o.OldContract.Split('.').Last()==oldType&&imports.Contains(o.OldContract[..o.OldContract.LastIndexOf('.')]));
            if(operation is null)continue;
            string service=operation.Service.Split('.').Last();
            var replacement=frozen.Members.OfType<FieldDeclarationSyntax>().Single(f=>f.Declaration.Type.ToString()==service);
            types[oldType]=service;
            foreach(var variable in field.Declaration.Variables)
            {
                names[variable.Identifier.ValueText]=replacement.Declaration.Variables.Single().Identifier.ValueText;
                calls[variable.Identifier.ValueText]=new(){["Handle"]=operation.Method};
                if(operation.Names.TryGetValue("HandleByDictionary",out var dictionary))calls[variable.Identifier.ValueText]["HandleByDictionary"]=dictionary;
            }
        }
        if(types.Count==0)return committed;
        var constructor=committed.Members.OfType<ConstructorDeclarationSyntax>().Single();var target=frozen.Members.OfType<ConstructorDeclarationSyntax>().Single();
        foreach(var parameter in constructor.ParameterList.Parameters)
            if(types.TryGetValue(parameter.Type!.ToString(),out var service))names[parameter.Identifier.ValueText]=target.ParameterList.Parameters.Single(p=>p.Type!.ToString()==service).Identifier.ValueText;
        var result=(ClassDeclarationSyntax)new Ops19Rewrite(names,calls,types).Visit(committed)!;
        var fields=new HashSet<string>();var members=result.Members.Where(m=>m is not FieldDeclarationSyntax f||fields.Add(f.Declaration.Variables.Single().Identifier.ValueText));
        result=result.WithMembers(SyntaxFactory.List(members));
        var updated=result.Members.OfType<ConstructorDeclarationSyntax>().Single();var parameters=new HashSet<string>();
        updated=updated.WithParameterList(updated.ParameterList.WithParameters(SyntaxFactory.SeparatedList(updated.ParameterList.Parameters.Where(p=>!types.Values.Contains(p.Type!.ToString())||parameters.Add(p.Type!.ToString())))))
            .WithBody(updated.Body!.WithStatements(SyntaxFactory.List(updated.Body.Statements.DistinctBy(Tokens))));
        return result.ReplaceNode(result.Members.OfType<ConstructorDeclarationSyntax>().Single(),updated);
    }
    internal static string Tokens(SyntaxNode n)=>string.Join("|",n.DescendantTokens().Select(t=>t.RawKind+":"+t.Text));
}
record Ops19Consumer(string Source);
record Ops19SqlFixture(string Source,string Fixture);
record Ops19Operation(string Feature,string Source,string ContractSource,string Assembly,string Original,string OldContract,string Service,string Method,string OldMethod,string Output,Dictionary<string,string> Names,Dictionary<string,string> PublicMethods,Dictionary<string,Dictionary<string,string>> Self);
class Ops19Rewrite(Dictionary<string,string> names,Dictionary<string,Dictionary<string,string>> calls,Dictionary<string,string> types):CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        if(node.Expression is IdentifierNameSyntax id&&calls.TryGetValue(id.Identifier.ValueText,out var methods)&&methods.TryGetValue(node.Name.Identifier.ValueText,out var method))
            return node.WithExpression(names.GetValueOrDefault(id.Identifier.ValueText)=="this"?SyntaxFactory.ThisExpression().WithTriviaFrom(id):(ExpressionSyntax)Visit(node.Expression)!).WithName(SyntaxFactory.IdentifierName(method).WithTriviaFrom(node.Name));
        return base.VisitMemberAccessExpression(node);
    }
    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if(node.Parent is MemberAccessExpressionSyntax member&&member.Name==node&&member.Expression.ToString()!="this"&&!node.Identifier.ValueText.StartsWith('_'))return node;
        if(types.TryGetValue(node.Identifier.ValueText,out var type))return SyntaxFactory.IdentifierName(type).WithTriviaFrom(node);
        return base.VisitIdentifierName(node);
    }
    public override SyntaxToken VisitToken(SyntaxToken t)
    {
        if(t.Parent is IdentifierNameSyntax id&&id.Parent is MemberAccessExpressionSyntax member&&member.Name==id&&member.Expression.ToString()!="this"&&!t.ValueText.StartsWith('_'))return t;
        return t.IsKind(SyntaxKind.IdentifierToken)&&names.TryGetValue(t.ValueText,out var name)&&name!="this"?SyntaxFactory.Identifier(t.LeadingTrivia,name,t.TrailingTrivia):base.VisitToken(t);
    }
}
class Ops19Route(string prefix):CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitAttribute(AttributeSyntax node)
    {
        if((node.Name.ToString()=="Route"||node.Name.ToString().StartsWith("Http"))&&node.ArgumentList?.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax literal&&literal.Token.ValueText.StartsWith("~/"+prefix+"/"))return node.ReplaceNode(literal,SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,SyntaxFactory.Literal(literal.Token.ValueText[(prefix.Length+3)..])));
        return base.VisitAttribute(node);
    }
}
class Ops19SqlNativeBoundary:CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        if(node.Type.ToString() is not ("SqlBulkCopy" or "SqlCommand"))return base.VisitObjectCreationExpression(node);
        var args=node.ArgumentList!.Arguments;int connection=node.Type.ToString()=="SqlBulkCopy"?0:1;
        args=args.Replace(args[connection],args[connection].WithExpression(((CastExpressionSyntax)args[connection].Expression).WithType(SyntaxFactory.ParseTypeName("IDbConnection"))));
        args=args.Replace(args[2],args[2].WithExpression(((CastExpressionSyntax)args[2].Expression).WithType(SyntaxFactory.ParseTypeName("IDbTransaction"))));
        return node.WithType(SyntaxFactory.ParseTypeName(node.Type.ToString()=="SqlBulkCopy"?"global::WorkflowRecordingBulkCopy":"global::Operations19RecordingSqlCommand")).WithArgumentList(node.ArgumentList.WithArguments(args));
    }
}

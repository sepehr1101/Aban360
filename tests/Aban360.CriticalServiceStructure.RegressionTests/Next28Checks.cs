using System.Diagnostics;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class Next28Checks
{
    const string Folder = "tests/Aban360.CriticalServiceStructure.RegressionTests/Next28Baseline/";
    // Earlier batches are checked against this batch's verified input. Run() separately
    // proves that those exact inputs still lead to the current production source.
    internal static string PriorSource(string path)
    {
        string normalized = path.Replace('\\', '/').TrimStart('.', '/');
        string salesReportsPrior = SalesReportsChecks.PriorSource(path);
        if (salesReportsPrior != path) return salesReportsPrior;
        // Later CollectBills consolidation has its own full source/runtime equivalence
        // suite. Earlier waves continue to verify against its frozen input.
        const string collectBillsSnapshots = "tests/Aban360.CollectBillsConsolidation.RegressionTests/Snapshots/";
        if (File.Exists(collectBillsSnapshots + "manifest.json"))
        {
            using var manifest = JsonDocument.Parse(File.ReadAllText(collectBillsSnapshots + "manifest.json"));
            foreach (var entry in manifest.RootElement.EnumerateArray())
                if (entry.GetProperty("Path").GetString() == normalized)
                    return collectBillsSnapshots + entry.GetProperty("Snapshot").GetString();
        }
        var consumers = JsonSerializer.Deserialize<Consumer[]>(File.ReadAllText(Folder + "consumers.json"))!;
        var consumer = consumers.SingleOrDefault(c => c.Path == normalized);
        if (consumer is not null) return consumer.Baseline;
        var operations = JsonSerializer.Deserialize<Operation[]>(File.ReadAllText(Folder + "operations.json"))!;
        var operation = operations.SingleOrDefault(o => o.Source == normalized);
        return operation is null ? path : Folder + operation.Name + ".txt";
    }
    internal static void Run()
    {
        if (File.Exists(SalesReportsChecks.Folder + "operations.json")) SalesReportsChecks.Run();
        if (File.Exists(CompactChecks.Folder + "operations.json")) CompactChecks.Run();
        var operations = JsonSerializer.Deserialize<Operation[]>(File.ReadAllText(Folder + "operations.json"))!;
        var consumers = JsonSerializer.Deserialize<Consumer[]>(File.ReadAllText(Folder + "consumers.json"))!;
        int checks = 0;
        void Check(bool condition, string message) { if (!condition) throw new Exception(message); checks++; }
        CompilationUnitSyntax Read(string path) => CSharpSyntaxTree.ParseText(File.ReadAllText(CompactChecks.PriorSource(path))).GetCompilationUnitRoot();
        string Tokens(SyntaxNode node) => string.Join("|", node.DescendantTokens().Select(t => t.RawKind + ":" + t.Text));
        string Head(string path)
        {
            var start = new ProcessStartInfo("git") { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, StandardOutputEncoding = System.Text.Encoding.UTF8 };
            start.ArgumentList.Add("show"); start.ArgumentList.Add("HEAD:" + path);
            using var process = Process.Start(start)!;
            string result = process.StandardOutput.ReadToEnd(); process.WaitForExit();
            Check(process.ExitCode == 0, "Cannot read baseline: " + path); return result;
        }
        Check(operations.Length == 39 && operations.Select(o => o.Number).Distinct().Count() == 28, "Incomplete selection");
        Check(!operations.Any(o => o.Number is 97 or 100), "Excluded capability was selected");
        foreach (var group in operations.GroupBy(o => o.Output))
        {
            var service = Read(group.Key).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            int expectedMethods = 0;
            foreach (var op in group)
            {
                var original = Read(Folder + op.Name + ".txt").DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                var committed = CSharpSyntaxTree.ParseText(Head(op.Source)).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                Check(Tokens(original) == Tokens(committed), "Frozen source differs from HEAD: " + op.Source);
                var rename = new Rename(op.Names);
                foreach (var method in original.Members.OfType<MethodDeclarationSyntax>())
                {
                    var expected = (MethodDeclarationSyntax)rename.Visit(method)!;
                    var actual = service.Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.ValueText == expected.Identifier.ValueText && Tokens(m.ParameterList) == Tokens(expected.ParameterList));
                    Check(Tokens(expected) == Tokens(actual), "Operation changed: " + op.Name + "." + method.Identifier);
                    expectedMethods++;
                }
                foreach (var field in original.Members.OfType<FieldDeclarationSyntax>())
                {
                    var expected = (FieldDeclarationSyntax)rename.Visit(field)!;
                    Check(service.Members.OfType<FieldDeclarationSyntax>().Any(f => Tokens(f) == Tokens(expected)), "Field or initializer changed: " + op.Name + "." + field);
                }
                foreach (var nested in original.Members.OfType<BaseTypeDeclarationSyntax>())
                    Check(service.Members.OfType<BaseTypeDeclarationSyntax>().Any(n => Tokens(n) == Tokens(rename.Visit(nested)!)), "Nested type changed: " + op.Name);
                var beforeCtor = original.Members.OfType<ConstructorDeclarationSyntax>().SingleOrDefault();
                var afterCtor = service.Members.OfType<ConstructorDeclarationSyntax>().Single();
                var ctorNames = new Dictionary<string, string>(op.Names);
                foreach (var parameter in beforeCtor?.ParameterList.Parameters ?? default)
                    ctorNames[parameter.Identifier.ValueText] = afterCtor.ParameterList.Parameters.Single(p => Tokens(p.Type!) == Tokens(parameter.Type!)).Identifier.ValueText;
                var ctorRename = new Rename(ctorNames);
                foreach (var statement in beforeCtor?.Body?.Statements ?? default)
                    Check(afterCtor.Body!.Statements.Any(s => Tokens(s) == Tokens(ctorRename.Visit(statement)!)), "Constructor guard/assignment lost: " + op.Name);
                if (beforeCtor?.Initializer is not null)
                    Check(afterCtor.Initializer is not null && Tokens(afterCtor.Initializer) == Tokens(ctorRename.Visit(beforeCtor.Initializer)!), "Base constructor changed: " + op.Name);
                if (op.Number is 71 or 87)
                {
                    var adapter = Read(op.Source).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                    var forwards = op.Number == 71
                        ? new Dictionary<string, string> { ["Handle"] = "_service.UpdateUsersInRoleAsync(roleId, cancellationToken)" }
                        : new Dictionary<string, string> { ["Handle"] = "_service.MeterReadingDetailSendSmsAsync(latestFlowId, userId, cancellationToken)", ["CreateJob"] = "_service.MeterReadingDetailSendSmsCreateJob(firstFlowId, userId)", ["SendSms"] = "_service.MeterReadingDetailSendSmsSendSms(newSmsFlowId, userId)" };
                    Check(adapter.Members.OfType<MethodDeclarationSyntax>().Count() == forwards.Count && adapter.Members.OfType<FieldDeclarationSyntax>().Single().Declaration.Type.ToString() == "I" + op.Service, "Legacy Hangfire adapter contains operation logic");
                    foreach (var forward in forwards)
                    {
                        var forwarded = adapter.Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.ValueText == forward.Key);
                        var legacyMethod = original.Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.ValueText == forward.Key);
                        Check(forwarded.ExpressionBody?.Expression.ToString() == forward.Value, "Legacy Hangfire forwarding changed: " + forward.Key);
                        Check(Tokens(forwarded.ParameterList) == Tokens(legacyMethod.ParameterList) && Tokens(forwarded.ReturnType) == Tokens(legacyMethod.ReturnType) && string.Join("|", forwarded.AttributeLists.Select(Tokens)) == string.Join("|", legacyMethod.AttributeLists.Select(Tokens)), "Legacy Hangfire signature/retry metadata changed: " + forward.Key);
                    }
                    Check(Tokens(Read(op.ContractSource)) == Tokens(CSharpSyntaxTree.ParseText(Head(op.ContractSource)).GetRoot()), "Persisted Hangfire contract changed");
                }
                else Check(!File.Exists(op.Source) && !File.Exists(op.ContractSource), "Legacy production declaration remains: " + op.Name);
            }
            Check(service.Members.OfType<MethodDeclarationSyntax>().Count() == expectedMethods, "Unexpected service methods: " + group.Key);
            var contract = Read(group.Key).DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single();
            Check(contract.Members.Count == service.Members.OfType<MethodDeclarationSyntax>().Count(m => m.Modifiers.Any(SyntaxKind.PublicKeyword)), "Contract count mismatch: " + group.Key);
        }
        var general = Read(operations.Single(o => o.Number == 98).Output).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
        var generalCtor = general.Members.OfType<ConstructorDeclarationSyntax>().Single();
        Check(generalCtor.ParameterList.Parameters.Any(p => p.Type!.ToString() == "IMoshtrakService") && generalCtor.Body!.Statements.Any(s => s.ToString() == "_moshtrakService = moshtrakService;") && generalCtor.Body.Statements.Any(s => s.ToString() == "_moshtrakService.NotNull(nameof(moshtrakService));"), "GeneralInformation dependency correction missing");
        var byContract = operations.ToDictionary(o => o.Contract);
        foreach (var consumer in consumers)
        {
            var original = Read(consumer.Baseline);
            Check(Tokens(original) == Tokens(CSharpSyntaxTree.ParseText(Head(consumer.Path)).GetRoot()), "Consumer snapshot differs from HEAD: " + consumer.Path);
            var actual = Read(consumer.Path);
            var rename = new ConsumerRename(byContract);
            foreach (var cls in original.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var expected = rename.Visit(cls)!;
                if (cls.Identifier.ValueText == "CalculationRequestController") expected = ConsolidateWorkflow((ClassDeclarationSyntax)expected);
                var current = actual.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c => c.Identifier.ValueText == cls.Identifier.ValueText);
                Check(Tokens(expected) == Tokens(current), "Consumer behavior/metadata changed: " + consumer.Path);
            }
        }
        Console.WriteLine($"Next28: {checks} source checks passed across 28 services, 39 active handlers and {consumers.Length} consumers.");
    }
    static ClassDeclarationSyntax ConsolidateWorkflow(ClassDeclarationSyntax cls)
    {
        var names = new Dictionary<string, string>
        {
            ["_reCalculationRequestHandler"] = "_calculationRequestWorkflowService",
            ["_calculationRequestDisplayHandler"] = "_calculationRequestWorkflowService",
            ["_calculationRequestConfirmHandler"] = "_calculationRequestWorkflowService",
            ["reCalculationRequestHandler"] = "calculationRequestWorkflowService",
            ["calculationRequestDisplayHandler"] = "calculationRequestWorkflowService",
            ["calculationRequestConfirmHandler"] = "calculationRequestWorkflowService"
        };
        var updated = (ClassDeclarationSyntax)new Rename(names).Visit(cls)!;
        bool found = false;
        var members = new List<MemberDeclarationSyntax>();
        foreach (var member in updated.Members)
        {
            if (member is FieldDeclarationSyntax field && field.Declaration.Type.ToString() == "ICalculationRequestWorkflowService")
            {
                if (found) continue;
                found = true;
            }
            if (member is ConstructorDeclarationSyntax ctor)
            {
                string Tokens(SyntaxNode node) => string.Join("|", node.DescendantTokens().Select(t => t.RawKind + ":" + t.Text));
                members.Add(ctor.WithParameterList(ctor.ParameterList.WithParameters(SyntaxFactory.SeparatedList(ctor.ParameterList.Parameters.DistinctBy(p => p.Identifier.ValueText))))
                    .WithBody(ctor.Body!.WithStatements(SyntaxFactory.List(ctor.Body.Statements.DistinctBy(Tokens)))));
            }
            else members.Add(member);
        }
        return updated.WithMembers(SyntaxFactory.List(members));
    }
    internal sealed record Operation(int Number, string Name, string Source, string Contract, string ContractSource, string Output, string Service, string Namespace, Dictionary<string, string> Names);
    sealed record Consumer(string Path, string Baseline);
    sealed class Rename(Dictionary<string, string> names) : CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node) => node.Expression is ThisExpressionSyntax ? base.VisitMemberAccessExpression(node) : node.WithExpression((ExpressionSyntax)Visit(node.Expression)!).WithName(node.Name);
        public override SyntaxToken VisitToken(SyntaxToken token) => token.IsKind(SyntaxKind.IdentifierToken) && names.TryGetValue(token.ValueText, out var name) ? SyntaxFactory.Identifier(token.LeadingTrivia, name, token.TrailingTrivia) : base.VisitToken(token);
    }
    sealed class ConsumerRename(Dictionary<string, Operation> operations) : CSharpSyntaxRewriter
    {
        Dictionary<string, string> fields = new();
        public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            fields = node.Members.OfType<FieldDeclarationSyntax>().Where(f => operations.ContainsKey(f.Declaration.Type.ToString())).SelectMany(f => f.Declaration.Variables.Select(v => (Name: v.Identifier.ValueText, Method: operations[f.Declaration.Type.ToString()].Names["Handle"]))).ToDictionary(x => x.Name, x => x.Method);
            return base.VisitClassDeclaration(node);
        }
        public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node) => node.Name.Identifier.ValueText == "Handle" && fields.TryGetValue(node.Expression.ToString(), out var name) ? base.VisitMemberAccessExpression(node.WithName(SyntaxFactory.IdentifierName(name))) : base.VisitMemberAccessExpression(node);
        public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node) => operations.TryGetValue(node.Identifier.ValueText, out var op) ? node.WithIdentifier(SyntaxFactory.Identifier("I" + op.Service)) : base.VisitIdentifierName(node);
    }
}

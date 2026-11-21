using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aban360.Tests.CompactSyntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Op = Aban360.Tests.CompactSyntax.Operation;
using BeforeConsumer = Aban360.Tests.CompactSyntax.Consumer;
using CompactRename = Aban360.Tests.CompactSyntax.Rename;

internal static class CompactChecks
{
    internal const string Folder = "tests/Aban360.CriticalServiceStructure.RegressionTests/CompactBaseline/";
    internal static string PriorSource(string path)
    {
        if (!File.Exists(Folder + "operations.json")) return path;
        string normalized = path.Replace('\\', '/').TrimStart('.', '/');
        var op = ReadOperations().SingleOrDefault(o => o.Source == normalized || o.ContractSource == normalized);
        if (op is not null) return op.Source == normalized ? op.Snapshot : op.ContractSnapshot;
        return JsonSerializer.Deserialize<BeforeConsumer[]>(File.ReadAllText(Folder + "consumers.json"))!.SingleOrDefault(c => c.Path == normalized)?.Snapshot ?? path;
    }
    static Op[] ReadOperations() => JsonSerializer.Deserialize<Op[]>(File.ReadAllText(Folder + "operations.json"))!;
    internal static void Run()
    {
        var operations = ReadOperations();
        var consumers = JsonSerializer.Deserialize<BeforeConsumer[]>(File.ReadAllText(Folder + "consumers.json"))!;
        int checks = 0, methodCount = 0;
        void Check(bool value, string message) { if (!value) throw new Exception(message); checks++; }
        string Tokens(SyntaxNode node) => ConsumerRewrite.Tokens(node);
        string Hash(string text) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text)));
        CompilationUnitSyntax Parse(string text) => CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
        CompilationUnitSyntax Read(string path) => Parse(File.ReadAllText(path));
        Check(operations.Length == 43 && operations.Select(o => o.Group).Distinct().Count() == 14, "Incomplete compact consolidation selection");
        Check(operations.All(o => o.OldName != "TrackingFlowGetHandler"), "Protected TrackingFlow was selected");
        var invoiceOriginal = Read(operations.Single(o => o.OldName == "InvoiceInserterHandler").Snapshot);
        var invoiceModels = Read("Aban360.CalculationPool.Application/Features/Bill/Handlers/Commands/Create/Implementation/InvoiceLineItemModels.cs");
        foreach (var model in invoiceOriginal.DescendantNodes().OfType<RecordDeclarationSyntax>())
            Check(invoiceModels.DescendantNodes().OfType<RecordDeclarationSyntax>().Any(n => Tokens(n) == Tokens(model)), "Invoice support record changed: " + model.Identifier);
        Check(invoiceModels.DescendantNodes().OfType<BaseNamespaceDeclarationSyntax>().Single().Name.ToString() == "Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation", "Invoice support type namespace changed");
        var factory = Read("Aban360.LocationPool.Application/Features/MainHierarchy/Factories/LocationTreeFactory.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
        Check(Tokens(factory) == Tokens(Read(Folder + "Support/LocationTreeFactory.txt").DescendantNodes().OfType<ClassDeclarationSyntax>().Single()), "Location factory changed during consolidation");
        Check(Tokens(factory) == Tokens(Read("tests/Aban360.MergedControllers.SmokeTests/SupportCompact/LocationTreeFactory.cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single()), "Location factory test substitute differs from production");
        var engineContract = Read("Aban360.CalculationPool.Application/Features/Base/IBaseIntervalTariffEngine.cs").DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single();
        var engineShim = Read("tests/Aban360.MergedControllers.SmokeTests/SupportCompact/IBaseIntervalTariffEngine.cs").DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single();
        Check(engineContract.Members.Select(Tokens).SequenceEqual(engineShim.Members.Select(Tokens)), "Frozen internal tariff-engine contract differs from production");
        foreach (var group in operations.GroupBy(o => o.Output))
        {
            var root = Read(group.Key);
            Check(!root.GetDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error), "Syntax error: " + group.Key);
            var after = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            var afterCtor = after.Members.OfType<ConstructorDeclarationSyntax>().SingleOrDefault();
            var parameters = after.ParameterList?.Parameters ?? afterCtor?.ParameterList.Parameters ?? default;
            Check(after.NormalizeWhitespace().ToFullString().Split('\n').Length <= 300, "Class size exceeds selected bound: " + group.Key);
            Check(parameters.Count <= 8 && parameters.Select(p => Tokens(p.Type!)).Distinct().Count() == parameters.Count, "Duplicate/excessive dependencies: " + group.Key);
            Check(after.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().Single().Name.ToString() == group.First().Namespace && after.Identifier.ValueText == group.First().Service, "Service identity changed");
            var expectedFields = new HashSet<string>(); var expectedDependencies = new HashSet<string>(); var expectedCtor = new List<string>(); int expectedMethods = 0;
            foreach (var op in group)
            {
                string frozen = File.ReadAllText(op.Snapshot), contract = File.ReadAllText(op.ContractSnapshot);
                Check(Hash(frozen) == op.Hash && Hash(contract) == op.ContractHash, "Frozen input changed: " + op.OldName);
                var before = Parse(frozen).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                var rename = new CompactRename(op.Names);
                foreach (var method in before.Members.OfType<MethodDeclarationSyntax>())
                {
                    var expected = (MethodDeclarationSyntax)rename.Visit(method)!;
                    var actual = after.Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.ValueText == expected.Identifier.ValueText && Tokens(m.ParameterList) == Tokens(expected.ParameterList));
                    Check(Tokens(actual) == Tokens(expected), "Executable syntax, signature or metadata changed: " + op.OldName + "." + method.Identifier);
                    expectedMethods++; methodCount++;
                }
                foreach (var field in before.Members.OfType<FieldDeclarationSyntax>())
                {
                    var expected = (FieldDeclarationSyntax)rename.Visit(field)!; expectedFields.Add(Tokens(expected));
                    Check(after.Members.OfType<FieldDeclarationSyntax>().Any(f => Tokens(f) == Tokens(expected)), "Field type/initializer changed: " + op.OldName);
                }
                foreach (var nested in before.Members.OfType<BaseTypeDeclarationSyntax>())
                    Check(after.Members.OfType<BaseTypeDeclarationSyntax>().Any(n => Tokens(n) == Tokens(rename.Visit(nested)!)), "Nested declaration changed: " + op.OldName);
                var ctor = before.Members.OfType<ConstructorDeclarationSyntax>().SingleOrDefault();
                var names = new Dictionary<string, string>(op.Names);
                foreach (var p in before.ParameterList?.Parameters ?? ctor?.ParameterList.Parameters ?? default)
                {
                    expectedDependencies.Add(Tokens(p.Type!));
                    names[p.Identifier.ValueText] = parameters.Single(a => Tokens(a.Type!) == Tokens(p.Type!)).Identifier.ValueText;
                }
                foreach (var statement in ctor?.Body?.Statements ?? default) expectedCtor.Add(Tokens(new CompactRename(names).Visit(statement)!));
                Check(!File.Exists(op.Source) && !File.Exists(op.ContractSource), "Duplicate legacy implementation/contract: " + op.OldName);
                // Runnable test fixtures must retain the original class exactly; only their
                // containing namespace and imports differ to avoid production name clashes.
                string runnable = "tests/Aban360.MergedControllers.SmokeTests/OriginalHandlersCompact/" + op.OldName + ".cs";
                if (File.Exists(runnable)) Check(Tokens(Read(runnable).DescendantNodes().OfType<ClassDeclarationSyntax>().Single()) == Tokens(before), "Runnable baseline changed: " + op.OldName);
            }
            Check(expectedFields.SetEquals(after.Members.OfType<FieldDeclarationSyntax>().Select(Tokens)), "Extra/unreviewed fields: " + group.Key);
            Check(expectedDependencies.SetEquals(parameters.Select(p => Tokens(p.Type!))), "Dependency union changed: " + group.Key);
            Check(expectedMethods == after.Members.OfType<MethodDeclarationSyntax>().Count(), "Extra/missing methods: " + group.Key);
            if (afterCtor is not null) Check(expectedCtor.Distinct().SequenceEqual(afterCtor.Body!.Statements.Select(Tokens)), "Constructor assignment/guard order changed: " + group.Key);
            var contractAfter = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single();
            foreach (var publicMethod in after.Members.OfType<MethodDeclarationSyntax>().Where(m => m.Modifiers.Any(SyntaxKind.PublicKeyword)))
                Check(contractAfter.Members.OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.ValueText == publicMethod.Identifier.ValueText && Tokens(m.ReturnType) == Tokens(publicMethod.ReturnType) && Tokens(m.ParameterList) == Tokens(publicMethod.ParameterList)), "Public contract signature mismatch: " + publicMethod.Identifier);
            Check(contractAfter.Members.Count == after.Members.OfType<MethodDeclarationSyntax>().Count(m => m.Modifiers.Any(SyntaxKind.PublicKeyword)), "Extra interface methods: " + group.Key);
        }
        foreach (var consumer in consumers)
        {
            string source = File.ReadAllText(consumer.Snapshot);
            Check(Hash(source) == consumer.Hash, "Frozen consumer input changed: " + consumer.Path);
            var expected = (CompilationUnitSyntax)new ConsumerRewrite(consumer.Bindings).Visit(Parse(source))!;
            var actual = Read(consumer.Path);
            foreach (var original in Parse(source).DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var current = actual.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c => c.Identifier.ValueText == original.Identifier.ValueText);
                Check(string.Join("|", original.AttributeLists.Select(Tokens)) == string.Join("|", current.AttributeLists.Select(Tokens)), "Consumer class metadata changed: " + consumer.Path);
                foreach (var originalMethod in original.Members.OfType<MethodDeclarationSyntax>())
                {
                    var currentMethod = current.Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.ValueText == originalMethod.Identifier.ValueText && Tokens(m.ParameterList) == Tokens(originalMethod.ParameterList));
                    Check(Tokens(originalMethod.ParameterList) == Tokens(currentMethod.ParameterList) && Tokens(originalMethod.ReturnType) == Tokens(currentMethod.ReturnType) && string.Join("|", originalMethod.AttributeLists.Select(Tokens)) == string.Join("|", currentMethod.AttributeLists.Select(Tokens)), "Consumer binding, signature or action metadata changed: " + consumer.Path);
                }
            }
            foreach (var cls in expected.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(c => c.Parent is not ClassDeclarationSyntax))
                Check(Tokens(cls) == Tokens(actual.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c => c.Identifier.ValueText == cls.Identifier.ValueText)), "Consumer logic, metadata, save order or service binding changed: " + consumer.Path);
            foreach (var binding in consumer.Bindings.Values.DistinctBy(b => b.Type))
                Check(actual.Usings.Any(u => u.Alias?.Name.Identifier.ValueText == binding.Type && u.Name?.ToString() == "global::" + binding.Namespace + "." + binding.Type), "Consumer contract resolves to an unintended namespace: " + consumer.Path);
        }
        Console.WriteLine($"Compact: {checks} source checks passed; {operations.Length} handlers / {operations.Select(o => o.Group).Distinct().Count()} services / {methodCount} complete methods / {consumers.Length} consumers. Every class <=300 lines and <=8 dependencies.");
    }
}

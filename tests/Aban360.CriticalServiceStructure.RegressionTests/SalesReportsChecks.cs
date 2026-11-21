using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aban360.Tests.CompactSyntax;
using SalesOperation = Aban360.Tests.CompactSyntax.Operation;
using SalesConsumer = Aban360.Tests.CompactSyntax.Consumer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class SalesReportsChecks
{
    internal const string Folder = "tests/Aban360.CriticalServiceStructure.RegressionTests/SalesReportsBaseline/";
    internal static string PriorSource(string path)
    {
        if (!File.Exists(Folder + "operations.json")) return path;
        string normalized = path.Replace('\\', '/').TrimStart('.', '/');
        var operation = JsonSerializer.Deserialize<SalesOperation[]>(File.ReadAllText(Folder + "operations.json"))!.FirstOrDefault(o => o.Source == normalized || o.ContractSource == normalized);
        if (operation is not null) return operation.Source == normalized ? operation.Snapshot : operation.ContractSnapshot;
        return JsonSerializer.Deserialize<SalesConsumer[]>(File.ReadAllText(Folder + "consumers.json"))!.SingleOrDefault(c => c.Path == normalized)?.Snapshot ?? path;
    }
    internal static void Run()
    {
        var operations = JsonSerializer.Deserialize<SalesOperation[]>(File.ReadAllText(Folder + "operations.json"))!;
        var consumers = JsonSerializer.Deserialize<SalesConsumer[]>(File.ReadAllText(Folder + "consumers.json"))!;
        int checks = 0, methods = 0;
        void Check(bool value, string message) { if (!value) throw new Exception(message); checks++; }
        string Tokens(SyntaxNode n) => ConsumerRewrite.Tokens(n);
        CompilationUnitSyntax Read(string p) => CSharpSyntaxTree.ParseText(File.ReadAllText(p)).GetCompilationUnitRoot();
        string Hash(string s) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(s)));
        Check(operations.Length == 16 && operations.Count(o => !o.OldName.EndsWith("Service")) == 13 && consumers.Length == 7, "Incomplete approved selection");
        var dependencyCounts = new Dictionary<string, int> { ["Article11"] = 5, ["InstallationAndEquipment"] = 5, ["TankerWaterDistanceTariff"] = 3, ["MeterReadingChangeReports"] = 4, ["TankerWaterQueries"] = 2 };
        foreach (var group in operations.GroupBy(o => o.Output))
        {
            var root = Read(group.Key); var after = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            var ctor = after.Members.OfType<ConstructorDeclarationSyntax>().Single();
            Check(!root.GetDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error), "Invalid source: " + group.Key);
            Check(after.ToFullString().Split('\n').Length <= 300, "Class size bound exceeded: " + group.Key);
            Check(ctor.ParameterList.Parameters.Count == dependencyCounts[group.First().Group], "Dependency union changed: " + group.Key);
            Check(ctor.ParameterList.Parameters.Select(p => Tokens(p.Type!)).Distinct().Count() == ctor.ParameterList.Parameters.Count, "Duplicate dependency");
            Check(after.Identifier.Text == group.First().Service && after.BaseList!.Types.Single().Type.ToString() == "I" + group.First().Service, "Unexpected service identity");
            var fields = new HashSet<string>(); var expectedCtor = new List<string>(); var dependencyTypes = new HashSet<string>(); int expectedMethods = 0;
            foreach (var op in group)
            {
                Check(Hash(File.ReadAllText(op.Snapshot)) == op.Hash && Hash(File.ReadAllText(op.ContractSnapshot)) == op.ContractHash, "Frozen input changed: " + op.OldName);
                var before = Read(op.Snapshot).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                var rename = new Rename(op.Names);
                foreach (var method in before.Members.OfType<MethodDeclarationSyntax>())
                {
                    var expected = (MethodDeclarationSyntax)rename.Visit(method)!;
                    Check(after.Members.OfType<MethodDeclarationSyntax>().Any(m => Tokens(m) == Tokens(expected)), "Full method syntax/metadata changed: " + op.OldName + "." + method.Identifier);
                    expectedMethods++; methods++;
                }
                foreach (var field in before.Members.OfType<FieldDeclarationSyntax>()) fields.Add(Tokens(rename.Visit(field)!));
                var names = new Dictionary<string, string>(op.Names);
                var beforeCtor = before.Members.OfType<ConstructorDeclarationSyntax>().Single();
                Check(beforeCtor.Initializer is null && ctor.Initializer is null, "Base/transaction boundary changed");
                foreach (var p in beforeCtor.ParameterList.Parameters)
                {
                    dependencyTypes.Add(Tokens(p.Type!));
                    names[p.Identifier.Text] = ctor.ParameterList.Parameters.Single(a => Tokens(a.Type!) == Tokens(p.Type!)).Identifier.Text;
                }
                foreach (var statement in beforeCtor.Body!.Statements) expectedCtor.Add(Tokens(new Rename(names).Visit(statement)!));
                var runnable = Read("tests/Aban360.MergedControllers.SmokeTests/OriginalSalesReports/" + op.OldName + ".cs").DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
                Check(Tokens(runnable) == Tokens(before.WithBaseList(null)), "Runnable original changed: " + op.OldName);
                if (!op.OldName.EndsWith("Service")) Check(!File.Exists(op.Source) && !File.Exists(op.ContractSource), "Duplicate old handler/contract remains: " + op.OldName);
                foreach (var method in Read(op.ContractSnapshot).DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single().Members.OfType<MethodDeclarationSyntax>())
                {
                    var expected = (MethodDeclarationSyntax)rename.Visit(method)!;
                    Check(root.DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single().Members.OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.Text == expected.Identifier.Text && Tokens(m.ReturnType) == Tokens(expected.ReturnType) && Tokens(m.ParameterList) == Tokens(expected.ParameterList)), "Public signature/default/binding changed: " + op.OldContract);
                }
            }
            Check(fields.SetEquals(after.Members.OfType<FieldDeclarationSyntax>().Select(Tokens)), "Field union/initializer changed: " + group.Key);
            Check(dependencyTypes.SetEquals(ctor.ParameterList.Parameters.Select(p => Tokens(p.Type!))), "Unreviewed dependency: " + group.Key);
            Check(expectedCtor.Distinct().SequenceEqual(ctor.Body!.Statements.Select(Tokens)), "Constructor guard/assignment order changed: " + group.Key);
            Check(expectedMethods == after.Members.OfType<MethodDeclarationSyntax>().Count(), "Extra/missing method: " + group.Key);
            Check(root.DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single().Members.Count == after.Members.OfType<MethodDeclarationSyntax>().Count(m => m.Modifiers.Any(SyntaxKind.PublicKeyword)), "Interface contains extra/missing operations");
        }
        foreach (var consumer in consumers)
        {
            Check(Hash(File.ReadAllText(consumer.Snapshot)) == consumer.Hash, "Consumer snapshot changed: " + consumer.Path);
            var before = Read(consumer.Snapshot); var after = Read(consumer.Path);
            var expected = (CompilationUnitSyntax)new ConsumerRewrite(consumer.Bindings).Visit(before)!;
            Check(Tokens(expected.DescendantNodes().OfType<ClassDeclarationSyntax>().Single()) == Tokens(after.DescendantNodes().OfType<ClassDeclarationSyntax>().Single()), "Consumer executable logic changed: " + consumer.Path);
            var priorClass = before.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(); var actualClass = after.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            Check(priorClass.AttributeLists.Select(Tokens).SequenceEqual(actualClass.AttributeLists.Select(Tokens)), "Controller routes/authorization changed");
            foreach (var method in priorClass.Members.OfType<MethodDeclarationSyntax>())
            {
                var actual = actualClass.Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == method.Identifier.Text && Tokens(m.ParameterList) == Tokens(method.ParameterList));
                Check(method.AttributeLists.Select(Tokens).SequenceEqual(actual.AttributeLists.Select(Tokens)) && Tokens(method.ReturnType) == Tokens(actual.ReturnType), "Action binding/permission/response metadata changed");
            }
            foreach (var binding in consumer.Bindings.Values.DistinctBy(b => b.Type)) Check(after.Usings.Any(u => u.Alias?.Name.ToString() == binding.Type && u.Name!.ToString() == "global::" + binding.Namespace + "." + binding.Type), "Incorrect service namespace binding");
            var runnable = Read("tests/Aban360.MergedControllers.SmokeTests/OriginalConsumersSalesReports/" + consumer.Path.Replace('/', '_')).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            Check(Tokens(runnable) == Tokens(priorClass), "Runnable consumer baseline changed");
        }
        Console.WriteLine($"SalesReports: {checks} source checks passed; 13 handlers / 5 services / {methods} complete methods / 7 consumers. Maximum five dependencies; every class under 300 lines.");
    }
}

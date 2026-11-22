using System.Collections;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.CollectBills.Services;
using Aban360.CalculationPool.Application.Extensions;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Hangfire;
using Hangfire.Storage;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var root = new DirectoryInfo(AppContext.BaseDirectory);
while (root is not null && !File.Exists(Path.Combine(root.FullName, "Aban360.sln"))) root = root.Parent;
Directory.SetCurrentDirectory(root!.FullName);
const string fx = "tests/Aban360.CollectBillsConsolidation.RegressionTests/Snapshots/";
const string target = "Aban360.CalculationPool.Application/Features/CollectBills/Services/CollectBillsApplicationService.cs";
int checks = 0;
void Check(bool value, string message) { if (!value) throw new Exception(message); checks++; }
string Tokens(SyntaxNode node) => string.Join(" ", node.DescendantTokens().Select(t => t.Text));
CompilationUnitSyntax Parse(string text) => CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
ClassDeclarationSyntax Class(string text) => Parse(text).DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
string Snapshot(int number) => File.ReadAllText(fx + number + ".txt");
foreach (var entry in JsonDocument.Parse(File.ReadAllText(fx + "manifest.json")).RootElement.EnumerateArray())
    Check(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(fx + entry.GetProperty("Snapshot").GetString()))).Equals(entry.GetProperty("Hash").GetString(), StringComparison.OrdinalIgnoreCase), "Frozen source changed");
var current = Class(File.ReadAllText(target));
Check(current.BaseList!.ToString().Contains("AbstractBaseConnection"), "Connection base changed");
foreach (int n in new[] { 0, 1, 2 })
{
    var prior = Class(Snapshot(n));
    foreach (var method in prior.Members.OfType<MethodDeclarationSyntax>())
    {
        var text = method.ToFullString();
        if (n == 1) text = text.Replace("_collectBillsDetailJobService.CreateZip(", "CreateZip(");
        if (n == 2) text = text.Replace(">> Handle(", ">> GetDetailWithLastStepAsync(");
        var expected = Parse("class T {" + text + "}").DescendantNodes().OfType<MethodDeclarationSyntax>().Single();
        var actual = current.Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == expected.Identifier.Text);
        Check(Tokens(actual) == Tokens(expected), "Method, SQL, job expression, transaction or file behavior changed: " + expected.Identifier);
    }
    foreach (var field in prior.Members.OfType<FieldDeclarationSyntax>())
    {
        if (n == 1 && field.Declaration.Type.ToString() == "ICollectBillsDetailJobService") continue;
        Check(current.Members.OfType<FieldDeclarationSyntax>().Any(f => Tokens(f) == Tokens(field)), "Field/initializer changed: " + field.Declaration);
    }
}
Check(current.Members.OfType<MethodDeclarationSyntax>().Count() == 12, "Unexpected method count");
Check(current.ToFullString().Split('\n').Length <= 300, "CollectBills class exceeds the selected size bound");
var unifiedContract = Parse(File.ReadAllText(target)).DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single();
Check(unifiedContract.Members.Count == 9, "Unexpected unified contract size");
foreach (var method in current.Members.OfType<MethodDeclarationSyntax>().Where(m => m.Modifiers.Any(SyntaxKind.PublicKeyword)))
    Check(unifiedContract.Members.OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.Text == method.Identifier.Text && Tokens(m.ReturnType) == Tokens(method.ReturnType) && m.ParameterList.Parameters.Select(p => Tokens(p.Type!)).SequenceEqual(method.ParameterList.Parameters.Select(p => Tokens(p.Type!)))), "Missing unified entry point: " + method.Identifier);
var legacySource = Parse(File.ReadAllText("Aban360.CalculationPool.Application/Features/Base/CollectBillsDetailJobService.cs"));
Check(Tokens(legacySource.DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single()) == Tokens(Parse(Snapshot(0)).DescendantNodes().OfType<InterfaceDeclarationSyntax>().Single()), "Persisted job interface changed");
foreach (var old in Class(Snapshot(0)).Members.OfType<MethodDeclarationSyntax>().Where(m => m.Modifiers.Any(SyntaxKind.PublicKeyword)))
{
    var adapter = legacySource.DescendantNodes().OfType<ClassDeclarationSyntax>().Single().Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == old.Identifier.Text);
    Check(Tokens(adapter.ReturnType) == Tokens(old.ReturnType) && Tokens(adapter.ParameterList) == Tokens(old.ParameterList) && adapter.AttributeLists.Select(Tokens).SequenceEqual(old.AttributeLists.Select(Tokens)), "Persisted concrete method metadata changed: " + old.Identifier);
}
var ctor = current.Members.OfType<ConstructorDeclarationSyntax>().Single();
Check(ctor.ParameterList.Parameters.Count == 6 && ctor.ParameterList.Parameters.Select(p => p.Type!.ToString()).Distinct().Count() == 6, "Dependency union changed or self-injected");
Check(Tokens(ctor.Initializer!) == Tokens(Class(Snapshot(0)).Members.OfType<ConstructorDeclarationSyntax>().Single().Initializer!), "Base constructor changed");
foreach (var statement in Class(Snapshot(0)).Members.OfType<ConstructorDeclarationSyntax>().Single().Body!.Statements)
    Check(ctor.Body!.Statements.Any(s => Tokens(s) == Tokens(statement)), "Original constructor guard/assignment lost");
string controller = "Aban360.Api/Controllers/V1/CalculationPool/CollectBills/CollectBillsController.cs";
var controllerNow = Class(File.ReadAllText(controller));
var controllerBefore = Class(Snapshot(4));
Check(Tokens(controllerNow.AttributeLists.Single()) == Tokens(controllerBefore.AttributeLists.Single()), "Controller route changed");
foreach (var method in controllerBefore.Members.OfType<MethodDeclarationSyntax>())
{
    var text = method.ToFullString().Replace("_collectBillsFilesService.", "_collectBillsService.").Replace("_collectBillsDetailWithLastStepHandler.Handle(", "_collectBillsService.GetDetailWithLastStepAsync(").Replace("_jobService.Initialize(", "_collectBillsService.Initialize(");
    if (method.Identifier.ValueText == "CreateFile")
        text = text.Replace("[HttpPost, HttpGet]", "[HttpPost]", StringComparison.Ordinal);
    var expected = Parse("class T {" + text + "}").DescendantNodes().OfType<MethodDeclarationSyntax>().Single();
    Check(Tokens(controllerNow.Members.OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == expected.Identifier.Text)) == Tokens(expected), "Controller contract or behavior changed: " + method.Identifier);
}
Check(controllerNow.Members.OfType<MethodDeclarationSyntax>().Count() == 6, "GET/POST action count changed");
Check(Tokens(Class(File.ReadAllText("Aban360.Api/Cronjobs/CollectBillsJob.cs"))) == Tokens(Class(Snapshot(5).Replace("ICollectBillsDetailJobService", "ICollectBillsApplicationService"))), "Recurring job behavior changed");
Check(!File.Exists("Aban360.CalculationPool.Application/Features/CollectBills/Services/CollectBillsFilesService.cs"), "Duplicate implementation remains");
Check(!File.Exists("Aban360.CalculationPool.Application/Features/CollectBills/Handlers/Queries/Implementations/CollectBillsDetailWithLastStepGetHadler.cs"), "Old handler remains");
Check(!File.Exists("Aban360.CalculationPool.Application/Features/CollectBills/Handlers/Queries/Contracts/ICollectBillsDetailWithLastStepGetHadler.cs"), "Old report contract remains");


int runtimeChecks=await CollectBillsRuntimeChecks.Run();
Console.WriteLine($"PASS CollectBills source: {checks} equivalence checks; {runtimeChecks} runtime checks.");

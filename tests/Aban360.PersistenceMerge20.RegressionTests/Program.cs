using System.Collections;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aban360.BlobPool.Persistence.Contexts.Implementations;
using Aban360.BlobPool.Persistence.Extensions;
using Aban360.CalculationPool.Persistence.Contexts.Implementations;
using Aban360.CalculationPool.Persistence.Extensions;
using Aban360.ClaimPool.Persistence.Contexts.Implementation;
using Aban360.ClaimPool.Persistence.Extensions;
using Aban360.InstallationPool.Persistence.Contexts.Implementations;
using Aban360.InstallationPool.Persistence.Extensions;
using Aban360.LocationPool.Persistence.Extensions;
using LocationContext = Aban360.UserPool.Persistence.Contexts.Implementation.LocationPoolContext;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

var root = FindRoot();
var testDirectory = Path.Combine(root, "tests", "Aban360.PersistenceMerge20.RegressionTests");
var items = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(Path.Combine(testDirectory, "manifest.json")))!;
if (args.Contains("--all50") || args.Contains("--all80")) items.AddRange(JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(Path.Combine(testDirectory, "next30.json")))!);
if (args.Contains("--all80")) items.AddRange(JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(Path.Combine(testDirectory, "rows51to80.json")))!);
var expectedCount = args.Contains("--all80") ? 80 : args.Contains("--all50") ? 50 : 20;
var checks = 0;
var methodsChecked = 0;
var scenarios = 0;
var maximumLines = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
    checks++;
}
string Tokens(SyntaxNode node) => string.Join(" ", node.DescendantTokens().Select(t => t.Kind() + ":" + t.Text));
string Signature(MethodDeclarationSyntax method) => method.Identifier.Text + "(" + string.Join(",", method.ParameterList.Parameters.Select(p => p.Type!.ToString())) + ")";
var baselineTrees = new List<SyntaxTree>();
foreach (var item in items)
{
    var currentTree = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(root, item.RepositoryPath)));
    Check(!currentTree.GetDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error), item.Name + " parses");
    var current = currentTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
    var count = currentTree.GetLineSpan(current.Span);
    maximumLines = Math.Max(maximumLines, count.EndLinePosition.Line - count.StartLinePosition.Line + 1);
    Check(current.BaseList!.Types.Count == 2, item.Name + " preserves both contracts");
    var expectedFields = item.Name == "RequestUser" ? 3 : 2;
    Check(current.Members.OfType<FieldDeclarationSyntax>().Count() == expectedFields, item.Name + " preserves the required UoW and DbSets");
    var expectedMethods = 0;
    foreach (var (oldType, oldPath, query) in new[] { (item.CommandType, item.CommandPath, false), (item.QueryType, item.QueryPath, true) })
    {
        Check(!File.Exists(Path.Combine(root, oldPath)), "Old implementation removed: " + oldType);
        var name = oldType.Split('.').Last();
        var source = File.ReadAllText(Path.Combine(testDirectory, "Baseline", name + ".cs.txt"));
        var syntax = CSharpSyntaxTree.ParseText(source).GetRoot();
        var original = syntax.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
        baselineTrees.Add(CSharpSyntaxTree.Create((CSharpSyntaxNode)new FrozenNamespace().Visit(syntax)!));
        foreach (var method in original.Members.OfType<MethodDeclarationSyntax>())
        {
            var normalized = query ? new RenameFields(item.FieldMap).Visit(method)! : method;
            var match = current.Members.OfType<MethodDeclarationSyntax>().Single(m => Signature(m) == Signature(method));
            Check(Tokens(normalized) == Tokens(match), item.Name + " unchanged operation " + Signature(method));
            methodsChecked++;
            expectedMethods++;
        }
        if (!query)
        {
            var originalConstructor = original.Members.OfType<ConstructorDeclarationSyntax>().Single();
            var constructor = current.Members.OfType<ConstructorDeclarationSyntax>().Single();
            if (item.Name == "RequestUser")
                Check(originalConstructor.Body!.Statements.Select(Tokens).SequenceEqual(constructor.Body!.Statements.Take(originalConstructor.Body.Statements.Count).Select(Tokens)), item.Name + " command initialization preserved before additional query initialization");
            else
                Check(Tokens(originalConstructor.Body!) == Tokens(constructor.Body!), item.Name + " command initialization preserved");
        }
    }
    Check(current.Members.Count == expectedMethods + expectedFields + 1, item.Name + " has no additional members or operations");
    foreach (var contract in item.Contracts)
        Check(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root, contract.Key)))) == contract.Value, "Contract unchanged: " + contract.Key);
}
Console.WriteLine($"Source review passed: {methodsChecked} unchanged operations and {checks} structural/contract checks.");

// Compile frozen implementations against the current, unchanged interfaces for before/after execution.
var referencePaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
    .Concat(Directory.GetFiles(AppContext.BaseDirectory, "*.dll"))
    .GroupBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase).Select(g => g.First());
var references = new List<MetadataReference>();
foreach (var path in referencePaths)
{
    try { references.Add(MetadataReference.CreateFromFile(path)); }
    catch (BadImageFormatException) { }
}
baselineTrees.Add(CSharpSyntaxTree.ParseText("global using System; global using System.Collections.Generic; global using System.Linq; global using System.Threading; global using System.Threading.Tasks;"));
var compilation = CSharpCompilation.Create("PersistenceMerge20Frozen", baselineTrees, references,
    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
using var buffer = new MemoryStream();
var emitted = compilation.Emit(buffer);
Check(emitted.Success, string.Join(Environment.NewLine, emitted.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)));
buffer.Position = 0;
var frozen = AssemblyLoadContext.Default.LoadFromStream(buffer);

var services = new ServiceCollection();
services.AddDbContext<BlobPoolContext>(o => o.UseInMemoryDatabase("blob-di-" + Guid.NewGuid()));
services.AddDbContext<CalculationPoolContext>(o => o.UseInMemoryDatabase("calc-di-" + Guid.NewGuid()));
services.AddDbContext<ClaimPoolContext>(o => o.UseInMemoryDatabase("claim-di-" + Guid.NewGuid()));
services.AddDbContext<InstallationPoolContext>(o => o.UseInMemoryDatabase("installation-di-" + Guid.NewGuid()));
services.AddDbContext<LocationContext>(o => o.UseInMemoryDatabase("location-di-" + Guid.NewGuid()));
services.AddBlobPoolPersistenceInjections();
services.AddCalculationPoolPersistenceInjections();
services.AddClaimPoolPersistenceInjections();
services.AddInstallationPoolPersistenceInjections();
services.AddLocationPoolPersistenceInjections();
using var provider = services.BuildServiceProvider();
using var firstScope = provider.CreateScope();
using var secondScope = provider.CreateScope();
foreach (var item in items)
{
    var assembly = item.Module.Contains("BlobPool") ? typeof(BlobPoolContext).Assembly
        : item.Module.Contains("ClaimPool") ? typeof(ClaimPoolContext).Assembly
        : item.Module.Contains("InstallationPool") ? typeof(InstallationPoolContext).Assembly
        : item.Module.Contains("LocationPool") ? typeof(LocationContext).Assembly : typeof(CalculationPoolContext).Assembly;
    var repository = assembly.GetType(item.RepositoryType, true)!;
    Check(ConstructorFailure(repository) == ConstructorFailure(frozen.GetType("Frozen." + item.CommandType, true)!), item.Name + " preserves null-UnitOfWork rejection");
    var contracts = repository.GetInterfaces();
    Check(contracts.Length == 2, item.Name + " implements exactly the two original contracts");
    var first = firstScope.ServiceProvider.GetRequiredService(contracts[0]);
    Check(ReferenceEquals(first, firstScope.ServiceProvider.GetRequiredService(contracts[1])), item.Name + " contracts share one scoped instance");
    Check(!ReferenceEquals(first, secondScope.ServiceProvider.GetRequiredService(contracts[0])), item.Name + " is isolated across scopes");
    foreach (var contract in contracts)
        Check(services.Count(d => d.ServiceType == contract) == 1, item.Name + " has exactly one DI mapping for " + contract.Name);
    var uowField = repository.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Single(f => f.Name == "_uow");
    Check(ReferenceEquals(uowField.GetValue(first), firstScope.ServiceProvider.GetRequiredService(uowField.FieldType)), item.Name + " keeps the scoped UnitOfWork");
    foreach (var oldName in new[] { item.CommandType, item.QueryType })
    {
        var oldType = frozen.GetType("Frozen." + oldName, true)!;
        foreach (var method in oldType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        foreach (var mode in item.Name == "CompanyServiceOffering" && method.Name == "Get" && method.GetParameters().Length == 1
                     ? new[] { "populated", "missing", "null", "single", "single-missing" }
                     : new[] { "populated", "missing", "null" })
        {
            var before = await Execute(oldType, method, item.Module, mode);
            var afterMethod = repository.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray())!;
            var after = await Execute(repository, afterMethod, item.Module, mode);
            Check(before == after, item.Name + "." + method.Name + " " + mode + Environment.NewLine + before + Environment.NewLine + after);
            scenarios++;
        }
    }
    Console.WriteLine("PASS runtime/DI: " + item.Name);
}
Check(items.Count == expectedCount, "Exactly the approved batch was exercised");
Check(maximumLines <= 100, "All merged classes remain below 100 physical lines");
Console.WriteLine($"PASS: {checks} checks; {items.Count} repositories; {methodsChecked} unchanged operations; {scenarios} before/after EF InMemory scenarios; maximum class length {maximumLines} lines. No SQL Server connection was used.");

static async Task<string> Execute(Type serviceType, MethodInfo method, string module, string mode)
{
    var database = Guid.NewGuid().ToString();
    DbContext Create() => module.Contains("BlobPool")
        ? new BlobPoolContext(new DbContextOptionsBuilder<BlobPoolContext>().UseInMemoryDatabase(database).Options)
        : module.Contains("ClaimPool")
            ? new ClaimPoolContext(new DbContextOptionsBuilder<ClaimPoolContext>().UseInMemoryDatabase(database).Options)
            : module.Contains("InstallationPool")
                ? new InstallationPoolContext(new DbContextOptionsBuilder<InstallationPoolContext>().UseInMemoryDatabase(database).Options)
                : module.Contains("LocationPool")
                    ? new LocationContext(new DbContextOptionsBuilder<LocationContext>().UseInMemoryDatabase(database).Options)
                    : new CalculationPoolContext(new DbContextOptionsBuilder<CalculationPoolContext>().UseInMemoryDatabase(database).Options);
    using var context = Create();
    var isRequestUser = serviceType.Name.StartsWith("RequestUser");
    var entityType = isRequestUser ? context.Model.GetEntityTypes().Single(e => e.ClrType.Name == "RequestWaterMeter").ClrType
        : serviceType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(f => f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(DbSet<>)).FieldType.GenericTypeArguments[0];
    var metadata = context.Model.FindEntityType(entityType)!;
    var seedCount = mode.StartsWith("single") ? 1 : 3;
    for (var i = 1; i <= seedCount; i++)
    {
        var entity = Entity(metadata, i, 0);
        if (entityType.Name == "RequestWaterMeter") entityType.GetProperty("BillId")!.SetValue(entity, i == 1 ? "match" : "other-" + i);
        if (entityType.Name == "WaterMeter")
        {
            entityType.GetProperty("BillId")!.SetValue(entity, (1000 + i).ToString());
            entityType.GetProperty("InstallationDate")!.SetValue(entity, i == 3 ? "2027/01/15" : "2026/01/15");
        }
        context.Add(entity);
    }
    await context.SaveChangesAsync();
    context.ChangeTracker.Clear();
    var service = Activator.CreateInstance(serviceType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { context }, null)!;
    var isRemove = method.Name == "Remove";
    object? Argument(Type type)
    {
        if (mode == "null" && !type.IsValueType) return null;
        if (type == entityType)
            return isRemove ? context.Find(entityType, KeyValues(metadata, mode == "missing" ? 404 : 1))! : Entity(metadata, 4, 0);
        if (type.Name == "RequestUser") return Activator.CreateInstance(type);
        if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(ICollection<>) || type.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
        {
            var element = type.GenericTypeArguments[0];
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(element))!;
            if (mode == "missing") return list;
            for (var i = 1; i <= 2; i++)
                list.Add(element == entityType ? (isRemove ? context.Find(entityType, KeyValues(metadata, i))! : Entity(metadata, i + 3, 0)) : Scalar(element, i));
            return list;
        }
        return type == typeof(string) ? (mode.EndsWith("missing") ? "missing" : "match") : Scalar(type, mode.EndsWith("missing") ? 404 : 1);
    }
    var dateRange = method.GetParameters().Length == 2 && method.GetParameters().All(p => p.ParameterType == typeof(string));
    var args = method.GetParameters().Select((p, i) => dateRange && mode != "null"
        ? (object)(mode == "missing" ? (i == 0 ? "1406/04/01" : "1406/06/31") : (i == 0 ? "1405/04/01" : "1405/06/31"))
        : Argument(p.ParameterType)).ToArray();
    if (entityType.Name == "WaterMeter" && method.GetParameters().Length == 7 && mode != "null")
        args = new object[] { mode == "missing" ? "2028/01/01" : "2026/01/01", mode == "missing" ? "2028/12/31" : "2026/12/31", (short)1, (short)1, "1000", "2000", 1 };
    object? result = null;
    string? exception = null;
    try
    {
        var isAsyncVoid = method.ReturnType == typeof(void) && method.GetCustomAttribute<System.Runtime.CompilerServices.AsyncStateMachineAttribute>() != null;
        var capture = new CapturingContext();
        var previous = SynchronizationContext.Current;
        try
        {
            if (isAsyncVoid) SynchronizationContext.SetSynchronizationContext(capture);
            result = method.Invoke(service, args);
        }
        finally { if (isAsyncVoid) SynchronizationContext.SetSynchronizationContext(previous); }
        if (capture.Exceptions.Count != 0) throw capture.Exceptions.Single();
        if (result is Task task)
        {
            await task;
            result = method.ReturnType.IsGenericType ? task.GetType().GetProperty("Result")?.GetValue(task) : null;
        }
    }
    catch (Exception e)
    {
        while (e is TargetInvocationException && e.InnerException != null) e = e.InnerException;
        exception = e.GetType().FullName + ":" + e.Message;
        result = null;
    }
    var knownUnfilteredSingle = serviceType.Name.StartsWith("CompanyServiceOffering") && method.Name == "Get" && method.GetParameters().Length == 1;
    if (knownUnfilteredSingle && seedCount == 3)
    {
        if (exception != typeof(InvalidOperationException).FullName + ":Sequence contains more than one element")
            throw new InvalidOperationException("The pre-existing unfiltered Single behavior changed: " + exception);
    }
    else if (isRequestUser && method.Name.StartsWith("Get"))
    {
        if (exception == null || !exception.StartsWith(typeof(InvalidOperationException).FullName + ":"))
            throw new InvalidOperationException("The pre-existing unmapped RequestUser query behavior changed: " + exception);
    }
    else if ((mode == "populated" || mode.StartsWith("single")) && method.Name.StartsWith("Get"))
    {
        if (exception != null || result == null || result is ICollection collection && collection.Count == 0)
            throw new InvalidOperationException("Populated query must return data: " + serviceType.Name + "." + method.Name + " " + exception);
    }
    var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };
    var value = JsonSerializer.Serialize(result, options);
    var tracking = context.ChangeTracker.Entries().Select(e => e.Metadata.ClrType.Name + ":" + string.Join(",", e.Metadata.FindPrimaryKey()!.Properties.Select(p => e.Property(p.Name).CurrentValue)) + ":" + e.State).Order().ToArray();
    using var observer = Create();
    var persisted = (int)typeof(Helpers).GetMethod(nameof(Helpers.Count))!.MakeGenericMethod(entityType).Invoke(null, new object[] { observer })!;
    if (persisted != seedCount) throw new InvalidOperationException("An operation committed changes implicitly: " + method.Name);
    return JsonSerializer.Serialize(new { value, exception, tracking, persisted });
}

static object Entity(IEntityType metadata, int index, int depth, Dictionary<(Type, int), object>? graph = null)
{
    graph ??= new();
    if (graph.TryGetValue((metadata.ClrType, index), out var existing)) return existing;
    var entity = Activator.CreateInstance(metadata.ClrType)!;
    graph[(metadata.ClrType, index)] = entity;
    foreach (var property in metadata.GetProperties().Where(p => p.PropertyInfo?.CanWrite == true))
    {
        if (property.ClrType == typeof(string)) property.PropertyInfo!.SetValue(entity,
            property.Name == "FromDateJalali" ? "1405/01/01" : property.Name == "ToDateJalali" ? "1405/12/29" : index <= 2 ? "match" : "other");
        else if (property.IsPrimaryKey()) property.PropertyInfo!.SetValue(entity, Scalar(property.ClrType, index));
        else if (property.ClrType == typeof(byte[])) property.PropertyInfo!.SetValue(entity, new byte[] { 1, 2 });
    }
    if (depth < 2)
    foreach (var foreignKey in metadata.GetForeignKeys())
    {
        var navigation = foreignKey.DependentToPrincipal?.PropertyInfo;
        if (navigation?.CanWrite != true || foreignKey.PrincipalEntityType.ClrType == metadata.ClrType) continue;
        var principal = Entity(foreignKey.PrincipalEntityType, index, depth + 1, graph);
        navigation.SetValue(entity, principal);
        for (var k = 0; k < foreignKey.Properties.Count; k++)
            foreignKey.Properties[k].PropertyInfo?.SetValue(entity, foreignKey.PrincipalKey.Properties[k].PropertyInfo?.GetValue(principal));
    }
    return entity;
}
static object[] KeyValues(IEntityType metadata, int index) => metadata.FindPrimaryKey()!.Properties.Select(p => Scalar(p.ClrType, index)).ToArray();
static object Scalar(Type type, int index)
{
    type = Nullable.GetUnderlyingType(type) ?? type;
    if (type == typeof(Guid)) return Guid.Parse($"00000000-0000-0000-0000-{index:D12}");
    if (type.IsEnum) return Enum.ToObject(type, index);
    if (type == typeof(DateTime)) return new DateTime(2026, 1, 1).AddDays(index);
    return Convert.ChangeType(index, type);
}
static string ConstructorFailure(Type type)
{
    try
    {
        Activator.CreateInstance(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new object?[] { null }, null);
        throw new InvalidOperationException("Constructor accepted a null UnitOfWork: " + type.Name);
    }
    catch (TargetInvocationException e)
    {
        return e.InnerException!.GetType().FullName + ":" + e.InnerException.Message;
    }
}
static string FindRoot()
{
    for (var directory = new DirectoryInfo(Environment.CurrentDirectory); directory != null; directory = directory.Parent)
        if (File.Exists(Path.Combine(directory.FullName, "AGENTS.md")) && Directory.Exists(Path.Combine(directory.FullName, "Aban360.Api"))) return directory.FullName;
    throw new DirectoryNotFoundException("Run this executable inside the native Aban360 checkout.");
}
record Item(string Name, string Module, string CommandPath, string QueryPath, string RepositoryPath, string CommandType, string QueryType, string RepositoryType, Dictionary<string, string> FieldMap, Dictionary<string, string> Contracts);
class RenameFields(Dictionary<string, string> names) : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node) => names.TryGetValue(node.Identifier.Text, out var name)
        ? node.WithIdentifier(SyntaxFactory.Identifier(name).WithTriviaFrom(node.Identifier)) : base.VisitIdentifierName(node);
}
class FrozenNamespace : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitNamespaceDeclaration(NamespaceDeclarationSyntax node) => base.VisitNamespaceDeclaration(node.WithName(SyntaxFactory.ParseName("Frozen." + node.Name)));
}
static class Helpers
{
    public static int Count<T>(DbContext context) where T : class => context.Set<T>().Count();
}
sealed class CapturingContext : SynchronizationContext
{
    public List<Exception> Exceptions { get; } = new();
    public override void Post(SendOrPostCallback callback, object? state)
    {
        try { callback(state); }
        catch (Exception exception) { Exceptions.Add(exception); }
    }
}

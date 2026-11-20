using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Aban360.Tests.CompactSyntax;

public sealed record Binding(string Type, string Namespace, Dictionary<string, string> Methods);
public sealed record Operation(string Group, string Module, string Source, string ContractSource, string OldName, string OldContract, string OldNamespace, string OldContractNamespace, string Namespace, string Service, string Output, string Snapshot, string ContractSnapshot, string Hash, string ContractHash, bool Primary, Dictionary<string, string> Names);
public sealed record Consumer(string Path, string Snapshot, string Hash, Dictionary<string, Binding> Bindings);

public sealed class Rename(IReadOnlyDictionary<string, string> names) : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node) => node.Expression is ThisExpressionSyntax
        ? base.VisitMemberAccessExpression(node)
        : node.WithExpression((ExpressionSyntax)Visit(node.Expression)!).WithName(node.Name);
    public override SyntaxToken VisitToken(SyntaxToken token) => token.IsKind(SyntaxKind.IdentifierToken) && names.TryGetValue(token.ValueText, out var name)
        ? SyntaxFactory.Identifier(token.LeadingTrivia, name, token.TrailingTrivia) : base.VisitToken(token);
}

// Rebind only selected injected contracts. Action bodies, attributes and data values
// otherwise retain their executable syntax. Duplicate references to one scoped
// service become a single constructor dependency with the same null guard.
public sealed class ConsumerRewrite(IReadOnlyDictionary<string, Binding> bindings) : CSharpSyntaxRewriter
{
    public static string Tokens(SyntaxNode node) => string.Join("|", node.DescendantTokens().Select(t => t.RawKind + ":" + t.Text));
    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var names = new Dictionary<string, string>();
        var calls = new Dictionary<string, Dictionary<string, string>>();
        var fieldByService = new Dictionary<string, string>();
        foreach (var field in node.Members.OfType<FieldDeclarationSyntax>())
        {
            if (!bindings.TryGetValue(field.Declaration.Type.ToString(), out var binding)) continue;
            string target = "_" + char.ToLowerInvariant(binding.Type[1]) + binding.Type[2..];
            if (fieldByService.TryGetValue(binding.Type, out var existing)) target = existing;
            else fieldByService[binding.Type] = target;
            foreach (var variable in field.Declaration.Variables) { names[variable.Identifier.ValueText] = target; calls[variable.Identifier.ValueText] = binding.Methods; }
        }
        var selectedParameters = node.Members.OfType<ConstructorDeclarationSyntax>().SelectMany(c => c.ParameterList.Parameters)
            .Concat(node.ParameterList?.Parameters ?? default);
        foreach (var parameter in selectedParameters)
        {
            if (parameter.Type is null || !bindings.TryGetValue(parameter.Type.ToString(), out var binding)) continue;
            string target = char.ToLowerInvariant(binding.Type[1]) + binding.Type[2..];
            names[parameter.Identifier.ValueText] = target;
            calls[parameter.Identifier.ValueText] = binding.Methods;
        }
        var rewrite = new BoundRewrite(bindings, names, calls);
        var updated = (ClassDeclarationSyntax)rewrite.Visit(node)!;
        var selectedTypes = bindings.Values.Select(b => b.Type).ToHashSet();
        var seenFields = new HashSet<string>();
        var members = new List<MemberDeclarationSyntax>();
        foreach (var member in updated.Members)
        {
            if (member is FieldDeclarationSyntax field && selectedTypes.Contains(field.Declaration.Type.ToString()) && !seenFields.Add(field.Declaration.Variables.Single().Identifier.ValueText)) continue;
            if (member is ConstructorDeclarationSyntax ctor)
            {
                if (ctor.Body is null) throw new InvalidOperationException("Expression-bodied consumer constructor requires explicit review: " + node.Identifier);
                members.Add(ctor.WithParameterList(Deduplicate(ctor.ParameterList)).WithBody(ctor.Body.WithStatements(SyntaxFactory.List(ctor.Body.Statements.DistinctBy(Tokens)))));
            }
            else if (member is ClassDeclarationSyntax nested) members.Add((MemberDeclarationSyntax)new ConsumerRewrite(bindings).Visit(nested)!);
            else members.Add(member);
        }
        updated = updated.WithMembers(SyntaxFactory.List(members));
        return updated.ParameterList is null ? updated : updated.WithParameterList(Deduplicate(updated.ParameterList));
    }
    static ParameterListSyntax Deduplicate(ParameterListSyntax list) => list.WithParameters(SyntaxFactory.SeparatedList(list.Parameters.DistinctBy(p => p.Identifier.ValueText)));
    sealed class BoundRewrite(IReadOnlyDictionary<string, Binding> bindings, IReadOnlyDictionary<string, string> names, IReadOnlyDictionary<string, Dictionary<string, string>> calls) : CSharpSyntaxRewriter
    {
        public override SyntaxToken VisitToken(SyntaxToken token) => token.IsKind(SyntaxKind.IdentifierToken) && names.TryGetValue(token.ValueText, out var target)
            ? SyntaxFactory.Identifier(token.LeadingTrivia, target, token.TrailingTrivia) : base.VisitToken(token);
        public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            string field = node.Expression is MemberAccessExpressionSyntax { Expression: ThisExpressionSyntax } access ? access.Name.Identifier.ValueText : node.Expression.ToString();
            if (calls.TryGetValue(field, out var methods) && methods.TryGetValue(node.Name.Identifier.ValueText, out var method))
                node = node.WithName(SyntaxFactory.IdentifierName(method).WithTriviaFrom(node.Name));
            return base.VisitMemberAccessExpression(node);
        }
        public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
        {
            string name = node.Identifier.ValueText;
            if (bindings.TryGetValue(name, out var binding)) return node.WithIdentifier(SyntaxFactory.Identifier(node.Identifier.LeadingTrivia, binding.Type, node.Identifier.TrailingTrivia));
            if (names.TryGetValue(name, out var target)) return node.WithIdentifier(SyntaxFactory.Identifier(node.Identifier.LeadingTrivia, target, node.Identifier.TrailingTrivia));
            return base.VisitIdentifierName(node);
        }
    }
}

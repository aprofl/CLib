using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

public class ComplexityAnalyzer
{
    public int CalculateComplexity(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();

        var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        var compilation = CSharpCompilation.Create("CodeAnalysis")
            .AddReferences(mscorlib)
            .AddSyntaxTrees(tree);

        var model = compilation.GetSemanticModel(tree);

        return root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .Max(method => CalculateMethodComplexity(method, model));
    }

    private int CalculateMethodComplexity(MethodDeclarationSyntax method, SemanticModel model)
    {
        var body = method.Body ?? method.ExpressionBody?.Expression as SyntaxNode;
        if (body == null)
            return 1;

        var complexity = 1;
        foreach (var statement in body.DescendantNodesAndSelf().OfType<StatementSyntax>())
            complexity += CalculateStatementComplexity(statement, model);

        return complexity;
    }

    private int CalculateStatementComplexity(StatementSyntax statement, SemanticModel model)
    {
        switch (statement)
        {
            case IfStatementSyntax _:
            case ForStatementSyntax _:
            case ForEachStatementSyntax _:
            case WhileStatementSyntax _:
            case DoStatementSyntax _:
                return 1;
            case SwitchStatementSyntax switchStatement:
                return 1 + switchStatement.Sections.Sum(section => section.Labels.Count);
            default:
                return 0;
        }
    }
}

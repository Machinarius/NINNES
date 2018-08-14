using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NINNES.RoslynAnalyzers {
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class ForbidMultiplicationAnalyzer : DiagnosticAnalyzer {
    public const string DiagnosticId = "NESMultiplicationForbidden";

    private static readonly LocalizableString Title =
      new LocalizableResourceString(nameof(Resources.MultiplicationForbiddenTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description =
      new LocalizableResourceString(nameof(Resources.MultiplicationForbiddenDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Message =
      new LocalizableResourceString(nameof(Resources.MultiplicationForbiddenDescription), Resources.ResourceManager, typeof(Resources));

    private static DiagnosticDescriptor MultiplicationForbiddenRule =
      new DiagnosticDescriptor(DiagnosticId, Title, Message, AnalyzerCategories.NESCPULimitations, DiagnosticSeverity.Error, true, Description);

    #region Overrides of DiagnosticAnalyzer

    public override void Initialize(AnalysisContext context) {
      context.RegisterCodeBlockAction(PerformCodeBlockAnalysis);
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
      ImmutableArray.Create(MultiplicationForbiddenRule);

    #endregion

    private void PerformCodeBlockAnalysis(CodeBlockAnalysisContext context) {
      var node = context.CodeBlock;
      var multiplicationSubnodes = node.DescendantNodes()
                                       .Where(cNode => cNode is BinaryExpressionSyntax)
                                       .Cast<BinaryExpressionSyntax>()
                                       .Where(expression => expression.IsKind(SyntaxKind.MultiplyExpression));

      foreach (var multNode in multiplicationSubnodes) {
        var errorDiagnostic = Diagnostic.Create(MultiplicationForbiddenRule, multNode.GetLocation());
        context.ReportDiagnostic(errorDiagnostic);
      }

      var multAssignmentNodes = node.DescendantNodes()
                                    .Where(cNode => cNode is AssignmentExpressionSyntax)
                                    .Cast<AssignmentExpressionSyntax>()
                                    .Where(expression => expression.IsKind(SyntaxKind.MultiplyAssignmentExpression));
      foreach (var assignmentNode in multAssignmentNodes) {
        var errorDiagnostic = Diagnostic.Create(MultiplicationForbiddenRule, assignmentNode.OperatorToken.GetLocation());
        context.ReportDiagnostic(errorDiagnostic);
      }
    }
  }
}

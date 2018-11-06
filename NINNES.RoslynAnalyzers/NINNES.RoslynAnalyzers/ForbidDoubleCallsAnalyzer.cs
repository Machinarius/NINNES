using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NINNES.RoslynAnalyzers {
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class ForbidDoubleCallsAnalyzer : DiagnosticAnalyzer {
    #region Boilerplate
    public const string DiagnosticId = "NESDoesNotSupportDecimmalCalls";

    private static readonly LocalizableString Title =
      new LocalizableResourceString(nameof(Resources.DoubleMethodsTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description =
      new LocalizableResourceString(nameof(Resources.DoubleMethodsDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Message =
      new LocalizableResourceString(nameof(Resources.DoubleMethodsMessage), Resources.ResourceManager, typeof(Resources));

    private static readonly DiagnosticDescriptor DoubleMethodInvocationsForbiddenRule =
      new DiagnosticDescriptor(DiagnosticId, Title, Description, AnalyzerCategories.NESCPULimitations, DiagnosticSeverity.Error, true, Message);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
      ImmutableArray.Create(DoubleMethodInvocationsForbiddenRule);
    #endregion

    public override void Initialize(AnalysisContext context) {
      context.RegisterSyntaxNodeAction(OnMethodInvocationFound, SyntaxKind.InvocationExpression);
    }

    private void OnMethodInvocationFound(SyntaxNodeAnalysisContext context) {
      // More info: https://johnkoerner.com/csharp/working-with-types-in-your-analyzer/
      var invExpr = (InvocationExpressionSyntax) context.Node;
      var methodInfo = (IMethodSymbol)context.SemanticModel.GetSymbolInfo(invExpr.Expression).Symbol;
      var returnType = methodInfo.ReturnType;
      if (returnType.SpecialType != SpecialType.System_Double) {
        return;
      }
      
      var errorDiagnostic = Diagnostic.Create(DoubleMethodInvocationsForbiddenRule, invExpr.Expression.GetLocation());
      context.ReportDiagnostic(errorDiagnostic);
    }
  }
}

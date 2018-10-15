using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NINNES.RoslynAnalyzers {
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class ForbidFloatDeclarationsAnalyzer : DiagnosticAnalyzer {
    public const string DiagnosticId = "NESDoesNotSupportDecimals";

    private static readonly LocalizableString Title =
      new LocalizableResourceString(nameof(Resources.FloatForbiddenTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description =
      new LocalizableResourceString(nameof(Resources.FloatForbiddenDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Message =
      new LocalizableResourceString(nameof(Resources.FloatForbiddenMessage), Resources.ResourceManager, typeof(Resources));

    private static readonly DiagnosticDescriptor FloatForbiddenRule =
      new DiagnosticDescriptor(DiagnosticId, Title, Description, AnalyzerCategories.NESCPULimitations, DiagnosticSeverity.Error, true, Message);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
      ImmutableArray.Create(FloatForbiddenRule);

    public override void Initialize(AnalysisContext context) {
      context.RegisterSyntaxNodeAction(OnPredefinedTypeNodeFound, SyntaxKind.PredefinedType);
      context.RegisterSyntaxNodeAction(OnNumericLiteralFound, SyntaxKind.NumericLiteralExpression);
    }

    private void OnNumericLiteralFound(SyntaxNodeAnalysisContext context) {
      var errorDiagnostic = Diagnostic.Create(FloatForbiddenRule, context.Node.GetLocation());
      context.ReportDiagnostic(errorDiagnostic);
    }

    private void OnPredefinedTypeNodeFound(SyntaxNodeAnalysisContext context) {
      var childToken = context.Node.ChildTokens().FirstOrDefault();
      if (childToken == null || !childToken.IsKind(SyntaxKind.FloatKeyword)) {
        return;
      }

      var errorDiagnostic = Diagnostic.Create(FloatForbiddenRule, context.Node.GetLocation());
      context.ReportDiagnostic(errorDiagnostic);
    }
  }
}

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace NINNES.RoslynAnalyzers {
  [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ForbidMultiplicationFixProvider)), Shared]
  public class ForbidMultiplicationFixProvider : CodeFixProvider {
    public const string Title = "Invoke NESMath.Multiply instead";

    public override ImmutableArray<string> FixableDiagnosticIds =>
      ImmutableArray.Create(ForbidMultiplicationAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() {
      return WellKnownFixAllProviders.BatchFixer;
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context) {
      var docRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
      var diagnostic = context.Diagnostics.First();
      var diagnosticSpan = diagnostic.Location.SourceSpan;

      Func<CancellationToken, Task<Document>> fixFunc = null;

      var node = docRoot.FindNode(diagnosticSpan);
      if (node.IsKind(SyntaxKind.MultiplyExpression)) {
        fixFunc = async (ct) => await FixMultiplyExpressionAsync(ct);
      } else if (node.IsKind(SyntaxKind.MultiplyAssignmentExpression)) {
        fixFunc = async (ct) => await FixMultiplyAssignmentExpressionAsync(ct, context.Document, node);
      } else {
        throw new InvalidOperationException("Cannot code fix an invalid diagnostic");
      }

      var codeFix = CodeAction.Create(
        title: Title,
        createChangedDocument: fixFunc,
        equivalenceKey: Title
      );

      context.RegisterCodeFix(codeFix, diagnostic);
    }

    private Task<Document> FixMultiplyExpressionAsync(CancellationToken cancelToken) {
      throw new NotImplementedException();
    }

    private async Task<Document> FixMultiplyAssignmentExpressionAsync(CancellationToken cancelToken, Document document, SyntaxNode node) {
      var variableNode = node.ChildNodes().First(child => child.IsKind(SyntaxKind.IdentifierName));
      var variableName = variableNode.ChildTokens().First().Text;
      var valueNode = node.ChildNodes().First(child => child.IsKind(SyntaxKind.NumericLiteralExpression));
      var valueText = valueNode.ChildTokens().First().Text;
      var valueInteger = int.Parse(valueText);

      // https://johnkoerner.com/csharp/creating-code-using-the-syntax-factory/
      // https://joshvarty.com/2015/08/18/learn-roslyn-now-part-12-the-documenteditor/
      var nesMath = SyntaxFactory.IdentifierName("NESMath");
      var multiply = SyntaxFactory.IdentifierName("Multiply");
      var multiplyAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, nesMath, multiply);

      var variableArgument = SyntaxFactory.Argument(SyntaxFactory.IdentifierName(variableName));
      var valueToken = SyntaxFactory.Literal(valueInteger);
      var valueSyntax = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, valueToken);
      var valueArgument = SyntaxFactory.Argument(valueSyntax);
      var argumentsList = SyntaxFactory.SeparatedList(new[] { variableArgument, valueArgument });
      var invocation = SyntaxFactory.InvocationExpression(multiplyAccess, SyntaxFactory.ArgumentList(argumentsList));
      var assignment = SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(variableName), invocation);
      
      var trackingAnnotation = new SyntaxAnnotation();
      assignment = assignment
        .WithAdditionalAnnotations(trackingAnnotation)
        .WithTriviaFrom(node);

      var syntaxRoot = await document.GetSyntaxRootAsync();
      var modifiedRoot = syntaxRoot.ReplaceNode(node, assignment);
      var modifiedNode = modifiedRoot.GetAnnotatedNodes(trackingAnnotation).First();

      var modifiedDocument = document.WithSyntaxRoot(modifiedRoot);
      modifiedDocument = await AddUsingStatementAsync(cancelToken, modifiedDocument);

      return modifiedDocument;
    }

    private async Task<Document> AddUsingStatementAsync(CancellationToken cancelToken, Document document) {
      var editor = await DocumentEditor.CreateAsync(document, cancelToken);
      var syntaxRoot = await document.GetSyntaxRootAsync();
      var usingStatement = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NINNES.Platform.Shims"));

      var compilationUnit = (CompilationUnitSyntax)syntaxRoot;
      var newCompilationUnit = compilationUnit.AddUsings(usingStatement);

      var modifiedDocument = document.WithSyntaxRoot(newCompilationUnit);
      return modifiedDocument;
    }
  }
}

﻿using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NINNES.RoslynAnalyzers {
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class ForbidMultiplicationAnalyzer : DiagnosticAnalyzer {
    #region Boilerplate

    public const string DiagnosticId = DiagnosticIds.MultiplicationForbidden;

    private static readonly LocalizableString Title =
      new LocalizableResourceString(nameof(Resources.MultiplicationForbiddenTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description =
      new LocalizableResourceString(nameof(Resources.MultiplicationForbiddenDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Message =
      new LocalizableResourceString(nameof(Resources.MultiplicationForbiddenDescription), Resources.ResourceManager, typeof(Resources));

    private static readonly DiagnosticDescriptor MultiplicationForbiddenRule =
      new DiagnosticDescriptor(DiagnosticId, Title, Message, AnalyzerCategories.NESCPULimitations, DiagnosticSeverity.Error, true, Description);
    #endregion
    
    #region Configuration
    public override void Initialize(AnalysisContext context) {
      context.RegisterSyntaxNodeAction(PerformNodeAnalysis, SyntaxKind.MultiplyExpression, SyntaxKind.MultiplyAssignmentExpression);
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
      ImmutableArray.Create(MultiplicationForbiddenRule);
    #endregion

    #region Diagnostic Creation
    private void PerformNodeAnalysis(SyntaxNodeAnalysisContext context) {
      var node = context.Node;
      switch (node.Kind()) {
        case SyntaxKind.MultiplyExpression:
          AnalyzeMultiplyExpression(context, (BinaryExpressionSyntax)node);
          break;
        case SyntaxKind.MultiplyAssignmentExpression:
          AnalyzeMultiplyAssignmentExpression(context, node);
          break;
        default:
          throw new InvalidOperationException("Unrecognized node type");
      }
    }

    private void AnalyzeMultiplyAssignmentExpression(SyntaxNodeAnalysisContext context, SyntaxNode node) {
      var errorDiagnostic = Diagnostic.Create(MultiplicationForbiddenRule, node.GetLocation());
      context.ReportDiagnostic(errorDiagnostic);
    }

    private void AnalyzeMultiplyExpression(SyntaxNodeAnalysisContext context, BinaryExpressionSyntax node) {
      var errorDiagnostic = Diagnostic.Create(MultiplicationForbiddenRule, node.GetLocation());
      context.ReportDiagnostic(errorDiagnostic);
    }
    #endregion
  }
}

﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NINNES.RoslynAnalyzers.Tests.Assets;
using NINNES.RoslynAnalyzers.Tests.Helpers;

namespace NINNES.RoslynAnalyzers.Tests {
  [TestClass]
  public class ForbidMultiplicationAnalyzerTests : CodeFixVerifier {

    [TestMethod]
    public void CodeWithoutMultiplicationsMayNotTriggerADiagnostic() {
      var testProgram = "";
      VerifyCSharpDiagnostic(testProgram);
    }

    [TestMethod]
    public void CodeWithMultiplicationsMustTriggerAnErrorDiagnostic() {
      var testProgram = TestAssetsReader.ReadTestAsset("SimpleMultiplication.cs");
      var multiplicationDiagnostic = new DiagnosticResult {
        Id = "NESMultiplicationForbidden",
        Message = "Use bit-shifting multiplication or NESMath.Multiply to multiply two numbers with an algorithm that can be run on the NES CPU",
        Severity = DiagnosticSeverity.Error,
        Locations = new[] {
          new DiagnosticResultLocation("Test0.cs", 4, 18)
        }
      };

      VerifyCSharpDiagnostic(testProgram, multiplicationDiagnostic);
    }

    [TestMethod]
    public void CodeWithMultiplicationAssignmentsMustTriggerAnErrorDiagnostic() {
      var testProgram = TestAssetsReader.ReadTestAsset("MultiplicationAssignment.cs");
      var multiplicationAssignmentDiagnostic = new DiagnosticResult {
        Id = "NESMultiplicationForbidden",
        Message = "Use bit-shifting multiplication or NESMath.Multiply to multiply two numbers with an algorithm that can be run on the NES CPU",
        Severity = DiagnosticSeverity.Error,
        Locations = new[] {
          new DiagnosticResultLocation("Test0.cs", 5, 7)
        }                                                              
      };

      VerifyCSharpDiagnostic(testProgram, multiplicationAssignmentDiagnostic);
    }

    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() {
      return new ForbidMultiplicationAnalyzer();
    }
  }
}
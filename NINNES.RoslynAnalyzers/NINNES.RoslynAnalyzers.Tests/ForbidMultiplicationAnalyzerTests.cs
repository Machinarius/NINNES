using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
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
          new DiagnosticResultLocation("Test0.cs", 7, 24)
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
          new DiagnosticResultLocation("Test0.cs", 8, 13)
        }                                                              
      };

      VerifyCSharpDiagnostic(testProgram, multiplicationAssignmentDiagnostic);
    }

    [TestMethod]
    public void AMultiplicationAssignmentForbiddenDiagnosticMustBeFixable() {
      var badProgram = TestAssetsReader.ReadTestAsset("MultiplicationAssignment.cs");
      var fixedProgram = TestAssetsReader.ReadTestAsset("MultiplicationAssignmentFixed.cs");
      VerifyCSharpFix(badProgram, fixedProgram);
    }
    
    [TestMethod]
    public void ASimpleMultiplicationForbiddenDiagnosticMustBeFixable() {
      var badProgram = TestAssetsReader.ReadTestAsset("SimpleMultiplication.cs");
      var fixedProgram = TestAssetsReader.ReadTestAsset("SimpleMultiplicationFixed.cs");
      VerifyCSharpFix(badProgram, fixedProgram);
    }

    [TestMethod]
    public void MultiplicationDiagnosticsWithVariablesMustBeFixable() {
      var badProgram = TestAssetsReader.ReadTestAsset("MultiplicationsWithVariables.cs");
      var fixedProgram = TestAssetsReader.ReadTestAsset("MultiplicationsWithVariablesFixed.cs");
      VerifyCSharpFix(badProgram, fixedProgram);
    }

    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() {
      return new ForbidMultiplicationAnalyzer();
    }

    protected override CodeFixProvider GetCSharpCodeFixProvider() {
      return new ForbidMultiplicationFixProvider();
    }
  }
}

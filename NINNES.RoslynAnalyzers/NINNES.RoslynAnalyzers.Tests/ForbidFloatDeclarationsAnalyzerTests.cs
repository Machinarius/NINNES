using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NINNES.RoslynAnalyzers.Tests.Assets;
using NINNES.RoslynAnalyzers.Tests.Helpers;

namespace NINNES.RoslynAnalyzers.Tests {
  [TestClass]
  public class ForbidFloatDeclarationsAnalyzerTests : CodeFixVerifier {
    [TestMethod]
    public void FloatLiteralsMustBeInvalidated() {
      var testProgram = TestAssetsReader.ReadTestAsset("FloatLiteral.cs");
      var expectedDiagnostic = new DiagnosticResult {
        Id = "NESDoesNotSupportDecimals",
        Message = "Rework your code to not depend on float nor double",
        Severity = DiagnosticSeverity.Error,
        Locations = new[] {
          new DiagnosticResultLocation("Test0.cs", 9, 41)
        }
      };

      VerifyCSharpDiagnostic(testProgram, expectedDiagnostic);
    }

    [TestMethod]
    public void FloatMethodReturnsAndParametersMustBeInvalidated() {
      var testProgram = TestAssetsReader.ReadTestAsset("FloatMethodDeclaration.cs");
      var expectedDiagnostics = new [] { 
        new DiagnosticResult {
          Id = "NESDoesNotSupportDecimals",
          Message = "Rework your code to not depend on float nor double",
          Severity = DiagnosticSeverity.Error,
          Locations = new[] {
            new DiagnosticResultLocation("Test0.cs", 7, 10)
          }
        },
        new DiagnosticResult {
          Id = "NESDoesNotSupportDecimals",
          Message = "Rework your code to not depend on float nor double",
          Severity = DiagnosticSeverity.Error,
          Locations = new[] {
            new DiagnosticResultLocation("Test0.cs", 7, 33)
          }
        }
      };

      VerifyCSharpDiagnostic(testProgram, expectedDiagnostics);
    }

    [TestMethod]
    public void IntegerConstantsMustNotBeAffectedByDiagnostics() {
      var testProgram = TestAssetsReader.ReadTestAsset("IntegerConstant.cs");
      var expectedDiagnostics = new DiagnosticResult[0];
      VerifyCSharpDiagnostic(testProgram, expectedDiagnostics);
    }

    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() {
      return new ForbidFloatDeclarationsAnalyzer();
    }
  }
}

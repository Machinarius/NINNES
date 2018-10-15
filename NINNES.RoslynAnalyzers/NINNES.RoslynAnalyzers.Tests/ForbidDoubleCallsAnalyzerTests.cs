using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NINNES.RoslynAnalyzers.Tests.Assets;
using NINNES.RoslynAnalyzers.Tests.Helpers;

namespace NINNES.RoslynAnalyzers.Tests {
  [TestClass]
  public class ForbidDoubleCallsAnalyzerTests : CodeFixVerifier {
    [TestMethod]
    public void CallsToMethodsReturningDoubleMustBeInvalidated() {
      var testProgram = TestAssetsReader.ReadTestAsset("DoubleMethodCall.cs");
      var expectedDiagnostic = new DiagnosticResult {
        Id = "NESDoesNotSupportDecimmalCalls",
        Message = "Do not invoke methods that return Double",
        Severity = DiagnosticSeverity.Error,
        Locations = new[] {
          new DiagnosticResultLocation("Test0.cs", 9, 23)
        }
      };

      VerifyCSharpDiagnostic(testProgram, expectedDiagnostic);
    }
    
    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() {
      return new ForbidDoubleCallsAnalyzer();
    }
  }
}

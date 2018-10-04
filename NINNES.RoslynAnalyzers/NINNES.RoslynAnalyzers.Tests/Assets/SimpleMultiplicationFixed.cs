using NINNES.Platform.Shims;

namespace NINNES.RoslynAnalyzers.Tests.Assets {
  class SimpleMultiplication {
    public static void Multiply() {
      var test = NESMath.Multiply(2, 42);
    }
  }
}

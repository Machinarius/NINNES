using NINNES.Platform.Shims;

namespace NINNES.RoslynAnalyzers.Tests.Assets
{
    class MultiplicationAssignment
    {
        public void Multiply()
        {
            var test = 2;
            test = NESMath.Multiply(test, 42);
        }
    }
}

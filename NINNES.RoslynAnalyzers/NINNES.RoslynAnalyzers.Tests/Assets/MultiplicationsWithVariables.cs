namespace NINNES.RoslynAnalyzers.Tests.Assets
{
    public class MultiplicationsWithVariables
    {
        public void Multiply()
        {
            var test = 42;
            var test2 = 2;
            var result = test * test2;
            test2 *= test;
        }
    }
}

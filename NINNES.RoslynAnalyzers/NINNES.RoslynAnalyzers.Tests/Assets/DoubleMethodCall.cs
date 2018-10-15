using System;

namespace NINNES.RoslynAnalyzers.Tests.Assets
{
	public class DoubleMethodCall
	{
		public void DoDoubleStuff()
		{
			var methodResult = Math.Sqrt(42);
		}
	}
}

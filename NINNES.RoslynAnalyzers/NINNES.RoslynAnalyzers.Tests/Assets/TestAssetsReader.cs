using System.IO;

namespace NINNES.RoslynAnalyzers.Tests.Assets {
  public static class TestAssetsReader {
    public static string ReadTestAsset(string assetName) {
      var currentAssembly = typeof(TestAssetsReader).Assembly;
      var assetsNamespace = typeof(TestAssetsReader).Namespace;
      var assetPath = assetsNamespace + "." + assetName;

      using (var assetStream = currentAssembly.GetManifestResourceStream(assetPath))
      using (var assetReader = new StreamReader(assetStream)) {
        var assetText = assetReader.ReadToEnd();
        return assetText;
      }

    }
  }
}

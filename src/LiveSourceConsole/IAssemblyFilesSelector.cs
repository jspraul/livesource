using System.IO;

namespace LiveSource.LiveSourceConsole 
{
    public interface IAssemblyFilesSelector 
    {
        string[] SelectAssemblies();
    }

    public class MockAssemblyFilesSelector : IAssemblyFilesSelector
    {
        public string[] SelectAssemblies() 
        {
              return new[] { @"E:\Code\LiveSource\build\Debug\LiveSource.UnitTests.dll" };
//            string sourceFilePath = @"E:\Code\Sourceforge\dotnet\chumchase\main\ChumChase\bin\Debug\ChumChase_2.exe";
//            string destinationFilePath = @"E:\Code\Sourceforge\dotnet\chumchase\main\ChumChase\bin\Debug\ChumChase.exe";
//            File.Copy(sourceFilePath, destinationFilePath, true);
//            return new[] { destinationFilePath };
        }
    }
}

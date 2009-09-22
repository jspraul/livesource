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
//              return new[] { @"E:\Code\LiveSource\build\Debug\LiveSource.UnitTests.dll" };
            string sourceFilePath = @"E:\code\Sourceforge\dotnet\wittytwitter\Witty\Witty\bin\Debug\Witty.exe";
            string destinationFilePath = @"E:\code\Sourceforge\dotnet\wittytwitter\Witty\Witty\bin\Debug\Witty_2.exe";
            File.Copy(sourceFilePath, destinationFilePath, true);
            return new[] { @"E:\code\Sourceforge\dotnet\wittytwitter\Witty\Witty\bin\Debug\Witty_2.exe" };
        }
    }
}

using System;
using LiveSource.Core;
using Ninject.Core;

namespace LiveSource.LiveSourceConsole
{
	public class ConsoleMain
	{   
        [Inject]
	    public IAssemblyFilesSelector AssemblyFilesSelector { get; set; }

		[STAThread]
		static void Main()
		{
		    Logger.Current.Info("Starting LiveSource Console.");
		    new ConsoleMain().Run();
		}

	    private void Run()
	    {
	        string[] assemblyFiles = IOC.Get<IAssemblyFilesSelector>().SelectAssemblies();

            if (null == assemblyFiles)
                return;

            foreach(String assemblyFile in assemblyFiles)
            {
                AssemblyData assemblyData = new AssemblyData(assemblyFile);
                assemblyData.InjectCode();
            }
	    }
	}
}
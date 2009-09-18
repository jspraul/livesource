using System;
using LiveSource.Core;
using LiveSource.Core.CecilModel;
using Ninject.Core;
using Ninject.Core.Parameters;

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
                IAssemblyData assemblyData = 
                    IOC.Get<IAssemblyData>(With.Parameters.ConstructorArgument("assemblyFile", assemblyFile)); 
                assemblyData.InjectCode();
            }
	    }
	}
}
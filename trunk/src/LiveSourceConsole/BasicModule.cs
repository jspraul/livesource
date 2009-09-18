using LiveSource.Core.CecilModel;
using Ninject.Core;

namespace LiveSource.LiveSourceConsole
{
    public class BasicModule : StandardModule
    {
        public override void Load()
        {
            Bind<IAssemblyFilesSelector>().To<MockAssemblyFilesSelector>();
            Bind<IAssemblyData>().To<CecilAssemblyData>();
        }
    }
}
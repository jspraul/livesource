using Ninject.Core;

namespace LiveSource.LiveSourceConsole
{
    public class BasicModule : StandardModule
    {
        public override void Load()
        {
            Bind<IAssemblyFilesSelector>().To<MockAssemblyFilesSelector>();
        }
    }
}
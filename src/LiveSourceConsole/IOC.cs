using LiveSource.Core.CecilModel;
using Ninject.Core;
using Ninject.Core.Parameters;

namespace LiveSource.LiveSourceConsole
{
    public class IOC 
    {
        private static readonly IKernel kernel;

        static IOC() 
        {
            kernel = new StandardKernel(new BasicModule());
        }

        public static T Get<T>() 
        {
            return kernel.Get<T>();
        }

        public static T Get<T>(ParameterCollectionBuilder builder)
        {
            return kernel.Get<T>(builder);
        }
    }
}
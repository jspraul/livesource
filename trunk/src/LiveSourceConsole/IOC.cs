using Ninject.Core;

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
    }
}
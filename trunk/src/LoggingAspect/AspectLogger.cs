using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace LoggingAspect 
{
    public class AspectLogger 
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger("AspectLogging");
        private static volatile AspectLogger current;
        private static readonly object syncRoot = new object();

        private AspectLogger()
        {
        }

        private static AspectLogger Current
        {
            get 
            {
                if (current == null) 
                {
                    lock (syncRoot) 
                    {
                        if (current == null) 
                        {
                            BasicConfigurator.Configure(new FileAppender(new PatternLayout("%n[%t][%d] %m%n"), "testfile.log.txt"));
                            current = new AspectLogger();
                        }
                    }
                }

                return current;
            }
        }

        public static void Debug(string message)
        {
            if (Current.log.IsDebugEnabled)
                Current.log.Debug(message);
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
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
                            BasicConfigurator.Configure(new FileAppender(new PatternLayout("%n[%t][%d] %m%n"), "tracefile.log.txt"));
//                            BasicConfigurator.Configure(new ConsoleAppender(new PatternLayout("%n[%t][%d] %m%n")));
                            current = new AspectLogger();
                        }
                    }
                }

                return current;
            }
        }

//        public static void EndTrace(string message)
//        {
//            Current.log.Info(string.Format("End - {0}", message));    
//        }
//
//        public static void BeginTrace(string message)
//        {
//            Current.log.Info(string.Format("Begin - {0}", message));
//        }

        public static void Trace(string message)
        {
            Current.log.Debug(message);
        }

        private static string ExtractInfo() 
        {
            var frame1 = new StackFrame(2, true);
            var methodName = frame1.GetMethod().ToString();
            var fileName = Path.GetFileName(frame1.GetFileName());
            var textArray1 = new[] { "File:", fileName, " - Method:", methodName };

            return string.Concat(textArray1);
        }
    }
}
 
using System;
using System.Diagnostics;
using System.IO;
using log4net;

[assembly: log4net.Config.XmlConfigurator]
[assembly: log4net.Config.Repository]

namespace LiveSource.Core
{
    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public class Logger
    {
        readonly ILog log = LogManager.GetLogger("Logging");

        private static volatile Logger current;
        private static readonly object syncRoot = new object();

        private Logger()
        { }

        public static Logger Current
        {
            get
            {
                if (current == null)
                {
                    lock (syncRoot)
                    {
                        if (current == null)
                        {
                            current = new Logger();
                        }
                    }
                }

                return current;
            }
        }

        public void Debug(string message)
        {
            if (this.log.IsDebugEnabled)
                this.log.Debug("[" + DateTime.Now + "] " + ExtractInfo(message));
        }

        public void Info(string message)
        {
            if (this.log.IsInfoEnabled)
                this.log.Info("[" + DateTime.Now + "] " + ExtractInfo(message));
        }

        public void Error(string message, Exception e)
        {
            if (this.log.IsErrorEnabled)
                this.log.Error("[" + DateTime.Now + "] " + ExtractInfo(message), e);
        }

        public void Error(string message)
        {
            if (this.log.IsErrorEnabled)
                this.log.Error("[" + DateTime.Now + "] " + ExtractInfo(message));
        }

        private static string ExtractInfo(string message)
        {
            var frame1 = new StackFrame(2, true);
            var methodName = frame1.GetMethod().ToString();
            var fileName = Path.GetFileName(frame1.GetFileName());
            var textArray1 = new[] { "File:", fileName, " - Method:", methodName, " - ", message };

            return string.Concat(textArray1);
        }
    }
}
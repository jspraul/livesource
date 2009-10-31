using System;
using Loom.JoinPoints;

namespace LoggingAspect 
{
    class GripperLogger : Loom.Aspect
    {
        [Loom.JoinPoints.IncludeAll]
        [Loom.JoinPoints.Call(Advice.Around) ]
        public T Trace<T>([JPContext] Context ctx, params object [] args)
        {
            Console.WriteLine(ctx.Instance + "." + ctx.CurrentMethod.Name);
            ctx.Invoke(args);
            return default(T);
        }
    }
}

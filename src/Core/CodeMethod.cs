using Mono.Cecil;

namespace LiveSource.Core
{
    internal class CodeMethod
    {
        public MethodDefinition MethodDefinition { get; set; }

        public CodeMethod(MethodDefinition methodDefinition)
        {
            MethodDefinition = methodDefinition;
        }
    }
}
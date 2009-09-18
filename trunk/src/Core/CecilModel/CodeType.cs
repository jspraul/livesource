using System.Collections.Generic;
using Mono.Cecil;

namespace LiveSource.Core.CecilModel
{
    internal class CodeType
    {
        public TypeDefinition TypeDefinition { get; set; }

        public CodeType(TypeDefinition typeDefinition)
        {
            TypeDefinition = typeDefinition;
        }

        public List<CodeMethod> Methods
        {
            get
            {
                List<CodeMethod> methods = new List<CodeMethod>();
                foreach (MethodDefinition methodDefinition in this.TypeDefinition.Methods)
                {
                    methods.Add(new CodeMethod(methodDefinition));
                }

                foreach (MethodDefinition constructor in this.TypeDefinition.Constructors)
                {
                    methods.Add(new CodeMethod(constructor));
                }

                return methods;
            }
        }
    }
}
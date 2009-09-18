using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace LiveSource.Core.CecilModel
{
    internal class CodeMethod
    {
        public MethodDefinition MethodDefinition { get; set; }

        public List<CodeInstruction> Instructions
        {
            get
            {
                List<CodeInstruction> instructions = new List<CodeInstruction>();

                foreach (Instruction instruction in this.MethodDefinition.Body.Instructions)
                {
                    instructions.Add(new CodeInstruction(instruction));
                }

                return instructions;
            }
        }

        public CodeMethod(MethodDefinition methodDefinition)
        {
            MethodDefinition = methodDefinition;
        }
    }
}
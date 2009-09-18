using Mono.Cecil.Cil;

namespace LiveSource.Core.CecilModel
{
    internal class CodeInstruction
    {
        public Instruction Instruction { get; set; }

        public CodeInstruction(Instruction instruction)
        {
            Instruction = instruction;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace LiveSource.Core
{
    internal class CodeBase
    {
        private readonly AssemblyDefinition _Assembly;

        private readonly MethodReference loggerDebug;

        public CodeBase(string assemblyFile)
        {
            _Assembly = AssemblyFactory.GetAssembly(assemblyFile);
            Logger.Current.Info(">>>> Adding assembly reference to LoggingAspect");
            AssemblyNameReference aspectAssemblyReference = new AssemblyNameReference("LoggingAspect.dll", "", new Version(1, 0, 0, 0));
            _Assembly.MainModule.AssemblyReferences.Add(aspectAssemblyReference);

            MethodInfo debugWriter = typeof(LoggingAspect.AspectLogger).GetMethod("Debug", new[] { typeof(string) });
            loggerDebug = _Assembly.MainModule.Import(debugWriter);
        }

        public List<CodeType> Types
        {
            get 
            {
                List<CodeType> codeTypes = new List<CodeType>();
                foreach(TypeDefinition typeDefinition in _Assembly.MainModule.Types)
                {
                    codeTypes.Add(new CodeType(typeDefinition));
                }

                return codeTypes;
            }
        }

        public void Inject(CodeMethod method)
        {
            Logger.Current.Debug("Method >> " + method.MethodDefinition.Name);
            if (null == method.MethodDefinition.Body)
                return;

            AddStartMethodStatement(method, method.MethodDefinition.Body.Instructions[0], ">Begin");

//            // TODO: Loop through all the instruction and add end statement before every ret instruction
//            foreach (Instruction instruction in method.MethodDefinition.Body.Instructions)
//            {
//                if (instruction.OpCode.Equals(OpCodes.Ret))
//                {
//                    Logger.Current.Debug("Instruction is Ret");
//                    AddEndMethodStatement(method, instruction, "End");
//                }
//            }
        }

        private void AddStartMethodStatement(CodeMethod method, Instruction instruction, string prefix)
        {
            Instruction beginSentence = method.MethodDefinition.Body.CilWorker.Create(OpCodes.Ldstr, prefix + "-" + method.MethodDefinition.DeclaringType.FullName + "-" + method.MethodDefinition.Name);   
            InsertStatement(method, instruction, beginSentence);
        }

        private void AddEndMethodStatement(CodeMethod method, Instruction instruction, string prefix)
        {
            Instruction endSentence = method.MethodDefinition.Body.CilWorker.Create(OpCodes.Ldstr, prefix + "-" + method.MethodDefinition.DeclaringType.FullName + "-" + method.MethodDefinition.Name);
            InsertStatement(method, instruction, endSentence);
        }

        private void InsertStatement(CodeMethod method, Instruction lastInstruction, Instruction endSentence)
        {
            Instruction callLoggerDebug = method.MethodDefinition.Body.CilWorker.Create(OpCodes.Call, loggerDebug);
            method.MethodDefinition.Body.CilWorker.InsertBefore(lastInstruction, endSentence);
            method.MethodDefinition.Body.CilWorker.InsertAfter(endSentence, callLoggerDebug);
        }

        public void Save(string assemblyFile)
        {
            AssemblyFactory.SaveAssembly(_Assembly, assemblyFile);
        }
    }
}
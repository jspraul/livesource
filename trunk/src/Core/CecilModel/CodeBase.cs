using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace LiveSource.Core.CecilModel
{
    internal class CodeBase
    {
        private readonly AssemblyDefinition TargetAssemblyDefinition;
        private readonly MethodReference importedTraceMethod;
        public readonly string AssemblyFile;

        public List<CodeType> Types
        {
            get
            {
                List<CodeType> codeTypes = new List<CodeType>();
                foreach (TypeDefinition typeDefinition in TargetAssemblyDefinition.MainModule.Types)
                {
                    codeTypes.Add(new CodeType(typeDefinition));
                }

                return codeTypes;
            }
        }

        public CodeBase(string assemblyFile)
        {
            this.AssemblyFile = assemblyFile;
            TargetAssemblyDefinition = AssemblyFactory.GetAssembly(this.AssemblyFile);

            TargetAssemblyDefinition.MainModule.FullLoad();

            MethodInfo traceWriter = typeof(LoggingAspect.AspectLogger).GetMethod("Trace", new[] { typeof(string) });
            importedTraceMethod = TargetAssemblyDefinition.MainModule.Import(traceWriter);
        }

        public void AddAssemblyReference()
        {
            Logger.Current.Info(">>>> Adding assembly reference to LoggingAspect");
            AssemblyNameReference aspectAssemblyReference = new AssemblyNameReference("LoggingAspect.dll", "",
                                                                                      new Version(1, 0, 0, 0));
            TargetAssemblyDefinition.MainModule.AssemblyReferences.Add(aspectAssemblyReference);
        }

        public void InjectLogStatements()
        {
            foreach (CodeType codeType in this.Types) 
            {
                foreach (CodeMethod codeMethod in codeType.Methods) 
                {
                    this.Inject(codeMethod);
                }
            }    
        }

        private void Inject(CodeMethod method)
        {
            Logger.Current.Debug("Method >> " + method.MethodDefinition.Name);

            if (!ValidMethod(method))
                return;

            method.MethodDefinition.Body.Simplify();

            AddStartMethodStatement(method, method.MethodDefinition.Body.Instructions[0], "Begin");

            foreach (CodeInstruction ins in method.Instructions)
            {
                if (ins.Instruction.OpCode.Equals(OpCodes.Ret))
                {
                    AddEndMethodStatement(method, ins.Instruction, "End");
                }
            }
            
//            int instructionCount = method.MethodDefinition.Body.Instructions.Count;
//            AddEndMethodStatement(method, method.MethodDefinition.Body.Instructions[instructionCount - 2], "End");

            method.MethodDefinition.Body.Optimize();
        }

        private static bool ValidMethod(CodeMethod method)
        {
            if (null == method.MethodDefinition.Body)
                return false;

            foreach (CustomAttribute customAttribute in method.MethodDefinition.CustomAttributes) 
            {
                // TODO: Make following configurable
                if (Equals(customAttribute.Constructor.DeclaringType.FullName,
                           "System.Diagnostics.DebuggerNonUserCodeAttribute") ||
                    Equals(customAttribute.Constructor.DeclaringType.FullName,
                           "System.Runtime.CompilerServices.CompilerGeneratedAttribute")) 
                {
                    return false;
                }
            }

            return true;
        }

        private void AddStartMethodStatement(CodeMethod method, Instruction instruction, string prefix) 
        {
            Instruction beginSentence = method.MethodDefinition.Body.CilWorker.Create(OpCodes.Ldstr,
                                                                                      prefix + "-" +
                                                                                      method.MethodDefinition.
                                                                                          DeclaringType.FullName + "-" +
                                                                                      method.MethodDefinition.Name);
            InsertStatement(method, instruction, beginSentence);
        }

        private void AddEndMethodStatement(CodeMethod method, Instruction instruction, string prefix) 
        {
            Instruction endSentence = method.MethodDefinition.Body.CilWorker.Create(OpCodes.Ldstr,
                                                                                    prefix + "-" +
                                                                                    method.MethodDefinition.
                                                                                        DeclaringType.FullName + "-" +
                                                                                    method.MethodDefinition.Name);
            InsertStatement(method, instruction, endSentence);
        }

        private void InsertStatement(CodeMethod method, Instruction lastInstruction, Instruction endSentence)
        {
            Instruction callLoggerDebug = method.MethodDefinition.Body.CilWorker.Create(OpCodes.Call, importedTraceMethod);
            method.MethodDefinition.Body.CilWorker.InsertBefore(lastInstruction, endSentence);
            method.MethodDefinition.Body.CilWorker.InsertAfter(endSentence, callLoggerDebug);
            method.MethodDefinition.Body.CilWorker.InsertAfter(callLoggerDebug, method.MethodDefinition.Body.CilWorker.Create(OpCodes.Nop));
        }

        private void Save(string assemblyFile)
        {
            AssemblyFactory.SaveAssembly(TargetAssemblyDefinition, assemblyFile);
        }

        ~CodeBase()
        {
            Save(this.AssemblyFile);
        }
    }
}
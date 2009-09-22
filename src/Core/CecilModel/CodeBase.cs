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
        private readonly MethodReference loggerDebug;
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

            MethodInfo debugWriter = typeof (LoggingAspect.AspectLogger).GetMethod("Debug", new[] {typeof (string)});
            loggerDebug = TargetAssemblyDefinition.MainModule.Import(debugWriter);
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

            AddStartMethodStatement(method, method.MethodDefinition.Body.Instructions[0], "Begin");
        }

        private static bool ValidMethod(CodeMethod method)
        {
            if (null == method.MethodDefinition.Body)
                return false;

            foreach (CustomAttribute customAttribute in method.MethodDefinition.CustomAttributes) {
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

        private void InsertStatement(CodeMethod method, Instruction lastInstruction, Instruction endSentence)
        {
            Instruction callLoggerDebug = method.MethodDefinition.Body.CilWorker.Create(OpCodes.Call, loggerDebug);
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
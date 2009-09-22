namespace LiveSource.Core.CecilModel
{
    public class CecilAssemblyData : IAssemblyData
    {
        public string AssemblyFile { get; set; }

        public CecilAssemblyData(string assemblyFile) 
        {
            AssemblyFile = assemblyFile;
        }

        public void InjectCode()
        {
            Logger.Current.Info("Processing assembly:" + this.AssemblyFile);

            // TODO: Refactor to dispose pattern
            CodeBase codeBase = new CodeBase(this.AssemblyFile);

            codeBase.AddAssemblyReference();
//            // TODO: Refactor the following code into CodeBase. just call codeBase.Inject();
            foreach (CodeType codeType in codeBase.Types) 
            {
                foreach (CodeMethod codeMethod in codeType.Methods) 
                {
                    codeBase.Inject(codeMethod);
                }
            }
            
            codeBase.Save(this.AssemblyFile);
        }
    }
}
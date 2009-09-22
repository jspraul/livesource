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

            CodeBase codeBase = new CodeBase(this.AssemblyFile);
            {
                codeBase.AddAssemblyReference();
                codeBase.InjectLogStatements();
            }
        }
    }
}
namespace LiveSource.Core
{
    public interface IAssemblyData
    {
        string AssemblyFile { get; set; }
        void InjectCode();
    }
}
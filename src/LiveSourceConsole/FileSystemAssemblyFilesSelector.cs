using System.Windows.Forms;

namespace LiveSource.LiveSourceConsole
{
    public class FileSystemAssemblyFilesSelector : IAssemblyFilesSelector
    {
        public string[] SelectAssemblies()
        {
            OpenFileDialog dialog = new OpenFileDialog { Multiselect = true };
            if (DialogResult.OK == dialog.ShowDialog()) {
                return dialog.FileNames;
            }

            return null;
        }
    }
}
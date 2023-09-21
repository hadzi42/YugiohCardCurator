using Microsoft.Win32;

namespace YugiohCardCurator.Views
{
    internal sealed class DialogHandler : IDialogHandler
    {
        public string ShowOpenFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog { Filter = "GZip file|*.gz" };
            dialog.ShowDialog();
            return dialog.FileName;
        }

        public string ShowSaveFileDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog { Filter = "GZip file|*.gz" };
            dialog.ShowDialog();
            return dialog.FileName;
        }
    }
}

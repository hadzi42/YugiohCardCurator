namespace YugiohCardCurator
{
    internal interface IDialogHandler
    {
        void ShowErrorDialog(string title, string message);
        string ShowOpenFileDialog();
        string ShowSaveFileDialog();
    }
}

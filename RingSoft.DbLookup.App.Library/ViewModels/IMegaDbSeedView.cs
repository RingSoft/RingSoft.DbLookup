namespace RSDbLookupApp.Library.ViewModels
{
    public interface IMegaDbSeedView
    {
        void ShowInformationMessage(string message, string title);

        void ShowValidationMessage(string message, string title);

        void ItemsTableSeederProgress(MegaDb.ItemsTableSeederProgressArgs e);

        void CloseWindow();
    }
}

using RingSoft.DbLookup.App.Library.MegaDb;

namespace RingSoft.DbLookup.App.Library.ViewModels
{
    public interface IMegaDbSeedView
    {
        string HotKeyPrefix { get; }

        void ShowInformationMessage(string message, string title);

        void ShowValidationMessage(string message, string title);

        void ItemsTableSeederProgress(ItemsTableSeederProgressArgs e);

        void CloseWindow();
    }
}

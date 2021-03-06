﻿using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.App.Library.ViewModels
{
    public interface IDbSetupView
    {
        bool ShowScriptDialog(DbDataProcessor dataProcessor, string scriptFileName, string sql, bool splitGo,
            string defaultDbName, string dbName, bool showExecSuccessMessage);

        string ShowOpenFileDialog(string initialDirectory, string fileName, string defaultExt, string filter);

        void ShowInformationMessage(string message, string title);

        void ShowValidationMessage(string message, string title);

        void ShowCriticalMessage(string message, string title);

        bool ShowYesNoMessage(string message, string title);

        void ValidationFailSetFocus_Northwind(NorthwindDbPlatforms platform);

        void ValidationFailSetFocus_MegaDb(MegaDbPlatforms platform);

        void ExitApplication();

        void CloseWindow();

        void ShowMegaDbItemsTableSeederForm();
    }
}

﻿using System;
using System.Windows.Forms;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Controls.WinForms
{
    public class ControlsUserInterface : IDbLookupUserInterface
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            var dataProcessResultViewer = new DataProcessResultForm(dataProcessResult);
            dataProcessResultViewer.ShowDialog();

        }

        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            var messageBoxIcon = MessageBoxIcon.Error;
            switch (icon)
            {
                case RsMessageBoxIcons.Error:
                    break;
                case RsMessageBoxIcons.Exclamation:
                    messageBoxIcon = MessageBoxIcon.Exclamation;
                    break;
                case RsMessageBoxIcons.Information:
                    messageBoxIcon = MessageBoxIcon.Information;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
            }

            MessageBox.Show(text, caption, MessageBoxButtons.OK, messageBoxIcon);
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            switch (cursor)
            {
                case WindowCursorTypes.Default:
                    Cursor.Current = Cursors.Default;
                    break;
                case WindowCursorTypes.Wait:
                    Cursor.Current = Cursors.WaitCursor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cursor), cursor, null);
            }
        }
    }
    public static class ControlsGlobals
    {
        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        public static void InitUi()
        {
            DbDataProcessor.UserInterface = _userInterface;
        }
    }
}

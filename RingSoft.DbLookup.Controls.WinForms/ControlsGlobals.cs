﻿using RingSoft.DbLookup.GetDataProcessor;
using System;
using System.Windows.Forms;

namespace RingSoft.DbLookup.Controls.WinForms
{
    public class ControlsUserInterface : IDataProcessResultViewer, IWindowCursor
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            var dataProcessResultViewer = new DataProcessResultForm(dataProcessResult);
            dataProcessResultViewer.ShowDialog();

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
            DbDataProcessor.DataProcessResultViewer = _userInterface;
            DbDataProcessor.WindowCursor = _userInterface;
        }
    }
}
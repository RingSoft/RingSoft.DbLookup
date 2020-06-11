using System;
using System.Windows;
using System.Windows.Input;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class ControlsUserInterface : IDbLookupUserInterface
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dataProcessResultWindow = new DataProcessResultWindow(dataProcessResult);
                dataProcessResultWindow.ShowDialog();
            });
        }

        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            var messageBoxImage = MessageBoxImage.Error;
            switch (icon)
            {
                case RsMessageBoxIcons.Error:
                    break;
                case RsMessageBoxIcons.Exclamation:
                    messageBoxImage = MessageBoxImage.Exclamation;
                    break;
                case RsMessageBoxIcons.Information:
                    messageBoxImage = MessageBoxImage.Information;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
            }

            MessageBox.Show(text, caption, MessageBoxButton.OK, messageBoxImage);
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            switch (cursor)
            {
                case WindowCursorTypes.Default:
                    Mouse.OverrideCursor = null;
                    break;
                case WindowCursorTypes.Wait:
                    Mouse.OverrideCursor = Cursors.Wait;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cursor), cursor, null);
            }
        }
    }
    public static class ControlsGlobals
    {
        public static LookupWindowFactory LookupWindowFactory { get; private set; } = new LookupWindowFactory();

        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        public static void InitUi()
        {
            DbDataProcessor.UserInterface = _userInterface;
        }

        public static void InitUi(LookupWindowFactory lookupWindowFactory)
        {
            DbDataProcessor.UserInterface = _userInterface;
            LookupWindowFactory = lookupWindowFactory;
        }
    }
}

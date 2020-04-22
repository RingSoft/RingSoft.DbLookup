using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

// ReSharper disable InconsistentNaming

namespace RingSoft.DbLookup.Controls.WPF
{
    public class BaseWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;

        private const int WS_BOTH = 0x30000; //maximize and minimize buttons
        public bool SetFocusToFirstControl { get; set; } = true;

        public bool HideControlBox { get; set; }

        public BaseWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PreviewKeyDown += (sender, args) =>
            {
                switch (args.Key)
                {
                    case Key.Escape:
                        Close();
                        args.Handled = true;
                        break;
                }
            };

            SourceInitialized += (sender, args) =>
            {
                if (HideControlBox)
                {
                    var hwnd = new WindowInteropHelper((Window) sender).Handle;
                    var value = GetWindowLong(hwnd, GWL_STYLE);
                    SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_BOTH);
                }
            };

            Loaded += (sender, args) =>
            {
                if (SetFocusToFirstControl)
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            };
        }
    }
}

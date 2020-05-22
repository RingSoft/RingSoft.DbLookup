using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
// ReSharper disable UnusedAutoPropertyAccessor.Global

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

        /// <summary>
        /// Gets or sets a value indicating whether to close window when the escape key is pressed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to close window when the escape key is pressed; otherwise, <c>false</c>.
        /// </value>
        public bool CloseOnEscape { get; set; } = true;

        public bool SetFocusToFirstControl { get; set; } = true;

        public bool HideControlBox { get; set; }

        public BaseWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            KeyDown += (sender, args) =>
            {
                switch (args.Key)
                {
                    case Key.Escape:
                        if (CloseOnEscape)
                        {
                            Close();
                            args.Handled = true;
                        }

                        break;
                }
            };

            SourceInitialized += (sender, args) =>
            {
                if (HideControlBox)
                {
                    var hwnd = new WindowInteropHelper((Window) sender ?? throw new InvalidOperationException()).Handle;
                    GetWindowLong(hwnd, GWL_STYLE);
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

using System.Windows;
using System.Windows.Input;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class BaseWindow : Window
    {
        protected virtual bool SetFocusToFirstControl { get; } = true;

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

            Loaded += (sender, args) =>
            {
                if (SetFocusToFirstControl)
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            };
        }
    }
}

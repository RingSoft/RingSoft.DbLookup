using System.Windows;
using System.Windows.Input;

namespace RingSoft.DbLookup.App.WPF.Views
{
    public class BaseWindow : Window
    {
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
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            };
        }
    }
}

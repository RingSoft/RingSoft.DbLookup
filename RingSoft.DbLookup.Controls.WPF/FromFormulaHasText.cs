using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class FromFormulaHasText : Button
    {
        public static readonly DependencyProperty NotificationVisibilityProperty =
            DependencyProperty.Register(nameof(NotificationVisibility), typeof(Visibility), typeof(FromFormulaHasText),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        public Visibility NotificationVisibility
        {
            get { return (Visibility)GetValue(NotificationVisibilityProperty); }
            set { SetValue(NotificationVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MemoHasTextProperty =
            DependencyProperty.Register(nameof(MemoHasText), typeof(bool), typeof(FromFormulaHasText));

        public bool MemoHasText
        {
            get { return (bool)GetValue(MemoHasTextProperty); }
            set { SetValue(MemoHasTextProperty, value); }
        }

        static FromFormulaHasText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FromFormulaHasText), new FrameworkPropertyMetadata(typeof(FromFormulaHasText)));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.Themes"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.Themes;assembly=RingSoft.DbLookup.Controls.WPF.Themes"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:AutoFillFormulaCellTemplate/>
    ///
    /// </summary>
    [TemplatePart(Name = "TextBox", Type = typeof(StringEditControl))]
    [TemplatePart(Name = "Button", Type = typeof(Button))]
    public class AutoFillMemoCellControl : Control
    {
        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var formulaAutoFillControl = (AutoFillMemoCellControl)obj;
            if (formulaAutoFillControl.TextBox != null)
            {
                formulaAutoFillControl.TextBox.Background = formulaAutoFillControl.Background;
            }
        }

        private static void HeightChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillMemoCellControl = (AutoFillMemoCellControl)obj;
            if (autoFillMemoCellControl.TextBox != null)
            {
                var height = autoFillMemoCellControl.Height;
                if (height > autoFillMemoCellControl.ActualHeight)
                {
                    height = autoFillMemoCellControl.ActualHeight;
                }
                autoFillMemoCellControl.TextBox.Height = height;
                if (autoFillMemoCellControl.Button != null)
                {
                    autoFillMemoCellControl.Button.Height = height;
                }
            }
        }

        private static void IsFocusedChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var formulaAutoFillControl = (AutoFillMemoCellControl)obj;
            if (formulaAutoFillControl.TextBox != null)
            {
                if (formulaAutoFillControl.IsFocused)
                    formulaAutoFillControl.TextBox.Focus();
            }
        }

        public event EventHandler ShowMemoEditorWindow;

        public StringEditControl TextBox { get; set; }

        public Button Button { get; set; }

        public string Text { get; set; }

        public string OriginalText { get; set; }
        static AutoFillMemoCellControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoFillMemoCellControl), new FrameworkPropertyMetadata(typeof(AutoFillMemoCellControl)));

            BackgroundProperty.OverrideMetadata(typeof(AutoFillMemoCellControl),
                new FrameworkPropertyMetadata(BackgroundChangedCallback));

            HeightProperty.OverrideMetadata(typeof(AutoFillMemoCellControl),
                new FrameworkPropertyMetadata(HeightChangedCallback));

            //VerticalAlignmentProperty.OverrideMetadata(typeof(AutoFillFormulaCellControl), new FrameworkPropertyMetadata(System.Windows.VerticalAlignment.Center));


            //IsFocusedProperty.OverrideMetadata(typeof(AutoFillFormulaCellControl),
            //    new FrameworkPropertyMetadata(IsFocusedChangedCallback));


        }

        public AutoFillMemoCellControl()
        {
            GotFocus += (sender, args) =>
            {
                TextBox?.Focus();
            };

            KeyDown += (sender, args) =>
            {
                if (args.Key == Key.F5)
                {
                    ShowMemoEditor();
                }
            };
        }

        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild(nameof(TextBox)) as StringEditControl;
            Button = GetTemplateChild(nameof(Button)) as Button;

            if (Button != null) 
                Button.Click += (sender, args) => ShowMemoEditor();

            if (TextBox != null)
                TextBox.TextChanged += (sender, args) =>
                {
                    if(!TextBox.IsReadOnly)
                        Text = TextBox.Text;
                };

            base.OnApplyTemplate();
        }

        private void ShowMemoEditor()
        {
            ShowMemoEditorWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}

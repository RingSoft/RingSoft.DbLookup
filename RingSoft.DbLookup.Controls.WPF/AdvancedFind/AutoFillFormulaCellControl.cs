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
    public class AutoFillFormulaCellControl : Control
    {
        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var formulaAutoFillControl = (AutoFillFormulaCellControl)obj;
            if (formulaAutoFillControl.TextBox != null)
            {
                formulaAutoFillControl.TextBox.Background = formulaAutoFillControl.Background;
            }
        }

        private static void HeightChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var formulaAutoFillControl = (AutoFillFormulaCellControl)obj;
            if (formulaAutoFillControl.TextBox != null)
            {
                formulaAutoFillControl.TextBox.Height = formulaAutoFillControl.ActualHeight;
            }
        }

        private static void IsFocusedChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var formulaAutoFillControl = (AutoFillFormulaCellControl)obj;
            if (formulaAutoFillControl.TextBox != null)
            {
                if (formulaAutoFillControl.IsFocused)
                    formulaAutoFillControl.TextBox.Focus();
            }
        }


        public StringEditControl TextBox { get; set; }

        public Button Button { get; set; }

        public string Formula { get; set; }

        public string OriginalFormula { get; set; }
        static AutoFillFormulaCellControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoFillFormulaCellControl), new FrameworkPropertyMetadata(typeof(AutoFillFormulaCellControl)));

            BackgroundProperty.OverrideMetadata(typeof(AutoFillFormulaCellControl),
                new FrameworkPropertyMetadata(BackgroundChangedCallback));

            HeightProperty.OverrideMetadata(typeof(AutoFillFormulaCellControl),
                new FrameworkPropertyMetadata(HeightChangedCallback));

            //VerticalAlignmentProperty.OverrideMetadata(typeof(AutoFillFormulaCellControl), new FrameworkPropertyMetadata(System.Windows.VerticalAlignment.Center));


            //IsFocusedProperty.OverrideMetadata(typeof(AutoFillFormulaCellControl),
            //    new FrameworkPropertyMetadata(IsFocusedChangedCallback));


        }

        public AutoFillFormulaCellControl()
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

            base.OnApplyTemplate();
        }

        private void ShowMemoEditor()
        {
            var memoEditor = new AdvancedFindGridMemoEditor(new DataEntryGridMemoValue(0){Text = Formula});
            memoEditor.Owner = Window.GetWindow(this);
            memoEditor.ShowInTaskbar = false;
            memoEditor.ShowDialog();
        }
    }
}

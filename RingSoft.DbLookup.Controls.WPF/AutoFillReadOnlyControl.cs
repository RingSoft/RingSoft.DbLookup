using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF;assembly=RingSoft.DbLookup.Controls.WPF"
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
    ///     <MyNamespace:AutoFillReadOnlyControl/>
    ///
    /// </summary>
    public class AutoFillReadOnlyControl : Control, IReadOnlyControl
    {
        public TextBlock TextBlock { get; set; }

        public Button DropDownButton { get; set; }

        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register("Setup", typeof(AutoFillSetup), typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        /// <summary>
        /// Gets or sets the AutoFillSetup to determine how this control will behave.
        /// </summary>
        /// <value>
        /// The AutoFillSetup.
        /// </value>
        public AutoFillSetup Setup
        {
            get { return (AutoFillSetup) GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillReadOnlyControl)obj;
            autoFillControl.CheckButton();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(AutoFillValue), typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        /// <summary>
        /// Gets or sets the AutoFillValue.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public AutoFillValue Value
        {
            get { return (AutoFillValue) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillReadOnlyControl)obj;
            autoFillControl.SetValue();
            autoFillControl.CheckButton();
        }

        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register("DesignText", typeof(string), typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillReadOnlyControl)obj;
            autoFillControl.SetDesignText();
        }


        private static void HeightChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillReadOnlyControl)obj;
            if (autoFillControl.TextBlock != null)
            {
                var height = autoFillControl.Height;
                if (height > autoFillControl.ActualHeight)
                {
                    height = autoFillControl.ActualHeight;
                }
                autoFillControl.TextBlock.Height = height;
                if (autoFillControl.DropDownButton != null)
                {
                    autoFillControl.DropDownButton.Height = height;
                }
            }
        }


        private AutoFillValue _pendingAutoFillValue;

        static AutoFillReadOnlyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(typeof(AutoFillReadOnlyControl)));

            HeightProperty.OverrideMetadata(typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(HeightChangedCallback));

        }

        public override void OnApplyTemplate()
        {
            TextBlock = GetTemplateChild(nameof(TextBlock)) as TextBlock;
            DropDownButton = GetTemplateChild(nameof(DropDownButton)) as Button;

            if (_pendingAutoFillValue != null)
            {
                Value = _pendingAutoFillValue;
            }

            SetDesignText();

            DropDownButton.Click += (sender, args) => ShowLookupWindow();

            base.OnApplyTemplate();
        }

        public virtual void ShowLookupWindow()
        {
            var initialText = TextBlock.Text;

            var lookupWindow = LookupControlsGlobals.LookupWindowFactory.CreateLookupWindow(Setup.LookupDefinition,
                Setup.AllowLookupAdd, Setup.AllowLookupView, initialText, Value?.PrimaryKeyValue, Value?.PrimaryKeyValue);
            lookupWindow.AddViewParameter = Setup.AddViewParameter;

            lookupWindow.Owner = Window.GetWindow(this);
            lookupWindow.RefreshData += (o, args) =>
            {
                if (Value != null)
                {
                    var value = Value.PrimaryKeyValue.KeyValueFields[0].Value;
                    Value =
                        Setup.LookupDefinition.TableDefinition.Context.OnAutoFillTextRequest(
                            Setup.LookupDefinition.TableDefinition, value);
                }
            };
            lookupWindow.SetReadOnlyMode(true);
            lookupWindow.ApplyNewLookup += (sender, args) => Setup.LookupDefinition = lookupWindow.LookupDefinition;
            lookupWindow.Closed += (sender, args) =>
            {
                Window.GetWindow(this).Activate();
            };
            lookupWindow.Show();
        }


        private void SetupControl()
        {
            if (Setup.LookupDefinition == null || Setup.LookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");
        }

        private void SetValue()
        {
            if (Value == null)
            {
                ClearValue();
            }
            else
            {
                if (TextBlock == null)
                    _pendingAutoFillValue = Value;
                else 
                {
                    TextBlock.Text = Value.Text;
                }
            }
        }

        private void ClearValue()
        {
            TextBlock.Text = string.Empty;
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            DropDownButton.IsEnabled = true;
        }

        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty() && TextBlock != null)
            {
                TextBlock.Text = DesignText;
            }
        }

        private void CheckButton()
        {
            if (Setup != null)
            {
                if (Setup.LookupDefinition.TableDefinition.CanViewTable)
                {
                    if (DropDownButton != null)
                    {
                        DropDownButton.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (DropDownButton != null)
                    {
                        DropDownButton.Visibility = Visibility.Collapsed;
                    }
                }
            }

        }
    }
}

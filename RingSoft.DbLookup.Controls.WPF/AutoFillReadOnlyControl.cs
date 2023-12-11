// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-23-2023
// ***********************************************************************
// <copyright file="AutoFillReadOnlyControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// Class AutoFillReadOnlyControl.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="IReadOnlyControl" />
    /// Implements the <see cref="IAutoFillControl" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="IReadOnlyControl" />
    /// <seealso cref="IAutoFillControl" />
    /// <font color="red">Badly formed XML comment.</font>
    public class AutoFillReadOnlyControl : Control, IReadOnlyControl, IAutoFillControl
    {
        /// <summary>
        /// Gets or sets the text block.
        /// </summary>
        /// <value>The text block.</value>
        public TextBlock TextBlock { get; set; }

        /// <summary>
        /// Gets or sets the drop down button.
        /// </summary>
        /// <value>The drop down button.</value>
        public Button DropDownButton { get; set; }

        /// <summary>
        /// The setup property
        /// </summary>
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register("Setup", typeof(AutoFillSetup), typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        /// <summary>
        /// Gets or sets the AutoFillSetup to determine how this control will behave.
        /// </summary>
        /// <value>The AutoFillSetup.</value>
        public AutoFillSetup Setup
        {
            get { return (AutoFillSetup) GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        /// <summary>
        /// Setups the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillReadOnlyControl)obj;
            autoFillControl.CheckButton();
        }

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(AutoFillValue), typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        /// <summary>
        /// Gets or sets the AutoFillValue.
        /// </summary>
        /// <value>The value.</value>
        public AutoFillValue Value
        {
            get { return (AutoFillValue) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Values the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillReadOnlyControl)obj;
            autoFillControl.SetValue();
            autoFillControl.CheckButton();
        }

        /// <summary>
        /// The design text property
        /// </summary>
        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register("DesignText", typeof(string), typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        /// <summary>
        /// Gets or sets the design text.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        /// <summary>
        /// Designs the text changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillReadOnlyControl)obj;
            autoFillControl.SetDesignText();
        }


        /// <summary>
        /// Heights the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Gets or sets the edit text.
        /// </summary>
        /// <value>The edit text.</value>
        public string EditText
        {
            get
            {
                return TextBlock.Text;
            }
            set
            {
                TextBlock.Text = value;
            }
        }
        /// <summary>
        /// Gets or sets the selection start.
        /// </summary>
        /// <value>The selection start.</value>
        public int SelectionStart { get; set; }
        /// <summary>
        /// Gets or sets the length of the selection.
        /// </summary>
        /// <value>The length of the selection.</value>
        public int SelectionLength { get; set; }
        /// <summary>
        /// Refreshes the value.
        /// </summary>
        /// <param name="token">The token.</param>
        public void RefreshValue(LookupCallBackToken token)
        {
            if (token.NewAutoFillValue.PrimaryKeyValue.IsEqualTo(Value.PrimaryKeyValue))
            {
                Value = token.NewAutoFillValue;
            }
        }

        /// <summary>
        /// Called when [select].
        /// </summary>
        public void OnSelect()
        {
            
        }

        /// <summary>
        /// The pending automatic fill value
        /// </summary>
        private AutoFillValue _pendingAutoFillValue;

        /// <summary>
        /// Initializes static members of the <see cref="AutoFillReadOnlyControl"/> class.
        /// </summary>
        static AutoFillReadOnlyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(typeof(AutoFillReadOnlyControl)));

            HeightProperty.OverrideMetadata(typeof(AutoFillReadOnlyControl),
                new FrameworkPropertyMetadata(HeightChangedCallback));

        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
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

        /// <summary>
        /// Shows the lookup window.
        /// </summary>
        public virtual void ShowLookupWindow()
        {
            var initialText = TextBlock.Text;

            var lookupWindow = LookupControlsGlobals.LookupWindowFactory.CreateLookupWindow(
                Setup.LookupDefinition
                , Setup.AllowLookupAdd
                , Setup.AllowLookupView
                , initialText
                , Value?.PrimaryKeyValue
                , this
                , Value?.PrimaryKeyValue);
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


        /// <summary>
        /// Setups the control.
        /// </summary>
        /// <exception cref="System.ArgumentException">Lookup definition does not have any visible columns defined or its initial sort column is null.</exception>
        private void SetupControl()
        {
            if (Setup.LookupDefinition == null || Setup.LookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
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

        /// <summary>
        /// Clears the value.
        /// </summary>
        private void ClearValue()
        {
            TextBlock.Text = string.Empty;
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            DropDownButton.IsEnabled = true;
        }

        /// <summary>
        /// Sets the design text.
        /// </summary>
        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty() && TextBlock != null)
            {
                TextBlock.Text = DesignText;
            }
        }

        /// <summary>
        /// Checks the button.
        /// </summary>
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

// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AutoFillMemoCellControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// Class AutoFillMemoCellControl.
    /// Implements the <see cref="Control" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <font color="red">Badly formed XML comment.</font>
    [TemplatePart(Name = "TextBox", Type = typeof(StringEditControl))]
    [TemplatePart(Name = "Button", Type = typeof(Button))]
    public class AutoFillMemoCellControl : Control
    {
        /// <summary>
        /// Backgrounds the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var formulaAutoFillControl = (AutoFillMemoCellControl)obj;
            if (formulaAutoFillControl.TextBox != null)
            {
                formulaAutoFillControl.TextBox.Background = formulaAutoFillControl.Background;
            }
        }

        /// <summary>
        /// Heights the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Determines whether [is focused changed callback] [the specified object].
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Occurs when [show memo editor window].
        /// </summary>
        public event EventHandler ShowMemoEditorWindow;

        /// <summary>
        /// Gets or sets the text box.
        /// </summary>
        /// <value>The text box.</value>
        public StringEditControl TextBox { get; set; }

        /// <summary>
        /// Gets or sets the button.
        /// </summary>
        /// <value>The button.</value>
        public Button Button { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the original text.
        /// </summary>
        /// <value>The original text.</value>
        public string OriginalText { get; set; }
        /// <summary>
        /// Initializes static members of the <see cref="AutoFillMemoCellControl"/> class.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillMemoCellControl"/> class.
        /// </summary>
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

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
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

        /// <summary>
        /// Shows the memo editor.
        /// </summary>
        private void ShowMemoEditor()
        {
            ShowMemoEditorWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}

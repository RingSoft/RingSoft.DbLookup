// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-22-2023
// ***********************************************************************
// <copyright file="LookupCustomContentControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupCustomContentColumn.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupColumn{RingSoft.DbLookup.Controls.WPF.LookupCustomContentControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupColumn{RingSoft.DbLookup.Controls.WPF.LookupCustomContentControl}" />
    public class LookupCustomContentColumn : LookupColumn<LookupCustomContentControl>
    {
        /// <summary>
        /// The content template
        /// </summary>
        private DataEntryCustomContentTemplate _contentTemplate;

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        /// <value>The content template.</value>
        public DataEntryCustomContentTemplate ContentTemplate
        {
            get => _contentTemplate;
            set
            {
                if (_contentTemplate == value)
                    return;

                _contentTemplate = value;
                OnPropertyChanged(nameof(ContentTemplate));
            }
        }

        /// <summary>
        /// The designer selected identifier
        /// </summary>
        private int _designerSelectedId;

        /// <summary>
        /// Gets or sets the designer selected identifier.
        /// </summary>
        /// <value>The designer selected identifier.</value>
        public int DesignerSelectedId
        {
            get => _designerSelectedId;
            set
            {
                if (_designerSelectedId == value)
                    return;

                _designerSelectedId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Processes the framework element factory.
        /// </summary>
        /// <param name="lookupControl">The lookup control.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <param name="lookupColumnDefinition">The lookup column definition.</param>
        /// <param name="designMode">if set to <c>true</c> [design mode].</param>
        protected override void ProcessFrameworkElementFactory(LookupControl lookupControl,
            FrameworkElementFactory factory, string dataColumnName,
            LookupColumnDefinitionBase lookupColumnDefinition, bool designMode)
        {
            if (ContentTemplate == null)
                return;// throw new Exception($"The {nameof(ContentTemplate)} Property has not been set.");

            factory.SetValue(CustomContentControl.ContentTemplateProperty, ContentTemplate);

            if (lookupColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                && lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerField)
            {
                if (integerField.EnumTranslation != null)
                {
                    factory.SetValue(LookupCustomContentControl.EnumFieldTranslationProperty, integerField.EnumTranslation);
                }
            }
            if (designMode)
            {
                factory.SetValue(LookupCustomContentControl.DesignerValueProperty, DesignerSelectedId);
            }
            else
            {
                factory.SetBinding(LookupCustomContentControl.DataTextProperty, new Binding(dataColumnName));
            }
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="dbValue">The database value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool SetValue(string dbValue)
        {
            return true;
        }
    }

    /// <summary>
    /// Class LookupCustomContentControl.
    /// Implements the <see cref="CustomContentControl" />
    /// </summary>
    /// <seealso cref="CustomContentControl" />
    /// <font color="red">Badly formed XML comment.</font>
    public class LookupCustomContentControl : CustomContentControl
    {
        /// <summary>
        /// The enum field translation property
        /// </summary>
        public static readonly DependencyProperty EnumFieldTranslationProperty =
            DependencyProperty.Register(nameof(EnumFieldTranslation), typeof(EnumFieldTranslation), typeof(LookupCustomContentControl));

        /// <summary>
        /// Gets or sets the enum field translation.
        /// </summary>
        /// <value>The enum field translation.</value>
        public EnumFieldTranslation EnumFieldTranslation
        {
            get { return (EnumFieldTranslation)GetValue(EnumFieldTranslationProperty); }
            set { SetValue(EnumFieldTranslationProperty, value); }
        }

        /// <summary>
        /// The data text property
        /// </summary>
        public static readonly DependencyProperty DataTextProperty =
            DependencyProperty.Register(nameof(DataText), typeof(string), typeof(LookupCustomContentControl),
                new FrameworkPropertyMetadata(DataTextChangedCallback));

        /// <summary>
        /// Gets or sets the data text.
        /// </summary>
        /// <value>The data text.</value>
        public string DataText
        {
            get { return (string)GetValue(DataTextProperty); }
            set { SetValue(DataTextProperty, value); }
        }

        /// <summary>
        /// Datas the text changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DataTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customControl = (LookupCustomContentControl)obj;
            customControl.SetDataText();
        }

        /// <summary>
        /// The designer value property
        /// </summary>
        public static readonly DependencyProperty DesignerValueProperty =
            DependencyProperty.Register(nameof(DesignerValue), typeof(int), typeof(LookupCustomContentControl),
                new FrameworkPropertyMetadata(DesignerValueChangedCallback));

        /// <summary>
        /// Gets or sets the designer value.
        /// </summary>
        /// <value>The designer value.</value>
        public int DesignerValue
        {
            get { return (int)GetValue(DesignerValueProperty); }
            set { SetValue(DesignerValueProperty, value); }
        }

        /// <summary>
        /// Designers the value changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DesignerValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customControl = (LookupCustomContentControl)obj;
            customControl.SelectedItemId = customControl.DesignerValue;
        }

        /// <summary>
        /// Initializes static members of the <see cref="LookupCustomContentControl"/> class.
        /// </summary>
        static LookupCustomContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LookupCustomContentControl), new FrameworkPropertyMetadata(typeof(LookupCustomContentControl)));
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (!DataText.IsNullOrEmpty())
                SetDataText();
        }

        /// <summary>
        /// Sets the data text.
        /// </summary>
        protected virtual void SetDataText()
        {
            if (EnumFieldTranslation == null)
            {
                SelectItem(DataText.ToInt());
            }
            else
            {
                var typeTranslation =
                    EnumFieldTranslation.TypeTranslations.FirstOrDefault(f => f.NumericValue == DataText.ToInt());
                if (typeTranslation != null)
                    SelectItem(typeTranslation.NumericValue);
            }
        }
    }
}

// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="LookupColumn.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using RingSoft.DbLookup.DataProcessor;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// A column in a LookupControl's ListView control.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public abstract class LookupColumnBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The header
        /// </summary>
        private string _header;
        /// <summary>
        /// Gets or sets the column header text.
        /// </summary>
        /// <value>The header.</value>
        public string Header
        {
            get => _header;
            set
            {
                if (_header == value)
                    return;

                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        /// <summary>
        /// The design text
        /// </summary>
        private string _designText;

        /// <summary>
        /// Gets or sets the text to put into this column's cell for all rows in the ListView when in design mode.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get => _designText;
            set
            {
                if (_designText == value)
                    return;

                _designText = value;
                OnPropertyChanged(nameof(DesignText));
            }
        }

        /// <summary>
        /// The width
        /// </summary>
        private double _width;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public double Width
        {
            get => _width;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_width == value)
                    return;

                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        /// <summary>
        /// The keep null empty
        /// </summary>
        private bool _keepNullEmpty;

        /// <summary>
        /// Gets or sets a value indicating whether [keep null empty].
        /// </summary>
        /// <value><c>true</c> if [keep null empty]; otherwise, <c>false</c>.</value>
        public bool KeepNullEmpty
        {
            get => _keepNullEmpty;
            set
            {
                if (_keepNullEmpty == value)
                    return;

                _keepNullEmpty = value;
                OnPropertyChanged(nameof(KeepNullEmpty));
            }
        }


        /// <summary>
        /// Gets or sets the name of the property.  This is used to map this column to a visible LookupColumnDefinition in the LookupDefintion.  Use the NameOfExtension to make sure this value is correct.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets the name of the data column.
        /// </summary>
        /// <value>The name of the data column.</value>
        public string DataColumnName { get; internal set; }

        /// <summary>
        /// Gets the lookup column definition.
        /// </summary>
        /// <value>The lookup column definition.</value>
        public LookupColumnDefinitionBase LookupColumnDefinition { get; internal set; }

        /// <summary>
        /// Gets the cell data template.
        /// </summary>
        /// <param name="lookupControl">The lookup control.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <param name="designMode">if set to <c>true</c> [design mode].</param>
        /// <returns>DataTemplate.</returns>
        internal abstract DataTemplate GetCellDataTemplate(LookupControl lookupControl, string dataColumnName,
            bool designMode);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="dbValue">The database value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool SetValue(string dbValue)
        {
            return false;
        }
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [Properties.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Class LookupColumn.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupColumnBase" />
    /// </summary>
    /// <typeparam name="TControl">The type of the t control.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupColumnBase" />
    public abstract class LookupColumn<TControl> : LookupColumnBase
        where TControl : FrameworkElement
    {
        /// <summary>
        /// Gets the cell data template.
        /// </summary>
        /// <param name="lookupControl">The lookup control.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <param name="designMode">if set to <c>true</c> [design mode].</param>
        /// <returns>DataTemplate.</returns>
        internal override DataTemplate GetCellDataTemplate(LookupControl lookupControl, string dataColumnName,
            bool designMode)
        {
            var template = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TControl));
            
            ProcessFrameworkElementFactory(lookupControl, factory, dataColumnName, LookupColumnDefinition, designMode);
            factory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(Loaded));

            template.VisualTree = factory;

            return template;
        }

        /// <summary>
        /// Processes the framework element factory.
        /// </summary>
        /// <param name="lookupControl">The lookup control.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <param name="lookupColumnDefinition">The lookup column definition.</param>
        /// <param name="designMode">if set to <c>true</c> [design mode].</param>
        protected abstract void ProcessFrameworkElementFactory(LookupControl lookupControl,
            FrameworkElementFactory factory, string dataColumnName,
            LookupColumnDefinitionBase lookupColumnDefinition, bool designMode);

        /// <summary>
        /// Loadeds the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TControl control)
            {

            }
        }
    }

    /// <summary>
    /// Class LookupColumn.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupColumnBase" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupColumnBase" />
    public class LookupColumn : LookupColumn<TextBlock>
    {
        /// <summary>
        /// The text alignment
        /// </summary>
        private LookupColumnAlignmentTypes _textAlignment = LookupColumnAlignmentTypes.Left;

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>The text alignment.</value>
        public LookupColumnAlignmentTypes TextAlignment
        {
            get => _textAlignment;
            set
            {
                TextAlignmentChanged = true;
                if (_textAlignment == value)
                    return;

                _textAlignment = value;
                OnPropertyChanged(nameof(TextAlignment));
            }
        }

        /// <summary>
        /// Gets a value indicating whether [text alignment changed].
        /// </summary>
        /// <value><c>true</c> if [text alignment changed]; otherwise, <c>false</c>.</value>
        public bool TextAlignmentChanged { get; private set; }

        /// <summary>
        /// Processes the framework element factory.
        /// </summary>
        /// <param name="lookupControl">The lookup control.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <param name="lookupColumnDefinition">The lookup column definition.</param>
        /// <param name="designMode">if set to <c>true</c> [design mode].</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void ProcessFrameworkElementFactory(LookupControl lookupControl,
            FrameworkElementFactory factory, string dataColumnName,
            LookupColumnDefinitionBase lookupColumnDefinition, bool designMode)
        {
            TextAlignment gridTextAlignment;
            switch (TextAlignment)
            {
                case LookupColumnAlignmentTypes.Left:
                    gridTextAlignment = System.Windows.TextAlignment.Left;
                    break;
                case LookupColumnAlignmentTypes.Center:
                    gridTextAlignment = System.Windows.TextAlignment.Center;
                    break;
                case LookupColumnAlignmentTypes.Right:
                    gridTextAlignment = System.Windows.TextAlignment.Right;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            factory.SetValue(TextBlock.TextAlignmentProperty, gridTextAlignment);
            factory.SetBinding(TextBlock.TextProperty, new Binding(dataColumnName));

            var setPositiveValuesInGreen = false;
            var setNegativeValuesInRed = false;

            if (lookupColumnDefinition is LookupColumnDefinitionBase lookupColumnBase)
            {
                setNegativeValuesInRed = lookupColumnBase.ShowNegativeValuesInRed;
                setPositiveValuesInGreen = lookupColumnBase.ShowPositiveValuesInGreen;
            }
            else if (lookupColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                     && lookupFieldColumn.FieldDefinition is DecimalFieldDefinition decimalField)
            {
                setNegativeValuesInRed = decimalField.ShowNegativeValuesInRed;
                setPositiveValuesInGreen = decimalField.ShowPositiveValuesInGreen;
            }

            var converterParameter = new ValueToForegroundParameter();
            converterParameter.ShowNegativeValuesInRed = setNegativeValuesInRed;
            converterParameter.ShowPositiveValuesInGreen = setPositiveValuesInGreen;
            converterParameter.Parameter = lookupControl?.ListView?.Foreground;

            if (setNegativeValuesInRed || setPositiveValuesInGreen)
            {
                var binding = new Binding(dataColumnName);
                binding.Converter = new ValueToForegroundConverter();
                binding.ConverterParameter = converterParameter;
                factory.SetBinding(TextBlock.ForegroundProperty, binding);
            }

            //if (setPositiveValuesInGreen)
            //{
            //    var binding = new Binding(dataColumnName);
            //    binding.Converter = new ValueToForegroundColorConverterGreen();
            //    binding.ConverterParameter = lookupControl?.ListView?.Foreground;
            //    factory.SetBinding(TextBlock.ForegroundProperty, binding);

            //}
        }
    }
}

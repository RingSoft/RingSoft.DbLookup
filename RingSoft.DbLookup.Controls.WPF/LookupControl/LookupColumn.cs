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
        private string _header;
        /// <summary>
        /// Gets or sets the column header text.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
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

        private string _designText;

        /// <summary>
        /// Gets or sets the text to put into this column's cell for all rows in the ListView when in design mode.
        /// </summary>
        /// <value>
        /// The design text.
        /// </value>
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

        private double _width;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
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

        private bool _keepNullEmpty;

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
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        public string DataColumnName { get; internal set; }

        public LookupColumnDefinitionBase LookupColumnDefinition { get; internal set; }

        internal abstract DataTemplate GetCellDataTemplate(LookupControl lookupControl, string dataColumnName,
            bool designMode);


        public event PropertyChangedEventHandler PropertyChanged;

        [Properties.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class LookupColumn<TControl> : LookupColumnBase
        where TControl : FrameworkElement
    {
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

        protected abstract void ProcessFrameworkElementFactory(LookupControl lookupControl,
            FrameworkElementFactory factory, string dataColumnName,
            LookupColumnDefinitionBase lookupColumnDefinition, bool designMode);

        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TControl control)
            {

            }
        }
    }

        public class LookupColumn : LookupColumn<TextBlock>
    {
        private LookupColumnAlignmentTypes _textAlignment = LookupColumnAlignmentTypes.Left;

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>
        /// The text alignment.
        /// </value>
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

        public bool TextAlignmentChanged { get; private set; }

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

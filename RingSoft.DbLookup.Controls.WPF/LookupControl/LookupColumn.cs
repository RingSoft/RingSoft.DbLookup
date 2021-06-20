using System;
using RingSoft.DbLookup.Controls.WPF.Properties;
using RingSoft.DbLookup.Lookup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        /// <summary>
        /// Gets or sets the name of the property.  This is used to map this column to a visible LookupColumnDefinition in the LookupDefintion.  Use the NameOfExtension to make sure this value is correct.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        public string DataColumnName { get; internal set; }

        public LookupColumnDefinitionBase LookupColumnDefinition { get; internal set; }

        internal abstract DataTemplate GetCellDataTemplate(string dataColumnName);


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class LookupColumn<TControl> : LookupColumnBase
        where TControl : FrameworkElement
    {
        internal override DataTemplate GetCellDataTemplate(string dataColumnName)
        {
            var template = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TControl));
            ProcessFrameworkElementFactory(factory, dataColumnName, LookupColumnDefinition);

            template.VisualTree = factory;

            return template;
        }

        protected abstract void ProcessFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName,
            LookupColumnDefinitionBase lookupColumnDefinition);
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

        protected override void ProcessFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName,
            LookupColumnDefinitionBase lookupColumnDefinition)
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
        }
    }
}

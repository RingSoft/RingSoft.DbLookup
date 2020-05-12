using RingSoft.DbLookup.Controls.WPF.Properties;
using RingSoft.DbLookup.Lookup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupColumn : INotifyPropertyChanged
    {
        private string _header;
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

        private TextAlignment _textAlignment = TextAlignment.Left;
        
        public TextAlignment TextAlignment
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

        public string PropertyName { get; set; }

        public string DataColumnName { get; internal set; }

        public LookupColumnBase LookupColumnDefinition { get; internal set; }
            
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using RingSoft.DbLookup.Controls.WPF.Properties;
using RingSoft.DbLookup.Lookup;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        private double _width = 100;

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

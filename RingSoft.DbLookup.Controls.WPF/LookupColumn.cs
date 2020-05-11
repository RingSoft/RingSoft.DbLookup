using RingSoft.DbLookup.Controls.WPF.Properties;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupColumn : DependencyObject, INotifyPropertyChanged
    {
        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                if (_content == value)
                    return;

                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public string PropertyName { get; set; }

        public string DataColumnName { get; internal set; }
            
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DbMaintenance
{
    public class DbMaintenanceStatusBarViewModel : INotifyPropertyChanged
    {
        private DateTime? _lastSavedDate;

        public DateTime? LastSavedDate
        {
            get => _lastSavedDate;
            set
            {
                if (_lastSavedDate == value)
                {
                    return;
                }
                _lastSavedDate = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

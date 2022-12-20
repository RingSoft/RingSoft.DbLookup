using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup
{
    public interface IDeleteRecordView
    {
        void CloseWindow(bool result);
    }
    public class DeleteRecordViewModel : INotifyPropertyChanged
    {
        private bool _deleteAllData;

        public bool DeleteAllData
        {
            get => _deleteAllData;
            set
            {
                if (_deleteAllData == value)
                {
                    return;
                }
                _deleteAllData = value;
                OnPropertyChanged();
            }
        }

        private bool _nullAllData;

        public bool NullAllData
        {
            get => _nullAllData;
            set
            {
                if (_nullAllData == value)
                {
                    return;
                }
                _nullAllData = value;
                OnPropertyChanged();
            }
        }


        public IDeleteRecordView View { get; private set; }

        public RelayCommand OkCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public DeleteRecordViewModel()
        {
            OkCommand = new RelayCommand(() =>
            {
                View.CloseWindow(true);
            });

            CancelCommand = new RelayCommand(() =>
            {
                View.CloseWindow(false);
            });
        }

        public void Initialize(IDeleteRecordView view)
        {
            View = view;
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

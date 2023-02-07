using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup
{
    public interface IPrinterSetupView
    {
        void PrintOutput();

        void CloseWindow();
    }
    public class PrinterSetupViewModel : INotifyPropertyChanged
    {
        private ReportOutputTypes _outputType;

        public ReportOutputTypes OutputType
        {
            get => _outputType;
            set
            {
                if (_outputType == value)
                {
                    return;
                }
                _outputType = value;
                OnPropertyChanged();
            }
        }

        public IPrinterSetupView View { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public PrinterSetupArgs PrinterSetupArgs { get; private set; }

        public PrinterSetupViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(() =>
            {
                View.CloseWindow();
            });
        }
        public void Initialize(IPrinterSetupView view, PrinterSetupArgs printerSetupArgs)
        {
            View = view;
            PrinterSetupArgs = printerSetupArgs;
            OutputType = ReportOutputTypes.Printer;
        }

        private void OnOk()
        {
            PrinterSetupArgs.PrintingProperties.ReportOutputType = OutputType;
            View.PrintOutput();
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

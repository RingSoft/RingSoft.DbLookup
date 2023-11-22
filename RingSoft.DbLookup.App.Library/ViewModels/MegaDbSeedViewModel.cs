using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbMaintenance;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RingSoft.DbLookup.App.Library.ViewModels
{
    public class MegaDbSeedViewModel : INotifyPropertyChanged
    {
        public int MaxRecords
        {
            get => _maxRecords;
            set
            {
                if (_maxRecords == value)
                    return;

                _maxRecords = value;
                OnPropertyChanged(nameof(MaxRecords));
            }
        }

        public bool StartButtonEnabled
        {
            get => _startButtonEnabled;
            set
            {
                if (_startButtonEnabled == value)
                    return;

                _startButtonEnabled = value;
                OnPropertyChanged(nameof(StartButtonEnabled));
            }
        }

        public string StartButtonText
        {
            get => _startButtonText;
            set
            {
                if (_startButtonText == value)
                    return;

                _startButtonText = value;
                OnPropertyChanged(nameof(StartButtonText));
            }
        }

        public string ProgressLabelText
        {
            get => _progressLabelText;
            set
            {
                if (_progressLabelText == value)
                    return;

                _progressLabelText = value;
                OnPropertyChanged(nameof(ProgressLabelText));
            }
        }

        public bool CloseButtonEnabled
        {
            get => _closeButtonEnabled;
            set
            {
                if (_closeButtonEnabled == value)
                    return;

                _closeButtonEnabled = value;
                OnPropertyChanged(nameof(CloseButtonEnabled));
            }
        }

        public bool Processing { get; private set; }

        private const string StartButtonDefaultText = "Start Process";
        private const string ProgressLabelDefaultText = "Progress";

        private int _maxRecords = 1000000;
        private bool _startButtonEnabled = true;
        private string _startButtonText = StartButtonDefaultText;
        private string _progressLabelText = ProgressLabelDefaultText;
        private bool _closeButtonEnabled = true;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private IMegaDbSeedView _view;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnViewLoaded(IMegaDbSeedView view)
        {
            _view = view;
            SetStartButtonText(StartButtonDefaultText);
        }

        private void SetStartButtonText(string text)
        {
            if (_view != null)
            {
                StartButtonText = $"{_view.HotKeyPrefix}{text}";
            }
        }

        public async void StartProcess()
        {
            if (Processing)
            {
                _cancellationTokenSource.Cancel();
                StartButtonEnabled = false;
                ProgressLabelText = "Stopping Process.  Please Wait...";
            }
            else
            {
                Processing = true;
                SetStartButtonText("Stop Process");
                CloseButtonEnabled = false;

                RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.ItemsTableSeederProgress += MegaDbEfDataProcessor_ItemsTableSeederProgress;

                _cancellationTokenSource = new CancellationTokenSource();
                _cancellationToken = _cancellationTokenSource.Token;
                int result = 0;
                try
                {
                    result =
                        await RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.SeedItemsTable(MaxRecords, _cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                FinishProcess(result <= 0);
                if (result > 0)
                {
                    var message = "Operation Complete!";
                    _view.ShowInformationMessage(message, @"Items Table Seeding Operation");
                    _view.CloseWindow();
                }
                else
                {
                    _view.ShowValidationMessage("Operation Cancelled!", @"Items Table Seeding Operation");
                }
            }
        }

        private void MegaDbEfDataProcessor_ItemsTableSeederProgress(object sender, ItemsTableSeederProgressArgs e)
        {
            _view.ItemsTableSeederProgress(e);
        }

        private void FinishProcess(bool reset)
        {
            Processing = false;
            SetStartButtonText(StartButtonDefaultText);
            CloseButtonEnabled = true;
            StartButtonEnabled = true;
            RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.ItemsTableSeederProgress -=
                MegaDbEfDataProcessor_ItemsTableSeederProgress;

            if (reset)
            {
                ProgressLabelText = ProgressLabelDefaultText;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class DbMaintenanceUserControl : BaseUserControl, IDbMaintenanceView
    {
        public DbMaintenanceViewModelBase ViewModel { get; private set; }

        public Control MaintenanceButtons { get; private set; }

        public DbMaintenanceStatusBar StatusBar { get; private set; }

        public DbMaintenanceUserControlProcessor Processor { get; private set; }

        public string Title { get; private set; }

        public LookupAddViewArgs LookupAddViewArgs { get; set; }

        public object AddViewParameter { get; set; }

        public bool SetStartupFocus { get; set; } = true;

        public IUserControlHost Host { get; internal set; }

        public double WindowWidth { get; set; } = double.NaN;

        public double WindowHeight { get; set; } = Double.NaN;

        private AutoFillControl _keyControl;
        private bool _loaded;
        private PrimaryKeyValue _initPrimaryKey;
        private HotKeyProcessor _hotKeyProcessor;
        private FrameworkElement _lostFocusElement;

        public DbMaintenanceUserControl()
        {
            EnterToTab = true;
            Title = GetTitle();
            IsTabStop = false;
            Loaded += (sender, args) =>
            {
                if (_loaded)
                {
                    return;
                }
                ViewModel = OnGetViewModel();
                ViewModel.RecordSelectedEvent += (sender, args) =>
                {
                    ShowRecordTitle();
                };

                //Peter Ringering - 11/23/2024 12:17:07 PM - E-76
                ViewModel.SavedEvent += (sender, args) =>
                {
                    ShowRecordTitle();
                };

                ViewModel.NewEvent += (sender, args) =>
                {
                    Host.ChangeTitle(Title);
                };

                MaintenanceButtons = OnGetMaintenanceButtons();
                StatusBar = OnGetStatusBar();
                Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetUserControlProcessor(
                    ViewModel
                    , MaintenanceButtons
                    , this
                    , StatusBar
                    , Host);
                if (_keyControl != null)
                {
                    RegisterFormKeyControl(_keyControl);
                }

                if (_hotKeyProcessor != null)
                {
                    foreach (var hotKey in _hotKeyProcessor.HotKeys)
                    {
                        Processor.HotKeyProcessor.AddHotKey(hotKey);
                    }

                    foreach (var ignoreKey in _hotKeyProcessor.IgnoreKeys)
                    {
                        Processor.HotKeyProcessor.AddIgnoreKey(ignoreKey);
                    }
                    _hotKeyProcessor = null;
                }
                ViewModel.Processor = Processor;
                if (LookupAddViewArgs != null) Processor.InitializeFromLookupData(LookupAddViewArgs);
                ViewModel.InputParameter = AddViewParameter;
                if (!MaintenanceButtons.IsLoaded)
                {
                    MaintenanceButtons.Loaded += (o, eventArgs) =>
                    {
                        if (!_loaded)
                        {
                            FinishInit();
                        }
                    };
                    return;
                }
                FinishInit();
            };
        }

        private void FinishInit()
        {
            Processor.Initialize();
            ViewModel.OnViewLoaded(this);
            if (_initPrimaryKey != null)
            {
                ViewModel.OnRecordSelected(_initPrimaryKey);
                _initPrimaryKey = null;
            }

            if (SetStartupFocus)
            {
                SetInitialFocus();
            }

            _loaded = true;
        }

        public void SelectRecord(PrimaryKeyValue primaryKey)
        {
            if (_loaded)
            {
                ViewModel.OnRecordSelected(primaryKey);
            }
            else
            {
                _initPrimaryKey = primaryKey;
            }
        }

        protected virtual void ShowRecordTitle()
        {
            if (ViewModel.KeyAutoFillValue.IsValid())
            {
                Host.ChangeTitle($"{Title} - {ViewModel.KeyAutoFillValue.Text}");
            }
        }

        protected abstract DbMaintenanceViewModelBase OnGetViewModel();

        protected abstract Control OnGetMaintenanceButtons();

        protected abstract DbMaintenanceStatusBar OnGetStatusBar();

        protected abstract string GetTitle();

        public virtual void SetInitialFocus()
        {
            if (ViewModel == null)
            {
                return;
            }
            if (ViewModel.KeyAutoFillValue.IsValid())
            {
                ViewModel.KeyAutoFillUiCommand.SetFocus();
            }
        }

        public virtual void ResetViewForNewRecord()
        {
            
        }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            if (Processor == null)
            {
                _keyControl = keyAutoFillControl;
                return;
            }
            Processor.RegisterFormKeyControl(keyAutoFillControl);
            _keyControl = null;
        }

        public void ResetFocus()
        {

        }

        protected void AddHotKey(HotKey hotKey)
        {
            if (Processor == null)
            {
                if (_hotKeyProcessor == null)
                {
                    _hotKeyProcessor = new HotKeyProcessor();
                }
                _hotKeyProcessor.AddHotKey(hotKey);
                return;
            }
            Processor.HotKeyProcessor.AddHotKey(hotKey);
            _hotKeyProcessor = null;
        }

        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            Processor.OnReadOnlyModeSet(readOnlyValue);
            base.OnReadOnlyModeSet(readOnlyValue);
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (Processor.SetControlReadOnlyMode(control, readOnlyValue))
                base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}

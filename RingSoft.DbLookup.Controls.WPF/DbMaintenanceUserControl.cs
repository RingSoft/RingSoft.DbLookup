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

        private AutoFillControl _keyControl;
        private bool _loaded;
        private PrimaryKeyValue _initPrimaryKey;

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
                ViewModel.Processor = Processor;
                if (LookupAddViewArgs != null) Processor.InitializeFromLookupData(LookupAddViewArgs);
                ViewModel.InputParameter = AddViewParameter;
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
            };
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

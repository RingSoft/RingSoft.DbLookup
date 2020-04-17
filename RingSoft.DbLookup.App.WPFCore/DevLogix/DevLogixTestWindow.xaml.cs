using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WPFCore.Annotations;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.App.WPFCore.DevLogix
{
    /// <summary>
    /// Interaction logic for DevLogixTestWindow.xaml
    /// </summary>
    public partial class DevLogixTestWindow : INotifyPropertyChanged
    {
        private AutoFillSetup _autoFillSetup;

        public AutoFillSetup AutoFillSetup
        {
            get => _autoFillSetup;
            set
            {
                if (_autoFillSetup == value)
                    return;

                _autoFillSetup = value;
                OnPropertyChanged(nameof(AutoFillSetup));
            }
        }

        private AutoFillValue _autoFillValue;

        public AutoFillValue AutoFillValue
        {
            get => _autoFillValue;
            set
            {
                if (_autoFillValue == value)
                    return;

                _autoFillValue = value;
                OnPropertyChanged(nameof(AutoFillValue));
            }
        }

        private LookupDefinitionBase _reusableLookupDefinition;

        public LookupDefinitionBase ReusableLookupDefinition
        {
            get => _reusableLookupDefinition;
            set
            {
                if (_reusableLookupDefinition == value)
                    return;

                _reusableLookupDefinition = value;
                OnPropertyChanged(nameof(ReusableLookupDefinition));
            }
        }

        private LookupCommand _reusableCommand;

        public LookupCommand ReusableCommand
        {
            get => _reusableCommand;
            set
            {
                if (_reusableCommand == value)
                    return;

                _reusableCommand = value;
                OnPropertyChanged(nameof(ReusableCommand));
            }
        }

        private DevLogixLookupContextEfCore _devLogixLookupContext;

        public DevLogixTestWindow()
        {
            _devLogixLookupContext = new DevLogixLookupContextEfCore();

            InitializeComponent();

            Loaded += DevLogixTestWindow_Loaded;

            ChangeButton.Click += (sender, args) =>
            {
                AutoFillSetup = new AutoFillSetup(_devLogixLookupContext.DevLogixConfiguration.IssuesLookup);
                ReusableLookupDefinition = _devLogixLookupContext.DevLogixConfiguration.IssuesLookup;
                ReusableCommand = new LookupCommand(LookupCommands.Refresh);
            };
        }

        private void DevLogixTestWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AutoFillSetup = new AutoFillSetup(_devLogixLookupContext.DevLogixConfiguration.ErrorsLookup);
            ReusableLookupDefinition = _devLogixLookupContext.DevLogixConfiguration.ErrorsLookup;
            ReusableCommand = new LookupCommand(LookupCommands.Refresh);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

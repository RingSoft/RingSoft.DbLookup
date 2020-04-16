using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WinForms;
using RingSoft.DbLookup.Controls.WinForms.Annotations;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.App.WinForms.Forms.DevLogix
{
    public partial class DevLogixTestForm : BaseForm, INotifyPropertyChanged
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

        public DevLogixTestForm()
        {
            InitializeComponent();

            ErrorsLookupControl.LookupDefinition = WinFormsAppStart.DevLogixLookupContext.DevLogixConfiguration.ErrorsLookup;
            ErrorsLookupControl.RefreshData(true);

            ErrorsLookupControl.SizeChanged += (sender, args) =>
            {
                //textBox1.Text = $"{Size} Page Size = {ErrorsLookupControl.PageSize}";
            };

            IssuesLookupButton.Click += (sender, args) =>
            {
                var issuesLookupForm = new IssuesLookupTestForm();
                issuesLookupForm.ShowDialog();
            };

            CloseButton.Click += (sender, args) => Close();

            TestAutoFillControl.DataBindings.Add(nameof(TestAutoFillControl.Value), this, nameof(AutoFillValue), true,
                DataSourceUpdateMode.OnPropertyChanged);
            TestAutoFillControl.DataBindings.Add(nameof(TestAutoFillControl.Setup), this, nameof(AutoFillSetup),
                true, DataSourceUpdateMode.Never);

            ReusableLookupControl.DataBindings.Add(nameof(ReusableLookupControl.Command), this, nameof(ReusableCommand),
                true, DataSourceUpdateMode.Never);
            ReusableLookupControl.DataBindings.Add(nameof(ReusableLookupControl.LookupDefinition), this,
                nameof(ReusableLookupDefinition), true, DataSourceUpdateMode.Never);

            ChangeButton.Click += (sender, args) =>
            {
                AutoFillSetup = new AutoFillSetup(WinFormsAppStart.DevLogixLookupContext.DevLogixConfiguration.IssuesLookup);
                ReusableLookupDefinition = WinFormsAppStart.DevLogixLookupContext.DevLogixConfiguration.IssuesLookup;
                ReusableCommand = new LookupCommand(LookupCommands.Refresh);
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            AutoFillSetup = new AutoFillSetup(WinFormsAppStart.DevLogixLookupContext.DevLogixConfiguration.ErrorsLookup);
            ReusableLookupDefinition = WinFormsAppStart.DevLogixLookupContext.DevLogixConfiguration.ErrorsLookup;
            ReusableCommand = new LookupCommand(LookupCommands.Refresh);

            var error = new Library.DevLogix.Model.Error();
            error.Id = 124;
            AutoFillValue = new AutoFillValue(WinFormsAppStart.DevLogixLookupContext.Errors.GetPrimaryKeyValueFromEntity(error),
                "E-124");
            base.OnLoad(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

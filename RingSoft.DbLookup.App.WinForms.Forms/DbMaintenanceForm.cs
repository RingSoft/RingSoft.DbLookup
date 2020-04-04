using RingSoft.DbLookup.Controls.WinForms;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public partial class DbMaintenanceForm : BaseForm, IDbMaintenanceView
    {
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public virtual DbMaintenanceViewModelBase ViewModel { get; }

        public DbMaintenanceForm()
        {
            InitializeComponent();
            ButtonsPanel.SizeChanged += ButtonsPanel_SizeChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (ViewModel != null)
            {
                SelectButton.DataBindings.Add(nameof(SelectButton.Enabled), ViewModel,
                    nameof(ViewModel.SelectButtonEnabled), false, DataSourceUpdateMode.Never);
                DeleteButton.DataBindings.Add(nameof(DeleteButton.Enabled), ViewModel,
                    nameof(ViewModel.DeleteButtonEnabled), false, DataSourceUpdateMode.Never);
            }

            PreviousButton.Click += (sender, args) => ViewModel.OnGotoPreviousButton();
            FindButton.Click += (sender, args) => ViewModel.OnFindButton();
            CloseButton.Click += (sender, args) => Close();
            SelectButton.Click += (sender, args) => ViewModel.OnSelectButton();
            NextButton.Click += (sender, args) => ViewModel.OnGotoNextButton();
            NewButton.Click += (sender, args) => ViewModel.OnNewButton();
            SaveButton.Click += (sender, args) => ViewModel.OnSaveButton();
            DeleteButton.Click += (sender, args) => ViewModel.OnDeleteButton();

            ViewModel?.OnViewLoaded(this);

            base.OnLoad(e);
        }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            keyAutoFillControl.DataBindings.Add(nameof(keyAutoFillControl.IsDirty), ViewModel,
                nameof(ViewModel.KeyValueDirty), false, DataSourceUpdateMode.OnPropertyChanged);
            keyAutoFillControl.DataBindings.Add(nameof(keyAutoFillControl.Setup), ViewModel,
                nameof(ViewModel.KeyAutoFillSetup), true, DataSourceUpdateMode.Never);
            keyAutoFillControl.DataBindings.Add(nameof(keyAutoFillControl.Value), ViewModel,
                nameof(ViewModel.KeyAutoFillValue), true, DataSourceUpdateMode.OnPropertyChanged);

            keyAutoFillControl.Leave += (sender, args) =>
            {
                ViewModel.OnKeyControlLeave();
            };
        }

        private void ButtonsPanel_SizeChanged(object sender, EventArgs e)
        {
            var center = ButtonsPanel.Width / 2;
            var left = center - CenterPanel.Width / 2;
            CenterPanel.Left = left;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // ReSharper disable once InconsistentNaming
            const int WM_KEYDOWN = 0x100;

            // ReSharper disable once InconsistentNaming
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData & Keys.KeyCode)
                {
                    case Keys.Left:
                        if (ModifierKeys.HasFlag(Keys.Control))
                        {
                            ViewModel.OnGotoPreviousButton();
                            return true;
                        }
                        break;
                    case Keys.Right:
                        if (ModifierKeys.HasFlag(Keys.Control))
                        {
                            ViewModel.OnGotoNextButton();
                            return true;
                        }
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.OnWindowClosing(e);
            base.OnClosing(e);
        }

        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            ViewModel.InitializeFromLookupData(e);
        }

        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public virtual void ResetViewForNewRecord()
        {
        }

        void IDbMaintenanceView.ShowFindLookupForm(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor)
        {
            var lookupForm = new LookupForm(lookupDefinition, allowAdd, allowView, initialSearchFor);
            lookupForm.Icon = Icon;

            var result = lookupForm.ShowDialog(this);
            if (result != null)
            {
                LookupFormReturn?.Invoke(this, result);
            }
        }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;

        public void CloseWindow()
        {
            Close();
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption)
        {
            var result = MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Yes:
                    return MessageButtons.Yes;
                case DialogResult.No:
                    return MessageButtons.No;
            }

            return MessageButtons.Cancel;
        }

        public bool ShowYesNoMessage(string text, string caption)
        {
            if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                return true;

            return false;
        }

        public void ShowRecordSavedMessage()
        {
            MessageBox.Show(@"Record Saved.", @"Record Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

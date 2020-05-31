using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.WinForms.Forms.MegaDb;
using RingSoft.DbLookup.App.WinForms.Forms.Northwind;
using RingSoft.DbLookup.Controls.WinForms;
using System;
using System.Windows.Forms;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public partial class MainForm : BaseForm
    {
        public event EventHandler Done;

        private RegistrySettings _registrySettings;

        public MainForm()
        {
            InitializeComponent();
            CloseOnEscape = false;

            DatabaseSettingsButton.Click += DatabaseSettingsButton_Click;
            NorthwindButton.Click += NorthwindButton_Click;
            MegaDbButton.Click += MegaDbButton_Click;
            StockTrackerButton.Click += StockTrackerButton_Click;
            ExitButton.Click += (sender, args) => { Close(); };
            timer1.Tick += (sender, args) =>
            {
                timer1.Enabled = false;
                Done?.Invoke(this, EventArgs.Empty);
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            _registrySettings = new RegistrySettings();
            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Activate();
            timer1.Enabled = true;
        }

        private void StockTrackerButton_Click(object sender, EventArgs e)
        {
            if (!ValidateMegaDbWindow())
                return;

            var stockMasterForm = new StockMasterForm();
            stockMasterForm.ShowDialog();
        }

        private void MegaDbButton_Click(object sender, EventArgs e)
        {
            if (!ValidateMegaDbWindow())
                return;

            var itemsForm = new ItemForm();
            itemsForm.ShowDialog();
        }

        private void NorthwindButton_Click(object sender, EventArgs e)
        {
            if (!RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupContextConfiguration.TestConnection())
            {
                DatabaseSettingsButton.PerformClick();
                return;
            }
            var ordersForm = new OrdersForm();
            ordersForm.ShowDialog();
        }

        private void DatabaseSettingsButton_Click(object sender, EventArgs e)
        {
            var dbSetupForm = new DbSetupForm();
            dbSetupForm.ShowDialog();
            _registrySettings.LoadFromRegistry();
        }

        private bool ValidateMegaDbWindow()
        {
            var result = true;
            if (_registrySettings.MegaDbPlatformType == MegaDbPlatforms.None)
            {
                var message =
                    "The Mega Database platform type is set to None.  You must set it to a valid platform type before launching this window.";

                MessageBox.Show(message, @"Invalid Mega Database Platform Type", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                result = false;
            }

            if (result && !RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.MegaDbContextConfiguration
                .TestConnection())
                result = false;

            if (!result)
            {
                DatabaseSettingsButton.PerformClick();
            }

            return result;
        }
    }
}

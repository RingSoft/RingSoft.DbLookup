using RingSoft.DbLookup.App.Library;
using System;
using RingSoft.DbLookup.App.WinForms.Forms.MegaDb;
using RingSoft.DbLookup.App.WinForms.Forms.Northwind;
using RingSoft.DbLookup.Controls.WinForms;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public partial class MainForm : BaseForm
    {
        public event EventHandler Done;

        public MainForm()
        {
            InitializeComponent();
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

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Activate();
            timer1.Enabled = true;
        }

        private void StockTrackerButton_Click(object sender, EventArgs e)
        {
            if (!RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration.TestConnection())
            {
                DatabaseSettingsButton.PerformClick();
            }
            var stockMasterForm = new StockMasterForm();
            stockMasterForm.ShowDialog();
        }

        private void MegaDbButton_Click(object sender, EventArgs e)
        {
            if (!RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration.TestConnection())
            {
                DatabaseSettingsButton.PerformClick();
            }
            var itemsForm = new ItemForm();
            itemsForm.ShowDialog();
        }

        private void NorthwindButton_Click(object sender, EventArgs e)
        {
            if (!RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupContextConfiguration.TestConnection())
            {
                DatabaseSettingsButton.PerformClick();
            }
            var ordersForm = new OrdersForm();
            ordersForm.ShowDialog();
        }

        private void DatabaseSettingsButton_Click(object sender, EventArgs e)
        {
            var dbSetupForm = new DbSetupForm();
            dbSetupForm.ShowDialog();
        }
    }
}

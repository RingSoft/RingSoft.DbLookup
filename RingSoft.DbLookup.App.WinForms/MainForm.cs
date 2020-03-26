using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.GetDataProcessor;
using System;

namespace RingSoft.DbLookup.App.WinForms
{
    public partial class MainForm : BaseForm, IGetDataResultErrorViewer
    {
        public MainForm()
        {
            InitializeComponent();
            DatabaseSettingsButton.Click += DatabaseSettingsButton_Click;
            NorthwindButton.Click += NorthwindButton_Click;
            MegaDbButton.Click += MegaDbButton_Click;
            StockTrackerButton.Click += StockTrackerButton_Click;
            ExitButton.Click += (sender, args) => { Close(); };
        }

        private void StockTrackerButton_Click(object sender, EventArgs e)
        {
            if (!RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration.TestConnection())
            {
                DatabaseSettingsButton.PerformClick();
            }
            //var stockMasterForm = new StockMasterForm();
            //stockMasterForm.ShowDialog();
        }

        private void MegaDbButton_Click(object sender, EventArgs e)
        {
            if (!RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration.TestConnection())
            {
                DatabaseSettingsButton.PerformClick();
            }
            //var itemsForm = new ItemForm();
            //itemsForm.ShowDialog();
        }

        private void NorthwindButton_Click(object sender, EventArgs e)
        {
            if (!RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupContextConfiguration.TestConnection())
            {
                DatabaseSettingsButton.PerformClick();
            }
            //var ordersForm = new OrdersForm();
            //ordersForm.ShowDialog();
        }

        private void DatabaseSettingsButton_Click(object sender, EventArgs e)
        {
            //var dbSetupForm = new DbSetupForm();
            //dbSetupForm.ShowDialog();
        }

        public void ShowGetDataError(GetDataResult getDataResult)
        {
            var errorViewerForm = new SQLViewerForm(getDataResult);
            errorViewerForm.ShowDialog();
        }
    }
}

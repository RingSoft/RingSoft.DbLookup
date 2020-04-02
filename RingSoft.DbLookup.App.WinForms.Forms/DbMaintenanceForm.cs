using System;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
//using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public partial class DbMaintenanceForm : BaseForm
    {
        public DbMaintenanceForm()
        {
            InitializeComponent();
        }

        //public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ResetViewForNewRecord()
        //{
        //    throw new NotImplementedException();
        //}

        //public void ShowFindLookupForm(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<LookupSelectArgs> LookupFormReturn;
        //public void CloseWindow()
        //{
        //    throw new NotImplementedException();
        //}

        //public MessageButtons ShowYesNoCancelMessage(string text, string caption)
        //{
        //    throw new NotImplementedException();
        //}

        public bool ShowYesNoMessage(string text, string caption)
        {
            throw new NotImplementedException();
        }

        public void ShowRecordSavedMessage()
        {
            throw new NotImplementedException();
        }
    }
}

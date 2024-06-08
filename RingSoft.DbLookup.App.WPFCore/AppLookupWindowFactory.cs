using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppLookupWindowFactory : LookupWindowFactory
    {
        public override bool CanDisplayField(FieldDefinition fieldDefinition)
        {
            //if (RsDbLookupAppGlobals.EfProcessor != null)
            //{
            //    if (RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext != null)
            //    {
            //        if (RsDbLookupAppGlobals
            //            .EfProcessor
            //            .NorthwindLookupContext
            //            .OrderDetails
            //            .GetFieldDefinition(p => p.UnitPrice) == fieldDefinition)
            //        {
            //            return false;
            //        }
            //    }
            //}
            return base.CanDisplayField(fieldDefinition);
        }
    }
}

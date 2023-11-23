using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.EfCore
{
    public abstract class AppLookupContextEfCore : LookupContext
    {
        public abstract AppLookupContextConfiguration LookupContextConfiguration { get; }

        public DataProcessorTypes DataProcessorType
        {
            get { return LookupContextConfiguration.DataProcessorType; }
            set { LookupContextConfiguration.DataProcessorType = value; }
        }

        public override DbDataProcessor DataProcessor => LookupContextConfiguration.DataProcessor;
    }
}

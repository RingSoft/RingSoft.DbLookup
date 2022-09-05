﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFilterCellProps : AdvancedFindMemoCellProps
    {
        public const int FilterControlId = 55;

        public AdvancedFilterReturn FilterReturn { get; set; }

        public AdvancedFindFilterCellProps(DataEntryGridRow row, int columnId, string text, 
            AdvancedFilterReturn filter) : base(row, columnId, text)
        {
            Text = text;
            FilterReturn = filter;
        }

        public override int EditingControlId => FilterControlId;

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return Text;
            //return base.GetDataValue(row, columnId, controlMode);
        }
    }
}
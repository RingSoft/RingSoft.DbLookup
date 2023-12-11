// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-23-2023
// ***********************************************************************
// <copyright file="AdvancedFindLookupConfiguration.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindLookupConfiguration.
    /// </summary>
    public class AdvancedFindLookupConfiguration
    {
        /// <summary>
        /// The lookup context
        /// </summary>
        private IAdvancedFindLookupContext _lookupContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindLookupConfiguration"/> class.
        /// </summary>
        /// <param name="lookupContext">The lookup context.</param>
        public AdvancedFindLookupConfiguration(IAdvancedFindLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
        }

        /// <summary>
        /// Configures the lookups.
        /// </summary>
        public void ConfigureLookups()
        {
            var advancedFindLookup = new LookupDefinition<AdvancedFindLookup, AdvancedFind>(_lookupContext.AdvancedFinds);
            advancedFindLookup.AddVisibleColumnDefinition(p => p.Name, "Name"
                , p => p.Name, 50);

            advancedFindLookup.AddVisibleColumnDefinition(p => p.TableName, "Table"
                , p => p.Table, 50);

            _lookupContext.AdvancedFindLookup = advancedFindLookup;

            _lookupContext.AdvancedFindColumnLookup = new LookupDefinition<AdvancedFindLookup, AdvancedFindColumn>(
                _lookupContext.AdvancedFindColumns);

            _lookupContext.AdvancedFindColumnLookup.AddVisibleColumnDefinition(
                p => p.Name
                , "Caption"
                , p => p.Caption, 99);
            _lookupContext.AdvancedFindColumns.HasLookupDefinition(_lookupContext.AdvancedFindColumnLookup);

            _lookupContext.AdvancedFindFilterLookup = new LookupDefinition<AdvFindFilterLookup, AdvancedFindFilter>(
                _lookupContext.AdvancedFindFilters);
            _lookupContext.AdvancedFindFilterLookup.AddVisibleColumnDefinition(
                p => p.TableName
                , "Table"
                , p => p.TableName, 50);
            _lookupContext.AdvancedFindFilterLookup.AddVisibleColumnDefinition(
                p => p.FieldName
                , "Field"
                , p => p.FieldName, 50);
            _lookupContext.AdvancedFindFilters.HasLookupDefinition(_lookupContext.AdvancedFindFilterLookup);

            _lookupContext.AdvancedFinds.HasLookupDefinition(advancedFindLookup);

            var recordLockingLookup = new LookupDefinition<RecordLockingLookup, RecordLock>(_lookupContext.RecordLocks);
            recordLockingLookup.AddVisibleColumnDefinition(p => p.Table, "Table"
                , p => p.Table, 34);
            recordLockingLookup.AddVisibleColumnDefinition(p => p.LockDate, "Lock Date"
                , p => p.LockDateTime, 33);
            recordLockingLookup.AddVisibleColumnDefinition(p => p.User, "User"
                , p => p.User, 33);

            _lookupContext.RecordLockingLookup = recordLockingLookup;

            _lookupContext.RecordLocks.HasLookupDefinition(recordLockingLookup);
        }

        /// <summary>
        /// Initializes the model.
        /// </summary>
        public void InitializeModel()
        {
            _lookupContext.AdvancedFinds.GetFieldDefinition(p => p.FromFormula).IsMemo();
            _lookupContext.AdvancedFinds.RecordDescription = "Advanced Find";
            _lookupContext.AdvancedFinds.PriorityLevel = 10;
            _lookupContext.AdvancedFinds.IsAdvancedFind = true;

            _lookupContext.AdvancedFindColumns.SetHeaderEntity<AdvancedFind>();
            _lookupContext.AdvancedFindColumns.GetFieldDefinition(p => p.Formula).IsMemo();

            _lookupContext.AdvancedFindColumns.GetFieldDefinition(p => p.DecimalFormatType)
                .IsEnum<DecimalEditFormatTypes>();

            _lookupContext.AdvancedFindColumns.GetFieldDefinition(p => p.FieldDataType)
                .IsEnum<FieldDataTypes>();
            _lookupContext.AdvancedFindColumns.IsAdvancedFind = true;

            _lookupContext.AdvancedFindFilters.SetHeaderEntity<AdvancedFind>();
            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.Formula).IsMemo();

            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.EndLogic)
                .IsEnum<EndLogics>();

            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.FormulaDataType)
                .IsEnum<FieldDataTypes>();

            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.Operand)
                .HasDescription("Condition").IsEnum<Conditions>();

            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.DateFilterType)
                .IsEnum<DateFilterTypes>();

            _lookupContext.AdvancedFindFilters.IsAdvancedFind = true;

            _lookupContext.RecordLocks.GetFieldDefinition(p => p.LockDateTime).HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();

            _lookupContext.RecordLocks.IsAdvancedFind = true;
        }
    }
}

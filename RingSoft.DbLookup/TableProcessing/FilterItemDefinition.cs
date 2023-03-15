using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Linq;
using RingSoft.DbLookup.Lookup;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using Google.Protobuf.WellKnownTypes;
using MySqlX.XDevAPI.Common;

namespace RingSoft.DbLookup.TableProcessing
{
    public enum FilterItemTypes
    {
        Field = 0,
        Formula = 1,
        AdvancedFind = 2
    }

    /// <summary>
    /// Represents a filter item in a TableFilterDefinition.
    /// </summary>
    public abstract class FilterItemDefinition
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public abstract FilterItemTypes Type { get; }

        /// <summary>
        /// Gets the table filter definition.
        /// </summary>
        /// <value>
        /// The table filter definition.
        /// </value>
        public TableFilterDefinitionBase TableFilterDefinition { get; internal set; }

        /// <summary>
        /// Gets the left parentheses count.
        /// </summary>
        /// <value>
        /// The left parentheses count.
        /// </value>
        public int LeftParenthesesCount { get; set; }

        /// <summary>
        /// Gets the right parentheses count.
        /// </summary>
        /// <value>
        /// The right parentheses count.
        /// </value>
        public int RightParenthesesCount { get; set; }

        /// <summary>
        /// Gets the end logic.
        /// </summary>
        /// <value>
        /// The end logic.
        /// </value>
        public EndLogics EndLogic { get; set; }

        /// <summary>
        /// Gets the join definition.
        /// </summary>
        /// <value>
        /// The join definition.
        /// </value>
        
        private TableFieldJoinDefinition _joinDefinition;

        public TableFieldJoinDefinition JoinDefinition
        {
            get { return _joinDefinition; }
            set
            {
                _joinDefinition = value;
                SetTableDescription();
            }
        }


        public string TableDescription { get; set; }

        public string FieldDescription { get; set; }

        public string ReportDescription { get; set; }

        public string Path { get; internal set; }

        public abstract TreeViewType TreeViewType { get; }

        public bool IsFixed { get; internal set; }

        /// <summary>
        /// Gets the value to filter.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; internal set; }

        private string _displayValue;

        public string DisplayValue
        {
            get
            {
                if (_displayValue.IsNullOrEmpty())
                {
                    return Value;
                }
                return _displayValue;
            }
            set { _displayValue = value; }
        }



        public DateFilterTypes DateFilterType { get; private set; }

        public int DateFilterValue { get; private set; }

        public ValueTypes ValueType { get; set; }

        private DbDateTypes? _dateType;
            
        public DbDateTypes DateType
        {
            get
            {
                if (_dateType == null)
                {
                    if (SystemGlobals.ConvertAllDatesToUniversalTime)
                    {
                        return DbDateTypes.DateTime;
                    }
                    return DbDateTypes.DateOnly;
                }

                return _dateType.Value;
            }
            internal set => _dateType = value;
        }

        public FilterItemDefinition(TableFilterDefinitionBase tableFilterDefinition)
        {
            TableFilterDefinition = tableFilterDefinition;
            TableDescription = tableFilterDefinition.TableDefinition.Description;
        }

        internal virtual void CopyFrom(FilterItemDefinition source)
        {
            LeftParenthesesCount = source.LeftParenthesesCount;
            RightParenthesesCount = source.RightParenthesesCount;
            TableFilterDefinition = source.TableFilterDefinition;
            JoinDefinition = source.JoinDefinition;
            EndLogic = source.EndLogic;
            TableDescription = source.TableDescription;
            ReportDescription = source.ReportDescription;
            ValueType = source.ValueType;
            DateFilterType = source.DateFilterType;
            DateFilterValue = source.DateFilterValue;
            IsFixed = source.IsFixed;
        }

        public virtual void SetTableDescription()
        {
            if (JoinDefinition != null && JoinDefinition.ForeignKeyDefinition != null)
            {
                TableDescription = JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField.Description;
            }

        }


        public abstract string GetReportText(LookupDefinitionBase lookupDefinition, bool printMode);

        public string GetPrintText(LookupDefinitionBase lookupDefinition)
        {
            var lParen = GblMethods.StringDuplicate("(", LeftParenthesesCount);
            var result = GetReportText(lookupDefinition, true);
            var rParen = GblMethods.StringDuplicate(")", RightParenthesesCount);
            result = lParen + result + rParen;

            return result;
        }

        public string PrintEndLogicText()
        {
            var enumTranslation = new EnumFieldTranslation();
            enumTranslation.LoadFromEnum<EndLogics>();

            var result = " " + enumTranslation.TypeTranslations
                .FirstOrDefault(p => p.NumericValue == (int)EndLogic).TextValue + "\r\n";

            return result;
        }

        public string GetDateReportText()
        {
            var result = string.Empty;
            if (ValueType == ValueTypes.DateTime)
            {
                if (DateFilterType == DateFilterTypes.SpecificDate)
                {
                    var date = DisplayValue.ToDate();
                    if (date.HasValue)
                    {
                        return date.Value.FormatDateValue(DateType, false);
                    }
                }
                var enumTrans = new EnumFieldTranslation();
                enumTrans.LoadFromEnum<DateFilterTypes>();
                var typeTranslation = enumTrans.TypeTranslations.FirstOrDefault(p => p.NumericValue == (int)DateFilterType);
                if (typeTranslation != null)
                {
                    return $"{DateFilterValue} {typeTranslation.TextValue}";
                }
            }
            return result;
        }

        public static string GetConditionText(Conditions condition)
        {
            var searchValue = string.Empty;
            switch (condition)
            {
                case Conditions.Equals:
                    searchValue = "= ";
                    break;
                case Conditions.NotEquals:
                    searchValue = "<> ";
                    break;
                case Conditions.GreaterThan:
                    searchValue = "> ";
                    break;
                case Conditions.GreaterThanEquals:
                    searchValue = ">= ";
                    break;
                case Conditions.LessThan:
                    searchValue = "< ";
                    break;
                case Conditions.LessThanEquals:
                    searchValue = "<= ";
                    break;
                case Conditions.Contains:
                    searchValue = "Contains ";
                    break;
                case Conditions.NotContains:
                    searchValue = "Does Not Contain ";
                    break;
                case Conditions.EqualsNull:
                    searchValue = "Equals NULL";
                    break;
                case Conditions.NotEqualsNull:
                    searchValue = "Does Not Equal NULL";
                    break;
                case Conditions.BeginsWith:
                    searchValue = "Begins With ";
                    break;
                case Conditions.EndsWith:
                    searchValue = "Ends With ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return searchValue;
        }

        public virtual bool LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition, string path = "")
        {
            LeftParenthesesCount = entity.LeftParentheses;
            RightParenthesesCount = entity.RightParentheses;
            EndLogic = (EndLogics)entity.EndLogic;
            DateFilterType = (DateFilterTypes)entity.DateFilterType;
            Value = entity.SearchForValue;
            if (ValueType == ValueTypes.DateTime && DateFilterType != DateFilterTypes.SpecificDate)
            {
                DateFilterValue = Value.ToInt();
            }
            Path = entity.Path;
            //var process = false;
            //if (entity.Path.IsNullOrEmpty())
            //{
            //    process = true;
            //}
            //else
            //{
            //    var newPath = entity.Path;
            //    if (entity.Path != path)
            //    {
            //        newPath = path + entity.Path;
            //    }
            //    var treeViewItem =
            //        lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(newPath, TreeViewType);
            //    process = treeViewItem != null;
            //}
            //if (!process)
            //{
            //    var message = "This advanced find is corrupt. Please delete it.";
            //    var caption = "Corrupt Advanced Find.";
            //    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Error);
            //}
            return true;
            //else
            {
                //if (filterItemDefinition is FormulaFilterDefinition formulaFilter)
                //{
                //    if (!entity.Path.IsNullOrEmpty())
                //    {
                //        filterItemDefinition.TableDescription = foundItem.Name;
                //    }
                //}
                //else
                //{
                //    filterItemDefinition.TableDescription = TableDefinition.Description;
                //}
            }

        }

        public virtual void SaveToEntity(AdvancedFindFilter entity)
        {
            entity.Path = Path;
            entity.LeftParentheses = (byte)LeftParenthesesCount;
            entity.RightParentheses = (byte)RightParenthesesCount;
            entity.EndLogic = (byte)EndLogic;
            entity.DateFilterType = (byte)DateFilterType;
            if (DateFilterType != DateFilterTypes.SpecificDate)
            {
                entity.SearchForValue = DateFilterValue.ToString();
            }
            else
            {
                entity.SearchForValue = DisplayValue;
            }
        }

        public virtual string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            var searchValue = filterReturn.SearchValue;
            DateFilterType = filterReturn.DateFilterType;

            if (ValueType == ValueTypes.DateTime && DateFilterType != DateFilterTypes.SpecificDate)
            {
                DateFilterValue = searchValue.ToInt();
            }
            //searchValue = GetSearchValue(searchValue);

            return searchValue;

        }

        protected internal virtual DateTime ConvertToUniversalTime(DateTime date)
        {
            return date;
        }

        public virtual string GetSearchValue(string searchValue)
        {
            if (ValueType == ValueTypes.DateTime)
            {
                DisplayValue = searchValue;
                searchValue = ConvertDate(searchValue);
                if (SystemGlobals.ConvertAllDatesToUniversalTime)
                {
                    var date = searchValue.ToDate();
                    if (date != null)
                    {
                        date = ConvertToUniversalTime(date.Value);
                        searchValue = date.Value.FormatDateValue(DbDateTypes.DateTime);
                    }

                }

            }

            return searchValue;
        }

        public virtual void SaveToFilterReturn(AdvancedFilterReturn filterReturn)
        {
            filterReturn.Path = Path;
            filterReturn.DateFilterType = DateFilterType;
            if (DateFilterType != DateFilterTypes.SpecificDate)
            {
                filterReturn.SearchValue = DateFilterValue.ToString();
            }
        }

        protected internal TableDefinitionBase GetTableFieldForFilter(LookupDefinitionBase lookupDefinition,
            AdvancedFindFilter entity, out FieldDefinition fieldDefinition,
            out FieldDefinition filterField)
        {
            var tableDefinition =
                lookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                    p.EntityName == entity.TableName);

            fieldDefinition = null;
            filterField = null;
            if (!entity.FieldName.IsNullOrEmpty())
            {
                filterField = fieldDefinition =
                    tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);
            }

            TableDefinitionBase primaryTable = null;
            FieldDefinition primaryField = null;
            if (!entity.PrimaryTableName.IsNullOrEmpty() && !entity.PrimaryFieldName.IsNullOrEmpty())
            {
                primaryTable =
                    lookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.TableName == entity.PrimaryTableName);

                primaryField =
                    primaryTable.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.PrimaryFieldName);

                if (fieldDefinition == null)
                {
                    tableDefinition = primaryTable;
                    fieldDefinition = primaryField;
                }
            }

            return tableDefinition;
        }

        protected internal virtual string ConvertDate(string value)
        {
            switch (DateFilterType)
            {
                case DateFilterTypes.SpecificDate:
                    break;
                default:
                    DateFilterValue = value.ToInt();
                    var result = LookupDefinitionBase.ProcessSearchValue(value, DateFilterType);
                    return result;
            }
            return value;
        }

        protected internal virtual string GetReportBeginTextPrintMode(LookupDefinitionBase lookupDefinition)
        {
            var result = string.Empty;
            TreeViewItem foundItem = null;

            if (!Path.IsNullOrEmpty())
            {
                foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path, TreeViewType);
            }

            if (foundItem != null)
            {
                if (foundItem.Parent != null)
                {
                    result += $"{foundItem.Parent.Name}.";
                }
                else
                {
                    result += $"{foundItem.FieldDefinition.TableDefinition.Description}.";
                }
                result += $"{foundItem.Name} ";
            }
            return result;
        }

        public abstract FilterItemDefinition GetNewFilterItemDefinition();

        internal abstract string GetNewPath();

        public void CopyToNewFilter(LookupDefinitionBase lookupDefinition)
        {
            var newFilter = GetNewFilterItemDefinition();
            if (newFilter != null)
            {
                newFilter.CopyFrom(this);
                lookupDefinition.FilterDefinition.AddFixedFilter(newFilter);
                if (Path.IsNullOrEmpty())
                {
                    //if (JoinDefinition != null)
                    //{
                    //    TableDescription = JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField.Description;
                    //}
                    Path = GetNewPath();
                }
                GetJoins(lookupDefinition, newFilter);
            }
        }

        private void GetJoins(LookupDefinitionBase lookupDefinition, FilterItemDefinition newFilter)
        {
            var foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path);
            if (foundItem != null)
            {
                var index = lookupDefinition.FilterDefinition.Joins.Count;
                var join = lookupDefinition.AdvancedFindTree.MakeIncludes(foundItem).LookupJoin;
                if (join != null)
                {
                    newFilter.JoinDefinition = join.JoinDefinition;
                }

                //TableDescription = foundItem.Name;
            }
        }
    }
}

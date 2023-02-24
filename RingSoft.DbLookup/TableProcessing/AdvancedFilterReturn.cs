using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    public class AdvancedFilterReturn
    {
        public FieldDefinition FieldDefinition { get; set; }
        public string PrimaryTableName { get; set; }
        public FieldDefinition PrimaryFieldDefinition { get; set; }
        public Conditions Condition { get; set; }
        public string SearchValue { get; set; }
        public string Formula { get; set; }
        public string FormulaDisplayValue { get; set; }
        public FieldDataTypes FormulaValueType { get; set; }
        public LookupDefinitionBase LookupDefinition { get; set; }
        public string TableDescription { get; set; }
        public DateFilterTypes DateFilterType { get; set; }
        public string Path { get; set; }
        public int NewIndex { get; set; } = -1;
    }
}

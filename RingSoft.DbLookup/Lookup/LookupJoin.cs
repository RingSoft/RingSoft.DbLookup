using System;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A lookup table join definition.
    /// </summary>
    public class LookupJoin : IJoinParent
    {
        /// <summary>
        /// Gets the join definition.
        /// </summary>
        /// <value>
        /// The join definition.
        /// </value>
        public TableFieldJoinDefinition JoinDefinition { get; internal set; }

        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>
        /// The lookup definition.
        /// </value>
        public LookupDefinitionBase LookupDefinition { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupJoin"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="foreignFieldDefinition">The foreign field definition.</param>
        public LookupJoin(LookupDefinitionBase lookupDefinition, FieldDefinition foreignFieldDefinition)
        {
            LookupDefinition = lookupDefinition;
            SetJoinDefinition(foreignFieldDefinition);
        }

        protected internal LookupJoin(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        protected internal void SetJoinDefinition(FieldDefinition foreignFieldDefinition)
        {
            if (foreignFieldDefinition.ParentJoinForeignKeyDefinition == null)
                throw new Exception("There is no parent join definition for this field.");

            if (JoinDefinition == null)
            {
                if (foreignFieldDefinition.TableDefinition != LookupDefinition.TableDefinition)
                    throw new Exception(
                        $"Field: '{foreignFieldDefinition}' is not in the lookup's table '{LookupDefinition.TableDefinition}'");
            }
            else
            {
                if (foreignFieldDefinition.TableDefinition != JoinDefinition.ForeignKeyDefinition.PrimaryTable)
                    throw new Exception("Parent Field Definition's table definition is not the parent join's primary table.");
            }

            var parentJoinAlias = string.Empty;
            if (JoinDefinition != null)
                parentJoinAlias = JoinDefinition.Alias;

            JoinDefinition = new TableFieldJoinDefinition
            {
                ForeignKeyDefinition = foreignFieldDefinition.ParentJoinForeignKeyDefinition,
                ParentAlias = parentJoinAlias
            };

            LookupDefinition.AddJoin(JoinDefinition);
        }

        /// <summary>
        /// Adds a visible column definition.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(FieldDefinition fieldDefinition)
        {
            return AddVisibleColumnDefinition(string.Empty, fieldDefinition, 0);
        }

        /// <summary>
        /// Adds a visible column definition.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="percentWidth">The percent of the lookup's total width.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(string caption, FieldDefinition fieldDefinition, double percentWidth)
        {
            ValidateFieldDefinition(fieldDefinition);

            var columnDefinition = LookupDefinition.AddVisibleColumnDefinition(caption, fieldDefinition, percentWidth, "");
            columnDefinition.ParentObject = this;
            columnDefinition.ParentField = ParentField;
            columnDefinition.JoinQueryTableAlias = JoinDefinition.Alias;
            return columnDefinition;
        }

        private void ValidateFieldDefinition(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition.TableDefinition != JoinDefinition.ForeignKeyDefinition.PrimaryTable)
                throw new ArgumentException(
                    $"Field Definition table definition '{fieldDefinition.TableDefinition}' doesn't match {JoinDefinition.ForeignKeyDefinition.PrimaryTable}");
        }

        public LookupFormulaColumnDefinition AddVisibleColumnDefinition(string caption, string formula,
            double percentWidth, FieldDataTypes fieldDataType)
        {
            var column = LookupDefinition.AddVisibleColumnDefinition(caption, formula, percentWidth, fieldDataType, JoinDefinition.Alias);
            column.JoinQueryTableAlias = JoinDefinition.Alias;
            return column;
        }


        /// <summary>
        /// Adds the hidden column.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddHiddenColumn(FieldDefinition fieldDefinition)
        {
            ValidateFieldDefinition(fieldDefinition);

            var columnDefinition = LookupDefinition.AddHiddenColumn(fieldDefinition);
            columnDefinition.JoinQueryTableAlias = JoinDefinition.Alias;
            return columnDefinition;
        }

        /// <summary>
        /// Includes the specified foreign field definition.
        /// </summary>
        /// <param name="foreignFieldDefinition">The foreign field definition.</param>
        /// <returns></returns>
        public LookupJoin Include(FieldDefinition foreignFieldDefinition)
        {
            var lookupJoin = new LookupJoin(LookupDefinition);
            lookupJoin.JoinDefinition = JoinDefinition;
            lookupJoin.SetJoinDefinition(foreignFieldDefinition);
            return lookupJoin;
        }

        public IJoinParent ParentObject { get; set; }
        public FieldDefinition ChildField { get; set; }
        public FieldDefinition ParentField { get; set; }

        public LookupJoin MakeInclude(LookupDefinitionBase lookupDefinition, FieldDefinition childField = null)
        {
            return new LookupJoin(lookupDefinition, ChildField);
        }

        public LookupColumnDefinitionBase AddVisibleColumnDefinitionField(string caption, FieldDefinition fieldDefinition,
            double percentWidth)
        {
            return AddVisibleColumnDefinition(caption, fieldDefinition, percentWidth);
        }
    }
}

using System;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// The AutoFill types.
    /// </summary>
    public enum AutoFillTypes
    {
        Field = 0,
        Formula = 1
    }

    /// <summary>
    /// The AutoFill definition base class used to determine how the AutoFill engine will behave.
    /// </summary>
    public abstract class AutoFillDefinitionBase
    {
        /// <summary>
        /// Gets the AutoFill Definition type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public abstract AutoFillTypes Type { get; }

        /// <summary>
        /// Gets the AS SQL alias used in the SELECT clause.
        /// </summary>
        /// <value>
        /// The select SQL alias.
        /// </value>
        public string SelectSqlAlias { get; private set; }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public TableDefinitionBase TableDefinition { get; }

        /// <summary>
        /// Gets the filter definition used to filter the data.
        /// </summary>
        /// <value>
        /// The filter definition.
        /// </value>
        public TableFilterDefinitionBase FilterDefinition { get; internal set; }

        public string FromFormula { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillDefinitionBase"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public AutoFillDefinitionBase(TableDefinitionBase tableDefinition)
        {
            SelectSqlAlias = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            TableDefinition = tableDefinition;
            FilterDefinition = new TableFilterDefinitionBase(tableDefinition);

            TableDefinition.Context.Initialize();
        }
    }
}

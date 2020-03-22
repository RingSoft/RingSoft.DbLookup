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
    /// The AutoFill base class.
    /// </summary>
    public abstract class AutoFillBase
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public abstract AutoFillTypes Type { get; }

        /// <summary>
        /// Gets the select SQL alias.
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
        /// Gets the filter definition.
        /// </summary>
        /// <value>
        /// The filter definition.
        /// </value>
        public TableFilterDefinitionBase FilterDefinition { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillBase"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public AutoFillBase(TableDefinitionBase tableDefinition)
        {
            SelectSqlAlias = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            TableDefinition = tableDefinition;
            FilterDefinition = new TableFilterDefinitionBase();

            TableDefinition.Context.Initialize();
        }
    }
}

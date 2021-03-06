﻿using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// An AutoFill formula.
    /// </summary>
    /// <seealso cref="AutoFillDefinitionBase" />
    public class AutoFillFormulaDefinition : AutoFillDefinitionBase
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override AutoFillTypes Type => AutoFillTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillFormulaDefinition"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="formula">The formula.</param>
        public AutoFillFormulaDefinition(TableDefinitionBase tableDefinition, string formula) : base(tableDefinition)
        {
            Formula = formula;
        }
    }
}

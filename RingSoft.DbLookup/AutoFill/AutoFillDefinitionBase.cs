// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AutoFillDefinitionBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
        /// <summary>
        /// The field
        /// </summary>
        Field = 0,
        /// <summary>
        /// The formula
        /// </summary>
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
        /// <value>The type.</value>
        public abstract AutoFillTypes Type { get; }

        /// <summary>
        /// Gets the AS SQL alias used in the SELECT clause.
        /// </summary>
        /// <value>The select SQL alias.</value>
        public string SelectSqlAlias { get; private set; }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; }

        /// <summary>
        /// Gets the filter definition used to filter the data.
        /// </summary>
        /// <value>The filter definition.</value>
        public TableFilterDefinitionBase FilterDefinition { get; internal set; }

        /// <summary>
        /// Gets from formula.
        /// </summary>
        /// <value>From formula.</value>
        public string FromFormula { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillDefinitionBase" /> class.
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

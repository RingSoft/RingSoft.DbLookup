// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AutoFillFieldDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// An AutoFill field.
    /// </summary>
    /// <seealso cref="AutoFillDefinitionBase" />
    public class AutoFillFieldDefinition : AutoFillDefinitionBase
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public override AutoFillTypes Type => AutoFillTypes.Field;

        /// <summary>
        /// Gets the string field definition.
        /// </summary>
        /// <value>The string field definition.</value>
        public FieldDefinition FieldDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether this auto fill is distinct.
        /// </summary>
        /// <value><c>true</c> if distinct; otherwise, <c>false</c>.</value>
        public bool Distinct { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillFieldDefinition" /> class.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        public AutoFillFieldDefinition(FieldDefinition fieldDefinition) 
            : base(fieldDefinition.TableDefinition)
        {
            FieldDefinition = fieldDefinition;
        }

        /// <summary>
        /// Determines whether this auto fill is distinct.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>This object for fluent processing.</returns>
        /// <exception cref="System.ArgumentException">The distinct value can only be set on primary key field auto fills where there are at least 2 fields in the primary key.</exception>
        public AutoFillFieldDefinition IsDistinct(bool value = true)
        {
            var isPrimaryKey = FieldDefinition.TableDefinition.PrimaryKeyFields.Count > 1 &&
                               FieldDefinition.TableDefinition.PrimaryKeyFields.Contains(FieldDefinition);
            if (!isPrimaryKey && value)
                throw new ArgumentException(
                    "The distinct value can only be set on primary key field auto fills where there are at least 2 fields in the primary key.");

            Distinct = value;
            return this;
        }

    }
}

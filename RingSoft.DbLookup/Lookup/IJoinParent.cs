// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 01-30-2023
// ***********************************************************************
// <copyright file="IJoinParent.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Interface IJoinParent
    /// </summary>
    public interface IJoinParent
    {
        /// <summary>
        /// Gets or sets the parent object.
        /// </summary>
        /// <value>The parent object.</value>
        IJoinParent ParentObject { get; set; }

        /// <summary>
        /// Gets or sets the child field.
        /// </summary>
        /// <value>The child field.</value>
        FieldDefinition ChildField { get; set; }

        /// <summary>
        /// Gets or sets the parent field.
        /// </summary>
        /// <value>The parent field.</value>
        FieldDefinition ParentField { get; set; }

        /// <summary>
        /// Makes the include.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="childField">The child field.</param>
        /// <returns>LookupJoin.</returns>
        LookupJoin MakeInclude(LookupDefinitionBase lookupDefinition, FieldDefinition childField = null);

        /// <summary>
        /// Adds the visible column definition field.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="percentWidth">Width of the percent.</param>
        /// <returns>LookupColumnDefinitionBase.</returns>
        LookupColumnDefinitionBase AddVisibleColumnDefinitionField(string caption, FieldDefinition fieldDefinition,
            double percentWidth);

        /// <summary>
        /// Makes the path.
        /// </summary>
        /// <returns>System.String.</returns>
        string MakePath();

        /// <summary>
        /// Gets the type of the join.
        /// </summary>
        /// <value>The type of the join.</value>
        JoinTypes JoinType { get; internal set; }
    }
}

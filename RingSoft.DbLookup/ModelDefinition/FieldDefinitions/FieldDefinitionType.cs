// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-02-2023
// ***********************************************************************
// <copyright file="FieldDefinitionType.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A generic field definition type class.
    /// </summary>
    /// <typeparam name="TFieldDefinition">A class that derives from this class.  Used in fluent API methods.</typeparam>
    /// <seealso cref="FieldDefinition" />
    public abstract class FieldDefinitionType<TFieldDefinition> : FieldDefinition  where TFieldDefinition : FieldDefinitionType<TFieldDefinition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDefinitionType{TFieldDefinition}"/> class.
        /// </summary>
        internal FieldDefinitionType()
        {
            
        }

        /// <summary>
        /// Sets the name of the field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>TFieldDefinition.</returns>
        public new TFieldDefinition HasFieldName(string fieldName)
        {
            base.HasFieldName(fieldName);
            return (TFieldDefinition)this;
        }

        /// <summary>
        /// Sets the field description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>This object.</returns>
        public new TFieldDefinition HasDescription(string description)
        {
            base.HasDescription(description);
            return (TFieldDefinition) this;
        }

        /// <summary>
        /// Determines whether this field will allow nulls.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TFieldDefinition.</returns>
        public new TFieldDefinition IsRequired(bool value = true)
        {
            base.IsRequired(value);
            return (TFieldDefinition) this;
        }

        /// <summary>
        /// Determines whether [has search for host identifier] [the specified host identifier].
        /// </summary>
        /// <param name="hostId">The host identifier.</param>
        /// <returns>TFieldDefinition.</returns>
        public new TFieldDefinition HasSearchForHostId(int hostId)
        {
            base.HasSearchForHostId(hostId);
            return (TFieldDefinition)this;
        }

        /// <summary>
        /// Determines whether [has lookup control column identifier] [the specified lookup control column identifier].
        /// </summary>
        /// <param name="lookupControlColumnId">The lookup control column identifier.</param>
        /// <returns>TFieldDefinition.</returns>
        public new TFieldDefinition HasLookupControlColumnId(int lookupControlColumnId)
        {
            base.HasLookupControlColumnId(lookupControlColumnId);
            return (TFieldDefinition) this;
        }

        /// <summary>
        /// Determines whether [is update only] [the specified value].
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TFieldDefinition.</returns>
        public TFieldDefinition IsUpdateOnly(bool value)
        {
            base.SetUpdateOnly(value);
            return (TFieldDefinition)this;
        }

        /// <summary>
        /// Determines whether this instance [can set null] the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TFieldDefinition.</returns>
        public new TFieldDefinition CanSetNull(bool value)
        {
            base.CanSetNull(value);
            return (TFieldDefinition)this;
        }

        /// <summary>
        /// Does the skip print.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TFieldDefinition.</returns>
        public TFieldDefinition DoSkipPrint(bool value = true)
        {
            base.DoSkipPrint(value);
            return (TFieldDefinition)this;
        }

        /// <summary>
        /// Determines whether [has formula object] [the specified lookup formula].
        /// </summary>
        /// <param name="lookupFormula">The lookup formula.</param>
        /// <returns>TFieldDefinition.</returns>
        public TFieldDefinition HasFormulaObject(ILookupFormula lookupFormula)
        {
            base.HasFormulaObject(lookupFormula);
            return (TFieldDefinition)this;
        }

        /// <summary>
        /// Determines whether [is generated key] [the specified value].
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TFieldDefinition.</returns>
        public new TFieldDefinition IsGeneratedKey(bool value = true)
        {
            base.IsGeneratedKey(value);
            return (TFieldDefinition)this;
        }
    }
}

// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="LookupColumnDefinitionType.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Class LookupColumnDefinitionType.
    /// Implements the <see cref="RingSoft.DbLookup.Lookup.LookupColumnDefinitionBase" />
    /// </summary>
    /// <typeparam name="TColumnDefinition">The type of the t column definition.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.Lookup.LookupColumnDefinitionBase" />
    public abstract class LookupColumnDefinitionType<TColumnDefinition> : LookupColumnDefinitionBase
        where TColumnDefinition : LookupColumnDefinitionType<TColumnDefinition>
    {

        /// <summary>
        /// Sets the horizontal alignment type.
        /// </summary>
        /// <param name="alignmentType">The new horizontal alignment type.</param>
        /// <returns>This object for fluent coding.</returns>
        public new TColumnDefinition HasHorizontalAlignmentType(LookupColumnAlignmentTypes alignmentType)
        {
            base.HasHorizontalAlignmentType(alignmentType);
            return (TColumnDefinition) this;
        }

        /// <summary>
        /// Determines whether [has search for host identifier] [the specified host identifier].
        /// </summary>
        /// <param name="hostId">The host identifier.</param>
        /// <returns>TColumnDefinition.</returns>
        public new TColumnDefinition HasSearchForHostId(int hostId)
        {
            base.HasSearchForHostId(hostId);
            return (TColumnDefinition) this;
        }

        /// <summary>
        /// Determines whether [has lookup control column identifier] [the specified lookup control column identifier].
        /// </summary>
        /// <param name="lookupControlColumnId">The lookup control column identifier.</param>
        /// <returns>TColumnDefinition.</returns>
        public new TColumnDefinition HasLookupControlColumnId(int lookupControlColumnId)
        {
            base.HasLookupControlColumnId(lookupControlColumnId);
            return (TColumnDefinition) this;
        }

        /// <summary>
        /// Determines whether [has content template identifier] [the specified content template identifier].
        /// </summary>
        /// <param name="contentTemplateId">The content template identifier.</param>
        /// <returns>TColumnDefinition.</returns>
        public new TColumnDefinition HasContentTemplateId(int contentTemplateId)
        {
            base.HasContentTemplateId(contentTemplateId);
            return (TColumnDefinition) this;
        }

        /// <summary>
        /// Determines whether [has keep null empty] [the specified value].
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TColumnDefinition.</returns>
        public new TColumnDefinition HasKeepNullEmpty(bool value = true)
        {
            base.HasKeepNullEmpty(value);
            return (TColumnDefinition)this;
        }

        /// <summary>
        /// Does the show negative values in red.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TColumnDefinition.</returns>
        public new TColumnDefinition DoShowNegativeValuesInRed(bool value = true)
        {
            base.DoShowNegativeValuesInRed(value);
            return (TColumnDefinition)this;
        }

        /// <summary>
        /// Does the show positive values in green.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TColumnDefinition.</returns>
        public new TColumnDefinition DoShowPositiveValuesInGreen(bool value = true)
        {
            base.DoShowPositiveValuesInGreen(value);
            return (TColumnDefinition) this;
        }
    }
}

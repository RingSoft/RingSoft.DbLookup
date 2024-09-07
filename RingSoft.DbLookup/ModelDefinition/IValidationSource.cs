// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="IValidationSource.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.ModelDefinition
{
    /// <summary>
    /// Interface IValidationSource
    /// </summary>
    public interface IValidationSource
    {
        /// <summary>
        /// Gets a value indicating whether [validate all at once].
        /// </summary>
        /// <value><c>true</c> if [validate all at once]; otherwise, <c>false</c>.</value>
        bool ValidateAllAtOnce { get; }

        /// <summary>
        /// Gets the automatic fill value for nullable foreign key field.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>AutoFillValue.</returns>
        AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition);

        /// <summary>
        /// Called when [validation fail].
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption);

        /// <summary>
        /// Validates the entity property.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="valueToValidate">The value to validate.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ValidateEntityProperty(FieldDefinition fieldDefinition, string valueToValidate);
    }
}

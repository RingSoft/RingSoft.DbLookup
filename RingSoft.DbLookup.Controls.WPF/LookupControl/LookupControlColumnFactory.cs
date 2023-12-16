// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="LookupControlColumnFactory.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupControlColumnFactory.
    /// </summary>
    public class LookupControlColumnFactory
    {
        public LookupControlColumnFactory()
        {
            LookupControlsGlobals.LookupControlColumnFactory = this;
        }
        /// <summary>
        /// Creates the lookup column.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        /// <returns>LookupColumnBase.</returns>
        /// <exception cref="System.Exception">No ContentTemplateId found for LookupColumnDefinition {columnDefinition.Caption}</exception>
        /// <exception cref="System.Exception">No ContentTemplate found for Id {contentTemplateId.Value}, Column: {columnDefinition.Caption}</exception>
        public virtual LookupColumnBase CreateLookupColumn(LookupColumnDefinitionBase columnDefinition)
        {
            if (columnDefinition.LookupControlColumnId == LookupDefaults.CustomContentColumnId)
            {
                
                var customContentColumn = new LookupCustomContentColumn();
                if (columnDefinition.ContentTemplateId == null)
                {
                    var contentTemplateId = columnDefinition.ContentTemplateId;
                    if (contentTemplateId == null && columnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                                                  && lookupFieldColumn.FieldDefinition is IntegerFieldDefinition
                                                      integerFieldDefinition)
                        contentTemplateId = integerFieldDefinition.ContentTemplateId;

                    if (contentTemplateId == null)
                        throw new Exception(
                            $"No ContentTemplateId found for LookupColumnDefinition {columnDefinition.Caption}");

                    customContentColumn.ContentTemplate =
                        LookupControlsGlobals.LookupControlContentTemplateFactory.GetContentTemplate(contentTemplateId
                            .Value);

                    if (customContentColumn.ContentTemplate == null)
                        throw new Exception(
                            $"No ContentTemplate found for Id {contentTemplateId.Value}, Column: {columnDefinition.Caption}");
                }
                return customContentColumn;
            }

            var result = new LookupColumn{TextAlignment = columnDefinition.HorizontalAlignment};
            return result;
        }
    }
}

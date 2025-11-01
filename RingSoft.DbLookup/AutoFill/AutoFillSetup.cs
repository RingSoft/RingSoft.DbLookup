// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AutoFillSetup.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// All the properties necessary to set up an AutoFill control.
    /// </summary>
    public class AutoFillSetup
    {
        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether allow the lookup window to add-on-the-fly.
        /// </summary>
        /// <value><c>true</c> if allow lookup add; otherwise, <c>false</c>.</value>
        public bool AllowLookupAdd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to allow the lookup window to view a record on-the-fly.
        /// </summary>
        /// <value><c>true</c> if allow lookup view; otherwise, <c>false</c>.</value>
        public bool AllowLookupView { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this AutoFill is distinct and not allow duplicate rows.  Used only for Lookup Definitions who have more than 2 viewable primary key fields.
        /// </summary>
        /// <value><c>true</c> if distinct; otherwise, <c>false</c>.</value>
        public bool Distinct { get; set; }

        /// <summary>
        /// Gets or sets the add view parameter.
        /// </summary>
        /// <value>The add view parameter.</value>
        public object AddViewParameter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [set dirty].
        /// </summary>
        /// <value><c>true</c> if [set dirty]; otherwise, <c>false</c>.</value>
        public bool SetDirty { get; set; } = true;

        public IAutoFillControl Control { get; set; }

        /// <summary>
        /// Gets the foreign field.
        /// </summary>
        /// <value>The foreign field.</value>
        public FieldDefinition ForeignField { get; }

        public bool CanLookupAdd
        {
            get
            {
                if (AllowLookupAdd)
                {
                    if (ForeignField != null && ForeignField.ParentJoinForeignKeyDefinition != null)
                    {
                        return ForeignField.ParentJoinForeignKeyDefinition.PrimaryTable.CanAddToTable;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public event EventHandler<LookupAddViewArgs> LookupView;

        public event EventHandler<LookupAddViewArgs> LookupAdd;


        /// <summary>
        /// Initializes a new instance of the class with a lookup definition that is attached to the primary table of the parent join definition of the foreign field definition parameter.
        /// </summary>
        /// <param name="foreignKeyFieldDefinition">The foreign key field definition whose value will be set in the AutoFillValue's PrimaryKeyValue property value.</param>
        /// <exception cref="System.ArgumentException">Foreign key field does not have a parent foreign key definition.  Make sure you configure it properly in the Entity Framework.</exception>
        /// <exception cref="System.ArgumentException">Parent table '{foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable}' does not have a lookup definition.  Make sure you attach it in the LookupContext.InitializeLookupDefinitions override and execute {foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable}.HasLookupDefinition()</exception>
        public AutoFillSetup(FieldDefinition foreignKeyFieldDefinition)
        {
            if (foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition == null)
                throw new ArgumentException("Foreign key field does not have a parent foreign key definition.  Make sure you configure it properly in the Entity Framework.");

            if (foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition == null)
                throw new ArgumentException($"Parent table '{foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable}' does not have a lookup definition.  Make sure you attach it in the LookupContext.InitializeLookupDefinitions override and execute {foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable}.HasLookupDefinition()");

            ForeignField = foreignKeyFieldDefinition;

            LookupDefinition = foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillSetup" /> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public AutoFillSetup(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        /// <summary>
        /// Gets the automatic fill value for identifier value.
        /// </summary>
        /// <param name="idValue">The identifier value.</param>
        /// <returns>AutoFillValue.</returns>
        public AutoFillValue GetAutoFillValueForIdValue(string idValue)
        {
            return LookupDefinition.TableDefinition.Context.OnAutoFillTextRequest(LookupDefinition.TableDefinition, idValue);
        }
        /// <summary>
        /// Gets the automatic fill value for identifier value.
        /// </summary>
        /// <param name="idValue">The identifier value.</param>
        /// <returns>AutoFillValue.</returns>
        public AutoFillValue GetAutoFillValueForIdValue(int idValue)
        {
            return GetAutoFillValueForIdValue(idValue.ToString());
        }

        /// <summary>
        /// Gets the automatic fill value for identifier value.
        /// </summary>
        /// <param name="idValue">The identifier value.</param>
        /// <returns>AutoFillValue.</returns>
        public AutoFillValue GetAutoFillValueForIdValue(int? idValue)
        {
            return GetAutoFillValueForIdValue(idValue?.ToString());
        }

        public void OnLookupAdd(LookupAddViewArgs args)
        {
            LookupAdd?.Invoke(this, args);
        }

        public void OnLookupView(LookupAddViewArgs args)
        {
            LookupView?.Invoke(this, args);
        }

        public void HandleValFail(string description = "", bool? allowNulls = null)
        {
            if (Control != null)
            {
                if (description.IsNullOrEmpty())
                {
                    if (ForeignField == null)
                    {
                        throw new ArgumentException("You must provide a description if Foreign Field is null");
                    }

                    if (allowNulls == null)
                    {
                        allowNulls = ForeignField.AllowNulls;
                    }
                    description = ForeignField.Description;
                }
                Control.HandleValFail(description, allowNulls.GetValueOrDefault());
            }
        }
    }
}

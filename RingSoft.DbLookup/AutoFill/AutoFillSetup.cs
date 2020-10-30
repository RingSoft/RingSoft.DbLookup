using System;
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
        /// <value>
        /// The lookup definition.
        /// </value>
        public LookupDefinitionBase LookupDefinition { get; }

        /// <summary>
        /// Gets or sets a value indicating whether allow the lookup window to add-on-the-fly.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allow lookup add; otherwise, <c>false</c>.
        /// </value>
        public bool AllowLookupAdd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to allow the lookup window to view a record on-the-fly.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allow lookup view; otherwise, <c>false</c>.
        /// </value>
        public bool AllowLookupView { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this AutoFill is distinct and not allow duplicate rows.  Used only for Lookup Definitions who have more than 2 viewable primary key fields.
        /// </summary>
        /// <value>
        ///   <c>true</c> if distinct; otherwise, <c>false</c>.
        /// </value>
        public bool Distinct { get; set; }

        public object AddViewParameter { get; set; }

        /// <summary>
        /// Initializes a new instance of the class with a lookup definition that is attached to the primary table of the parent join definition of the foreign field definition parameter.
        /// </summary>
        /// <param name="foreignKeyFieldDefinition">The foreign key field definition whose value will be set in the AutoFillValue's PrimaryKeyValue property value.</param>
        /// <exception cref="ArgumentException">
        /// Foreign key field does not have a parent foreign key definition.  Make sure you configure it properly in the Entity Framework.
        /// or
        /// Parent table does not have a lookup definition.  Make sure you attach it in the LookupContext.InitializeLookupDefinitions override and execute HasLookupDefinition()
        /// </exception>
        public AutoFillSetup(FieldDefinition foreignKeyFieldDefinition)
        {
            if (foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition == null)
                throw new ArgumentException("Foreign key field does not have a parent foreign key definition.  Make sure you configure it properly in the Entity Framework.");

            if (foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition == null)
                throw new ArgumentException($"Parent table '{foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable}' does not have a lookup definition.  Make sure you attach it in the LookupContext.InitializeLookupDefinitions override and execute {foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable}.HasLookupDefinition()");

            LookupDefinition = foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillSetup"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public AutoFillSetup(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }
    }
}

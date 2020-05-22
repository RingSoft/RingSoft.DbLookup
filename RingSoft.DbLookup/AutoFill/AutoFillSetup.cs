using System;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// All the AutoFill setup properties.
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
        /// Gets or sets a value indicating whether allow lookup add.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allow lookup add; otherwise, <c>false</c>.
        /// </value>
        public bool AllowLookupAdd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to allow lookup view.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allow lookup view; otherwise, <c>false</c>.
        /// </value>
        public bool AllowLookupView { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this AutoFill is distinct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if distinct; otherwise, <c>false</c>.
        /// </value>
        public bool Distinct { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillSetup"/> class with a lookup definition that is attached to the primary table of the parent join definition of the foreign field definition parameter.
        /// </summary>
        /// <param name="foreignKeyFieldDefinition">The foreign key field definition.</param>
        /// <exception cref="ArgumentException">
        /// Foreign key field does not have a parent foreign key definition.  Make sure you define a parent join or configure it properly in the Entity Framework.
        /// or
        /// Parent table does not have a lookup definition.  Make sure you attach it in the LookupContext.InitializeLookupDefinitions override and execute HasLookupDefinition()
        /// </exception>
        public AutoFillSetup(FieldDefinition foreignKeyFieldDefinition)
        {
            if (foreignKeyFieldDefinition.ParentJoinForeignKeyDefinition == null)
                throw new ArgumentException("Foreign key field does not have a parent foreign key definition.  Make sure you define a parent join or configure it properly in the Entity Framework.");

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

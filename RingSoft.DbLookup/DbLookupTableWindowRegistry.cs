// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-14-2023
//
// Last Modified By : petem
// Last Modified On : 09-05-2024
// ***********************************************************************
// <copyright file="DbLookupTableWindowRegistry.cs" company="Peter Ringering">
//     2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Shows a window for a table definition.
    /// </summary>
    public abstract class DbLookupTableWindowRegistry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbLookupTableWindowRegistry" /> class.
        /// </summary>
        public DbLookupTableWindowRegistry()
        {
            ActivateRegistry();
        }

        /// <summary>
        /// Activates the registry.
        /// </summary>
        public virtual void ActivateRegistry()
        {
            SystemGlobals.TableRegistry = this;
        }

        /// <summary>
        /// Determines whether [is table registered] [the specified table definition].
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns><c>true</c> if [is table registered] [the specified table definition]; otherwise, <c>false</c>.</returns>
        public abstract bool IsTableRegistered(TableDefinitionBase tableDefinition);

        public abstract bool IsControlRegistered(TableDefinitionBase tableDefinition);

        /// <summary>
        /// Shows the add onthe fly window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="addViewArgs">The add view arguments.</param>
        /// <param name="inputParameter">The input parameter.</param>
        public abstract void ShowAddOntheFlyWindow(
            TableDefinitionBase tableDefinition
            , LookupAddViewArgs addViewArgs = null
            , object inputParameter = null);

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="inputParameter">The input parameter.</param>
        public abstract void ShowWindow(TableDefinitionBase tableDefinition, object inputParameter = null);

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="inputParameter">The input parameter.</param>
        public abstract void ShowDialog(TableDefinitionBase tableDefinition, object inputParameter = null);
    }
}

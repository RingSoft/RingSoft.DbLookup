// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 05-22-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="TestLookupDataBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Class TestLookupDataBase.
    /// Implements the <see cref="RingSoft.DbLookup.Lookup.ILookupDataBase" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.Lookup.ILookupDataBase" />
    public class TestLookupDataBase<TEntity> : ILookupDataBase where TEntity : class, new()
    {
        /// <summary>
        /// Gets the table to process.
        /// </summary>
        /// <value>The table to process.</value>
        public IQueryable<TEntity> TableToProcess { get; }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinition<TEntity> TableDefinition { get; }

        /// <summary>
        /// Occurs when [print output].
        /// </summary>
        public event EventHandler<LookupDataMauiPrintOutput> PrintOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLookupDataBase{TEntity}" /> class.
        /// </summary>
        /// <param name="listToQuery">The list to query.</param>
        /// <param name="tableDefinition">The table definition.</param>
        public TestLookupDataBase(IQueryable<TEntity> listToQuery, TableDefinition<TEntity> tableDefinition)
        {
            TableDefinition = tableDefinition;
            TableToProcess = listToQuery;
        }

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetRecordCount()
        {
            return TableToProcess.Count();
        }

        /// <summary>
        /// Does the print output.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        public void DoPrintOutput(int pageSize)
        {
            var args = new LookupDataMauiPrintOutput();
            foreach (var entity in TableToProcess)
            {
                var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                args.Result.Add(primaryKey);
            }

            PrintOutput?.Invoke(this, args);
        }
    }
}

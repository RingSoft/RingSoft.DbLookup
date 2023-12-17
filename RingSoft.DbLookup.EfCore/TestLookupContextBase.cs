// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 07-13-2023
//
// Last Modified By : petem
// Last Modified On : 11-23-2023
// ***********************************************************************
// <copyright file="TestLookupContextBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.Testing;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Class TestLookupContextBase.
    /// Implements the <see cref="RingSoft.DbLookup.EfCore.LookupContext" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.EfCore.LookupContext" />
    public abstract class TestLookupContextBase : LookupContext
    {
        /// <summary>
        /// Gets the data repository.
        /// </summary>
        /// <value>The data repository.</value>
        public TestDataRepository DataRepository { get; private set; }

        /// <summary>
        /// Gets the Entity Framework Core database context used to set up the table and field definition properties of inheriting classes.
        /// </summary>
        /// <value>The database context.</value>
        protected override DbContext DbContext => _context;

        /// <summary>
        /// The context
        /// </summary>
        private DbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLookupContextBase"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TestLookupContextBase(DbContext context)
        {
            DataRepository = new TestDataRepository(new DataRepositoryRegistry());
            DataRepository.Initialize();
            _context = context;
            Initialize();
        }

    }
}

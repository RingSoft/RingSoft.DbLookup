﻿// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 05-21-2024
// ***********************************************************************
// <copyright file="SystemGlobals.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.Printing.Interop;
using System;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Enum AlertLevels
    /// </summary>
    public enum AlertLevels
    {
        /// <summary>
        /// The green
        /// </summary>
        Green = 0,
        /// <summary>
        /// The yellow
        /// </summary>
        Yellow = 1,
        /// <summary>
        /// The red
        /// </summary>
        Red = 2,
    }

    /// <summary>
    /// Contains all the properties for this library.
    /// </summary>
    public static class SystemGlobals
    {
        /// <summary>
        /// Gets the advanced find database processor.
        /// </summary>
        /// <value>The advanced find database processor.</value>
        public static IAdvancedFindDbProcessor AdvancedFindDbProcessor { get; internal set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public static string UserName { get; set; }

        /// <summary>
        /// The program data folder
        /// </summary>
        private static string _programDataFolder;

        /// <summary>
        /// Gets or sets the program data folder.
        /// </summary>
        /// <value>The program data folder.</value>
        public static string ProgramDataFolder
        {
            get => _programDataFolder;
            set
            {
                _programDataFolder = value;
                PrintingInteropGlobals.Initialize(_programDataFolder);
            }
        }

        /// <summary>
        /// The context
        /// </summary>
        private static IAdvancedFindLookupContext _context;

        /// <summary>
        /// Gets the advanced find lookup context.
        /// </summary>
        /// <value>The advanced find lookup context.</value>
        /// <exception cref="System.Exception">Need to inherit and instantiate {nameof(LookupContextBase)} and run Initialize.</exception>
        public static IAdvancedFindLookupContext AdvancedFindLookupContext
        {
            get
            {
                if (_context == null)
                {
                    throw new Exception($"Need to inherit and instantiate {nameof(LookupContextBase)} and run Initialize.");
                }
                return _context;
            }
            internal set => _context = value;
        }

        /// <summary>
        /// The data repository
        /// </summary>
        private static SystemDataRepository _dataRepository;

        /// <summary>
        /// Gets the data repository.
        /// </summary>
        /// <value>The data repository.</value>
        /// <exception cref="System.ApplicationException"></exception>
        public static SystemDataRepository DataRepository
        {
            get
            {
                if (_dataRepository == null)
                {
                    var message = $"Need To inherit and instantiate {nameof(SystemDataRepository)}.";
                    throw new ApplicationException(message);
                }
                return _dataRepository;
            }
            internal set => _dataRepository = value;
        }


        /// <summary>
        /// Gets or sets the window alert level.
        /// </summary>
        /// <value>The window alert level.</value>
        public static AlertLevels WindowAlertLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [convert all dates to universal time].
        /// </summary>
        /// <value><c>true</c> if [convert all dates to universal time]; otherwise, <c>false</c>.</value>
        public static bool ConvertAllDatesToUniversalTime { get; set; }

        /// <summary>
        /// Gets or sets all dates format.
        /// </summary>
        /// <value>All dates format.</value>
        public static DbDateTypes AllDatesFormat { get; set; } = DbDateTypes.DateOnly;

        /// <summary>
        /// The lookup context
        /// </summary>
        private static LookupContextBase _lookupContext;
        /// <summary>
        /// Gets or sets the lookup context.
        /// </summary>
        /// <value>The lookup context.</value>
        /// <exception cref="System.Exception">Need to inherit and instantiate {nameof(LookupContextBase)}.</exception>
        public static LookupContextBase LookupContext
        {
            get
            {
                if (_lookupContext == null)
                {
                    throw new Exception($"Need to inherit and instantiate {nameof(LookupContextBase)}.");
                }
                return _lookupContext;
            }
            set => _lookupContext = value;
        }

        /// <summary>
        /// Gets the table registry.
        /// </summary>
        /// <value>The table registry.</value>
        public static DbLookupTableWindowRegistry TableRegistry { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether [unit test mode].
        /// </summary>
        /// <value><c>true</c> if [unit test mode]; otherwise, <c>false</c>.</value>
        public static bool UnitTestMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [validate deleted data].
        /// </summary>
        /// <value><c>true</c> if [validate deleted data]; otherwise, <c>false</c>.</value>
        public static bool ValidateDeletedData { get; set; } = true;

        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        /// <value>The rights.</value>
        public static AppRights Rights { get; set; }

        /// <summary>
        /// Gets or sets the item rights factory.
        /// </summary>
        /// <value>The item rights factory.</value>
        public static ItemRightsFactory ItemRightsFactory { get; set; }
    }

    /// <summary>
    /// Class ItemRightsFactory.
    /// </summary>
    public abstract class ItemRightsFactory
    {
        /// <summary>
        /// Gets the new item rights.
        /// </summary>
        /// <returns>ItemRights.</returns>
        public abstract ItemRights GetNewItemRights();
    }
}

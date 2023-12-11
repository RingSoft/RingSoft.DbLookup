// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 03-05-2023
// ***********************************************************************
// <copyright file="IDbLookupUserInterface.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Implement this to so DbLookup classes can interact with the user interface.
    /// </summary>
    public interface IDbLookupUserInterface
    {
        /// <summary>
        /// Shows the data process execution result.
        /// </summary>
        /// <param name="dataProcessResult">The data process result.</param>
        void ShowDataProcessResult(DataProcessResult dataProcessResult);

        /// <summary>
        /// Shows the add on the fly window.
        /// </summary>
        /// <param name="e">The e.</param>
        void ShowAddOnTheFlyWindow(LookupAddViewArgs e);

        /// <summary>
        /// Plays the system sound.
        /// </summary>
        /// <param name="icon">The icon.</param>
        void PlaySystemSound(RsMessageBoxIcons icon);

        /// <summary>
        /// Gets the owner window.
        /// </summary>
        /// <returns>System.Object.</returns>
        object GetOwnerWindow();

        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hostId">The host identifier.</param>
        /// <returns>System.String.</returns>
        string FormatValue(string value, int hostId);
    }

    /// <summary>
    /// Class DefaultUserInterface.
    /// Implements the <see cref="RingSoft.DbLookup.IDbLookupUserInterface" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.IDbLookupUserInterface" />
    internal class DefaultUserInterface : IDbLookupUserInterface
    {
        /// <summary>
        /// Shows the data process execution result.
        /// </summary>
        /// <param name="dataProcessResult">The data process result.</param>
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Console.WriteLine(dataProcessResult.Message);
        }

        /// <summary>
        /// Shows the add on the fly window.
        /// </summary>
        /// <param name="e">The e.</param>
        public void ShowAddOnTheFlyWindow(LookupAddViewArgs e)
        {
            
        }

        /// <summary>
        /// Plays the system sound.
        /// </summary>
        /// <param name="icon">The icon.</param>
        public void PlaySystemSound(RsMessageBoxIcons icon)
        {
            
        }

        /// <summary>
        /// Gets the owner window.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetOwnerWindow()
        {
            return null;
        }

        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hostId">The host identifier.</param>
        /// <returns>System.String.</returns>
        public string FormatValue(string value, int hostId)
        {
            return value;
        }
    }
}

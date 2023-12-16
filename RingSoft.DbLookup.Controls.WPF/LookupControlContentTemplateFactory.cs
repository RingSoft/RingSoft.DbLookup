// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="LookupControlContentTemplateFactory.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupControlContentTemplateFactory.
    /// </summary>
    public class LookupControlContentTemplateFactory
    {
        public LookupControlContentTemplateFactory()
        {
            LookupControlsGlobals.LookupControlContentTemplateFactory = this;
        }
        /// <summary>
        /// Gets the content template.
        /// </summary>
        /// <param name="contentTemplateId">The content template identifier.</param>
        /// <returns>DataEntryCustomContentTemplate.</returns>
        public virtual DataEntryCustomContentTemplate GetContentTemplate(int contentTemplateId)
        {
            return null;
        }

        /// <summary>
        /// Gets the image for alert level.
        /// </summary>
        /// <param name="alertLevel">The alert level.</param>
        /// <returns>Image.</returns>
        public virtual Image GetImageForAlertLevel(AlertLevels alertLevel)
        {
            return new Image{Source = Application.Current.MainWindow.Icon};
        }
    }
}

// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AdvancedFindRefreshRateWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindRefreshRateWindow.
    /// Implements the <see cref="BaseWindow" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <font color="red">Badly formed XML comment.</font>
    public class AdvancedFindRefreshRateWindow : BaseWindow
    {
        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; set; }

        /// <summary>
        /// Gets or sets the yellow alert image.
        /// </summary>
        /// <value>The yellow alert image.</value>
        public Image YellowAlertImage { get; set; }

        /// <summary>
        /// Gets or sets the red alert image.
        /// </summary>
        /// <value>The red alert image.</value>
        public Image RedAlertImage { get; set; }

        /// <summary>
        /// Gets or sets the ok button.
        /// </summary>
        /// <value>The ok button.</value>
        public Button OkButton { get; set; }

        /// <summary>
        /// Gets or sets the cancel button.
        /// </summary>
        /// <value>The cancel button.</value>
        public Button CancelButton { get; set; }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public AdvancedFindRefreshViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="AdvancedFindRefreshRateWindow" /> class.
        /// </summary>
        static AdvancedFindRefreshRateWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindRefreshRateWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindRefreshRateWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindRefreshRateWindow" /> class.
        /// </summary>
        /// <param name="advancedFind">The advanced find.</param>
        public AdvancedFindRefreshRateWindow(DbLookup.AdvancedFind.AdvancedFind advancedFind)
        {
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(advancedFind);
                var yellowAlertImage =
                    LookupControlsGlobals.LookupControlContentTemplateFactory.GetImageForAlertLevel(AlertLevels.Yellow);

                var redAlertImage =
                    LookupControlsGlobals.LookupControlContentTemplateFactory.GetImageForAlertLevel(AlertLevels.Red);

                YellowAlertImage.Source = yellowAlertImage.Source;
                RedAlertImage.Source = redAlertImage.Source;

                OkButton.Click += (o, eventArgs) =>
                {
                    ViewModel.RefreshProperties();
                    DialogResult = true;
                    Close();
                };

                CancelButton.Click += (o, eventArgs) => Close();
            };
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            YellowAlertImage = GetTemplateChild(nameof(YellowAlertImage)) as Image;
            RedAlertImage = GetTemplateChild(nameof(RedAlertImage)) as Image;
            OkButton = GetTemplateChild(nameof(OkButton)) as Button;
            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;

            ViewModel = Border.TryFindResource("AdvancedFindRefreshViewModel") as AdvancedFindRefreshViewModel;

            base.OnApplyTemplate();
        }
    }
}

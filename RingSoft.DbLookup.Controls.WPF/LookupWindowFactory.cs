// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-07-2023
// ***********************************************************************
// <copyright file="LookupWindowFactory.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Hardcodet.Wpf.TaskbarNotification;
using RingSoft.DbLookup.Lookup;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Constructs a lookup window.
    /// </summary>
    public class LookupWindowFactory
    {
        /// <summary>
        /// The taskbar icon
        /// </summary>
        private TaskbarIcon _taskbarIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupWindowFactory"/> class.
        /// </summary>
        public LookupWindowFactory()
        {
            LookupControlsGlobals.LookupWindowFactory = this;
            _taskbarIcon = new TaskbarIcon();
        }
        /// <summary>
        /// Creates the lookup window.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">if set to <c>true</c> [allow add].</param>
        /// <param name="allowView">if set to <c>true</c> [allow view].</param>
        /// <param name="initialSearchFor">The initial search for.</param>
        /// <param name="initialPrimaryKeyValue">The initial primary key value.</param>
        /// <param name="autoFillControl">The automatic fill control.</param>
        /// <param name="readOnlyPrimaryKeyValue">The read only primary key value.</param>
        /// <returns>LookupWindow.</returns>
        public virtual LookupWindow CreateLookupWindow(
            LookupDefinitionBase lookupDefinition
            , bool allowAdd
            , bool allowView
            , string initialSearchFor
            , PrimaryKeyValue initialPrimaryKeyValue = null
            , IAutoFillControl autoFillControl = null
            , PrimaryKeyValue readOnlyPrimaryKeyValue = null)
        {
            return new LookupWindow(lookupDefinition
                    , allowAdd
                    , allowView
                    , initialSearchFor
                    , autoFillControl
                    , readOnlyPrimaryKeyValue)
                {InitialSearchForPrimaryKeyValue = initialPrimaryKeyValue};
        }

        /// <summary>
        /// Sets the alert level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="disabled">if set to <c>true</c> [disabled].</param>
        /// <param name="window">The window.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">level - null</exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void SetAlertLevel(AlertLevels level, bool disabled, Window window, string message = "")
        {
            //var advancedFindWindows = Dispatcher.Invoke(() => Application.Current.Windows.OfType<AdvancedFindWindow>().ToList());
            var image = LookupControlsGlobals.LookupControlContentTemplateFactory
                .GetImageForAlertLevel(level);
            var title = string.Empty;
            var baloonIcon = BalloonIcon.Info;
            switch (level)
            {
                case AlertLevels.Green:
                    break;
                case AlertLevels.Yellow:
                    title = "Warning!";
                    break;
                case AlertLevels.Red:
                    title = "Red Alert!";
                    baloonIcon = BalloonIcon.Error;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            if (image == null)
            {
                var errorMessage = "No icon found for alert level: ";
                switch (level)
                {
                    case AlertLevels.Green:
                        errorMessage += "Green";
                        break;
                    case AlertLevels.Yellow:
                        errorMessage += "Yellow";
                        break;
                    case AlertLevels.Red:
                        errorMessage += "Red";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }

                throw new ApplicationException(errorMessage);

            }
            window.Dispatcher.Invoke(() => window.Icon = image.Source);


            //if (advancedFindWindows.Count >= 2)
            //{
            //    if (SystemGlobals.WindowAlertLevel < level)
            //    {
            //        SystemGlobals.WindowAlertLevel = level;
            //        Dispatcher.Invoke(() =>
            //        {
            //            ShowLevelIcon(level, message, image, title, baloonIcon);
            //            return;
            //        });
            //    }

            //}
            //else
            //{
            SystemGlobals.WindowAlertLevel = level;
            window.Dispatcher.Invoke(() => { ShowLevelIcon(level, message, image, title, baloonIcon, disabled); });

            //}

        }
        /// <summary>
        /// Shows the level icon.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="image">The image.</param>
        /// <param name="title">The title.</param>
        /// <param name="baloonIcon">The baloon icon.</param>
        /// <param name="disabled">if set to <c>true</c> [disabled].</param>
        private void ShowLevelIcon(AlertLevels level, string message, Image image, string title, BalloonIcon baloonIcon, bool disabled)
        {
            if (level == AlertLevels.Green)
            {
                _taskbarIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (image.Source != null)
                {
                    _taskbarIcon.ToolTipText = message;
                    _taskbarIcon.IconSource = image.Source;
                    _taskbarIcon.Visibility = Visibility.Visible;

                    if (!disabled)
                    {
                        _taskbarIcon.ShowBalloonTip(title, message, baloonIcon);
                        //System.Threading.Thread.Sleep(5000);
                        //_taskbarIcon.HideBalloonTip();
                    }

                    return;

                    //return Application.Current.MainWindow.Icon = image.Source;
                }

                return;
            }
        }

    }
}

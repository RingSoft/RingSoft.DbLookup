// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-07-2023
// ***********************************************************************
// <copyright file="LookupRefresher.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Timers;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Class RefreshAlertLevelArgs.
    /// </summary>
    public class RefreshAlertLevelArgs
    {
        /// <summary>
        /// Gets or sets the alert level.
        /// </summary>
        /// <value>The alert level.</value>
        public AlertLevels AlertLevel { get; set; }
    }
    /// <summary>
    /// Class LookupRefresher.
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class LookupRefresher : IDisposable
    {
        /// <summary>
        /// Gets or sets the refresh rate.
        /// </summary>
        /// <value>The refresh rate.</value>
        public RefreshRate RefreshRate { get; set; } = RefreshRate.None;
        /// <summary>
        /// Gets or sets the refresh value.
        /// </summary>
        /// <value>The refresh value.</value>
        public int RefreshValue { get; set; }
        /// <summary>
        /// Gets or sets the refresh condition.
        /// </summary>
        /// <value>The refresh condition.</value>
        public Conditions  RefreshCondition { get; set; }
        /// <summary>
        /// Gets or sets the yellow alert.
        /// </summary>
        /// <value>The yellow alert.</value>
        public int YellowAlert { get; set; }
        /// <summary>
        /// Gets or sets the red alert.
        /// </summary>
        /// <value>The red alert.</value>
        public int RedAlert { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LookupRefresher"/> is disabled.
        /// </summary>
        /// <value><c>true</c> if disabled; otherwise, <c>false</c>.</value>
        public bool Disabled { get; set; }

        /// <summary>
        /// Occurs when [refresh record count event].
        /// </summary>
        public event EventHandler RefreshRecordCountEvent;
        /// <summary>
        /// Occurs when [set alert level event].
        /// </summary>
        public event EventHandler<RefreshAlertLevelArgs> SetAlertLevelEvent;

        /// <summary>
        /// The timer
        /// </summary>
        private System.Timers.Timer _timer;
        /// <summary>
        /// The interval
        /// </summary>
        private int _interval = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupRefresher"/> class.
        /// </summary>
        public LookupRefresher()
        {
        }

        /// <summary>
        /// Starts the refresh.
        /// </summary>
        public void StartRefresh()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }

        /// <summary>
        /// Loads from adv find.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void LoadFromAdvFind(AdvancedFind.AdvancedFind entity)
        {
            if (entity.RefreshRate != null) RefreshRate = (RefreshRate)entity.RefreshRate.Value;
            if (entity.RefreshValue != null) RefreshValue = entity.RefreshValue.Value;
            if (entity.RefreshCondition != null) RefreshCondition = (Conditions)entity.RefreshCondition;
            if (entity.YellowAlert != null) YellowAlert = (int)entity.YellowAlert;
            if (entity.RedAlert != null) RedAlert = (int)entity.RedAlert;
            if (entity.Disabled != null) Disabled = entity.Disabled.Value;
        }

        /// <summary>
        /// Resets the count.
        /// </summary>
        private void ResetCount() => RefreshRecordCountEvent?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Sets the alert level.
        /// </summary>
        /// <param name="alertLevel">The alert level.</param>
        private void SetAlertLevel(AlertLevels alertLevel) =>
            SetAlertLevelEvent.Invoke(this, new RefreshAlertLevelArgs{AlertLevel = alertLevel});

        /// <summary>
        /// Handles the Elapsed event of the _timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _interval++;
            switch (RefreshRate)
            {
                case RefreshRate.Hours:
                    if (_interval == (RefreshValue * 60) * 60)
                    {
                        _interval = 0;
                        ResetCount();
                    }
                    break;
                case RefreshRate.Minutes:
                    if (_interval == RefreshValue * 60)
                    {
                        _interval = 0;
                        ResetCount();
                    }
                    break;
                //case RefreshRate.Seconds:
                //    if (_interval == RefreshValue)
                //    {
                //        _interval = 0;
                //        ResetCount();
                //    }
                //    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void ResetTimer()
        {
            _interval = 0;
        }

        /// <summary>
        /// Updates the record count.
        /// </summary>
        /// <param name="recordCount">The record count.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void UpdateRecordCount(int recordCount)
        {
            if (RefreshRate == RefreshRate.None)
            {
                SetAlertLevel(AlertLevels.Green);
                return;
            }
            var level = AlertLevels.Green;
            switch (RefreshCondition)
            {
                case Conditions.Equals:
                    if (recordCount == YellowAlert)
                    {
                        SetAlertLevel(AlertLevels.Yellow);
                    }
                    if (recordCount == RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Red);
                    }
                    if (recordCount != YellowAlert && recordCount != RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Green);
                    }
                    break;
                case Conditions.NotEquals:
                    if (recordCount != YellowAlert)
                    {
                        SetAlertLevel(AlertLevels.Yellow);
                    }
                    else if (recordCount != RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Red);
                    }
                    if (recordCount == YellowAlert && recordCount == RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Green);
                    }
                    break;
                case Conditions.GreaterThan:
                    if (recordCount > RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Red);
                    }
                    else if (recordCount > YellowAlert)
                    {
                        SetAlertLevel(AlertLevels.Yellow);
                    }
                    if (recordCount < YellowAlert && recordCount < RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Green);
                    }
                    break;
                case Conditions.GreaterThanEquals:
                    if (recordCount >= YellowAlert)
                    {
                        SetAlertLevel(AlertLevels.Yellow);
                    }
                    if (recordCount >= RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Red);
                    }
                    if (recordCount <= YellowAlert && recordCount <= RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Green);
                    }
                    break;
                case Conditions.LessThan:
                    if (recordCount < YellowAlert)
                    {
                        SetAlertLevel(AlertLevels.Yellow);
                    }
                    else if (recordCount < RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Red);
                    }
                    if (recordCount > YellowAlert && recordCount > RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Green);
                    }
                    break;
                case Conditions.LessThanEquals:
                    if (recordCount <= YellowAlert)
                    {
                        SetAlertLevel(AlertLevels.Yellow);
                    }
                    else if (recordCount <= RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Red);
                    }
                    if (recordCount >= YellowAlert && recordCount >= RedAlert)
                    {
                        SetAlertLevel(AlertLevels.Green);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        /// <summary>
        /// Gets the record count message.
        /// </summary>
        /// <param name="recordCount">The record count.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        public string GetRecordCountMessage(int recordCount, string name)
        {
            var formattedCount = GblMethods.FormatValue(FieldDataTypes.Integer,
                recordCount.ToString(), GblMethods.GetNumFormat(0, false));
            var message =
                $"There are {formattedCount} records in the {name} Advanced Find.";
            return message;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Enabled = false;
                _timer.Dispose();
            }
        }
    }
}

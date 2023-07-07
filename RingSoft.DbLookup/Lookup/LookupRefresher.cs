using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Timers;

namespace RingSoft.DbLookup.Lookup
{
    public class RefreshAlertLevelArgs
    {
        public AlertLevels AlertLevel { get; set; }
    }
    public class LookupRefresher : IDisposable
    {
        public RefreshRate RefreshRate { get; set; } = RefreshRate.None;
        public int RefreshValue { get; set; }
        public Conditions  RefreshCondition { get; set; }
        public int YellowAlert { get; set; }
        public int RedAlert { get; set; }
        public bool Disabled { get; set; }

        public event EventHandler RefreshRecordCountEvent;
        public event EventHandler<RefreshAlertLevelArgs> SetAlertLevelEvent;

        private System.Timers.Timer _timer;
        private int _interval = 0;

        public LookupRefresher()
        {
        }

        public void StartRefresh()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }

        public void LoadFromAdvFind(AdvancedFind.AdvancedFind entity)
        {
            if (entity.RefreshRate != null) RefreshRate = (RefreshRate)entity.RefreshRate.Value;
            if (entity.RefreshValue != null) RefreshValue = entity.RefreshValue.Value;
            if (entity.RefreshCondition != null) RefreshCondition = (Conditions)entity.RefreshCondition;
            if (entity.YellowAlert != null) YellowAlert = (int)entity.YellowAlert;
            if (entity.RedAlert != null) RedAlert = (int)entity.RedAlert;
            if (entity.Disabled != null) Disabled = entity.Disabled.Value;
        }

        private void ResetCount() => RefreshRecordCountEvent?.Invoke(this, EventArgs.Empty);

        private void SetAlertLevel(AlertLevels alertLevel) =>
            SetAlertLevelEvent.Invoke(this, new RefreshAlertLevelArgs{AlertLevel = alertLevel});

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

        public void ResetTimer()
        {
            _interval = 0;
        }

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

        public string GetRecordCountMessage(int recordCount, string name)
        {
            var formattedCount = GblMethods.FormatValue(FieldDataTypes.Integer,
                recordCount.ToString(), GblMethods.GetNumFormat(0, false));
            var message =
                $"There are {formattedCount} records in the {name} Advanced Find.";
            return message;
        }

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

using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.ViewModels
{
    public interface ISqlScriptView
    {
        string ShowSaveFileDialog(string initialDirectory, string fileName, string defaultExt, string filter);

        void CloseWindow();
    }

    public class SqlScriptViewModel : INotifyPropertyChanged
    {
        public string SqlText
        {
            get => _sqlText;
            set
            {
                if (_sqlText == value)
                    return;

                _sqlText = value;
                OnPropertyChanged(nameof(SqlText));
            }
        }

        private string _fileName;
        private DbDataProcessor _dataProcessor;
        private string _sqlText;
        private bool _splitGo;
        private ISqlScriptView _view;
        private string _defaultDbName;
        private string _dbName;

        public void OnViewLoaded(ISqlScriptView view, DbDataProcessor dataProcessor, string fileName, string sqlText,
            bool splitGo, string defaultDbName, string dbName)
        {
            _view = view;
            _dataProcessor = dataProcessor;
            _fileName = fileName;
            _splitGo = splitGo;
            _defaultDbName = defaultDbName;
            _dbName = dbName;

            SqlText = ScrubSqlText(sqlText);
        }

        private string ScrubSqlText(string sqlText)
        {
            if (_dbName == _defaultDbName)
                return sqlText;

            var result = sqlText;

            if (_defaultDbName.Any(char.IsUpper))
                result = result.Replace(_defaultDbName.ToLower(), _dbName.ToLower());

            result = result.Replace(_defaultDbName, _dbName);

            return result;
        }

        public void SaveAs()
        {
            var fileName = Path.GetFileName(_fileName);
            var initialDirectory = Path.GetDirectoryName(_fileName);
            var defaultExt = ".sql";
            var filter = @"SQL Scripts (*.sql)| *.sql";

            fileName = _view.ShowSaveFileDialog(initialDirectory, fileName, defaultExt, filter);
            if (!fileName.IsNullOrEmpty())
            {
                File.WriteAllText(fileName, _sqlText);
            }
        }

        public void ExecuteButton_Click()
        {
            DbDataProcessor.ShowSqlStatementWindow();
            if (_splitGo)
            {
                var sqls = RsDbLookupAppGlobals.SplitSqlServerStatements(SqlText);
                _dataProcessor.ExecuteSqls(sqls, true);
            }
            else
            {
                _dataProcessor.ExecuteSql(SqlText, true);
            }

            DbDataProcessor.ShowSqlStatementWindow(false);
            _view.CloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

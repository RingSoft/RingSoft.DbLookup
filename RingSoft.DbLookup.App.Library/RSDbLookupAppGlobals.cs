using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace RingSoft.DbLookup.App.Library
{
    public enum GlobalsProgressStatus
    {
        Northwind,
        MegaDb
    }

    public class AppStartProgressArgs
    {
        public string ProgressText { get; set; }

        public AppStartProgressArgs()
        {
        }
    }
    public class RsDbLookupAppGlobals
    {
        public static IEfProcessor EfProcessor { get; set; }

        public static string SqlServerNorthwindScript =>
            $"{AssemblyDirectory}\\Northwind\\Northwind_SqlServerScript.sql";
        public static string MySqlNorthwindScript =>
            $"{AssemblyDirectory}\\Northwind\\Northwind_MySqlScript.sql";
        public static string SqlServerMegaDbScript =>
            $"{AssemblyDirectory}\\MegaDb\\MegaDb_SqlServerScript.sql";
        public static string MySqlMegaDbScript =>
            $"{AssemblyDirectory}\\MegaDb\\MegaDb_MySqlScript.sql";

        public static event EventHandler<AppStartProgressArgs> AppStartProgress;

        public const string AppRegKeyName = "RSDbLookupApp";
        public static void SaveSetting(string valueName, string value)
        {
            SaveSetting(AppRegKeyName, valueName, value);
        }
        public static void SaveSetting(string appName, string valueName, string value)
        {
            var keyName = GetRegistryKeyName(appName);

            Registry.SetValue(keyName, valueName, value);
        }
        public static string GetSetting(string valueName)
        {
            return GetSetting(valueName, "");
        }
        public static string GetSetting(string valueName, string defaultValue)
        {
            return GetSetting(AppRegKeyName, valueName, defaultValue);
        }
        public static string GetSetting(string appName, string valueName, string defaultValue)
        {
            var keyName = GetRegistryKeyName(appName);

            var result = (string)Registry.GetValue(keyName, valueName, defaultValue);
            if (string.IsNullOrEmpty(result))
                result = defaultValue;

            return result;
        }
        public static string GetRegistryKeyName(string appName)
        {
            return $"HKEY_CURRENT_USER\\{appName}";
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string OpenTextFile(string fileName)
        {
            var openFile = new System.IO.StreamReader(fileName);
            return openFile.ReadToEnd();
        }

        public static List<string> SplitSqlServerStatements(string sqlScript)
        {
            // Split by "GO" statements
            var statements = Regex.Split(
                sqlScript,
                @"^[\t\r\n]*GO[\t\r\n]*\d*[\t\r\n]*(?:--.*)?$",
                RegexOptions.Multiline |
                RegexOptions.IgnorePatternWhitespace |
                RegexOptions.IgnoreCase);

            // Remove empties, trim, and return
            return statements
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim(' ', '\r', '\n')).ToList();
        }

        public static void UpdateGlobalsProgressStatus(GlobalsProgressStatus status)
        {
            var appStartProgress = new AppStartProgressArgs();
            switch (status)
            {
                case GlobalsProgressStatus.Northwind:
                    appStartProgress.ProgressText = "Initializing Northwind Entity Framework Structure.";
                    break;
                case GlobalsProgressStatus.MegaDb:
                    appStartProgress.ProgressText = "Initializing Mega Database Entity Framework Structure.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            AppStartProgress?.Invoke(null, appStartProgress);
        }
    }
}

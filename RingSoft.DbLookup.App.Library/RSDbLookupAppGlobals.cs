﻿using RingSoft.DbLookup.GetDataProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

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

    }

    public class RsDbLookupAppGlobals
    {
        public static IEfProcessor EfProcessor { get; set; }

        public static string RegistryFileName { get; private set; }

        public static string SqlServerNorthwindScript =>
            $"{AssemblyDirectory}\\Northwind\\Northwind_SqlServerScript.sql";
        public static string MySqlNorthwindScript =>
            $"{AssemblyDirectory}\\Northwind\\Northwind_MySqlScript.sql";
        public static string SqlServerMegaDbScript =>
            $"{AssemblyDirectory}\\MegaDb\\MegaDb_SqlServerScript.sql";
        public static string MySqlMegaDbScript =>
            $"{AssemblyDirectory}\\MegaDb\\MegaDb_MySqlScript.sql";


        public static event EventHandler<AppStartProgressArgs> AppStartProgress;

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

        public static string AppDataDirectory
        {
            get
            {
#if DEBUG
                return AssemblyDirectory;
#else
                return $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoftDbLookupApp";
#endif
            }
        }

        public static void Initialize()
        {
            var registryElementName = "RegistryFileName";
            var appSettingsFile = $"{AppDataDirectory}\\AppSettings.xml";
            var xmlProcessor = new XmlProcessor("AppSettings");
            if (File.Exists(appSettingsFile))
            {
                var xml = OpenTextFile(appSettingsFile);
                xmlProcessor.LoadFromXml(xml);
            }
            else
            {
                xmlProcessor.SetElementValue(registryElementName, $"{AppDataDirectory}\\Registry.xml");
                var xml = xmlProcessor.OutputXml();
                WriteTextFile(appSettingsFile, xml);
            }

            RegistryFileName = xmlProcessor.GetElementValue(registryElementName, string.Empty);
            RegistrySettings.LoadFromRegistryFile();
        }

        public static string OpenTextFile(string fileName)
        {
            var result = string.Empty;
            try
            {
                var openFile = new StreamReader(fileName);
                result = openFile.ReadToEnd();
            }
            catch (Exception e)
            {
                DbDataProcessor.DisplayDataException(e, "Opening text file.");
            }

            return result;
        }

        public static void WriteTextFile(string fileName, string text)
        {
            try
            {
                var directory = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(directory))
                    if (directory != null)
                        Directory.CreateDirectory(directory);

                File.WriteAllText(fileName, text);
            }
            catch (Exception e)
            {
                DbDataProcessor.DisplayDataException(e, "Writing text file.");
            }
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

        public static string EncryptString(string text)
        {
            if (text.IsNullOrEmpty())
                return text;

            var crypto = new RijndaelSimpleTest.Crypto();
            return crypto.Encrypt(text);
        }

        public static string DecryptString(string encrypted)
        {
            if (encrypted.IsNullOrEmpty())
                return encrypted;

            var crypto = new RijndaelSimpleTest.Crypto();
            return crypto.Decrypt(encrypted);
        }
    }
}

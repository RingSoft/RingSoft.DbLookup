using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.Northwind;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace RingSoft.DbLookup.App.Library
{
    public enum GlobalsProgressStatus
    {
        InitNorthwind,
        InitMegaDb,
        ConnectingToNorthwind,
        ConnectingToMegaDb
    }

    public enum ItemIcons
    {
        Red = RsDbLookupAppGlobals.ItemIconRedId,
        Green = RsDbLookupAppGlobals.ItemIconGreenId,
        Blue = RsDbLookupAppGlobals.ItemIconBlueId,
        Yellow = RsDbLookupAppGlobals.ItemIconYellowId
    }

    public class AppStartProgressArgs
    {
        public string ProgressText { get; set; }

    }

    public class RsDbLookupAppGlobals
    {
        public const int IconTypeTemplateId = 100;

        public const int ItemIconRedId = 0;
        public const int ItemIconGreenId = 1;
        public const int ItemIconBlueId = 2;
        public const int ItemIconYellowId = 3;

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
                return $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\DbLookupDemoApp\\{AppSection}";
#endif
            }
        }

        public static string AppSection { get; set; }

        public static bool FirstTime { get; set; }

        public static bool UnitTest { get; set; }

        public static void Initialize(string appSection)
        {
            AppSection = appSection;
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
                FirstTime = true;
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
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error Opening Text File", RsMessageBoxIcons.Error);
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
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error Writing Text File", RsMessageBoxIcons.Error);
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
                case GlobalsProgressStatus.InitNorthwind:
                    appStartProgress.ProgressText = "Initializing Northwind Entity Framework Structure.";
                    break;
                case GlobalsProgressStatus.InitMegaDb:
                    appStartProgress.ProgressText = "Initializing Mega Database Entity Framework Structure.";
                    break;
                case GlobalsProgressStatus.ConnectingToNorthwind:
                    appStartProgress.ProgressText = "Connecting to the Northwind Database.";
                    break;
                case GlobalsProgressStatus.ConnectingToMegaDb:
                    appStartProgress.ProgressText = "Connecting to the Mega Database.";
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

            var crypto = new Crypto();
            return crypto.Encrypt(text);
        }

        public static string DecryptString(string encrypted)
        {
            if (encrypted.IsNullOrEmpty())
                return encrypted;

            var crypto = new Crypto();
            return crypto.Decrypt(encrypted);
        }

        public static void ConnectToNorthwind(INorthwindEfDataProcessor processor,
            INorthwindLookupContext lookupContext)
        {
            processor.SetDataContext();
            try
            {
                var context = SystemGlobals.DataRepository.GetDataContext();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void ConnectToMegaDb(IMegaDbEfDataProcessor processor, IMegaDbLookupContext lookupContext)
        {
            try
            {
                processor.GetItem(1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

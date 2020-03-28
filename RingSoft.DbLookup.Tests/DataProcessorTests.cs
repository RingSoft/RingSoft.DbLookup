using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix;
using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.Tests
{
    [TestClass]
    public class DataProcessorTests
    {
        private static SqlServerDataProcessor _sqlServerDataProcessor;
        private static MySqlDataProcessor _mySqlDataProcessor;
        private static SqliteDataProcessor _sqliteDataProcessor;

        private static SelectQuery _complexQuery;
        private static SelectQuery _multiLevelJoinAliasesQuery;
        private static SelectQuery _enumQuery;
        private static SelectQuery _caseSensitiveQuery;
        private static SelectQuery _distinctQuery;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            SetupDataProcessors();

            SetupComplexQuery();

            SetupMultiLevelJoinAliasesQuery();

            SetupEnumQuery();

            SetupCaseSensitiveQuery();

            SetupDistinctQuery();
        }

        //[TestInitialize]
        //public void Setup()
        //{
        //    _dataProcessor = new MsAccessDataProcessor
        //    {
        //        FilePath = "C:\\Users\\petem\\OneDrive\\Ring2Data",
        //        FileName = "DevLogix.mdb"
        //    };
        //}

        private static void SetupDataProcessors()
        {
            DbDataProcessor.SqlErrorViewer = new TestGetDataErrorViewer();
            _sqlServerDataProcessor = new SqlServerDataProcessor()
            {
                Server = "localhost\\SQLEXPRESS",
                Database = "DevLogix"
            };

            _mySqlDataProcessor = new MySqlDataProcessor()
            {
                Server = "localhost",
                Database = "devlogix",
                UserName = "root",
                Password = "ring203301971"
            };

            _sqliteDataProcessor = new SqliteDataProcessor()
            {
                FilePath = "C:\\Users\\petem\\source\\repos\\RSDbLookup\\RSDbLookupApp.Library\\DevLogix",
                FileName = "DevLogix.sqlite"
            };
        }

        private static void SetupComplexQuery()
        {
            var baseQuery = new SelectQuery("TB_Errors");

            var statusTable = baseQuery.AddPrimaryJoinTable(JoinTypes.InnerJoin, "TB_ErrorStatus")
                .AddJoinField("intStatusID", "intStatusID");
            var foundVersionJoinTable = baseQuery.AddPrimaryJoinTable(JoinTypes.LeftOuterJoin, "TB_Versions",
                    "TB_Errors_TB_Versions_intFoundVersionID")
                .AddJoinField("intVersionID", "intFoundVersionID");
            var fixedVersionJoinTable = baseQuery.AddPrimaryJoinTable(JoinTypes.LeftOuterJoin, "TB_Versions",
                    "TB_Errors_TB_Versions_intFixedVersionID")
                .AddJoinField("intVersionID", "intFixedVersionID");

            baseQuery.SetMaxRecords(10) //Have to do this for SQL Server.  ORDER BY clauses not allowed
                                        //for sub queries unless TOP n is specified.
                .AddSelectColumn("intErrorID")
                .AddSelectColumn("strErrorNo")
                .AddSelectColumn("dteDate")
                .AddSelectColumn("intStatusID")
                .AddSelectColumn("strStatus", statusTable)
                .AddSelectColumn("intFoundVersionID")
                .AddSelectColumn("strVersion", foundVersionJoinTable, "strFoundVersion")
                .AddSelectColumn("dteCreated", foundVersionJoinTable, "dteFoundVersionCreated")
                .AddSelectColumn("strVersion", fixedVersionJoinTable, "strFixedVersion")
                .AddSelectColumn("decHrsSpent")
                .AddSelectColumn("txtDescription")
                .AddOrderBySegment("strErrorNo", OrderByTypes.Ascending)
                .AddOrderBySegment(statusTable, "strStatus", OrderByTypes.Ascending)
                .AddOrderBySegment(foundVersionJoinTable, "strVersion", OrderByTypes.Ascending);

            baseQuery.AddWhereItem(foundVersionJoinTable, "dteCreated", Conditions.GreaterThan, new DateTime(2000, 1, 1),
                    DbDateTypes.DateOnly)
                .SetEndLogic(EndLogics.And);
            baseQuery.AddWhereItem("dteDate", Conditions.LessThan, new DateTime(2019, 12, 31), DbDateTypes.DateTime);

            var firstWhereItem = baseQuery.WhereItems[0];
            var lastWhereItem = baseQuery.WhereItems[baseQuery.WhereItems.Count - 1];
            firstWhereItem.SetLeftParenthesesCount(firstWhereItem.LeftParenthesesCount + 2);
            lastWhereItem.SetRightParenthesesCount(lastWhereItem.RightParenthesesCount + 2)
                .SetEndLogic(EndLogics.Or);

            baseQuery.AddWhereItem("txtDescription", Conditions.Contains, "a'", true)
                .SetLeftParenthesesCount(2);
            baseQuery.AddWhereItem("decHrsSpent", Conditions.GreaterThanEquals, 0.01);
            baseQuery.AddWhereItem("txtDescription", Conditions.NotEqualsNull, "", true);
            baseQuery.AddWhereItem("decHrsSpent", Conditions.LessThanEquals, 99.32)
                .SetRightParenthesesCount(2);

            var level2Query = new SelectQuery("Level2", baseQuery)
                .AddSelectColumn("strErrorNo")
                .AddSelectColumn("dteDate")
                .AddSelectColumn("intStatusID")
                .AddSelectColumn("strStatus")
                .AddSelectColumn("intFoundVersionID")
                .AddSelectColumn("strFoundVersion")
                .AddSelectColumn("dteFoundVersionCreated")
                .AddSelectColumn("strFixedVersion")
                .AddSelectColumn("decHrsSpent")
                .AddSelectColumn("txtDescription");

            _complexQuery = new SelectQuery("Top", level2Query)
                .SetMaxRecords(10);
        }

        private static void SetupMultiLevelJoinAliasesQuery()
        {
            _multiLevelJoinAliasesQuery = new SelectQuery("TB_Issues");

            var taskTable = _multiLevelJoinAliasesQuery.AddPrimaryJoinTable(JoinTypes.InnerJoin, "TB_Tasks", "TB_Issues_TB_Tasks_intTaskID")
                .AddJoinField("intTaskID", "intTaskID");
            var projectTable = _multiLevelJoinAliasesQuery.AddPrimaryJoinTable(JoinTypes.InnerJoin, taskTable, "TB_Projects", "TB_Tasks_TB_Projects_intProjectID")
                .AddJoinField("intProjectID", "intProjectID");

            _multiLevelJoinAliasesQuery.AddSelectColumn("intIssueID")
                .AddSelectColumn("strIssueDesc")
                .AddSelectColumn("strTaskDesc", taskTable, "Task")
                .AddSelectColumn("strProject", projectTable, "Project");

            _multiLevelJoinAliasesQuery.AddWhereItem("bolResolved", Conditions.Equals, true).SetEndLogic(EndLogics.Or);
            _multiLevelJoinAliasesQuery.AddWhereItem("bolResolved", Conditions.Equals, false);
        }

        private static void SetupEnumQuery()
        {
            var baseEnumQuery = new SelectQuery("TB_Users")
                .SetMaxRecords(10)
                .AddSelectColumn("intUserId")
                .AddSelectColumn("strUserName")
                .AddSelectEnumColumn<DeveloperTypes>("bytDeveloperType", "strDeveloperType")
                .AddOrderByEnumSegment<DeveloperTypes>("bytDeveloperType", OrderByTypes.Ascending)
                .AddOrderBySegment("strUserName", OrderByTypes.Ascending);

            baseEnumQuery.AddWhereItemEnum<DeveloperTypes>("bytDeveloperType", Conditions.GreaterThanEquals, "d")
                .SetLeftParenthesesCount(1);
            baseEnumQuery.AddWhereItemEnum<DeveloperTypes>("bytDeveloperType", Conditions.LessThan, "e")
                .SetRightParenthesesCount(1);

            _enumQuery = new SelectQuery("Top", baseEnumQuery);
        }

        private static void SetupCaseSensitiveQuery()
        {
            _caseSensitiveQuery = new SelectQuery("TB_Tasks")
                .AddSelectColumn("strTaskDesc")
                .AddOrderBySegment("strTaskDesc", OrderByTypes.Ascending, true);
            _caseSensitiveQuery.AddWhereItem("strTaskDesc", Conditions.GreaterThanEquals, "C")
                .IsCaseSensitive();
        }

        private static void SetupDistinctQuery()
        {
            _distinctQuery = new SelectQuery("StockMaster")
                .AddSelectColumn("StockNumber", "StockNumberAlias", true)
                .AddSelectColumn("StockNumber");
        }

        private const string ValidCaseSensitiveResult = "CPTRGrid";

        private static string GetCaseSensitiveTopRowValue(GetDataResult result)
        {
            return result.DataSet.Tables[0].Rows[0].GetRowValue("strTaskDesc");
        }

        //SQL Server-------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetData_SqlServerComplexSqlStatement_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _sqlServerDataProcessor.GetData(_complexQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            Assert.AreEqual(10, actualResult.DataSet.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void GetData_SqlServerMultiLevelJoinAliases_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _sqlServerDataProcessor.GetData(_multiLevelJoinAliasesQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);
        }

        [TestMethod]
        public void GetData_SqlServerEnumQuery_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _sqlServerDataProcessor.GetData(_enumQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            Assert.AreEqual(2, actualResult.DataSet.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void GetData_SqlServerFormulaQuery_ReturnsSuccess()
        {
            //Arrange
            var estHrs = "[TB_Tasks].[decEstHrs]";
            var hrsSpent = "[TB_Tasks].[decHrsSpent]";
            var formula = $"IsNull({estHrs}, 0)\r\n\t- IsNull({hrsSpent}, 0)";

            var formulaQuery = new SelectQuery("TB_Tasks")
                .AddSelectColumn("intTaskID")
                .AddSelectColumn("strTaskDesc")
                .AddSelectColumn("decEstHrs")
                .AddSelectColumn("decHrsSpent")
                .AddSelectFormulaColumn("decRemainingHours", formula)
                .AddOrderByFormulaSegment(formula, OrderByTypes.Ascending)
                .AddOrderBySegment("strTaskDesc", OrderByTypes.Ascending);

            formulaQuery.AddWhereItemFormula(formula, Conditions.GreaterThanEquals, 5)
                .SetLeftParenthesesCount(1);
            formulaQuery.AddWhereItemFormula(formula, Conditions.LessThanEquals, 50)
                .SetRightParenthesesCount(1);

            //Act
            var actualResult = _sqlServerDataProcessor.GetData(formulaQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);
        }

        [TestMethod]
        public void GetData_SqlServerCaseSensitiveQuery_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _sqlServerDataProcessor.GetData(_caseSensitiveQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            var currentCaseSensitiveResult = GetCaseSensitiveTopRowValue(actualResult);

            Assert.AreEqual(ValidCaseSensitiveResult, currentCaseSensitiveResult);
        }

        [TestMethod]
        public void GetData_SqlServerDistinctQuery_ReturnsSuccess()
        {
            var actualResult = _sqlServerDataProcessor.GetData(_distinctQuery);
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);
        }

        //MySQL-------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetData_MySqlComplexSqlStatement_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _mySqlDataProcessor.GetData(_complexQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            Assert.AreEqual(10, actualResult.DataSet.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void GetData_MySqlMultiLevelJoinAliases_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _mySqlDataProcessor.GetData(_multiLevelJoinAliasesQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);
        }

        [TestMethod]
        public void GetData_MySqlEnumQuery_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _mySqlDataProcessor.GetData(_enumQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            Assert.AreEqual(2, actualResult.DataSet.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void GetData_MySqlCaseSensitiveQuery_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _mySqlDataProcessor.GetData(_caseSensitiveQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            var currentCaseSensitiveResult = GetCaseSensitiveTopRowValue(actualResult);

            Assert.AreEqual(ValidCaseSensitiveResult, currentCaseSensitiveResult);
        }

        [TestMethod]
        public void GetData_MySqlDistinctQuery_ReturnsSuccess()
        {
            var actualResult = _mySqlDataProcessor.GetData(_distinctQuery);
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);
        }

        //Sqlite-------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetData_SqliteComplexSqlStatement_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _sqliteDataProcessor.GetData(_complexQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            Assert.AreEqual(10, actualResult.DataSet.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void GetData_SqliteMultiLevelJoinAliases_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _sqliteDataProcessor.GetData(_multiLevelJoinAliasesQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);
        }

        [TestMethod]
        public void GetData_SqliteEnumQuery_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _sqliteDataProcessor.GetData(_enumQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            Assert.AreEqual(2, actualResult.DataSet.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void GetData_SqliteCaseSensitiveQuery_ReturnsSuccess()
        {
            //Arrange

            //Act
            var actualResult = _sqliteDataProcessor.GetData(_caseSensitiveQuery);

            //Assert
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);

            var currentCaseSensitiveResult = GetCaseSensitiveTopRowValue(actualResult);

            Assert.AreEqual(ValidCaseSensitiveResult, currentCaseSensitiveResult);
        }

        [TestMethod]
        public void GetData_SqlliteDistinctQuery_ReturnsSuccess()
        {
            var actualResult = _sqliteDataProcessor.GetData(_distinctQuery);
            Assert.AreEqual(GetDataResultCodes.Success, actualResult.ResultCode);
        }

        //Miscellaneous-------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetData_AllDbsBeginsWith_ReturnsCorrectValue()
        {
            var context = new DevLogixLookupContextEfCore();
            var query = new SelectQuery(context.Issues.TableName).SetMaxRecords(1)
                .AddSelectColumn(context.Issues.GetFieldDefinition(p => p.Id).FieldName)
                .AddSelectColumn(context.Issues.GetFieldDefinition(p => p.Description).FieldName)
                .AddOrderBySegment(context.Issues.GetFieldDefinition(p => p.Description).FieldName,
                    OrderByTypes.Ascending);

            query.AddWhereItem(context.Issues.GetFieldDefinition(p => p.Description).FieldName,
                Conditions.BeginsWith, "ch");

            context.DataProcessorType = DataProcessorTypes.Sqlite;
            var result = context.DataProcessor.GetData(query);
            CheckResult(result, context);

            context.DataProcessorType = DataProcessorTypes.SqlServer;
            result = context.DataProcessor.GetData(query);
            CheckResult(result, context);

            context.DataProcessorType = DataProcessorTypes.MySql;
            result = context.DataProcessor.GetData(query);
            CheckResult(result, context);
        }

        private static void CheckResult(GetDataResult result, DevLogixLookupContextEfCore context)
        {
            if (result.ResultCode == GetDataResultCodes.Success)
            {
                var resultValue = result.DataSet.Tables[0].Rows[0]
                    .GetRowValue(context.Issues.GetFieldDefinition(p => p.Description).FieldName);
                Assert.AreEqual("Change PunchTo", resultValue);
            }
        }
    }
}

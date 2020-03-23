-- Script Date: 12/25/2019 12:45 PM  - ErikEJ.SqlCeScripting version 3.5.2.72
SELECT 1;
PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE [VERSYS] (
  [strVersion] nvarchar(50) NOT NULL
);
CREATE TABLE [TB_Versions] (
  [intVersionID] int NOT NULL
, [strVersion] nvarchar(50) NOT NULL
, [dteCreated] datetime NOT NULL
, [intProductID] int NOT NULL
, [dteRelToQA] datetime NULL
, [dteRelToCust] datetime NULL
, [dteClosed] datetime NULL
, [txtNotes] ntext NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_UserSupervisors] (
  [intUserID] int NOT NULL
, [intSupID] int NOT NULL
);
CREATE TABLE [TB_Users] (
  [intUserID] int NOT NULL
, [strUserName] nvarchar(50) NOT NULL
, [strEmailAddress] nvarchar(100) NULL
, [txtNotes] ntext NULL
, [strInitials] nvarchar(3) NULL
, [bolSUP] bit NOT NULL
, [strPassword] nvarchar(250) NOT NULL
, [bytDeveloperType] tinyint NOT NULL
, [txtRights] ntext NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_UserGroups] (
  [intUserID] int NOT NULL
, [intGroupID] int NOT NULL
);
CREATE TABLE [TB_UserDaysOff] (
  [intUserID] int NOT NULL
, [dteDateOff] datetime NOT NULL
, [strDescription] nvarchar(50) NOT NULL
);
CREATE TABLE [TB_Timeclock] (
  [intTimeclockId] int NOT NULL
, [bytType] tinyint NOT NULL
, [intUserID] int NOT NULL
, [dtePunchIn] datetime NOT NULL
, [dtePunchOut] datetime NOT NULL
, [intTaskID] int NULL
, [intOutlineID] int NULL
, [intErrorID] int NULL
, [txtNotes] ntext NOT NULL
, [bolPunchChanged] bit NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_TestTemplDetails] (
  [intTemplateID] int NOT NULL
, [intDetailID] int NOT NULL
, [strText] nvarchar(100) NOT NULL
);
CREATE TABLE [TB_TestingTemplates] (
  [intTemplateID] int NOT NULL
, [strName] nvarchar(50) NOT NULL
, [txtNotes] ntext NULL
, [intBaseTemplateID] int NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_TestingOutlines] (
  [intOutlineID] int NOT NULL
, [strName] nvarchar(50) NOT NULL
, [intProductID] int NOT NULL
, [intCreatedByID] int NOT NULL
, [intAssignedToID] int NULL
, [dteDueDate] datetime NULL
, [decPercentComplete] float NOT NULL
, [txtNotes] ntext NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
, [decHrsSpent] float NULL
);
CREATE TABLE [TB_TaskStatus] (
  [intStatusID] int NOT NULL
, [strDescription] nvarchar(50) NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_Tasks] (
  [intTaskID] int NOT NULL
, [strTaskDesc] nvarchar(100) NOT NULL
, [dteDueDate] datetime NULL
, [dteCompletedDate] datetime NULL
, [decEstHrs] numeric(18,2) NOT NULL
, [decHrsSpent] numeric(18,2) NULL
, [txtNotes] ntext NULL
, [decOrigEst] numeric(18,2) NULL
, [intProjectID] int NULL
, [strCMSTaskID] nvarchar(50) NULL
, [intStatusID] int NULL
, [intPriorityID] int NULL
, [decPercentComplete] float NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
, [intPriorityNo] int NOT NULL
, [intAssignedToID] int NULL
);
CREATE TABLE [TB_TaskPriority] (
  [intPriorityID] int NOT NULL
, [strDescription] nvarchar(50) NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
, [intPriorityNo] int NOT NULL
);
CREATE TABLE [TB_System] (
  [intSysUnique] int NOT NULL
, [txtSettings] ntext NOT NULL
, [intWriteOffStatus] int NULL
, [intPassStatus] int NULL
, [intFailStatus] int NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
, [strErrorNoPrefix] nvarchar(50) NOT NULL
);
CREATE TABLE [TB_ProjectUsers] (
  [intProjectID] int NOT NULL
, [intUserID] int NOT NULL
, [bolStandard] bit NOT NULL
, [decDay0Hrs] float NOT NULL
, [decDay1Hrs] float NOT NULL
, [decDay2Hrs] float NOT NULL
, [decDay3Hrs] float NOT NULL
, [decDay4Hrs] float NOT NULL
, [decDay5Hrs] float NOT NULL
, [decDay6Hrs] float NOT NULL
);
CREATE TABLE [TB_Projects] (
  [intProjectID] int NOT NULL
, [strProject] nvarchar(50) NULL
, [txtNotes] ntext NULL
, [dteDeadline] datetime NOT NULL
, [dteOriginal] datetime NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_ProjectDays] (
  [intProjectID] int NOT NULL
, [bytDayIndex] tinyint NOT NULL
, [decWorkingHrs] float NOT NULL
);
CREATE TABLE [TB_Products] (
  [intProductID] int NOT NULL
, [strProduct] nvarchar(50) NOT NULL
, [txtNotes] ntext NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_OutlineTemplates] (
  [intOutlineID] int NOT NULL
, [intTemplateID] int NOT NULL
);
CREATE TABLE [TB_OutlineDetails] (
  [intOutlineID] int NOT NULL
, [intDetailID] int NOT NULL
, [strText] nvarchar(100) NOT NULL
, [bolComplete] bit NOT NULL
, [intCompletedVersionID] int NULL
, [intTemplateID] int NULL
);
CREATE TABLE [TB_Issues] (
  [intIssueID] int NOT NULL
, [intTaskID] int NOT NULL
, [strIssueDesc] nvarchar(100) NOT NULL
, [bolResolved] bit NOT NULL
, [dteResolved] datetime NULL
, [txtNotes] ntext NULL
, [intIssueLevelID] int NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_IssueLevels] (
  [intIssueLevelID] int NOT NULL
, [strDescription] nvarchar(50) NOT NULL
, [intLevelNo] int NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_Holidays] (
  [dteHoliday] datetime NULL
, [strDescription] nvarchar(50) NULL
);
CREATE TABLE [TB_Groups] (
  [intGroupID] int NOT NULL
, [strGroupName] nvarchar(50) NOT NULL
, [txtRights] ntext NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_Goals] (
  [intGoalID] int NOT NULL
, [intUserID] int NULL
, [dteGoalDate] datetime NULL
, [decWorkingHrs] numeric(18,4) NULL
, [txtBeginNotes] ntext NULL
, [txtEndNotes] ntext NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_GoalDetails] (
  [intGoalID] int NOT NULL
, [intGoalDetailID] int NOT NULL
, [intTaskID] int NULL
, [decHrsToSpend] numeric(18,2) NULL
, [intErrorID] int NULL
, [intOutlineID] int NULL
, [bytLineType] tinyint NOT NULL
);
CREATE TABLE [TB_ErrorStatus] (
  [intStatusID] int NOT NULL
, [strStatus] nvarchar(50) NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_ErrorsFoundBy] (
  [intErrorID] int NOT NULL
, [intUserID] int NOT NULL
);
CREATE TABLE [TB_ErrorsFixedBy] (
  [intErrorID] int NOT NULL
, [intUserID] int NOT NULL
);
CREATE TABLE [TB_Errors] (
  [intErrorID] int NOT NULL
, [strErrorNo] nvarchar(50) NULL
, [dteDate] datetime NULL
, [intStatusID] int NULL
, [intProductID] int NULL
, [intPriorityID] int NULL
, [dteFixedDate] datetime NULL
, [intAssignedToID] int NULL
, [txtDescription] ntext NULL
, [txtResolution] ntext NULL
, [decEstHrs] nvarchar(50) NULL
, [intTesterID] int NULL
, [dteCompletedDate] datetime NULL
, [intFoundVersionID] int NOT NULL
, [intFixedVersionID] int NULL
, [intOutlineID] int NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
, [decHrsSpent] float NULL
);
CREATE TABLE [TB_ErrorPriorities] (
  [intPriorityID] int NOT NULL
, [strDescription] nvarchar(50) NULL
, [intLevelNo] int NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_Charts] (
  [intChartID] int NOT NULL
, [strTitle] nvarchar(50) NOT NULL
, [intRefreshRate] int NOT NULL
, [strXAxisTitle] nvarchar(50) NULL
, [strYAxisTitle] nvarchar(50) NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_ChartBars] (
  [intChartID] int NOT NULL
, [intChartBarID] int NOT NULL
, [intAdvFindId] int NOT NULL
, [strCaption] nvarchar(50) NOT NULL
, [bolUseFlag] bit NOT NULL
, [bytFlagType] tinyint NULL
, [intRedFlagLevel] int NULL
, [intYellowFlagLevel] int NULL
);
CREATE TABLE [TB_AdvFinds] (
  [intAdvFindId] int NOT NULL
, [strDescription] nvarchar(50) NOT NULL
, [strTableID] nvarchar(50) NOT NULL
, [strTableDesc] nvarchar(50) NOT NULL
, [dteModifiedDate] datetime NULL
, [strModifiedBy] nvarchar(50) NULL
);
CREATE TABLE [TB_AdvFindFilters] (
  [intAdvFindId] int NOT NULL
, [intAdvFindFilterID] int NOT NULL
, [intLeftParentheses] int NULL
, [strTableID] nvarchar(50) NULL
, [strFieldID] nvarchar(50) NULL
, [bytOperand] tinyint NULL
, [strSearchValue] nvarchar(50) NULL
, [strDisplayValue] nvarchar(50) NULL
, [strFormula] ntext NULL
, [intSearchValueAdvFindID] int NULL
, [bolCustomDate] bit NOT NULL
, [bytEndLogic] tinyint NULL
, [intRightParentheses] int NULL
);
CREATE TABLE [TB_AdvFindColumns] (
  [intAdvFindId] int NOT NULL
, [intAdvFindColumnID] int NOT NULL
, [strTableID] nvarchar(50) NULL
, [strFieldID] nvarchar(50) NULL
, [strCaption] nvarchar(50) NOT NULL
, [dblPercentWidth] float NOT NULL
, [strTablePKField] nvarchar(50) NULL
, [intSortOrder] int NULL
, [bytSortType] tinyint NULL
, [strFormula] ntext NULL
, [bytFormulaDataType] tinyint NULL
);
CREATE TABLE [StockMaster] (
  [StockNumber] nvarchar(50) NOT NULL
, [Location] nvarchar(50) NOT NULL
, [Price] numeric(18,0) NOT NULL
, CONSTRAINT [PK_StockMaster] PRIMARY KEY ([StockNumber],[Location])
);
CREATE TABLE [StockCostQuantity] (
  [StockNumber] nvarchar(50) NOT NULL
, [Location] nvarchar(50) NOT NULL
, [PurchasedDateTime] datetime NOT NULL
, [Quantity] numeric(18,0) NOT NULL
, [Cost] numeric(18,0) NOT NULL
, CONSTRAINT [PK_StockCostQuantity] PRIMARY KEY ([StockNumber],[Location],[PurchasedDateTime])
, FOREIGN KEY ([StockNumber], [Location]) REFERENCES [StockMaster] ([StockNumber], [Location]) ON DELETE NO ACTION ON UPDATE NO ACTION
);
INSERT INTO [VERSYS] ([strVersion]) VALUES (
'000051');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Version 0','2015-01-01 00:00:00.000',30,'2015-08-21 19:47:45.000',NULL,'2015-08-21 19:47:10.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'00.85.0001','2015-01-01 00:00:00.000',30,'2015-08-21 16:39:42.000',NULL,'2015-08-21 16:39:47.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
3,'00.85.0002','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
4,'00.85.0003','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
5,'00.85.0004','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
6,'00.85.0006','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
7,'00.85.0005','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
8,'00.91.0008','2015-01-01 00:00:00.000',28,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
9,'00.91.0007','2015-01-01 00:00:00.000',28,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
10,'00.91.0006','2015-01-01 00:00:00.000',28,'2015-08-21 21:53:18.000',NULL,'2015-08-21 21:53:25.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
11,'00.95.0005','2015-01-01 00:00:00.000',29,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
12,'00.95.0004','2015-01-01 00:00:00.000',29,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
13,'00.95.0002','2015-01-01 00:00:00.000',29,'2015-08-21 21:54:43.000',NULL,'2015-08-21 21:54:45.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
14,'00.85.0007','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
15,'00.91.0009','2015-01-01 00:00:00.000',28,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
16,'00.95.0006','2015-01-01 00:00:00.000',29,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
17,'00.85.0008','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
18,'00.95.0007','2015-01-01 00:00:00.000',29,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
19,'00.91.0010','2015-01-01 00:00:00.000',28,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
20,'00.91.0011','2015-01-01 00:00:00.000',28,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
21,'00.95.0008','2015-01-01 00:00:00.000',29,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
22,'00.85.0009','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
45,'00.95.0009','2015-01-01 00:00:00.000',29,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
46,'00.95.0010','2015-01-01 00:00:00.000',29,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
47,'00.91.0012','2015-01-01 00:00:00.000',28,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
48,'00.91.0013','2015-01-01 00:00:00.000',28,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
49,'00.85.0010','2015-01-01 00:00:00.000',30,'2015-01-01 00:00:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
50,'00.95.0011','2015-04-25 00:00:00.000',29,'2015-04-27 10:32:01.000','2015-04-27 10:32:47.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
51,'00.91.0014','2015-02-04 17:03:12.000',28,'2015-02-04 17:03:12.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
52,'00.95.0012','2015-02-04 17:42:04.000',29,'2015-02-04 17:42:04.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
53,'00.85.0011','2015-02-04 17:43:35.000',30,'2015-02-04 17:43:35.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
54,'00.91.0015','2015-02-23 16:43:09.000',28,'2015-02-23 16:43:09.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
56,'00.85.0012','2015-02-23 16:51:06.000',30,'2015-02-23 16:51:06.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
57,'00.91.0016','2015-04-27 19:03:33.000',28,'2015-04-27 19:03:33.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
58,'00.95.0013','2015-04-27 19:04:37.000',29,'2015-04-27 19:04:37.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
59,'00.85.0013','2015-04-27 19:06:29.000',30,'2015-04-27 19:06:29.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
60,'00.92.0017','2015-05-18 19:07:04.000',28,'2015-05-18 19:07:04.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
61,'00.95.0014','2015-05-18 19:11:54.000',29,'2015-05-18 19:11:54.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
62,'00.92.0018','2015-05-18 19:14:52.000',28,'2015-05-18 19:14:52.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
63,'00.85.0014','2015-05-18 21:26:25.000',30,'2015-05-18 21:26:25.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
64,'00.91.0005','2014-08-24 21:48:04.000',30,'2014-08-24 21:48:04.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
65,'00.85.0015','2015-05-19 22:21:16.000',30,'2015-05-19 22:21:16.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
66,'00.85.0016','2015-05-20 15:10:49.000',30,'2015-05-20 15:10:49.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
67,'00.85.0017','2015-05-22 14:38:48.000',30,'2015-05-22 14:38:48.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
68,'00.92.0019','2015-05-22 14:39:30.000',28,'2015-05-22 14:39:30.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
69,'00.95.0015','2015-05-22 14:40:07.000',29,'2015-05-22 14:40:07.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
70,'00.91.0018','2015-05-23 10:24:12.000',30,'2015-05-23 10:24:12.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
71,'00.92.0020','2015-05-23 10:24:49.000',28,'2015-05-23 10:24:49.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
72,'00.96.0016','2015-05-23 10:25:49.000',29,'2015-05-23 10:25:49.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
73,'00.91.0019','2015-05-23 11:20:32.000',30,'2015-05-23 11:20:32.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
74,'00.92.0021','2015-05-23 11:21:33.000',28,'2015-05-23 11:21:33.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
75,'00.96.0017','2015-05-23 11:22:03.000',29,'2015-05-23 11:22:03.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
76,'00.91.0020','2015-05-26 20:03:49.000',30,'2015-05-26 20:03:49.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
77,'00.96.0018','2015-05-26 20:05:11.000',29,'2015-05-27 10:34:11.000','2015-06-07 10:34:22.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
78,'00.92.0022','2015-05-26 20:06:04.000',28,'2015-05-26 20:06:04.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
79,'00.92.0023','2015-06-09 20:52:05.000',28,'2015-06-10 10:24:17.000','2015-07-05 10:24:45.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
80,'00.96.0019','2015-06-09 20:55:18.000',29,'2015-06-10 10:35:55.000','2015-07-05 10:36:03.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
81,'00.91.0021','2015-06-09 20:58:32.000',30,'2015-06-09 20:58:32.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
82,'00.93.0024','2015-07-07 15:48:28.000',28,'2015-07-07 15:48:28.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
83,'00.97.0020','2015-07-07 15:52:04.000',29,'2015-07-08 10:37:21.000','2015-07-08 10:37:29.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
84,'00.91.0022','2015-07-07 16:58:38.000',30,'2015-07-07 16:58:38.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
85,'1.00.0025','2015-07-24 13:35:13.000',28,'2015-07-24 13:35:13.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
86,'1.00.0021','2015-07-24 13:37:10.000',29,'2015-07-24 13:37:10.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
87,'00.91.0023','2015-07-24 13:52:07.000',30,'2015-07-24 13:52:07.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
88,'1.00.0026','2015-07-29 10:46:09.000',28,'2015-07-29 10:26:37.000','2015-07-29 10:26:53.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
89,'1.00.0022','2015-07-29 10:47:17.000',29,'2015-07-29 10:47:17.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
90,'00.91.0024','2015-07-29 10:48:37.000',30,'2015-07-29 10:48:37.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
91,'00.91.0025','2015-07-31 16:21:00.000',30,'2015-07-31 16:21:00.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
92,'1.00.0027','2015-07-31 18:01:27.000',28,'2015-07-31 18:01:27.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
93,'1.00.0029','2015-07-31 18:05:32.000',28,'2015-07-31 18:05:32.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
94,'1.00.0024','2015-07-31 18:06:27.000',29,'2015-07-31 18:06:27.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
95,'00.91.0026','2015-08-04 21:48:04.000',30,'2015-08-04 21:48:04.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
96,'1.00.0030','2015-08-04 21:49:13.000',28,'2015-08-04 21:49:13.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
97,'1.00.0031','2015-08-06 22:00:00.000',28,'2015-08-25 12:05:47.000','2015-08-07 10:28:33.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
98,'1.01.0032','2015-08-11 13:40:33.000',28,'2015-08-12 10:29:38.000','2015-08-14 10:29:58.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
99,'00.91.0027','2015-08-11 13:44:38.000',30,'2015-08-11 13:44:38.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
100,'1.02.0033','2015-08-16 22:11:48.000',28,'2015-08-16 22:11:48.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
101,'00.91.0028','2015-08-16 22:13:28.000',30,'2015-08-16 22:13:28.000',NULL,'2015-08-22 00:00:00.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
102,'1.01.0028','2015-08-19 20:36:52.000',29,'2015-08-22 19:09:18.000',NULL,'2015-08-22 22:17:40.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
103,'00.91.0029','2015-08-19 20:38:16.000',30,'2015-08-22 19:10:53.000',NULL,'2015-08-22 22:19:20.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
104,'1.02.0034','2015-08-19 20:39:02.000',28,'2015-08-22 19:05:49.000',NULL,'2015-08-22 22:16:44.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
105,'1.00.0027','2015-08-19 10:39:01.000',29,'2015-08-19 10:39:40.000','2015-08-19 10:39:49.000',NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
106,'00.85.0001','2015-08-22 16:50:02.000',33,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
107,'1.02.0035','2015-08-22 19:06:21.000',28,'2015-08-22 22:15:48.000',NULL,'2015-08-24 14:36:29.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
108,'1.01.0029','2015-08-22 19:09:23.000',29,'2015-08-22 22:18:18.000',NULL,'2015-08-24 14:51:55.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
109,'00.91.0030','2015-08-22 19:10:57.000',30,'2015-08-22 22:19:34.000',NULL,'2015-08-24 14:53:14.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
110,'1.02.0036','2015-08-22 22:15:59.000',28,'2015-08-24 22:09:35.000',NULL,'2015-08-25 17:50:44.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
111,'1.01.0030','2015-08-22 22:18:23.000',29,'2015-08-24 22:10:28.000',NULL,'2015-08-25 17:54:23.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
112,'00.91.0031','2015-08-22 22:19:38.000',30,'2015-08-24 22:11:28.000',NULL,'2015-08-25 17:55:41.000',NULL,NULL,NULL);
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
113,'1.02.0037','2015-08-24 22:09:58.000',28,'2015-08-25 17:51:11.000',NULL,'2015-09-10 11:35:12.000',NULL,'2015-09-10 11:35:14.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
114,'1.01.0031','2015-08-24 22:10:41.000',29,'2015-08-25 17:54:11.000',NULL,'2015-09-10 11:36:57.000',NULL,'2015-09-10 11:36:59.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
115,'00.91.0032','2015-08-24 22:11:13.000',30,'2015-08-25 17:55:26.000',NULL,'2015-09-10 11:38:20.000',NULL,'2015-09-10 11:38:21.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
118,'1.02.0038','2015-08-25 17:51:16.000',28,'2015-09-10 11:34:49.000',NULL,'2015-09-12 11:49:11.000',NULL,'2015-09-12 11:49:12.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
119,'1.01.0032','2015-08-25 17:53:48.000',29,'2015-09-10 11:36:47.000',NULL,'2015-09-12 11:50:46.000',NULL,'2015-09-12 11:50:48.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
120,'00.91.0033','2015-08-25 17:54:58.000',30,'2015-09-10 11:38:09.000',NULL,'2015-09-12 11:51:33.000',NULL,'2015-09-12 11:51:34.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
121,'1.02.0039','2015-09-10 11:34:00.000',28,'2015-09-12 11:49:00.000',NULL,'2015-09-16 22:10:59.000',NULL,'2015-09-16 22:11:00.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
122,'1.01.0033','2015-09-10 11:36:10.000',29,'2015-09-12 11:50:36.000',NULL,'2015-09-16 22:12:40.000',NULL,'2015-09-16 22:12:46.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
123,'00.91.0034','2015-09-10 11:37:36.000',30,'2015-09-12 11:51:24.000',NULL,'2015-09-16 22:13:31.000',NULL,'2015-09-16 22:13:34.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
124,'1.02.0040','2015-09-12 11:48:14.000',28,'2015-09-16 22:10:46.000',NULL,'2015-09-19 20:59:26.000',NULL,'2015-09-19 20:59:28.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
125,'1.01.0034','2015-09-12 11:50:07.000',29,'2015-09-16 22:12:03.000',NULL,'2015-10-08 12:39:03.000',NULL,'2015-10-08 12:39:07.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
126,'00.91.0035','2015-09-12 11:51:07.000',30,'2015-09-16 22:13:21.000',NULL,'2015-10-10 21:50:04.000',NULL,'2015-10-10 21:50:07.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
127,'1.02.0041','2015-09-16 22:10:22.000',28,'2015-09-19 20:59:05.000','2015-09-19 20:59:10.000',NULL,NULL,'2015-09-19 20:59:13.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
128,'2.00.0035','2015-09-24 21:51:13.000',29,'2015-10-08 12:38:36.000',NULL,'2015-10-10 21:45:31.000',NULL,'2015-10-10 21:45:35.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
129,'00.91.0036','2015-09-16 22:13:01.000',30,'2015-10-10 21:49:31.000',NULL,'2015-12-02 13:47:52.000',NULL,'2015-12-02 13:47:54.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
130,'1.03.0042','2015-09-19 20:57:47.000',28,'2015-10-10 21:43:03.000',NULL,'2015-12-02 13:38:26.000',NULL,'2015-12-02 13:38:29.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
131,'2.00.0036','2015-10-08 12:39:25.000',29,'2015-10-10 21:44:43.000',NULL,'2015-10-17 22:00:25.000',NULL,'2015-10-17 22:00:27.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
132,'1.03.0043','2015-10-10 21:42:38.000',28,'2015-12-02 13:38:53.000',NULL,'2015-12-05 21:06:15.000',NULL,'2015-12-05 21:06:19.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
133,'00.91.0037','2015-10-10 21:50:22.000',30,'2015-12-02 13:47:40.000',NULL,'2015-12-05 21:07:52.000',NULL,'2015-12-05 21:07:54.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
134,'2.00.0037','2015-10-17 22:00:00.000',29,'2015-10-17 22:00:14.000','2015-10-17 22:00:14.000',NULL,NULL,'2015-12-02 13:45:55.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
135,'1.03.0044','2015-12-02 13:39:06.000',28,'2015-12-05 21:06:30.000','2015-12-05 21:06:34.000',NULL,NULL,'2015-12-05 21:06:37.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
136,'3.00.0038','2015-12-02 13:43:31.000',29,'2015-12-02 13:43:49.000',NULL,'2015-12-05 21:07:00.000',NULL,'2015-12-05 21:07:01.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
137,'3.00.0039','2015-12-02 13:44:16.000',29,'2015-12-05 21:07:10.000','2015-12-05 21:07:13.000','2015-12-08 17:17:13.000',NULL,'2015-12-08 17:17:16.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
138,'00.91.0038','2015-12-02 13:47:02.000',30,'2015-12-05 21:08:14.000','2015-12-05 22:15:44.000','2015-12-05 22:15:56.000',NULL,'2019-05-04 13:16:40.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
139,'00.91.0039','2015-12-05 21:08:20.000',30,'2019-05-04 13:14:18.000','2019-05-04 13:14:31.000','2019-05-04 13:14:34.000',NULL,'2019-05-04 13:14:37.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
140,'3.01.0040','2015-12-08 17:17:22.000',29,'2015-12-08 17:17:36.000','2015-12-08 17:17:39.000',NULL,NULL,'2015-12-08 17:17:42.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
141,'3.01.0041','2016-01-11 18:01:34.000',29,'2016-01-11 18:01:48.000','2016-01-11 18:01:50.000',NULL,NULL,'2016-01-11 18:02:25.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
142,'3.01.0042','2016-01-11 18:03:11.000',29,NULL,NULL,NULL,NULL,'2016-01-11 18:03:26.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
143,'1.03.0045','2016-01-11 18:04:37.000',28,'2016-01-11 18:05:00.000','2016-01-11 18:05:01.000',NULL,NULL,'2016-01-11 18:05:14.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
144,'1.03.0046','2016-01-11 18:05:30.000',28,NULL,NULL,NULL,NULL,'2016-01-11 18:06:27.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
145,'00.92.0050','2019-05-04 13:18:13.000',30,'2019-05-04 13:18:31.000',NULL,'2019-05-07 13:50:52.000',NULL,'2019-05-07 13:51:00.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
146,'00.92.0051','2019-05-04 14:51:08.000',30,'2019-05-07 13:51:13.000',NULL,'2019-05-15 12:00:37.000',NULL,'2019-05-15 12:00:38.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
147,'00.92.0052','2019-05-07 13:51:26.000',30,'2019-05-15 12:00:24.000',NULL,'2019-05-21 12:18:16.000',NULL,'2019-05-21 12:18:20.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
148,'00.92.0053','2019-05-15 12:00:04.000',30,'2019-05-21 12:18:50.000',NULL,'2019-05-22 14:30:35.000',NULL,'2019-05-22 14:30:37.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
149,'00.92.0054','2019-05-21 12:18:25.000',30,'2019-05-22 14:30:27.000',NULL,'2019-08-16 15:37:53.000',NULL,'2019-08-16 15:37:57.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
151,'00.92.0055','2019-05-22 14:30:11.000',30,'2019-08-16 15:38:10.000',NULL,'2019-08-16 15:38:14.000',NULL,'2019-08-16 15:38:17.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
152,'00.93.0056','2019-08-16 15:38:29.000',30,'2019-08-16 15:38:57.000',NULL,NULL,NULL,'2019-08-16 15:39:04.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
153,'00.93.0057','2019-08-16 15:39:26.000',30,'2019-08-17 12:16:08.000',NULL,'2019-08-17 12:16:12.000',NULL,'2019-08-17 12:16:15.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
154,'00.94.0058','2019-08-17 12:16:17.000',30,'2019-08-17 12:16:41.000',NULL,NULL,'First Visual Studio 2019 version.','2019-08-17 12:17:43.000','Peter Ringering');
INSERT INTO [TB_Versions] ([intVersionID],[strVersion],[dteCreated],[intProductID],[dteRelToQA],[dteRelToCust],[dteClosed],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
155,'00.94.0059','2019-08-17 12:16:49.000',30,NULL,NULL,NULL,NULL,'2019-08-17 12:16:59.000','Peter Ringering');
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
1,2);
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
1,3);
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
1,4);
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
1,5);
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
3,1);
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
9,1);
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
10,1);
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
11,1);
INSERT INTO [TB_UserSupervisors] ([intUserID],[intSupID]) VALUES (
12,1);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Peter Ringering','Peter Ringering',NULL,'PTR',0,'111000111',2,'CgCBJvx4/KZyKktbJQH52d2wGHdbX+V4z52+EKqSrfu8AijUlxboSBlOjuYBUJyinj/MS9TuL4wN53kBDIGwePlArGPg8qRbXyJIxX9BPn7glxvoY9kHqT669yPP/Pm/2MDya1PXFve1AU/y9+8ESusK+i8PdkAInwfKPNv7+oxIMdQYy9K8mDUh8a8t/+cu813frqVGxNTpiNozJeqkAwgujoUsV9iFhZy937Nk/NZuy2gbLHBWiiX0wXtzg8fpBWvLO7mT2JsNSrgvUijSQi0KTR7J5etjt2CB/NB4W6aAkhPOBaN8sftsAlJLRTbHoF1rjKKusJAPsXIS/UKiZ0X22guI1Yj5DNJHtV5ZOaRwDXpq0aFrWY4mYF0sYD6PmuYvEP/gpaf7wF0WnIilh8dda7WGVnavScuPSVnkg+nR50hROqnJBpPKb2a+Hedha/fMO4plijE7mr02ZNweFUP1z2MMdx2KSOFn+yaV4/bfTb9A6WZPHlJJmAH6MOn/L2o0AUi9qHUaocdVUJ72NIKrezpMlkUZ8hSKHvO5mp/m/jRw8VB+XOh/WAFyYlay5Y3YAhqRtoNr5XQqFmmlORPpSTG5O9QCaSSwFq1TCFI/qbIoQS7ERXEEnY4N2P1ITB1io+qf5yrX1pLtUq6/HIDDRANNcViJhFWFq9r8QDDxNM1YSko29N3XpKHoaOjT2h7IB3A4RMtzuPXloXMbEdBtcoKk6Ca6bMcIwN6wPllPSuVsEhQqA8ean+vcfSGhChXL/W5Rxp2UWPhs8r7PiphI73KI9oGcInj56zmRNBFNWibd9Yp4dbwbPttaUNaCZAAQkngihtEFXomJ3wZj/4OJLJr1L8FrnkVNl/L7jzX57MUdX2SY0OgbLZLF4pG2','2019-03-06 16:48:41.000','Peter Ringering');
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'Walter Obrien','Walter OBryan',NULL,NULL,0,'111000111',0,'kh/vvUBtGWg2JHC5RzvxSeZ0Z89HZ8mxIYr4bCmWRraNQweSkQqOAr0sP0/b30yv33Q59WZaaz05WMdSGZnN2Y5zlX7Uz4uhtajiflRAVqrkr18xQU3a25xk4CegLsoMJssXjyeYLQfoUslPMi1ZL1dCluECdWIutoB4uEZH5RvaWrXOYoDRDSHcrKXj6q0BbleMizpQlz2XEP47z11z2RQ3KVgyyvQLLhrsOIWj/3Ib/WnEgAsvmQhdxzNtofZbdpFGxB/srjRBzGAujbimx+EHzcsV00nMnWyLdtViTydupSWwpoWG8uOE4jIa+vWUbXbjF/mBzdTZ4+NLYAHxUvg/mQgU9by9ScoStlhfaFlVCqofvL3P0f2gOTraYjpl4qGJ/oQgEwXXGuG5GXFvrwGnKnZ20tziINwJFFvlmUr6OwOrmySsM6wsF7RzNjxUYo8AFtgxwOzimAgrvXfOqc3IKD803gk13Rg80FGkzIXkFKnLHFbgkMkod6AnBWbnHVDc8CQw84NhYZ+GYTFgfpZigkTFEwfA5T1l4Y1S/wc6CYUdSML81fjS+A0aOxyrXLzUgValS0SsM/X1dXG1v/O/QxRyyMA0CN1pg2xsD2/K92P8x0jAMxbHNjJbvayxQu8U7IZNV1HcBO27whPaN+ptiuvIyLtWA/wmX+xnzlC5qEVai08SoNIDncvD9F1B6E+8qJpwvj75v2e7ecXfyoW0jN4Jt3PIhjZ/E0peqUpomoxgIyJWlN4A81wScEi//3JYVruq4a4qU7Hf6622MA==',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
3,'David Matthews','David Matthews',NULL,NULL,0,'111000111',1,'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
6,'Everyone',NULL,'Notes',NULL,0,'111000111',0,'6cWHQkuVDGJsBDhBPwp89w==',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
4,'Kevin Thornberg','Kevin Thornberg',NULL,NULL,0,'111000111',0,'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
5,'Steven Jones','Steven Jones',NULL,NULL,0,'111000111',0,'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
7,'Tim Benson','Tim Benson',NULL,NULL,0,'111000111',1,'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
9,'Dawna Harkitay','Dawna Harkitay',NULL,NULL,0,'111000111',0,'oJTyStKbblS7RO/6a5aeSIT0a5FhmxmZxXsxgMiLiC/ZH2tsv4sXzstY7bJSiFO+WE4O53e1tJvdumovihskAShO3/w8/LXZx+r4xjwgE2vSo/eIgCkXWJlcgy4P/wjFUrHjlfSLXrkzjhpMQUOLhs3TbXQxmTJGuda1pqK0w7gUxGr3B4/mIWsZBr9TBFwXkAp0Er+tI9Z9qeHRkMx+qqbL0GE/X/+3qT0d21xkJjL0uiedA5YI0XtichybWIbDWqXSTxPeYM6xLxLOFSGFI3Ae3Gf+xP9LBhOL9hSEGURS07ROq7zel87r0hA5XOwp4gegd/h0RHDpbuOq0otbCJOw9B3zz/G1kQoAwHh2CxZqdHgm/4LbOWSYRWtrq6UPKh5RjdmgRVPPsJczihWXQZ4hB2ClBkyw55IEoG3YfTWKEYL4OCwP2GntSAW7eYka/fO1zpuveJa7EMksIMRwINbkWT8RKoJR47JVBY5PD51Dro9ROLVZsGB4TZc5nk8mPrsoFybqtiReds07t/wkL75trUzMA4qXSxA5DEj+gBjiDuAW5EvtyTKMZ7yea/Fam0KXGtEN5Dsa8/9AnuCC3OUImBz3oCXWgB7qic3g4SPihmdg/Iomrl1htRZ3JaTXJBygzAmnqKzdBhRJ60pt1U6Rp58Rp0cUI5mBw9GQgkt9INYb0D60nm30sXIEK89D/buA2BG0uM+zLaqImLV4ceHqlUr9o7iFEv/iKs7QxvKDQ6SfqCk5cW6o9jDdxt3u','2019-03-02 12:22:09.000','Dawna Harkitay');
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
10,'Becca Smith','Becca Smith',NULL,NULL,0,'111000111',2,'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3LrXXAOy58so98PYSHSSs3fy6eG9088GEU/1cSXCJpSbHGPhn7DnbHNmErC/k3UTTRK1oHw1mKz6i+WEXIi/szQHGYDLt7PNX6bnWtRuZ7RBbaDmtyurjRQGhHH0+zBzPo6CcSkvIwYrrDrVrfw6OOtIe/Z2l2TbhemJVhQJSpZqMzg2GFoqF6BxXuPjq1GtWA86GbIQh9iWmIWUOT/OtnvWPzdsdZhJPbjRTkMQ8QTZBTYgWKaDapRIN6yZ97MfclYZ0eVabzujJGBEhp5kMqE6gMqV1vUZH9RS4vVw+ap+OI0IaGk1PNKA24UDhL2UHZOQFpZp49uTgMbss9MXGww8zWnLrEae/xdivrgREt3YmVNe4cyEkMiDRG99mUeSyRY5TDqTKPMnyMzmnDQI5e9Zbjj6S1sNcBKQl3906w8v0pLPA3TrfdyF1bYXyAge5JMxA8oT1aiHBI2+d0Q1fi8A49N3XqI020YGdnodCKN5jCtBGHBHFFwlzp8cdQVOQ+b0X3cptCSVx0r08pVqZ72uU5zLTHxCUXip7VyVJ5oYg==',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
11,'Samuel Dunkin','Samuel Dunkin',NULL,NULL,0,'111000111',1,'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
12,'Patrick Jane','Patrick Jane',NULL,NULL,0,'111000111',1,'KBBs78uo5EIRXasbqJ/VX0xUWVkiXfsQr6ZkhFEhDfVEviFuySZBSbNW7ALhW3HvXF4csvWubvjjvDlws9qPOBadwyqy6rLQKf11+sU9BfFgquT+nZ3Zdxlob0x6vkYGo+Ug5OhbKRFouozZIpb0u/sVIyx5Me7VbtBtv9qXLU/vCpESN1AgIEovTg2fSjEk3Ds5o81ZRj0XrQGKEue965WRk43N8jS14k/cKBYAaeQUqnzdUAdPDmHun0k1nPwk251QVXRl6jUAdj9imoPOEBxivP5rKU2AFD9MZ1hqjS8KuuM+Q3kBibaXIiSQkP1huMBu7IdJr1/KTG53w7GjjRwv03kcJDWLF1WtoZxh3gdMXVDZfJGArdRGb75eQxe+bxP7qi2zDZgE488SqLAiYJxAbHmjVbdoE70SrD6CkTrjagab+UpTEY26gksY3RlCymmgCaY+uh1Mq+fUjd2MZ8Sy6Ti3HfAXVhgl7kxmrg2hAWGj+8UvAy7WNeUoi5xF1dOmYpTdWy9Ha3bTWVF5qPTxaSc/sjk6LuWcSz5faIgaMI2r5k+MHGGRiaT5fuLJI8ipZw078PYJNY3FTvZeADirdcfxvXRkLKgj3qXX/LX2DO7krNPNfIFGF0EvRXLlZk7LPhyb/LEmlhgcEubrIKi6XjiIqmJBKmuWntMKPf9kaI+P8zNwIz8nMMAtpKlQ+HSuVuCc95XdHiyowdeouQIq3b4wYaN+i/BdU630bGdz8uLpGG/Vb4lo51QD48+SkVSK1BkShAxwfDuFk4OwwF35XaTF429eCP17AGTU3hVBHt4qje7taz60hRwFPBGD',NULL,NULL);
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
17,'Supervisor',NULL,NULL,NULL,1,'111000111',0,'CgCBJvx4/KZyKktbJQH52d2wGHdbX+V4z52+EKqSrfu8AijUlxboSBlOjuYBUJyinj/MS9TuL4wN53kBDIGwePlArGPg8qRbXyJIxX9BPn7gucW3xZ6uyR+d3/oxB015JFp/Kv0PmvVdhBZtNSVDKGetfN+P5PhPuW2fO8Qy6gcx9RXv2QYMh2zHl9K+JRghwKa6Ray6SoqoLRx+nErN+H0dhmOD2DvnvE/sSb7wA8Ohlvek65mDdobWHPtag6FlpTLPie7wYhNFAywJndP1y/EOAcPZozI23hXWhu1XvAmZxyeKSORki0OPd2+hHFblt1YMPkLHVVsKf+WWLlMxqHC0k0nZBHDocSzzXUDLNWMnsUkemxhX9Dv0AVJT0cN/dRK5Cd79Rx6geLFx2C2B6/gjUJ9p+xRbyd5M8qsJTFEkxdoZGToz8SGo730JLW5fFhRuDc0DWYb48yBKdbQbCKqkgVilOpH8/3Rgf+07CTiqJ4NP+EN/IEHxVaIIUyhEnPb+cpelwdEhsDILWrNFrVv8+atcsFHji9Zm3pRy+EaCYxj/15mQp+qP+dI6JtRk21rS2qwZzV88cv/qkX21dZERjFq9NMOiJhV9JLBr4/RD62KuD/ExTZFIgfbeRJaxEREPn4Bdg5wxRbCBUX5rEGhQ7PXvIufC67fSKiFNNblF+00XLwjORXysWcLIVLSYxjqLB3mAjo64VD9St5J1VTjJSxBRmw12otN42OWjE/cWYiyUHpfxNgn8/zpSNwxAr357UJHeFT98oRYakCMCJfkQ17Sm5TP3F4FAqGpAwsDsyTlq9O02BJ3PwrvltL7Y/vZkZpNN8CBJZXmS6qYeo7AwYCjrP2kOVEnDThA1O/XE7ab5mIDv9JHpGwIzi0hb','2019-03-02 11:47:58.000','Supervisor');
INSERT INTO [TB_Users] ([intUserID],[strUserName],[strEmailAddress],[txtNotes],[strInitials],[bolSUP],[strPassword],[bytDeveloperType],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
60,'Teresa Lisbond','Teresa Lisbond',NULL,NULL,0,'111000111',1,'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==',NULL,NULL);
INSERT INTO [TB_UserGroups] ([intUserID],[intGroupID]) VALUES (
1,1);
INSERT INTO [TB_UserGroups] ([intUserID],[intGroupID]) VALUES (
9,3);
INSERT INTO [TB_UserDaysOff] ([intUserID],[dteDateOff],[strDescription]) VALUES (
1,'2019-03-30 00:00:00.000','Birthday');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
39,2,1,'2019-05-15 13:00:11.000','2019-05-15 13:05:13.000',NULL,NULL,239,'Passed.',0,'2019-05-15 13:05:23.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
40,2,1,'2019-05-15 13:07:47.000','2019-05-15 14:01:42.000',NULL,NULL,198,'Fixing error.',0,'2019-05-15 14:01:53.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
41,2,1,'2019-05-20 14:35:36.000','2019-05-20 15:50:37.000',NULL,NULL,204,'Fixed error.',0,'2019-05-20 15:50:52.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
42,2,1,'2019-05-21 12:17:12.000','2019-05-21 12:28:27.000',NULL,NULL,196,'Tested and failed.',0,'2019-05-21 12:28:42.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
43,2,1,'2019-05-21 12:29:02.000','2019-05-21 12:35:16.000',NULL,NULL,198,'Tested and failed.',0,'2019-05-21 12:35:31.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
44,2,1,'2019-05-21 12:35:45.000','2019-05-21 12:42:14.000',NULL,NULL,199,'Tested and passed.',0,'2019-05-21 12:42:29.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
45,2,1,'2019-05-21 12:48:41.000','2019-05-21 13:33:03.000',NULL,NULL,229,'Tested and failed.',0,'2019-05-21 13:33:15.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
46,2,1,'2019-05-21 13:33:42.000','2019-05-21 13:37:07.000',NULL,NULL,231,'Tested and passed.',0,'2019-05-21 13:37:19.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
47,2,1,'2019-05-21 13:39:47.000','2019-05-21 13:51:08.000',NULL,NULL,196,'Fixed.',0,'2019-05-21 13:51:17.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
48,2,1,'2019-05-21 13:52:28.000','2019-05-21 16:18:07.000',NULL,NULL,198,'Fixed error.',0,'2019-05-21 16:18:16.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
49,2,1,'2019-05-21 16:19:16.000','2019-05-21 16:32:24.000',NULL,NULL,229,'Fixed error.',0,'2019-05-21 16:32:34.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
50,2,1,'2019-05-22 13:26:01.000','2019-05-22 13:39:17.000',NULL,NULL,232,'Fixed error.',0,'2019-05-22 13:39:27.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
51,2,1,'2019-05-22 13:39:57.000','2019-05-22 14:03:05.000',NULL,NULL,233,'Fixed error.',0,'2019-05-22 14:03:15.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
52,2,1,'2019-05-22 14:09:50.000','2019-05-22 14:23:15.000',NULL,NULL,241,'Fixed Error.',0,'2019-05-22 14:23:38.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
53,2,1,'2019-05-22 14:31:17.000','2019-05-22 14:36:04.000',NULL,NULL,196,'Tested and passed error.',0,'2019-05-22 14:37:10.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
54,0,1,'2019-05-22 14:55:59.000','2019-05-22 16:50:50.000',519,NULL,NULL,'Working on back end XML.',0,'2019-05-22 16:51:15.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
55,0,1,'2019-05-31 11:57:46.000','2019-05-31 14:48:52.000',520,NULL,NULL,'Code Complete',0,'2019-05-31 14:49:07.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
56,2,1,'2019-06-01 12:33:25.000','2019-06-01 13:00:35.000',NULL,NULL,269,'Fixed error.',0,'2019-06-01 13:00:46.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
57,0,1,'2019-06-01 13:04:17.000','2019-06-01 13:50:50.000',519,NULL,NULL,'Working on PTRReports.mdb',0,'2019-06-01 13:51:13.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
58,0,1,'2019-06-06 13:30:56.000','2019-06-06 15:23:28.000',519,NULL,NULL,'Designing',0,'2019-06-06 15:23:43.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
59,0,1,'2019-06-14 11:40:31.000','2019-06-14 12:13:33.000',519,NULL,NULL,'Got report UI form mostly done.  Researching report object.',0,'2019-06-14 12:14:25.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
60,0,1,'2019-06-14 14:18:09.000','2019-06-14 15:10:43.000',519,NULL,NULL,'Working on report SQL.',0,'2019-06-14 15:11:03.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
61,0,1,'2019-06-19 13:30:56.000','2019-06-19 16:26:38.000',519,NULL,NULL,'Working on SQL Statement.',0,'2019-06-19 16:27:00.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
62,0,1,'2019-07-09 13:18:12.000','2019-07-09 14:52:41.000',519,NULL,NULL,'Got filter complete.',0,'2019-07-09 14:53:04.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
63,0,1,'2019-07-09 14:53:11.000','2019-07-09 16:40:04.000',519,NULL,NULL,'Finished User Report Sql.  Got started on Code Type Report Sql.',0,'2019-07-09 16:41:45.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
64,0,1,'2019-07-16 13:11:14.000','2019-07-16 15:46:24.000',519,NULL,NULL,'Got report data to output to PTRReports.mdb.  Started working on Crystal Reports file.',0,'2019-07-16 15:47:34.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
65,0,1,'2019-07-24 12:15:54.000','2019-07-24 16:05:59.000',519,NULL,NULL,'Got hard part of User report done.  Started on Product and Project SQL.',0,'2019-07-24 16:06:56.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
66,0,1,'2019-07-25 10:10:37.000','2019-07-25 16:38:13.000',519,NULL,NULL,'Almost done with Product sql statement.',0,'2019-07-25 16:39:07.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
67,0,1,'2019-07-26 11:24:15.000','2019-07-26 15:31:34.000',519,NULL,NULL,'Code refactoring.',0,'2019-07-26 15:31:53.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
68,0,12,'2019-07-27 13:31:45.000','2019-07-27 14:31:48.000',521,NULL,NULL,'Aaaaaaaaaa',1,'2019-07-27 13:32:24.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
69,0,10,'2019-07-27 14:33:01.000','2019-07-27 14:59:05.000',522,NULL,NULL,'qqqqqqqqq',1,'2019-07-27 13:33:49.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
70,1,12,'2019-07-27 15:15:24.000','2019-07-27 15:57:31.000',NULL,5,NULL,'aaaaaaaaaa',1,'2019-07-27 15:16:01.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
71,1,10,'2019-07-26 15:16:07.000','2019-07-26 16:01:10.000',NULL,5,NULL,'qqqqqqqq',1,'2019-07-27 15:16:58.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
72,0,1,'2019-07-27 09:58:02.000','2019-07-27 17:16:12.000',519,NULL,NULL,'Finished all report SQL.  Added features to filter non-user reports on user.  Also filter user and product reports on time clock type.',0,'2019-07-27 17:18:25.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
73,0,1,'2019-07-30 13:05:23.000','2019-07-30 15:24:47.000',519,NULL,NULL,'Code refactoring.',0,'2019-07-30 15:25:08.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
2,2,1,'2019-03-23 12:27:58.000','2019-03-23 12:48:03.000',NULL,NULL,123,'20 Minutes',0,'2019-04-20 10:07:48.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
3,1,1,'2019-03-23 12:28:58.000','2019-03-23 14:48:02.000',NULL,26,NULL,'2 Hours, 19 Minutes',0,'2019-04-20 10:08:59.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
4,0,1,'2019-04-05 12:56:50.000','2019-04-05 13:45:52.000',516,NULL,NULL,'aaaaaaa',0,'2019-04-05 12:57:58.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
5,0,1,'2019-04-05 13:55:06.000','2019-04-05 14:12:10.000',516,NULL,NULL,'ssssssss',0,'2019-04-05 13:55:50.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
6,0,10,'2019-04-09 14:39:53.000','2019-04-09 14:55:17.000',471,NULL,NULL,'aaaaaaaaa',0,'2019-07-16 16:38:29.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
7,0,10,'2019-04-09 14:46:05.000','2019-04-09 15:26:10.000',516,NULL,NULL,'aaaaaa',0,'2019-07-16 16:32:54.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
9,0,2,'2019-04-19 11:50:30.000','2019-04-19 12:50:33.000',516,NULL,NULL,'aaaaaaaa',1,'2019-07-16 16:32:24.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
10,0,2,'2019-04-19 14:31:10.000','2019-04-19 14:31:14.000',516,NULL,NULL,'aaaaaaaa',0,'2019-07-16 16:39:01.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
11,1,1,'2019-04-25 15:31:06.000','2019-04-25 15:31:19.000',NULL,1,NULL,'Test',0,'2019-04-25 15:31:41.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
12,1,1,'2019-04-25 15:33:22.000','2019-04-25 16:00:25.000',NULL,1,NULL,'Test',1,'2019-04-25 15:33:49.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
13,0,1,'2019-04-27 13:38:11.000','2019-04-27 13:38:23.000',516,NULL,NULL,'cccccccc',0,'2019-04-27 13:38:31.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
14,0,1,'2019-04-27 13:38:46.000','2019-04-27 13:39:03.000',516,NULL,NULL,'aaaaaaaaa',0,'2019-04-27 13:39:07.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
15,0,1,'2019-04-27 13:42:04.000','2019-04-27 13:42:27.000',516,NULL,NULL,'aaaaaa',0,'2019-04-27 13:42:33.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
16,2,1,'2019-04-30 13:33:45.000','2019-04-30 13:33:49.000',NULL,NULL,123,'aaaaaaa',0,'2019-04-30 13:33:59.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
17,0,1,'2019-05-01 15:02:20.000','2019-05-01 15:06:05.000',516,NULL,NULL,'aaaaaa',0,'2019-05-01 15:06:16.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
18,0,1,'2019-05-01 15:06:30.000','2019-05-01 15:27:07.000',516,NULL,NULL,'Test',0,'2019-05-01 15:27:22.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
19,2,1,'2019-05-04 15:09:09.000','2019-05-04 16:00:07.000',NULL,NULL,237,'Fixed error.',0,'2019-05-04 16:00:26.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
21,2,1,'2019-05-07 15:15:47.000','2019-05-07 17:43:35.000',NULL,NULL,239,'Got error mostly fixed.',0,'2019-05-07 17:44:00.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
22,2,1,'2019-05-08 09:55:20.000','2019-05-08 10:59:48.000',NULL,NULL,239,'Fixed the error.',0,'2019-05-08 11:00:06.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
23,2,1,'2019-05-08 11:06:25.000','2019-05-08 11:56:38.000',NULL,NULL,242,'Test',1,'2019-05-08 11:07:08.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
24,2,1,'2019-05-08 11:08:21.000','2019-05-08 11:55:23.000',NULL,NULL,242,'Test',1,'2019-05-08 11:08:44.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
25,2,1,'2019-05-08 11:17:57.000','2019-05-08 11:19:13.000',NULL,NULL,239,'Test',0,'2019-05-08 11:19:24.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
26,2,1,'2019-05-08 11:21:25.000','2019-05-08 11:55:28.000',NULL,NULL,242,'Test',1,'2019-05-08 11:21:46.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
27,2,1,'2019-05-09 14:53:29.000','2019-05-09 15:24:10.000',NULL,NULL,243,'Researched and closed.',0,'2019-05-09 15:24:34.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
28,2,1,'2019-05-09 15:39:58.000','2019-05-09 16:37:37.000',NULL,NULL,184,'Fixed Error',0,'2019-05-09 16:37:52.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
29,2,1,'2019-05-13 14:47:48.000','2019-05-13 16:14:00.000',NULL,NULL,196,'Got error halfway fixed.',0,'2019-05-13 16:14:21.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
30,0,1,'2019-05-15 11:23:37.000','2019-05-15 12:31:41.000',516,NULL,NULL,'aaaaaaaaa',1,'2019-05-15 11:24:25.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
31,0,1,'2019-05-15 11:25:13.000','2019-05-15 12:35:15.000',517,NULL,NULL,'aaaaaaa',1,'2019-05-15 11:25:46.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
32,0,1,'2019-05-15 11:26:03.000','2019-05-15 12:46:06.000',517,NULL,NULL,'qqqqq',1,'2019-05-15 11:26:41.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
33,0,1,'2019-05-15 11:29:38.000','2019-05-15 12:45:42.000',517,NULL,NULL,'wwwwwwwww',1,'2019-05-15 11:30:04.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
34,0,1,'2019-05-15 11:33:07.000','2019-05-15 12:45:12.000',517,NULL,NULL,'qqqqqq',1,'2019-05-15 11:33:33.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
35,0,1,'2019-05-15 11:33:07.000','2019-05-15 12:55:14.000',517,NULL,NULL,'qqqqqqqqq',1,'2019-05-15 11:46:40.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
36,0,1,'2019-05-15 11:50:20.000','2019-05-15 13:01:13.000',517,NULL,NULL,'qqqqqqq',1,'2019-05-15 11:51:19.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
37,2,1,'2019-05-15 12:14:46.000','2019-05-15 12:55:32.000',NULL,NULL,196,'Fixed error.',0,'2019-05-15 12:55:44.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
38,2,1,'2019-05-15 12:57:03.000','2019-05-15 12:59:23.000',NULL,NULL,184,'Passed',0,'2019-05-15 12:59:33.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
74,0,1,'2019-07-30 15:25:13.000','2019-07-30 16:36:04.000',519,NULL,NULL,'Fixxed Include group box border color.  Added report format.',0,'2019-07-30 16:36:49.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
75,0,1,'2019-07-31 10:50:14.000','2019-07-31 11:22:39.000',519,NULL,NULL,'Working on Crystal user report.',0,'2019-07-31 11:23:14.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
76,0,1,'2019-07-31 12:48:03.000','2019-07-31 14:25:16.000',519,NULL,NULL,'Working on User Crystal report.',0,'2019-07-31 14:25:58.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
77,0,1,'2019-08-05 13:47:18.000','2019-08-05 16:06:07.000',519,NULL,NULL,'User report code complete.',0,'2019-08-05 16:06:28.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
78,0,1,'2019-08-06 12:30:32.000','2019-08-06 15:37:24.000',519,NULL,NULL,'Task, Testing Outline, Error Reports code complete.',0,'2019-08-06 15:38:29.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
79,0,1,'2019-08-15 15:19:37.000','2019-08-15 16:24:49.000',519,NULL,NULL,'Almost done with Product report.',0,'2019-08-15 16:25:11.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
80,0,1,'2019-08-16 14:29:54.000','2019-08-16 15:26:53.000',519,NULL,NULL,'All reports and form code complete.',0,'2019-08-16 15:27:29.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
81,0,1,'2019-08-19 16:00:54.000','2019-08-19 18:16:30.000',523,NULL,NULL,'Researched SqlkData.  Created DevLogix project.  Organized RSDbLookup dll.  Added console project.  Started Query, QueryTable and SelectColumn classes.',1,'2019-08-19 18:18:12.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
82,0,1,'2019-08-19 19:32:11.000','2019-08-19 20:22:32.000',523,NULL,NULL,'Finished SelectColumn and QueryTable classes.',0,'2019-08-19 20:23:33.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
83,0,1,'2019-08-19 20:23:51.000','2019-08-19 21:02:18.000',523,NULL,NULL,'Created interface and MS Access class.',0,'2019-08-20 10:06:21.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
84,0,1,'2019-08-20 10:10:50.000','2019-08-20 17:55:41.000',523,NULL,NULL,'Finished SELECT, FROM, and JOIN clauses.',0,'2019-08-20 17:56:20.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
85,0,1,'2019-08-21 10:51:03.000','2019-08-21 13:12:15.000',523,NULL,NULL,'Converted ISelectSqlGenerator into abstract class and moved most of the code in MsAccessSelectSqlGenerator into it.',0,'2019-08-21 13:14:25.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
86,0,1,'2019-08-21 13:14:31.000','2019-08-21 17:52:04.000',525,NULL,NULL,'Code complete!',0,'2019-08-21 17:52:18.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
87,0,1,'2019-08-22 10:43:56.000','2019-08-22 12:02:17.000',525,NULL,NULL,'Organized and documented GetDataProcessor.',0,'2019-08-22 12:02:37.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
88,0,1,'2019-08-22 12:04:01.000','2019-08-22 16:04:06.000',526,NULL,NULL,'Done with class.',0,'2019-08-22 16:04:20.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
89,0,1,'2019-08-22 16:05:09.000','2019-08-22 17:53:43.000',523,NULL,NULL,'Added WHERE functionality to Query class.',0,'2019-08-22 17:54:31.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
90,0,1,'2019-08-23 10:45:59.000','2019-08-23 18:51:38.000',523,NULL,NULL,'Nested query and WHERE clause code complete.  Just need to document WHERE clause.',0,'2019-08-23 18:52:46.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
91,0,1,'2019-08-24 11:10:28.000','2019-08-24 21:07:50.000',523,NULL,NULL,'Added support for SQL Server, MySQL and Sqlite.',0,'2019-08-24 21:08:47.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
92,0,1,'2019-08-25 10:09:00.000','2019-08-25 16:07:24.000',527,NULL,NULL,'Got basic structure done.',0,'2019-08-25 16:07:48.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
93,0,1,'2019-08-26 12:07:31.000','2019-08-26 12:51:04.000',527,NULL,NULL,'Added RSDbLookupContext.',0,'2019-08-26 12:51:58.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
94,0,1,'2019-08-26 12:52:12.000','2019-08-26 17:50:20.000',523,NULL,NULL,'Got most of Enum fields done.',0,'2019-08-26 17:50:44.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
95,0,1,'2019-08-26 19:10:07.000','2019-08-26 21:17:40.000',523,NULL,NULL,'Almost finished enum wheres.',0,'2019-08-26 21:18:13.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
96,0,1,'2019-08-27 10:55:57.000','2019-08-27 18:18:06.000',523,NULL,NULL,'Finished Enums and formulas.  Almost done with order by.',0,'2019-08-27 18:18:49.000','Peter Ringering');
INSERT INTO [TB_Timeclock] ([intTimeclockId],[bytType],[intUserID],[dtePunchIn],[dtePunchOut],[intTaskID],[intOutlineID],[intErrorID],[txtNotes],[bolPunchChanged],[dteModifiedDate],[strModifiedBy]) VALUES (
97,0,1,'2019-08-28 10:19:54.000','2019-08-28 17:40:24.000',527,NULL,NULL,'Got basic striucture done.  Added support for date and enum field definitions.',0,'2019-08-28 17:41:56.000','Peter Ringering');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,0,'Tab through all controls');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,1,'ENTER through all controls');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,2,'Enter max values on all controls and save');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,3,'Resize/Verify minumum size');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,4,'Check form header text');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,5,'Click help button');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,6,'Check form rights');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,7,'Launch lookups');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,8,'Check lookup add-on-the-fly');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,9,'ESC  key closes form');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
1,10,'All buttons do something');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,0,'ENTER/Tab through all editable columns');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,1,'Set max values in all columns and save');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,2,'Check dirty flag');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,3,'Cut/Copy/Paste rows');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,4,'Drag/Drop rows');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,5,'Clear Grid');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,6,'Insert Row');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,7,'Delete Row');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,8,'Lanch Lookups');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,9,'Lookup columns data validation');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,10,'Verify Combo columns dropdown width');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
6,11,'Enter 2 rows with same primary key values and save');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
7,0,'ENTER launches add-on-the-fly');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
7,1,'Add/Edit button');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
7,2,'Sort all columns');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
7,3,'Search all columns');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
8,0,'Clean Install');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
8,1,'Create New Database');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
8,2,'Insert new records into new database');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
8,3,'Change Database--Update globals');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,0,'Exit button/Esc');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,1,'Save button/Ctrl + S');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,2,'New button/Ctrl + N');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,3,'Delete button/Ctrl + D');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,4,'Validate Delete Foreign Keys');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,5,'Previous button/Ctrl + Left Arrow');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,6,'Next button/Ctrl + Right Arrow');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,7,'Find button/Ctrl + F');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,8,'Check Dirty Flag on all controls');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,9,'Test validation on all controls.');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,10,'Notes control tab *');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,11,'Print Report');
INSERT INTO [TB_TestTemplDetails] ([intTemplateID],[intDetailID],[strText]) VALUES (
2,12,'Advanced Find Table');
INSERT INTO [TB_TestingTemplates] ([intTemplateID],[strName],[txtNotes],[intBaseTemplateID],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Basic Form',NULL,5,NULL,NULL);
INSERT INTO [TB_TestingTemplates] ([intTemplateID],[strName],[txtNotes],[intBaseTemplateID],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'Record Maintenance Form',NULL,1,'2019-05-15 17:18:45.000','Peter Ringering');
INSERT INTO [TB_TestingTemplates] ([intTemplateID],[strName],[txtNotes],[intBaseTemplateID],[dteModifiedDate],[strModifiedBy]) VALUES (
6,'Grid',NULL,NULL,'2015-10-08 12:55:21.000','Peter Ringering');
INSERT INTO [TB_TestingTemplates] ([intTemplateID],[strName],[txtNotes],[intBaseTemplateID],[dteModifiedDate],[strModifiedBy]) VALUES (
7,'Lookup Control',NULL,NULL,NULL,NULL);
INSERT INTO [TB_TestingTemplates] ([intTemplateID],[strName],[txtNotes],[intBaseTemplateID],[dteModifiedDate],[strModifiedBy]) VALUES (
8,'Application',NULL,NULL,'2019-05-15 12:58:48.000','Peter Ringering');
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
1,'Testing Outlines',30,1,1,NULL,0.1,NULL,'2019-05-15 17:19:23.000','Peter Ringering',0.45);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
4,'Errors Form',30,1,1,NULL,0,NULL,'2019-05-15 17:19:36.000','Peter Ringering',NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
5,'Test',30,1,12,NULL,0,NULL,'2019-05-15 17:19:50.000','Peter Ringering',1.45);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
6,'Advanced Find',30,1,1,NULL,0,NULL,NULL,NULL,NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
9,'Bank Account Manager',28,1,1,NULL,1,NULL,NULL,NULL,NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
10,'Recurring Templates',28,1,1,NULL,1,NULL,NULL,NULL,NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
21,'Prepare New Grocery Shopping List',29,1,1,NULL,0.9,NULL,'2015-10-08 21:49:03.000','Peter Ringering',NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
22,'Projects window',30,1,1,NULL,0,NULL,'2019-05-15 17:21:13.000','Peter Ringering',NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
23,'Add/Edit Items',29,1,1,NULL,0.9,NULL,'2019-05-15 17:22:03.000','Peter Ringering',NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
25,'Setup Categories Order',29,1,1,NULL,0.9,NULL,'2015-10-08 15:53:03.000','Peter Ringering',NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
26,'Add/Edit Categories',29,1,1,NULL,0.9,NULL,'2019-05-15 17:28:36.000','Peter Ringering',NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
27,'Add/Edit Shopping Lists',29,1,1,NULL,0.9,NULL,'2019-05-15 17:28:53.000','Peter Ringering',NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
28,'Tools/Options',29,1,1,NULL,0.9,NULL,'2015-10-08 16:01:09.000','Peter Ringering',NULL);
INSERT INTO [TB_TestingOutlines] ([intOutlineID],[strName],[intProductID],[intCreatedByID],[intAssignedToID],[dteDueDate],[decPercentComplete],[txtNotes],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
31,'Error Test',33,1,NULL,NULL,0,NULL,'2019-05-22 14:32:45.000','Peter Ringering',NULL);
INSERT INTO [TB_TaskStatus] ([intStatusID],[strDescription],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Not Started',NULL,NULL);
INSERT INTO [TB_TaskStatus] ([intStatusID],[strDescription],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'In Progress',NULL,NULL);
INSERT INTO [TB_TaskStatus] ([intStatusID],[strDescription],[dteModifiedDate],[strModifiedBy]) VALUES (
3,'On Hold',NULL,NULL);
INSERT INTO [TB_TaskStatus] ([intStatusID],[strDescription],[dteModifiedDate],[strModifiedBy]) VALUES (
4,'Code Complete',NULL,NULL);
INSERT INTO [TB_TaskStatus] ([intStatusID],[strDescription],[dteModifiedDate],[strModifiedBy]) VALUES (
5,'Earliest Convenience',NULL,NULL);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
471,'CPTRGrid','2012-08-31 00:00:00.000',NULL,100.00,0.25,NULL,100.00,14,NULL,2,3,0,'2015-09-07 14:36:37.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
472,'CPTRSimpleGrid','2012-12-31 00:00:00.000',NULL,100.00,0.00,NULL,100.00,14,NULL,1,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
474,'Tasks','2014-10-09 00:00:00.000','2014-10-09 00:00:00.000',8.00,8.00,NULL,8.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
475,'User Goals Update Task Hours Spent',NULL,NULL,32.00,NULL,NULL,32.00,16,NULL,1,3,1,'2019-05-22 13:11:03.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
476,'Projects',NULL,NULL,8.00,4.00,NULL,8.00,16,NULL,3,3,1,'2019-05-22 13:16:17.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
477,'Security',NULL,NULL,32.00,32.00,NULL,32.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
478,'Advanced Find','2014-10-16 00:00:00.000',NULL,40.00,80.00,NULL,40.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
479,'Chart Definition',NULL,NULL,40.00,32.00,NULL,32.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
480,'Advanced Find Report','2015-04-09 00:00:00.000','2015-04-09 17:49:00.000',32.00,32.00,NULL,32.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
481,'Upsize To SQL Server',NULL,NULL,40.00,60.00,NULL,40.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
482,'PTRFormsV2 - Internal controls',NULL,NULL,4.00,NULL,NULL,8.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
484,'Error Sub Forms',NULL,NULL,8.00,6.00,NULL,NULL,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
485,'Add/Edit Errors',NULL,NULL,8.00,80.00,NULL,8.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
487,'User Login','2014-12-09 00:00:00.000',NULL,8.00,40.00,NULL,8.00,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
488,'Project status report - Calculate estimated completion date.',NULL,NULL,24.00,NULL,NULL,NULL,16,NULL,1,3,1,'2019-05-22 13:07:39.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
489,'Bank Account Report','2015-05-21 00:00:00.000',NULL,8.00,NULL,NULL,NULL,17,NULL,2,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
490,'User Goals','2015-05-19 00:00:00.000','2015-05-19 22:18:00.000',4.00,16.00,NULL,NULL,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
491,'User Goals Report',NULL,NULL,8.00,NULL,NULL,NULL,16,NULL,1,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
492,'Clear Registry in -dev mode',NULL,'2015-05-20 00:00:00.000',4.00,2.00,NULL,NULL,NULL,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
493,'Help/About',NULL,'2015-05-20 00:00:00.000',2.00,2.00,NULL,NULL,16,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
494,'Prep Family Bank Account for Release',NULL,NULL,4.00,NULL,NULL,NULL,17,NULL,2,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
495,'Set Registry App Name in Debug Build',NULL,'2015-05-20 00:00:00.000',2.00,2.00,NULL,NULL,NULL,NULL,4,3,1,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
496,'Help Document',NULL,NULL,8.00,NULL,NULL,NULL,18,NULL,2,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
497,'Help Document',NULL,NULL,8.00,NULL,NULL,NULL,17,NULL,2,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
499,'Prep Shopper IS for Release',NULL,NULL,8.00,NULL,NULL,NULL,18,NULL,2,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
500,'Globals Help Document',NULL,NULL,8.00,NULL,NULL,NULL,NULL,NULL,2,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
501,'Record Locking',NULL,NULL,40.00,40.00,'SQL Server = SELECT GETDATE() AS dteTimestamp
Access = SELECT NOW() AS dteTimestamp

See http://www.codeproject.com/Articles/114262/ways-of-doing-locking-in-NET-Pessimistic-and-opt

Add nullable dteModified field and strModifiedBy on all non-grid tables.

Insert - 
  Set dteModified = SELECT GETDATE()/NOW()
  Set strModifiedBy = <Current User name.>
SELECT @@IDValue
SELECT dteModified FROM Table WHERE intPKID = ###

Get Record - Retrieve dteModified along with everything else.

Update -
  Set dteModified = SELECT GETDATE()/NOW()
  Set strModifiedBy = <Current User name.>
  WHERE intPKID = value AND (dteModifiedDate = dteCurrentModified OR dteModifiedDate IS NULL)
If Records affected = 0
  SELECT dteCurrentModified, strModifiedBy FROM Table WHERE intPKID = value.
  If 0 records
    Run Insert
  else
    Raise error saying user modified record on dteCurrentModified.
    Retrieve latest record.
',40.00,16,NULL,4,3,1,'2015-09-10 14:21:52.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
502,'Clean Install - Create Demo Database',NULL,NULL,40.00,NULL,NULL,NULL,16,NULL,1,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
503,'Graphics and Icons',NULL,NULL,20.00,NULL,NULL,NULL,16,NULL,1,3,1,'2019-05-08 14:56:48.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
504,'Clean Install - Add default records',NULL,NULL,32.00,NULL,NULL,NULL,16,NULL,1,3,0,NULL,NULL,0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
508,'Switch Adv. Find To Maint Form',NULL,NULL,10.00,NULL,NULL,10.00,14,NULL,NULL,2,0,'2018-10-22 18:13:07.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
509,'No Project',NULL,NULL,8.00,NULL,NULL,8.00,NULL,NULL,NULL,3,0,'2018-12-10 12:56:23.000','Peter Ringering',0,NULL);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
516,'Unassigned Task',NULL,NULL,32.00,2.98,NULL,32.00,16,NULL,NULL,3,1,'2019-05-22 13:10:00.000','Peter Ringering',0,NULL);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
517,'Timeclock',NULL,NULL,10.00,1.18,NULL,NULL,NULL,NULL,NULL,3,0,'2019-05-15 11:50:17.000','Peter Ringering',0,NULL);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
519,'Time Clock Report','2019-05-22 00:00:00.000','2019-08-16 00:00:00.000',50.00,49.70,NULL,20.00,16,NULL,NULL,3,1,'2019-08-16 15:28:01.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
520,'User Time Clock Lookup',NULL,NULL,5.00,2.85,NULL,NULL,16,NULL,NULL,3,1,'2019-05-31 14:50:04.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
521,'Task01',NULL,NULL,5.00,1.00,NULL,5.00,20,NULL,NULL,3,0,'2019-07-27 13:31:23.000','Peter Ringering',0,12);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
522,'Task02',NULL,NULL,5.00,0.43,NULL,NULL,20,NULL,NULL,3,0,'2019-07-27 13:32:59.000','Peter Ringering',0,10);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
523,'Query Builder/Select Sql Generator',NULL,NULL,80.00,48.18,NULL,80.00,21,NULL,NULL,3,1,'2019-08-27 21:08:03.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
525,'GetDataProcessor',NULL,NULL,40.00,5.94,NULL,40.00,21,NULL,NULL,3,1,'2019-08-22 10:43:53.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
526,'Unit Testing',NULL,NULL,40.00,4.00,NULL,NULL,21,NULL,NULL,3,1,'2019-08-22 16:04:33.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
527,'Model Definitiions',NULL,NULL,80.00,14.04,NULL,NULL,21,NULL,NULL,3,0,'2019-08-25 16:08:00.000','Peter Ringering',0,1);
INSERT INTO [TB_Tasks] ([intTaskID],[strTaskDesc],[dteDueDate],[dteCompletedDate],[decEstHrs],[decHrsSpent],[txtNotes],[decOrigEst],[intProjectID],[strCMSTaskID],[intStatusID],[intPriorityID],[decPercentComplete],[dteModifiedDate],[strModifiedBy],[intPriorityNo],[intAssignedToID]) VALUES (
528,'Lookup Definition',NULL,NULL,80.00,NULL,NULL,NULL,21,NULL,NULL,3,0,'2019-08-25 10:08:13.000','Peter Ringering',0,1);
INSERT INTO [TB_TaskPriority] ([intPriorityID],[strDescription],[dteModifiedDate],[strModifiedBy],[intPriorityNo]) VALUES (
1,'ASAP',NULL,NULL,0);
INSERT INTO [TB_TaskPriority] ([intPriorityID],[strDescription],[dteModifiedDate],[strModifiedBy],[intPriorityNo]) VALUES (
2,'Highest',NULL,NULL,0);
INSERT INTO [TB_TaskPriority] ([intPriorityID],[strDescription],[dteModifiedDate],[strModifiedBy],[intPriorityNo]) VALUES (
3,'To Do',NULL,NULL,0);
INSERT INTO [TB_System] ([intSysUnique],[txtSettings],[intWriteOffStatus],[intPassStatus],[intFailStatus],[dteModifiedDate],[strModifiedBy],[strErrorNoPrefix]) VALUES (
1,'gmWiVkm/SH/E4LQoZFT0YA==',8,5,1,'2019-08-19 17:26:50.000','Peter Ringering','E');
INSERT INTO [TB_ProjectUsers] ([intProjectID],[intUserID],[bolStandard],[decDay0Hrs],[decDay1Hrs],[decDay2Hrs],[decDay3Hrs],[decDay4Hrs],[decDay5Hrs],[decDay6Hrs]) VALUES (
14,1,1,0,6,5,6,6,2,6);
INSERT INTO [TB_ProjectUsers] ([intProjectID],[intUserID],[bolStandard],[decDay0Hrs],[decDay1Hrs],[decDay2Hrs],[decDay3Hrs],[decDay4Hrs],[decDay5Hrs],[decDay6Hrs]) VALUES (
16,1,1,0,7,7,7,7,5,5);
INSERT INTO [TB_ProjectUsers] ([intProjectID],[intUserID],[bolStandard],[decDay0Hrs],[decDay1Hrs],[decDay2Hrs],[decDay3Hrs],[decDay4Hrs],[decDay5Hrs],[decDay6Hrs]) VALUES (
16,4,1,0,7,7,7,7,5,5);
INSERT INTO [TB_ProjectUsers] ([intProjectID],[intUserID],[bolStandard],[decDay0Hrs],[decDay1Hrs],[decDay2Hrs],[decDay3Hrs],[decDay4Hrs],[decDay5Hrs],[decDay6Hrs]) VALUES (
16,9,0,0,4,4,4,4,4,0);
INSERT INTO [TB_ProjectUsers] ([intProjectID],[intUserID],[bolStandard],[decDay0Hrs],[decDay1Hrs],[decDay2Hrs],[decDay3Hrs],[decDay4Hrs],[decDay5Hrs],[decDay6Hrs]) VALUES (
16,10,1,0,7,7,7,7,5,5);
INSERT INTO [TB_ProjectUsers] ([intProjectID],[intUserID],[bolStandard],[decDay0Hrs],[decDay1Hrs],[decDay2Hrs],[decDay3Hrs],[decDay4Hrs],[decDay5Hrs],[decDay6Hrs]) VALUES (
21,1,1,0,9,9,9,9,6,9);
INSERT INTO [TB_Projects] ([intProjectID],[strProject],[txtNotes],[dteDeadline],[dteOriginal],[dteModifiedDate],[strModifiedBy]) VALUES (
14,'PTRApps',NULL,'2012-08-31 00:00:00.000','2012-08-31 00:00:00.000','2018-10-22 17:54:09.000','Peter Ringering');
INSERT INTO [TB_Projects] ([intProjectID],[strProject],[txtNotes],[dteDeadline],[dteOriginal],[dteModifiedDate],[strModifiedBy]) VALUES (
16,'DevLogix',NULL,'2015-03-31 00:00:00.000','2015-03-31 00:00:00.000','2019-02-23 13:59:13.000','Peter Ringering');
INSERT INTO [TB_Projects] ([intProjectID],[strProject],[txtNotes],[dteDeadline],[dteOriginal],[dteModifiedDate],[strModifiedBy]) VALUES (
17,'Family Bank IS',NULL,'2015-05-26 00:00:00.000','2015-05-26 00:00:00.000','2018-11-30 13:11:24.000','Peter Ringering');
INSERT INTO [TB_Projects] ([intProjectID],[strProject],[txtNotes],[dteDeadline],[dteOriginal],[dteModifiedDate],[strModifiedBy]) VALUES (
18,'Shopper IS',NULL,'2015-12-31 00:00:00.000','2015-12-31 00:00:00.000',NULL,NULL);
INSERT INTO [TB_Projects] ([intProjectID],[strProject],[txtNotes],[dteDeadline],[dteOriginal],[dteModifiedDate],[strModifiedBy]) VALUES (
20,'Test',NULL,'2019-07-27 00:00:00.000','2019-07-27 00:00:00.000','2019-07-27 13:30:32.000','Peter Ringering');
INSERT INTO [TB_Projects] ([intProjectID],[strProject],[txtNotes],[dteDeadline],[dteOriginal],[dteModifiedDate],[strModifiedBy]) VALUES (
21,'DbLookup',NULL,'2019-12-02 00:00:00.000','2019-12-02 00:00:00.000','2019-08-26 19:09:27.000','Peter Ringering');
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
14,0,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
14,1,6);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
14,2,5);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
14,3,6);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
14,4,6);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
14,5,2);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
14,6,6);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
16,0,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
16,1,7);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
16,2,7);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
16,3,7);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
16,4,7);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
16,5,5);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
16,6,5);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
17,0,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
17,1,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
17,2,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
17,3,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
17,4,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
17,5,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
17,6,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
20,0,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
20,1,8);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
20,2,6);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
20,3,8);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
20,4,8);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
20,5,8);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
20,6,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
21,0,0);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
21,1,9);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
21,2,9);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
21,3,9);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
21,4,9);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
21,5,6);
INSERT INTO [TB_ProjectDays] ([intProjectID],[bytDayIndex],[decWorkingHrs]) VALUES (
21,6,9);
INSERT INTO [TB_Products] ([intProductID],[strProduct],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
28,'Checking Account Manager',NULL,'2016-01-11 20:07:43.000','Peter Ringering');
INSERT INTO [TB_Products] ([intProductID],[strProduct],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
29,'Grocery List Creator','a','2016-01-11 20:08:32.000','Peter Ringering');
INSERT INTO [TB_Products] ([intProductID],[strProduct],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
30,'DevLogix',NULL,'2019-05-04 13:18:58.000','Peter Ringering');
INSERT INTO [TB_Products] ([intProductID],[strProduct],[txtNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
33,'Error Test',NULL,NULL,NULL);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
6,1);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
9,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
9,6);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
9,7);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
10,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
21,1);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
21,6);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
25,1);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
25,6);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
1,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
4,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
4,6);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
5,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
22,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
22,6);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
23,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
26,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
26,7);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
27,2);
INSERT INTO [TB_OutlineTemplates] ([intOutlineID],[intTemplateID]) VALUES (
27,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,1,'Tab through all controls',1,126,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,2,'Resize/Verify minumum size',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,3,'Launch lookups',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,4,'ESC  key closes form',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,5,'ENTER through all controls',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,6,'Enter max values on all controls and save',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,7,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,8,'Check lookup add-on-the-fly',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,9,'Check form rights',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,10,'Check form header text',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,11,'All buttons do something',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,12,'Save button/Ctrl + S',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,13,'Previous button/Ctrl + Left Arrow',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,14,'Next button/Ctrl + Right Arrow',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,15,'New button/Ctrl + N',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,16,'Find button/Ctrl + F',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,17,'Exit button/Esc',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,18,'Delete button/Ctrl + D',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,19,'Check Dirty Flag on all controls',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,20,'Test validation on all controls.',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,21,'Validate Delete Foreign Keys',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,22,'Notes control tab *',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,23,'Print Report',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
1,24,'Advanced Find Table',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,1,'Tab through all controls',1,49,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,2,'ENTER through all controls',1,49,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,3,'Enter max values on all controls and save',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,4,'Resize/Verify minumum size',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,5,'Check form header text',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,6,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,7,'Check form rights',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,8,'Launch lookups',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,9,'Check lookup add-on-the-fly',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,10,'ESC  key closes form',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,11,'All buttons do something',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,12,'Exit button/Esc',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,13,'Save button/Ctrl + S',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,14,'New button/Ctrl + N',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,15,'Delete button/Ctrl + D',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,16,'Previous button/Ctrl + Left Arrow',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,17,'Next button/Ctrl + Right Arrow',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,18,'Find button/Ctrl + F',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,19,'Check Dirty Flag on all controls',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,20,'Test validation on all controls.',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,21,'ENTER/Tab through all editable columns',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,22,'Set max values in all columns and save',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,23,'Check dirty flag',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,24,'Cut/Copy/Paste rows',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,25,'Drag/Drop rows',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,26,'Lookup columns data validation',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,27,'Lanch Lookups',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,28,'Verify Combo columns dropdown width',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,29,'Clear Grid',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,1,'Tab through all controls',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,2,'ENTER through all controls',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,3,'Enter max values on all controls and save',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,4,'Resize/Verify minumum size',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,5,'Check form header text',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,6,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,7,'Check form rights',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,8,'Launch lookups',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,9,'Check lookup add-on-the-fly',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,10,'ESC  key closes form',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
6,11,'All buttons do something',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,1,'Tab through all controls',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,2,'ENTER through all controls',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,3,'Enter max values on all controls and save',1,88,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,4,'Resize/Verify minumum size',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,5,'Check form header text',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,6,'Click help button',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,7,'Check form rights',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,8,'Launch lookups',1,88,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,9,'Check lookup add-on-the-fly',1,88,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,10,'ESC  key closes form',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,11,'All buttons do something',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,12,'Exit button/Esc',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,13,'Save button/Ctrl + S',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,14,'New button/Ctrl + N',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,15,'Delete button/Ctrl + D',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,30,'Insert Row',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,31,'Delete Row',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,32,'Enter 2 rows with same primary key values and save',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,33,'Validate Delete Foreign Keys',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,34,'Notes control tab *',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,35,'Print Report',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,1,'All buttons do something',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,2,'Check form header text',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,3,'Check form rights',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,4,'Check lookup add-on-the-fly',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,5,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,6,'Enter max values on all controls and save',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,7,'ENTER through all controls',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,8,'ESC  key closes form',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,9,'Launch lookups',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,10,'Resize/Verify minumum size',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,11,'Tab through all controls',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,12,'Exit button/Esc',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,13,'Save button/Ctrl + S',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,14,'New button/Ctrl + N',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,15,'Delete button/Ctrl + D',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,16,'Previous button/Ctrl + Left Arrow',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,17,'Next button/Ctrl + Right Arrow',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,18,'Find button/Ctrl + F',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,19,'Check Dirty Flag on all controls',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,20,'Test validation on all controls.',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,21,'Validate Delete Foreign Keys',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,22,'Notes control tab *',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,23,'Print Report',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,16,'Previous button/Ctrl + Left Arrow',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,17,'Next button/Ctrl + Right Arrow',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,18,'Find button/Ctrl + F',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,19,'Check Dirty Flag on all controls',1,88,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,20,'Test validation on all controls.',1,88,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,21,'Notes control tab *',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,22,'Validate Delete Foreign Keys',1,88,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,23,'Print Report',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,24,'ENTER/Tab through all editable columns',1,85,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,25,'Check dirty flag',1,88,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,26,'Cut/Copy/Paste rows',1,85,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,27,'Drag/Drop rows',1,85,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,28,'Lanch Lookups',1,88,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,29,'Lookup columns data validation',1,88,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,30,'Verify Combo columns dropdown width',1,85,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,31,'Set max values in all columns and save',1,85,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,32,'Clear Grid',1,85,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,33,'Insert Row',1,85,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,34,'Delete Row',1,85,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,35,'ENTER launches add-on-the-fly',1,85,7);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,36,'Add/Edit button',1,85,7);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,37,'Sort all columns',1,85,7);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,38,'Search all columns',1,85,7);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
9,39,'Generate From Recurring',1,85,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,1,'Tab through all controls',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,2,'ENTER through all controls',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,3,'Enter max values on all controls and save',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,4,'Resize/Verify minumum size',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,5,'Check form header text',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,6,'Click help button',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,7,'Check form rights',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,8,'Launch lookups',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,9,'Check lookup add-on-the-fly',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,10,'ESC  key closes form',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,11,'All buttons do something',1,85,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,12,'Exit button/Esc',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,13,'Save button/Ctrl + S',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,14,'New button/Ctrl + N',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,15,'Delete button/Ctrl + D',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,16,'Validate Delete Foreign Keys',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,17,'Previous button/Ctrl + Left Arrow',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,18,'Next button/Ctrl + Right Arrow',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,19,'Find button/Ctrl + F',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,20,'Check Dirty Flag on all controls',1,85,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,21,'Test validation on all controls.',1,88,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,22,'Notes control tab *',1,88,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
10,23,'Print Report',1,88,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,1,'Tab through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,2,'ENTER through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,3,'Enter max values on all controls and save',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,4,'Resize/Verify minumum size',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,5,'Check form header text',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,6,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,7,'Check form rights',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,8,'Launch lookups',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,9,'Check lookup add-on-the-fly',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,10,'ESC  key closes form',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,11,'All buttons do something',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,12,'ENTER/Tab through all editable columns',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,13,'Set max values in all columns and save',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,14,'Check dirty flag',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,15,'Cut/Copy/Paste rows',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,16,'Drag/Drop rows',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,17,'Clear Grid',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,18,'Insert Row',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,19,'Delete Row',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,20,'Lanch Lookups',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,21,'Lookup columns data validation',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,22,'Verify Combo columns dropdown width',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,23,'Enter 2 rows with same primary key values and save',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,24,'Use a new clean database',1,128,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,25,'Generate Shopping List',1,128,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,26,'Entry Validate Item',1,128,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,27,'Entry Validate Category',1,128,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,28,'Save Checklist & Item data',1,128,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
21,29,'Advanced Find Functionality',1,128,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,1,'Tab through all controls',1,90,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,2,'ENTER through all controls',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,3,'Enter max values on all controls and save',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,4,'Resize/Verify minumum size',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,5,'Check form header text',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,6,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,7,'Check form rights',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,8,'Launch lookups',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,9,'Check lookup add-on-the-fly',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,10,'ESC  key closes form',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,11,'All buttons do something',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,12,'Exit button/Esc',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,13,'Save button/Ctrl + S',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,14,'New button/Ctrl + N',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,15,'Delete button/Ctrl + D',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,16,'Validate Delete Foreign Keys',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,17,'Previous button/Ctrl + Left Arrow',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,18,'Next button/Ctrl + Right Arrow',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,19,'Find button/Ctrl + F',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,20,'Check Dirty Flag on all controls',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,21,'Test validation on all controls.',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,22,'Notes control tab *',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,23,'Print Report',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,24,'ENTER/Tab through all editable columns',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,25,'Set max values in all columns and save',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,26,'Check dirty flag',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,27,'Cut/Copy/Paste rows',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,28,'Drag/Drop rows',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,29,'Clear Grid',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,30,'Insert Row',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,31,'Delete Row',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,32,'Lanch Lookups',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,33,'Lookup columns data validation',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,1,'Tab through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,2,'ENTER through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,3,'Enter max values on all controls and save',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,4,'Resize/Verify minumum size',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,5,'Check form header text',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,6,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,7,'Check form rights',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,8,'Launch lookups',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,9,'Check lookup add-on-the-fly',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,10,'ESC  key closes form',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,11,'All buttons do something',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,12,'ENTER/Tab through all editable columns',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,13,'Set max values in all columns and save',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,14,'Check dirty flag',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,15,'Cut/Copy/Paste rows',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,16,'Drag/Drop rows',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,17,'Clear Grid',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,18,'Insert Row',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,19,'Delete Row',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,20,'Lanch Lookups',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,21,'Lookup columns data validation',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,22,'Verify Combo columns dropdown width',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,23,'Enter 2 rows with same primary key values and save',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,24,'Resort',1,128,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
25,25,'OK--Verify Store location index',1,128,NULL);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,34,'Verify Combo columns dropdown width',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,1,'Tab through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,2,'ENTER through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,3,'Enter max values on all controls and save',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,4,'Resize/Verify minumum size',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,5,'Check form header text',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,6,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,7,'Check form rights',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,8,'Launch lookups',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,9,'Check lookup add-on-the-fly',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,10,'ESC  key closes form',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,11,'All buttons do something',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,12,'Exit button/Esc',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,13,'Save button/Ctrl + S',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,14,'New button/Ctrl + N',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,15,'Delete button/Ctrl + D',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,16,'Validate Delete Foreign Keys',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,17,'Previous button/Ctrl + Left Arrow',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,18,'Next button/Ctrl + Right Arrow',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,19,'Find button/Ctrl + F',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,20,'Check Dirty Flag on all controls',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,21,'Test validation on all controls.',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,22,'Notes control tab *',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,23,'Print Report',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,1,'Tab through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,2,'ENTER through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,3,'Enter max values on all controls and save',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,4,'Resize/Verify minumum size',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,5,'Check form header text',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,6,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,7,'Check form rights',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,8,'Launch lookups',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,9,'Check lookup add-on-the-fly',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,10,'ESC  key closes form',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,11,'All buttons do something',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,12,'Exit button/Esc',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,13,'Save button/Ctrl + S',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,14,'New button/Ctrl + N',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,15,'Delete button/Ctrl + D',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,16,'Validate Delete Foreign Keys',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,17,'Previous button/Ctrl + Left Arrow',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,18,'Next button/Ctrl + Right Arrow',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,19,'Find button/Ctrl + F',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,20,'Check Dirty Flag on all controls',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,21,'Test validation on all controls.',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,22,'Notes control tab *',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,23,'Print Report',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,24,'Advanced Find Table',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,25,'ENTER/Tab through all editable columns',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,26,'Set max values in all columns and save',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,27,'Check dirty flag',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,28,'Cut/Copy/Paste rows',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,29,'Drag/Drop rows',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,30,'Clear Grid',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,31,'Insert Row',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
4,36,'Advanced Find Table',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
5,24,'Advanced Find Table',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,35,'Enter 2 rows with same primary key values and save',0,NULL,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
22,36,'Advanced Find Table',0,NULL,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,1,'Tab through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,2,'ENTER through all controls',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,3,'Enter max values on all controls and save',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,4,'Resize/Verify minumum size',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,5,'Check form header text',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,6,'Click help button',0,NULL,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,7,'Check form rights',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,8,'Launch lookups',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,9,'Check lookup add-on-the-fly',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,10,'ESC  key closes form',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,11,'All buttons do something',1,128,1);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,12,'Exit button/Esc',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,13,'Save button/Ctrl + S',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,14,'New button/Ctrl + N',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,15,'Delete button/Ctrl + D',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,16,'Validate Delete Foreign Keys',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,17,'Previous button/Ctrl + Left Arrow',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,18,'Next button/Ctrl + Right Arrow',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,19,'Find button/Ctrl + F',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,20,'Check Dirty Flag on all controls',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,21,'Test validation on all controls.',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,22,'Notes control tab *',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,23,'Print Report',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
23,24,'Advanced Find Table',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,24,'ENTER launches add-on-the-fly',1,128,7);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,25,'Add/Edit button',1,128,7);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,26,'Sort all columns',1,128,7);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,27,'Search all columns',1,128,7);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
26,28,'Advanced Find Table',1,128,2);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,32,'Delete Row',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,33,'Lanch Lookups',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,34,'Lookup columns data validation',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,35,'Verify Combo columns dropdown width',1,128,6);
INSERT INTO [TB_OutlineDetails] ([intOutlineID],[intDetailID],[strText],[bolComplete],[intCompletedVersionID],[intTemplateID]) VALUES (
27,36,'Enter 2 rows with same primary key values and save',1,128,6);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
373,471,'Grid row height is too large',1,'2012-05-26 11:43:25.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
375,471,'Alternate cell light blue/light yellow on grid',1,'2012-05-26 16:49:37.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
376,471,'Inventory Line',1,'2012-05-28 17:18:22.000','x Validate on leaving lookup cell.',1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
377,471,'Validate when changing line types',1,'2012-05-28 10:07:05.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
378,471,'Delete row via CTRL+Delete and context menu',0,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
379,471,'Insert row via CTRL-Ins and context menu',0,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
380,471,'Build context menu',0,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
381,471,'CPTRGridRow::IsRowEmpty',1,'2012-05-28 11:08:52.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
382,471,'CROW_ProtoQPBase',1,'2012-05-28 17:16:52.000','x Description
x Quantity
x Price
x Validate Quantity x Price
x Extended Price',1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
383,471,'CPTREdit::SetSelAll',1,'2012-05-28 20:31:15.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
385,475,'Task Timeclock tab page',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
387,475,'Punch In',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
388,475,'Punch Out',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
389,475,'Change PunchTo',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
392,476,'Design Issues',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
393,476,'Design Issues Form',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
394,475,'Task Timeclock Form',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
395,478,'Base Table',1,'2014-10-12 16:52:14.000',NULL,2,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
396,481,'SQL Connect',1,'2015-01-09 17:06:37.000',NULL,2,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
397,481,'Create SQL Server Database',1,'2015-01-09 17:06:07.000',NULL,3,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
398,481,'Create SQL Server Tables',1,'2015-01-09 17:06:21.000',NULL,4,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
399,481,'Copy Data from Access to SQLServer',1,'2015-01-09 17:05:29.000',NULL,5,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
400,478,'Add Columns',1,'2014-10-24 20:28:06.000',NULL,3,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
401,478,'Edit Filter',1,'2014-11-24 16:13:30.000',NULL,4,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
402,478,'Populate Lookup',1,'2014-12-03 15:00:53.000',NULL,5,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
403,478,'Form Design',1,'2014-10-12 16:51:57.000',NULL,2,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
404,474,'Add % Complete',1,'2014-11-24 17:40:49.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
405,478,'Save',1,'2014-12-03 15:01:29.000',NULL,6,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
406,478,'Load from file',1,'2014-12-03 14:58:10.000',NULL,7,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
407,478,'Table Combo Selection Change clear query',1,'2014-11-24 16:12:37.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
408,478,'Disable/Enable table combo',1,'2014-11-24 16:13:08.000','On Add Column or Add Filter - disable combo.
When both grids are clear then enable table combo.',1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
409,482,'PTRGrid control classes set to internal',1,'2015-05-19 13:45:21.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
410,482,'Lookup control classes mark as internal',1,'2015-05-19 13:45:28.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
411,478,'UserSupervisors Lookups',1,'2014-10-24 20:27:41.000','After change caption and validation.  After ENTER on tree.',1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
412,479,'Flags',1,'2015-08-25 15:27:59.000',NULL,1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
413,479,'Mouse Hover over bar',1,'2014-11-25 13:37:00.000','Show tool tip with current value.',1,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
414,484,'Error Status',1,'2014-11-25 19:03:41.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
415,484,'Library - Table, Lookup Only',1,'2014-11-25 19:03:51.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
416,484,'Error Priority',1,'2014-11-25 19:03:57.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
417,485,'Error Form',1,'2014-11-27 20:31:59.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
418,485,'Write Off',1,'2014-12-17 15:58:13.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
419,485,'Clipboard Copy',1,'2014-12-17 15:56:17.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
420,485,'Error Number Generator',1,'2014-11-27 20:32:51.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
421,485,'Fixed By Auto Populate on Write Off',1,'2014-12-17 15:57:48.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
422,485,'Found By Auto Populate on Save',1,'2014-12-17 15:58:43.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
423,477,'Group Rights Form',1,'2014-12-30 20:26:54.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
424,477,'User Rights Form',1,'2014-12-30 20:27:08.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
425,477,'Apply Rights to application forms',1,'2014-12-30 20:27:49.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
426,477,'Apply Rights to Lookups Add-on-the-fly',1,'2014-12-30 20:28:06.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
427,477,'Apply Rights to Advanced Find',1,'2014-12-30 20:28:19.000',NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
428,487,'DBBuilder TableProcessed Event',1,'2014-12-14 21:03:29.000','Respond to event in client ensuring Supervisor record exists in TB_Users.',9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
429,487,'TableProcessed--TB_System table',1,'2014-12-14 21:03:57.000','Check existing record in TB_System.  Otherwise, add record.

Login Type = Auto Login to Default User.
Default User = UserID of Supervisor.',9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
430,487,'Add Password field to TB_Users',1,'2014-12-14 21:09:11.000','Encrypt on save.  Decrypt on Load.
Allow null.
255 length.  Display Length is 20.',9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
431,487,'App Startup--after GetSettings in SoftDevApp.InitApp',1,'2014-12-14 21:09:38.000','If AutoLogin, then get default user.  Otherwise ask for user and password.
If default user has a password, then ask for password.',9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
432,487,'Users Form',1,'2014-12-14 21:13:42.000','Add Set Password button.  If user has an existing password, then ask user for old password.  Ask user for new password twice.',9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
433,490,'Timeclock',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
434,476,'Add Product ID--allow nulls',0,NULL,NULL,9,NULL,NULL);
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
435,490,'Task Lookup',0,NULL,'Add Project to Task Lookup',9,'2018-10-08 16:42:30.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
436,509,'No Project',0,NULL,NULL,2,'2018-11-01 14:13:57.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
438,519,'Backend XML',0,NULL,NULL,9,'2019-05-22 15:07:22.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
439,519,'Save XML To PTRReports.mdb',0,NULL,NULL,9,'2019-05-22 15:07:33.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
440,519,'Crystal Report',0,NULL,NULL,9,'2019-05-22 15:07:44.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
441,519,'Front End XML',0,NULL,NULL,9,'2019-05-22 15:07:53.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
442,519,'Front End Form',0,NULL,NULL,9,'2019-05-22 15:07:08.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
453,523,'Where Clause',1,'2019-08-26 12:52:35.000',NULL,1,'2019-08-26 12:52:37.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
454,523,'Order By Clause',0,NULL,NULL,1,'2019-08-22 16:53:10.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
455,523,'Nested Query',1,'2019-08-23 12:50:01.000',NULL,1,'2019-08-23 12:50:03.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
456,523,'Formulas',0,NULL,NULL,1,'2019-08-23 12:49:40.000','Peter Ringering');
INSERT INTO [TB_Issues] ([intIssueID],[intTaskID],[strIssueDesc],[bolResolved],[dteResolved],[txtNotes],[intIssueLevelID],[dteModifiedDate],[strModifiedBy]) VALUES (
457,523,'Enum Fields',0,NULL,NULL,1,'2019-08-26 12:53:15.000','Peter Ringering');
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Issue',100,NULL,NULL);
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'Milestone 01',200,NULL,NULL);
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
3,'Milestone 02',300,NULL,NULL);
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
4,'Milestone 03',400,NULL,NULL);
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
5,'Milestone 04',500,NULL,NULL);
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
6,'Milestone 05',600,NULL,NULL);
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
7,'Milestone 06',700,NULL,NULL);
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
8,'Milestone 07',800,'2015-09-19 15:28:23.000','Peter Ringering');
INSERT INTO [TB_IssueLevels] ([intIssueLevelID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
9,'Subtask',100,NULL,NULL);
INSERT INTO [TB_Holidays] ([dteHoliday],[strDescription]) VALUES (
'2019-05-27 00:00:00.000','Memorial Day');
INSERT INTO [TB_Holidays] ([dteHoliday],[strDescription]) VALUES (
'2019-07-04 00:00:00.000','4th of July');
INSERT INTO [TB_Holidays] ([dteHoliday],[strDescription]) VALUES (
'2019-09-02 00:00:00.000','Labor Day');
INSERT INTO [TB_Holidays] ([dteHoliday],[strDescription]) VALUES (
'2019-11-28 00:00:00.000','Thanksgiving Day');
INSERT INTO [TB_Holidays] ([dteHoliday],[strDescription]) VALUES (
'2019-12-25 00:00:00.000','Christmas Day');
INSERT INTO [TB_Holidays] ([dteHoliday],[strDescription]) VALUES (
'2020-01-01 00:00:00.000','New Years Day');
INSERT INTO [TB_Groups] ([intGroupID],[strGroupName],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Supervisors','KBBs78uo5EIRXasbqJ/VX0xUWVkiXfsQr6ZkhFEhDfVEviFuySZBSbNW7ALhW3HvXF4csvWubvjjvDlws9qPOBadwyqy6rLQKf11+sU9BfFgquT+nZ3Zdxlob0x6vkYGo+Ug5OhbKRFouozZIpb0u/sVIyx5Me7VbtBtv9qXLU/vCpESN1AgIEovTg2fSjEk3Ds5o81ZRj0XrQGKEue965WRk43N8jS14k/cKBYAaeQUqnzdUAdPDmHun0k1nPwk251QVXRl6jUAdj9imoPOEINK/A42qT76u2KPG4r9yCM5FbR8CLMB1mmzfwBn7Xbs3lEFuJbbhnJykte5FiA8nHaRf3/JdOxsAElVc5GW0FZYPOBPnMElLBfYkIdBJU7kD8UCFirjpnqualF9Sg2eLXitSIYBK4IYBeRFPz8w+aWQhwY1yiLz1VDdD1g6RIGO2kYYohH426MuhWLusp2z6M2eEjdVCMoONFXOi2Hokgrw5Vkiqaear3qR3wz/BSVJb/v7XFhfSG7XU+diHTc9BJNk39LUa+4pUp3AsL92bYfQ42cJISGNS5Q2QNTmeXylUzUEU6LLDQPy0bo/APJ4bYXZOQNpMBB7j1xsEJ2Gp+2LiACScsxxY7lp3BfdGI5gZFua1cNASKJyZnRk6FXZEt5nXX9p44okKgcuWZ74T6witsCDnIIDQyOe8BbvddRSKsjIMehqfvSwEwZlDvsPCWu7uHtYJosCOQ+K5YIjD+ko7EeYNsojLkTq63aGenMI9wqdyz6jK93+Oyzul3dB7BZgDy+kXwYfVa2acuF982lEm3OddTbsPQjK4Dq9/8V4TfzVoPdmGJWh7RRhchwaXgmBLrj/PEHnXYjXis4yPic=','2015-09-14 14:27:20.000','Peter Ringering');
INSERT INTO [TB_Groups] ([intGroupID],[strGroupName],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'QC Users','tULv9UCW6mm1n0iwRCyltMdIdsnHt53pyOZl0JB2jl4X0VqKm1ra+nrIxepqIAidO3vFGSpgW1hR8D1mFJVVTSfMtKsN4/GmVReX4tEuRuo9lwGL1cLXdFDLAsVirngy5BJYTLDk3r9fl7/42OGHIJKrb+rS3CBg7L86tsdGJ9EbfNu4HxHhF6sfurnFsRTjyFN8bRAcArMGVuZuVM+DpimgRODKD9eRfoeUn/XBNxsJ88MY9Y2MyS8MQOARuwHYTrKW0Ysa1jEbwC5Fp1A7jYf4nGB+cUUn5eRMmr5+yl+NECUpfsj7butqgqaKDs+4Mv2lXCB11RkB7o+MEs8pqGd7nOLwALOaNnO5FJsKhqfPbRv9AjtSavNjYZmBfR2kMzMheqASqdPIheADzerYQettLlyzeP+fyfM0Kh3JymlyBKav0AY67cQV7Ue00TovuedqVCcnv+I8Q2hkhicYAiYwqe4G3Fb9IFf5Mq2Ldvl3R2F4T824nNiSWCfPI67+cKWBEmCFypxQ/kzz+aYC26Z0KM+tnNm4csr52gwmas79DzZca0KLv4xYW4ZJGjUoX4Sw/L696ZDl3YY2p4QJ5Jc2lCmPR68V1ygYKhc2CkOs2GBwBdd7+y+HpBEp2FJxi9SYidthWM75CWL+fb/CKWsBYNErtT3sqcuAgNlHD5xDV3UFYRQV0DCUw58NB7UH',NULL,NULL);
INSERT INTO [TB_Groups] ([intGroupID],[strGroupName],[txtRights],[dteModifiedDate],[strModifiedBy]) VALUES (
3,'Programmers','tULv9UCW6mm1n0iwRCyltMVnVGf5rJ84TEQBLRq1rc7+l+YEsZ92rURQhSoXm4dNEz3uto4NoQ1pRzBwELHLCHyROKI150Skut4xjtMcyJHrre2XTCGIFLd2iS/WV8dhk25qsXZDYQ+FuU+UK8Q5LBtmiOW+9C/aOPUoke7Pvo8xeEOLhngpjETupR/NYYfGnwUaaqezYbWbbQ6xu4qblS30cgpMSpA2+LH+Hru83BSIbg3RiJI/sKUBUcnc1pQqMvP9utsqEnls42JXqXTy/tFLRT9cxL5TXwcW15N+kInQ6cy5NmZp2E0OSp+DxBuuegSNfARiyio5bBPwP9dvaL0kKFDueMsdIxTd6AqbwM1u5Ko5I4Ixa1mOwNY4syztg5IVzIPF31Dw5xfQooJPzn4Wo+p3t/Eo9tNU5KVHX1AyYYYsh5WqU3AW5cDvQ7BSGRzji46XAhbv5zuMQ6e4BBGcJFXj2cbSuRc55w0gWhlqsZzwX58C1TZ8NknRIuiFzFwUxdu4hZwxXp4DRZ6EiJqz7HPEPotRAOxpC0wudP6upMZJ3fCVZZGfbAJBpxBm1EgbBCwghY2S82eYDNbwLahq/7eQUVdREK0qQ3gCOfXYsZUlZtikFXKEerydSnWOtiVheuroPpoIySp06CBbSiWtwPd2UhjZTf8gJtPBuS8=',NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
281,1,'2015-05-11 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
284,1,'2015-05-18 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
285,1,'2015-05-19 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
286,1,'2015-05-20 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
287,1,'2015-05-21 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
288,1,'2015-05-22 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
291,1,'2015-07-25 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
292,1,'2015-07-26 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
293,1,'2015-07-27 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
294,1,'2015-07-28 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
296,1,'2015-07-29 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
297,1,'2015-07-30 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
298,1,'2015-08-11 00:00:00.000',8.0000,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
299,1,'2018-10-09 00:00:00.000',2.0000,NULL,NULL,'2018-10-08 16:44:32.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
300,1,'2019-08-19 00:00:00.000',8.0000,NULL,NULL,'2019-08-20 10:07:30.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
301,1,'2019-08-20 00:00:00.000',8.0000,NULL,NULL,'2019-08-20 14:24:17.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
302,1,'2019-08-21 00:00:00.000',8.0000,NULL,NULL,'2019-08-21 17:52:54.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
303,1,'2019-08-22 00:00:00.000',8.0000,NULL,NULL,'2019-08-22 12:03:53.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
304,1,'2019-08-23 00:00:00.000',8.0000,NULL,NULL,'2019-08-23 10:45:50.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
305,1,'2019-08-24 00:00:00.000',8.0000,NULL,NULL,'2019-08-24 11:10:06.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
306,1,'2019-08-25 00:00:00.000',8.0000,NULL,NULL,'2019-08-25 10:08:47.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
307,1,'2019-08-26 00:00:00.000',8.0000,NULL,NULL,'2019-08-26 21:18:24.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
308,1,'2019-08-27 00:00:00.000',8.0000,NULL,NULL,'2019-08-27 21:08:19.000','Peter Ringering');
INSERT INTO [TB_Goals] ([intGoalID],[intUserID],[dteGoalDate],[decWorkingHrs],[txtBeginNotes],[txtEndNotes],[dteModifiedDate],[strModifiedBy]) VALUES (
309,1,'2019-08-28 00:00:00.000',8.0000,NULL,NULL,'2019-08-28 10:19:44.000','Peter Ringering');
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
285,0,492,6.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
285,1,490,6.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
286,0,492,4.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
286,1,493,2.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
286,2,495,2.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
286,3,497,0.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
287,0,500,8.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
288,0,500,4.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
288,1,497,4.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
291,0,0,8.00,0,9,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
292,0,0,7.00,0,9,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
292,1,0,1.00,0,10,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
293,0,0,8.00,0,10,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
294,0,0,8.00,0,10,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
294,1,0,0.00,0,20,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
296,0,0,2.00,0,9,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
296,1,0,2.00,0,10,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
296,2,0,2.00,0,21,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
297,0,0,8.00,0,21,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
298,0,0,8.00,0,21,1);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
299,0,490,2.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
300,0,523,4.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
301,0,523,8.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
302,0,523,2.50,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
302,1,525,5.50,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
303,0,526,5.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
303,1,523,1.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
304,0,523,7.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
305,0,523,7.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
306,0,527,2.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
306,1,523,2.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
307,0,527,2.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
307,1,523,7.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
308,0,523,6.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
308,1,527,2.00,0,0,0);
INSERT INTO [TB_GoalDetails] ([intGoalID],[intGoalDetailID],[intTaskID],[decHrsToSpend],[intErrorID],[intOutlineID],[bytLineType]) VALUES (
309,0,527,9.00,0,0,0);
INSERT INTO [TB_ErrorStatus] ([intStatusID],[strStatus],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Pending Correction','2019-05-15 12:57:59.000','Peter Ringering');
INSERT INTO [TB_ErrorStatus] ([intStatusID],[strStatus],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'Open',NULL,NULL);
INSERT INTO [TB_ErrorStatus] ([intStatusID],[strStatus],[dteModifiedDate],[strModifiedBy]) VALUES (
3,'Pending Unit Test',NULL,NULL);
INSERT INTO [TB_ErrorStatus] ([intStatusID],[strStatus],[dteModifiedDate],[strModifiedBy]) VALUES (
5,'Closed',NULL,NULL);
INSERT INTO [TB_ErrorStatus] ([intStatusID],[strStatus],[dteModifiedDate],[strModifiedBy]) VALUES (
6,'Design',NULL,NULL);
INSERT INTO [TB_ErrorStatus] ([intStatusID],[strStatus],[dteModifiedDate],[strModifiedBy]) VALUES (
8,'Pending QA Test','2019-05-09 16:29:37.000','Peter Ringering');
INSERT INTO [TB_ErrorStatus] ([intStatusID],[strStatus],[dteModifiedDate],[strModifiedBy]) VALUES (
10,'Test',NULL,NULL);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
5,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
7,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
8,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
9,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
10,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
11,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
12,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
13,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
14,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
17,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
18,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
19,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
23,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
24,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
25,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
27,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
28,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
29,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
31,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
32,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
35,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
35,4);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
36,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
36,4);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
37,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
38,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
43,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
44,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
52,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
53,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
54,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
55,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
56,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
57,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
58,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
59,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
60,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
61,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
62,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
63,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
64,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
65,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
66,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
67,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
68,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
69,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
70,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
71,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
72,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
73,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
74,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
75,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
76,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
77,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
78,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
79,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
80,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
81,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
82,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
83,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
84,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
85,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
86,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
87,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
88,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
89,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
90,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
91,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
92,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
93,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
94,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
95,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
96,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
97,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
98,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
99,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
100,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
101,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
102,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
103,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
104,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
107,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
108,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
109,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
111,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
112,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
113,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
114,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
115,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
117,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
119,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
120,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
121,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
123,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
124,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
125,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
126,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
127,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
128,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
129,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
130,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
131,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
132,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
133,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
134,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
135,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
136,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
137,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
138,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
139,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
140,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
141,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
142,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
143,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
144,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
145,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
146,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
147,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
148,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
149,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
150,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
151,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
152,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
153,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
154,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
155,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
156,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
157,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
158,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
159,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
160,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
161,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
162,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
163,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
164,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
165,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
166,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
167,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
168,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
172,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
173,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
174,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
177,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
178,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
179,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
180,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
181,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
182,3);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
183,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
188,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
189,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
190,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
191,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
192,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
193,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
200,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
207,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
208,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
209,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
210,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
212,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
216,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
217,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
218,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
219,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
220,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
221,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
222,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
223,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
224,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
225,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
226,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
234,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
235,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
236,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
237,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
240,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
243,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
194,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
184,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
239,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
242,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
201,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
202,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
203,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
205,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
206,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
228,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
227,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
199,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
204,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
231,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
254,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
196,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
198,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
229,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
232,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
233,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
241,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
269,1);
INSERT INTO [TB_ErrorsFoundBy] ([intErrorID],[intUserID]) VALUES (
270,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
5,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
7,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
8,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
12,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
13,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
14,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
17,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
18,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
23,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
24,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
27,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
28,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
29,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
31,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
32,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
35,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
36,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
37,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
38,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
43,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
44,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
52,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
53,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
54,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
55,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
56,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
57,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
58,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
59,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
60,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
61,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
62,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
63,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
64,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
65,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
66,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
67,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
68,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
69,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
70,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
71,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
72,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
73,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
74,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
75,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
76,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
77,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
78,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
79,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
80,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
81,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
82,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
83,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
84,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
85,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
86,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
87,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
88,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
89,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
90,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
91,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
92,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
94,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
95,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
96,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
97,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
98,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
99,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
100,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
101,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
102,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
103,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
104,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
107,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
108,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
109,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
111,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
112,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
113,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
114,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
115,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
117,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
119,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
120,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
121,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
123,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
124,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
125,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
126,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
127,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
128,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
129,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
130,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
131,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
132,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
133,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
134,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
135,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
136,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
137,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
138,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
139,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
140,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
141,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
142,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
143,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
144,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
145,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
146,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
147,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
148,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
149,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
150,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
151,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
152,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
153,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
154,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
155,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
156,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
157,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
158,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
159,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
160,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
161,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
162,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
163,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
164,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
165,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
166,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
167,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
168,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
172,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
174,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
177,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
178,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
179,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
180,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
181,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
182,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
183,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
188,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
189,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
190,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
191,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
192,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
193,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
208,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
209,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
212,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
216,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
217,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
218,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
219,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
220,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
221,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
222,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
223,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
224,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
237,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
243,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
194,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
184,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
239,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
202,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
203,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
205,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
206,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
228,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
227,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
199,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
204,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
231,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
196,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
198,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
229,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
232,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
233,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
241,1);
INSERT INTO [TB_ErrorsFixedBy] ([intErrorID],[intUserID]) VALUES (
269,1);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
123,'E-122','2013-04-21 14:49:13.000',5,28,4,'2015-07-24 14:20:44.000',1,'Need to code server side to have get data and save data SQL statements in the same DB transaction so if 1 SQL statement fails, then everything is rolled back.

Example On save, we save the header then get the last PKIdent value all in the same SQL transaction.

Otherwise, if 2 people enter data and insert at the same time, they''ll get the wrong PKIdent values back and when they go save the grid data, the PKIdent value will point back to the wrong header record.','Peter Ringering - 07/24/2015 02:23:05 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:20:44 PM - Fixed. Insert code saves and retrieves the PKIdent value in the same server side call.  Changes affect PTRShared.dll
',NULL,NULL,'2015-07-24 14:23:05.000',1,85,NULL,'2019-05-10 15:19:18.000','Peter Ringering',0);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
124,'E-124','2013-04-21 15:02:32.000',5,28,2,'2013-06-14 16:36:30.000',1,'Need a cool "Record Saved" form.','Peter Ringering - 06/14/2013 04:36:30 PM - Added an icon to the Record Saved form.  Changes affect PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
125,'E-125','2013-06-06 21:34:12.000',5,28,3,'2013-06-07 21:13:06.000',1,'When you backspace once on text on a lookup textbox control, it erases the whole field.  To reproduce, type some text on a lookup textbox control, move the cursor to the end of the text.  Hit Back Space once.  Notice everything is erased.','Peter Ringering - 06/07/2013 09:13:06 PM - Code was clearing out everything if there was nothing in the database and the backspace key was pressed.  Also have to get to work when user hits backspace on the only and last character in the control.  Fixed so the control gets cleared out when backspace is pressed on the only character on the control.  Changes affect PTRFormsV2.DLL and CTLDBLookupText.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
126,'E-126','2013-06-07 12:25:21.000',5,28,3,'2013-06-07 21:25:21.000',1,'Maintenance forms.  Hide the print button by default.  There will be no  generic reports.  Shopping List can have it visible since it has a special report type.','Peter Ringering - 06/07/2013 09:25:21 PM - Corrected PTRMaintForm.cs constructor to hide the print button.  Derived classes can make it visible as needed.  Changes affect PTRFormsV2.dll and PTRMaintForm.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
127,'E-127','2013-06-07 12:30:15.000',5,28,3,'2013-06-17 20:41:02.000',1,'Need a splash screen that shows what''s going on at application startup.  It should show beginning with app startup until the user has control of the app.','Peter Ringering - 06/17/2013 08:41:02 PM - Code Complete.  Changes affect Ring2FamilyIS.exe.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
128,'E-128','2013-06-07 12:45:54.000',5,28,3,'2013-06-18 13:53:08.000',1,'Need first-time startup wizard to help new user how to quickly get Bank Account/Bill information into the system.','Peter Ringering - 06/18/2013 01:53:08 PM - Not much can be done in this area.  User can see what needs to be done with the current design.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
129,'E-129','2013-06-07 15:22:46.000',5,28,3,'2013-06-09 15:23:39.000',1,'Need to fix grid so that an empty row is added when the user enters data on the last row or when it is loaded from the database..','Peter Ringering - 06/09/2013 03:23:39 PM - Corrected.  Related to E-149.  Changes affect PTRFormsV2.dll and PTRGrid.cs.
Peter Ringering - 06/08/2013 03:30:14 PM - Fail.  When focus is on the last new line, and then you hit Ctrl+D, it erases the row.  What should happen is nothing.
Peter Ringering - 06/08/2013 02:16:10 PM - Corrected code to add a row when the user keys in new data on the last row or when the grid is loaded from the database.  Changes affect PTRFormsV2.dll and PTRGrid.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
142,'E-142','2013-06-07 17:32:37.000',5,28,2,'2013-06-18 21:29:29.000',1,'All forms.  Change the word ''Bill'' to ''Transaction''.','Peter Ringering - 06/18/2013 09:29:29 PM - Corrected.  Changes affect Ring2FamilyIS.exe.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
155,'E-155','2013-06-09 18:56:40.000',5,28,2,'2013-06-22 21:38:02.000',1,'The main sub-menus and context menus need icons.','Peter Ringering - 06/22/2013 09:38:02 PM - Fixed.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
193,'E-193','2015-02-05 18:00:31.000',5,30,3,'2019-05-10 15:01:39.000',1,'################','################',NULL,NULL,'2019-05-10 15:02:02.000',53,147,NULL,'2019-05-10 15:02:07.000',NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
194,'E-194','2015-02-13 21:11:13.000',5,30,4,'2019-05-10 15:08:02.000',1,'################','Peter Ringering - 05/10/2019 03:56:00 PM - QA Tested and Passed. 
################',NULL,NULL,'2019-05-10 15:56:00.000',53,147,NULL,'2019-05-10 15:56:05.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
130,'E-130','2013-06-07 15:21:03.000',5,28,2,'2013-06-09 16:40:11.000',1,'Recurring Template column header is underlined when grid does not have focus and the active cell is that column.','Peter Ringering - 06/09/2013 04:40:11 PM - Moved column header underline code in OnCellEnter to OnCellBeginEdit.  This will only underline the header if there''s a lookup control showing.  I kept the code in OnCellLeave so when the user leaves the cell, the underline goes away.  Changes affect PTRFormsV2.dll and PTRGrid.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
131,'E-131','2013-06-07 15:26:09.000',5,28,3,'2013-06-14 10:07:52.000',1,'When the user tries to launch the maintenance form lookup, the program needs to check to see if there are any records in the table and if not, then a message should show that there are no records.  Please enter and save a record.  This should also work when the user hits the <-- and --> maintenance form buttons.','Peter Ringering - 06/14/2013 10:07:52 AM - Corrected for the arrow buttons.  Decided against the lookup because it will just show an empty lookup which is acceptable.  Changes affect PTRFormsV2.dll','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
132,'E-132','2013-06-07 15:31:55.000',5,28,3,'2013-06-09 14:42:18.000',1,'When Bank Accounts form is in add mode and the Bills grid gets focus, we need to ask the user to save the record first.  If the user says no, then focus needs to go to the next control in the form''s tab order','Peter Ringering - 06/09/2013 02:42:18 PM - Showing a message box when grid gets focus and then changing focus was causing many more errors and design issues.  Corrected code so that in Add Mode, a message is put on top of the grid saying that the bank account must be saved before the user can enter transactions and the grid is disabled.  Once the form goes into edit mode, the grid is enabled and the save label goes away.  Changes affect Ring2FamilyIS.exe and frmBankAcctMgr.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
133,'E-133','2013-06-07 16:40:36.000',5,28,3,'2013-06-13 20:30:34.000',1,'Peter Ringering - 06/09/2013 05:25:42 PM - Correction.  What should happen is, when the user keys in a recurring template into the grid, and the template recurring type is not One Time Only, then it should auto-populate the row per E-136.  Then it should ask the user if he wants us to update the template''s Generate Starting date so that this transaction is not a duplicate.  Also, this message box should have a "Do not ask again--always update" checkbox.  If the user checks it, then the message box will not show and we will always update the template without asking the user.

Generate From Recurring.  When generating, the code should check to see if there''s a bill with the same date as what''s being generated.  If there is, then the bill should not be generated, but the Generate Starting date on the recurring template should still be updated.','Peter Ringering - 06/13/2013 08:30:34 PM - Implemented the design changes per 06/09/2013.  Changes affect PTRFormsV2.dll and Ring2FamilyIS.exe.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
134,'E-134','2013-06-07 16:47:40.000',5,28,3,'2013-06-09 19:56:51.000',1,'Bank Account Manager, Bills grid.  The recurring template lookup and autofill needs to filter to only show recurring templates that are attached to the current bank account.','Peter Ringering - 06/09/2013 07:56:51 PM - Added code to EnableGrid to filter the recurring lookup when in edit mode.  Changes affect Ring2IS.exe and frmBankAcctMgr.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
207,'E-207','2015-05-31 19:15:34.000',2,30,3,NULL,1,'When showing Help from the Print Codes and Print Options forms, the help shows but then the form that launched the Help topic closes.',NULL,NULL,NULL,NULL,76,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
225,'E-225','2015-10-11 22:11:29.000',1,29,2,NULL,1,'Launch the app.  Now maximize the main window.  Notice the embedded graphic is duplicated.',NULL,NULL,NULL,NULL,131,NULL,NULL,'2015-10-11 22:13:07.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
235,'E-235','2019-05-04 13:33:18.000',1,30,3,NULL,1,'Take Error ID off the Errors lookup.',NULL,NULL,NULL,NULL,145,NULL,NULL,'2019-05-04 13:36:54.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
236,'E-236','2019-05-04 13:36:57.000',1,30,2,NULL,1,'Advanced Find, Errors table.  Hours Spent field has 2 spaces between ''Hours'' and ''Spent''.',NULL,NULL,NULL,NULL,145,NULL,NULL,'2019-05-04 13:41:01.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
135,'E-135','2013-06-07 16:52:58.000',5,28,3,'2013-06-10 13:59:46.000',1,'Bank Account Manager, Bills grid.  When the recurring template maintenance form is launched in add-on-the-fly mode, the bank account control should populated with the bank account info in the Bank Account Manager.','Peter Ringering - 06/10/2013 01:59:46 PM - Corrected code to work as described.  Also had bank account disabled when launched from bank account manager form.  Changes affect Ring2FamilyIS.exe and frmRecurTemplates.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
136,'E-136','2013-06-07 16:57:04.000',5,28,3,'2013-06-13 20:32:40.000',1,'Bank Account Manager, Bills grid.  When a recurring template is entered and the user hits tab.  
1.  The program should validate that the recurring template is a valid one.  If not, it should show a dialog (similar to Shopping List) asking the user if he wants to add a new one or cancel.  If add, then it should show the recurring template in add-on-the-fly mode.
2.  After a valid code has been entered, the program should look in the recurring templates record and auto-populate the cells to the right with the information.','Peter Ringering - 06/13/2013 08:32:40 PM - Implemented #2.  Changes affect Ring2IS.exe and PTRFormsV2.dll.
Peter Ringering - 06/11/2013 08:59:32 PM - Implemented #1.  Changes affect Ring2IS.exe and PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
137,'E-137','2013-06-07 17:06:44.000',5,28,2,'2013-06-13 20:34:33.000',1,'Change Income/Expense to Deposit/Withdrawal.  Fix Bank Account Manager Bills grid and Recurring Templates maintenance form.','Peter Ringering - 06/13/2013 08:34:33 PM - Fixed Bank Account Manager and Recurring Templates.  Changes affect Ring2FamilyIS.exe.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
138,'E-138','2013-06-07 17:11:45.000',5,28,3,'2013-06-14 14:40:06.000',1,'Need to convert ENTER to TAB on all forms so if the user hits ENTER on a control, it functions just like a TAB.','Peter Ringering - 06/14/2013 02:40:06 PM - Corrected.  Made sure this ran correctly when textbox control accepts ENTER like the notes textbox.  Changes affect PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
139,'E-139','2013-06-07 17:18:30.000',5,28,3,'2013-06-14 16:06:02.000',1,'All forms, textbox controls.  When a textbox control gets focus, all contents should be selected.','Peter Ringering - 06/14/2013 04:06:02 PM - Corrected.  Changes affect PTRFormsV2.dll','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
140,'E-140','2013-06-07 17:20:17.000',5,28,3,'2013-06-18 21:02:08.000',1,'Research grid date control to see if it''s possible to set focus to next cell when focus is on the year and the user hits --> and to set focus to previous cell when focus is on the day and the user hits <--.','Peter Ringering - 06/18/2013 09:02:08 PM - Way too much work for now.  Would have to make a whole new control from scratch.  There''s nothing in CodeProject that I can use.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
141,'E-141','2013-06-07 17:28:52.000',5,28,3,'2013-06-18 21:22:14.000',1,'Generate from recurring button should be disabled when Bank Account Manager form is in add mode.','Peter Ringering - 06/18/2013 09:22:14 PM - Corrected.  Changes affect Ring2FamilyIS.exe.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
143,'E-143','2013-06-07 17:49:33.000',5,28,2,'2013-06-19 12:46:39.000',1,'Dashboard, Bank Account Details.  Refresh does not clear out the beginning and ending balances before reloading the form.  On a new database with no bank accounts, if you create a bank account, then goto Bank Account Details, then delete that bank account, the beginning and ending balances are the same.','Peter Ringering - 06/19/2013 12:46:39 PM - Corrected.  Changes affect Ring2FamilyIS.exe.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
242,'E-242','2019-05-08 11:05:31.000',5,30,1,NULL,1,'Test','Peter Ringering - 05/15/2019 05:15:43 PM - QA Tested and Passed. 
',NULL,NULL,'2019-05-15 17:15:43.000',147,NULL,NULL,'2019-05-15 17:15:46.000','Peter Ringering',2.18);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
144,'E-144','2013-06-07 17:53:32.000',5,28,3,'2013-06-07 20:13:39.000',1,'Set .NET Framework version to 4.0 to ensure compatibility with Windows XP.','Peter Ringering - 06/07/2013 08:13:39 PM - Set all projects''s .NET Framework version from 4.5 to 4.0.  One error resulted in XML Viewer using async/await which is only available in 4.5.  Changed code to use Background Worker control instead.  Changes affect all EXE''s and DLL''s.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
145,'E-145','2013-06-07 20:43:22.000',5,28,3,'2013-06-19 15:15:20.000',1,'MDB Login form.  Need to add a button called "Find" below the MDB textbox.  When the user clicks on it, it will open an Explorer window with the folder containing the MDB.','Peter Ringering - 06/19/2013 03:15:20 PM - Added "Locate" button below the MDB textbox.  Changes affect PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
146,'E-146','2013-06-08 15:16:01.000',5,28,3,'2013-06-19 12:09:58.000',1,'On grid, when focus is on a cell in the grid and then hit Ctrl+S the "Record Saved" form shows but then focus goes to the first cell on the first row.  Focus should not move.','Peter Ringering - 06/19/2013 12:09:58 PM - Only occurs on Bank Account Manager grid which is per design.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
147,'E-147','2013-06-08 15:18:48.000',5,28,4,'2013-06-08 20:27:31.000',1,'Generate From Recurring.  When this is run on data that has multiple weekly templates, it groups all the transactions together instead of by date, to reproduce, create a new bank account, add 3 recurring templates, all of them should be weekly types.  Now click on Generate From Recurring and notice the output.','Peter Ringering - 06/08/2013 08:27:31 PM - Corrected code so when the Bank Account Manager Transactions grid loads up, it is sorted by date and then by line number.  It would take too much code for Generate From Recurring to redo the line numbers in order to sort by date.  Changes affect Ring2FamilyIS.exe, and PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
148,'E-148','2013-06-08 15:31:02.000',5,28,3,'2013-06-21 21:08:04.000',1,'Need context menu on all grids for add row and delete row.','Peter Ringering - 06/21/2013 09:08:04 PM - Fixed multiple parts of PTRGrid and PTRGrid controls.  Changes affect PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
149,'E-149','2013-06-08 15:40:45.000',5,28,3,'2013-06-09 15:22:15.000',1,'When focus is on a row other than the last blank line and the user hits Ctrl+D, the focus should go up 1 row.  If the user hits Ctrl+D on the first row,  the focus should stay on the first row.','Peter Ringering - 06/09/2013 03:22:15 PM - Corrected code as described.  Related to E-129.  Changes affect PTRFormsV2.dll and PTRGrid.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
150,'E-150','2013-06-08 17:10:03.000',5,28,3,'2013-06-21 21:27:31.000',1,'Maintenance form tab with lookup list control and Add/Modify button.  When the add/modify form shows and then is closed, the lookup list control needs to be refreshed.','Peter Ringering - 06/21/2013 09:27:31 PM - Added code to refresh the list control after add/modify form closes.  Changes affect PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
151,'E-151','2013-06-08 17:19:25.000',5,28,2,'2013-06-21 21:36:57.000',1,'Add-on-the-fly forms should not show in the windows taskbar.','Peter Ringering - 06/21/2013 09:36:57 PM - Corrected code to not show maint. forms on the taskbar when in add-on-the-fly mode.  Changes affect PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
152,'E-152','2013-06-08 17:23:29.000',5,28,2,'2013-06-22 11:10:38.000',1,'When focus leaves a currency or number textbox, the content in the textbox needs to be formatted.','Peter Ringering - 06/22/2013 11:10:38 AM - Corrected so that when focus leaves a numeric textbox, it is formatted correctly.  Changes affect PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
153,'E-153','2013-06-08 19:52:41.000',5,28,3,'2013-06-22 14:44:31.000',1,'When you type Ctrl+C on a lookup edit control, it makes a call to the database to autofill.  Need to not do so on any key when the Ctrl or Alt keys are also down.','Peter Ringering - 06/22/2013 02:44:31 PM - Supressed autofill when control key is down.  Alt key is already supressed.  Changes affect PTRFormsV2.dll.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
154,'E-154','2013-06-08 21:02:50.000',5,28,2,'2013-06-22 14:55:10.000',1,'Bank Account Manager.  The tool tip on the top header buttons  shows "Bank Accounts" (plural).  They should show "Bank Account."','Peter Ringering - 06/22/2013 02:55:10 PM - Corrected verbage.  Changes affect Ring2FamilyIS.exe.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
156,'E-156','2013-06-09 20:22:16.000',5,28,3,'2013-06-22 15:20:10.000',1,'Modify Recurring Template lookup list.  Add Amount and Type columns.','Peter Ringering - 06/22/2013 03:20:10 PM - Added the Amount column.  Adding the Type column won''t work because, it is built into the amount column.  Changes affect Ring2FamilyIS.exe.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
157,'E-157','2013-06-11 21:03:22.000',5,28,3,'2013-06-11 21:06:39.000',1,'Bank Account Manager.  When focus is on the grid and is in edit mode on any column, if you then click on the bank name lookup control on the form, the edit control on the grid does not go away.  If you instead click on the current balance textbox, it works fine.','Peter Ringering - 06/11/2013 09:06:39 PM - Found that CausesValidation property on the lookup control was set to false.  Changing it to true made the problem go away.  Changes affect Ring2FamilyIS.exe, all forms.',NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
158,'E-158','2013-06-22 12:11:24.000',5,28,5,'2013-06-22 14:30:04.000',1,'Bank Account Manager, Transactions grid.  Load up a bank account with transactions.  Goto the first row, Recurring Template column.  Change the value to a new template.  Click Yes to add to the database.  When Recurring Template form shows, select the original template.  Click Save/Select.  Click No on the message box that asks if you want to update the date.  Notice that add to database message appears again (Error #1).  Click No and then the application crashes.','Peter Ringering - 06/22/2013 02:30:04 PM - This was occurring in the recurring transactions grid because it can override what''s in the lookup control when the user does an add-on-the-fly.  Corrected so the grid updates the control''s value to what''s in the memory after saving.  It will be the responsibility of the row object to update the passed in value object so that the grid can update the control.  Changes affect PTRFormsV2.dll and Ring2FamilyIS.exe.',NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
159,'E-159','2014-09-21 10:29:53.000',5,29,3,'2014-09-28 11:38:07.000',1,'ShopperIS, Add/Edit Shopping Lists - When you select an item from the lookup on the unspecified row in the grid, the auto tab tabs out of the grid.  If you type it in and hit TAB, the focus goes to the next row.','Peter Ringering - 09/28/2014 11:34:55 AM - Corrected PTRGrid.cs and PTRGridLDBLookupText.cs so EndEdit is called before the cell is moved.  This commits the text and updates the background row object so focus behaves per the new row object.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
160,'E-160','2014-09-21 10:38:04.000',5,29,3,'2014-10-01 13:55:57.000',1,'ShopperIS, Prepare New Grocery Shopping List.  On the checklist form, double click on the row header on an item row.  Then on the destination grid, go to the quantity column.  Change the value but stay on that cell.  Then double click on a different row header on the checklist.  Notice the quantity value goes back to the original value.','Peter Ringering - 10/01/2014 01:55:57 PM - Corrected OnLeave to commit when grid looses focus.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
161,'E-161','2014-09-22 15:12:05.000',5,28,2,'2014-10-01 14:16:27.000',1,'Print Options Dialog.  When the form starts up, there is no visual cue over the Printer radio button to show that it has focus.  If you right arrow there still is no visual cue.','Peter Ringering - 10/01/2014 02:16:27 PM - Added ShowVisualCues = true override per MSDN documentation.  Changes affect PTRRadioButton.cs and PTRCheckbox.cs.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
162,'E-162','2014-09-22 17:24:21.000',5,28,2,'2014-10-01 15:45:10.000',1,'Print Options dialog, Number of copies textbox.  Type in 100, then with cursor at the end, hit backspace once.  The whole value is erased.  What should happen is for only the last 0 be erased.','Peter Ringering - 10/01/2014 03:45:10 PM - Corrected character handling  code in PTRNumericTextBox.cs.  Changes affect all numeric and currency textboxes.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
163,'E-163','2014-09-30 12:48:35.000',5,29,3,'2014-10-01 16:02:02.000',1,'After changing the base database to a different database, the default grocery checklist and sales tax rate don''t change as well.  To reproduce,log in and notice what the grocery checklist is in Tools, Options.  Now go to Tools, Change Database and select a different database.  Now go back to Tools, Options and notice nothing has changed.','Peter Ringering - 10/01/2014 04:02:02 PM - Corrected ChangeDBMenu_Click to reload globals from database after the database is succesfully changed.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
164,'E-164','2014-09-30 13:07:44.000',5,29,2,'2014-10-01 20:05:27.000',1,'When a form is launched in add-on-the-fly mode, it needs to center to the base form.  To reproduce, goto Add/Edit Shopping Lists.  On the grid, type in something new.  Select add a new Item.  Notice the Add/Edit Items form is centered to the screen.','Peter Ringering - 10/01/2014 08:05:27 PM - Corrected centering issues with the maintenance forms and lookups.','0',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
165,'E-165','2014-09-30 13:16:16.000',5,29,3,'2014-12-17 16:47:39.000',1,'Take out the History tab out of Items form and move notes to be on the form and take out the tab control altogether.','Peter Ringering - 12/17/2014 04:47:44 PM - QC Tested and Passed. 
Peter Ringering - 12/17/2014 04:47:39 PM - Fixed. 
',NULL,NULL,'2014-12-17 16:47:44.000',1,0,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
166,'E-166','2014-10-09 14:44:47.000',5,28,2,'2015-01-11 20:51:08.000',1,'Need to Subclass all Tab controls and overridw SetFocusCues property and set to true.','Peter Ringering - 01/11/2015 08:53:02 PM - QC Closed.
Peter Ringering - 01/11/2015 08:51:08 PM - Subclassed 1 new form''s tab control and the error did NOT go away.  Therefore I conclude that this is a Microsoft error and cannot be fixed in code.
',NULL,NULL,'2015-01-11 20:53:02.000',1,14,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
167,'E-167','2014-10-09 15:10:16.000',5,28,3,'2014-10-09 21:03:48.000',1,'Rip out all autofill code and just keep box.  Causing problems with data entry.','Peter Ringering - 10/09/2014 09:03:48 PM - Fixed code in lookup so that Autofill is shut off but the autofill box stays.  Made functionality work like Visual Studio Intellisense.  Changes affect CTLDBLookupText and Grid.',NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
168,'E-168','2014-10-11 15:48:41.000',5,29,5,'2014-10-11 16:47:26.000',1,'ShopperIS, Prepare New Shopping List, Type in a valid item and hit TAB.  Go back to the item cell that you just entered.  Type in an  invalid item.  Select Special Order.  You get caught in loop with the validate dialog constantly repeating.','Peter Ringering - 10/11/2014 04:47:26 PM - Code was creating a new Item instead of what the user chooses (which in this case was special order) when user tried to overwrite the item which caused double validation. Corrected ROW_SLUnspec so if the new item ID was 0 and new row type was item that the result is to cancel changes.  Else if new item ID was 0 and item text is empty, then we''ll convert it to an unspecific row.',NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
172,'E-172','2014-12-16 00:00:00.000',5,28,5,'2014-12-17 16:22:26.000',1,'Advanced Find crashes when load from chart definition chart bar is double clicked.  To reproduce, create a chart with a bar that is greater than 0.  Set it as the main chart.  Double click a chart bar.  Click Show Setup.  App crashes.','Peter Ringering - 07/24/2015 02:49:05 PM - QA Tested and Passed. 
Peter Ringering - 12/17/2014 04:22:26 PM - Fixed. Had to through a lot of code in the lookups and advanced find to make it so the scrollbar is disabled when the size of the lookup is bigger than the number of records and then when Show Setup is clicked, the scrollbar shows properly since there''s more data than the lookup is displaying.  Had the lookup start over when show/hide setup is clicked.  Changes affect advanced find and lookups.
',NULL,NULL,'2015-07-24 14:49:05.000',1,4,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
174,'E-174','2014-12-17 11:32:51.000',5,28,2,'2015-07-24 14:49:19.000',1,'Advanced Find does not have minimum size set.','Peter Ringering - 07/24/2015 02:49:25 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:49:19 PM - Fixed. 
',NULL,NULL,'2015-07-24 14:49:25.000',1,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
177,'E-175','2014-12-17 16:10:47.000',5,28,3,'2015-07-24 14:53:24.000',1,'Lookups don''t refresh in the same order as they were in when the user views a row on the fly and then closes the form.  To reproduce.  Go to Tools, Advanced Find.  Create a lookup that contains multiple columns and many records.  Click on Find Now.  Click on the column header of a different column to order by it.  Double click on the record.  Click cancel to get out  of the form.  Notice the order by went back to the first column.','Peter Ringering - 07/24/2015 02:53:28 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:53:24 PM - Fixed. 
',NULL,NULL,'2015-07-24 14:53:28.000',5,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
178,'E-178','2015-01-09 17:55:15.000',5,28,3,'2015-07-24 18:15:16.000',1,'Getting a SQL Error when user launches an add-on-the-fly form from Advanced Find and the filter has a row pointing to a table that is not the base table.  To reproduce, run SoftDevIS_Outstanding Issues.lkp.  Double click on a row and the Task Issues  form launches. Click on Next.  2 SQL errors display.  Happens in Access and SQL Server.','Peter Ringering - 07/24/2015 06:15:55 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 06:15:16 PM - Fixed.   See E-208 for more information.
',NULL,NULL,'2015-07-24 18:15:55.000',6,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
179,'E-179','2015-01-09 18:06:24.000',5,28,3,'2015-07-24 14:56:09.000',1,'When a chart bar is  double clicked (which brings up Advanced Find), then the user sets focus to another running program, then sets focus to Advanced Find form, then closes the Advanced Find form, focus does not go to the main form.','Peter Ringering - 07/24/2015 02:56:12 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:56:09 PM - Fixed. 
',NULL,NULL,'2015-07-24 14:56:12.000',6,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
180,'E-180','2015-01-13 20:28:13.000',5,28,2,'2015-07-24 14:59:40.000',1,'Null values in lookups are formatted which show misinformation.  To reproduce, goto Software DeveloperIS, Project Management, Tasks.  Click on Find.  Notice that null date valuesshow as 01/01/0001 12:00:00 AM.  They should show as blank.','Peter Ringering - 07/24/2015 02:59:42 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:59:40 PM - Fixed. 
',NULL,NULL,'2015-07-24 14:59:42.000',14,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
181,'E-181','2015-01-14 15:57:18.000',5,28,3,'2015-07-24 15:10:02.000',1,'When combo box shows the drop down box and then the user hits ESC, the form closes.  What should happen is when the user  hits ESC, the combo box drop down should close and the form stay active.','Peter Ringering - 07/24/2015 03:10:04 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 03:10:02 PM - Fixed. 
',NULL,NULL,'2015-07-24 15:10:04.000',14,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
182,'E-182','2015-01-14 16:03:34.000',5,30,2,'2015-02-04 17:44:58.000',1,'When the user closes any form in SoftDevIS, the chart does not refresh to show the changes.','David Matthews - 02/05/2015 05:45:56 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 05:44:58 PM - Fixed main form so it refreshes the chart every time form is closed.
',NULL,3,'2015-02-05 17:45:56.000',14,53,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
183,'E-183','2015-01-17 13:57:28.000',5,28,4,'2015-07-24 15:10:32.000',1,'Change the save routine in maintenance forms so the next record identity number comes back to the client in the same transaction that the header record is saved in.  That way if 2 people try to save a new record at the same time, they won''t get the same identity number.','Peter Ringering - 07/24/2015 03:10:33 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 03:10:32 PM - Fixed. 
',NULL,NULL,'2015-07-24 15:10:33.000',14,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
184,'E-184','2015-01-19 13:11:18.000',5,30,4,'2019-05-10 15:22:40.000',1,'Forms with a ID field and Description lookup control, don''t load after clicking tab.  To reproduce, in SoftDevIS, go to Error Status form, key in Pending Correction and hit tab. Nothing happens.  Hitting Save will save and  create duplicate records.','Peter Ringering - 05/15/2019 12:59:16 PM - QA Tested and Passed. 
Peter Ringering - 05/10/2019 03:22:40 PM - Fixed. 
Peter Ringering - 05/09/2019 04:34:04 PM - Fixed. Corrected code so if the description control has focus and it has a value and the form is in add mode and thwew user clicks save, then it will trigger the description field''s leave event and load up the form and then save.  Changes affect PTRFormsV2.dll and MaintManager.cpp.
',NULL,NULL,'2019-05-15 12:59:16.000',17,147,NULL,'2019-05-15 12:59:38.000','Peter Ringering',0.99);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
188,'E-188','2015-02-04 13:47:00.000',5,30,2,'2015-02-04 17:47:08.000',1,'QA Versions form.  The form should be widened to show all the record maintenance buttons.  Currently there are no icons showing and it looks really ugly.','Peter Ringering - 02/04/2015 06:02:37 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 05:47:08 PM - Adjusted form width.
',NULL,NULL,'2015-02-04 18:02:37.000',22,53,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
189,'E-189','2015-02-04 13:56:24.000',5,30,3,'2015-02-04 16:44:42.000',1,'Need a created datetime field added to the versions table and lookup.  It should be required and default to the current date & time.','Peter Ringering - 02/04/2015 04:45:13 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 04:44:42 PM - Fixed. Added date to form, db and lookup.
',NULL,NULL,'2015-02-04 16:45:13.000',22,49,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
190,'E-190','2015-02-04 15:22:08.000',5,28,3,'2015-02-04 17:51:12.000',1,'The bank dashboard graph and list do not update after closing the bank manager form.','Peter Ringering - 02/04/2015 06:00:46 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 05:51:12 PM - Fixed. 
',NULL,NULL,'2015-02-04 18:00:46.000',48,51,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
191,'E-191','2015-02-04 15:27:02.000',5,30,3,'2015-02-04 18:03:50.000',1,'The main chart does not update after closing the any form.','Peter Ringering - 02/04/2015 06:05:07 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 06:03:50 PM - Fixed. Overrode the ShowMDIForm to refdresh the chart after a form closes.
',NULL,NULL,'2015-02-04 18:05:07.000',49,53,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
192,'E-192','2015-02-04 17:33:17.000',5,30,3,'2015-07-24 15:15:44.000',1,'Versions form.  Clearing the date does not trigger the dirty flag.  To reproduce, retrieve an already saved version record.  Clear out the date and hit ESC.  No dirty  flag message.','Peter Ringering - 07/24/2015 03:15:46 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 03:15:44 PM - Fixed. 
',NULL,NULL,'2015-07-24 15:15:46.000',49,87,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
202,'E-202','2015-05-20 11:58:12.000',5,30,3,'2019-05-20 11:49:59.000',1,'Tasks, Assigned To.  Set default DB value to 0 and remove validation on save.','Peter Ringering - 05/20/2019 11:50:20 AM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 11:49:59 AM - Already fixed.
',NULL,NULL,'2019-05-20 11:50:20.000',65,148,NULL,'2019-05-20 11:50:22.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
196,'E-196','2015-02-20 15:38:38.000',5,30,3,'2019-05-21 13:47:31.000',1,'The Application column should be removed from the Testing Outlines Errors lookup control.','Peter Ringering - 05/22/2019 02:34:39 PM - QA Tested and Passed. 
Peter Ringering - 05/21/2019 01:47:31 PM - Fixed. Updated code so if Product changes, whatever is in the Testing Outline control is erased.  Changes affect frmErrors.css and RSDevLogix.exe.
Peter Ringering - 05/21/2019 12:25:00 PM - QA Fail. Changing the Product value should erase what''s in the Testing Outline control.  Should work like Version control where if you change the Product value, it updates the Found Version control.
Peter Ringering - 05/15/2019 12:51:40 PM - Fixed so that Testing Outline lookup is filtered for the error product like the version lookup.  Had to modify maintmanager so a form could override a control''s lookup definition.  Changes affect PTRFormsV2.dll, MaintManager.cpp, RSDevLogix.exe, and frmErrors.cs.
',NULL,NULL,'2019-05-22 14:34:39.000',53,149,4,'2019-05-22 14:34:42.000','Peter Ringering',2.59);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
198,'E-198','2015-02-21 14:43:15.000',5,30,4,'2019-05-21 16:07:37.000',1,'Errors form generates the error number before it is saved to the DB.  This will cause problems if 2 people try to save a new error at the same time.  What should happen is to set the new error number to a GUID, save to DB and get the new error ID at the same time, and then change the error number to be correct.','Peter Ringering - 05/22/2019 02:39:47 PM - QA Tested and Passed. 
Peter Ringering - 05/21/2019 04:07:37 PM - Fixed. Added functionality to MaintenanceManager to map the form lookup control to a class that implements IMaintControl interface.  Created error number class which implements the IMaintControl interface and mapped it to the error number field.  Had class set error number to a GUID in add mode and whatever is in the edit control in edit mode.  Changes affect PTRFormsV2.dll, MaintManager.cpp, RSDevLogix.exe, ErrorNumber.cpp and frmErroors.cpp.  Need to test all Error form functionality.
Peter Ringering - 05/21/2019 12:33:21 PM - QA Fail. Create a new error.  Keep Description blank.  Save.  Get validation error but Error Number shows GUID which looks ugly.
Peter Ringering - 05/15/2019 05:59:57 PM - Fixed. Corrected so error number is set to a GUID just before saving and then rename it to error number after.  Changes affect PTRDLLV2.dll, GblMethods.cpp, RSDevLogix.exe and frmErrors.cpp.
Peter Ringering - 05/10/2019 03:54:50 PM - QA Fail. 
Peter Ringering - 05/10/2019 03:54:39 PM - Fixed. 
',NULL,NULL,'2019-05-22 14:39:47.000',53,149,NULL,'2019-05-22 14:39:50.000','Peter Ringering',3.43);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
199,'E-199','2015-04-06 17:50:43.000',5,30,3,'2019-05-15 18:03:54.000',1,'Advanced Find does notvalidate to ensure filter left parentheses count equal right parentheses count.','Peter Ringering - 05/21/2019 12:42:05 PM - QA Tested and Passed. 
Peter Ringering - 05/15/2019 06:03:54 PM - Can no longer reproduce.
',NULL,NULL,'2019-05-21 12:42:05.000',56,148,NULL,'2019-05-21 12:42:11.000','Peter Ringering',0.11);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
200,'E-200','2015-05-16 12:43:56.000',2,28,3,NULL,1,'Bank Manager, Select a bank account with at least 1 transaction.  Put focus on last blank, disabled row, cleared checkbox cell.  Click on Save.  Notice critical, DataGridView, Error Dialog shows twice.',NULL,NULL,NULL,NULL,57,NULL,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
201,'E-201','2015-05-20 11:43:56.000',5,30,3,NULL,1,'Tasks Set default percent complete to 0 and remove validation on save.','Peter Ringering - 05/20/2019 11:47:29 AM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 11:47:52 AM - Already fixed.',NULL,NULL,'2019-05-20 11:47:29.000',65,NULL,NULL,'2019-05-20 11:48:57.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
203,'E-203','2015-05-20 12:01:06.000',5,30,3,'2019-05-20 11:53:38.000',1,'Errors, Details Tab, Description and Resolution textboxes.  Set Accepts Return to true.','Peter Ringering - 05/20/2019 11:54:01 AM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 11:53:38 AM - Already fixed.
',NULL,NULL,'2019-05-20 11:54:01.000',65,148,NULL,'2019-05-20 11:54:03.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
204,'E-204','2015-05-20 12:04:00.000',5,30,3,'2019-05-20 15:47:57.000',1,'Errors and Tools, Options.  Error Number prefix needs to be set by the user to something other than "E".','Peter Ringering - 05/21/2019 12:48:24 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 03:47:57 PM - Added functionality.  Changes affect RSDevLogix.exe, FrmOptions.cpp, DevLogixGlobals.cpp, DbConstants.cpp and FrmErrors.cpp.
',NULL,NULL,'2019-05-21 12:48:24.000',65,148,NULL,'2019-05-21 12:48:27.000','Peter Ringering',1.25);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
205,'E-205','2015-05-20 15:11:21.000',5,30,7,'2019-05-20 15:55:18.000',1,'Task Status, Tasl Priority, Task Issue Levels, Versions, Error Priority and Error Status forms need to be widened to show icons.  In add-on-the-fly mode,they currently just show captions in the top buttons with no icons. They look ugly.','Peter Ringering - 05/20/2019 03:55:38 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 03:55:18 PM - Already fixed.
',NULL,NULL,'2019-05-20 15:55:38.000',66,148,NULL,'2019-05-20 15:55:40.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
206,'E-206','2015-05-20 15:34:02.000',5,30,7,'2019-05-20 15:59:47.000',1,'Change the word "Application" to "Product" on all forms.','Peter Ringering - 05/20/2019 04:00:01 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 03:59:47 PM - Already fixed.
',NULL,NULL,'2019-05-20 16:00:01.000',66,148,NULL,'2019-05-20 16:00:03.000','Peter Ringering',0);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
208,'E-208','2015-06-11 13:00:15.000',5,28,5,'2015-07-24 17:58:46.000',1,'Maintenance form,  codes report, show report crashes after loading from advanced find and the filter is on sub table, non-key field.  To reproduce, run Family Bank IS.  Go to Advanced Find, Load RecurTemplatesAdv.lkp.  Click Find Now, then double click on a row to bring up maintenance form.  Click Print.  OK on code dialog and print setup dialog.  Notice the error.','Peter Ringering - 07/24/2015 06:06:38 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 05:58:46 PM - Fixed.   Added IsAdvancedFind property to lookup def.  Set to true in Adv. Find.  So when IsAdvFind is true, it will not set the form lookup instance.  Also, changed lookup control to not refresh on return in adv find mode.  That way user doesn''t loose place after launching the maintenance form. Changes affect PTRDLLV2.DLL and PTRFormsV2.DLL.
',NULL,NULL,'2015-07-24 18:06:38.000',79,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
209,'E-209','2015-07-02 10:43:26.000',5,28,3,'2015-07-24 18:14:35.000',1,'Launching recur template add on fly window from transaction grid, "Add New Recur Template?" messagebox, does not center the recur template window.  To reproduce, goto Bank Manager.  Select a valid bank account, go to the recurring template cell on the transactions grid.  Type in a new template.  Hit TAB. On the messagebox, click yes to create a new recur template.  Notice the recur template form is not centered to the bank acct manager form.','Peter Ringering - 07/24/2015 06:14:41 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 06:14:35 PM - Fixed. 
',NULL,NULL,'2015-07-24 18:14:41.000',79,85,NULL,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
210,'E-210','2015-07-26 21:50:19.000',2,28,5,NULL,1,'Create a recurring template with Recurs Every set  to 999,999,999 months and setting date to be today.  Now go into Bank Manager for the bank.  Click Generate From Recurring.  Get Unhandled Exception.',NULL,NULL,NULL,NULL,85,NULL,10,NULL,NULL,NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
212,'E-212','2015-08-01 14:48:09.000',5,28,4,'2015-10-08 16:05:21.000',1,'Bank Manager, Transactions Grid, Recurring Template column.  When you type in some text that is not in the database, then click on the Bank Account control, the Create New Recurring Template messagebox shows.  Click on No to not create.  The invalid value stays in the lookup textbox.  Click Save and the row is not saved.  In code, when you key in an invalid value, it saves first, then validates.  Otherwise, if after you enter an invalid value and hit TAB, it erases the text.','Peter Ringering - 10/08/2015 04:05:37 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 04:05:21 PM - Fixed awhile ago.
',NULL,NULL,'2015-10-08 16:05:37.000',93,130,NULL,'2015-10-08 16:05:43.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
216,'E-213','2015-10-08 14:07:17.000',5,29,3,'2015-10-08 16:29:31.000',1,'When you set a new record in Add/Edit Items, the Category box defaults to "1".','Peter Ringering - 10/08/2015 04:34:26 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 04:29:31 PM - Fixed. Added DBDeffaultValue property to DBFieldDef so if is not empty, it will use that value to put in non-null fields during conversion, otherwise, it uses default value.  Changes affect PTRDLLV2.dll.
',NULL,NULL,'2015-10-08 16:34:26.000',131,131,23,'2015-10-08 16:34:29.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
217,'E-217','2015-10-08 14:34:19.000',5,29,3,'2015-10-08 17:46:16.000',1,'When the user launches the item lookup on the last row and selects a record, the value is not returned.','Peter Ringering - 10/08/2015 05:55:07 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 05:46:16 PM - Fixed PTRGrid.RecalcIndex so it only refreshes the edit control if the current column is the index column and the edit control is showing.  Code was refreshing the edit anytime recalcindex was called.  This would  cause new value to match old value and prevented DataGridView to not push the value to the row object.  Changes affect PTRGrid.
',NULL,NULL,'2015-10-08 17:55:07.000',131,131,27,'2015-10-08 17:55:11.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
218,'E-218','2015-10-08 14:52:39.000',5,29,3,'2015-10-08 19:02:15.000',1,'Add/Edit Shopping Lists: User can drag/drop last empty row.  This should not be allowed.','Peter Ringering - 10/08/2015 07:03:27 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 07:02:15 PM - Fixed so that last row cannot be moved unless the grtid is a fixed grid.  Changes affect PTRGrid.
',NULL,NULL,'2015-10-08 19:03:27.000',131,131,27,'2015-10-08 19:03:31.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
219,'E-219','2015-10-08 14:58:09.000',5,29,3,'2015-10-08 21:08:26.000',1,'Add/Edit Shopping Lists:  When the user launches the context menu on the last empty row and the grid has other rows, Clear Grid is disabled.','Peter Ringering - 10/08/2015 09:16:47 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 09:08:26 PM - Code was disabling clear grid when delete row was disabled Fixed PTRGrid so that clear grid is enabled when there is more than 1 row or in fixed grid mode.  Also fixed so in fixed grid mode it removes all rows from the data source.
',NULL,NULL,'2015-10-08 21:16:47.000',131,131,27,'2015-10-08 21:16:50.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
220,'E-220','2015-10-08 15:06:52.000',5,29,3,'2015-10-08 21:30:10.000',1,'Prepare New Grocery Shopping List.  Tab order is incorrect.  Tabs from Close to Select All checkbox then to Help button.','Peter Ringering - 10/08/2015 09:32:01 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 09:30:10 PM - Fixed tab order.  Also corrected error where ENTER to TAB works on any form that doesn''t have an accept button.
',NULL,NULL,'2015-10-08 21:32:01.000',131,131,21,'2015-10-08 21:32:08.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
221,'E-221','2015-10-08 15:54:19.000',5,29,3,'2015-10-08 21:43:07.000',1,'Tools/Options:  Accept button not set.','Peter Ringering - 10/08/2015 09:43:14 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 09:43:07 PM - Fixed. 
',NULL,NULL,'2015-10-08 21:43:14.000',131,131,28,'2015-10-08 21:43:17.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
222,'E-222','2015-10-08 15:58:24.000',5,29,3,'2015-10-08 21:43:53.000',1,'Tools/Options:  Sales Tax has no max value set.','Peter Ringering - 10/08/2015 09:44:03 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 09:43:53 PM - Fixed. 
',NULL,NULL,'2015-10-08 21:44:03.000',131,131,28,'2015-10-08 21:44:06.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
223,'E-223','2015-10-11 10:30:26.000',5,29,3,'2015-10-11 17:17:44.000',1,'When a lookup textbox has focus and all its content is selected, when the backspace key is pressed, nothing happens.  It should erase all the selected text.','Peter Ringering - 10/11/2015 05:19:59 PM - QA Tested and Passed. 
Peter Ringering - 10/11/2015 05:17:44 PM - Fixed. PTRFormsV2.dll.CTLDBLookupText.  This and many other errors started when the autofill box was introduced.  There are many different scenarios where this would fail.  Corrected them all.
',NULL,NULL,'2015-10-11 17:19:59.000',131,131,NULL,'2015-10-11 17:20:03.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
224,'E-224','2015-10-11 22:04:37.000',5,29,3,'2015-10-15 12:18:55.000',1,'Advanced Find - Add 3 columns.  On the columns grid, drag the last row and drop it on the first row.  Get duplicate column message and a critical error dialog.','Peter Ringering - 10/15/2015 12:23:33 PM - QA Tested and Passed. 
Peter Ringering - 10/15/2015 12:18:55 PM - Fixed. Put code in OnCellValidating override so it doesn''t validate in the middle of drag/drop operations.
',NULL,NULL,'2015-10-15 12:23:33.000',131,131,NULL,'2015-10-15 12:23:39.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
226,'E-226','2015-10-11 22:17:27.000',1,29,3,NULL,1,'Advanced Find.  Add 3 columns but do not click on Find Now.  In the list control, click on the 2nd column header to sort.  Note that the sort arrow stays on the first column.  Click Find Now and then sort on any column.  The sort arrow still stays on the first column.',NULL,NULL,NULL,NULL,131,NULL,NULL,'2016-06-25 12:16:26.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
227,'E-227','2018-10-16 14:39:10.000',5,30,4,'2019-05-20 16:16:08.000',1,'User login.  When you key in a user name and then press ENTER, it tries to save the user name right away.  It should go to the next control on the form.  To reproduce, start the app, type in part of a username that already exists in the database.  DO NOT PRESS TAB.  Press ENTER, notice, it saves the partial username.  It should login the selected username in the dropdown box.','Peter Ringering - 05/20/2019 04:16:23 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 04:16:08 PM - Already fixed.
',NULL,NULL,'2019-05-20 16:16:23.000',139,148,NULL,'2019-05-20 16:16:25.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
228,'E-228','2018-10-22 16:04:12.000',5,30,3,'2019-05-20 16:09:07.000',1,'All maintenance forms.  The "Save" button should always be enabled.','Peter Ringering - 05/20/2019 04:09:25 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 04:09:07 PM - Already fixed.
',NULL,NULL,'2019-05-20 16:09:25.000',139,148,NULL,'2019-05-20 16:09:27.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
229,'E-229','2018-10-22 16:12:45.000',5,30,3,'2019-05-21 16:28:27.000',1,'All maintenance forms.  Need to warn user when renaming table description values.','Peter Ringering - 05/22/2019 02:44:03 PM - QA Tested and Passed. 
Peter Ringering - 05/21/2019 04:28:27 PM - Fixed form record names.  Changes affect RSDevLogix.exe, PTRFormsV2.dll and PTRMaintForm.cpp.
Peter Ringering - 05/21/2019 01:06:50 PM - QA Fail. 
The following form''s unique error message show''s "*s''s" instead of "*''s."
* Testing Outlines
* Testing Templates
* Errors
* Advanced Find
* Chart Definition
Peter Ringering - 05/20/2019 04:09:40 PM - Already fixed.
',NULL,NULL,'2019-05-22 14:44:03.000',139,149,NULL,'2019-05-22 14:44:06.000','Peter Ringering',0.96);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
231,'E-231','2018-12-10 12:01:24.000',5,30,3,'2019-05-20 16:10:33.000',1,'Advanced Find, percent field filters.  Needs to show percent sign.  To reproduce, goto Project Management, Projects.  Select a project with tasks.  Click the "Tasks" tab.  Click "Advanced".  Select the "Percent Complete" field.  Click "Add Filter".  Notice the % sign is missing.  Put in "Less Than 100".  Click OK.  Notice, it shows all records including those that are 100% complete.','Peter Ringering - 05/21/2019 01:37:03 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 04:10:33 PM - Already ixed.
',NULL,NULL,'2019-05-21 13:37:03.000',139,148,NULL,'2019-05-21 13:37:05.000','Peter Ringering',0.06);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
232,'E-232','2019-02-20 14:11:01.000',5,30,3,'2019-05-22 13:37:58.000',1,'Tools, Options, Holidays grid.  When you change a holiday date value, then click OK.  It saves the data then brings up the dirty flag message.','Peter Ringering - 05/22/2019 02:46:51 PM - QA Tested and Passed. 
Peter Ringering - 05/22/2019 01:37:58 PM - Fixed. Set dirty flag to false after a successful save.  Changes affect RSDevLogix.exe and FrmOptions.cpp.
',NULL,NULL,'2019-05-22 14:46:51.000',139,149,NULL,'2019-05-22 14:46:53.000','Peter Ringering',0.22);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
233,'E-233','2019-05-04 13:09:37.000',5,30,3,'2019-05-22 13:58:11.000',1,'Lookups.  Clicking on ''Add'' on an existing record, then clicking ''New'' trips the dirty flag message.  To reproduce, goto DevLogix, Quality Assurance, Add/Edit Errors.  Bring up an existing error.  Go to the Status field.  Do a lookup.  Click ''Add''.  Click ''New''  Notice the dirty flag message eventhough nothing has changed.','Peter Ringering - 05/22/2019 02:52:17 PM - QA Tested and Passed. 
Peter Ringering - 05/22/2019 01:58:11 PM - Fixed. Corrected code so that when the user launches a maintenance form (like Error Status) by clicking Add from the lookup form, to only raise the dirty flag if the record doesn''t exist.  Changes affect all maintenance forms.  Changes affect PTRFormsV2.dll and PTRMaintForm.cpp.
',NULL,NULL,'2019-05-22 14:52:17.000',145,149,NULL,'2019-05-22 14:52:31.000','Peter Ringering',0.39);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
234,'E-234','2019-05-04 13:27:50.000',1,30,3,NULL,1,'Chart Definition.  Changing an existing Advanced Find, does not update the Caption value.  To reproduce, go to Tools, Chart Definition.  Bring up an existing chart.  Change an Advanced Find and press Tab.  Notice the caption doesn''t change.',NULL,NULL,NULL,NULL,145,NULL,NULL,'2019-05-04 13:32:51.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
237,'E-237','2019-05-04 14:00:00.000',5,30,3,'2019-05-04 15:53:25.000',1,'Advanced Find.  Percent fields.  Need to add Percent control when filtering percent fields.  To reproduce, go to DevLogix, Tools, Advanced Find.  Select ''Testing Outlines'' table.  Select ''Percent Complete'' field.  Click Add Filter.  Notice Search Value show "0".  It should show 0%.','Peter Ringering - 05/07/2019 02:11:10 PM - QA Tested and Passed. 
Peter Ringering - 05/04/2019 03:53:25 PM - Fixed. Added PTRPercentTextBox to FrmAdvFindFilterDef form.  Had to fix a small error with currency and numeric searches on <> type searches.  Changes affect PTRFormsV2.dll, FrmAdvFindFilterDef.cs and PTRPercentTextBox.cs.
',NULL,1,'2019-05-07 14:11:10.000',145,146,NULL,'2019-05-07 14:11:21.000','Peter Ringering',0.83);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
239,'E-239','2019-05-04 14:21:36.000',5,30,3,'2019-05-08 10:54:00.000',1,'Advanced Find.  Filtering on an Advanced Find definition with no filters and then adding another filter does not filter.  To reproduce, go to DevLogix, Tools, Advanced Find.  Select ''Testing Outlines'' table.  Click Load Default.  In Fields list, Select <Advanced Find Filter>.  Click ''Add Filter''.  Do a lookup and click ''Add''.  Click ''Load Default''.  Type in a Title and click Save/Select.  In the derived Advanced Find Goto ''Product'' column.  Click Add Filter.  Search Type is ''=''.  Type in a product to search for.  Click OK.  Notice it does not search for that product.','Peter Ringering - 05/15/2019 01:05:08 PM - QA Tested and Passed. 
Peter Ringering - 05/08/2019 10:54:00 AM - Fixed.  Added validation so Advanced Find won''t run if an Advanced Find filter has no filters defined.  Also fixed error that was allowing delete of an Advanced Find record that is an Advanced Find Filter of another Advanced Find.  Changes affect PTRFormsV2.dll and PTRDLLV2.dll and multiple .css files.
Peter Ringering - 05/07/2019 02:35:00 PM - QA Fail. Adding a no filter advanced find after an existing filter returns a SQL Execution Error.  To reproduce, follow steps in Description but put in the no filter advanced find definition AFTER the search for Product filter.
Peter Ringering - 05/04/2019 02:50:30 PM - Fixed. Code was returning an empty query if the Advanced Find filter was empty.  Corrected so it would go to next filter in this situation.  Changes affect PTRDDLLV2.DLL, DbFilterDef.cs.
',NULL,NULL,'2019-05-15 13:05:08.000',145,147,NULL,'2019-05-15 13:05:30.000','Peter Ringering',0.62);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
240,'E-240','2019-05-04 16:04:25.000',1,30,4,NULL,1,'Task Punch In/Out.  Selecting a task, punching in, and then deleting causes orphan time clock record.  To reproduce, goto Project Management, Add/Edit Tasks.  Create a task and save.  Punch In.  Delete.  Then punch out.  Time clock record is lost.',NULL,NULL,NULL,NULL,146,NULL,NULL,'2019-05-04 16:11:33.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
241,'E-241','2019-05-07 14:13:19.000',5,30,3,'2019-05-22 14:21:59.000',1,'Advanced Find.  The lookup doesn''t refresh after modifying a found record.  To reproduce, goto Tools, Advanced Find.  Select the Errors Table.  Click Load Default.  Select Status and click Add Filter.  Set it to = Pending Correction.  Go to the lookup and modify a record.  Change the status to Open.  Click Save and then close the form.  Notice the lookup does not refresh.','Peter Ringering - 05/22/2019 02:52:54 PM - QA Tested and Passed. 
Peter Ringering - 05/22/2019 02:21:59 PM - Fixed. Error was introduced in fix for E-208.  Corrected so advanced find refreshes when maintenance form closes.  Changes affect PTRForrmsV2.dll and CtlDbLookup.cs.',NULL,NULL,'2019-05-22 14:52:54.000',147,149,NULL,'2019-05-22 14:52:56.000','Peter Ringering',0.22);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
243,'E-243','2019-05-09 14:46:52.000',5,30,3,'2019-05-09 15:21:39.000',1,'Tab key doesn''t work after Alt + Tab to switch to another program then alt tab back to main form.  To reproduce, launch DevLogix.  Press Alt + Tab to switch to another program.  Press Alt + Tab to switch back to DevLogix.  Press tab key and notice nothing happens.  Press tab again and the File menu is highlighted.  You should not have to press tab twice in this scenario to get the menu bar highlighted.','Peter Ringering - 05/09/2019 03:23:30 PM - QA Tested and Passed. 
Peter Ringering - 05/09/2019 03:21:39 PM - This is happening on MS Word and Outlook as well.  This is a computer problem and not an error specific to the DevLogix application.
',NULL,NULL,'2019-05-09 15:23:30.000',147,147,NULL,'2019-05-09 15:24:51.000','Peter Ringering',0.5);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
254,'E-254','2019-05-21 12:50:36.000',1,30,3,NULL,1,'Tools, Options.  Changing a value then saving and exiting app causes main chart to go back to icons.  To reproduce, set the Dashboard item to be Custom Chart.  Then exit app and go back in.  Goto Tools, Options.  Change Error Prefix to be something different.  Click OK.  Exit app.  Go back in and notice the main chart is gone and it''s back to icons.',NULL,NULL,NULL,NULL,149,NULL,NULL,'2019-05-21 15:14:27.000','Peter Ringering',NULL);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
269,'E-269','2019-06-01 12:29:09.000',8,30,3,'2019-06-01 12:58:56.000',1,'Advanced Find.  Lookup Definitions with formula columns don''t print the results of a formula.  To reproduce, go to Errors.  Select an error that has Time Clock entries.  On The Time Clock tab, click Advanced.  Click Print.  Notice the Hours Spent column is blank.','Peter Ringering - 06/01/2019 12:58:56 PM - Fixed. Added formula columns to report object.  Changes affect PTRFormsV2.dll and FrmAdvancedFind.cpp.
',NULL,NULL,NULL,151,151,NULL,'2019-06-01 13:00:26.000','Peter Ringering',0.45);
INSERT INTO [TB_Errors] ([intErrorID],[strErrorNo],[dteDate],[intStatusID],[intProductID],[intPriorityID],[dteFixedDate],[intAssignedToID],[txtDescription],[txtResolution],[decEstHrs],[intTesterID],[dteCompletedDate],[intFoundVersionID],[intFixedVersionID],[intOutlineID],[dteModifiedDate],[strModifiedBy],[decHrsSpent]) VALUES (
270,'E-270','2019-08-06 15:38:48.000',1,30,3,NULL,1,'Tasks form.  When viewing a task from the Project Tasks control, the Remaining Hours control is 0.  To reproduce, goto Project Management, Projects window.  Click Find and select a project that has tasks.  Click on the Tasks tab.  Select a task that has Remaining Hours <> 0.  When the Task window loads up, notice the Remaining Hours is 0.',NULL,NULL,NULL,NULL,151,NULL,NULL,'2019-08-06 15:46:33.000','Peter Ringering',NULL);
INSERT INTO [TB_ErrorPriorities] ([intPriorityID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Design',1,'2015-09-19 14:31:58.000','Peter Ringering');
INSERT INTO [TB_ErrorPriorities] ([intPriorityID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'Cosmetic',2,'2015-09-19 15:47:55.000','Peter Ringering');
INSERT INTO [TB_ErrorPriorities] ([intPriorityID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
3,'Procedural',3,NULL,NULL);
INSERT INTO [TB_ErrorPriorities] ([intPriorityID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
4,'Data Corruption',4,NULL,NULL);
INSERT INTO [TB_ErrorPriorities] ([intPriorityID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
5,'Fatal - Application Crash',5,NULL,NULL);
INSERT INTO [TB_ErrorPriorities] ([intPriorityID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
6,'Urgent Design',5,NULL,NULL);
INSERT INTO [TB_ErrorPriorities] ([intPriorityID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
7,'Urgent Cosmetic',5,NULL,NULL);
INSERT INTO [TB_ErrorPriorities] ([intPriorityID],[strDescription],[intLevelNo],[dteModifiedDate],[strModifiedBy]) VALUES (
8,'Urgent Procedural',5,NULL,NULL);
INSERT INTO [TB_Charts] ([intChartID],[strTitle],[intRefreshRate],[strXAxisTitle],[strYAxisTitle],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'zTest',1,'a','b','2018-11-24 18:21:31.000','Peter Ringering');
INSERT INTO [TB_Charts] ([intChartID],[strTitle],[intRefreshRate],[strXAxisTitle],[strYAxisTitle],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'Peter''s To Do',5,NULL,NULL,'2019-05-07 14:09:52.000','Peter Ringering');
INSERT INTO [TB_ChartBars] ([intChartID],[intChartBarID],[intAdvFindId],[strCaption],[bolUseFlag],[bytFlagType],[intRedFlagLevel],[intYellowFlagLevel]) VALUES (
1,0,1,'Errors',1,3,30,15);
INSERT INTO [TB_ChartBars] ([intChartID],[intChartBarID],[intAdvFindId],[strCaption],[bolUseFlag],[bytFlagType],[intRedFlagLevel],[intYellowFlagLevel]) VALUES (
1,1,13,'Tasks',0,3,0,0);
INSERT INTO [TB_ChartBars] ([intChartID],[intChartBarID],[intAdvFindId],[strCaption],[bolUseFlag],[bytFlagType],[intRedFlagLevel],[intYellowFlagLevel]) VALUES (
1,2,7,'zTest',0,3,0,0);
INSERT INTO [TB_ChartBars] ([intChartID],[intChartBarID],[intAdvFindId],[strCaption],[bolUseFlag],[bytFlagType],[intRedFlagLevel],[intYellowFlagLevel]) VALUES (
2,0,18,'DevLogix Unfixed Errors',0,3,0,0);
INSERT INTO [TB_ChartBars] ([intChartID],[intChartBarID],[intAdvFindId],[strCaption],[bolUseFlag],[bytFlagType],[intRedFlagLevel],[intYellowFlagLevel]) VALUES (
2,1,22,'DevLogix Utested Errors',0,3,0,0);
INSERT INTO [TB_ChartBars] ([intChartID],[intChartBarID],[intAdvFindId],[strCaption],[bolUseFlag],[bytFlagType],[intRedFlagLevel],[intYellowFlagLevel]) VALUES (
2,2,20,'DevLogix Incomplete Testing Outlines',0,3,0,0);
INSERT INTO [TB_ChartBars] ([intChartID],[intChartBarID],[intAdvFindId],[strCaption],[bolUseFlag],[bytFlagType],[intRedFlagLevel],[intYellowFlagLevel]) VALUES (
2,3,23,'DevLogix Incomplete Tasks',0,3,0,0);
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
1,'Errors','TB_Errors','Errors','2018-11-17 15:17:55.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
2,'Task Issues','TB_Issues','Task Issues','2018-10-31 15:54:57.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
7,'zTest','TB_Issues','Task Issues','2018-11-23 20:58:54.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
8,'Goal Details Test','TB_GoalDetails','User Goal Details','2018-11-04 15:17:27.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
9,'zTestErrors Fixed By','TB_ErrorsFixedBy','Errors Fixed By','2018-11-04 15:29:15.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
10,'Unfixed Errors','TB_Errors','Errors','2018-11-05 13:51:47.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
11,'Becca''s Unfixed','TB_Errors','Errors','2018-11-08 13:26:32.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
12,'All Unfixed','TB_Errors','Errors','2018-11-08 13:20:00.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
13,'Tasks','TB_Tasks','Tasks','2019-05-15 13:04:36.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
14,'Peter''s Testing Outlines','TB_TestingOutlines','Testing Outlines','2018-11-26 12:45:14.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
16,'Time Clock','TB_Timeclock','Timeclock','2019-04-19 13:20:50.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
18,'DevLogix Unfixed Errors','TB_Errors','Errors','2019-05-22 14:25:02.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
19,'Incomplete Testing Outlines','TB_TestingOutlines','Testing Outlines','2019-05-04 15:51:00.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
20,'DevLogix Incomplete Testing Outlines','TB_TestingOutlines','Testing Outlines','2019-05-07 14:05:56.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
21,'Untested Errors','TB_Errors','Errors','2019-05-07 13:55:27.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
22,'DevLogix Utested Errors','TB_Errors','Errors','2019-05-10 15:13:38.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
23,'DevLogix Incomplete Tasks','TB_Tasks','Tasks','2019-05-22 13:16:22.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
24,'No Filter','TB_Tasks','Tasks','2019-05-15 13:04:24.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
26,'No Filter1','TB_Tasks','Tasks','2019-05-15 13:03:14.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
28,'User Time Clock','TB_Timeclock','Timeclock','2019-05-31 11:51:57.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
29,'E-269','TB_Timeclock','Timeclock','2019-06-01 12:46:20.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
30,'RSDbLookup Incomplete Tasks','TB_Tasks','Tasks','2019-08-19 17:26:13.000','Peter Ringering');
INSERT INTO [TB_AdvFinds] ([intAdvFindId],[strDescription],[strTableID],[strTableDesc],[dteModifiedDate],[strModifiedBy]) VALUES (
31,'Unresolved Issues','TB_Issues','Task Issues','2019-08-19 20:10:59.000','Peter Ringering');
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
1,0,0,'TB_Errors','strErrorNo',6,'E','E',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
1,1,0,NULL,NULL,0,NULL,'Unfixed Errors',NULL,10,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
7,0,0,'TB_Tasks','intProjectID',2,'d','d',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
7,1,1,'TB_Issues','intIssueLevelID',6,'Mil','Mil',NULL,NULL,0,2,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
7,2,0,'TB_Issues','intTaskID',2,'b','b',NULL,NULL,0,2,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
7,3,0,'TB_Tasks','intPriorityID',2,'q','q',NULL,NULL,0,1,1);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
7,4,0,'TB_Issues','intIssueLevelID',0,'2','Milestone 01',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
9,0,0,'TB_ErrorsFixedBy','intUserID',2,'C','C',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
9,1,0,'TB_ErrorsFixedBy','intErrorID',2,'E-150','E-150',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
9,2,0,'TB_Errors','intProductID',2,'d','d',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
9,3,0,'TB_Errors','intFixedVersionID',2,'2.00.0035','2.00.0035',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
10,0,0,'TB_Errors','intStatusID',0,'2','Open',NULL,NULL,0,2,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
10,1,0,'TB_Errors','intStatusID',0,'1','Pending Correction',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
11,0,0,'TB_Errors','intAssignedToID',0,'10','Becca Smith',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
11,1,0,NULL,NULL,0,NULL,NULL,NULL,10,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
12,0,0,NULL,NULL,0,NULL,NULL,NULL,10,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
14,0,0,'TB_TestingOutlines','intAssignedToID',0,'1','Peter Ringering',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
19,0,0,'TB_TestingOutlines','decPercentComplete',4,'1','100 %',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
20,0,0,NULL,NULL,0,NULL,'Incomplete Testing Outlines',NULL,19,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
20,1,0,'TB_TestingOutlines','intProductID',0,'30','DevLogix',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
21,0,0,'TB_Errors','intStatusID',0,'8','Pending QA Test',NULL,NULL,0,2,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
21,1,0,'TB_Errors','intStatusID',0,'3','Pending Unit Test',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
22,0,0,NULL,NULL,0,NULL,'Untested Errors',NULL,21,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
22,1,0,'TB_Errors','intProductID',0,'30','DevLogix',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
13,0,0,NULL,NULL,0,NULL,'Remaining Hours > 0','([TB_Tasks].[decEstHrs] - [TB_Tasks].[decHrsSpent]) > 0',NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
13,1,0,'TB_Tasks','intAssignedToID',0,'1','Peter Ringering',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
23,0,0,'TB_Tasks','intProjectID',0,'16','DevLogix',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
23,1,0,'TB_Tasks','decPercentComplete',4,'1','100 %',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
18,0,0,NULL,NULL,0,NULL,'Unfixed Errors',NULL,10,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
18,1,0,'TB_Errors','intProductID',0,'30','DevLogix',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
30,0,0,'TB_Tasks','intProjectID',0,'21','RSDbLookup',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
30,1,0,'TB_Tasks','decPercentComplete',4,'1','100 %',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindFilters] ([intAdvFindId],[intAdvFindFilterID],[intLeftParentheses],[strTableID],[strFieldID],[bytOperand],[strSearchValue],[strDisplayValue],[strFormula],[intSearchValueAdvFindID],[bolCustomDate],[bytEndLogic],[intRightParentheses]) VALUES (
31,1,0,'TB_Issues','bolResolved',0,'0','False',NULL,NULL,0,1,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
1,0,'TB_Errors','strErrorNo','Error Number',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
1,1,'TB_Errors','intAssignedToID','Assigned Developer',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
1,2,'TB_Errors','dteDate','Date',0.2,NULL,NULL,2,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
1,3,'TB_Errors','intFoundVersionID','Found Version',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
1,4,'TB_Errors','intStatusID','Status',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
2,0,'TB_Issues','strIssueDesc','Issue Name',0.2,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
2,1,'TB_Tasks','strTaskDesc','Task Name',0.2,'intTaskID',NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
2,2,'TB_Tasks','intAssignedToID','Assigned User',0.2,'intTaskID',NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
2,3,'TB_Projects','strProject','Project Name',0.2,'intProjectID',NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
7,0,'TB_Issues','intIssueLevelID','Issue Level',0.2,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
7,1,'TB_Issues','intTaskID','Task',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
7,2,'TB_Tasks','intPriorityID','Priority',0.2,'intTaskID',NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
7,3,'TB_Tasks','intProjectID','Project',0.2,'intTaskID',NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
8,0,'TB_GoalDetails','intGoalID','Goal',0.2,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
8,1,'TB_Goals','intUserID','User',0.2,'intGoalID',NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
8,2,'TB_GoalDetails','intErrorID','Error',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
9,0,'TB_ErrorsFixedBy','intUserID','User',0.2,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
9,1,'TB_ErrorsFixedBy','intErrorID','Error',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
9,2,'TB_Errors','intProductID','Product',0.2,'intErrorID',NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
9,3,'TB_Errors','intFixedVersionID','Fixed Version',0.2,'intErrorID',NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
10,0,'TB_Errors','strErrorNo','Error Number',0.2,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
10,1,'TB_Errors','intPriorityID','Priority',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
10,2,'TB_Errors','intProductID','Product',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
10,3,'TB_Errors','intStatusID','Status',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
11,0,'TB_Errors','strErrorNo','Error Number',0.2,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
11,1,'TB_Errors','intStatusID','Status',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
11,2,'TB_Errors','intPriorityID','Priority',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
11,3,'TB_Errors','intAssignedToID','Assigned Developer',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
12,0,'TB_Errors','strErrorNo','Error Number',0.2,NULL,NULL,1,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
12,1,'TB_Errors','intAssignedToID','Assigned Developer',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
12,2,'TB_Errors','intStatusID','Status',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
12,3,'TB_Errors','intPriorityID','Priority',0.2,NULL,NULL,0,NULL,NULL);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
14,0,'TB_TestingOutlines','strName','Outline Name',0.4,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
14,1,'TB_TestingOutlines','intProductID','Product',0.3,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
13,0,NULL,NULL,'Remaining Hours',0.2,NULL,NULL,2,'[TB_Tasks].[decEstHrs] - [TB_Tasks].[decHrsSpent]',1);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
13,1,'TB_Tasks','strTaskDesc','Name',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
13,2,'TB_Tasks','intProjectID','Project',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
13,3,NULL,NULL,'Formula2',0.2,NULL,NULL,0,'[TB_Tasks].[decEstHrs] - [TB_Tasks].[decHrsSpent]',0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
16,0,'TB_Timeclock','intUserID','User',0.25,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
16,1,'TB_Timeclock','dtePunchIn','Punch In',0.25,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
16,2,'TB_Timeclock','dtePunchOut','Punch Out',0.25,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
16,3,NULL,NULL,'Hours Spent',0.25,NULL,NULL,0,'ROUND(DATEDIFF("n", [TB_Timeclock].[dtePunchIn], [TB_Timeclock].[dtePunchOut]) / 60, 2)',1);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
19,0,'TB_TestingOutlines','strName','Outline Name',0.3,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
19,1,'TB_TestingOutlines','intProductID','Product',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
19,2,'TB_TestingOutlines','intAssignedToID','Assigned To',0.3,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
19,3,'TB_TestingOutlines','decPercentComplete','Percent Complete',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
20,0,'TB_TestingOutlines','strName','Outline Name',0.4,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
20,1,'TB_TestingOutlines','intProductID','Product',0.3,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
20,2,'TB_TestingOutlines','intAssignedToID','Assigned To',0.3,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
21,0,'TB_Errors','intErrorID','Error ID',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
21,1,'TB_Errors','strErrorNo','Error Number',0.2,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
21,2,'TB_Errors','intProductID','Product',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
21,3,'TB_Errors','intStatusID','Status',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
21,4,'TB_Errors','intPriorityID','Priority',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
22,0,'TB_Errors','strErrorNo','Error Number',0.2,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
22,1,'TB_Errors','intPriorityID','Priority',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
22,2,'TB_Errors','intTesterID','Assigned Tester',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
22,3,'TB_Errors','txtDescription','Description',0.4,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
26,0,'TB_Tasks','strTaskDesc','Name',0.4,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
26,1,'TB_Tasks','dteDueDate','Due Date',0.15,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
26,2,'TB_Tasks','decPercentComplete','% Complete',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
26,3,NULL,NULL,'Hours Left',0.25,NULL,NULL,0,'IIF(IsNull([TB_Tasks].[decEstHrs]), 0, [TB_Tasks].[decEstHrs]) - IIF(IsNull([TB_Tasks].[decHrsSpent]), 0, [TB_Tasks].[decHrsSpent])',1);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
24,0,'TB_Tasks','strTaskDesc','Name',0.4,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
24,1,'TB_Tasks','dteDueDate','Due Date',0.15,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
24,2,'TB_Tasks','decPercentComplete','% Complete',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
24,3,NULL,NULL,'Hours Left',0.25,NULL,NULL,0,'IIF(IsNull([TB_Tasks].[decEstHrs]), 0, [TB_Tasks].[decEstHrs]) - IIF(IsNull([TB_Tasks].[decHrsSpent]), 0, [TB_Tasks].[decHrsSpent])',1);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
23,0,'TB_Tasks','strTaskDesc','Name',0.4,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
23,1,'TB_Tasks','dteDueDate','Due Date',0.15,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
23,2,'TB_Tasks','decPercentComplete','% Complete',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
23,3,NULL,NULL,'Hours Left',0.25,NULL,NULL,0,'IIF(IsNull([TB_Tasks].[decEstHrs]), 0, [TB_Tasks].[decEstHrs]) - IIF(IsNull([TB_Tasks].[decHrsSpent]), 0, [TB_Tasks].[decHrsSpent])',1);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
18,0,'TB_Errors','strErrorNo','Error Number',0.2,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
18,1,'TB_Errors','intStatusID','Status',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
18,2,'TB_Errors','intPriorityID','Priority',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
18,3,'TB_Errors','txtDescription','Description',0.4,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
28,0,'TB_Timeclock','dtePunchIn','Punch In Date',0.2,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
28,1,'TB_Timeclock','dtePunchOut','Punch Out Date',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
28,2,'TB_Timeclock','bytType','Punch Type',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
28,3,NULL,NULL,'Code',0.2,NULL,NULL,0,'IIF([TB_Timeclock].[bytType] = 0, [TB_Tasks_intTaskID].[strTaskDesc]
  , IIF([TB_Timeclock].[bytType] = 1, [TB_TestingOutlines_intOutlineID].[strName]
  , IIF([TB_Timeclock].[bytType] = 2, [TB_Errors_intErrorID].[strErrorNo])))',0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
28,4,NULL,NULL,'Hours Spent',0.2,NULL,NULL,0,'ROUND(DATEDIFF("n", [TB_Timeclock].[dtePunchIn], [TB_Timeclock].[dtePunchOut]) / 60, 2)',1);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
29,0,'TB_Timeclock','dtePunchIn','Punch In',0.25,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
29,1,'TB_Timeclock','dtePunchOut','Punch Out',0.25,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
29,2,'TB_Timeclock','bytType','Type',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
29,3,NULL,NULL,'Code',0.15,NULL,NULL,0,'IIF([TB_Timeclock].[bytType] = 0, [TB_Tasks_intTaskID].[strTaskDesc]
, IIF([TB_Timeclock].[bytType] = 1, [TB_TestingOutlines_intOutlineID].[strName]
, IIF([TB_Timeclock].[bytType] = 2, [TB_Errors_intErrorID].[strErrorNo])))',0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
29,4,NULL,NULL,'Hours Spent',0.15,NULL,NULL,0,'ROUND(DATEDIFF("n", [TB_Timeclock].[dtePunchIn], [TB_Timeclock].[dtePunchOut]) / 60, 2)',1);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
30,0,'TB_Tasks','strTaskDesc','Name',0.4,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
30,1,'TB_Tasks','dteDueDate','Due Date',0.15,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
30,2,'TB_Tasks','decPercentComplete','% Complete',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
30,3,NULL,NULL,'Hours Left',0.25,NULL,NULL,0,'IIF(IsNull([TB_Tasks].[decEstHrs]), 0, [TB_Tasks].[decEstHrs]) - IIF(IsNull([TB_Tasks].[decHrsSpent]), 0, [TB_Tasks].[decHrsSpent])',1);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
31,0,'TB_Issues','strIssueDesc','Description',0.45,NULL,NULL,1,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
31,1,'TB_Issues','intIssueLevelID','Issue Level',0.2,NULL,NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
31,2,'TB_IssueLevels','intLevelNo','Level Number',0.2,'intIssueLevelID',NULL,0,NULL,0);
INSERT INTO [TB_AdvFindColumns] ([intAdvFindId],[intAdvFindColumnID],[strTableID],[strFieldID],[strCaption],[dblPercentWidth],[strTablePKField],[intSortOrder],[bytSortType],[strFormula],[bytFormulaDataType]) VALUES (
31,3,'TB_Issues','bolResolved','Resolved?',0.15,NULL,NULL,0,NULL,0);
INSERT INTO [StockMaster] ([StockNumber],[Location],[Price]) VALUES (
'1','Back Room',20);
INSERT INTO [StockMaster] ([StockNumber],[Location],[Price]) VALUES (
'1','Produce',10);
INSERT INTO [StockMaster] ([StockNumber],[Location],[Price]) VALUES (
'1','Warehouse',20);
INSERT INTO [StockMaster] ([StockNumber],[Location],[Price]) VALUES (
'2','Back Room',3);
INSERT INTO [StockMaster] ([StockNumber],[Location],[Price]) VALUES (
'2','Produce',3);
INSERT INTO [StockMaster] ([StockNumber],[Location],[Price]) VALUES (
'2','Warehouse',3);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Back Room','2019-02-01 00:00:00.000',5,5);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Back Room','2019-03-01 00:00:00.000',6,6);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Back Room','2019-04-01 00:00:00.000',6,6);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Produce','2019-01-10 00:00:00.000',10,2);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Produce','2019-02-01 00:00:00.000',2,3);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Produce','2019-02-10 00:00:00.000',6,3);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Warehouse','2019-01-10 00:00:00.000',3,4);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Warehouse','2019-01-14 00:00:00.000',6,3);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'1','Warehouse','2019-03-01 00:00:00.000',5,3);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Back Room','2019-02-02 00:00:00.000',3,3);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Back Room','2019-04-02 00:00:00.000',5,5);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Back Room','2019-06-01 00:00:00.000',8,8);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Produce','2019-01-03 00:00:00.000',7,7);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Produce','2019-04-01 00:00:00.000',7,7);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Produce','2019-12-01 00:00:00.000',8,8);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Warehouse','2019-01-17 00:00:00.000',5,5);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Warehouse','2019-03-08 00:00:00.000',6,6);
INSERT INTO [StockCostQuantity] ([StockNumber],[Location],[PurchasedDateTime],[Quantity],[Cost]) VALUES (
'2','Warehouse','2019-05-01 00:00:00.000',5,5);
COMMIT;


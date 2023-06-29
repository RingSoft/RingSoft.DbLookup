USE [master]
GO

if exists (select * from sysdatabases where name='DevLogix')
		drop database DevLogix
GO

DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1

/****** Object:  Database [DevLogix]    Script Date: 12/19/2019 4:43:33 PM ******/
EXECUTE (N'CREATE DATABASE DevLogix
  ON PRIMARY (NAME = N''DevLogix'', FILENAME = N''' + @device_directory + N'DevLogix.mdf'')
  LOG ON (NAME = N''DevLogix_log'',  FILENAME = N''' + @device_directory + N'DevLogix.ldf'')')
GO
ALTER DATABASE [DevLogix] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DevLogix].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DevLogix] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DevLogix] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DevLogix] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DevLogix] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DevLogix] SET ARITHABORT OFF 
GO
ALTER DATABASE [DevLogix] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DevLogix] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DevLogix] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DevLogix] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DevLogix] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DevLogix] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DevLogix] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DevLogix] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DevLogix] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DevLogix] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DevLogix] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DevLogix] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DevLogix] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DevLogix] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DevLogix] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DevLogix] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DevLogix] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DevLogix] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DevLogix] SET  MULTI_USER 
GO
ALTER DATABASE [DevLogix] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DevLogix] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DevLogix] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DevLogix] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DevLogix] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DevLogix] SET QUERY_STORE = OFF
GO
USE [DevLogix]
GO
/****** Object:  Table [dbo].[StockCostQuantity]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockCostQuantity](
	[StockNumber] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[PurchasedDateTime] [datetime] NOT NULL,
	[Quantity] [double](18, 0) NOT NULL,
	[Cost] [double](18, 0) NOT NULL,
 CONSTRAINT [PK_StockCostQuantity] PRIMARY KEY CLUSTERED 
(
	[StockNumber] ASC,
	[Location] ASC,
	[PurchasedDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockMaster]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockMaster](
	[StockNumber] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[Price] [double](18, 0) NOT NULL,
 CONSTRAINT [PK_StockMaster] PRIMARY KEY CLUSTERED 
(
	[StockNumber] ASC,
	[Location] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_AdvFindColumns]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_AdvFindColumns](
	[intAdvFindId] [int] NOT NULL,
	[intAdvFindColumnID] [int] NOT NULL,
	[strTableID] [nvarchar](50) NULL,
	[strFieldID] [nvarchar](50) NULL,
	[strCaption] [nvarchar](50) NOT NULL,
	[dblPercentWidth] [float] NOT NULL,
	[strTablePKField] [nvarchar](50) NULL,
	[intSortOrder] [int] NULL,
	[bytSortType] [tinyint] NULL,
	[strFormula] [nvarchar](max) NULL,
	[bytFormulaDataType] [tinyint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_AdvFindFilters]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_AdvFindFilters](
	[intAdvFindId] [int] NOT NULL,
	[intAdvFindFilterID] [int] NOT NULL,
	[intLeftParentheses] [int] NULL,
	[strTableID] [nvarchar](50) NULL,
	[strFieldID] [nvarchar](50) NULL,
	[bytOperand] [tinyint] NULL,
	[strSearchValue] [nvarchar](50) NULL,
	[strDisplayValue] [nvarchar](50) NULL,
	[strFormula] [nvarchar](max) NULL,
	[intSearchValueAdvFindID] [int] NULL,
	[bolCustomDate] [bit] NOT NULL,
	[bytEndLogic] [tinyint] NULL,
	[intRightParentheses] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_AdvFinds]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_AdvFinds](
	[intAdvFindId] [int] NOT NULL,
	[strDescription] [nvarchar](50) NOT NULL,
	[strTableID] [nvarchar](50) NOT NULL,
	[strTableDesc] [nvarchar](50) NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_ChartBars]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_ChartBars](
	[intChartID] [int] NOT NULL,
	[intChartBarID] [int] NOT NULL,
	[intAdvFindId] [int] NOT NULL,
	[strCaption] [nvarchar](50) NOT NULL,
	[bolUseFlag] [bit] NOT NULL,
	[bytFlagType] [tinyint] NULL,
	[intRedFlagLevel] [int] NULL,
	[intYellowFlagLevel] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Charts]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Charts](
	[intChartID] [int] NOT NULL,
	[strTitle] [nvarchar](50) NOT NULL,
	[intRefreshRate] [int] NOT NULL,
	[strXAxisTitle] [nvarchar](50) NULL,
	[strYAxisTitle] [nvarchar](50) NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_ErrorPriorities]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_ErrorPriorities](
	[intPriorityID] [int] NOT NULL,
	[strDescription] [nvarchar](50) NULL,
	[intLevelNo] [int] NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Errors]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Errors](
	[intErrorID] [int] NOT NULL,
	[strErrorNo] [nvarchar](50) NULL,
	[dteDate] [datetime] NULL,
	[intStatusID] [int] NULL,
	[intProductID] [int] NULL,
	[intPriorityID] [int] NULL,
	[dteFixedDate] [datetime] NULL,
	[intAssignedToID] [int] NULL,
	[txtDescription] [nvarchar](max) NULL,
	[txtResolution] [nvarchar](max) NULL,
	[decEstHrs] [nvarchar](50) NULL,
	[intTesterID] [int] NULL,
	[dteCompletedDate] [datetime] NULL,
	[intFoundVersionID] [int] NOT NULL,
	[intFixedVersionID] [int] NULL,
	[intOutlineID] [int] NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL,
	[decHrsSpent] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_ErrorsFixedBy]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_ErrorsFixedBy](
	[intErrorID] [int] NOT NULL,
	[intUserID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_ErrorsFoundBy]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_ErrorsFoundBy](
	[intErrorID] [int] NOT NULL,
	[intUserID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_ErrorStatus]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_ErrorStatus](
	[intStatusID] [int] NOT NULL,
	[strStatus] [nvarchar](50) NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_GoalDetails]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_GoalDetails](
	[intGoalID] [int] NOT NULL,
	[intGoalDetailID] [int] NOT NULL,
	[intTaskID] [int] NULL,
	[decHrsToSpend] [double](18, 2) NULL,
	[intErrorID] [int] NULL,
	[intOutlineID] [int] NULL,
	[bytLineType] [tinyint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Goals]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Goals](
	[intGoalID] [int] NOT NULL,
	[intUserID] [int] NULL,
	[dteGoalDate] [datetime] NULL,
	[decWorkingHrs] [double](18, 4) NULL,
	[txtBeginNotes] [nvarchar](max) NULL,
	[txtEndNotes] [nvarchar](max) NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Groups]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Groups](
	[intGroupID] [int] NOT NULL,
	[strGroupName] [nvarchar](50) NOT NULL,
	[txtRights] [nvarchar](max) NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Holidays]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Holidays](
	[dteHoliday] [datetime] NULL,
	[strDescription] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_IssueLevels]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_IssueLevels](
	[intIssueLevelID] [int] NOT NULL,
	[strDescription] [nvarchar](50) NOT NULL,
	[intLevelNo] [int] NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Issues]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Issues](
	[intIssueID] [int] NOT NULL,
	[intTaskID] [int] NOT NULL,
	[strIssueDesc] [nvarchar](100) NOT NULL,
	[bolResolved] [bit] NOT NULL,
	[dteResolved] [datetime] NULL,
	[txtNotes] [nvarchar](max) NULL,
	[intIssueLevelID] [int] NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_OutlineDetails]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_OutlineDetails](
	[intOutlineID] [int] NOT NULL,
	[intDetailID] [int] NOT NULL,
	[strText] [nvarchar](100) NOT NULL,
	[bolComplete] [bit] NOT NULL,
	[intCompletedVersionID] [int] NULL,
	[intTemplateID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_OutlineTemplates]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_OutlineTemplates](
	[intOutlineID] [int] NOT NULL,
	[intTemplateID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Products]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Products](
	[intProductID] [int] NOT NULL,
	[strProduct] [nvarchar](50) NOT NULL,
	[txtNotes] [nvarchar](max) NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_ProjectDays]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_ProjectDays](
	[intProjectID] [int] NOT NULL,
	[bytDayIndex] [tinyint] NOT NULL,
	[decWorkingHrs] [float] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Projects]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Projects](
	[intProjectID] [int] NOT NULL,
	[strProject] [nvarchar](50) NULL,
	[txtNotes] [nvarchar](max) NULL,
	[dteDeadline] [datetime] NOT NULL,
	[dteOriginal] [datetime] NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_ProjectUsers]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_ProjectUsers](
	[intProjectID] [int] NOT NULL,
	[intUserID] [int] NOT NULL,
	[bolStandard] [bit] NOT NULL,
	[decDay0Hrs] [float] NOT NULL,
	[decDay1Hrs] [float] NOT NULL,
	[decDay2Hrs] [float] NOT NULL,
	[decDay3Hrs] [float] NOT NULL,
	[decDay4Hrs] [float] NOT NULL,
	[decDay5Hrs] [float] NOT NULL,
	[decDay6Hrs] [float] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_System]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_System](
	[intSysUnique] [int] NOT NULL,
	[txtSettings] [nvarchar](max) NOT NULL,
	[intWriteOffStatus] [int] NULL,
	[intPassStatus] [int] NULL,
	[intFailStatus] [int] NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL,
	[strErrorNoPrefix] [nvarchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_TaskPriority]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_TaskPriority](
	[intPriorityID] [int] NOT NULL,
	[strDescription] [nvarchar](50) NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL,
	[intPriorityNo] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Tasks]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Tasks](
	[intTaskID] [int] NOT NULL,
	[strTaskDesc] [nvarchar](100) NOT NULL,
	[dteDueDate] [datetime] NULL,
	[dteCompletedDate] [datetime] NULL,
	[decEstHrs] [double](18, 2) NOT NULL,
	[decHrsSpent] [double](18, 2) NULL,
	[txtNotes] [nvarchar](max) NULL,
	[decOrigEst] [double](18, 2) NULL,
	[intProjectID] [int] NULL,
	[strCMSTaskID] [nvarchar](50) NULL,
	[intStatusID] [int] NULL,
	[intPriorityID] [int] NULL,
	[decPercentComplete] [float] NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL,
	[intPriorityNo] [int] NOT NULL,
	[intAssignedToID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_TaskStatus]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_TaskStatus](
	[intStatusID] [int] NOT NULL,
	[strDescription] [nvarchar](50) NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_TestingOutlines]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_TestingOutlines](
	[intOutlineID] [int] NOT NULL,
	[strName] [nvarchar](50) NOT NULL,
	[intProductID] [int] NOT NULL,
	[intCreatedByID] [int] NOT NULL,
	[intAssignedToID] [int] NULL,
	[dteDueDate] [datetime] NULL,
	[decPercentComplete] [float] NOT NULL,
	[txtNotes] [nvarchar](max) NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL,
	[decHrsSpent] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_TestingTemplates]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_TestingTemplates](
	[intTemplateID] [int] NOT NULL,
	[strName] [nvarchar](50) NOT NULL,
	[txtNotes] [nvarchar](max) NULL,
	[intBaseTemplateID] [int] NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_TestTemplDetails]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_TestTemplDetails](
	[intTemplateID] [int] NOT NULL,
	[intDetailID] [int] NOT NULL,
	[strText] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Timeclock]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Timeclock](
	[intTimeclockId] [int] NOT NULL,
	[bytType] [tinyint] NOT NULL,
	[intUserID] [int] NOT NULL,
	[dtePunchIn] [datetime] NOT NULL,
	[dtePunchOut] [datetime] NOT NULL,
	[intTaskID] [int] NULL,
	[intOutlineID] [int] NULL,
	[intErrorID] [int] NULL,
	[txtNotes] [nvarchar](max) NOT NULL,
	[bolPunchChanged] [bit] NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_UserDaysOff]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_UserDaysOff](
	[intUserID] [int] NOT NULL,
	[dteDateOff] [datetime] NOT NULL,
	[strDescription] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_UserGroups]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_UserGroups](
	[intUserID] [int] NOT NULL,
	[intGroupID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Users]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Users](
	[intUserID] [int] NOT NULL,
	[strUserName] [nvarchar](50) NOT NULL,
	[strEmailAddress] [nvarchar](100) NULL,
	[txtNotes] [nvarchar](max) NULL,
	[strInitials] [nvarchar](3) NULL,
	[bolSUP] [bit] NOT NULL,
	[strPassword] [nvarchar](250) NOT NULL,
	[bytDeveloperType] [tinyint] NOT NULL,
	[txtRights] [nvarchar](max) NOT NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_UserSupervisors]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_UserSupervisors](
	[intUserID] [int] NOT NULL,
	[intSupID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Versions]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Versions](
	[intVersionID] [int] NOT NULL,
	[strVersion] [nvarchar](50) NOT NULL,
	[dteCreated] [datetime] NOT NULL,
	[intProductID] [int] NOT NULL,
	[dteRelToQA] [datetime] NULL,
	[dteRelToCust] [datetime] NULL,
	[dteClosed] [datetime] NULL,
	[txtNotes] [nvarchar](max) NULL,
	[dteModifiedDate] [datetime] NULL,
	[strModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VERSYS]    Script Date: 12/25/2019 12:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VERSYS](
	[strVersion] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Back Room', CAST(N'2019-02-01T00:00:00.000' AS DateTime), CAST(5 AS Decimal(18, 0)), CAST(5 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Back Room', CAST(N'2019-03-01T00:00:00.000' AS DateTime), CAST(6 AS Decimal(18, 0)), CAST(6 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Back Room', CAST(N'2019-04-01T00:00:00.000' AS DateTime), CAST(6 AS Decimal(18, 0)), CAST(6 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Produce', CAST(N'2019-01-10T00:00:00.000' AS DateTime), CAST(10 AS Decimal(18, 0)), CAST(2 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Produce', CAST(N'2019-02-01T00:00:00.000' AS DateTime), CAST(2 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Produce', CAST(N'2019-02-10T00:00:00.000' AS DateTime), CAST(6 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Warehouse', CAST(N'2019-01-10T00:00:00.000' AS DateTime), CAST(3 AS Decimal(18, 0)), CAST(4 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Warehouse', CAST(N'2019-01-14T00:00:00.000' AS DateTime), CAST(6 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'1', N'Warehouse', CAST(N'2019-03-01T00:00:00.000' AS DateTime), CAST(5 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Back Room', CAST(N'2019-02-02T00:00:00.000' AS DateTime), CAST(3 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Back Room', CAST(N'2019-04-02T00:00:00.000' AS DateTime), CAST(5 AS Decimal(18, 0)), CAST(5 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Back Room', CAST(N'2019-06-01T00:00:00.000' AS DateTime), CAST(8 AS Decimal(18, 0)), CAST(8 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Produce', CAST(N'2019-01-03T00:00:00.000' AS DateTime), CAST(7 AS Decimal(18, 0)), CAST(7 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Produce', CAST(N'2019-04-01T00:00:00.000' AS DateTime), CAST(7 AS Decimal(18, 0)), CAST(7 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Produce', CAST(N'2019-12-01T00:00:00.000' AS DateTime), CAST(8 AS Decimal(18, 0)), CAST(8 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Warehouse', CAST(N'2019-01-17T00:00:00.000' AS DateTime), CAST(5 AS Decimal(18, 0)), CAST(5 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Warehouse', CAST(N'2019-03-08T00:00:00.000' AS DateTime), CAST(6 AS Decimal(18, 0)), CAST(6 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'2', N'Warehouse', CAST(N'2019-05-01T00:00:00.000' AS DateTime), CAST(5 AS Decimal(18, 0)), CAST(5 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'1', N'Back Room', CAST(20 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'1', N'Produce', CAST(10 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'1', N'Warehouse', CAST(20 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'2', N'Back Room', CAST(3 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'2', N'Produce', CAST(3 AS Decimal(18, 0)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'2', N'Warehouse', CAST(3 AS Decimal(18, 0)))
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (1, 0, N'TB_Errors', N'strErrorNo', N'Error Number', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (1, 1, N'TB_Errors', N'intAssignedToID', N'Assigned Developer', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (1, 2, N'TB_Errors', N'dteDate', N'Date', 0.2, NULL, NULL, 2, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (1, 3, N'TB_Errors', N'intFoundVersionID', N'Found Version', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (1, 4, N'TB_Errors', N'intStatusID', N'Status', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (2, 0, N'TB_Issues', N'strIssueDesc', N'Issue Name', 0.2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (2, 1, N'TB_Tasks', N'strTaskDesc', N'Task Name', 0.2, N'intTaskID', NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (2, 2, N'TB_Tasks', N'intAssignedToID', N'Assigned User', 0.2, N'intTaskID', NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (2, 3, N'TB_Projects', N'strProject', N'Project Name', 0.2, N'intProjectID', NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (7, 0, N'TB_Issues', N'intIssueLevelID', N'Issue Level', 0.2, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (7, 1, N'TB_Issues', N'intTaskID', N'Task', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (7, 2, N'TB_Tasks', N'intPriorityID', N'Priority', 0.2, N'intTaskID', NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (7, 3, N'TB_Tasks', N'intProjectID', N'Project', 0.2, N'intTaskID', NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (8, 0, N'TB_GoalDetails', N'intGoalID', N'Goal', 0.2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (8, 1, N'TB_Goals', N'intUserID', N'User', 0.2, N'intGoalID', NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (8, 2, N'TB_GoalDetails', N'intErrorID', N'Error', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (9, 0, N'TB_ErrorsFixedBy', N'intUserID', N'User', 0.2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (9, 1, N'TB_ErrorsFixedBy', N'intErrorID', N'Error', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (9, 2, N'TB_Errors', N'intProductID', N'Product', 0.2, N'intErrorID', NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (9, 3, N'TB_Errors', N'intFixedVersionID', N'Fixed Version', 0.2, N'intErrorID', NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (10, 0, N'TB_Errors', N'strErrorNo', N'Error Number', 0.2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (10, 1, N'TB_Errors', N'intPriorityID', N'Priority', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (10, 2, N'TB_Errors', N'intProductID', N'Product', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (10, 3, N'TB_Errors', N'intStatusID', N'Status', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (11, 0, N'TB_Errors', N'strErrorNo', N'Error Number', 0.2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (11, 1, N'TB_Errors', N'intStatusID', N'Status', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (11, 2, N'TB_Errors', N'intPriorityID', N'Priority', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (11, 3, N'TB_Errors', N'intAssignedToID', N'Assigned Developer', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (12, 0, N'TB_Errors', N'strErrorNo', N'Error Number', 0.2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (12, 1, N'TB_Errors', N'intAssignedToID', N'Assigned Developer', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (12, 2, N'TB_Errors', N'intStatusID', N'Status', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (12, 3, N'TB_Errors', N'intPriorityID', N'Priority', 0.2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (14, 0, N'TB_TestingOutlines', N'strName', N'Outline Name', 0.4, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (14, 1, N'TB_TestingOutlines', N'intProductID', N'Product', 0.3, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (13, 0, NULL, NULL, N'Remaining Hours', 0.2, NULL, NULL, 2, N'[TB_Tasks].[decEstHrs] - [TB_Tasks].[decHrsSpent]', 1)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (13, 1, N'TB_Tasks', N'strTaskDesc', N'Name', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (13, 2, N'TB_Tasks', N'intProjectID', N'Project', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (13, 3, NULL, NULL, N'Formula2', 0.2, NULL, NULL, 0, N'[TB_Tasks].[decEstHrs] - [TB_Tasks].[decHrsSpent]', 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (16, 0, N'TB_Timeclock', N'intUserID', N'User', 0.25, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (16, 1, N'TB_Timeclock', N'dtePunchIn', N'Punch In', 0.25, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (16, 2, N'TB_Timeclock', N'dtePunchOut', N'Punch Out', 0.25, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (16, 3, NULL, NULL, N'Hours Spent', 0.25, NULL, NULL, 0, N'ROUND(DATEDIFF("n", [TB_Timeclock].[dtePunchIn], [TB_Timeclock].[dtePunchOut]) / 60, 2)', 1)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (19, 0, N'TB_TestingOutlines', N'strName', N'Outline Name', 0.3, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (19, 1, N'TB_TestingOutlines', N'intProductID', N'Product', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (19, 2, N'TB_TestingOutlines', N'intAssignedToID', N'Assigned To', 0.3, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (19, 3, N'TB_TestingOutlines', N'decPercentComplete', N'Percent Complete', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (20, 0, N'TB_TestingOutlines', N'strName', N'Outline Name', 0.4, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (20, 1, N'TB_TestingOutlines', N'intProductID', N'Product', 0.3, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (20, 2, N'TB_TestingOutlines', N'intAssignedToID', N'Assigned To', 0.3, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (21, 0, N'TB_Errors', N'intErrorID', N'Error ID', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (21, 1, N'TB_Errors', N'strErrorNo', N'Error Number', 0.2, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (21, 2, N'TB_Errors', N'intProductID', N'Product', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (21, 3, N'TB_Errors', N'intStatusID', N'Status', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (21, 4, N'TB_Errors', N'intPriorityID', N'Priority', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (22, 0, N'TB_Errors', N'strErrorNo', N'Error Number', 0.2, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (22, 1, N'TB_Errors', N'intPriorityID', N'Priority', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (22, 2, N'TB_Errors', N'intTesterID', N'Assigned Tester', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (22, 3, N'TB_Errors', N'txtDescription', N'Description', 0.4, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (26, 0, N'TB_Tasks', N'strTaskDesc', N'Name', 0.4, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (26, 1, N'TB_Tasks', N'dteDueDate', N'Due Date', 0.15, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (26, 2, N'TB_Tasks', N'decPercentComplete', N'% Complete', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (26, 3, NULL, NULL, N'Hours Left', 0.25, NULL, NULL, 0, N'IIF(IsNull([TB_Tasks].[decEstHrs]), 0, [TB_Tasks].[decEstHrs]) - IIF(IsNull([TB_Tasks].[decHrsSpent]), 0, [TB_Tasks].[decHrsSpent])', 1)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (24, 0, N'TB_Tasks', N'strTaskDesc', N'Name', 0.4, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (24, 1, N'TB_Tasks', N'dteDueDate', N'Due Date', 0.15, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (24, 2, N'TB_Tasks', N'decPercentComplete', N'% Complete', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (24, 3, NULL, NULL, N'Hours Left', 0.25, NULL, NULL, 0, N'IIF(IsNull([TB_Tasks].[decEstHrs]), 0, [TB_Tasks].[decEstHrs]) - IIF(IsNull([TB_Tasks].[decHrsSpent]), 0, [TB_Tasks].[decHrsSpent])', 1)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (23, 0, N'TB_Tasks', N'strTaskDesc', N'Name', 0.4, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (23, 1, N'TB_Tasks', N'dteDueDate', N'Due Date', 0.15, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (23, 2, N'TB_Tasks', N'decPercentComplete', N'% Complete', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (23, 3, NULL, NULL, N'Hours Left', 0.25, NULL, NULL, 0, N'IIF(IsNull([TB_Tasks].[decEstHrs]), 0, [TB_Tasks].[decEstHrs]) - IIF(IsNull([TB_Tasks].[decHrsSpent]), 0, [TB_Tasks].[decHrsSpent])', 1)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (18, 0, N'TB_Errors', N'strErrorNo', N'Error Number', 0.2, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (18, 1, N'TB_Errors', N'intStatusID', N'Status', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (18, 2, N'TB_Errors', N'intPriorityID', N'Priority', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (18, 3, N'TB_Errors', N'txtDescription', N'Description', 0.4, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (28, 0, N'TB_Timeclock', N'dtePunchIn', N'Punch In Date', 0.2, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (28, 1, N'TB_Timeclock', N'dtePunchOut', N'Punch Out Date', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (28, 2, N'TB_Timeclock', N'bytType', N'Punch Type', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (28, 3, NULL, NULL, N'Code', 0.2, NULL, NULL, 0, N'IIF([TB_Timeclock].[bytType] = 0, [TB_Tasks_intTaskID].[strTaskDesc]
  , IIF([TB_Timeclock].[bytType] = 1, [TB_TestingOutlines_intOutlineID].[strName]
  , IIF([TB_Timeclock].[bytType] = 2, [TB_Errors_intErrorID].[strErrorNo])))', 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (28, 4, NULL, NULL, N'Hours Spent', 0.2, NULL, NULL, 0, N'ROUND(DATEDIFF("n", [TB_Timeclock].[dtePunchIn], [TB_Timeclock].[dtePunchOut]) / 60, 2)', 1)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (29, 0, N'TB_Timeclock', N'dtePunchIn', N'Punch In', 0.25, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (29, 1, N'TB_Timeclock', N'dtePunchOut', N'Punch Out', 0.25, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (29, 2, N'TB_Timeclock', N'bytType', N'Type', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (29, 3, NULL, NULL, N'Code', 0.15, NULL, NULL, 0, N'IIF([TB_Timeclock].[bytType] = 0, [TB_Tasks_intTaskID].[strTaskDesc]
, IIF([TB_Timeclock].[bytType] = 1, [TB_TestingOutlines_intOutlineID].[strName]
, IIF([TB_Timeclock].[bytType] = 2, [TB_Errors_intErrorID].[strErrorNo])))', 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (29, 4, NULL, NULL, N'Hours Spent', 0.15, NULL, NULL, 0, N'ROUND(DATEDIFF("n", [TB_Timeclock].[dtePunchIn], [TB_Timeclock].[dtePunchOut]) / 60, 2)', 1)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (30, 0, N'TB_Tasks', N'strTaskDesc', N'Name', 0.4, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (30, 1, N'TB_Tasks', N'dteDueDate', N'Due Date', 0.15, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (30, 2, N'TB_Tasks', N'decPercentComplete', N'% Complete', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (30, 3, NULL, NULL, N'Hours Left', 0.25, NULL, NULL, 0, N'IIF(IsNull([TB_Tasks].[decEstHrs]), 0, [TB_Tasks].[decEstHrs]) - IIF(IsNull([TB_Tasks].[decHrsSpent]), 0, [TB_Tasks].[decHrsSpent])', 1)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (31, 0, N'TB_Issues', N'strIssueDesc', N'Description', 0.45, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (31, 1, N'TB_Issues', N'intIssueLevelID', N'Issue Level', 0.2, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (31, 2, N'TB_IssueLevels', N'intLevelNo', N'Level Number', 0.2, N'intIssueLevelID', NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindColumns] ([intAdvFindId], [intAdvFindColumnID], [strTableID], [strFieldID], [strCaption], [dblPercentWidth], [strTablePKField], [intSortOrder], [bytSortType], [strFormula], [bytFormulaDataType]) VALUES (31, 3, N'TB_Issues', N'bolResolved', N'Resolved?', 0.15, NULL, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (1, 0, 0, N'TB_Errors', N'strErrorNo', 6, N'E', N'E', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (1, 1, 0, NULL, NULL, 0, NULL, N'Unfixed Errors', NULL, 10, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (7, 0, 0, N'TB_Tasks', N'intProjectID', 2, N'd', N'd', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (7, 1, 1, N'TB_Issues', N'intIssueLevelID', 6, N'Mil', N'Mil', NULL, NULL, 0, 2, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (7, 2, 0, N'TB_Issues', N'intTaskID', 2, N'b', N'b', NULL, NULL, 0, 2, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (7, 3, 0, N'TB_Tasks', N'intPriorityID', 2, N'q', N'q', NULL, NULL, 0, 1, 1)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (7, 4, 0, N'TB_Issues', N'intIssueLevelID', 0, N'2', N'Milestone 01', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (9, 0, 0, N'TB_ErrorsFixedBy', N'intUserID', 2, N'C', N'C', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (9, 1, 0, N'TB_ErrorsFixedBy', N'intErrorID', 2, N'E-150', N'E-150', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (9, 2, 0, N'TB_Errors', N'intProductID', 2, N'd', N'd', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (9, 3, 0, N'TB_Errors', N'intFixedVersionID', 2, N'2.00.0035', N'2.00.0035', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (10, 0, 0, N'TB_Errors', N'intStatusID', 0, N'2', N'Open', NULL, NULL, 0, 2, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (10, 1, 0, N'TB_Errors', N'intStatusID', 0, N'1', N'Pending Correction', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (11, 0, 0, N'TB_Errors', N'intAssignedToID', 0, N'10', N'Becca Smith', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (11, 1, 0, NULL, NULL, 0, NULL, NULL, NULL, 10, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (12, 0, 0, NULL, NULL, 0, NULL, NULL, NULL, 10, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (14, 0, 0, N'TB_TestingOutlines', N'intAssignedToID', 0, N'1', N'Peter Ringering', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (19, 0, 0, N'TB_TestingOutlines', N'decPercentComplete', 4, N'1', N'100 %', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (20, 0, 0, NULL, NULL, 0, NULL, N'Incomplete Testing Outlines', NULL, 19, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (20, 1, 0, N'TB_TestingOutlines', N'intProductID', 0, N'30', N'DevLogix', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (21, 0, 0, N'TB_Errors', N'intStatusID', 0, N'8', N'Pending QA Test', NULL, NULL, 0, 2, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (21, 1, 0, N'TB_Errors', N'intStatusID', 0, N'3', N'Pending Unit Test', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (22, 0, 0, NULL, NULL, 0, NULL, N'Untested Errors', NULL, 21, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (22, 1, 0, N'TB_Errors', N'intProductID', 0, N'30', N'DevLogix', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (13, 0, 0, NULL, NULL, 0, NULL, N'Remaining Hours > 0', N'([TB_Tasks].[decEstHrs] - [TB_Tasks].[decHrsSpent]) > 0', NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (13, 1, 0, N'TB_Tasks', N'intAssignedToID', 0, N'1', N'Peter Ringering', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (23, 0, 0, N'TB_Tasks', N'intProjectID', 0, N'16', N'DevLogix', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (23, 1, 0, N'TB_Tasks', N'decPercentComplete', 4, N'1', N'100 %', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (18, 0, 0, NULL, NULL, 0, NULL, N'Unfixed Errors', NULL, 10, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (18, 1, 0, N'TB_Errors', N'intProductID', 0, N'30', N'DevLogix', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (30, 0, 0, N'TB_Tasks', N'intProjectID', 0, N'21', N'RSDbLookup', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (30, 1, 0, N'TB_Tasks', N'decPercentComplete', 4, N'1', N'100 %', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFindFilters] ([intAdvFindId], [intAdvFindFilterID], [intLeftParentheses], [strTableID], [strFieldID], [bytOperand], [strSearchValue], [strDisplayValue], [strFormula], [intSearchValueAdvFindID], [bolCustomDate], [bytEndLogic], [intRightParentheses]) VALUES (31, 1, 0, N'TB_Issues', N'bolResolved', 0, N'0', N'False', NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Errors', N'TB_Errors', N'Errors', CAST(N'2018-11-17T15:17:55.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'Task Issues', N'TB_Issues', N'Task Issues', CAST(N'2018-10-31T15:54:57.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (7, N'zTest', N'TB_Issues', N'Task Issues', CAST(N'2018-11-23T20:58:54.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (8, N'Goal Details Test', N'TB_GoalDetails', N'User Goal Details', CAST(N'2018-11-04T15:17:27.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (9, N'zTestErrors Fixed By', N'TB_ErrorsFixedBy', N'Errors Fixed By', CAST(N'2018-11-04T15:29:15.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (10, N'Unfixed Errors', N'TB_Errors', N'Errors', CAST(N'2018-11-05T13:51:47.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (11, N'Becca''s Unfixed', N'TB_Errors', N'Errors', CAST(N'2018-11-08T13:26:32.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (12, N'All Unfixed', N'TB_Errors', N'Errors', CAST(N'2018-11-08T13:20:00.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (13, N'Tasks', N'TB_Tasks', N'Tasks', CAST(N'2019-05-15T13:04:36.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (14, N'Peter''s Testing Outlines', N'TB_TestingOutlines', N'Testing Outlines', CAST(N'2018-11-26T12:45:14.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (16, N'Time Clock', N'TB_Timeclock', N'Timeclock', CAST(N'2019-04-19T13:20:50.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (18, N'DevLogix Unfixed Errors', N'TB_Errors', N'Errors', CAST(N'2019-05-22T14:25:02.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (19, N'Incomplete Testing Outlines', N'TB_TestingOutlines', N'Testing Outlines', CAST(N'2019-05-04T15:51:00.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (20, N'DevLogix Incomplete Testing Outlines', N'TB_TestingOutlines', N'Testing Outlines', CAST(N'2019-05-07T14:05:56.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (21, N'Untested Errors', N'TB_Errors', N'Errors', CAST(N'2019-05-07T13:55:27.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (22, N'DevLogix Utested Errors', N'TB_Errors', N'Errors', CAST(N'2019-05-10T15:13:38.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (23, N'DevLogix Incomplete Tasks', N'TB_Tasks', N'Tasks', CAST(N'2019-05-22T13:16:22.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (24, N'No Filter', N'TB_Tasks', N'Tasks', CAST(N'2019-05-15T13:04:24.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (26, N'No Filter1', N'TB_Tasks', N'Tasks', CAST(N'2019-05-15T13:03:14.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (28, N'User Time Clock', N'TB_Timeclock', N'Timeclock', CAST(N'2019-05-31T11:51:57.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (29, N'E-269', N'TB_Timeclock', N'Timeclock', CAST(N'2019-06-01T12:46:20.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (30, N'RSDbLookup Incomplete Tasks', N'TB_Tasks', N'Tasks', CAST(N'2019-08-19T17:26:13.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_AdvFinds] ([intAdvFindId], [strDescription], [strTableID], [strTableDesc], [dteModifiedDate], [strModifiedBy]) VALUES (31, N'Unresolved Issues', N'TB_Issues', N'Task Issues', CAST(N'2019-08-19T20:10:59.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_ChartBars] ([intChartID], [intChartBarID], [intAdvFindId], [strCaption], [bolUseFlag], [bytFlagType], [intRedFlagLevel], [intYellowFlagLevel]) VALUES (1, 0, 1, N'Errors', 1, 3, 30, 15)
GO
INSERT [dbo].[TB_ChartBars] ([intChartID], [intChartBarID], [intAdvFindId], [strCaption], [bolUseFlag], [bytFlagType], [intRedFlagLevel], [intYellowFlagLevel]) VALUES (1, 1, 13, N'Tasks', 0, 3, 0, 0)
GO
INSERT [dbo].[TB_ChartBars] ([intChartID], [intChartBarID], [intAdvFindId], [strCaption], [bolUseFlag], [bytFlagType], [intRedFlagLevel], [intYellowFlagLevel]) VALUES (1, 2, 7, N'zTest', 0, 3, 0, 0)
GO
INSERT [dbo].[TB_ChartBars] ([intChartID], [intChartBarID], [intAdvFindId], [strCaption], [bolUseFlag], [bytFlagType], [intRedFlagLevel], [intYellowFlagLevel]) VALUES (2, 0, 18, N'DevLogix Unfixed Errors', 0, 3, 0, 0)
GO
INSERT [dbo].[TB_ChartBars] ([intChartID], [intChartBarID], [intAdvFindId], [strCaption], [bolUseFlag], [bytFlagType], [intRedFlagLevel], [intYellowFlagLevel]) VALUES (2, 1, 22, N'DevLogix Utested Errors', 0, 3, 0, 0)
GO
INSERT [dbo].[TB_ChartBars] ([intChartID], [intChartBarID], [intAdvFindId], [strCaption], [bolUseFlag], [bytFlagType], [intRedFlagLevel], [intYellowFlagLevel]) VALUES (2, 2, 20, N'DevLogix Incomplete Testing Outlines', 0, 3, 0, 0)
GO
INSERT [dbo].[TB_ChartBars] ([intChartID], [intChartBarID], [intAdvFindId], [strCaption], [bolUseFlag], [bytFlagType], [intRedFlagLevel], [intYellowFlagLevel]) VALUES (2, 3, 23, N'DevLogix Incomplete Tasks', 0, 3, 0, 0)
GO
INSERT [dbo].[TB_Charts] ([intChartID], [strTitle], [intRefreshRate], [strXAxisTitle], [strYAxisTitle], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'zTest', 1, N'a', N'b', CAST(N'2018-11-24T18:21:31.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Charts] ([intChartID], [strTitle], [intRefreshRate], [strXAxisTitle], [strYAxisTitle], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'Peter''s To Do', 5, NULL, NULL, CAST(N'2019-05-07T14:09:52.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_ErrorPriorities] ([intPriorityID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Design', 1, CAST(N'2015-09-19T14:31:58.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_ErrorPriorities] ([intPriorityID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'Cosmetic', 2, CAST(N'2015-09-19T15:47:55.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_ErrorPriorities] ([intPriorityID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (3, N'Procedural', 3, NULL, NULL)
GO
INSERT [dbo].[TB_ErrorPriorities] ([intPriorityID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (4, N'Data Corruption', 4, NULL, NULL)
GO
INSERT [dbo].[TB_ErrorPriorities] ([intPriorityID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (5, N'Fatal - Application Crash', 5, NULL, NULL)
GO
INSERT [dbo].[TB_ErrorPriorities] ([intPriorityID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (6, N'Urgent Design', 5, NULL, NULL)
GO
INSERT [dbo].[TB_ErrorPriorities] ([intPriorityID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (7, N'Urgent Cosmetic', 5, NULL, NULL)
GO
INSERT [dbo].[TB_ErrorPriorities] ([intPriorityID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (8, N'Urgent Procedural', 5, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (123, N'E-122', CAST(N'2013-04-21T14:49:13.000' AS DateTime), 5, 28, 4, CAST(N'2015-07-24T14:20:44.000' AS DateTime), 1, N'Need to code server side to have get data and save data SQL statements in the same DB transaction so if 1 SQL statement fails, then everything is rolled back.

Example On save, we save the header then get the last PKIdent value all in the same SQL transaction.

Otherwise, if 2 people enter data and insert at the same time, they''ll get the wrong PKIdent values back and when they go save the grid data, the PKIdent value will point back to the wrong header record.', N'Peter Ringering - 07/24/2015 02:23:05 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:20:44 PM - Fixed. Insert code saves and retrieves the PKIdent value in the same server side call.  Changes affect PTRShared.dll
', NULL, NULL, CAST(N'2015-07-24T14:23:05.000' AS DateTime), 1, 85, NULL, CAST(N'2019-05-10T15:19:18.000' AS DateTime), N'Peter Ringering', 0)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (124, N'E-124', CAST(N'2013-04-21T15:02:32.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-14T16:36:30.000' AS DateTime), 1, N'Need a cool "Record Saved" form.', N'Peter Ringering - 06/14/2013 04:36:30 PM - Added an icon to the Record Saved form.  Changes affect PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (125, N'E-125', CAST(N'2013-06-06T21:34:12.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-07T21:13:06.000' AS DateTime), 1, N'When you backspace once on text on a lookup textbox control, it erases the whole field.  To reproduce, type some text on a lookup textbox control, move the cursor to the end of the text.  Hit Back Space once.  Notice everything is erased.', N'Peter Ringering - 06/07/2013 09:13:06 PM - Code was clearing out everything if there was nothing in the database and the backspace key was pressed.  Also have to get to work when user hits backspace on the only and last character in the control.  Fixed so the control gets cleared out when backspace is pressed on the only character on the control.  Changes affect PTRFormsV2.DLL and CTLDBLookupText.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (126, N'E-126', CAST(N'2013-06-07T12:25:21.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-07T21:25:21.000' AS DateTime), 1, N'Maintenance forms.  Hide the print button by default.  There will be no  generic reports.  Shopping List can have it visible since it has a special report type.', N'Peter Ringering - 06/07/2013 09:25:21 PM - Corrected PTRMaintForm.cs constructor to hide the print button.  Derived classes can make it visible as needed.  Changes affect PTRFormsV2.dll and PTRMaintForm.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (127, N'E-127', CAST(N'2013-06-07T12:30:15.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-17T20:41:02.000' AS DateTime), 1, N'Need a splash screen that shows what''s going on at application startup.  It should show beginning with app startup until the user has control of the app.', N'Peter Ringering - 06/17/2013 08:41:02 PM - Code Complete.  Changes affect Ring2FamilyIS.exe.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (128, N'E-128', CAST(N'2013-06-07T12:45:54.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-18T13:53:08.000' AS DateTime), 1, N'Need first-time startup wizard to help new user how to quickly get Bank Account/Bill information into the system.', N'Peter Ringering - 06/18/2013 01:53:08 PM - Not much can be done in this area.  User can see what needs to be done with the current design.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (129, N'E-129', CAST(N'2013-06-07T15:22:46.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-09T15:23:39.000' AS DateTime), 1, N'Need to fix grid so that an empty row is added when the user enters data on the last row or when it is loaded from the database..', N'Peter Ringering - 06/09/2013 03:23:39 PM - Corrected.  Related to E-149.  Changes affect PTRFormsV2.dll and PTRGrid.cs.
Peter Ringering - 06/08/2013 03:30:14 PM - Fail.  When focus is on the last new line, and then you hit Ctrl+D, it erases the row.  What should happen is nothing.
Peter Ringering - 06/08/2013 02:16:10 PM - Corrected code to add a row when the user keys in new data on the last row or when the grid is loaded from the database.  Changes affect PTRFormsV2.dll and PTRGrid.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (142, N'E-142', CAST(N'2013-06-07T17:32:37.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-18T21:29:29.000' AS DateTime), 1, N'All forms.  Change the word ''Bill'' to ''Transaction''.', N'Peter Ringering - 06/18/2013 09:29:29 PM - Corrected.  Changes affect Ring2FamilyIS.exe.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (155, N'E-155', CAST(N'2013-06-09T18:56:40.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-22T21:38:02.000' AS DateTime), 1, N'The main sub-menus and context menus need icons.', N'Peter Ringering - 06/22/2013 09:38:02 PM - Fixed.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (193, N'E-193', CAST(N'2015-02-05T18:00:31.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-10T15:01:39.000' AS DateTime), 1, N'################', N'################', NULL, NULL, CAST(N'2019-05-10T15:02:02.000' AS DateTime), 53, 147, NULL, CAST(N'2019-05-10T15:02:07.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (194, N'E-194', CAST(N'2015-02-13T21:11:13.000' AS DateTime), 5, 30, 4, CAST(N'2019-05-10T15:08:02.000' AS DateTime), 1, N'################', N'Peter Ringering - 05/10/2019 03:56:00 PM - QA Tested and Passed. 
################', NULL, NULL, CAST(N'2019-05-10T15:56:00.000' AS DateTime), 53, 147, NULL, CAST(N'2019-05-10T15:56:05.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (130, N'E-130', CAST(N'2013-06-07T15:21:03.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-09T16:40:11.000' AS DateTime), 1, N'Recurring Template column header is underlined when grid does not have focus and the active cell is that column.', N'Peter Ringering - 06/09/2013 04:40:11 PM - Moved column header underline code in OnCellEnter to OnCellBeginEdit.  This will only underline the header if there''s a lookup control showing.  I kept the code in OnCellLeave so when the user leaves the cell, the underline goes away.  Changes affect PTRFormsV2.dll and PTRGrid.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (131, N'E-131', CAST(N'2013-06-07T15:26:09.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-14T10:07:52.000' AS DateTime), 1, N'When the user tries to launch the maintenance form lookup, the program needs to check to see if there are any records in the table and if not, then a message should show that there are no records.  Please enter and save a record.  This should also work when the user hits the <-- and --> maintenance form buttons.', N'Peter Ringering - 06/14/2013 10:07:52 AM - Corrected for the arrow buttons.  Decided against the lookup because it will just show an empty lookup which is acceptable.  Changes affect PTRFormsV2.dll', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (132, N'E-132', CAST(N'2013-06-07T15:31:55.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-09T14:42:18.000' AS DateTime), 1, N'When Bank Accounts form is in add mode and the Bills grid gets focus, we need to ask the user to save the record first.  If the user says no, then focus needs to go to the next control in the form''s tab order', N'Peter Ringering - 06/09/2013 02:42:18 PM - Showing a message box when grid gets focus and then changing focus was causing many more errors and design issues.  Corrected code so that in Add Mode, a message is put on top of the grid saying that the bank account must be saved before the user can enter transactions and the grid is disabled.  Once the form goes into edit mode, the grid is enabled and the save label goes away.  Changes affect Ring2FamilyIS.exe and frmBankAcctMgr.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (133, N'E-133', CAST(N'2013-06-07T16:40:36.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-13T20:30:34.000' AS DateTime), 1, N'Peter Ringering - 06/09/2013 05:25:42 PM - Correction.  What should happen is, when the user keys in a recurring template into the grid, and the template recurring type is not One Time Only, then it should auto-populate the row per E-136.  Then it should ask the user if he wants us to update the template''s Generate Starting date so that this transaction is not a duplicate.  Also, this message box should have a "Do not ask again--always update" checkbox.  If the user checks it, then the message box will not show and we will always update the template without asking the user.

Generate From Recurring.  When generating, the code should check to see if there''s a bill with the same date as what''s being generated.  If there is, then the bill should not be generated, but the Generate Starting date on the recurring template should still be updated.', N'Peter Ringering - 06/13/2013 08:30:34 PM - Implemented the design changes per 06/09/2013.  Changes affect PTRFormsV2.dll and Ring2FamilyIS.exe.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (134, N'E-134', CAST(N'2013-06-07T16:47:40.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-09T19:56:51.000' AS DateTime), 1, N'Bank Account Manager, Bills grid.  The recurring template lookup and autofill needs to filter to only show recurring templates that are attached to the current bank account.', N'Peter Ringering - 06/09/2013 07:56:51 PM - Added code to EnableGrid to filter the recurring lookup when in edit mode.  Changes affect Ring2IS.exe and frmBankAcctMgr.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (207, N'E-207', CAST(N'2015-05-31T19:15:34.000' AS DateTime), 2, 30, 3, NULL, 1, N'When showing Help from the Print Codes and Print Options forms, the help shows but then the form that launched the Help topic closes.', NULL, NULL, NULL, NULL, 76, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (225, N'E-225', CAST(N'2015-10-11T22:11:29.000' AS DateTime), 1, 29, 2, NULL, 1, N'Launch the app.  Now maximize the main window.  Notice the embedded graphic is duplicated.', NULL, NULL, NULL, NULL, 131, NULL, NULL, CAST(N'2015-10-11T22:13:07.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (235, N'E-235', CAST(N'2019-05-04T13:33:18.000' AS DateTime), 1, 30, 3, NULL, 1, N'Take Error ID off the Errors lookup.', NULL, NULL, NULL, NULL, 145, NULL, NULL, CAST(N'2019-05-04T13:36:54.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (236, N'E-236', CAST(N'2019-05-04T13:36:57.000' AS DateTime), 1, 30, 2, NULL, 1, N'Advanced Find, Errors table.  Hours Spent field has 2 spaces between ''Hours'' and ''Spent''.', NULL, NULL, NULL, NULL, 145, NULL, NULL, CAST(N'2019-05-04T13:41:01.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (135, N'E-135', CAST(N'2013-06-07T16:52:58.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-10T13:59:46.000' AS DateTime), 1, N'Bank Account Manager, Bills grid.  When the recurring template maintenance form is launched in add-on-the-fly mode, the bank account control should populated with the bank account info in the Bank Account Manager.', N'Peter Ringering - 06/10/2013 01:59:46 PM - Corrected code to work as described.  Also had bank account disabled when launched from bank account manager form.  Changes affect Ring2FamilyIS.exe and frmRecurTemplates.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (136, N'E-136', CAST(N'2013-06-07T16:57:04.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-13T20:32:40.000' AS DateTime), 1, N'Bank Account Manager, Bills grid.  When a recurring template is entered and the user hits tab.  
1.  The program should validate that the recurring template is a valid one.  If not, it should show a dialog (similar to Shopping List) asking the user if he wants to add a new one or cancel.  If add, then it should show the recurring template in add-on-the-fly mode.
2.  After a valid code has been entered, the program should look in the recurring templates record and auto-populate the cells to the right with the information.', N'Peter Ringering - 06/13/2013 08:32:40 PM - Implemented #2.  Changes affect Ring2IS.exe and PTRFormsV2.dll.
Peter Ringering - 06/11/2013 08:59:32 PM - Implemented #1.  Changes affect Ring2IS.exe and PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (137, N'E-137', CAST(N'2013-06-07T17:06:44.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-13T20:34:33.000' AS DateTime), 1, N'Change Income/Expense to Deposit/Withdrawal.  Fix Bank Account Manager Bills grid and Recurring Templates maintenance form.', N'Peter Ringering - 06/13/2013 08:34:33 PM - Fixed Bank Account Manager and Recurring Templates.  Changes affect Ring2FamilyIS.exe.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (138, N'E-138', CAST(N'2013-06-07T17:11:45.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-14T14:40:06.000' AS DateTime), 1, N'Need to convert ENTER to TAB on all forms so if the user hits ENTER on a control, it functions just like a TAB.', N'Peter Ringering - 06/14/2013 02:40:06 PM - Corrected.  Made sure this ran correctly when textbox control accepts ENTER like the notes textbox.  Changes affect PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (139, N'E-139', CAST(N'2013-06-07T17:18:30.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-14T16:06:02.000' AS DateTime), 1, N'All forms, textbox controls.  When a textbox control gets focus, all contents should be selected.', N'Peter Ringering - 06/14/2013 04:06:02 PM - Corrected.  Changes affect PTRFormsV2.dll', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (140, N'E-140', CAST(N'2013-06-07T17:20:17.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-18T21:02:08.000' AS DateTime), 1, N'Research grid date control to see if it''s possible to set focus to next cell when focus is on the year and the user hits --> and to set focus to previous cell when focus is on the day and the user hits <--.', N'Peter Ringering - 06/18/2013 09:02:08 PM - Way too much work for now.  Would have to make a whole new control from scratch.  There''s nothing in CodeProject that I can use.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (141, N'E-141', CAST(N'2013-06-07T17:28:52.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-18T21:22:14.000' AS DateTime), 1, N'Generate from recurring button should be disabled when Bank Account Manager form is in add mode.', N'Peter Ringering - 06/18/2013 09:22:14 PM - Corrected.  Changes affect Ring2FamilyIS.exe.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (143, N'E-143', CAST(N'2013-06-07T17:49:33.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-19T12:46:39.000' AS DateTime), 1, N'Dashboard, Bank Account Details.  Refresh does not clear out the beginning and ending balances before reloading the form.  On a new database with no bank accounts, if you create a bank account, then goto Bank Account Details, then delete that bank account, the beginning and ending balances are the same.', N'Peter Ringering - 06/19/2013 12:46:39 PM - Corrected.  Changes affect Ring2FamilyIS.exe.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (144, N'E-144', CAST(N'2013-06-07T17:53:32.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-07T20:13:39.000' AS DateTime), 1, N'Set .NET Framework version to 4.0 to ensure compatibility with Windows XP.', N'Peter Ringering - 06/07/2013 08:13:39 PM - Set all projects''s .NET Framework version from 4.5 to 4.0.  One error resulted in XML Viewer using async/await which is only available in 4.5.  Changed code to use Background Worker control instead.  Changes affect all EXE''s and DLL''s.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (145, N'E-145', CAST(N'2013-06-07T20:43:22.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-19T15:15:20.000' AS DateTime), 1, N'MDB Login form.  Need to add a button called "Find" below the MDB textbox.  When the user clicks on it, it will open an Explorer window with the folder containing the MDB.', N'Peter Ringering - 06/19/2013 03:15:20 PM - Added "Locate" button below the MDB textbox.  Changes affect PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (146, N'E-146', CAST(N'2013-06-08T15:16:01.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-19T12:09:58.000' AS DateTime), 1, N'On grid, when focus is on a cell in the grid and then hit Ctrl+S the "Record Saved" form shows but then focus goes to the first cell on the first row.  Focus should not move.', N'Peter Ringering - 06/19/2013 12:09:58 PM - Only occurs on Bank Account Manager grid which is per design.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (147, N'E-147', CAST(N'2013-06-08T15:18:48.000' AS DateTime), 5, 28, 4, CAST(N'2013-06-08T20:27:31.000' AS DateTime), 1, N'Generate From Recurring.  When this is run on data that has multiple weekly templates, it groups all the transactions together instead of by date, to reproduce, create a new bank account, add 3 recurring templates, all of them should be weekly types.  Now click on Generate From Recurring and notice the output.', N'Peter Ringering - 06/08/2013 08:27:31 PM - Corrected code so when the Bank Account Manager Transactions grid loads up, it is sorted by date and then by line number.  It would take too much code for Generate From Recurring to redo the line numbers in order to sort by date.  Changes affect Ring2FamilyIS.exe, and PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (148, N'E-148', CAST(N'2013-06-08T15:31:02.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-21T21:08:04.000' AS DateTime), 1, N'Need context menu on all grids for add row and delete row.', N'Peter Ringering - 06/21/2013 09:08:04 PM - Fixed multiple parts of PTRGrid and PTRGrid controls.  Changes affect PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (149, N'E-149', CAST(N'2013-06-08T15:40:45.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-09T15:22:15.000' AS DateTime), 1, N'When focus is on a row other than the last blank line and the user hits Ctrl+D, the focus should go up 1 row.  If the user hits Ctrl+D on the first row,  the focus should stay on the first row.', N'Peter Ringering - 06/09/2013 03:22:15 PM - Corrected code as described.  Related to E-129.  Changes affect PTRFormsV2.dll and PTRGrid.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (150, N'E-150', CAST(N'2013-06-08T17:10:03.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-21T21:27:31.000' AS DateTime), 1, N'Maintenance form tab with lookup list control and Add/Modify button.  When the add/modify form shows and then is closed, the lookup list control needs to be refreshed.', N'Peter Ringering - 06/21/2013 09:27:31 PM - Added code to refresh the list control after add/modify form closes.  Changes affect PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (151, N'E-151', CAST(N'2013-06-08T17:19:25.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-21T21:36:57.000' AS DateTime), 1, N'Add-on-the-fly forms should not show in the windows taskbar.', N'Peter Ringering - 06/21/2013 09:36:57 PM - Corrected code to not show maint. forms on the taskbar when in add-on-the-fly mode.  Changes affect PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (152, N'E-152', CAST(N'2013-06-08T17:23:29.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-22T11:10:38.000' AS DateTime), 1, N'When focus leaves a currency or number textbox, the content in the textbox needs to be formatted.', N'Peter Ringering - 06/22/2013 11:10:38 AM - Corrected so that when focus leaves a numeric textbox, it is formatted correctly.  Changes affect PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (153, N'E-153', CAST(N'2013-06-08T19:52:41.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-22T14:44:31.000' AS DateTime), 1, N'When you type Ctrl+C on a lookup edit control, it makes a call to the database to autofill.  Need to not do so on any key when the Ctrl or Alt keys are also down.', N'Peter Ringering - 06/22/2013 02:44:31 PM - Supressed autofill when control key is down.  Alt key is already supressed.  Changes affect PTRFormsV2.dll.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (154, N'E-154', CAST(N'2013-06-08T21:02:50.000' AS DateTime), 5, 28, 2, CAST(N'2013-06-22T14:55:10.000' AS DateTime), 1, N'Bank Account Manager.  The tool tip on the top header buttons  shows "Bank Accounts" (plural).  They should show "Bank Account."', N'Peter Ringering - 06/22/2013 02:55:10 PM - Corrected verbage.  Changes affect Ring2FamilyIS.exe.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (156, N'E-156', CAST(N'2013-06-09T20:22:16.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-22T15:20:10.000' AS DateTime), 1, N'Modify Recurring Template lookup list.  Add Amount and Type columns.', N'Peter Ringering - 06/22/2013 03:20:10 PM - Added the Amount column.  Adding the Type column won''t work because, it is built into the amount column.  Changes affect Ring2FamilyIS.exe.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (157, N'E-157', CAST(N'2013-06-11T21:03:22.000' AS DateTime), 5, 28, 3, CAST(N'2013-06-11T21:06:39.000' AS DateTime), 1, N'Bank Account Manager.  When focus is on the grid and is in edit mode on any column, if you then click on the bank name lookup control on the form, the edit control on the grid does not go away.  If you instead click on the current balance textbox, it works fine.', N'Peter Ringering - 06/11/2013 09:06:39 PM - Found that CausesValidation property on the lookup control was set to false.  Changing it to true made the problem go away.  Changes affect Ring2FamilyIS.exe, all forms.', NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (158, N'E-158', CAST(N'2013-06-22T12:11:24.000' AS DateTime), 5, 28, 5, CAST(N'2013-06-22T14:30:04.000' AS DateTime), 1, N'Bank Account Manager, Transactions grid.  Load up a bank account with transactions.  Goto the first row, Recurring Template column.  Change the value to a new template.  Click Yes to add to the database.  When Recurring Template form shows, select the original template.  Click Save/Select.  Click No on the message box that asks if you want to update the date.  Notice that add to database message appears again (Error #1).  Click No and then the application crashes.', N'Peter Ringering - 06/22/2013 02:30:04 PM - This was occurring in the recurring transactions grid because it can override what''s in the lookup control when the user does an add-on-the-fly.  Corrected so the grid updates the control''s value to what''s in the memory after saving.  It will be the responsibility of the row object to update the passed in value object so that the grid can update the control.  Changes affect PTRFormsV2.dll and Ring2FamilyIS.exe.', NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (159, N'E-159', CAST(N'2014-09-21T10:29:53.000' AS DateTime), 5, 29, 3, CAST(N'2014-09-28T11:38:07.000' AS DateTime), 1, N'ShopperIS, Add/Edit Shopping Lists - When you select an item from the lookup on the unspecified row in the grid, the auto tab tabs out of the grid.  If you type it in and hit TAB, the focus goes to the next row.', N'Peter Ringering - 09/28/2014 11:34:55 AM - Corrected PTRGrid.cs and PTRGridLDBLookupText.cs so EndEdit is called before the cell is moved.  This commits the text and updates the background row object so focus behaves per the new row object.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (160, N'E-160', CAST(N'2014-09-21T10:38:04.000' AS DateTime), 5, 29, 3, CAST(N'2014-10-01T13:55:57.000' AS DateTime), 1, N'ShopperIS, Prepare New Grocery Shopping List.  On the checklist form, double click on the row header on an item row.  Then on the destination grid, go to the quantity column.  Change the value but stay on that cell.  Then double click on a different row header on the checklist.  Notice the quantity value goes back to the original value.', N'Peter Ringering - 10/01/2014 01:55:57 PM - Corrected OnLeave to commit when grid looses focus.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (161, N'E-161', CAST(N'2014-09-22T15:12:05.000' AS DateTime), 5, 28, 2, CAST(N'2014-10-01T14:16:27.000' AS DateTime), 1, N'Print Options Dialog.  When the form starts up, there is no visual cue over the Printer radio button to show that it has focus.  If you right arrow there still is no visual cue.', N'Peter Ringering - 10/01/2014 02:16:27 PM - Added ShowVisualCues = true override per MSDN documentation.  Changes affect PTRRadioButton.cs and PTRCheckbox.cs.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (162, N'E-162', CAST(N'2014-09-22T17:24:21.000' AS DateTime), 5, 28, 2, CAST(N'2014-10-01T15:45:10.000' AS DateTime), 1, N'Print Options dialog, Number of copies textbox.  Type in 100, then with cursor at the end, hit backspace once.  The whole value is erased.  What should happen is for only the last 0 be erased.', N'Peter Ringering - 10/01/2014 03:45:10 PM - Corrected character handling  code in PTRNumericTextBox.cs.  Changes affect all numeric and currency textboxes.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (163, N'E-163', CAST(N'2014-09-30T12:48:35.000' AS DateTime), 5, 29, 3, CAST(N'2014-10-01T16:02:02.000' AS DateTime), 1, N'After changing the base database to a different database, the default grocery checklist and sales tax rate don''t change as well.  To reproduce,log in and notice what the grocery checklist is in Tools, Options.  Now go to Tools, Change Database and select a different database.  Now go back to Tools, Options and notice nothing has changed.', N'Peter Ringering - 10/01/2014 04:02:02 PM - Corrected ChangeDBMenu_Click to reload globals from database after the database is succesfully changed.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (164, N'E-164', CAST(N'2014-09-30T13:07:44.000' AS DateTime), 5, 29, 2, CAST(N'2014-10-01T20:05:27.000' AS DateTime), 1, N'When a form is launched in add-on-the-fly mode, it needs to center to the base form.  To reproduce, goto Add/Edit Shopping Lists.  On the grid, type in something new.  Select add a new Item.  Notice the Add/Edit Items form is centered to the screen.', N'Peter Ringering - 10/01/2014 08:05:27 PM - Corrected centering issues with the maintenance forms and lookups.', N'0', NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (165, N'E-165', CAST(N'2014-09-30T13:16:16.000' AS DateTime), 5, 29, 3, CAST(N'2014-12-17T16:47:39.000' AS DateTime), 1, N'Take out the History tab out of Items form and move notes to be on the form and take out the tab control altogether.', N'Peter Ringering - 12/17/2014 04:47:44 PM - QC Tested and Passed. 
Peter Ringering - 12/17/2014 04:47:39 PM - Fixed. 
', NULL, NULL, CAST(N'2014-12-17T16:47:44.000' AS DateTime), 1, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (166, N'E-166', CAST(N'2014-10-09T14:44:47.000' AS DateTime), 5, 28, 2, CAST(N'2015-01-11T20:51:08.000' AS DateTime), 1, N'Need to Subclass all Tab controls and overridw SetFocusCues property and set to true.', N'Peter Ringering - 01/11/2015 08:53:02 PM - QC Closed.
Peter Ringering - 01/11/2015 08:51:08 PM - Subclassed 1 new form''s tab control and the error did NOT go away.  Therefore I conclude that this is a Microsoft error and cannot be fixed in code.
', NULL, NULL, CAST(N'2015-01-11T20:53:02.000' AS DateTime), 1, 14, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (167, N'E-167', CAST(N'2014-10-09T15:10:16.000' AS DateTime), 5, 28, 3, CAST(N'2014-10-09T21:03:48.000' AS DateTime), 1, N'Rip out all autofill code and just keep box.  Causing problems with data entry.', N'Peter Ringering - 10/09/2014 09:03:48 PM - Fixed code in lookup so that Autofill is shut off but the autofill box stays.  Made functionality work like Visual Studio Intellisense.  Changes affect CTLDBLookupText and Grid.', NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (168, N'E-168', CAST(N'2014-10-11T15:48:41.000' AS DateTime), 5, 29, 5, CAST(N'2014-10-11T16:47:26.000' AS DateTime), 1, N'ShopperIS, Prepare New Shopping List, Type in a valid item and hit TAB.  Go back to the item cell that you just entered.  Type in an  invalid item.  Select Special Order.  You get caught in loop with the validate dialog constantly repeating.', N'Peter Ringering - 10/11/2014 04:47:26 PM - Code was creating a new Item instead of what the user chooses (which in this case was special order) when user tried to overwrite the item which caused double validation. Corrected ROW_SLUnspec so if the new item ID was 0 and new row type was item that the result is to cancel changes.  Else if new item ID was 0 and item text is empty, then we''ll convert it to an unspecific row.', NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (172, N'E-172', CAST(N'2014-12-16T00:00:00.000' AS DateTime), 5, 28, 5, CAST(N'2014-12-17T16:22:26.000' AS DateTime), 1, N'Advanced Find crashes when load from chart definition chart bar is double clicked.  To reproduce, create a chart with a bar that is greater than 0.  Set it as the main chart.  Double click a chart bar.  Click Show Setup.  App crashes.', N'Peter Ringering - 07/24/2015 02:49:05 PM - QA Tested and Passed. 
Peter Ringering - 12/17/2014 04:22:26 PM - Fixed. Had to through a lot of code in the lookups and advanced find to make it so the scrollbar is disabled when the size of the lookup is bigger than the number of records and then when Show Setup is clicked, the scrollbar shows properly since there''s more data than the lookup is displaying.  Had the lookup start over when show/hide setup is clicked.  Changes affect advanced find and lookups.
', NULL, NULL, CAST(N'2015-07-24T14:49:05.000' AS DateTime), 1, 4, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (174, N'E-174', CAST(N'2014-12-17T11:32:51.000' AS DateTime), 5, 28, 2, CAST(N'2015-07-24T14:49:19.000' AS DateTime), 1, N'Advanced Find does not have minimum size set.', N'Peter Ringering - 07/24/2015 02:49:25 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:49:19 PM - Fixed. 
', NULL, NULL, CAST(N'2015-07-24T14:49:25.000' AS DateTime), 1, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (177, N'E-175', CAST(N'2014-12-17T16:10:47.000' AS DateTime), 5, 28, 3, CAST(N'2015-07-24T14:53:24.000' AS DateTime), 1, N'Lookups don''t refresh in the same order as they were in when the user views a row on the fly and then closes the form.  To reproduce.  Go to Tools, Advanced Find.  Create a lookup that contains multiple columns and many records.  Click on Find Now.  Click on the column header of a different column to order by it.  Double click on the record.  Click cancel to get out  of the form.  Notice the order by went back to the first column.', N'Peter Ringering - 07/24/2015 02:53:28 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:53:24 PM - Fixed. 
', NULL, NULL, CAST(N'2015-07-24T14:53:28.000' AS DateTime), 5, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (178, N'E-178', CAST(N'2015-01-09T17:55:15.000' AS DateTime), 5, 28, 3, CAST(N'2015-07-24T18:15:16.000' AS DateTime), 1, N'Getting a SQL Error when user launches an add-on-the-fly form from Advanced Find and the filter has a row pointing to a table that is not the base table.  To reproduce, run SoftDevIS_Outstanding Issues.lkp.  Double click on a row and the Task Issues  form launches. Click on Next.  2 SQL errors display.  Happens in Access and SQL Server.', N'Peter Ringering - 07/24/2015 06:15:55 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 06:15:16 PM - Fixed.   See E-208 for more information.
', NULL, NULL, CAST(N'2015-07-24T18:15:55.000' AS DateTime), 6, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (179, N'E-179', CAST(N'2015-01-09T18:06:24.000' AS DateTime), 5, 28, 3, CAST(N'2015-07-24T14:56:09.000' AS DateTime), 1, N'When a chart bar is  double clicked (which brings up Advanced Find), then the user sets focus to another running program, then sets focus to Advanced Find form, then closes the Advanced Find form, focus does not go to the main form.', N'Peter Ringering - 07/24/2015 02:56:12 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:56:09 PM - Fixed. 
', NULL, NULL, CAST(N'2015-07-24T14:56:12.000' AS DateTime), 6, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (180, N'E-180', CAST(N'2015-01-13T20:28:13.000' AS DateTime), 5, 28, 2, CAST(N'2015-07-24T14:59:40.000' AS DateTime), 1, N'Null values in lookups are formatted which show misinformation.  To reproduce, goto Software DeveloperIS, Project Management, Tasks.  Click on Find.  Notice that null date valuesshow as 01/01/0001 12:00:00 AM.  They should show as blank.', N'Peter Ringering - 07/24/2015 02:59:42 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 02:59:40 PM - Fixed. 
', NULL, NULL, CAST(N'2015-07-24T14:59:42.000' AS DateTime), 14, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (181, N'E-181', CAST(N'2015-01-14T15:57:18.000' AS DateTime), 5, 28, 3, CAST(N'2015-07-24T15:10:02.000' AS DateTime), 1, N'When combo box shows the drop down box and then the user hits ESC, the form closes.  What should happen is when the user  hits ESC, the combo box drop down should close and the form stay active.', N'Peter Ringering - 07/24/2015 03:10:04 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 03:10:02 PM - Fixed. 
', NULL, NULL, CAST(N'2015-07-24T15:10:04.000' AS DateTime), 14, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (182, N'E-182', CAST(N'2015-01-14T16:03:34.000' AS DateTime), 5, 30, 2, CAST(N'2015-02-04T17:44:58.000' AS DateTime), 1, N'When the user closes any form in SoftDevIS, the chart does not refresh to show the changes.', N'David Matthews - 02/05/2015 05:45:56 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 05:44:58 PM - Fixed main form so it refreshes the chart every time form is closed.
', NULL, 3, CAST(N'2015-02-05T17:45:56.000' AS DateTime), 14, 53, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (183, N'E-183', CAST(N'2015-01-17T13:57:28.000' AS DateTime), 5, 28, 4, CAST(N'2015-07-24T15:10:32.000' AS DateTime), 1, N'Change the save routine in maintenance forms so the next record identity number comes back to the client in the same transaction that the header record is saved in.  That way if 2 people try to save a new record at the same time, they won''t get the same identity number.', N'Peter Ringering - 07/24/2015 03:10:33 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 03:10:32 PM - Fixed. 
', NULL, NULL, CAST(N'2015-07-24T15:10:33.000' AS DateTime), 14, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (184, N'E-184', CAST(N'2015-01-19T13:11:18.000' AS DateTime), 5, 30, 4, CAST(N'2019-05-10T15:22:40.000' AS DateTime), 1, N'Forms with a ID field and Description lookup control, don''t load after clicking tab.  To reproduce, in SoftDevIS, go to Error Status form, key in Pending Correction and hit tab. Nothing happens.  Hitting Save will save and  create duplicate records.', N'Peter Ringering - 05/15/2019 12:59:16 PM - QA Tested and Passed. 
Peter Ringering - 05/10/2019 03:22:40 PM - Fixed. 
Peter Ringering - 05/09/2019 04:34:04 PM - Fixed. Corrected code so if the description control has focus and it has a value and the form is in add mode and thwew user clicks save, then it will trigger the description field''s leave event and load up the form and then save.  Changes affect PTRFormsV2.dll and MaintManager.cpp.
', NULL, NULL, CAST(N'2019-05-15T12:59:16.000' AS DateTime), 17, 147, NULL, CAST(N'2019-05-15T12:59:38.000' AS DateTime), N'Peter Ringering', 0.99)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (188, N'E-188', CAST(N'2015-02-04T13:47:00.000' AS DateTime), 5, 30, 2, CAST(N'2015-02-04T17:47:08.000' AS DateTime), 1, N'QA Versions form.  The form should be widened to show all the record maintenance buttons.  Currently there are no icons showing and it looks really ugly.', N'Peter Ringering - 02/04/2015 06:02:37 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 05:47:08 PM - Adjusted form width.
', NULL, NULL, CAST(N'2015-02-04T18:02:37.000' AS DateTime), 22, 53, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (189, N'E-189', CAST(N'2015-02-04T13:56:24.000' AS DateTime), 5, 30, 3, CAST(N'2015-02-04T16:44:42.000' AS DateTime), 1, N'Need a created datetime field added to the versions table and lookup.  It should be required and default to the current date & time.', N'Peter Ringering - 02/04/2015 04:45:13 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 04:44:42 PM - Fixed. Added date to form, db and lookup.
', NULL, NULL, CAST(N'2015-02-04T16:45:13.000' AS DateTime), 22, 49, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (190, N'E-190', CAST(N'2015-02-04T15:22:08.000' AS DateTime), 5, 28, 3, CAST(N'2015-02-04T17:51:12.000' AS DateTime), 1, N'The bank dashboard graph and list do not update after closing the bank manager form.', N'Peter Ringering - 02/04/2015 06:00:46 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 05:51:12 PM - Fixed. 
', NULL, NULL, CAST(N'2015-02-04T18:00:46.000' AS DateTime), 48, 51, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (191, N'E-191', CAST(N'2015-02-04T15:27:02.000' AS DateTime), 5, 30, 3, CAST(N'2015-02-04T18:03:50.000' AS DateTime), 1, N'The main chart does not update after closing the any form.', N'Peter Ringering - 02/04/2015 06:05:07 PM - QA Tested and Passed. 
Peter Ringering - 02/04/2015 06:03:50 PM - Fixed. Overrode the ShowMDIForm to refdresh the chart after a form closes.
', NULL, NULL, CAST(N'2015-02-04T18:05:07.000' AS DateTime), 49, 53, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (192, N'E-192', CAST(N'2015-02-04T17:33:17.000' AS DateTime), 5, 30, 3, CAST(N'2015-07-24T15:15:44.000' AS DateTime), 1, N'Versions form.  Clearing the date does not trigger the dirty flag.  To reproduce, retrieve an already saved version record.  Clear out the date and hit ESC.  No dirty  flag message.', N'Peter Ringering - 07/24/2015 03:15:46 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 03:15:44 PM - Fixed. 
', NULL, NULL, CAST(N'2015-07-24T15:15:46.000' AS DateTime), 49, 87, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (202, N'E-202', CAST(N'2015-05-20T11:58:12.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-20T11:49:59.000' AS DateTime), 1, N'Tasks, Assigned To.  Set default DB value to 0 and remove validation on save.', N'Peter Ringering - 05/20/2019 11:50:20 AM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 11:49:59 AM - Already fixed.
', NULL, NULL, CAST(N'2019-05-20T11:50:20.000' AS DateTime), 65, 148, NULL, CAST(N'2019-05-20T11:50:22.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (196, N'E-196', CAST(N'2015-02-20T15:38:38.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-21T13:47:31.000' AS DateTime), 1, N'The Application column should be removed from the Testing Outlines Errors lookup control.', N'Peter Ringering - 05/22/2019 02:34:39 PM - QA Tested and Passed. 
Peter Ringering - 05/21/2019 01:47:31 PM - Fixed. Updated code so if Product changes, whatever is in the Testing Outline control is erased.  Changes affect frmErrors.css and RSDevLogix.exe.
Peter Ringering - 05/21/2019 12:25:00 PM - QA Fail. Changing the Product value should erase what''s in the Testing Outline control.  Should work like Version control where if you change the Product value, it updates the Found Version control.
Peter Ringering - 05/15/2019 12:51:40 PM - Fixed so that Testing Outline lookup is filtered for the error product like the version lookup.  Had to modify maintmanager so a form could override a control''s lookup definition.  Changes affect PTRFormsV2.dll, MaintManager.cpp, RSDevLogix.exe, and frmErrors.cs.
', NULL, NULL, CAST(N'2019-05-22T14:34:39.000' AS DateTime), 53, 149, 4, CAST(N'2019-05-22T14:34:42.000' AS DateTime), N'Peter Ringering', 2.59)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (198, N'E-198', CAST(N'2015-02-21T14:43:15.000' AS DateTime), 5, 30, 4, CAST(N'2019-05-21T16:07:37.000' AS DateTime), 1, N'Errors form generates the error number before it is saved to the DB.  This will cause problems if 2 people try to save a new error at the same time.  What should happen is to set the new error number to a GUID, save to DB and get the new error ID at the same time, and then change the error number to be correct.', N'Peter Ringering - 05/22/2019 02:39:47 PM - QA Tested and Passed. 
Peter Ringering - 05/21/2019 04:07:37 PM - Fixed. Added functionality to MaintenanceManager to map the form lookup control to a class that implements IMaintControl interface.  Created error number class which implements the IMaintControl interface and mapped it to the error number field.  Had class set error number to a GUID in add mode and whatever is in the edit control in edit mode.  Changes affect PTRFormsV2.dll, MaintManager.cpp, RSDevLogix.exe, ErrorNumber.cpp and frmErroors.cpp.  Need to test all Error form functionality.
Peter Ringering - 05/21/2019 12:33:21 PM - QA Fail. Create a new error.  Keep Description blank.  Save.  Get validation error but Error Number shows GUID which looks ugly.
Peter Ringering - 05/15/2019 05:59:57 PM - Fixed. Corrected so error number is set to a GUID just before saving and then rename it to error number after.  Changes affect PTRDLLV2.dll, GblMethods.cpp, RSDevLogix.exe and frmErrors.cpp.
Peter Ringering - 05/10/2019 03:54:50 PM - QA Fail. 
Peter Ringering - 05/10/2019 03:54:39 PM - Fixed. 
', NULL, NULL, CAST(N'2019-05-22T14:39:47.000' AS DateTime), 53, 149, NULL, CAST(N'2019-05-22T14:39:50.000' AS DateTime), N'Peter Ringering', 3.43)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (199, N'E-199', CAST(N'2015-04-06T17:50:43.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-15T18:03:54.000' AS DateTime), 1, N'Advanced Find does notvalidate to ensure filter left parentheses count equal right parentheses count.', N'Peter Ringering - 05/21/2019 12:42:05 PM - QA Tested and Passed. 
Peter Ringering - 05/15/2019 06:03:54 PM - Can no longer reproduce.
', NULL, NULL, CAST(N'2019-05-21T12:42:05.000' AS DateTime), 56, 148, NULL, CAST(N'2019-05-21T12:42:11.000' AS DateTime), N'Peter Ringering', 0.11)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (200, N'E-200', CAST(N'2015-05-16T12:43:56.000' AS DateTime), 2, 28, 3, NULL, 1, N'Bank Manager, Select a bank account with at least 1 transaction.  Put focus on last blank, disabled row, cleared checkbox cell.  Click on Save.  Notice critical, DataGridView, Error Dialog shows twice.', NULL, NULL, NULL, NULL, 57, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (201, N'E-201', CAST(N'2015-05-20T11:43:56.000' AS DateTime), 5, 30, 3, NULL, 1, N'Tasks Set default percent complete to 0 and remove validation on save.', N'Peter Ringering - 05/20/2019 11:47:29 AM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 11:47:52 AM - Already fixed.', NULL, NULL, CAST(N'2019-05-20T11:47:29.000' AS DateTime), 65, NULL, NULL, CAST(N'2019-05-20T11:48:57.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (203, N'E-203', CAST(N'2015-05-20T12:01:06.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-20T11:53:38.000' AS DateTime), 1, N'Errors, Details Tab, Description and Resolution textboxes.  Set Accepts Return to true.', N'Peter Ringering - 05/20/2019 11:54:01 AM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 11:53:38 AM - Already fixed.
', NULL, NULL, CAST(N'2019-05-20T11:54:01.000' AS DateTime), 65, 148, NULL, CAST(N'2019-05-20T11:54:03.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (204, N'E-204', CAST(N'2015-05-20T12:04:00.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-20T15:47:57.000' AS DateTime), 1, N'Errors and Tools, Options.  Error Number prefix needs to be set by the user to something other than "E".', N'Peter Ringering - 05/21/2019 12:48:24 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 03:47:57 PM - Added functionality.  Changes affect RSDevLogix.exe, FrmOptions.cpp, DevLogixGlobals.cpp, DbConstants.cpp and FrmErrors.cpp.
', NULL, NULL, CAST(N'2019-05-21T12:48:24.000' AS DateTime), 65, 148, NULL, CAST(N'2019-05-21T12:48:27.000' AS DateTime), N'Peter Ringering', 1.25)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (205, N'E-205', CAST(N'2015-05-20T15:11:21.000' AS DateTime), 5, 30, 7, CAST(N'2019-05-20T15:55:18.000' AS DateTime), 1, N'Task Status, Tasl Priority, Task Issue Levels, Versions, Error Priority and Error Status forms need to be widened to show icons.  In add-on-the-fly mode,they currently just show captions in the top buttons with no icons. They look ugly.', N'Peter Ringering - 05/20/2019 03:55:38 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 03:55:18 PM - Already fixed.
', NULL, NULL, CAST(N'2019-05-20T15:55:38.000' AS DateTime), 66, 148, NULL, CAST(N'2019-05-20T15:55:40.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (206, N'E-206', CAST(N'2015-05-20T15:34:02.000' AS DateTime), 5, 30, 7, CAST(N'2019-05-20T15:59:47.000' AS DateTime), 1, N'Change the word "Application" to "Product" on all forms.', N'Peter Ringering - 05/20/2019 04:00:01 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 03:59:47 PM - Already fixed.
', NULL, NULL, CAST(N'2019-05-20T16:00:01.000' AS DateTime), 66, 148, NULL, CAST(N'2019-05-20T16:00:03.000' AS DateTime), N'Peter Ringering', 0)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (208, N'E-208', CAST(N'2015-06-11T13:00:15.000' AS DateTime), 5, 28, 5, CAST(N'2015-07-24T17:58:46.000' AS DateTime), 1, N'Maintenance form,  codes report, show report crashes after loading from advanced find and the filter is on sub table, non-key field.  To reproduce, run Family Bank IS.  Go to Advanced Find, Load RecurTemplatesAdv.lkp.  Click Find Now, then double click on a row to bring up maintenance form.  Click Print.  OK on code dialog and print setup dialog.  Notice the error.', N'Peter Ringering - 07/24/2015 06:06:38 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 05:58:46 PM - Fixed.   Added IsAdvancedFind property to lookup def.  Set to true in Adv. Find.  So when IsAdvFind is true, it will not set the form lookup instance.  Also, changed lookup control to not refresh on return in adv find mode.  That way user doesn''t loose place after launching the maintenance form. Changes affect PTRDLLV2.DLL and PTRFormsV2.DLL.
', NULL, NULL, CAST(N'2015-07-24T18:06:38.000' AS DateTime), 79, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (209, N'E-209', CAST(N'2015-07-02T10:43:26.000' AS DateTime), 5, 28, 3, CAST(N'2015-07-24T18:14:35.000' AS DateTime), 1, N'Launching recur template add on fly window from transaction grid, "Add New Recur Template?" messagebox, does not center the recur template window.  To reproduce, goto Bank Manager.  Select a valid bank account, go to the recurring template cell on the transactions grid.  Type in a new template.  Hit TAB. On the messagebox, click yes to create a new recur template.  Notice the recur template form is not centered to the bank acct manager form.', N'Peter Ringering - 07/24/2015 06:14:41 PM - QA Tested and Passed. 
Peter Ringering - 07/24/2015 06:14:35 PM - Fixed. 
', NULL, NULL, CAST(N'2015-07-24T18:14:41.000' AS DateTime), 79, 85, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (210, N'E-210', CAST(N'2015-07-26T21:50:19.000' AS DateTime), 2, 28, 5, NULL, 1, N'Create a recurring template with Recurs Every set  to 999,999,999 months and setting date to be today.  Now go into Bank Manager for the bank.  Click Generate From Recurring.  Get Unhandled Exception.', NULL, NULL, NULL, NULL, 85, NULL, 10, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (212, N'E-212', CAST(N'2015-08-01T14:48:09.000' AS DateTime), 5, 28, 4, CAST(N'2015-10-08T16:05:21.000' AS DateTime), 1, N'Bank Manager, Transactions Grid, Recurring Template column.  When you type in some text that is not in the database, then click on the Bank Account control, the Create New Recurring Template messagebox shows.  Click on No to not create.  The invalid value stays in the lookup textbox.  Click Save and the row is not saved.  In code, when you key in an invalid value, it saves first, then validates.  Otherwise, if after you enter an invalid value and hit TAB, it erases the text.', N'Peter Ringering - 10/08/2015 04:05:37 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 04:05:21 PM - Fixed awhile ago.
', NULL, NULL, CAST(N'2015-10-08T16:05:37.000' AS DateTime), 93, 130, NULL, CAST(N'2015-10-08T16:05:43.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (242, N'E-242', CAST(N'2019-05-08T11:05:31.000' AS DateTime), 5, 30, 1, NULL, 1, N'Test', N'Peter Ringering - 05/15/2019 05:15:43 PM - QA Tested and Passed. 
', NULL, NULL, CAST(N'2019-05-15T17:15:43.000' AS DateTime), 147, NULL, NULL, CAST(N'2019-05-15T17:15:46.000' AS DateTime), N'Peter Ringering', 2.18)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (216, N'E-213', CAST(N'2015-10-08T14:07:17.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-08T16:29:31.000' AS DateTime), 1, N'When you set a new record in Add/Edit Items, the Category box defaults to "1".', N'Peter Ringering - 10/08/2015 04:34:26 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 04:29:31 PM - Fixed. Added DBDeffaultValue property to DBFieldDef so if is not empty, it will use that value to put in non-null fields during conversion, otherwise, it uses default value.  Changes affect PTRDLLV2.dll.
', NULL, NULL, CAST(N'2015-10-08T16:34:26.000' AS DateTime), 131, 131, 23, CAST(N'2015-10-08T16:34:29.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (217, N'E-217', CAST(N'2015-10-08T14:34:19.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-08T17:46:16.000' AS DateTime), 1, N'When the user launches the item lookup on the last row and selects a record, the value is not returned.', N'Peter Ringering - 10/08/2015 05:55:07 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 05:46:16 PM - Fixed PTRGrid.RecalcIndex so it only refreshes the edit control if the current column is the index column and the edit control is showing.  Code was refreshing the edit anytime recalcindex was called.  This would  cause new value to match old value and prevented DataGridView to not push the value to the row object.  Changes affect PTRGrid.
', NULL, NULL, CAST(N'2015-10-08T17:55:07.000' AS DateTime), 131, 131, 27, CAST(N'2015-10-08T17:55:11.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (218, N'E-218', CAST(N'2015-10-08T14:52:39.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-08T19:02:15.000' AS DateTime), 1, N'Add/Edit Shopping Lists: User can drag/drop last empty row.  This should not be allowed.', N'Peter Ringering - 10/08/2015 07:03:27 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 07:02:15 PM - Fixed so that last row cannot be moved unless the grtid is a fixed grid.  Changes affect PTRGrid.
', NULL, NULL, CAST(N'2015-10-08T19:03:27.000' AS DateTime), 131, 131, 27, CAST(N'2015-10-08T19:03:31.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (219, N'E-219', CAST(N'2015-10-08T14:58:09.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-08T21:08:26.000' AS DateTime), 1, N'Add/Edit Shopping Lists:  When the user launches the context menu on the last empty row and the grid has other rows, Clear Grid is disabled.', N'Peter Ringering - 10/08/2015 09:16:47 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 09:08:26 PM - Code was disabling clear grid when delete row was disabled Fixed PTRGrid so that clear grid is enabled when there is more than 1 row or in fixed grid mode.  Also fixed so in fixed grid mode it removes all rows from the data source.
', NULL, NULL, CAST(N'2015-10-08T21:16:47.000' AS DateTime), 131, 131, 27, CAST(N'2015-10-08T21:16:50.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (220, N'E-220', CAST(N'2015-10-08T15:06:52.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-08T21:30:10.000' AS DateTime), 1, N'Prepare New Grocery Shopping List.  Tab order is incorrect.  Tabs from Close to Select All checkbox then to Help button.', N'Peter Ringering - 10/08/2015 09:32:01 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 09:30:10 PM - Fixed tab order.  Also corrected error where ENTER to TAB works on any form that doesn''t have an accept button.
', NULL, NULL, CAST(N'2015-10-08T21:32:01.000' AS DateTime), 131, 131, 21, CAST(N'2015-10-08T21:32:08.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (221, N'E-221', CAST(N'2015-10-08T15:54:19.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-08T21:43:07.000' AS DateTime), 1, N'Tools/Options:  Accept button not set.', N'Peter Ringering - 10/08/2015 09:43:14 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 09:43:07 PM - Fixed. 
', NULL, NULL, CAST(N'2015-10-08T21:43:14.000' AS DateTime), 131, 131, 28, CAST(N'2015-10-08T21:43:17.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (222, N'E-222', CAST(N'2015-10-08T15:58:24.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-08T21:43:53.000' AS DateTime), 1, N'Tools/Options:  Sales Tax has no max value set.', N'Peter Ringering - 10/08/2015 09:44:03 PM - QA Tested and Passed. 
Peter Ringering - 10/08/2015 09:43:53 PM - Fixed. 
', NULL, NULL, CAST(N'2015-10-08T21:44:03.000' AS DateTime), 131, 131, 28, CAST(N'2015-10-08T21:44:06.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (223, N'E-223', CAST(N'2015-10-11T10:30:26.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-11T17:17:44.000' AS DateTime), 1, N'When a lookup textbox has focus and all its content is selected, when the backspace key is pressed, nothing happens.  It should erase all the selected text.', N'Peter Ringering - 10/11/2015 05:19:59 PM - QA Tested and Passed. 
Peter Ringering - 10/11/2015 05:17:44 PM - Fixed. PTRFormsV2.dll.CTLDBLookupText.  This and many other errors started when the autofill box was introduced.  There are many different scenarios where this would fail.  Corrected them all.
', NULL, NULL, CAST(N'2015-10-11T17:19:59.000' AS DateTime), 131, 131, NULL, CAST(N'2015-10-11T17:20:03.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (224, N'E-224', CAST(N'2015-10-11T22:04:37.000' AS DateTime), 5, 29, 3, CAST(N'2015-10-15T12:18:55.000' AS DateTime), 1, N'Advanced Find - Add 3 columns.  On the columns grid, drag the last row and drop it on the first row.  Get duplicate column message and a critical error dialog.', N'Peter Ringering - 10/15/2015 12:23:33 PM - QA Tested and Passed. 
Peter Ringering - 10/15/2015 12:18:55 PM - Fixed. Put code in OnCellValidating override so it doesn''t validate in the middle of drag/drop operations.
', NULL, NULL, CAST(N'2015-10-15T12:23:33.000' AS DateTime), 131, 131, NULL, CAST(N'2015-10-15T12:23:39.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (226, N'E-226', CAST(N'2015-10-11T22:17:27.000' AS DateTime), 1, 29, 3, NULL, 1, N'Advanced Find.  Add 3 columns but do not click on Find Now.  In the list control, click on the 2nd column header to sort.  Note that the sort arrow stays on the first column.  Click Find Now and then sort on any column.  The sort arrow still stays on the first column.', NULL, NULL, NULL, NULL, 131, NULL, NULL, CAST(N'2016-06-25T12:16:26.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (227, N'E-227', CAST(N'2018-10-16T14:39:10.000' AS DateTime), 5, 30, 4, CAST(N'2019-05-20T16:16:08.000' AS DateTime), 1, N'User login.  When you key in a user name and then press ENTER, it tries to save the user name right away.  It should go to the next control on the form.  To reproduce, start the app, type in part of a username that already exists in the database.  DO NOT PRESS TAB.  Press ENTER, notice, it saves the partial username.  It should login the selected username in the dropdown box.', N'Peter Ringering - 05/20/2019 04:16:23 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 04:16:08 PM - Already fixed.
', NULL, NULL, CAST(N'2019-05-20T16:16:23.000' AS DateTime), 139, 148, NULL, CAST(N'2019-05-20T16:16:25.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (228, N'E-228', CAST(N'2018-10-22T16:04:12.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-20T16:09:07.000' AS DateTime), 1, N'All maintenance forms.  The "Save" button should always be enabled.', N'Peter Ringering - 05/20/2019 04:09:25 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 04:09:07 PM - Already fixed.
', NULL, NULL, CAST(N'2019-05-20T16:09:25.000' AS DateTime), 139, 148, NULL, CAST(N'2019-05-20T16:09:27.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (229, N'E-229', CAST(N'2018-10-22T16:12:45.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-21T16:28:27.000' AS DateTime), 1, N'All maintenance forms.  Need to warn user when renaming table description values.', N'Peter Ringering - 05/22/2019 02:44:03 PM - QA Tested and Passed. 
Peter Ringering - 05/21/2019 04:28:27 PM - Fixed form record names.  Changes affect RSDevLogix.exe, PTRFormsV2.dll and PTRMaintForm.cpp.
Peter Ringering - 05/21/2019 01:06:50 PM - QA Fail. 
The following form''s unique error message show''s "*s''s" instead of "*''s."
* Testing Outlines
* Testing Templates
* Errors
* Advanced Find
* Chart Definition
Peter Ringering - 05/20/2019 04:09:40 PM - Already fixed.
', NULL, NULL, CAST(N'2019-05-22T14:44:03.000' AS DateTime), 139, 149, NULL, CAST(N'2019-05-22T14:44:06.000' AS DateTime), N'Peter Ringering', 0.96)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (231, N'E-231', CAST(N'2018-12-10T12:01:24.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-20T16:10:33.000' AS DateTime), 1, N'Advanced Find, percent field filters.  Needs to show percent sign.  To reproduce, goto Project Management, Projects.  Select a project with tasks.  Click the "Tasks" tab.  Click "Advanced".  Select the "Percent Complete" field.  Click "Add Filter".  Notice the % sign is missing.  Put in "Less Than 100".  Click OK.  Notice, it shows all records including those that are 100% complete.', N'Peter Ringering - 05/21/2019 01:37:03 PM - QA Tested and Passed. 
Peter Ringering - 05/20/2019 04:10:33 PM - Already ixed.
', NULL, NULL, CAST(N'2019-05-21T13:37:03.000' AS DateTime), 139, 148, NULL, CAST(N'2019-05-21T13:37:05.000' AS DateTime), N'Peter Ringering', 0.06)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (232, N'E-232', CAST(N'2019-02-20T14:11:01.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-22T13:37:58.000' AS DateTime), 1, N'Tools, Options, Holidays grid.  When you change a holiday date value, then click OK.  It saves the data then brings up the dirty flag message.', N'Peter Ringering - 05/22/2019 02:46:51 PM - QA Tested and Passed. 
Peter Ringering - 05/22/2019 01:37:58 PM - Fixed. Set dirty flag to false after a successful save.  Changes affect RSDevLogix.exe and FrmOptions.cpp.
', NULL, NULL, CAST(N'2019-05-22T14:46:51.000' AS DateTime), 139, 149, NULL, CAST(N'2019-05-22T14:46:53.000' AS DateTime), N'Peter Ringering', 0.22)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (233, N'E-233', CAST(N'2019-05-04T13:09:37.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-22T13:58:11.000' AS DateTime), 1, N'Lookups.  Clicking on ''Add'' on an existing record, then clicking ''New'' trips the dirty flag message.  To reproduce, goto DevLogix, Quality Assurance, Add/Edit Errors.  Bring up an existing error.  Go to the Status field.  Do a lookup.  Click ''Add''.  Click ''New''  Notice the dirty flag message eventhough nothing has changed.', N'Peter Ringering - 05/22/2019 02:52:17 PM - QA Tested and Passed. 
Peter Ringering - 05/22/2019 01:58:11 PM - Fixed. Corrected code so that when the user launches a maintenance form (like Error Status) by clicking Add from the lookup form, to only raise the dirty flag if the record doesn''t exist.  Changes affect all maintenance forms.  Changes affect PTRFormsV2.dll and PTRMaintForm.cpp.
', NULL, NULL, CAST(N'2019-05-22T14:52:17.000' AS DateTime), 145, 149, NULL, CAST(N'2019-05-22T14:52:31.000' AS DateTime), N'Peter Ringering', 0.39)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (234, N'E-234', CAST(N'2019-05-04T13:27:50.000' AS DateTime), 1, 30, 3, NULL, 1, N'Chart Definition.  Changing an existing Advanced Find, does not update the Caption value.  To reproduce, go to Tools, Chart Definition.  Bring up an existing chart.  Change an Advanced Find and press Tab.  Notice the caption doesn''t change.', NULL, NULL, NULL, NULL, 145, NULL, NULL, CAST(N'2019-05-04T13:32:51.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (237, N'E-237', CAST(N'2019-05-04T14:00:00.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-04T15:53:25.000' AS DateTime), 1, N'Advanced Find.  Percent fields.  Need to add Percent control when filtering percent fields.  To reproduce, go to DevLogix, Tools, Advanced Find.  Select ''Testing Outlines'' table.  Select ''Percent Complete'' field.  Click Add Filter.  Notice Search Value show "0".  It should show 0%.', N'Peter Ringering - 05/07/2019 02:11:10 PM - QA Tested and Passed. 
Peter Ringering - 05/04/2019 03:53:25 PM - Fixed. Added PTRPercentTextBox to FrmAdvFindFilterDef form.  Had to fix a small error with currency and numeric searches on <> type searches.  Changes affect PTRFormsV2.dll, FrmAdvFindFilterDef.cs and PTRPercentTextBox.cs.
', NULL, 1, CAST(N'2019-05-07T14:11:10.000' AS DateTime), 145, 146, NULL, CAST(N'2019-05-07T14:11:21.000' AS DateTime), N'Peter Ringering', 0.83)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (239, N'E-239', CAST(N'2019-05-04T14:21:36.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-08T10:54:00.000' AS DateTime), 1, N'Advanced Find.  Filtering on an Advanced Find definition with no filters and then adding another filter does not filter.  To reproduce, go to DevLogix, Tools, Advanced Find.  Select ''Testing Outlines'' table.  Click Load Default.  In Fields list, Select <Advanced Find Filter>.  Click ''Add Filter''.  Do a lookup and click ''Add''.  Click ''Load Default''.  Type in a Title and click Save/Select.  In the derived Advanced Find Goto ''Product'' column.  Click Add Filter.  Search Type is ''=''.  Type in a product to search for.  Click OK.  Notice it does not search for that product.', N'Peter Ringering - 05/15/2019 01:05:08 PM - QA Tested and Passed. 
Peter Ringering - 05/08/2019 10:54:00 AM - Fixed.  Added validation so Advanced Find won''t run if an Advanced Find filter has no filters defined.  Also fixed error that was allowing delete of an Advanced Find record that is an Advanced Find Filter of another Advanced Find.  Changes affect PTRFormsV2.dll and PTRDLLV2.dll and multiple .css files.
Peter Ringering - 05/07/2019 02:35:00 PM - QA Fail. Adding a no filter advanced find after an existing filter returns a SQL Execution Error.  To reproduce, follow steps in Description but put in the no filter advanced find definition AFTER the search for Product filter.
Peter Ringering - 05/04/2019 02:50:30 PM - Fixed. Code was returning an empty query if the Advanced Find filter was empty.  Corrected so it would go to next filter in this situation.  Changes affect PTRDDLLV2.DLL, DbFilterDef.cs.
', NULL, NULL, CAST(N'2019-05-15T13:05:08.000' AS DateTime), 145, 147, NULL, CAST(N'2019-05-15T13:05:30.000' AS DateTime), N'Peter Ringering', 0.62)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (240, N'E-240', CAST(N'2019-05-04T16:04:25.000' AS DateTime), 1, 30, 4, NULL, 1, N'Task Punch In/Out.  Selecting a task, punching in, and then deleting causes orphan time clock record.  To reproduce, goto Project Management, Add/Edit Tasks.  Create a task and save.  Punch In.  Delete.  Then punch out.  Time clock record is lost.', NULL, NULL, NULL, NULL, 146, NULL, NULL, CAST(N'2019-05-04T16:11:33.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (241, N'E-241', CAST(N'2019-05-07T14:13:19.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-22T14:21:59.000' AS DateTime), 1, N'Advanced Find.  The lookup doesn''t refresh after modifying a found record.  To reproduce, goto Tools, Advanced Find.  Select the Errors Table.  Click Load Default.  Select Status and click Add Filter.  Set it to = Pending Correction.  Go to the lookup and modify a record.  Change the status to Open.  Click Save and then close the form.  Notice the lookup does not refresh.', N'Peter Ringering - 05/22/2019 02:52:54 PM - QA Tested and Passed. 
Peter Ringering - 05/22/2019 02:21:59 PM - Fixed. Error was introduced in fix for E-208.  Corrected so advanced find refreshes when maintenance form closes.  Changes affect PTRForrmsV2.dll and CtlDbLookup.cs.', NULL, NULL, CAST(N'2019-05-22T14:52:54.000' AS DateTime), 147, 149, NULL, CAST(N'2019-05-22T14:52:56.000' AS DateTime), N'Peter Ringering', 0.22)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (243, N'E-243', CAST(N'2019-05-09T14:46:52.000' AS DateTime), 5, 30, 3, CAST(N'2019-05-09T15:21:39.000' AS DateTime), 1, N'Tab key doesn''t work after Alt + Tab to switch to another program then alt tab back to main form.  To reproduce, launch DevLogix.  Press Alt + Tab to switch to another program.  Press Alt + Tab to switch back to DevLogix.  Press tab key and notice nothing happens.  Press tab again and the File menu is highlighted.  You should not have to press tab twice in this scenario to get the menu bar highlighted.', N'Peter Ringering - 05/09/2019 03:23:30 PM - QA Tested and Passed. 
Peter Ringering - 05/09/2019 03:21:39 PM - This is happening on MS Word and Outlook as well.  This is a computer problem and not an error specific to the DevLogix application.
', NULL, NULL, CAST(N'2019-05-09T15:23:30.000' AS DateTime), 147, 147, NULL, CAST(N'2019-05-09T15:24:51.000' AS DateTime), N'Peter Ringering', 0.5)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (254, N'E-254', CAST(N'2019-05-21T12:50:36.000' AS DateTime), 1, 30, 3, NULL, 1, N'Tools, Options.  Changing a value then saving and exiting app causes main chart to go back to icons.  To reproduce, set the Dashboard item to be Custom Chart.  Then exit app and go back in.  Goto Tools, Options.  Change Error Prefix to be something different.  Click OK.  Exit app.  Go back in and notice the main chart is gone and it''s back to icons.', NULL, NULL, NULL, NULL, 149, NULL, NULL, CAST(N'2019-05-21T15:14:27.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (269, N'E-269', CAST(N'2019-06-01T12:29:09.000' AS DateTime), 8, 30, 3, CAST(N'2019-06-01T12:58:56.000' AS DateTime), 1, N'Advanced Find.  Lookup Definitions with formula columns don''t print the results of a formula.  To reproduce, go to Errors.  Select an error that has Time Clock entries.  On The Time Clock tab, click Advanced.  Click Print.  Notice the Hours Spent column is blank.', N'Peter Ringering - 06/01/2019 12:58:56 PM - Fixed. Added formula columns to report object.  Changes affect PTRFormsV2.dll and FrmAdvancedFind.cpp.
', NULL, NULL, NULL, 151, 151, NULL, CAST(N'2019-06-01T13:00:26.000' AS DateTime), N'Peter Ringering', 0.45)
GO
INSERT [dbo].[TB_Errors] ([intErrorID], [strErrorNo], [dteDate], [intStatusID], [intProductID], [intPriorityID], [dteFixedDate], [intAssignedToID], [txtDescription], [txtResolution], [decEstHrs], [intTesterID], [dteCompletedDate], [intFoundVersionID], [intFixedVersionID], [intOutlineID], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (270, N'E-270', CAST(N'2019-08-06T15:38:48.000' AS DateTime), 1, 30, 3, NULL, 1, N'Tasks form.  When viewing a task from the Project Tasks control, the Remaining Hours control is 0.  To reproduce, goto Project Management, Projects window.  Click Find and select a project that has tasks.  Click on the Tasks tab.  Select a task that has Remaining Hours <> 0.  When the Task window loads up, notice the Remaining Hours is 0.', NULL, NULL, NULL, NULL, 151, NULL, NULL, CAST(N'2019-08-06T15:46:33.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (5, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (7, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (8, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (12, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (13, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (14, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (17, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (18, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (23, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (24, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (27, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (28, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (29, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (31, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (32, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (35, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (36, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (37, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (38, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (43, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (44, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (52, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (53, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (54, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (55, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (56, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (57, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (58, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (59, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (60, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (61, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (62, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (63, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (64, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (65, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (66, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (67, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (68, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (69, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (70, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (71, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (72, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (73, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (74, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (75, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (76, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (77, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (78, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (79, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (80, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (81, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (82, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (83, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (84, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (85, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (86, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (87, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (88, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (89, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (90, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (91, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (92, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (94, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (95, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (96, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (97, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (98, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (99, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (100, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (101, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (102, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (103, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (104, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (107, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (108, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (109, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (111, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (112, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (113, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (114, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (115, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (117, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (119, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (120, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (121, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (123, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (124, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (125, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (126, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (127, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (128, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (129, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (130, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (131, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (132, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (133, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (134, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (135, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (136, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (137, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (138, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (139, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (140, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (141, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (142, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (143, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (144, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (145, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (146, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (147, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (148, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (149, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (150, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (151, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (152, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (153, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (154, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (155, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (156, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (157, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (158, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (159, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (160, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (161, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (162, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (163, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (164, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (165, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (166, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (167, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (168, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (172, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (174, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (177, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (178, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (179, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (180, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (181, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (182, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (183, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (188, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (189, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (190, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (191, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (192, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (193, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (208, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (209, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (212, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (216, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (217, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (218, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (219, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (220, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (221, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (222, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (223, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (224, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (237, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (243, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (194, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (184, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (239, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (202, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (203, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (205, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (206, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (228, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (227, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (199, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (204, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (231, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (196, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (198, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (229, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (232, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (233, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (241, 1)
GO
INSERT [dbo].[TB_ErrorsFixedBy] ([intErrorID], [intUserID]) VALUES (269, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (5, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (7, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (8, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (9, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (10, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (11, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (12, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (13, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (14, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (17, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (18, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (19, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (23, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (24, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (25, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (27, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (28, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (29, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (31, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (32, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (35, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (35, 4)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (36, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (36, 4)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (37, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (38, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (43, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (44, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (52, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (53, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (54, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (55, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (56, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (57, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (58, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (59, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (60, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (61, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (62, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (63, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (64, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (65, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (66, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (67, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (68, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (69, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (70, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (71, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (72, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (73, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (74, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (75, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (76, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (77, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (78, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (79, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (80, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (81, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (82, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (83, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (84, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (85, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (86, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (87, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (88, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (89, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (90, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (91, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (92, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (93, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (94, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (95, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (96, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (97, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (98, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (99, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (100, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (101, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (102, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (103, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (104, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (107, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (108, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (109, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (111, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (112, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (113, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (114, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (115, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (117, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (119, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (120, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (121, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (123, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (124, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (125, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (126, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (127, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (128, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (129, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (130, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (131, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (132, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (133, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (134, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (135, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (136, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (137, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (138, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (139, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (140, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (141, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (142, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (143, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (144, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (145, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (146, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (147, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (148, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (149, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (150, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (151, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (152, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (153, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (154, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (155, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (156, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (157, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (158, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (159, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (160, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (161, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (162, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (163, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (164, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (165, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (166, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (167, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (168, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (172, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (173, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (174, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (177, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (178, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (179, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (180, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (181, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (182, 3)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (183, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (188, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (189, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (190, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (191, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (192, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (193, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (200, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (207, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (208, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (209, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (210, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (212, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (216, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (217, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (218, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (219, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (220, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (221, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (222, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (223, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (224, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (225, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (226, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (234, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (235, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (236, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (237, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (240, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (243, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (194, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (184, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (239, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (242, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (201, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (202, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (203, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (205, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (206, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (228, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (227, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (199, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (204, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (231, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (254, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (196, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (198, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (229, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (232, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (233, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (241, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (269, 1)
GO
INSERT [dbo].[TB_ErrorsFoundBy] ([intErrorID], [intUserID]) VALUES (270, 1)
GO
INSERT [dbo].[TB_ErrorStatus] ([intStatusID], [strStatus], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Pending Correction', CAST(N'2019-05-15T12:57:59.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_ErrorStatus] ([intStatusID], [strStatus], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'Open', NULL, NULL)
GO
INSERT [dbo].[TB_ErrorStatus] ([intStatusID], [strStatus], [dteModifiedDate], [strModifiedBy]) VALUES (3, N'Pending Unit Test', NULL, NULL)
GO
INSERT [dbo].[TB_ErrorStatus] ([intStatusID], [strStatus], [dteModifiedDate], [strModifiedBy]) VALUES (5, N'Closed', NULL, NULL)
GO
INSERT [dbo].[TB_ErrorStatus] ([intStatusID], [strStatus], [dteModifiedDate], [strModifiedBy]) VALUES (6, N'Design', NULL, NULL)
GO
INSERT [dbo].[TB_ErrorStatus] ([intStatusID], [strStatus], [dteModifiedDate], [strModifiedBy]) VALUES (8, N'Pending QA Test', CAST(N'2019-05-09T16:29:37.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_ErrorStatus] ([intStatusID], [strStatus], [dteModifiedDate], [strModifiedBy]) VALUES (10, N'Test', NULL, NULL)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (285, 0, 492, CAST(6.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (285, 1, 490, CAST(6.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (286, 0, 492, CAST(4.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (286, 1, 493, CAST(2.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (286, 2, 495, CAST(2.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (286, 3, 497, CAST(0.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (287, 0, 500, CAST(8.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (288, 0, 500, CAST(4.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (288, 1, 497, CAST(4.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (291, 0, 0, CAST(8.00 AS Decimal(18, 2)), 0, 9, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (292, 0, 0, CAST(7.00 AS Decimal(18, 2)), 0, 9, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (292, 1, 0, CAST(1.00 AS Decimal(18, 2)), 0, 10, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (293, 0, 0, CAST(8.00 AS Decimal(18, 2)), 0, 10, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (294, 0, 0, CAST(8.00 AS Decimal(18, 2)), 0, 10, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (294, 1, 0, CAST(0.00 AS Decimal(18, 2)), 0, 20, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (296, 0, 0, CAST(2.00 AS Decimal(18, 2)), 0, 9, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (296, 1, 0, CAST(2.00 AS Decimal(18, 2)), 0, 10, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (296, 2, 0, CAST(2.00 AS Decimal(18, 2)), 0, 21, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (297, 0, 0, CAST(8.00 AS Decimal(18, 2)), 0, 21, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (298, 0, 0, CAST(8.00 AS Decimal(18, 2)), 0, 21, 1)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (299, 0, 490, CAST(2.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (300, 0, 523, CAST(4.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (301, 0, 523, CAST(8.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (302, 0, 523, CAST(2.50 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (302, 1, 525, CAST(5.50 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (303, 0, 526, CAST(5.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (303, 1, 523, CAST(1.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (304, 0, 523, CAST(7.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (305, 0, 523, CAST(7.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (306, 0, 527, CAST(2.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (306, 1, 523, CAST(2.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (307, 0, 527, CAST(2.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (307, 1, 523, CAST(7.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (308, 0, 523, CAST(6.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (308, 1, 527, CAST(2.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_GoalDetails] ([intGoalID], [intGoalDetailID], [intTaskID], [decHrsToSpend], [intErrorID], [intOutlineID], [bytLineType]) VALUES (309, 0, 527, CAST(9.00 AS Decimal(18, 2)), 0, 0, 0)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (281, 1, CAST(N'2015-05-11T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (284, 1, CAST(N'2015-05-18T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (285, 1, CAST(N'2015-05-19T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (286, 1, CAST(N'2015-05-20T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (287, 1, CAST(N'2015-05-21T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (288, 1, CAST(N'2015-05-22T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (291, 1, CAST(N'2015-07-25T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (292, 1, CAST(N'2015-07-26T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (293, 1, CAST(N'2015-07-27T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (294, 1, CAST(N'2015-07-28T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (296, 1, CAST(N'2015-07-29T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (297, 1, CAST(N'2015-07-30T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (298, 1, CAST(N'2015-08-11T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (299, 1, CAST(N'2018-10-09T00:00:00.000' AS DateTime), CAST(2.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2018-10-08T16:44:32.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (300, 1, CAST(N'2019-08-19T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-20T10:07:30.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (301, 1, CAST(N'2019-08-20T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-20T14:24:17.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (302, 1, CAST(N'2019-08-21T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-21T17:52:54.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (303, 1, CAST(N'2019-08-22T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-22T12:03:53.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (304, 1, CAST(N'2019-08-23T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-23T10:45:50.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (305, 1, CAST(N'2019-08-24T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-24T11:10:06.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (306, 1, CAST(N'2019-08-25T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-25T10:08:47.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (307, 1, CAST(N'2019-08-26T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-26T21:18:24.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (308, 1, CAST(N'2019-08-27T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-27T21:08:19.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Goals] ([intGoalID], [intUserID], [dteGoalDate], [decWorkingHrs], [txtBeginNotes], [txtEndNotes], [dteModifiedDate], [strModifiedBy]) VALUES (309, 1, CAST(N'2019-08-28T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), NULL, NULL, CAST(N'2019-08-28T10:19:44.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Groups] ([intGroupID], [strGroupName], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Supervisors', N'KBBs78uo5EIRXasbqJ/VX0xUWVkiXfsQr6ZkhFEhDfVEviFuySZBSbNW7ALhW3HvXF4csvWubvjjvDlws9qPOBadwyqy6rLQKf11+sU9BfFgquT+nZ3Zdxlob0x6vkYGo+Ug5OhbKRFouozZIpb0u/sVIyx5Me7VbtBtv9qXLU/vCpESN1AgIEovTg2fSjEk3Ds5o81ZRj0XrQGKEue965WRk43N8jS14k/cKBYAaeQUqnzdUAdPDmHun0k1nPwk251QVXRl6jUAdj9imoPOEINK/A42qT76u2KPG4r9yCM5FbR8CLMB1mmzfwBn7Xbs3lEFuJbbhnJykte5FiA8nHaRf3/JdOxsAElVc5GW0FZYPOBPnMElLBfYkIdBJU7kD8UCFirjpnqualF9Sg2eLXitSIYBK4IYBeRFPz8w+aWQhwY1yiLz1VDdD1g6RIGO2kYYohH426MuhWLusp2z6M2eEjdVCMoONFXOi2Hokgrw5Vkiqaear3qR3wz/BSVJb/v7XFhfSG7XU+diHTc9BJNk39LUa+4pUp3AsL92bYfQ42cJISGNS5Q2QNTmeXylUzUEU6LLDQPy0bo/APJ4bYXZOQNpMBB7j1xsEJ2Gp+2LiACScsxxY7lp3BfdGI5gZFua1cNASKJyZnRk6FXZEt5nXX9p44okKgcuWZ74T6witsCDnIIDQyOe8BbvddRSKsjIMehqfvSwEwZlDvsPCWu7uHtYJosCOQ+K5YIjD+ko7EeYNsojLkTq63aGenMI9wqdyz6jK93+Oyzul3dB7BZgDy+kXwYfVa2acuF982lEm3OddTbsPQjK4Dq9/8V4TfzVoPdmGJWh7RRhchwaXgmBLrj/PEHnXYjXis4yPic=', CAST(N'2015-09-14T14:27:20.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Groups] ([intGroupID], [strGroupName], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'QC Users', N'tULv9UCW6mm1n0iwRCyltMdIdsnHt53pyOZl0JB2jl4X0VqKm1ra+nrIxepqIAidO3vFGSpgW1hR8D1mFJVVTSfMtKsN4/GmVReX4tEuRuo9lwGL1cLXdFDLAsVirngy5BJYTLDk3r9fl7/42OGHIJKrb+rS3CBg7L86tsdGJ9EbfNu4HxHhF6sfurnFsRTjyFN8bRAcArMGVuZuVM+DpimgRODKD9eRfoeUn/XBNxsJ88MY9Y2MyS8MQOARuwHYTrKW0Ysa1jEbwC5Fp1A7jYf4nGB+cUUn5eRMmr5+yl+NECUpfsj7butqgqaKDs+4Mv2lXCB11RkB7o+MEs8pqGd7nOLwALOaNnO5FJsKhqfPbRv9AjtSavNjYZmBfR2kMzMheqASqdPIheADzerYQettLlyzeP+fyfM0Kh3JymlyBKav0AY67cQV7Ue00TovuedqVCcnv+I8Q2hkhicYAiYwqe4G3Fb9IFf5Mq2Ldvl3R2F4T824nNiSWCfPI67+cKWBEmCFypxQ/kzz+aYC26Z0KM+tnNm4csr52gwmas79DzZca0KLv4xYW4ZJGjUoX4Sw/L696ZDl3YY2p4QJ5Jc2lCmPR68V1ygYKhc2CkOs2GBwBdd7+y+HpBEp2FJxi9SYidthWM75CWL+fb/CKWsBYNErtT3sqcuAgNlHD5xDV3UFYRQV0DCUw58NB7UH', NULL, NULL)
GO
INSERT [dbo].[TB_Groups] ([intGroupID], [strGroupName], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (3, N'Programmers', N'tULv9UCW6mm1n0iwRCyltMVnVGf5rJ84TEQBLRq1rc7+l+YEsZ92rURQhSoXm4dNEz3uto4NoQ1pRzBwELHLCHyROKI150Skut4xjtMcyJHrre2XTCGIFLd2iS/WV8dhk25qsXZDYQ+FuU+UK8Q5LBtmiOW+9C/aOPUoke7Pvo8xeEOLhngpjETupR/NYYfGnwUaaqezYbWbbQ6xu4qblS30cgpMSpA2+LH+Hru83BSIbg3RiJI/sKUBUcnc1pQqMvP9utsqEnls42JXqXTy/tFLRT9cxL5TXwcW15N+kInQ6cy5NmZp2E0OSp+DxBuuegSNfARiyio5bBPwP9dvaL0kKFDueMsdIxTd6AqbwM1u5Ko5I4Ixa1mOwNY4syztg5IVzIPF31Dw5xfQooJPzn4Wo+p3t/Eo9tNU5KVHX1AyYYYsh5WqU3AW5cDvQ7BSGRzji46XAhbv5zuMQ6e4BBGcJFXj2cbSuRc55w0gWhlqsZzwX58C1TZ8NknRIuiFzFwUxdu4hZwxXp4DRZ6EiJqz7HPEPotRAOxpC0wudP6upMZJ3fCVZZGfbAJBpxBm1EgbBCwghY2S82eYDNbwLahq/7eQUVdREK0qQ3gCOfXYsZUlZtikFXKEerydSnWOtiVheuroPpoIySp06CBbSiWtwPd2UhjZTf8gJtPBuS8=', NULL, NULL)
GO
INSERT [dbo].[TB_Holidays] ([dteHoliday], [strDescription]) VALUES (CAST(N'2019-05-27T00:00:00.000' AS DateTime), N'Memorial Day')
GO
INSERT [dbo].[TB_Holidays] ([dteHoliday], [strDescription]) VALUES (CAST(N'2019-07-04T00:00:00.000' AS DateTime), N'4th of July')
GO
INSERT [dbo].[TB_Holidays] ([dteHoliday], [strDescription]) VALUES (CAST(N'2019-09-02T00:00:00.000' AS DateTime), N'Labor Day')
GO
INSERT [dbo].[TB_Holidays] ([dteHoliday], [strDescription]) VALUES (CAST(N'2019-11-28T00:00:00.000' AS DateTime), N'Thanksgiving Day')
GO
INSERT [dbo].[TB_Holidays] ([dteHoliday], [strDescription]) VALUES (CAST(N'2019-12-25T00:00:00.000' AS DateTime), N'Christmas Day')
GO
INSERT [dbo].[TB_Holidays] ([dteHoliday], [strDescription]) VALUES (CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'New Years Day')
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Issue', 100, NULL, NULL)
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'Milestone 01', 200, NULL, NULL)
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (3, N'Milestone 02', 300, NULL, NULL)
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (4, N'Milestone 03', 400, NULL, NULL)
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (5, N'Milestone 04', 500, NULL, NULL)
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (6, N'Milestone 05', 600, NULL, NULL)
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (7, N'Milestone 06', 700, NULL, NULL)
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (8, N'Milestone 07', 800, CAST(N'2015-09-19T15:28:23.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_IssueLevels] ([intIssueLevelID], [strDescription], [intLevelNo], [dteModifiedDate], [strModifiedBy]) VALUES (9, N'Subtask', 100, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (373, 471, N'Grid row height is too large', 1, CAST(N'2012-05-26T11:43:25.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (375, 471, N'Alternate cell light blue/light yellow on grid', 1, CAST(N'2012-05-26T16:49:37.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (376, 471, N'Inventory Line', 1, CAST(N'2012-05-28T17:18:22.000' AS DateTime), N'x Validate on leaving lookup cell.', 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (377, 471, N'Validate when changing line types', 1, CAST(N'2012-05-28T10:07:05.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (378, 471, N'Delete row via CTRL+Delete and context menu', 0, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (379, 471, N'Insert row via CTRL-Ins and context menu', 0, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (380, 471, N'Build context menu', 0, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (381, 471, N'CPTRGridRow::IsRowEmpty', 1, CAST(N'2012-05-28T11:08:52.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (382, 471, N'CROW_ProtoQPBase', 1, CAST(N'2012-05-28T17:16:52.000' AS DateTime), N'x Description
x Quantity
x Price
x Validate Quantity x Price
x Extended Price', 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (383, 471, N'CPTREdit::SetSelAll', 1, CAST(N'2012-05-28T20:31:15.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (385, 475, N'Task Timeclock tab page', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (387, 475, N'Punch In', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (388, 475, N'Punch Out', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (389, 475, N'Change PunchTo', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (392, 476, N'Design Issues', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (393, 476, N'Design Issues Form', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (394, 475, N'Task Timeclock Form', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (395, 478, N'Base Table', 1, CAST(N'2014-10-12T16:52:14.000' AS DateTime), NULL, 2, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (396, 481, N'SQL Connect', 1, CAST(N'2015-01-09T17:06:37.000' AS DateTime), NULL, 2, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (397, 481, N'Create SQL Server Database', 1, CAST(N'2015-01-09T17:06:07.000' AS DateTime), NULL, 3, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (398, 481, N'Create SQL Server Tables', 1, CAST(N'2015-01-09T17:06:21.000' AS DateTime), NULL, 4, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (399, 481, N'Copy Data from Access to SQLServer', 1, CAST(N'2015-01-09T17:05:29.000' AS DateTime), NULL, 5, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (400, 478, N'Add Columns', 1, CAST(N'2014-10-24T20:28:06.000' AS DateTime), NULL, 3, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (401, 478, N'Edit Filter', 1, CAST(N'2014-11-24T16:13:30.000' AS DateTime), NULL, 4, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (402, 478, N'Populate Lookup', 1, CAST(N'2014-12-03T15:00:53.000' AS DateTime), NULL, 5, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (403, 478, N'Form Design', 1, CAST(N'2014-10-12T16:51:57.000' AS DateTime), NULL, 2, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (404, 474, N'Add % Complete', 1, CAST(N'2014-11-24T17:40:49.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (405, 478, N'Save', 1, CAST(N'2014-12-03T15:01:29.000' AS DateTime), NULL, 6, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (406, 478, N'Load from file', 1, CAST(N'2014-12-03T14:58:10.000' AS DateTime), NULL, 7, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (407, 478, N'Table Combo Selection Change clear query', 1, CAST(N'2014-11-24T16:12:37.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (408, 478, N'Disable/Enable table combo', 1, CAST(N'2014-11-24T16:13:08.000' AS DateTime), N'On Add Column or Add Filter - disable combo.
When both grids are clear then enable table combo.', 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (409, 482, N'PTRGrid control classes set to internal', 1, CAST(N'2015-05-19T13:45:21.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (410, 482, N'Lookup control classes mark as internal', 1, CAST(N'2015-05-19T13:45:28.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (411, 478, N'UserSupervisors Lookups', 1, CAST(N'2014-10-24T20:27:41.000' AS DateTime), N'After change caption and validation.  After ENTER on tree.', 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (412, 479, N'Flags', 1, CAST(N'2015-08-25T15:27:59.000' AS DateTime), NULL, 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (413, 479, N'Mouse Hover over bar', 1, CAST(N'2014-11-25T13:37:00.000' AS DateTime), N'Show tool tip with current value.', 1, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (414, 484, N'Error Status', 1, CAST(N'2014-11-25T19:03:41.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (415, 484, N'Library - Table, Lookup Only', 1, CAST(N'2014-11-25T19:03:51.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (416, 484, N'Error Priority', 1, CAST(N'2014-11-25T19:03:57.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (417, 485, N'Error Form', 1, CAST(N'2014-11-27T20:31:59.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (418, 485, N'Write Off', 1, CAST(N'2014-12-17T15:58:13.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (419, 485, N'Clipboard Copy', 1, CAST(N'2014-12-17T15:56:17.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (420, 485, N'Error Number Generator', 1, CAST(N'2014-11-27T20:32:51.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (421, 485, N'Fixed By Auto Populate on Write Off', 1, CAST(N'2014-12-17T15:57:48.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (422, 485, N'Found By Auto Populate on Save', 1, CAST(N'2014-12-17T15:58:43.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (423, 477, N'Group Rights Form', 1, CAST(N'2014-12-30T20:26:54.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (424, 477, N'User Rights Form', 1, CAST(N'2014-12-30T20:27:08.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (425, 477, N'Apply Rights to application forms', 1, CAST(N'2014-12-30T20:27:49.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (426, 477, N'Apply Rights to Lookups Add-on-the-fly', 1, CAST(N'2014-12-30T20:28:06.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (427, 477, N'Apply Rights to Advanced Find', 1, CAST(N'2014-12-30T20:28:19.000' AS DateTime), NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (428, 487, N'DBBuilder TableProcessed Event', 1, CAST(N'2014-12-14T21:03:29.000' AS DateTime), N'Respond to event in client ensuring Supervisor record exists in TB_Users.', 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (429, 487, N'TableProcessed--TB_System table', 1, CAST(N'2014-12-14T21:03:57.000' AS DateTime), N'Check existing record in TB_System.  Otherwise, add record.

Login Type = Auto Login to Default User.
Default User = UserID of Supervisor.', 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (430, 487, N'Add Password field to TB_Users', 1, CAST(N'2014-12-14T21:09:11.000' AS DateTime), N'Encrypt on save.  Decrypt on Load.
Allow null.
255 length.  Display Length is 20.', 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (431, 487, N'App Startup--after GetSettings in SoftDevApp.InitApp', 1, CAST(N'2014-12-14T21:09:38.000' AS DateTime), N'If AutoLogin, then get default user.  Otherwise ask for user and password.
If default user has a password, then ask for password.', 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (432, 487, N'Users Form', 1, CAST(N'2014-12-14T21:13:42.000' AS DateTime), N'Add Set Password button.  If user has an existing password, then ask user for old password.  Ask user for new password twice.', 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (433, 490, N'Timeclock', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (434, 476, N'Add Product ID--allow nulls', 0, NULL, NULL, 9, NULL, NULL)
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (435, 490, N'Task Lookup', 0, NULL, N'Add Project to Task Lookup', 9, CAST(N'2018-10-08T16:42:30.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (436, 509, N'No Project', 0, NULL, NULL, 2, CAST(N'2018-11-01T14:13:57.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (438, 519, N'Backend XML', 0, NULL, NULL, 9, CAST(N'2019-05-22T15:07:22.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (439, 519, N'Save XML To PTRReports.mdb', 0, NULL, NULL, 9, CAST(N'2019-05-22T15:07:33.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (440, 519, N'Crystal Report', 0, NULL, NULL, 9, CAST(N'2019-05-22T15:07:44.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (441, 519, N'Front End XML', 0, NULL, NULL, 9, CAST(N'2019-05-22T15:07:53.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (442, 519, N'Front End Form', 0, NULL, NULL, 9, CAST(N'2019-05-22T15:07:08.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (453, 523, N'Where Clause', 1, CAST(N'2019-08-26T12:52:35.000' AS DateTime), NULL, 1, CAST(N'2019-08-26T12:52:37.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (454, 523, N'Order By Clause', 0, NULL, NULL, 1, CAST(N'2019-08-22T16:53:10.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (455, 523, N'Nested Query', 1, CAST(N'2019-08-23T12:50:01.000' AS DateTime), NULL, 1, CAST(N'2019-08-23T12:50:03.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (456, 523, N'Formulas', 0, NULL, NULL, 1, CAST(N'2019-08-23T12:49:40.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Issues] ([intIssueID], [intTaskID], [strIssueDesc], [bolResolved], [dteResolved], [txtNotes], [intIssueLevelID], [dteModifiedDate], [strModifiedBy]) VALUES (457, 523, N'Enum Fields', 0, NULL, NULL, 1, CAST(N'2019-08-26T12:53:15.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 1, N'Tab through all controls', 1, 126, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 2, N'Resize/Verify minumum size', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 3, N'Launch lookups', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 4, N'ESC  key closes form', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 5, N'ENTER through all controls', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 6, N'Enter max values on all controls and save', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 7, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 8, N'Check lookup add-on-the-fly', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 9, N'Check form rights', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 10, N'Check form header text', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 11, N'All buttons do something', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 12, N'Save button/Ctrl + S', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 13, N'Previous button/Ctrl + Left Arrow', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 14, N'Next button/Ctrl + Right Arrow', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 15, N'New button/Ctrl + N', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 16, N'Find button/Ctrl + F', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 17, N'Exit button/Esc', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 18, N'Delete button/Ctrl + D', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 19, N'Check Dirty Flag on all controls', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 20, N'Test validation on all controls.', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 21, N'Validate Delete Foreign Keys', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 22, N'Notes control tab *', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 23, N'Print Report', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (1, 24, N'Advanced Find Table', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 1, N'Tab through all controls', 1, 49, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 2, N'ENTER through all controls', 1, 49, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 3, N'Enter max values on all controls and save', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 4, N'Resize/Verify minumum size', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 5, N'Check form header text', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 6, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 7, N'Check form rights', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 8, N'Launch lookups', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 9, N'Check lookup add-on-the-fly', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 10, N'ESC  key closes form', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 11, N'All buttons do something', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 12, N'Exit button/Esc', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 13, N'Save button/Ctrl + S', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 14, N'New button/Ctrl + N', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 15, N'Delete button/Ctrl + D', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 16, N'Previous button/Ctrl + Left Arrow', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 17, N'Next button/Ctrl + Right Arrow', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 18, N'Find button/Ctrl + F', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 19, N'Check Dirty Flag on all controls', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 20, N'Test validation on all controls.', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 21, N'ENTER/Tab through all editable columns', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 22, N'Set max values in all columns and save', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 23, N'Check dirty flag', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 24, N'Cut/Copy/Paste rows', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 25, N'Drag/Drop rows', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 26, N'Lookup columns data validation', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 27, N'Lanch Lookups', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 28, N'Verify Combo columns dropdown width', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 29, N'Clear Grid', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 1, N'Tab through all controls', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 2, N'ENTER through all controls', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 3, N'Enter max values on all controls and save', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 4, N'Resize/Verify minumum size', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 5, N'Check form header text', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 6, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 7, N'Check form rights', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 8, N'Launch lookups', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 9, N'Check lookup add-on-the-fly', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 10, N'ESC  key closes form', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (6, 11, N'All buttons do something', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 1, N'Tab through all controls', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 2, N'ENTER through all controls', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 3, N'Enter max values on all controls and save', 1, 88, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 4, N'Resize/Verify minumum size', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 5, N'Check form header text', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 6, N'Click help button', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 7, N'Check form rights', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 8, N'Launch lookups', 1, 88, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 9, N'Check lookup add-on-the-fly', 1, 88, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 10, N'ESC  key closes form', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 11, N'All buttons do something', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 12, N'Exit button/Esc', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 13, N'Save button/Ctrl + S', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 14, N'New button/Ctrl + N', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 15, N'Delete button/Ctrl + D', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 30, N'Insert Row', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 31, N'Delete Row', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 32, N'Enter 2 rows with same primary key values and save', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 33, N'Validate Delete Foreign Keys', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 34, N'Notes control tab *', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 35, N'Print Report', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 1, N'All buttons do something', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 2, N'Check form header text', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 3, N'Check form rights', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 4, N'Check lookup add-on-the-fly', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 5, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 6, N'Enter max values on all controls and save', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 7, N'ENTER through all controls', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 8, N'ESC  key closes form', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 9, N'Launch lookups', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 10, N'Resize/Verify minumum size', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 11, N'Tab through all controls', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 12, N'Exit button/Esc', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 13, N'Save button/Ctrl + S', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 14, N'New button/Ctrl + N', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 15, N'Delete button/Ctrl + D', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 16, N'Previous button/Ctrl + Left Arrow', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 17, N'Next button/Ctrl + Right Arrow', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 18, N'Find button/Ctrl + F', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 19, N'Check Dirty Flag on all controls', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 20, N'Test validation on all controls.', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 21, N'Validate Delete Foreign Keys', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 22, N'Notes control tab *', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 23, N'Print Report', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 16, N'Previous button/Ctrl + Left Arrow', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 17, N'Next button/Ctrl + Right Arrow', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 18, N'Find button/Ctrl + F', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 19, N'Check Dirty Flag on all controls', 1, 88, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 20, N'Test validation on all controls.', 1, 88, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 21, N'Notes control tab *', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 22, N'Validate Delete Foreign Keys', 1, 88, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 23, N'Print Report', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 24, N'ENTER/Tab through all editable columns', 1, 85, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 25, N'Check dirty flag', 1, 88, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 26, N'Cut/Copy/Paste rows', 1, 85, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 27, N'Drag/Drop rows', 1, 85, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 28, N'Lanch Lookups', 1, 88, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 29, N'Lookup columns data validation', 1, 88, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 30, N'Verify Combo columns dropdown width', 1, 85, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 31, N'Set max values in all columns and save', 1, 85, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 32, N'Clear Grid', 1, 85, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 33, N'Insert Row', 1, 85, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 34, N'Delete Row', 1, 85, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 35, N'ENTER launches add-on-the-fly', 1, 85, 7)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 36, N'Add/Edit button', 1, 85, 7)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 37, N'Sort all columns', 1, 85, 7)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 38, N'Search all columns', 1, 85, 7)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (9, 39, N'Generate From Recurring', 1, 85, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 1, N'Tab through all controls', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 2, N'ENTER through all controls', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 3, N'Enter max values on all controls and save', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 4, N'Resize/Verify minumum size', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 5, N'Check form header text', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 6, N'Click help button', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 7, N'Check form rights', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 8, N'Launch lookups', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 9, N'Check lookup add-on-the-fly', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 10, N'ESC  key closes form', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 11, N'All buttons do something', 1, 85, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 12, N'Exit button/Esc', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 13, N'Save button/Ctrl + S', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 14, N'New button/Ctrl + N', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 15, N'Delete button/Ctrl + D', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 16, N'Validate Delete Foreign Keys', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 17, N'Previous button/Ctrl + Left Arrow', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 18, N'Next button/Ctrl + Right Arrow', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 19, N'Find button/Ctrl + F', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 20, N'Check Dirty Flag on all controls', 1, 85, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 21, N'Test validation on all controls.', 1, 88, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 22, N'Notes control tab *', 1, 88, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (10, 23, N'Print Report', 1, 88, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 1, N'Tab through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 2, N'ENTER through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 3, N'Enter max values on all controls and save', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 4, N'Resize/Verify minumum size', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 5, N'Check form header text', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 6, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 7, N'Check form rights', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 8, N'Launch lookups', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 9, N'Check lookup add-on-the-fly', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 10, N'ESC  key closes form', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 11, N'All buttons do something', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 12, N'ENTER/Tab through all editable columns', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 13, N'Set max values in all columns and save', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 14, N'Check dirty flag', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 15, N'Cut/Copy/Paste rows', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 16, N'Drag/Drop rows', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 17, N'Clear Grid', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 18, N'Insert Row', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 19, N'Delete Row', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 20, N'Lanch Lookups', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 21, N'Lookup columns data validation', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 22, N'Verify Combo columns dropdown width', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 23, N'Enter 2 rows with same primary key values and save', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 24, N'Use a new clean database', 1, 128, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 25, N'Generate Shopping List', 1, 128, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 26, N'Entry Validate Item', 1, 128, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 27, N'Entry Validate Category', 1, 128, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 28, N'Save Checklist & Item data', 1, 128, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (21, 29, N'Advanced Find Functionality', 1, 128, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 1, N'Tab through all controls', 1, 90, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 2, N'ENTER through all controls', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 3, N'Enter max values on all controls and save', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 4, N'Resize/Verify minumum size', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 5, N'Check form header text', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 6, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 7, N'Check form rights', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 8, N'Launch lookups', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 9, N'Check lookup add-on-the-fly', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 10, N'ESC  key closes form', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 11, N'All buttons do something', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 12, N'Exit button/Esc', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 13, N'Save button/Ctrl + S', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 14, N'New button/Ctrl + N', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 15, N'Delete button/Ctrl + D', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 16, N'Validate Delete Foreign Keys', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 17, N'Previous button/Ctrl + Left Arrow', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 18, N'Next button/Ctrl + Right Arrow', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 19, N'Find button/Ctrl + F', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 20, N'Check Dirty Flag on all controls', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 21, N'Test validation on all controls.', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 22, N'Notes control tab *', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 23, N'Print Report', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 24, N'ENTER/Tab through all editable columns', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 25, N'Set max values in all columns and save', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 26, N'Check dirty flag', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 27, N'Cut/Copy/Paste rows', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 28, N'Drag/Drop rows', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 29, N'Clear Grid', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 30, N'Insert Row', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 31, N'Delete Row', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 32, N'Lanch Lookups', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 33, N'Lookup columns data validation', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 1, N'Tab through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 2, N'ENTER through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 3, N'Enter max values on all controls and save', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 4, N'Resize/Verify minumum size', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 5, N'Check form header text', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 6, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 7, N'Check form rights', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 8, N'Launch lookups', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 9, N'Check lookup add-on-the-fly', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 10, N'ESC  key closes form', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 11, N'All buttons do something', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 12, N'ENTER/Tab through all editable columns', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 13, N'Set max values in all columns and save', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 14, N'Check dirty flag', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 15, N'Cut/Copy/Paste rows', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 16, N'Drag/Drop rows', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 17, N'Clear Grid', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 18, N'Insert Row', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 19, N'Delete Row', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 20, N'Lanch Lookups', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 21, N'Lookup columns data validation', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 22, N'Verify Combo columns dropdown width', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 23, N'Enter 2 rows with same primary key values and save', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 24, N'Resort', 1, 128, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (25, 25, N'OK--Verify Store location index', 1, 128, NULL)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 34, N'Verify Combo columns dropdown width', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 1, N'Tab through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 2, N'ENTER through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 3, N'Enter max values on all controls and save', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 4, N'Resize/Verify minumum size', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 5, N'Check form header text', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 6, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 7, N'Check form rights', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 8, N'Launch lookups', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 9, N'Check lookup add-on-the-fly', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 10, N'ESC  key closes form', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 11, N'All buttons do something', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 12, N'Exit button/Esc', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 13, N'Save button/Ctrl + S', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 14, N'New button/Ctrl + N', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 15, N'Delete button/Ctrl + D', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 16, N'Validate Delete Foreign Keys', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 17, N'Previous button/Ctrl + Left Arrow', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 18, N'Next button/Ctrl + Right Arrow', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 19, N'Find button/Ctrl + F', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 20, N'Check Dirty Flag on all controls', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 21, N'Test validation on all controls.', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 22, N'Notes control tab *', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 23, N'Print Report', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 1, N'Tab through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 2, N'ENTER through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 3, N'Enter max values on all controls and save', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 4, N'Resize/Verify minumum size', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 5, N'Check form header text', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 6, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 7, N'Check form rights', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 8, N'Launch lookups', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 9, N'Check lookup add-on-the-fly', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 10, N'ESC  key closes form', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 11, N'All buttons do something', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 12, N'Exit button/Esc', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 13, N'Save button/Ctrl + S', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 14, N'New button/Ctrl + N', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 15, N'Delete button/Ctrl + D', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 16, N'Validate Delete Foreign Keys', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 17, N'Previous button/Ctrl + Left Arrow', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 18, N'Next button/Ctrl + Right Arrow', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 19, N'Find button/Ctrl + F', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 20, N'Check Dirty Flag on all controls', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 21, N'Test validation on all controls.', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 22, N'Notes control tab *', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 23, N'Print Report', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 24, N'Advanced Find Table', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 25, N'ENTER/Tab through all editable columns', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 26, N'Set max values in all columns and save', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 27, N'Check dirty flag', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 28, N'Cut/Copy/Paste rows', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 29, N'Drag/Drop rows', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 30, N'Clear Grid', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 31, N'Insert Row', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (4, 36, N'Advanced Find Table', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (5, 24, N'Advanced Find Table', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 35, N'Enter 2 rows with same primary key values and save', 0, NULL, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (22, 36, N'Advanced Find Table', 0, NULL, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 1, N'Tab through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 2, N'ENTER through all controls', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 3, N'Enter max values on all controls and save', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 4, N'Resize/Verify minumum size', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 5, N'Check form header text', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 6, N'Click help button', 0, NULL, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 7, N'Check form rights', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 8, N'Launch lookups', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 9, N'Check lookup add-on-the-fly', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 10, N'ESC  key closes form', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 11, N'All buttons do something', 1, 128, 1)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 12, N'Exit button/Esc', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 13, N'Save button/Ctrl + S', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 14, N'New button/Ctrl + N', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 15, N'Delete button/Ctrl + D', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 16, N'Validate Delete Foreign Keys', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 17, N'Previous button/Ctrl + Left Arrow', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 18, N'Next button/Ctrl + Right Arrow', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 19, N'Find button/Ctrl + F', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 20, N'Check Dirty Flag on all controls', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 21, N'Test validation on all controls.', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 22, N'Notes control tab *', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 23, N'Print Report', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (23, 24, N'Advanced Find Table', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 24, N'ENTER launches add-on-the-fly', 1, 128, 7)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 25, N'Add/Edit button', 1, 128, 7)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 26, N'Sort all columns', 1, 128, 7)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 27, N'Search all columns', 1, 128, 7)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (26, 28, N'Advanced Find Table', 1, 128, 2)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 32, N'Delete Row', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 33, N'Lanch Lookups', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 34, N'Lookup columns data validation', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 35, N'Verify Combo columns dropdown width', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineDetails] ([intOutlineID], [intDetailID], [strText], [bolComplete], [intCompletedVersionID], [intTemplateID]) VALUES (27, 36, N'Enter 2 rows with same primary key values and save', 1, 128, 6)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (6, 1)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (9, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (9, 6)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (9, 7)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (10, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (21, 1)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (21, 6)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (25, 1)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (25, 6)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (1, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (4, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (4, 6)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (5, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (22, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (22, 6)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (23, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (26, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (26, 7)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (27, 2)
GO
INSERT [dbo].[TB_OutlineTemplates] ([intOutlineID], [intTemplateID]) VALUES (27, 6)
GO
INSERT [dbo].[TB_Products] ([intProductID], [strProduct], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (28, N'Checking Account Manager', NULL, CAST(N'2016-01-11T20:07:43.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Products] ([intProductID], [strProduct], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (29, N'Grocery List Creator', N'a', CAST(N'2016-01-11T20:08:32.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Products] ([intProductID], [strProduct], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (30, N'DevLogix', NULL, CAST(N'2019-05-04T13:18:58.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Products] ([intProductID], [strProduct], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (33, N'Error Test', NULL, NULL, NULL)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (14, 0, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (14, 1, 6)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (14, 2, 5)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (14, 3, 6)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (14, 4, 6)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (14, 5, 2)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (14, 6, 6)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (16, 0, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (16, 1, 7)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (16, 2, 7)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (16, 3, 7)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (16, 4, 7)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (16, 5, 5)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (16, 6, 5)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (17, 0, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (17, 1, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (17, 2, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (17, 3, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (17, 4, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (17, 5, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (17, 6, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (20, 0, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (20, 1, 8)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (20, 2, 6)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (20, 3, 8)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (20, 4, 8)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (20, 5, 8)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (20, 6, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (21, 0, 0)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (21, 1, 9)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (21, 2, 9)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (21, 3, 9)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (21, 4, 9)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (21, 5, 6)
GO
INSERT [dbo].[TB_ProjectDays] ([intProjectID], [bytDayIndex], [decWorkingHrs]) VALUES (21, 6, 9)
GO
INSERT [dbo].[TB_Projects] ([intProjectID], [strProject], [txtNotes], [dteDeadline], [dteOriginal], [dteModifiedDate], [strModifiedBy]) VALUES (14, N'PTRApps', NULL, CAST(N'2012-08-31T00:00:00.000' AS DateTime), CAST(N'2012-08-31T00:00:00.000' AS DateTime), CAST(N'2018-10-22T17:54:09.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Projects] ([intProjectID], [strProject], [txtNotes], [dteDeadline], [dteOriginal], [dteModifiedDate], [strModifiedBy]) VALUES (16, N'DevLogix', NULL, CAST(N'2015-03-31T00:00:00.000' AS DateTime), CAST(N'2015-03-31T00:00:00.000' AS DateTime), CAST(N'2019-02-23T13:59:13.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Projects] ([intProjectID], [strProject], [txtNotes], [dteDeadline], [dteOriginal], [dteModifiedDate], [strModifiedBy]) VALUES (17, N'Family Bank IS', NULL, CAST(N'2015-05-26T00:00:00.000' AS DateTime), CAST(N'2015-05-26T00:00:00.000' AS DateTime), CAST(N'2018-11-30T13:11:24.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Projects] ([intProjectID], [strProject], [txtNotes], [dteDeadline], [dteOriginal], [dteModifiedDate], [strModifiedBy]) VALUES (18, N'Shopper IS', NULL, CAST(N'2015-12-31T00:00:00.000' AS DateTime), CAST(N'2015-12-31T00:00:00.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[TB_Projects] ([intProjectID], [strProject], [txtNotes], [dteDeadline], [dteOriginal], [dteModifiedDate], [strModifiedBy]) VALUES (20, N'Test', NULL, CAST(N'2019-07-27T00:00:00.000' AS DateTime), CAST(N'2019-07-27T00:00:00.000' AS DateTime), CAST(N'2019-07-27T13:30:32.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Projects] ([intProjectID], [strProject], [txtNotes], [dteDeadline], [dteOriginal], [dteModifiedDate], [strModifiedBy]) VALUES (21, N'DbLookup', NULL, CAST(N'2019-12-02T00:00:00.000' AS DateTime), CAST(N'2019-12-02T00:00:00.000' AS DateTime), CAST(N'2019-08-26T19:09:27.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_ProjectUsers] ([intProjectID], [intUserID], [bolStandard], [decDay0Hrs], [decDay1Hrs], [decDay2Hrs], [decDay3Hrs], [decDay4Hrs], [decDay5Hrs], [decDay6Hrs]) VALUES (14, 1, 1, 0, 6, 5, 6, 6, 2, 6)
GO
INSERT [dbo].[TB_ProjectUsers] ([intProjectID], [intUserID], [bolStandard], [decDay0Hrs], [decDay1Hrs], [decDay2Hrs], [decDay3Hrs], [decDay4Hrs], [decDay5Hrs], [decDay6Hrs]) VALUES (16, 1, 1, 0, 7, 7, 7, 7, 5, 5)
GO
INSERT [dbo].[TB_ProjectUsers] ([intProjectID], [intUserID], [bolStandard], [decDay0Hrs], [decDay1Hrs], [decDay2Hrs], [decDay3Hrs], [decDay4Hrs], [decDay5Hrs], [decDay6Hrs]) VALUES (16, 4, 1, 0, 7, 7, 7, 7, 5, 5)
GO
INSERT [dbo].[TB_ProjectUsers] ([intProjectID], [intUserID], [bolStandard], [decDay0Hrs], [decDay1Hrs], [decDay2Hrs], [decDay3Hrs], [decDay4Hrs], [decDay5Hrs], [decDay6Hrs]) VALUES (16, 9, 0, 0, 4, 4, 4, 4, 4, 0)
GO
INSERT [dbo].[TB_ProjectUsers] ([intProjectID], [intUserID], [bolStandard], [decDay0Hrs], [decDay1Hrs], [decDay2Hrs], [decDay3Hrs], [decDay4Hrs], [decDay5Hrs], [decDay6Hrs]) VALUES (16, 10, 1, 0, 7, 7, 7, 7, 5, 5)
GO
INSERT [dbo].[TB_ProjectUsers] ([intProjectID], [intUserID], [bolStandard], [decDay0Hrs], [decDay1Hrs], [decDay2Hrs], [decDay3Hrs], [decDay4Hrs], [decDay5Hrs], [decDay6Hrs]) VALUES (21, 1, 1, 0, 9, 9, 9, 9, 6, 9)
GO
INSERT [dbo].[TB_System] ([intSysUnique], [txtSettings], [intWriteOffStatus], [intPassStatus], [intFailStatus], [dteModifiedDate], [strModifiedBy], [strErrorNoPrefix]) VALUES (1, N'gmWiVkm/SH/E4LQoZFT0YA==', 8, 5, 1, CAST(N'2019-08-19T17:26:50.000' AS DateTime), N'Peter Ringering', N'E')
GO
INSERT [dbo].[TB_TaskPriority] ([intPriorityID], [strDescription], [dteModifiedDate], [strModifiedBy], [intPriorityNo]) VALUES (1, N'ASAP', NULL, NULL, 0)
GO
INSERT [dbo].[TB_TaskPriority] ([intPriorityID], [strDescription], [dteModifiedDate], [strModifiedBy], [intPriorityNo]) VALUES (2, N'Highest', NULL, NULL, 0)
GO
INSERT [dbo].[TB_TaskPriority] ([intPriorityID], [strDescription], [dteModifiedDate], [strModifiedBy], [intPriorityNo]) VALUES (3, N'To Do', NULL, NULL, 0)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (471, N'CPTRGrid', CAST(N'2012-08-31T00:00:00.000' AS DateTime), NULL, CAST(100.00 AS Decimal(18, 2)), CAST(0.25 AS Decimal(18, 2)), NULL, CAST(100.00 AS Decimal(18, 2)), 14, NULL, 2, 3, 0, CAST(N'2015-09-07T14:36:37.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (472, N'CPTRSimpleGrid', CAST(N'2012-12-31T00:00:00.000' AS DateTime), NULL, CAST(100.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(100.00 AS Decimal(18, 2)), 14, NULL, 1, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (474, N'Tasks', CAST(N'2014-10-09T00:00:00.000' AS DateTime), CAST(N'2014-10-09T00:00:00.000' AS DateTime), CAST(8.00 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)), NULL, CAST(8.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (475, N'User Goals Update Task Hours Spent', NULL, NULL, CAST(32.00 AS Decimal(18, 2)), NULL, NULL, CAST(32.00 AS Decimal(18, 2)), 16, NULL, 1, 3, 1, CAST(N'2019-05-22T13:11:03.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (476, N'Projects', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), NULL, CAST(8.00 AS Decimal(18, 2)), 16, NULL, 3, 3, 1, CAST(N'2019-05-22T13:16:17.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (477, N'Security', NULL, NULL, CAST(32.00 AS Decimal(18, 2)), CAST(32.00 AS Decimal(18, 2)), NULL, CAST(32.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (478, N'Advanced Find', CAST(N'2014-10-16T00:00:00.000' AS DateTime), NULL, CAST(40.00 AS Decimal(18, 2)), CAST(80.00 AS Decimal(18, 2)), NULL, CAST(40.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (479, N'Chart Definition', NULL, NULL, CAST(40.00 AS Decimal(18, 2)), CAST(32.00 AS Decimal(18, 2)), NULL, CAST(32.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (480, N'Advanced Find Report', CAST(N'2015-04-09T00:00:00.000' AS DateTime), CAST(N'2015-04-09T17:49:00.000' AS DateTime), CAST(32.00 AS Decimal(18, 2)), CAST(32.00 AS Decimal(18, 2)), NULL, CAST(32.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (481, N'Upsize To SQL Server', NULL, NULL, CAST(40.00 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), NULL, CAST(40.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (482, N'PTRFormsV2 - Internal controls', NULL, NULL, CAST(4.00 AS Decimal(18, 2)), NULL, NULL, CAST(8.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (484, N'Error Sub Forms', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), NULL, NULL, 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (485, N'Add/Edit Errors', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), CAST(80.00 AS Decimal(18, 2)), NULL, CAST(8.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (487, N'User Login', CAST(N'2014-12-09T00:00:00.000' AS DateTime), NULL, CAST(8.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), NULL, CAST(8.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (488, N'Project status report - Calculate estimated completion date.', NULL, NULL, CAST(24.00 AS Decimal(18, 2)), NULL, NULL, NULL, 16, NULL, 1, 3, 1, CAST(N'2019-05-22T13:07:39.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (489, N'Bank Account Report', CAST(N'2015-05-21T00:00:00.000' AS DateTime), NULL, CAST(8.00 AS Decimal(18, 2)), NULL, NULL, NULL, 17, NULL, 2, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (490, N'User Goals', CAST(N'2015-05-19T00:00:00.000' AS DateTime), CAST(N'2015-05-19T22:18:00.000' AS DateTime), CAST(4.00 AS Decimal(18, 2)), CAST(16.00 AS Decimal(18, 2)), NULL, NULL, 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (491, N'User Goals Report', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), NULL, NULL, NULL, 16, NULL, 1, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (492, N'Clear Registry in -dev mode', NULL, CAST(N'2015-05-20T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (493, N'Help/About', NULL, CAST(N'2015-05-20T00:00:00.000' AS DateTime), CAST(2.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), NULL, NULL, 16, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (494, N'Prep Family Bank Account for Release', NULL, NULL, CAST(4.00 AS Decimal(18, 2)), NULL, NULL, NULL, 17, NULL, 2, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (495, N'Set Registry App Name in Debug Build', NULL, CAST(N'2015-05-20T00:00:00.000' AS DateTime), CAST(2.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, 4, 3, 1, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (496, N'Help Document', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), NULL, NULL, NULL, 18, NULL, 2, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (497, N'Help Document', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), NULL, NULL, NULL, 17, NULL, 2, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (499, N'Prep Shopper IS for Release', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), NULL, NULL, NULL, 18, NULL, 2, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (500, N'Globals Help Document', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, NULL, 2, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (501, N'Record Locking', NULL, NULL, CAST(40.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), N'SQL Server = SELECT GETDATE() AS dteTimestamp
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
', CAST(40.00 AS Decimal(18, 2)), 16, NULL, 4, 3, 1, CAST(N'2015-09-10T14:21:52.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (502, N'Clean Install - Create Demo Database', NULL, NULL, CAST(40.00 AS Decimal(18, 2)), NULL, NULL, NULL, 16, NULL, 1, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (503, N'Graphics and Icons', NULL, NULL, CAST(20.00 AS Decimal(18, 2)), NULL, NULL, NULL, 16, NULL, 1, 3, 1, CAST(N'2019-05-08T14:56:48.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (504, N'Clean Install - Add default records', NULL, NULL, CAST(32.00 AS Decimal(18, 2)), NULL, NULL, NULL, 16, NULL, 1, 3, 0, NULL, NULL, 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (508, N'Switch Adv. Find To Maint Form', NULL, NULL, CAST(10.00 AS Decimal(18, 2)), NULL, NULL, CAST(10.00 AS Decimal(18, 2)), 14, NULL, NULL, 2, 0, CAST(N'2018-10-22T18:13:07.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (509, N'No Project', NULL, NULL, CAST(8.00 AS Decimal(18, 2)), NULL, NULL, CAST(8.00 AS Decimal(18, 2)), NULL, NULL, NULL, 3, 0, CAST(N'2018-12-10T12:56:23.000' AS DateTime), N'Peter Ringering', 0, NULL)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (516, N'Unassigned Task', NULL, NULL, CAST(32.00 AS Decimal(18, 2)), CAST(2.98 AS Decimal(18, 2)), NULL, CAST(32.00 AS Decimal(18, 2)), 16, NULL, NULL, 3, 1, CAST(N'2019-05-22T13:10:00.000' AS DateTime), N'Peter Ringering', 0, NULL)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (517, N'Timeclock', NULL, NULL, CAST(10.00 AS Decimal(18, 2)), CAST(1.18 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, NULL, 3, 0, CAST(N'2019-05-15T11:50:17.000' AS DateTime), N'Peter Ringering', 0, NULL)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (519, N'Time Clock Report', CAST(N'2019-05-22T00:00:00.000' AS DateTime), CAST(N'2019-08-16T00:00:00.000' AS DateTime), CAST(50.00 AS Decimal(18, 2)), CAST(49.70 AS Decimal(18, 2)), NULL, CAST(20.00 AS Decimal(18, 2)), 16, NULL, NULL, 3, 1, CAST(N'2019-08-16T15:28:01.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (520, N'User Time Clock Lookup', NULL, NULL, CAST(5.00 AS Decimal(18, 2)), CAST(2.85 AS Decimal(18, 2)), NULL, NULL, 16, NULL, NULL, 3, 1, CAST(N'2019-05-31T14:50:04.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (521, N'Task01', NULL, NULL, CAST(5.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), NULL, CAST(5.00 AS Decimal(18, 2)), 20, NULL, NULL, 3, 0, CAST(N'2019-07-27T13:31:23.000' AS DateTime), N'Peter Ringering', 0, 12)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (522, N'Task02', NULL, NULL, CAST(5.00 AS Decimal(18, 2)), CAST(0.43 AS Decimal(18, 2)), NULL, NULL, 20, NULL, NULL, 3, 0, CAST(N'2019-07-27T13:32:59.000' AS DateTime), N'Peter Ringering', 0, 10)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (523, N'Query Builder/Select Sql Generator', NULL, NULL, CAST(80.00 AS Decimal(18, 2)), CAST(48.18 AS Decimal(18, 2)), NULL, CAST(80.00 AS Decimal(18, 2)), 21, NULL, NULL, 3, 1, CAST(N'2019-08-27T21:08:03.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (525, N'GetDataProcessor', NULL, NULL, CAST(40.00 AS Decimal(18, 2)), CAST(5.94 AS Decimal(18, 2)), NULL, CAST(40.00 AS Decimal(18, 2)), 21, NULL, NULL, 3, 1, CAST(N'2019-08-22T10:43:53.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (526, N'Unit Testing', NULL, NULL, CAST(40.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), NULL, NULL, 21, NULL, NULL, 3, 1, CAST(N'2019-08-22T16:04:33.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (527, N'Model Definitiions', NULL, NULL, CAST(80.00 AS Decimal(18, 2)), CAST(14.04 AS Decimal(18, 2)), NULL, NULL, 21, NULL, NULL, 3, 0, CAST(N'2019-08-25T16:08:00.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_Tasks] ([intTaskID], [strTaskDesc], [dteDueDate], [dteCompletedDate], [decEstHrs], [decHrsSpent], [txtNotes], [decOrigEst], [intProjectID], [strCMSTaskID], [intStatusID], [intPriorityID], [decPercentComplete], [dteModifiedDate], [strModifiedBy], [intPriorityNo], [intAssignedToID]) VALUES (528, N'Lookup Definition', NULL, NULL, CAST(80.00 AS Decimal(18, 2)), NULL, NULL, NULL, 21, NULL, NULL, 3, 0, CAST(N'2019-08-25T10:08:13.000' AS DateTime), N'Peter Ringering', 0, 1)
GO
INSERT [dbo].[TB_TaskStatus] ([intStatusID], [strDescription], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Not Started', NULL, NULL)
GO
INSERT [dbo].[TB_TaskStatus] ([intStatusID], [strDescription], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'In Progress', NULL, NULL)
GO
INSERT [dbo].[TB_TaskStatus] ([intStatusID], [strDescription], [dteModifiedDate], [strModifiedBy]) VALUES (3, N'On Hold', NULL, NULL)
GO
INSERT [dbo].[TB_TaskStatus] ([intStatusID], [strDescription], [dteModifiedDate], [strModifiedBy]) VALUES (4, N'Code Complete', NULL, NULL)
GO
INSERT [dbo].[TB_TaskStatus] ([intStatusID], [strDescription], [dteModifiedDate], [strModifiedBy]) VALUES (5, N'Earliest Convenience', NULL, NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (1, N'Testing Outlines', 30, 1, 1, NULL, 0.1, NULL, CAST(N'2019-05-15T17:19:23.000' AS DateTime), N'Peter Ringering', 0.45)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (4, N'Errors Form', 30, 1, 1, NULL, 0, NULL, CAST(N'2019-05-15T17:19:36.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (5, N'Test', 30, 1, 12, NULL, 0, NULL, CAST(N'2019-05-15T17:19:50.000' AS DateTime), N'Peter Ringering', 1.45)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (6, N'Advanced Find', 30, 1, 1, NULL, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (9, N'Bank Account Manager', 28, 1, 1, NULL, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (10, N'Recurring Templates', 28, 1, 1, NULL, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (21, N'Prepare New Grocery Shopping List', 29, 1, 1, NULL, 0.9, NULL, CAST(N'2015-10-08T21:49:03.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (22, N'Projects window', 30, 1, 1, NULL, 0, NULL, CAST(N'2019-05-15T17:21:13.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (23, N'Add/Edit Items', 29, 1, 1, NULL, 0.9, NULL, CAST(N'2019-05-15T17:22:03.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (25, N'Setup Categories Order', 29, 1, 1, NULL, 0.9, NULL, CAST(N'2015-10-08T15:53:03.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (26, N'Add/Edit Categories', 29, 1, 1, NULL, 0.9, NULL, CAST(N'2019-05-15T17:28:36.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (27, N'Add/Edit Shopping Lists', 29, 1, 1, NULL, 0.9, NULL, CAST(N'2019-05-15T17:28:53.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (28, N'Tools/Options', 29, 1, 1, NULL, 0.9, NULL, CAST(N'2015-10-08T16:01:09.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingOutlines] ([intOutlineID], [strName], [intProductID], [intCreatedByID], [intAssignedToID], [dteDueDate], [decPercentComplete], [txtNotes], [dteModifiedDate], [strModifiedBy], [decHrsSpent]) VALUES (31, N'Error Test', 33, 1, NULL, NULL, 0, NULL, CAST(N'2019-05-22T14:32:45.000' AS DateTime), N'Peter Ringering', NULL)
GO
INSERT [dbo].[TB_TestingTemplates] ([intTemplateID], [strName], [txtNotes], [intBaseTemplateID], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Basic Form', NULL, 5, NULL, NULL)
GO
INSERT [dbo].[TB_TestingTemplates] ([intTemplateID], [strName], [txtNotes], [intBaseTemplateID], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'Record Maintenance Form', NULL, 1, CAST(N'2019-05-15T17:18:45.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_TestingTemplates] ([intTemplateID], [strName], [txtNotes], [intBaseTemplateID], [dteModifiedDate], [strModifiedBy]) VALUES (6, N'Grid', NULL, NULL, CAST(N'2015-10-08T12:55:21.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_TestingTemplates] ([intTemplateID], [strName], [txtNotes], [intBaseTemplateID], [dteModifiedDate], [strModifiedBy]) VALUES (7, N'Lookup Control', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_TestingTemplates] ([intTemplateID], [strName], [txtNotes], [intBaseTemplateID], [dteModifiedDate], [strModifiedBy]) VALUES (8, N'Application', NULL, NULL, CAST(N'2019-05-15T12:58:48.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 0, N'Tab through all controls')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 1, N'ENTER through all controls')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 2, N'Enter max values on all controls and save')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 3, N'Resize/Verify minumum size')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 4, N'Check form header text')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 5, N'Click help button')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 6, N'Check form rights')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 7, N'Launch lookups')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 8, N'Check lookup add-on-the-fly')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 9, N'ESC  key closes form')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (1, 10, N'All buttons do something')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 0, N'ENTER/Tab through all editable columns')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 1, N'Set max values in all columns and save')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 2, N'Check dirty flag')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 3, N'Cut/Copy/Paste rows')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 4, N'Drag/Drop rows')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 5, N'Clear Grid')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 6, N'Insert Row')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 7, N'Delete Row')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 8, N'Lanch Lookups')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 9, N'Lookup columns data validation')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 10, N'Verify Combo columns dropdown width')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (6, 11, N'Enter 2 rows with same primary key values and save')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (7, 0, N'ENTER launches add-on-the-fly')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (7, 1, N'Add/Edit button')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (7, 2, N'Sort all columns')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (7, 3, N'Search all columns')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (8, 0, N'Clean Install')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (8, 1, N'Create New Database')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (8, 2, N'Insert new records into new database')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (8, 3, N'Change Database--Update globals')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 0, N'Exit button/Esc')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 1, N'Save button/Ctrl + S')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 2, N'New button/Ctrl + N')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 3, N'Delete button/Ctrl + D')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 4, N'Validate Delete Foreign Keys')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 5, N'Previous button/Ctrl + Left Arrow')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 6, N'Next button/Ctrl + Right Arrow')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 7, N'Find button/Ctrl + F')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 8, N'Check Dirty Flag on all controls')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 9, N'Test validation on all controls.')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 10, N'Notes control tab *')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 11, N'Print Report')
GO
INSERT [dbo].[TB_TestTemplDetails] ([intTemplateID], [intDetailID], [strText]) VALUES (2, 12, N'Advanced Find Table')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (39, 2, 1, CAST(N'2019-05-15T13:00:11.000' AS DateTime), CAST(N'2019-05-15T13:05:13.000' AS DateTime), NULL, NULL, 239, N'Passed.', 0, CAST(N'2019-05-15T13:05:23.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (40, 2, 1, CAST(N'2019-05-15T13:07:47.000' AS DateTime), CAST(N'2019-05-15T14:01:42.000' AS DateTime), NULL, NULL, 198, N'Fixing error.', 0, CAST(N'2019-05-15T14:01:53.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (41, 2, 1, CAST(N'2019-05-20T14:35:36.000' AS DateTime), CAST(N'2019-05-20T15:50:37.000' AS DateTime), NULL, NULL, 204, N'Fixed error.', 0, CAST(N'2019-05-20T15:50:52.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (42, 2, 1, CAST(N'2019-05-21T12:17:12.000' AS DateTime), CAST(N'2019-05-21T12:28:27.000' AS DateTime), NULL, NULL, 196, N'Tested and failed.', 0, CAST(N'2019-05-21T12:28:42.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (43, 2, 1, CAST(N'2019-05-21T12:29:02.000' AS DateTime), CAST(N'2019-05-21T12:35:16.000' AS DateTime), NULL, NULL, 198, N'Tested and failed.', 0, CAST(N'2019-05-21T12:35:31.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (44, 2, 1, CAST(N'2019-05-21T12:35:45.000' AS DateTime), CAST(N'2019-05-21T12:42:14.000' AS DateTime), NULL, NULL, 199, N'Tested and passed.', 0, CAST(N'2019-05-21T12:42:29.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (45, 2, 1, CAST(N'2019-05-21T12:48:41.000' AS DateTime), CAST(N'2019-05-21T13:33:03.000' AS DateTime), NULL, NULL, 229, N'Tested and failed.', 0, CAST(N'2019-05-21T13:33:15.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (46, 2, 1, CAST(N'2019-05-21T13:33:42.000' AS DateTime), CAST(N'2019-05-21T13:37:07.000' AS DateTime), NULL, NULL, 231, N'Tested and passed.', 0, CAST(N'2019-05-21T13:37:19.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (47, 2, 1, CAST(N'2019-05-21T13:39:47.000' AS DateTime), CAST(N'2019-05-21T13:51:08.000' AS DateTime), NULL, NULL, 196, N'Fixed.', 0, CAST(N'2019-05-21T13:51:17.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (48, 2, 1, CAST(N'2019-05-21T13:52:28.000' AS DateTime), CAST(N'2019-05-21T16:18:07.000' AS DateTime), NULL, NULL, 198, N'Fixed error.', 0, CAST(N'2019-05-21T16:18:16.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (49, 2, 1, CAST(N'2019-05-21T16:19:16.000' AS DateTime), CAST(N'2019-05-21T16:32:24.000' AS DateTime), NULL, NULL, 229, N'Fixed error.', 0, CAST(N'2019-05-21T16:32:34.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (50, 2, 1, CAST(N'2019-05-22T13:26:01.000' AS DateTime), CAST(N'2019-05-22T13:39:17.000' AS DateTime), NULL, NULL, 232, N'Fixed error.', 0, CAST(N'2019-05-22T13:39:27.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (51, 2, 1, CAST(N'2019-05-22T13:39:57.000' AS DateTime), CAST(N'2019-05-22T14:03:05.000' AS DateTime), NULL, NULL, 233, N'Fixed error.', 0, CAST(N'2019-05-22T14:03:15.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (52, 2, 1, CAST(N'2019-05-22T14:09:50.000' AS DateTime), CAST(N'2019-05-22T14:23:15.000' AS DateTime), NULL, NULL, 241, N'Fixed Error.', 0, CAST(N'2019-05-22T14:23:38.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (53, 2, 1, CAST(N'2019-05-22T14:31:17.000' AS DateTime), CAST(N'2019-05-22T14:36:04.000' AS DateTime), NULL, NULL, 196, N'Tested and passed error.', 0, CAST(N'2019-05-22T14:37:10.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (54, 0, 1, CAST(N'2019-05-22T14:55:59.000' AS DateTime), CAST(N'2019-05-22T16:50:50.000' AS DateTime), 519, NULL, NULL, N'Working on back end XML.', 0, CAST(N'2019-05-22T16:51:15.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (55, 0, 1, CAST(N'2019-05-31T11:57:46.000' AS DateTime), CAST(N'2019-05-31T14:48:52.000' AS DateTime), 520, NULL, NULL, N'Code Complete', 0, CAST(N'2019-05-31T14:49:07.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (56, 2, 1, CAST(N'2019-06-01T12:33:25.000' AS DateTime), CAST(N'2019-06-01T13:00:35.000' AS DateTime), NULL, NULL, 269, N'Fixed error.', 0, CAST(N'2019-06-01T13:00:46.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (57, 0, 1, CAST(N'2019-06-01T13:04:17.000' AS DateTime), CAST(N'2019-06-01T13:50:50.000' AS DateTime), 519, NULL, NULL, N'Working on PTRReports.mdb', 0, CAST(N'2019-06-01T13:51:13.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (58, 0, 1, CAST(N'2019-06-06T13:30:56.000' AS DateTime), CAST(N'2019-06-06T15:23:28.000' AS DateTime), 519, NULL, NULL, N'Designing', 0, CAST(N'2019-06-06T15:23:43.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (59, 0, 1, CAST(N'2019-06-14T11:40:31.000' AS DateTime), CAST(N'2019-06-14T12:13:33.000' AS DateTime), 519, NULL, NULL, N'Got report UI form mostly done.  Researching report object.', 0, CAST(N'2019-06-14T12:14:25.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (60, 0, 1, CAST(N'2019-06-14T14:18:09.000' AS DateTime), CAST(N'2019-06-14T15:10:43.000' AS DateTime), 519, NULL, NULL, N'Working on report SQL.', 0, CAST(N'2019-06-14T15:11:03.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (61, 0, 1, CAST(N'2019-06-19T13:30:56.000' AS DateTime), CAST(N'2019-06-19T16:26:38.000' AS DateTime), 519, NULL, NULL, N'Working on SQL Statement.', 0, CAST(N'2019-06-19T16:27:00.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (62, 0, 1, CAST(N'2019-07-09T13:18:12.000' AS DateTime), CAST(N'2019-07-09T14:52:41.000' AS DateTime), 519, NULL, NULL, N'Got filter complete.', 0, CAST(N'2019-07-09T14:53:04.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (63, 0, 1, CAST(N'2019-07-09T14:53:11.000' AS DateTime), CAST(N'2019-07-09T16:40:04.000' AS DateTime), 519, NULL, NULL, N'Finished User Report Sql.  Got started on Code Type Report Sql.', 0, CAST(N'2019-07-09T16:41:45.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (64, 0, 1, CAST(N'2019-07-16T13:11:14.000' AS DateTime), CAST(N'2019-07-16T15:46:24.000' AS DateTime), 519, NULL, NULL, N'Got report data to output to PTRReports.mdb.  Started working on Crystal Reports file.', 0, CAST(N'2019-07-16T15:47:34.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (65, 0, 1, CAST(N'2019-07-24T12:15:54.000' AS DateTime), CAST(N'2019-07-24T16:05:59.000' AS DateTime), 519, NULL, NULL, N'Got hard part of User report done.  Started on Product and Project SQL.', 0, CAST(N'2019-07-24T16:06:56.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (66, 0, 1, CAST(N'2019-07-25T10:10:37.000' AS DateTime), CAST(N'2019-07-25T16:38:13.000' AS DateTime), 519, NULL, NULL, N'Almost done with Product sql statement.', 0, CAST(N'2019-07-25T16:39:07.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (67, 0, 1, CAST(N'2019-07-26T11:24:15.000' AS DateTime), CAST(N'2019-07-26T15:31:34.000' AS DateTime), 519, NULL, NULL, N'Code refactoring.', 0, CAST(N'2019-07-26T15:31:53.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (68, 0, 12, CAST(N'2019-07-27T13:31:45.000' AS DateTime), CAST(N'2019-07-27T14:31:48.000' AS DateTime), 521, NULL, NULL, N'Aaaaaaaaaa', 1, CAST(N'2019-07-27T13:32:24.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (69, 0, 10, CAST(N'2019-07-27T14:33:01.000' AS DateTime), CAST(N'2019-07-27T14:59:05.000' AS DateTime), 522, NULL, NULL, N'qqqqqqqqq', 1, CAST(N'2019-07-27T13:33:49.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (70, 1, 12, CAST(N'2019-07-27T15:15:24.000' AS DateTime), CAST(N'2019-07-27T15:57:31.000' AS DateTime), NULL, 5, NULL, N'aaaaaaaaaa', 1, CAST(N'2019-07-27T15:16:01.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (71, 1, 10, CAST(N'2019-07-26T15:16:07.000' AS DateTime), CAST(N'2019-07-26T16:01:10.000' AS DateTime), NULL, 5, NULL, N'qqqqqqqq', 1, CAST(N'2019-07-27T15:16:58.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (72, 0, 1, CAST(N'2019-07-27T09:58:02.000' AS DateTime), CAST(N'2019-07-27T17:16:12.000' AS DateTime), 519, NULL, NULL, N'Finished all report SQL.  Added features to filter non-user reports on user.  Also filter user and product reports on time clock type.', 0, CAST(N'2019-07-27T17:18:25.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (73, 0, 1, CAST(N'2019-07-30T13:05:23.000' AS DateTime), CAST(N'2019-07-30T15:24:47.000' AS DateTime), 519, NULL, NULL, N'Code refactoring.', 0, CAST(N'2019-07-30T15:25:08.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (2, 2, 1, CAST(N'2019-03-23T12:27:58.000' AS DateTime), CAST(N'2019-03-23T12:48:03.000' AS DateTime), NULL, NULL, 123, N'20 Minutes', 0, CAST(N'2019-04-20T10:07:48.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (3, 1, 1, CAST(N'2019-03-23T12:28:58.000' AS DateTime), CAST(N'2019-03-23T14:48:02.000' AS DateTime), NULL, 26, NULL, N'2 Hours, 19 Minutes', 0, CAST(N'2019-04-20T10:08:59.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (4, 0, 1, CAST(N'2019-04-05T12:56:50.000' AS DateTime), CAST(N'2019-04-05T13:45:52.000' AS DateTime), 516, NULL, NULL, N'aaaaaaa', 0, CAST(N'2019-04-05T12:57:58.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (5, 0, 1, CAST(N'2019-04-05T13:55:06.000' AS DateTime), CAST(N'2019-04-05T14:12:10.000' AS DateTime), 516, NULL, NULL, N'ssssssss', 0, CAST(N'2019-04-05T13:55:50.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (6, 0, 10, CAST(N'2019-04-09T14:39:53.000' AS DateTime), CAST(N'2019-04-09T14:55:17.000' AS DateTime), 471, NULL, NULL, N'aaaaaaaaa', 0, CAST(N'2019-07-16T16:38:29.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (7, 0, 10, CAST(N'2019-04-09T14:46:05.000' AS DateTime), CAST(N'2019-04-09T15:26:10.000' AS DateTime), 516, NULL, NULL, N'aaaaaa', 0, CAST(N'2019-07-16T16:32:54.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (9, 0, 2, CAST(N'2019-04-19T11:50:30.000' AS DateTime), CAST(N'2019-04-19T12:50:33.000' AS DateTime), 516, NULL, NULL, N'aaaaaaaa', 1, CAST(N'2019-07-16T16:32:24.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (10, 0, 2, CAST(N'2019-04-19T14:31:10.000' AS DateTime), CAST(N'2019-04-19T14:31:14.000' AS DateTime), 516, NULL, NULL, N'aaaaaaaa', 0, CAST(N'2019-07-16T16:39:01.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (11, 1, 1, CAST(N'2019-04-25T15:31:06.000' AS DateTime), CAST(N'2019-04-25T15:31:19.000' AS DateTime), NULL, 1, NULL, N'Test', 0, CAST(N'2019-04-25T15:31:41.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (12, 1, 1, CAST(N'2019-04-25T15:33:22.000' AS DateTime), CAST(N'2019-04-25T16:00:25.000' AS DateTime), NULL, 1, NULL, N'Test', 1, CAST(N'2019-04-25T15:33:49.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (13, 0, 1, CAST(N'2019-04-27T13:38:11.000' AS DateTime), CAST(N'2019-04-27T13:38:23.000' AS DateTime), 516, NULL, NULL, N'cccccccc', 0, CAST(N'2019-04-27T13:38:31.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (14, 0, 1, CAST(N'2019-04-27T13:38:46.000' AS DateTime), CAST(N'2019-04-27T13:39:03.000' AS DateTime), 516, NULL, NULL, N'aaaaaaaaa', 0, CAST(N'2019-04-27T13:39:07.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (15, 0, 1, CAST(N'2019-04-27T13:42:04.000' AS DateTime), CAST(N'2019-04-27T13:42:27.000' AS DateTime), 516, NULL, NULL, N'aaaaaa', 0, CAST(N'2019-04-27T13:42:33.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (16, 2, 1, CAST(N'2019-04-30T13:33:45.000' AS DateTime), CAST(N'2019-04-30T13:33:49.000' AS DateTime), NULL, NULL, 123, N'aaaaaaa', 0, CAST(N'2019-04-30T13:33:59.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (17, 0, 1, CAST(N'2019-05-01T15:02:20.000' AS DateTime), CAST(N'2019-05-01T15:06:05.000' AS DateTime), 516, NULL, NULL, N'aaaaaa', 0, CAST(N'2019-05-01T15:06:16.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (18, 0, 1, CAST(N'2019-05-01T15:06:30.000' AS DateTime), CAST(N'2019-05-01T15:27:07.000' AS DateTime), 516, NULL, NULL, N'Test', 0, CAST(N'2019-05-01T15:27:22.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (19, 2, 1, CAST(N'2019-05-04T15:09:09.000' AS DateTime), CAST(N'2019-05-04T16:00:07.000' AS DateTime), NULL, NULL, 237, N'Fixed error.', 0, CAST(N'2019-05-04T16:00:26.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (21, 2, 1, CAST(N'2019-05-07T15:15:47.000' AS DateTime), CAST(N'2019-05-07T17:43:35.000' AS DateTime), NULL, NULL, 239, N'Got error mostly fixed.', 0, CAST(N'2019-05-07T17:44:00.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (22, 2, 1, CAST(N'2019-05-08T09:55:20.000' AS DateTime), CAST(N'2019-05-08T10:59:48.000' AS DateTime), NULL, NULL, 239, N'Fixed the error.', 0, CAST(N'2019-05-08T11:00:06.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (23, 2, 1, CAST(N'2019-05-08T11:06:25.000' AS DateTime), CAST(N'2019-05-08T11:56:38.000' AS DateTime), NULL, NULL, 242, N'Test', 1, CAST(N'2019-05-08T11:07:08.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (24, 2, 1, CAST(N'2019-05-08T11:08:21.000' AS DateTime), CAST(N'2019-05-08T11:55:23.000' AS DateTime), NULL, NULL, 242, N'Test', 1, CAST(N'2019-05-08T11:08:44.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (25, 2, 1, CAST(N'2019-05-08T11:17:57.000' AS DateTime), CAST(N'2019-05-08T11:19:13.000' AS DateTime), NULL, NULL, 239, N'Test', 0, CAST(N'2019-05-08T11:19:24.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (26, 2, 1, CAST(N'2019-05-08T11:21:25.000' AS DateTime), CAST(N'2019-05-08T11:55:28.000' AS DateTime), NULL, NULL, 242, N'Test', 1, CAST(N'2019-05-08T11:21:46.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (27, 2, 1, CAST(N'2019-05-09T14:53:29.000' AS DateTime), CAST(N'2019-05-09T15:24:10.000' AS DateTime), NULL, NULL, 243, N'Researched and closed.', 0, CAST(N'2019-05-09T15:24:34.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (28, 2, 1, CAST(N'2019-05-09T15:39:58.000' AS DateTime), CAST(N'2019-05-09T16:37:37.000' AS DateTime), NULL, NULL, 184, N'Fixed Error', 0, CAST(N'2019-05-09T16:37:52.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (29, 2, 1, CAST(N'2019-05-13T14:47:48.000' AS DateTime), CAST(N'2019-05-13T16:14:00.000' AS DateTime), NULL, NULL, 196, N'Got error halfway fixed.', 0, CAST(N'2019-05-13T16:14:21.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (30, 0, 1, CAST(N'2019-05-15T11:23:37.000' AS DateTime), CAST(N'2019-05-15T12:31:41.000' AS DateTime), 516, NULL, NULL, N'aaaaaaaaa', 1, CAST(N'2019-05-15T11:24:25.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (31, 0, 1, CAST(N'2019-05-15T11:25:13.000' AS DateTime), CAST(N'2019-05-15T12:35:15.000' AS DateTime), 517, NULL, NULL, N'aaaaaaa', 1, CAST(N'2019-05-15T11:25:46.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (32, 0, 1, CAST(N'2019-05-15T11:26:03.000' AS DateTime), CAST(N'2019-05-15T12:46:06.000' AS DateTime), 517, NULL, NULL, N'qqqqq', 1, CAST(N'2019-05-15T11:26:41.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (33, 0, 1, CAST(N'2019-05-15T11:29:38.000' AS DateTime), CAST(N'2019-05-15T12:45:42.000' AS DateTime), 517, NULL, NULL, N'wwwwwwwww', 1, CAST(N'2019-05-15T11:30:04.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (34, 0, 1, CAST(N'2019-05-15T11:33:07.000' AS DateTime), CAST(N'2019-05-15T12:45:12.000' AS DateTime), 517, NULL, NULL, N'qqqqqq', 1, CAST(N'2019-05-15T11:33:33.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (35, 0, 1, CAST(N'2019-05-15T11:33:07.000' AS DateTime), CAST(N'2019-05-15T12:55:14.000' AS DateTime), 517, NULL, NULL, N'qqqqqqqqq', 1, CAST(N'2019-05-15T11:46:40.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (36, 0, 1, CAST(N'2019-05-15T11:50:20.000' AS DateTime), CAST(N'2019-05-15T13:01:13.000' AS DateTime), 517, NULL, NULL, N'qqqqqqq', 1, CAST(N'2019-05-15T11:51:19.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (37, 2, 1, CAST(N'2019-05-15T12:14:46.000' AS DateTime), CAST(N'2019-05-15T12:55:32.000' AS DateTime), NULL, NULL, 196, N'Fixed error.', 0, CAST(N'2019-05-15T12:55:44.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (38, 2, 1, CAST(N'2019-05-15T12:57:03.000' AS DateTime), CAST(N'2019-05-15T12:59:23.000' AS DateTime), NULL, NULL, 184, N'Passed', 0, CAST(N'2019-05-15T12:59:33.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (74, 0, 1, CAST(N'2019-07-30T15:25:13.000' AS DateTime), CAST(N'2019-07-30T16:36:04.000' AS DateTime), 519, NULL, NULL, N'Fixxed Include group box border color.  Added report format.', 0, CAST(N'2019-07-30T16:36:49.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (75, 0, 1, CAST(N'2019-07-31T10:50:14.000' AS DateTime), CAST(N'2019-07-31T11:22:39.000' AS DateTime), 519, NULL, NULL, N'Working on Crystal user report.', 0, CAST(N'2019-07-31T11:23:14.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (76, 0, 1, CAST(N'2019-07-31T12:48:03.000' AS DateTime), CAST(N'2019-07-31T14:25:16.000' AS DateTime), 519, NULL, NULL, N'Working on User Crystal report.', 0, CAST(N'2019-07-31T14:25:58.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (77, 0, 1, CAST(N'2019-08-05T13:47:18.000' AS DateTime), CAST(N'2019-08-05T16:06:07.000' AS DateTime), 519, NULL, NULL, N'User report code complete.', 0, CAST(N'2019-08-05T16:06:28.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (78, 0, 1, CAST(N'2019-08-06T12:30:32.000' AS DateTime), CAST(N'2019-08-06T15:37:24.000' AS DateTime), 519, NULL, NULL, N'Task, Testing Outline, Error Reports code complete.', 0, CAST(N'2019-08-06T15:38:29.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (79, 0, 1, CAST(N'2019-08-15T15:19:37.000' AS DateTime), CAST(N'2019-08-15T16:24:49.000' AS DateTime), 519, NULL, NULL, N'Almost done with Product report.', 0, CAST(N'2019-08-15T16:25:11.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (80, 0, 1, CAST(N'2019-08-16T14:29:54.000' AS DateTime), CAST(N'2019-08-16T15:26:53.000' AS DateTime), 519, NULL, NULL, N'All reports and form code complete.', 0, CAST(N'2019-08-16T15:27:29.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (81, 0, 1, CAST(N'2019-08-19T16:00:54.000' AS DateTime), CAST(N'2019-08-19T18:16:30.000' AS DateTime), 523, NULL, NULL, N'Researched SqlkData.  Created DevLogix project.  Organized RSDbLookup dll.  Added console project.  Started Query, QueryTable and SelectColumn classes.', 1, CAST(N'2019-08-19T18:18:12.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (82, 0, 1, CAST(N'2019-08-19T19:32:11.000' AS DateTime), CAST(N'2019-08-19T20:22:32.000' AS DateTime), 523, NULL, NULL, N'Finished SelectColumn and QueryTable classes.', 0, CAST(N'2019-08-19T20:23:33.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (83, 0, 1, CAST(N'2019-08-19T20:23:51.000' AS DateTime), CAST(N'2019-08-19T21:02:18.000' AS DateTime), 523, NULL, NULL, N'Created interface and MS Access class.', 0, CAST(N'2019-08-20T10:06:21.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (84, 0, 1, CAST(N'2019-08-20T10:10:50.000' AS DateTime), CAST(N'2019-08-20T17:55:41.000' AS DateTime), 523, NULL, NULL, N'Finished SELECT, FROM, and JOIN clauses.', 0, CAST(N'2019-08-20T17:56:20.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (85, 0, 1, CAST(N'2019-08-21T10:51:03.000' AS DateTime), CAST(N'2019-08-21T13:12:15.000' AS DateTime), 523, NULL, NULL, N'Converted ISelectSqlGenerator into abstract class and moved most of the code in MsAccessSelectSqlGenerator into it.', 0, CAST(N'2019-08-21T13:14:25.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (86, 0, 1, CAST(N'2019-08-21T13:14:31.000' AS DateTime), CAST(N'2019-08-21T17:52:04.000' AS DateTime), 525, NULL, NULL, N'Code complete!', 0, CAST(N'2019-08-21T17:52:18.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (87, 0, 1, CAST(N'2019-08-22T10:43:56.000' AS DateTime), CAST(N'2019-08-22T12:02:17.000' AS DateTime), 525, NULL, NULL, N'Organized and documented GetDataProcessor.', 0, CAST(N'2019-08-22T12:02:37.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (88, 0, 1, CAST(N'2019-08-22T12:04:01.000' AS DateTime), CAST(N'2019-08-22T16:04:06.000' AS DateTime), 526, NULL, NULL, N'Done with class.', 0, CAST(N'2019-08-22T16:04:20.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (89, 0, 1, CAST(N'2019-08-22T16:05:09.000' AS DateTime), CAST(N'2019-08-22T17:53:43.000' AS DateTime), 523, NULL, NULL, N'Added WHERE functionality to Query class.', 0, CAST(N'2019-08-22T17:54:31.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (90, 0, 1, CAST(N'2019-08-23T10:45:59.000' AS DateTime), CAST(N'2019-08-23T18:51:38.000' AS DateTime), 523, NULL, NULL, N'Nested query and WHERE clause code complete.  Just need to document WHERE clause.', 0, CAST(N'2019-08-23T18:52:46.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (91, 0, 1, CAST(N'2019-08-24T11:10:28.000' AS DateTime), CAST(N'2019-08-24T21:07:50.000' AS DateTime), 523, NULL, NULL, N'Added support for SQL Server, MySQL and Sqlite.', 0, CAST(N'2019-08-24T21:08:47.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (92, 0, 1, CAST(N'2019-08-25T10:09:00.000' AS DateTime), CAST(N'2019-08-25T16:07:24.000' AS DateTime), 527, NULL, NULL, N'Got basic structure done.', 0, CAST(N'2019-08-25T16:07:48.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (93, 0, 1, CAST(N'2019-08-26T12:07:31.000' AS DateTime), CAST(N'2019-08-26T12:51:04.000' AS DateTime), 527, NULL, NULL, N'Added RSDbLookupContext.', 0, CAST(N'2019-08-26T12:51:58.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (94, 0, 1, CAST(N'2019-08-26T12:52:12.000' AS DateTime), CAST(N'2019-08-26T17:50:20.000' AS DateTime), 523, NULL, NULL, N'Got most of Enum fields done.', 0, CAST(N'2019-08-26T17:50:44.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (95, 0, 1, CAST(N'2019-08-26T19:10:07.000' AS DateTime), CAST(N'2019-08-26T21:17:40.000' AS DateTime), 523, NULL, NULL, N'Almost finished enum wheres.', 0, CAST(N'2019-08-26T21:18:13.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (96, 0, 1, CAST(N'2019-08-27T10:55:57.000' AS DateTime), CAST(N'2019-08-27T18:18:06.000' AS DateTime), 523, NULL, NULL, N'Finished Enums and formulas.  Almost done with order by.', 0, CAST(N'2019-08-27T18:18:49.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Timeclock] ([intTimeclockId], [bytType], [intUserID], [dtePunchIn], [dtePunchOut], [intTaskID], [intOutlineID], [intErrorID], [txtNotes], [bolPunchChanged], [dteModifiedDate], [strModifiedBy]) VALUES (97, 0, 1, CAST(N'2019-08-28T10:19:54.000' AS DateTime), CAST(N'2019-08-28T17:40:24.000' AS DateTime), 527, NULL, NULL, N'Got basic striucture done.  Added support for date and enum field definitions.', 0, CAST(N'2019-08-28T17:41:56.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_UserDaysOff] ([intUserID], [dteDateOff], [strDescription]) VALUES (1, CAST(N'2019-03-30T00:00:00.000' AS DateTime), N'Birthday')
GO
INSERT [dbo].[TB_UserGroups] ([intUserID], [intGroupID]) VALUES (1, 1)
GO
INSERT [dbo].[TB_UserGroups] ([intUserID], [intGroupID]) VALUES (9, 3)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Peter Ringering', N'Peter Ringering', NULL, N'PTR', 0, N'111000111', 2, N'CgCBJvx4/KZyKktbJQH52d2wGHdbX+V4z52+EKqSrfu8AijUlxboSBlOjuYBUJyinj/MS9TuL4wN53kBDIGwePlArGPg8qRbXyJIxX9BPn7glxvoY9kHqT669yPP/Pm/2MDya1PXFve1AU/y9+8ESusK+i8PdkAInwfKPNv7+oxIMdQYy9K8mDUh8a8t/+cu813frqVGxNTpiNozJeqkAwgujoUsV9iFhZy937Nk/NZuy2gbLHBWiiX0wXtzg8fpBWvLO7mT2JsNSrgvUijSQi0KTR7J5etjt2CB/NB4W6aAkhPOBaN8sftsAlJLRTbHoF1rjKKusJAPsXIS/UKiZ0X22guI1Yj5DNJHtV5ZOaRwDXpq0aFrWY4mYF0sYD6PmuYvEP/gpaf7wF0WnIilh8dda7WGVnavScuPSVnkg+nR50hROqnJBpPKb2a+Hedha/fMO4plijE7mr02ZNweFUP1z2MMdx2KSOFn+yaV4/bfTb9A6WZPHlJJmAH6MOn/L2o0AUi9qHUaocdVUJ72NIKrezpMlkUZ8hSKHvO5mp/m/jRw8VB+XOh/WAFyYlay5Y3YAhqRtoNr5XQqFmmlORPpSTG5O9QCaSSwFq1TCFI/qbIoQS7ERXEEnY4N2P1ITB1io+qf5yrX1pLtUq6/HIDDRANNcViJhFWFq9r8QDDxNM1YSko29N3XpKHoaOjT2h7IB3A4RMtzuPXloXMbEdBtcoKk6Ca6bMcIwN6wPllPSuVsEhQqA8ean+vcfSGhChXL/W5Rxp2UWPhs8r7PiphI73KI9oGcInj56zmRNBFNWibd9Yp4dbwbPttaUNaCZAAQkngihtEFXomJ3wZj/4OJLJr1L8FrnkVNl/L7jzX57MUdX2SY0OgbLZLF4pG2', CAST(N'2019-03-06T16:48:41.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'Walter Obrien', N'Walter OBryan', NULL, NULL, 0, N'111000111', 0, N'kh/vvUBtGWg2JHC5RzvxSeZ0Z89HZ8mxIYr4bCmWRraNQweSkQqOAr0sP0/b30yv33Q59WZaaz05WMdSGZnN2Y5zlX7Uz4uhtajiflRAVqrkr18xQU3a25xk4CegLsoMJssXjyeYLQfoUslPMi1ZL1dCluECdWIutoB4uEZH5RvaWrXOYoDRDSHcrKXj6q0BbleMizpQlz2XEP47z11z2RQ3KVgyyvQLLhrsOIWj/3Ib/WnEgAsvmQhdxzNtofZbdpFGxB/srjRBzGAujbimx+EHzcsV00nMnWyLdtViTydupSWwpoWG8uOE4jIa+vWUbXbjF/mBzdTZ4+NLYAHxUvg/mQgU9by9ScoStlhfaFlVCqofvL3P0f2gOTraYjpl4qGJ/oQgEwXXGuG5GXFvrwGnKnZ20tziINwJFFvlmUr6OwOrmySsM6wsF7RzNjxUYo8AFtgxwOzimAgrvXfOqc3IKD803gk13Rg80FGkzIXkFKnLHFbgkMkod6AnBWbnHVDc8CQw84NhYZ+GYTFgfpZigkTFEwfA5T1l4Y1S/wc6CYUdSML81fjS+A0aOxyrXLzUgValS0SsM/X1dXG1v/O/QxRyyMA0CN1pg2xsD2/K92P8x0jAMxbHNjJbvayxQu8U7IZNV1HcBO27whPaN+ptiuvIyLtWA/wmX+xnzlC5qEVai08SoNIDncvD9F1B6E+8qJpwvj75v2e7ecXfyoW0jN4Jt3PIhjZ/E0peqUpomoxgIyJWlN4A81wScEi//3JYVruq4a4qU7Hf6622MA==', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (3, N'David Matthews', N'David Matthews', NULL, NULL, 0, N'111000111', 1, N'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (6, N'Everyone', NULL, N'Notes', NULL, 0, N'111000111', 0, N'6cWHQkuVDGJsBDhBPwp89w==', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (4, N'Kevin Thornberg', N'Kevin Thornberg', NULL, NULL, 0, N'111000111', 0, N'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (5, N'Steven Jones', N'Steven Jones', NULL, NULL, 0, N'111000111', 0, N'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (7, N'Tim Benson', N'Tim Benson', NULL, NULL, 0, N'111000111', 1, N'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (9, N'Dawna Harkitay', N'Dawna Harkitay', NULL, NULL, 0, N'111000111', 0, N'oJTyStKbblS7RO/6a5aeSIT0a5FhmxmZxXsxgMiLiC/ZH2tsv4sXzstY7bJSiFO+WE4O53e1tJvdumovihskAShO3/w8/LXZx+r4xjwgE2vSo/eIgCkXWJlcgy4P/wjFUrHjlfSLXrkzjhpMQUOLhs3TbXQxmTJGuda1pqK0w7gUxGr3B4/mIWsZBr9TBFwXkAp0Er+tI9Z9qeHRkMx+qqbL0GE/X/+3qT0d21xkJjL0uiedA5YI0XtichybWIbDWqXSTxPeYM6xLxLOFSGFI3Ae3Gf+xP9LBhOL9hSEGURS07ROq7zel87r0hA5XOwp4gegd/h0RHDpbuOq0otbCJOw9B3zz/G1kQoAwHh2CxZqdHgm/4LbOWSYRWtrq6UPKh5RjdmgRVPPsJczihWXQZ4hB2ClBkyw55IEoG3YfTWKEYL4OCwP2GntSAW7eYka/fO1zpuveJa7EMksIMRwINbkWT8RKoJR47JVBY5PD51Dro9ROLVZsGB4TZc5nk8mPrsoFybqtiReds07t/wkL75trUzMA4qXSxA5DEj+gBjiDuAW5EvtyTKMZ7yea/Fam0KXGtEN5Dsa8/9AnuCC3OUImBz3oCXWgB7qic3g4SPihmdg/Iomrl1htRZ3JaTXJBygzAmnqKzdBhRJ60pt1U6Rp58Rp0cUI5mBw9GQgkt9INYb0D60nm30sXIEK89D/buA2BG0uM+zLaqImLV4ceHqlUr9o7iFEv/iKs7QxvKDQ6SfqCk5cW6o9jDdxt3u', CAST(N'2019-03-02T12:22:09.000' AS DateTime), N'Dawna Harkitay')
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (10, N'Becca Smith', N'Becca Smith', NULL, NULL, 0, N'111000111', 2, N'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3LrXXAOy58so98PYSHSSs3fy6eG9088GEU/1cSXCJpSbHGPhn7DnbHNmErC/k3UTTRK1oHw1mKz6i+WEXIi/szQHGYDLt7PNX6bnWtRuZ7RBbaDmtyurjRQGhHH0+zBzPo6CcSkvIwYrrDrVrfw6OOtIe/Z2l2TbhemJVhQJSpZqMzg2GFoqF6BxXuPjq1GtWA86GbIQh9iWmIWUOT/OtnvWPzdsdZhJPbjRTkMQ8QTZBTYgWKaDapRIN6yZ97MfclYZ0eVabzujJGBEhp5kMqE6gMqV1vUZH9RS4vVw+ap+OI0IaGk1PNKA24UDhL2UHZOQFpZp49uTgMbss9MXGww8zWnLrEae/xdivrgREt3YmVNe4cyEkMiDRG99mUeSyRY5TDqTKPMnyMzmnDQI5e9Zbjj6S1sNcBKQl3906w8v0pLPA3TrfdyF1bYXyAge5JMxA8oT1aiHBI2+d0Q1fi8A49N3XqI020YGdnodCKN5jCtBGHBHFFwlzp8cdQVOQ+b0X3cptCSVx0r08pVqZ72uU5zLTHxCUXip7VyVJ5oYg==', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (11, N'Samuel Dunkin', N'Samuel Dunkin', NULL, NULL, 0, N'111000111', 1, N'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (12, N'Patrick Jane', N'Patrick Jane', NULL, NULL, 0, N'111000111', 1, N'KBBs78uo5EIRXasbqJ/VX0xUWVkiXfsQr6ZkhFEhDfVEviFuySZBSbNW7ALhW3HvXF4csvWubvjjvDlws9qPOBadwyqy6rLQKf11+sU9BfFgquT+nZ3Zdxlob0x6vkYGo+Ug5OhbKRFouozZIpb0u/sVIyx5Me7VbtBtv9qXLU/vCpESN1AgIEovTg2fSjEk3Ds5o81ZRj0XrQGKEue965WRk43N8jS14k/cKBYAaeQUqnzdUAdPDmHun0k1nPwk251QVXRl6jUAdj9imoPOEBxivP5rKU2AFD9MZ1hqjS8KuuM+Q3kBibaXIiSQkP1huMBu7IdJr1/KTG53w7GjjRwv03kcJDWLF1WtoZxh3gdMXVDZfJGArdRGb75eQxe+bxP7qi2zDZgE488SqLAiYJxAbHmjVbdoE70SrD6CkTrjagab+UpTEY26gksY3RlCymmgCaY+uh1Mq+fUjd2MZ8Sy6Ti3HfAXVhgl7kxmrg2hAWGj+8UvAy7WNeUoi5xF1dOmYpTdWy9Ha3bTWVF5qPTxaSc/sjk6LuWcSz5faIgaMI2r5k+MHGGRiaT5fuLJI8ipZw078PYJNY3FTvZeADirdcfxvXRkLKgj3qXX/LX2DO7krNPNfIFGF0EvRXLlZk7LPhyb/LEmlhgcEubrIKi6XjiIqmJBKmuWntMKPf9kaI+P8zNwIz8nMMAtpKlQ+HSuVuCc95XdHiyowdeouQIq3b4wYaN+i/BdU630bGdz8uLpGG/Vb4lo51QD48+SkVSK1BkShAxwfDuFk4OwwF35XaTF429eCP17AGTU3hVBHt4qje7taz60hRwFPBGD', NULL, NULL)
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (17, N'Supervisor', NULL, NULL, NULL, 1, N'111000111', 0, N'CgCBJvx4/KZyKktbJQH52d2wGHdbX+V4z52+EKqSrfu8AijUlxboSBlOjuYBUJyinj/MS9TuL4wN53kBDIGwePlArGPg8qRbXyJIxX9BPn7gucW3xZ6uyR+d3/oxB015JFp/Kv0PmvVdhBZtNSVDKGetfN+P5PhPuW2fO8Qy6gcx9RXv2QYMh2zHl9K+JRghwKa6Ray6SoqoLRx+nErN+H0dhmOD2DvnvE/sSb7wA8Ohlvek65mDdobWHPtag6FlpTLPie7wYhNFAywJndP1y/EOAcPZozI23hXWhu1XvAmZxyeKSORki0OPd2+hHFblt1YMPkLHVVsKf+WWLlMxqHC0k0nZBHDocSzzXUDLNWMnsUkemxhX9Dv0AVJT0cN/dRK5Cd79Rx6geLFx2C2B6/gjUJ9p+xRbyd5M8qsJTFEkxdoZGToz8SGo730JLW5fFhRuDc0DWYb48yBKdbQbCKqkgVilOpH8/3Rgf+07CTiqJ4NP+EN/IEHxVaIIUyhEnPb+cpelwdEhsDILWrNFrVv8+atcsFHji9Zm3pRy+EaCYxj/15mQp+qP+dI6JtRk21rS2qwZzV88cv/qkX21dZERjFq9NMOiJhV9JLBr4/RD62KuD/ExTZFIgfbeRJaxEREPn4Bdg5wxRbCBUX5rEGhQ7PXvIufC67fSKiFNNblF+00XLwjORXysWcLIVLSYxjqLB3mAjo64VD9St5J1VTjJSxBRmw12otN42OWjE/cWYiyUHpfxNgn8/zpSNwxAr357UJHeFT98oRYakCMCJfkQ17Sm5TP3F4FAqGpAwsDsyTlq9O02BJ3PwrvltL7Y/vZkZpNN8CBJZXmS6qYeo7AwYCjrP2kOVEnDThA1O/XE7ab5mIDv9JHpGwIzi0hb', CAST(N'2019-03-02T11:47:58.000' AS DateTime), N'Supervisor')
GO
INSERT [dbo].[TB_Users] ([intUserID], [strUserName], [strEmailAddress], [txtNotes], [strInitials], [bolSUP], [strPassword], [bytDeveloperType], [txtRights], [dteModifiedDate], [strModifiedBy]) VALUES (60, N'Teresa Lisbond', N'Teresa Lisbond', NULL, NULL, 0, N'111000111', 1, N'oJTyStKbblS7RO/6a5aeSL0K/3RdAbLH030zp63jGkYGmTEmYGLik2OMHxd82NJ09OR81T1XxGOsWkyqX9XUMmdQFhdqtmfPOAENznV3huwt7ZVqCTMLWebZZr6zgQdUkKApuTc3tfGVLc5E5QUBeiIxUaiXX9J0w1MjFrG59enEm0ZzQucj0eeIlK/solOBZEoGx0l8et6MO5XE8lEl0omUk4UdH0sKiB7OvvBKN3KKDlZMpmjUGIBLxGml35aC9Mb9Efs2hTXpjBT45Xv5UbLlJaxpfovdiI8WOannvoKYDKOLM3uRkgonuhVD5uG9E53d4dsQPZR96KmoxLxr48Mp6eqM1nt9mSLyd3BiJM8qjVHbYdv6JfYnu6EBRMMxYSOIqBqN1glz3ph4Ka/ng6oLO1NGXIzYhL6fBtvR7kCYSpi2hz70+pFjnb9ZGN95E60eO/0YCZwU6NPjU0GquDLfFDzjG07qWaPx8Ul2grRiUkUzyOy/+QIL1fIE0HSI2f0Zlo/+RmND6MBYl0uvIPKsXzk5JfrwY82ZK9g06NgpXmnbf4gt/KewFMwoZFpgOLuUeOtuemIJTKaHmCBpY1gsVFWokEEz3UIv0ABoUmftSZzScSxbqqVRDwwPhzxLmnfxBXox9UdpMuag5siuk/ctupidYGdhyskf2aOPdMT3diNZl6w+ily9eoIshqfjBZK9jjJvezYH51jiygrMslm5jTWtsxFk7dKLZLWNVK4jEqldUu6UrmY0BVnynSVwWAFvNTNYCgPIJyX8g3GCdA==', NULL, NULL)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (1, 2)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (1, 3)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (1, 4)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (1, 5)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (3, 1)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (9, 1)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (10, 1)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (11, 1)
GO
INSERT [dbo].[TB_UserSupervisors] ([intUserID], [intSupID]) VALUES (12, 1)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (1, N'Version 0', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-08-21T19:47:45.000' AS DateTime), NULL, CAST(N'2015-08-21T19:47:10.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (2, N'00.85.0001', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-08-21T16:39:42.000' AS DateTime), NULL, CAST(N'2015-08-21T16:39:47.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (3, N'00.85.0002', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (4, N'00.85.0003', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (5, N'00.85.0004', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (6, N'00.85.0006', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (7, N'00.85.0005', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (8, N'00.91.0008', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 28, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (9, N'00.91.0007', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 28, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (10, N'00.91.0006', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 28, CAST(N'2015-08-21T21:53:18.000' AS DateTime), NULL, CAST(N'2015-08-21T21:53:25.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (11, N'00.95.0005', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 29, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (12, N'00.95.0004', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 29, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (13, N'00.95.0002', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 29, CAST(N'2015-08-21T21:54:43.000' AS DateTime), NULL, CAST(N'2015-08-21T21:54:45.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (14, N'00.85.0007', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (15, N'00.91.0009', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 28, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (16, N'00.95.0006', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 29, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (17, N'00.85.0008', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (18, N'00.95.0007', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 29, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (19, N'00.91.0010', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 28, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (20, N'00.91.0011', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 28, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (21, N'00.95.0008', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 29, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (22, N'00.85.0009', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (45, N'00.95.0009', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 29, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (46, N'00.95.0010', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 29, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (47, N'00.91.0012', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 28, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (48, N'00.91.0013', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 28, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (49, N'00.85.0010', CAST(N'2015-01-01T00:00:00.000' AS DateTime), 30, CAST(N'2015-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (50, N'00.95.0011', CAST(N'2015-04-25T00:00:00.000' AS DateTime), 29, CAST(N'2015-04-27T10:32:01.000' AS DateTime), CAST(N'2015-04-27T10:32:47.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (51, N'00.91.0014', CAST(N'2015-02-04T17:03:12.000' AS DateTime), 28, CAST(N'2015-02-04T17:03:12.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (52, N'00.95.0012', CAST(N'2015-02-04T17:42:04.000' AS DateTime), 29, CAST(N'2015-02-04T17:42:04.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (53, N'00.85.0011', CAST(N'2015-02-04T17:43:35.000' AS DateTime), 30, CAST(N'2015-02-04T17:43:35.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (54, N'00.91.0015', CAST(N'2015-02-23T16:43:09.000' AS DateTime), 28, CAST(N'2015-02-23T16:43:09.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (56, N'00.85.0012', CAST(N'2015-02-23T16:51:06.000' AS DateTime), 30, CAST(N'2015-02-23T16:51:06.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (57, N'00.91.0016', CAST(N'2015-04-27T19:03:33.000' AS DateTime), 28, CAST(N'2015-04-27T19:03:33.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (58, N'00.95.0013', CAST(N'2015-04-27T19:04:37.000' AS DateTime), 29, CAST(N'2015-04-27T19:04:37.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (59, N'00.85.0013', CAST(N'2015-04-27T19:06:29.000' AS DateTime), 30, CAST(N'2015-04-27T19:06:29.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (60, N'00.92.0017', CAST(N'2015-05-18T19:07:04.000' AS DateTime), 28, CAST(N'2015-05-18T19:07:04.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (61, N'00.95.0014', CAST(N'2015-05-18T19:11:54.000' AS DateTime), 29, CAST(N'2015-05-18T19:11:54.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (62, N'00.92.0018', CAST(N'2015-05-18T19:14:52.000' AS DateTime), 28, CAST(N'2015-05-18T19:14:52.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (63, N'00.85.0014', CAST(N'2015-05-18T21:26:25.000' AS DateTime), 30, CAST(N'2015-05-18T21:26:25.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (64, N'00.91.0005', CAST(N'2014-08-24T21:48:04.000' AS DateTime), 30, CAST(N'2014-08-24T21:48:04.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (65, N'00.85.0015', CAST(N'2015-05-19T22:21:16.000' AS DateTime), 30, CAST(N'2015-05-19T22:21:16.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (66, N'00.85.0016', CAST(N'2015-05-20T15:10:49.000' AS DateTime), 30, CAST(N'2015-05-20T15:10:49.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (67, N'00.85.0017', CAST(N'2015-05-22T14:38:48.000' AS DateTime), 30, CAST(N'2015-05-22T14:38:48.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (68, N'00.92.0019', CAST(N'2015-05-22T14:39:30.000' AS DateTime), 28, CAST(N'2015-05-22T14:39:30.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (69, N'00.95.0015', CAST(N'2015-05-22T14:40:07.000' AS DateTime), 29, CAST(N'2015-05-22T14:40:07.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (70, N'00.91.0018', CAST(N'2015-05-23T10:24:12.000' AS DateTime), 30, CAST(N'2015-05-23T10:24:12.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (71, N'00.92.0020', CAST(N'2015-05-23T10:24:49.000' AS DateTime), 28, CAST(N'2015-05-23T10:24:49.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (72, N'00.96.0016', CAST(N'2015-05-23T10:25:49.000' AS DateTime), 29, CAST(N'2015-05-23T10:25:49.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (73, N'00.91.0019', CAST(N'2015-05-23T11:20:32.000' AS DateTime), 30, CAST(N'2015-05-23T11:20:32.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (74, N'00.92.0021', CAST(N'2015-05-23T11:21:33.000' AS DateTime), 28, CAST(N'2015-05-23T11:21:33.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (75, N'00.96.0017', CAST(N'2015-05-23T11:22:03.000' AS DateTime), 29, CAST(N'2015-05-23T11:22:03.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (76, N'00.91.0020', CAST(N'2015-05-26T20:03:49.000' AS DateTime), 30, CAST(N'2015-05-26T20:03:49.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (77, N'00.96.0018', CAST(N'2015-05-26T20:05:11.000' AS DateTime), 29, CAST(N'2015-05-27T10:34:11.000' AS DateTime), CAST(N'2015-06-07T10:34:22.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (78, N'00.92.0022', CAST(N'2015-05-26T20:06:04.000' AS DateTime), 28, CAST(N'2015-05-26T20:06:04.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (79, N'00.92.0023', CAST(N'2015-06-09T20:52:05.000' AS DateTime), 28, CAST(N'2015-06-10T10:24:17.000' AS DateTime), CAST(N'2015-07-05T10:24:45.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (80, N'00.96.0019', CAST(N'2015-06-09T20:55:18.000' AS DateTime), 29, CAST(N'2015-06-10T10:35:55.000' AS DateTime), CAST(N'2015-07-05T10:36:03.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (81, N'00.91.0021', CAST(N'2015-06-09T20:58:32.000' AS DateTime), 30, CAST(N'2015-06-09T20:58:32.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (82, N'00.93.0024', CAST(N'2015-07-07T15:48:28.000' AS DateTime), 28, CAST(N'2015-07-07T15:48:28.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (83, N'00.97.0020', CAST(N'2015-07-07T15:52:04.000' AS DateTime), 29, CAST(N'2015-07-08T10:37:21.000' AS DateTime), CAST(N'2015-07-08T10:37:29.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (84, N'00.91.0022', CAST(N'2015-07-07T16:58:38.000' AS DateTime), 30, CAST(N'2015-07-07T16:58:38.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (85, N'1.00.0025', CAST(N'2015-07-24T13:35:13.000' AS DateTime), 28, CAST(N'2015-07-24T13:35:13.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (86, N'1.00.0021', CAST(N'2015-07-24T13:37:10.000' AS DateTime), 29, CAST(N'2015-07-24T13:37:10.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (87, N'00.91.0023', CAST(N'2015-07-24T13:52:07.000' AS DateTime), 30, CAST(N'2015-07-24T13:52:07.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (88, N'1.00.0026', CAST(N'2015-07-29T10:46:09.000' AS DateTime), 28, CAST(N'2015-07-29T10:26:37.000' AS DateTime), CAST(N'2015-07-29T10:26:53.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (89, N'1.00.0022', CAST(N'2015-07-29T10:47:17.000' AS DateTime), 29, CAST(N'2015-07-29T10:47:17.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (90, N'00.91.0024', CAST(N'2015-07-29T10:48:37.000' AS DateTime), 30, CAST(N'2015-07-29T10:48:37.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (91, N'00.91.0025', CAST(N'2015-07-31T16:21:00.000' AS DateTime), 30, CAST(N'2015-07-31T16:21:00.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (92, N'1.00.0027', CAST(N'2015-07-31T18:01:27.000' AS DateTime), 28, CAST(N'2015-07-31T18:01:27.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (93, N'1.00.0029', CAST(N'2015-07-31T18:05:32.000' AS DateTime), 28, CAST(N'2015-07-31T18:05:32.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (94, N'1.00.0024', CAST(N'2015-07-31T18:06:27.000' AS DateTime), 29, CAST(N'2015-07-31T18:06:27.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (95, N'00.91.0026', CAST(N'2015-08-04T21:48:04.000' AS DateTime), 30, CAST(N'2015-08-04T21:48:04.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (96, N'1.00.0030', CAST(N'2015-08-04T21:49:13.000' AS DateTime), 28, CAST(N'2015-08-04T21:49:13.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (97, N'1.00.0031', CAST(N'2015-08-06T22:00:00.000' AS DateTime), 28, CAST(N'2015-08-25T12:05:47.000' AS DateTime), CAST(N'2015-08-07T10:28:33.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (98, N'1.01.0032', CAST(N'2015-08-11T13:40:33.000' AS DateTime), 28, CAST(N'2015-08-12T10:29:38.000' AS DateTime), CAST(N'2015-08-14T10:29:58.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (99, N'00.91.0027', CAST(N'2015-08-11T13:44:38.000' AS DateTime), 30, CAST(N'2015-08-11T13:44:38.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (100, N'1.02.0033', CAST(N'2015-08-16T22:11:48.000' AS DateTime), 28, CAST(N'2015-08-16T22:11:48.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (101, N'00.91.0028', CAST(N'2015-08-16T22:13:28.000' AS DateTime), 30, CAST(N'2015-08-16T22:13:28.000' AS DateTime), NULL, CAST(N'2015-08-22T00:00:00.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (102, N'1.01.0028', CAST(N'2015-08-19T20:36:52.000' AS DateTime), 29, CAST(N'2015-08-22T19:09:18.000' AS DateTime), NULL, CAST(N'2015-08-22T22:17:40.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (103, N'00.91.0029', CAST(N'2015-08-19T20:38:16.000' AS DateTime), 30, CAST(N'2015-08-22T19:10:53.000' AS DateTime), NULL, CAST(N'2015-08-22T22:19:20.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (104, N'1.02.0034', CAST(N'2015-08-19T20:39:02.000' AS DateTime), 28, CAST(N'2015-08-22T19:05:49.000' AS DateTime), NULL, CAST(N'2015-08-22T22:16:44.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (105, N'1.00.0027', CAST(N'2015-08-19T10:39:01.000' AS DateTime), 29, CAST(N'2015-08-19T10:39:40.000' AS DateTime), CAST(N'2015-08-19T10:39:49.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (106, N'00.85.0001', CAST(N'2015-08-22T16:50:02.000' AS DateTime), 33, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (107, N'1.02.0035', CAST(N'2015-08-22T19:06:21.000' AS DateTime), 28, CAST(N'2015-08-22T22:15:48.000' AS DateTime), NULL, CAST(N'2015-08-24T14:36:29.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (108, N'1.01.0029', CAST(N'2015-08-22T19:09:23.000' AS DateTime), 29, CAST(N'2015-08-22T22:18:18.000' AS DateTime), NULL, CAST(N'2015-08-24T14:51:55.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (109, N'00.91.0030', CAST(N'2015-08-22T19:10:57.000' AS DateTime), 30, CAST(N'2015-08-22T22:19:34.000' AS DateTime), NULL, CAST(N'2015-08-24T14:53:14.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (110, N'1.02.0036', CAST(N'2015-08-22T22:15:59.000' AS DateTime), 28, CAST(N'2015-08-24T22:09:35.000' AS DateTime), NULL, CAST(N'2015-08-25T17:50:44.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (111, N'1.01.0030', CAST(N'2015-08-22T22:18:23.000' AS DateTime), 29, CAST(N'2015-08-24T22:10:28.000' AS DateTime), NULL, CAST(N'2015-08-25T17:54:23.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (112, N'00.91.0031', CAST(N'2015-08-22T22:19:38.000' AS DateTime), 30, CAST(N'2015-08-24T22:11:28.000' AS DateTime), NULL, CAST(N'2015-08-25T17:55:41.000' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (113, N'1.02.0037', CAST(N'2015-08-24T22:09:58.000' AS DateTime), 28, CAST(N'2015-08-25T17:51:11.000' AS DateTime), NULL, CAST(N'2015-09-10T11:35:12.000' AS DateTime), NULL, CAST(N'2015-09-10T11:35:14.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (114, N'1.01.0031', CAST(N'2015-08-24T22:10:41.000' AS DateTime), 29, CAST(N'2015-08-25T17:54:11.000' AS DateTime), NULL, CAST(N'2015-09-10T11:36:57.000' AS DateTime), NULL, CAST(N'2015-09-10T11:36:59.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (115, N'00.91.0032', CAST(N'2015-08-24T22:11:13.000' AS DateTime), 30, CAST(N'2015-08-25T17:55:26.000' AS DateTime), NULL, CAST(N'2015-09-10T11:38:20.000' AS DateTime), NULL, CAST(N'2015-09-10T11:38:21.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (118, N'1.02.0038', CAST(N'2015-08-25T17:51:16.000' AS DateTime), 28, CAST(N'2015-09-10T11:34:49.000' AS DateTime), NULL, CAST(N'2015-09-12T11:49:11.000' AS DateTime), NULL, CAST(N'2015-09-12T11:49:12.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (119, N'1.01.0032', CAST(N'2015-08-25T17:53:48.000' AS DateTime), 29, CAST(N'2015-09-10T11:36:47.000' AS DateTime), NULL, CAST(N'2015-09-12T11:50:46.000' AS DateTime), NULL, CAST(N'2015-09-12T11:50:48.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (120, N'00.91.0033', CAST(N'2015-08-25T17:54:58.000' AS DateTime), 30, CAST(N'2015-09-10T11:38:09.000' AS DateTime), NULL, CAST(N'2015-09-12T11:51:33.000' AS DateTime), NULL, CAST(N'2015-09-12T11:51:34.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (121, N'1.02.0039', CAST(N'2015-09-10T11:34:00.000' AS DateTime), 28, CAST(N'2015-09-12T11:49:00.000' AS DateTime), NULL, CAST(N'2015-09-16T22:10:59.000' AS DateTime), NULL, CAST(N'2015-09-16T22:11:00.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (122, N'1.01.0033', CAST(N'2015-09-10T11:36:10.000' AS DateTime), 29, CAST(N'2015-09-12T11:50:36.000' AS DateTime), NULL, CAST(N'2015-09-16T22:12:40.000' AS DateTime), NULL, CAST(N'2015-09-16T22:12:46.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (123, N'00.91.0034', CAST(N'2015-09-10T11:37:36.000' AS DateTime), 30, CAST(N'2015-09-12T11:51:24.000' AS DateTime), NULL, CAST(N'2015-09-16T22:13:31.000' AS DateTime), NULL, CAST(N'2015-09-16T22:13:34.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (124, N'1.02.0040', CAST(N'2015-09-12T11:48:14.000' AS DateTime), 28, CAST(N'2015-09-16T22:10:46.000' AS DateTime), NULL, CAST(N'2015-09-19T20:59:26.000' AS DateTime), NULL, CAST(N'2015-09-19T20:59:28.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (125, N'1.01.0034', CAST(N'2015-09-12T11:50:07.000' AS DateTime), 29, CAST(N'2015-09-16T22:12:03.000' AS DateTime), NULL, CAST(N'2015-10-08T12:39:03.000' AS DateTime), NULL, CAST(N'2015-10-08T12:39:07.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (126, N'00.91.0035', CAST(N'2015-09-12T11:51:07.000' AS DateTime), 30, CAST(N'2015-09-16T22:13:21.000' AS DateTime), NULL, CAST(N'2015-10-10T21:50:04.000' AS DateTime), NULL, CAST(N'2015-10-10T21:50:07.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (127, N'1.02.0041', CAST(N'2015-09-16T22:10:22.000' AS DateTime), 28, CAST(N'2015-09-19T20:59:05.000' AS DateTime), CAST(N'2015-09-19T20:59:10.000' AS DateTime), NULL, NULL, CAST(N'2015-09-19T20:59:13.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (128, N'2.00.0035', CAST(N'2015-09-24T21:51:13.000' AS DateTime), 29, CAST(N'2015-10-08T12:38:36.000' AS DateTime), NULL, CAST(N'2015-10-10T21:45:31.000' AS DateTime), NULL, CAST(N'2015-10-10T21:45:35.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (129, N'00.91.0036', CAST(N'2015-09-16T22:13:01.000' AS DateTime), 30, CAST(N'2015-10-10T21:49:31.000' AS DateTime), NULL, CAST(N'2015-12-02T13:47:52.000' AS DateTime), NULL, CAST(N'2015-12-02T13:47:54.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (130, N'1.03.0042', CAST(N'2015-09-19T20:57:47.000' AS DateTime), 28, CAST(N'2015-10-10T21:43:03.000' AS DateTime), NULL, CAST(N'2015-12-02T13:38:26.000' AS DateTime), NULL, CAST(N'2015-12-02T13:38:29.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (131, N'2.00.0036', CAST(N'2015-10-08T12:39:25.000' AS DateTime), 29, CAST(N'2015-10-10T21:44:43.000' AS DateTime), NULL, CAST(N'2015-10-17T22:00:25.000' AS DateTime), NULL, CAST(N'2015-10-17T22:00:27.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (132, N'1.03.0043', CAST(N'2015-10-10T21:42:38.000' AS DateTime), 28, CAST(N'2015-12-02T13:38:53.000' AS DateTime), NULL, CAST(N'2015-12-05T21:06:15.000' AS DateTime), NULL, CAST(N'2015-12-05T21:06:19.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (133, N'00.91.0037', CAST(N'2015-10-10T21:50:22.000' AS DateTime), 30, CAST(N'2015-12-02T13:47:40.000' AS DateTime), NULL, CAST(N'2015-12-05T21:07:52.000' AS DateTime), NULL, CAST(N'2015-12-05T21:07:54.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (134, N'2.00.0037', CAST(N'2015-10-17T22:00:00.000' AS DateTime), 29, CAST(N'2015-10-17T22:00:14.000' AS DateTime), CAST(N'2015-10-17T22:00:14.000' AS DateTime), NULL, NULL, CAST(N'2015-12-02T13:45:55.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (135, N'1.03.0044', CAST(N'2015-12-02T13:39:06.000' AS DateTime), 28, CAST(N'2015-12-05T21:06:30.000' AS DateTime), CAST(N'2015-12-05T21:06:34.000' AS DateTime), NULL, NULL, CAST(N'2015-12-05T21:06:37.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (136, N'3.00.0038', CAST(N'2015-12-02T13:43:31.000' AS DateTime), 29, CAST(N'2015-12-02T13:43:49.000' AS DateTime), NULL, CAST(N'2015-12-05T21:07:00.000' AS DateTime), NULL, CAST(N'2015-12-05T21:07:01.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (137, N'3.00.0039', CAST(N'2015-12-02T13:44:16.000' AS DateTime), 29, CAST(N'2015-12-05T21:07:10.000' AS DateTime), CAST(N'2015-12-05T21:07:13.000' AS DateTime), CAST(N'2015-12-08T17:17:13.000' AS DateTime), NULL, CAST(N'2015-12-08T17:17:16.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (138, N'00.91.0038', CAST(N'2015-12-02T13:47:02.000' AS DateTime), 30, CAST(N'2015-12-05T21:08:14.000' AS DateTime), CAST(N'2015-12-05T22:15:44.000' AS DateTime), CAST(N'2015-12-05T22:15:56.000' AS DateTime), NULL, CAST(N'2019-05-04T13:16:40.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (139, N'00.91.0039', CAST(N'2015-12-05T21:08:20.000' AS DateTime), 30, CAST(N'2019-05-04T13:14:18.000' AS DateTime), CAST(N'2019-05-04T13:14:31.000' AS DateTime), CAST(N'2019-05-04T13:14:34.000' AS DateTime), NULL, CAST(N'2019-05-04T13:14:37.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (140, N'3.01.0040', CAST(N'2015-12-08T17:17:22.000' AS DateTime), 29, CAST(N'2015-12-08T17:17:36.000' AS DateTime), CAST(N'2015-12-08T17:17:39.000' AS DateTime), NULL, NULL, CAST(N'2015-12-08T17:17:42.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (141, N'3.01.0041', CAST(N'2016-01-11T18:01:34.000' AS DateTime), 29, CAST(N'2016-01-11T18:01:48.000' AS DateTime), CAST(N'2016-01-11T18:01:50.000' AS DateTime), NULL, NULL, CAST(N'2016-01-11T18:02:25.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (142, N'3.01.0042', CAST(N'2016-01-11T18:03:11.000' AS DateTime), 29, NULL, NULL, NULL, NULL, CAST(N'2016-01-11T18:03:26.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (143, N'1.03.0045', CAST(N'2016-01-11T18:04:37.000' AS DateTime), 28, CAST(N'2016-01-11T18:05:00.000' AS DateTime), CAST(N'2016-01-11T18:05:01.000' AS DateTime), NULL, NULL, CAST(N'2016-01-11T18:05:14.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (144, N'1.03.0046', CAST(N'2016-01-11T18:05:30.000' AS DateTime), 28, NULL, NULL, NULL, NULL, CAST(N'2016-01-11T18:06:27.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (145, N'00.92.0050', CAST(N'2019-05-04T13:18:13.000' AS DateTime), 30, CAST(N'2019-05-04T13:18:31.000' AS DateTime), NULL, CAST(N'2019-05-07T13:50:52.000' AS DateTime), NULL, CAST(N'2019-05-07T13:51:00.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (146, N'00.92.0051', CAST(N'2019-05-04T14:51:08.000' AS DateTime), 30, CAST(N'2019-05-07T13:51:13.000' AS DateTime), NULL, CAST(N'2019-05-15T12:00:37.000' AS DateTime), NULL, CAST(N'2019-05-15T12:00:38.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (147, N'00.92.0052', CAST(N'2019-05-07T13:51:26.000' AS DateTime), 30, CAST(N'2019-05-15T12:00:24.000' AS DateTime), NULL, CAST(N'2019-05-21T12:18:16.000' AS DateTime), NULL, CAST(N'2019-05-21T12:18:20.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (148, N'00.92.0053', CAST(N'2019-05-15T12:00:04.000' AS DateTime), 30, CAST(N'2019-05-21T12:18:50.000' AS DateTime), NULL, CAST(N'2019-05-22T14:30:35.000' AS DateTime), NULL, CAST(N'2019-05-22T14:30:37.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (149, N'00.92.0054', CAST(N'2019-05-21T12:18:25.000' AS DateTime), 30, CAST(N'2019-05-22T14:30:27.000' AS DateTime), NULL, CAST(N'2019-08-16T15:37:53.000' AS DateTime), NULL, CAST(N'2019-08-16T15:37:57.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (151, N'00.92.0055', CAST(N'2019-05-22T14:30:11.000' AS DateTime), 30, CAST(N'2019-08-16T15:38:10.000' AS DateTime), NULL, CAST(N'2019-08-16T15:38:14.000' AS DateTime), NULL, CAST(N'2019-08-16T15:38:17.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (152, N'00.93.0056', CAST(N'2019-08-16T15:38:29.000' AS DateTime), 30, CAST(N'2019-08-16T15:38:57.000' AS DateTime), NULL, NULL, NULL, CAST(N'2019-08-16T15:39:04.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (153, N'00.93.0057', CAST(N'2019-08-16T15:39:26.000' AS DateTime), 30, CAST(N'2019-08-17T12:16:08.000' AS DateTime), NULL, CAST(N'2019-08-17T12:16:12.000' AS DateTime), NULL, CAST(N'2019-08-17T12:16:15.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (154, N'00.94.0058', CAST(N'2019-08-17T12:16:17.000' AS DateTime), 30, CAST(N'2019-08-17T12:16:41.000' AS DateTime), NULL, NULL, N'First Visual Studio 2019 version.', CAST(N'2019-08-17T12:17:43.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[TB_Versions] ([intVersionID], [strVersion], [dteCreated], [intProductID], [dteRelToQA], [dteRelToCust], [dteClosed], [txtNotes], [dteModifiedDate], [strModifiedBy]) VALUES (155, N'00.94.0059', CAST(N'2019-08-17T12:16:49.000' AS DateTime), 30, NULL, NULL, NULL, NULL, CAST(N'2019-08-17T12:16:59.000' AS DateTime), N'Peter Ringering')
GO
INSERT [dbo].[VERSYS] ([strVersion]) VALUES (N'000051')
GO
ALTER TABLE [dbo].[StockCostQuantity]  WITH CHECK ADD  CONSTRAINT [FK_StockCostQuantity_StockMaster] FOREIGN KEY([StockNumber], [Location])
REFERENCES [dbo].[StockMaster] ([StockNumber], [Location])
GO
ALTER TABLE [dbo].[StockCostQuantity] CHECK CONSTRAINT [FK_StockCostQuantity_StockMaster]
GO
USE [master]
GO
ALTER DATABASE [DevLogix] SET  READ_WRITE 
GO

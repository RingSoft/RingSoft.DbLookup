USE [master]
GO

if exists (select * from sysdatabases where name='MegaDb')
		drop database [MegaDb]
GO

DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1

EXECUTE (N'CREATE DATABASE [MegaDb]
  ON PRIMARY (NAME = N''MegaDb'', FILENAME = N''' + @device_directory + N'megadb.mdf'')
  LOG ON (NAME = N''MegaDb_log'',  FILENAME = N''' + @device_directory + N'megadb.ldf'')')
GO

ALTER DATABASE [MegaDb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MegaDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MegaDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MegaDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MegaDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MegaDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MegaDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [MegaDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MegaDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MegaDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MegaDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MegaDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MegaDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MegaDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MegaDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MegaDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MegaDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MegaDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MegaDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MegaDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MegaDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MegaDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MegaDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MegaDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MegaDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MegaDb] SET  MULTI_USER 
GO
ALTER DATABASE [MegaDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MegaDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MegaDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MegaDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MegaDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MegaDb] SET QUERY_STORE = OFF
GO
USE [MegaDb]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 2/12/2020 1:45:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[LocationID] [int] NOT NULL,
	[ManufacturerID] [int] NOT NULL,
	[IconType] [tinyint] NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 2/12/2020 1:45:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufacturers]    Script Date: 2/12/2020 1:45:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufacturers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Manufacturers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockCostQuantity]    Script Date: 2/12/2020 1:45:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockCostQuantity](
	[StockNumber] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[PurchasedDateTime] [datetime] NOT NULL,
	[Quantity] [decimal](18, 4) NOT NULL,
	[Cost] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_StockCostQuantity] PRIMARY KEY CLUSTERED 
(
	[StockNumber] ASC,
	[Location] ASC,
	[PurchasedDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockMaster]    Script Date: 2/12/2020 1:45:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockMaster](
	[StockNumber] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[Price] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_StockMaster] PRIMARY KEY CLUSTERED 
(
	[StockNumber] ASC,
	[Location] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[AdvancedFinds](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Table] [nvarchar](50) NOT NULL,
	[FromFormula] [ntext] NULL,
	[RefreshRate] [tinyint] NULL,
	[RefreshValue] [integer] NULL,
	[RefreshCondition] [tinyint] NULL,
	[YellowAlert] [integer] NULL,
	[RedAlert] [integer] NULL,
	[Disabled] [bit] NULL

 CONSTRAINT [PK_AdvancedFind] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[AdvancedFindColumns](
  [AdvancedFindId] [int] NOT NULL,
  [ColumnId] [int] NOT NULL,
  [TableName] [nvarchar](50) NOT NULL,
  [FieldName] [nvarchar](50) NULL,
  [PrimaryTableName] [nvarchar](50) NULL,
  [PrimaryFieldName] [nvarchar](50) NULL,
  [Caption] [nvarchar](50) NOT NULL,
  [PercentWidth] [decimal](18, 4) NOT NULL,
  [Formula] [ntext] NULL,
  [FieldDataType] [tinyint] NULL,
  [DecimalFormatType] [tinyint] NULL

 CONSTRAINT [PK_AdvancedFindColumn] PRIMARY KEY CLUSTERED 
(
	[AdvancedFindId] ASC,
	[ColumnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[AdvancedFindFilters](
  [AdvancedFindId] [int] NOT NULL,
  [FilterId] [int] NOT NULL,
  [LeftParentheses] [tinyint] NULL,
  [TableName] [nvarchar](50) NULL,
  [FieldName] [nvarchar](50) NULL,
  [PrimaryTableName] [nvarchar](50) NULL,
  [PrimaryFieldName] [nvarchar](50) NULL,
  [Operand] [tinyint] NOT NULL,
  [SearchForValue] [nvarchar](50) NULL,
  [Formula] [ntext] NULL,
  [FormulaDataType] [tinyint] NULL,
  [FormulaDisplayValue] [nvarchar](50) NULL,
  [SearchForAdvancedFindId] [int] NULL,
  [CustomDate] [bit] NULL,
  [RightParentheses] [tinyint] NULL,
  [EndLogic] [tinyint] NULL
 CONSTRAINT [PK_AdvancedFindFilter] PRIMARY KEY CLUSTERED 
(
	[AdvancedFindId] ASC,
	[FilterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[RecordLocks](
  [Table] [nvarchar](50) NOT NULL,
  [PrimaryKey] [nvarchar](50) NOT NULL,
  [LockDateTime] [datetime] NOT NULL,
  [User] [nvarchar](50) NULL
CONSTRAINT [PK_RecordLock] PRIMARY KEY CLUSTERED 
(
	[Table] ASC,
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Locations] ON 
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (1, N'Aisle 1')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (2, N'Aisle 2')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (3, N'Aisle 3')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (4, N'Aisle 4')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (5, N'Aisle 5')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (10, N'Bakery')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (7, N'Dairy')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (9, N'Deli')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (8, N'Meat')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (6, N'Produce')
GO
INSERT [dbo].[Locations] ([Id], [Name]) VALUES (11, N'Seafood')
GO
SET IDENTITY_INSERT [dbo].[Locations] OFF
GO
SET IDENTITY_INSERT [dbo].[Manufacturers] ON 
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (8, N'Albertsons')
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (4, N'Amazon')
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (3, N'Generic')
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (6, N'Great Value')
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (9, N'Homestyle')
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (1, N'Kraft')
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (7, N'Kroger')
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (5, N'Sam''s Choice')
GO
INSERT [dbo].[Manufacturers] ([Id], [Name]) VALUES (2, N'Western Family')
GO
SET IDENTITY_INSERT [dbo].[Manufacturers] OFF
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Boise, ID', CAST(N'2016-01-12T00:00:00.000' AS DateTime), CAST(1.0000 AS Decimal(18, 4)), CAST(23.5800 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Boise, ID', CAST(N'2016-02-12T00:00:00.000' AS DateTime), CAST(3.0000 AS Decimal(18, 4)), CAST(29.3600 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Boise, ID', CAST(N'2016-03-14T00:00:00.000' AS DateTime), CAST(5.0000 AS Decimal(18, 4)), CAST(28.5400 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Portland, OR', CAST(N'2016-01-12T00:00:00.000' AS DateTime), CAST(3.0000 AS Decimal(18, 4)), CAST(55.3600 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Portland, OR', CAST(N'2016-02-01T00:00:00.000' AS DateTime), CAST(5.0000 AS Decimal(18, 4)), CAST(54.3600 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Portland, OR', CAST(N'2016-02-12T00:00:00.000' AS DateTime), CAST(4.0000 AS Decimal(18, 4)), CAST(51.2500 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Salt Lake City, UT', CAST(N'2016-03-14T00:00:00.000' AS DateTime), CAST(5.0000 AS Decimal(18, 4)), CAST(32.3200 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Salt Lake City, UT', CAST(N'2016-04-01T00:00:00.000' AS DateTime), CAST(5.0000 AS Decimal(18, 4)), CAST(36.1200 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Salt Lake City, UT', CAST(N'2016-05-12T00:00:00.000' AS DateTime), CAST(4.0000 AS Decimal(18, 4)), CAST(33.1200 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Seattle, WA', CAST(N'2016-02-12T00:00:00.000' AS DateTime), CAST(6.0000 AS Decimal(18, 4)), CAST(61.3500 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Seattle, WA', CAST(N'2016-03-01T00:00:00.000' AS DateTime), CAST(8.0000 AS Decimal(18, 4)), CAST(59.3300 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Chair #1 Swivel', N'Seattle, WA', CAST(N'2016-04-15T00:00:00.000' AS DateTime), CAST(5.0000 AS Decimal(18, 4)), CAST(58.6400 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Boise, ID', CAST(N'2016-01-16T00:00:00.000' AS DateTime), CAST(3.0000 AS Decimal(18, 4)), CAST(125.3600 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Boise, ID', CAST(N'2016-02-21T00:00:00.000' AS DateTime), CAST(3.0000 AS Decimal(18, 4)), CAST(128.9800 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Boise, ID', CAST(N'2016-03-25T00:00:00.000' AS DateTime), CAST(4.0000 AS Decimal(18, 4)), CAST(123.6500 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Portland, OR', CAST(N'2016-01-05T00:00:00.000' AS DateTime), CAST(3.0000 AS Decimal(18, 4)), CAST(135.6500 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Portland, OR', CAST(N'2016-01-30T00:00:00.000' AS DateTime), CAST(5.0000 AS Decimal(18, 4)), CAST(141.3200 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Portland, OR', CAST(N'2016-03-01T00:00:00.000' AS DateTime), CAST(4.0000 AS Decimal(18, 4)), CAST(138.7800 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Seattle, WA', CAST(N'2016-01-05T00:00:00.000' AS DateTime), CAST(12.0000 AS Decimal(18, 4)), CAST(178.2100 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Seattle, WA', CAST(N'2016-02-03T00:00:00.000' AS DateTime), CAST(4.0000 AS Decimal(18, 4)), CAST(169.9800 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockCostQuantity] ([StockNumber], [Location], [PurchasedDateTime], [Quantity], [Cost]) VALUES (N'Desk 30 X 48', N'Seattle, WA', CAST(N'2016-04-03T00:00:00.000' AS DateTime), CAST(2.0000 AS Decimal(18, 4)), CAST(171.2100 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'Chair #1 Swivel', N'Boise, ID', CAST(55.3500 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'Chair #1 Swivel', N'Portland, OR', CAST(78.3600 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'Chair #1 Swivel', N'Salt Lake City, UT', CAST(65.0100 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'Chair #1 Swivel', N'Seattle, WA', CAST(89.3200 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'Desk 30 X 48', N'Boise, ID', CAST(156.3200 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'Desk 30 X 48', N'Portland, OR', CAST(178.3900 AS Decimal(18, 4)))
GO
INSERT [dbo].[StockMaster] ([StockNumber], [Location], [Price]) VALUES (N'Desk 30 X 48', N'Seattle, WA', CAST(201.5500 AS Decimal(18, 4)))
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Items]    Script Date: 2/12/2020 1:45:17 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Items] ON [dbo].[Items]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Items_Location]    Script Date: 2/12/2020 1:45:17 PM ******/
CREATE NONCLUSTERED INDEX [IX_Items_Location] ON [dbo].[Items]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Items_Manufacturer]    Script Date: 2/12/2020 1:45:17 PM ******/
CREATE NONCLUSTERED INDEX [IX_Items_Manufacturer] ON [dbo].[Items]
(
	[ManufacturerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Locations]    Script Date: 2/12/2020 1:45:17 PM ******/
CREATE NONCLUSTERED INDEX [IX_Locations] ON [dbo].[Locations]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Manufacturers]    Script Date: 2/12/2020 1:45:17 PM ******/
CREATE NONCLUSTERED INDEX [IX_Manufacturers] ON [dbo].[Manufacturers]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Locations] FOREIGN KEY([LocationID])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_Locations]
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Manufacturers] FOREIGN KEY([ManufacturerID])
REFERENCES [dbo].[Manufacturers] ([Id])
GO
ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_Manufacturers]
GO
ALTER TABLE [dbo].[StockCostQuantity]  WITH CHECK ADD  CONSTRAINT [FK_StockCostQuantity_StockMaster] FOREIGN KEY([StockNumber], [Location])
REFERENCES [dbo].[StockMaster] ([StockNumber], [Location])
GO
ALTER TABLE [dbo].[StockCostQuantity] CHECK CONSTRAINT [FK_StockCostQuantity_StockMaster]
GO
USE [master]
GO
ALTER DATABASE [MegaDb] SET  READ_WRITE 
GO

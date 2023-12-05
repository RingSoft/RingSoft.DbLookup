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
/****** Object:  Table [dbo].[MliLocationsTable]    Script Date: 11/30/2023 10:16:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Table [dbo].[MliLocationsTable]    Script Date: 12/1/2023 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MliLocationsTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MliLocationsTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[StockMaster]    Script Date: 12/1/2023 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StockId] [int] NOT NULL,
	[MliLocationId] [int] NOT NULL,
	[Price] [decimal](38, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StocksTable]    Script Date: 12/1/2023 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StocksTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_StocksTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockCostQuantity]    Script Date: 12/3/2023 4:08:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockCostQuantity](
	[StockMasterId] [int] NOT NULL,
	[PurchasedDateTime] [datetime] NOT NULL,
	[Quantity] [decimal](38, 2) NOT NULL,
	[Cost] [decimal](38, 2) NOT NULL,
 CONSTRAINT [PK_StockCostQuantity] PRIMARY KEY CLUSTERED 
(
	[StockMasterId] ASC,
	[PurchasedDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
  [TableName] [nvarchar](50) NULL,
  [FieldName] [nvarchar](50) NULL,
  [PrimaryTableName] [nvarchar](50) NULL,
  [PrimaryFieldName] [nvarchar](50) NULL,
  [Path] [nvarchar](1000) NOT NULL,
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
  [Path] [nvarchar](1000) NOT NULL,
  [Operand] [tinyint] NOT NULL,
  [SearchForValue] [nvarchar](50) NULL,
  [Formula] [ntext] NULL,
  [FormulaDataType] [tinyint] NULL,
  [FormulaDisplayValue] [nvarchar](50) NULL,
  [SearchForAdvancedFindId] [int] NULL,
  [CustomDate] [bit] NULL,
  [RightParentheses] [tinyint] NULL,
  [EndLogic] [tinyint] NULL,
  [DateFilterType] [tinyint] NULL
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

SET IDENTITY_INSERT [dbo].[Manufacturers] OFF 
GO
SET IDENTITY_INSERT [dbo].[MliLocationsTable] ON 
GO
INSERT [dbo].[MliLocationsTable] ([Id], [Name]) VALUES (1, N'Seattle, WA')
GO
INSERT [dbo].[MliLocationsTable] ([Id], [Name]) VALUES (2, N'Portland, OR')
GO
INSERT [dbo].[MliLocationsTable] ([Id], [Name]) VALUES (3, N'Boise, ID')
GO
INSERT [dbo].[MliLocationsTable] ([Id], [Name]) VALUES (4, N'Spokane, WA')
GO
SET IDENTITY_INSERT [dbo].[MliLocationsTable] OFF
GO
SET IDENTITY_INSERT [dbo].[StocksTable] ON 
GO
INSERT [dbo].[StocksTable] ([Id], [Name]) VALUES (1, N'Chair #1 Swivel')
GO
INSERT [dbo].[StocksTable] ([Id], [Name]) VALUES (2, N'Desk 30 X 48')
GO
SET IDENTITY_INSERT [dbo].[StocksTable] OFF
GO
SET ANSI_PADDING ON
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (1, CAST(N'2023-11-21T00:00:00.000' AS DateTime), CAST(3.00 AS Decimal(38, 2)), CAST(10.32 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (1, CAST(N'2023-11-28T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(38, 2)), CAST(11.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (1, CAST(N'2023-12-04T00:00:00.000' AS DateTime), CAST(6.00 AS Decimal(38, 2)), CAST(14.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (2, CAST(N'2023-11-28T00:00:00.000' AS DateTime), CAST(3.00 AS Decimal(38, 2)), CAST(10.21 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (2, CAST(N'2023-11-29T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(38, 2)), CAST(9.65 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (2, CAST(N'2023-12-01T00:00:00.000' AS DateTime), CAST(3.00 AS Decimal(38, 2)), CAST(11.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (3, CAST(N'2023-11-14T00:00:00.000' AS DateTime), CAST(3.00 AS Decimal(38, 2)), CAST(10.01 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (3, CAST(N'2023-11-15T00:00:00.000' AS DateTime), CAST(2.00 AS Decimal(38, 2)), CAST(9.25 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (3, CAST(N'2023-11-21T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(38, 2)), CAST(8.99 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (4, CAST(N'2023-12-05T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(38, 2)), CAST(10.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (4, CAST(N'2023-12-06T00:00:00.000' AS DateTime), CAST(3.00 AS Decimal(38, 2)), CAST(9.25 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (4, CAST(N'2023-12-07T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(38, 2)), CAST(11.21 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (5, CAST(N'2023-11-27T00:00:00.000' AS DateTime), CAST(3.00 AS Decimal(38, 2)), CAST(19.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (5, CAST(N'2023-11-28T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(38, 2)), CAST(19.32 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (5, CAST(N'2023-12-01T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(38, 2)), CAST(20.01 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (6, CAST(N'2023-11-21T00:00:00.000' AS DateTime), CAST(2.00 AS Decimal(38, 2)), CAST(20.32 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (6, CAST(N'2023-11-22T00:00:00.000' AS DateTime), CAST(5.00 AS Decimal(38, 2)), CAST(19.65 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (6, CAST(N'2023-11-30T00:00:00.000' AS DateTime), CAST(2.00 AS Decimal(38, 2)), CAST(18.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (7, CAST(N'2023-12-12T00:00:00.000' AS DateTime), CAST(2.00 AS Decimal(38, 2)), CAST(21.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (7, CAST(N'2023-12-14T00:00:00.000' AS DateTime), CAST(4.00 AS Decimal(38, 2)), CAST(21.35 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (7, CAST(N'2023-12-21T00:00:00.000' AS DateTime), CAST(5.00 AS Decimal(38, 2)), CAST(20.95 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (8, CAST(N'2023-12-03T00:00:00.000' AS DateTime), CAST(2.00 AS Decimal(38, 2)), CAST(12.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockCostQuantity] ([StockMasterId], [PurchasedDateTime], [Quantity], [Cost]) VALUES (8, CAST(N'2023-12-04T00:00:00.000' AS DateTime), CAST(3.00 AS Decimal(38, 2)), CAST(11.99 AS Decimal(38, 2)))
GO
SET IDENTITY_INSERT [dbo].[StockMaster] ON 
GO
INSERT [dbo].[StockMaster] ([Id], [StockId], [MliLocationId], [Price]) VALUES (1, 1, 2, CAST(11.75 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockMaster] ([Id], [StockId], [MliLocationId], [Price]) VALUES (2, 1, 3, CAST(12.35 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockMaster] ([Id], [StockId], [MliLocationId], [Price]) VALUES (3, 1, 1, CAST(13.32 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockMaster] ([Id], [StockId], [MliLocationId], [Price]) VALUES (4, 1, 4, CAST(11.36 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockMaster] ([Id], [StockId], [MliLocationId], [Price]) VALUES (5, 2, 2, CAST(21.65 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockMaster] ([Id], [StockId], [MliLocationId], [Price]) VALUES (6, 2, 3, CAST(22.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockMaster] ([Id], [StockId], [MliLocationId], [Price]) VALUES (7, 2, 4, CAST(24.00 AS Decimal(38, 2)))
GO
INSERT [dbo].[StockMaster] ([Id], [StockId], [MliLocationId], [Price]) VALUES (8, 2, 1, CAST(25.32 AS Decimal(38, 2)))
GO
SET IDENTITY_INSERT [dbo].[StockMaster] OFF
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
ALTER TABLE [dbo].[StockMaster]  WITH CHECK ADD  CONSTRAINT [FK_StockMaster_MliLocationsTable] FOREIGN KEY([MliLocationId])
REFERENCES [dbo].[MliLocationsTable] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockMaster] CHECK CONSTRAINT [FK_StockMaster_MliLocationsTable]
GO
ALTER TABLE [dbo].[StockMaster]  WITH CHECK ADD  CONSTRAINT [FK_StockMaster_StocksTable] FOREIGN KEY([StockId])
REFERENCES [dbo].[StocksTable] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockMaster] CHECK CONSTRAINT [FK_StockMaster_StocksTable]
GO
ALTER TABLE [dbo].[StockCostQuantity]  WITH CHECK ADD  CONSTRAINT [FK_StockCostQuantity_StockMaster] FOREIGN KEY([StockMasterId])
REFERENCES [dbo].[StockMaster] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockCostQuantity] CHECK CONSTRAINT [FK_StockCostQuantity_StockMaster]
GO
USE [master]
GO
ALTER DATABASE [MegaDb] SET  READ_WRITE 
GO

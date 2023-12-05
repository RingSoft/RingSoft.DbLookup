CREATE DATABASE  IF NOT EXISTS `megadb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `megadb`;
-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: localhost    Database: megadb
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `advancedfindcolumns`
--

DROP TABLE IF EXISTS `advancedfindcolumns`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `advancedfindcolumns` (
  `AdvancedFindId` int NOT NULL,
  `ColumnId` int NOT NULL,
  `TableName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FieldName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PrimaryTableName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PrimaryFieldName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Path` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Caption` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PercentWidth` double(18,4) NOT NULL,
  `Formula` longtext,
  `FieldDataType` tinyint unsigned DEFAULT NULL,
  `DecimalFormatType` tinyint unsigned DEFAULT NULL,
  PRIMARY KEY (`AdvancedFindId`,`ColumnId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advancedfindcolumns`
--

LOCK TABLES `advancedfindcolumns` WRITE;
/*!40000 ALTER TABLE `advancedfindcolumns` DISABLE KEYS */;
/*!40000 ALTER TABLE `advancedfindcolumns` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `advancedfindfilters`
--

DROP TABLE IF EXISTS `advancedfindfilters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `advancedfindfilters` (
  `AdvancedFindId` int NOT NULL,
  `FilterId` int NOT NULL,
  `LeftParentheses` tinyint unsigned DEFAULT NULL,
  `TableName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FieldName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PrimaryTableName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PrimaryFieldName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Path` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Operand` tinyint unsigned NOT NULL,
  `SearchForValue` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Formula` longtext,
  `FormulaDataType` tinyint unsigned DEFAULT NULL,
  `FormulaDisplayValue` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SearchForAdvancedFindId` int DEFAULT NULL,
  `CustomDate` tinyint(1) DEFAULT NULL,
  `RightParentheses` tinyint unsigned DEFAULT NULL,
  `EndLogic` tinyint unsigned DEFAULT NULL,
  PRIMARY KEY (`AdvancedFindId`,`FilterId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advancedfindfilters`
--

LOCK TABLES `advancedfindfilters` WRITE;
/*!40000 ALTER TABLE `advancedfindfilters` DISABLE KEYS */;
/*!40000 ALTER TABLE `advancedfindfilters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `advancedfinds`
--

DROP TABLE IF EXISTS `advancedfinds`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `advancedfinds` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Table` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FromFormula` longtext,
  `RefreshRate` tinyint unsigned DEFAULT NULL,
  `RefreshValue` int DEFAULT NULL,
  `RefreshCondition` tinyint unsigned DEFAULT NULL,
  `YellowAlert` int DEFAULT NULL,
  `RedAlert` int DEFAULT NULL,
  `Disabled` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advancedfinds`
--

LOCK TABLES `advancedfinds` WRITE;
/*!40000 ALTER TABLE `advancedfinds` DISABLE KEYS */;
/*!40000 ALTER TABLE `advancedfinds` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `items`
--

DROP TABLE IF EXISTS `items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `items` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LocationID` int NOT NULL,
  `ManufacturerID` int NOT NULL,
  `IconType` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Items` (`Name`),
  KEY `IX_Items_Location` (`LocationID`),
  KEY `IX_Items_Manufacturer` (`ManufacturerID`),
  CONSTRAINT `FK_Items_Locations` FOREIGN KEY (`LocationID`) REFERENCES `locations` (`Id`),
  CONSTRAINT `FK_Items_Manufacturers` FOREIGN KEY (`ManufacturerID`) REFERENCES `manufacturers` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `items`
--

LOCK TABLES `items` WRITE;
/*!40000 ALTER TABLE `items` DISABLE KEYS */;
/*!40000 ALTER TABLE `items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `locations`
--

DROP TABLE IF EXISTS `locations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `locations` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Locations` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `locations`
--

LOCK TABLES `locations` WRITE;
/*!40000 ALTER TABLE `locations` DISABLE KEYS */;
INSERT INTO `locations` VALUES (1,'Aisle 1'),(2,'Aisle 2'),(3,'Aisle 3'),(4,'Aisle 4'),(5,'Aisle 5'),(10,'Bakery'),(7,'Dairy'),(9,'Deli'),(8,'Meat'),(6,'Produce'),(11,'Seafood');
/*!40000 ALTER TABLE `locations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufacturers`
--

DROP TABLE IF EXISTS `manufacturers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufacturers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Manufacturers` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufacturers`
--

DROP TABLE IF EXISTS `recordlocks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recordlocks` (
  `Table` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrimaryKey` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LockDateTime` datetime(6) NOT NULL,
  `User` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Table`,`PrimaryKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recordlocks`
--

LOCK TABLES `recordlocks` WRITE;
/*!40000 ALTER TABLE `recordlocks` DISABLE KEYS */;
/*!40000 ALTER TABLE `recordlocks` ENABLE KEYS */;
UNLOCK TABLES;

LOCK TABLES `manufacturers` WRITE;
/*!40000 ALTER TABLE `manufacturers` DISABLE KEYS */;
INSERT INTO `manufacturers` VALUES (8,'Albertsons'),(4,'Amazon'),(3,'Generic'),(6,'Great Value'),(9,'Homestyle'),(1,'Kraft'),(7,'Kroger'),(5,'Sam\'s Choice'),(2,'Western Family');
/*!40000 ALTER TABLE `manufacturers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mlilocationstable`
--

DROP TABLE IF EXISTS `mlilocationstable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mlilocationstable` (
  `Id` int NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mlilocationstable`
--

LOCK TABLES `mlilocationstable` WRITE;
/*!40000 ALTER TABLE `mlilocationstable` DISABLE KEYS */;
INSERT INTO `mlilocationstable` VALUES (1,'Seattle, WA'),(2,'Portland, OR'),(3,'Boise, ID'),(4,'Spokane, WA');
/*!40000 ALTER TABLE `mlilocationstable` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stockcostquantity`
--

DROP TABLE IF EXISTS `stockcostquantity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stockcostquantity` (
  `StockMasterId` int NOT NULL,
  `PurchasedDateTime` datetime(6) NOT NULL,
  `Quantity` decimal(38,2) NOT NULL,
  `Cost` decimal(38,2) NOT NULL,
  PRIMARY KEY (`StockMasterId`,`PurchasedDateTime`),
  CONSTRAINT `FK_StockCostQuantity_StockMaster` FOREIGN KEY (`StockMasterId`) REFERENCES `stockmaster` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stockcostquantity`
--

LOCK TABLES `stockcostquantity` WRITE;
/*!40000 ALTER TABLE `stockcostquantity` DISABLE KEYS */;
INSERT INTO `stockcostquantity` VALUES (1,'2023-11-21 00:00:00.000000',3.00,10.32),(1,'2023-11-28 00:00:00.000000',4.00,11.00),(1,'2023-12-04 00:00:00.000000',6.00,14.00),(2,'2023-11-28 00:00:00.000000',3.00,10.21),(2,'2023-11-29 00:00:00.000000',4.00,9.65),(2,'2023-12-01 00:00:00.000000',3.00,11.00),(3,'2023-11-14 00:00:00.000000',3.00,10.01),(3,'2023-11-15 00:00:00.000000',2.00,9.25),(3,'2023-11-21 00:00:00.000000',4.00,8.99),(4,'2023-12-05 00:00:00.000000',4.00,10.00),(4,'2023-12-06 00:00:00.000000',3.00,9.25),(4,'2023-12-07 00:00:00.000000',4.00,11.21),(5,'2023-11-27 00:00:00.000000',3.00,19.00),(5,'2023-11-28 00:00:00.000000',4.00,19.32),(5,'2023-12-01 00:00:00.000000',4.00,20.01),(6,'2023-11-21 00:00:00.000000',2.00,20.32),(6,'2023-11-22 00:00:00.000000',5.00,19.65),(6,'2023-11-30 00:00:00.000000',2.00,18.00),(7,'2023-12-12 00:00:00.000000',2.00,21.00),(7,'2023-12-14 00:00:00.000000',4.00,21.35),(7,'2023-12-21 00:00:00.000000',5.00,20.95),(8,'2023-12-03 00:00:00.000000',2.00,12.00),(8,'2023-12-04 00:00:00.000000',3.00,11.99);
/*!40000 ALTER TABLE `stockcostquantity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stockmaster`
--

DROP TABLE IF EXISTS `stockmaster`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stockmaster` (
  `Id` int NOT NULL,
  `StockId` int NOT NULL,
  `MliLocationId` int NOT NULL,
  `Price` decimal(38,2) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_StockMaster_MliLocationsTable` (`MliLocationId`),
  KEY `FK_StockMaster_StocksTable` (`StockId`),
  CONSTRAINT `FK_StockMaster_MliLocationsTable` FOREIGN KEY (`MliLocationId`) REFERENCES `mlilocationstable` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_StockMaster_StocksTable` FOREIGN KEY (`StockId`) REFERENCES `stockstable` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stockmaster`
--

LOCK TABLES `stockmaster` WRITE;
/*!40000 ALTER TABLE `stockmaster` DISABLE KEYS */;
INSERT INTO `stockmaster` VALUES (1,1,2,11.75),(2,1,3,12.35),(3,1,1,13.32),(4,1,4,11.36),(5,2,2,21.65),(6,2,3,22.00),(7,2,4,24.00),(8,2,1,25.32);
/*!40000 ALTER TABLE `stockmaster` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stockstable`
--

DROP TABLE IF EXISTS `stockstable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stockstable` (
  `Id` int NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stockstable`
--

LOCK TABLES `stockstable` WRITE;
/*!40000 ALTER TABLE `stockstable` DISABLE KEYS */;
INSERT INTO `stockstable` VALUES (1,'Chair #1 Swivel'),(2,'Desk 30 X 48');
/*!40000 ALTER TABLE `stockstable` ENABLE KEYS */;
UNLOCK TABLES;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-09-10 17:16:52

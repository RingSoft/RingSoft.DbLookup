CREATE DATABASE  IF NOT EXISTS `megadb` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `megadb`;
-- MySQL dump 10.13  Distrib 8.0.18, for Win64 (x86_64)
--
-- Host: localhost    Database: megadb
-- ------------------------------------------------------
-- Server version	5.7.23-log

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
-- Table structure for table `items`
--

DROP TABLE IF EXISTS `items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `items` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  `LocationID` int(11) NOT NULL,
  `ManufacturerID` int(11) NOT NULL,
  `IconType` tinyint(3) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Items` (`Name`),
  KEY `IX_Items_Location` (`LocationID`),
  KEY `IX_Items_Manufacturer` (`ManufacturerID`),
  CONSTRAINT `FK_Items_Locations` FOREIGN KEY (`LocationID`) REFERENCES `locations` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_Items_Manufacturers` FOREIGN KEY (`ManufacturerID`) REFERENCES `manufacturers` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
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
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Locations` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;
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
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Manufacturers` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufacturers`
--

LOCK TABLES `manufacturers` WRITE;
/*!40000 ALTER TABLE `manufacturers` DISABLE KEYS */;
INSERT INTO `manufacturers` VALUES (8,'Albertsons'),(4,'Amazon'),(3,'Generic'),(6,'Great Value'),(9,'Homestyle'),(1,'Kraft'),(7,'Kroger'),(5,'Sam\'s Choice'),(2,'Western Family');
/*!40000 ALTER TABLE `manufacturers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stockcostquantity`
--

DROP TABLE IF EXISTS `stockcostquantity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stockcostquantity` (
  `StockNumber` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  `Location` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  `PurchasedDateTime` datetime(6) NOT NULL,
  `Quantity` decimal(18,4) NOT NULL,
  `Cost` decimal(18,4) NOT NULL,
  PRIMARY KEY (`StockNumber`,`Location`,`PurchasedDateTime`),
  CONSTRAINT `FK_StockCostQuantity_StockMaster` FOREIGN KEY (`StockNumber`, `Location`) REFERENCES `stockmaster` (`StockNumber`, `Location`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stockcostquantity`
--

LOCK TABLES `stockcostquantity` WRITE;
/*!40000 ALTER TABLE `stockcostquantity` DISABLE KEYS */;
INSERT INTO `stockcostquantity` VALUES ('Chair #1 Swivel','Boise, ID','2016-01-12 00:00:00.000000',1.0000,23.5800),('Chair #1 Swivel','Boise, ID','2016-02-12 00:00:00.000000',3.0000,29.3600),('Chair #1 Swivel','Boise, ID','2016-03-14 00:00:00.000000',5.0000,28.5400),('Chair #1 Swivel','Portland, OR','2016-01-12 00:00:00.000000',3.0000,55.3600),('Chair #1 Swivel','Portland, OR','2016-02-01 00:00:00.000000',5.0000,54.3600),('Chair #1 Swivel','Portland, OR','2016-02-12 00:00:00.000000',4.0000,51.2500),('Chair #1 Swivel','Salt Lake City, UT','2016-03-14 00:00:00.000000',5.0000,32.3200),('Chair #1 Swivel','Salt Lake City, UT','2016-04-01 00:00:00.000000',5.0000,36.1200),('Chair #1 Swivel','Salt Lake City, UT','2016-05-12 00:00:00.000000',4.0000,33.1200),('Chair #1 Swivel','Seattle, WA','2016-02-12 00:00:00.000000',6.0000,61.3500),('Chair #1 Swivel','Seattle, WA','2016-03-01 00:00:00.000000',8.0000,59.3300),('Chair #1 Swivel','Seattle, WA','2016-04-15 00:00:00.000000',5.0000,58.6400),('Desk 30 X 48','Boise, ID','2016-01-16 00:00:00.000000',3.0000,125.3600),('Desk 30 X 48','Boise, ID','2016-02-21 00:00:00.000000',3.0000,128.9800),('Desk 30 X 48','Boise, ID','2016-03-25 00:00:00.000000',4.0000,123.6500),('Desk 30 X 48','Portland, OR','2016-01-05 00:00:00.000000',3.0000,135.6500),('Desk 30 X 48','Portland, OR','2016-01-30 00:00:00.000000',5.0000,141.3200),('Desk 30 X 48','Portland, OR','2016-03-01 00:00:00.000000',4.0000,138.7800),('Desk 30 X 48','Seattle, WA','2016-01-05 00:00:00.000000',12.0000,178.2100),('Desk 30 X 48','Seattle, WA','2016-02-03 00:00:00.000000',4.0000,169.9800),('Desk 30 X 48','Seattle, WA','2016-04-03 00:00:00.000000',2.0000,171.2100);
/*!40000 ALTER TABLE `stockcostquantity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stockmaster`
--

DROP TABLE IF EXISTS `stockmaster`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stockmaster` (
  `StockNumber` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  `Location` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  `Price` decimal(18,4) NOT NULL,
  PRIMARY KEY (`StockNumber`,`Location`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stockmaster`
--

LOCK TABLES `stockmaster` WRITE;
/*!40000 ALTER TABLE `stockmaster` DISABLE KEYS */;
INSERT INTO `stockmaster` VALUES ('Chair #1 Swivel','Boise, ID',55.3500),('Chair #1 Swivel','Portland, OR',78.3600),('Chair #1 Swivel','Salt Lake City, UT',65.0100),('Chair #1 Swivel','Seattle, WA',89.3200),('Desk 30 X 48','Boise, ID',156.3200),('Desk 30 X 48','Portland, OR',178.3900),('Desk 30 X 48','Seattle, WA',201.5500);
/*!40000 ALTER TABLE `stockmaster` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-13 12:52:19

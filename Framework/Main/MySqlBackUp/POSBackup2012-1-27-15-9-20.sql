-- MySQL dump 10.13  Distrib 5.5.11, for Win32 (x86)
--
-- Host: 192.168.5.76    Database: framework
-- ------------------------------------------------------
-- Server version	5.5.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `backuptablename`
--

DROP TABLE IF EXISTS `backuptablename`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `backuptablename` (
  `TableName` varchar(50) DEFAULT NULL,
  `MasterFile` enum('Y','N') DEFAULT 'N',
  `Transactional` enum('Y','N') DEFAULT 'N',
  `UpdateBackup` enum('Y','N') DEFAULT 'N'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `backuptablename`
--

LOCK TABLES `backuptablename` WRITE;
/*!40000 ALTER TABLE `backuptablename` DISABLE KEYS */;
INSERT INTO `backuptablename` VALUES ('CashierPeriod','N','Y','N'),('VoidOrder','N','Y','N'),('Customer','N','Y','N'),('DailyTimeRecord','N','Y','N'),('DiscountTransaction','N','Y','N'),('OrderDetail','N','Y','N'),('OrderHeader','N','Y','N'),('DeliveredItem','N','Y','N'),('UsedItem','N','Y','N'),('ReturnItem','N','Y','N'),('RefundOrder','N','Y','N'),('OrderDetailAssembledItem','N','Y','N'),('SlicedItem','N','Y','N'),('InventoryDetail','N','Y','N'),('SalesDay','N','Y','N'),('InventoryHeader','N','Y','N'),('Payments','N','Y','N'),('Sequence','N','Y','Y'),('AssembledItem','Y','N','N'),('Discount','Y','N','N'),('EmployeeSchedule','Y','N','N'),('OrderType','Y','N','N'),('Surcharge','Y','N','N'),('TableLocation','Y','N','N'),('Table','Y','N','N'),('UserGroupMenuItems','Y','N','N'),('InventoryGroup','Y','N','N'),('Item','Y','N','N'),('ItemGroup','Y','N','N'),('MenuItem','Y','N','N'),('ModeOfPayment','Y','N','N'),('Shift','Y','N','N'),('SystemConfiguration','Y','N','N'),('Unit','Y','N','N'),('UserGroup','Y','N','N'),('Customer','Y','N','N'),('Sequence','Y','N','N');
/*!40000 ALTER TABLE `backuptablename` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `company`
--

DROP TABLE IF EXISTS `company`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `company` (
  `CompanyCode` varchar(30) NOT NULL,
  `CompanyName` varchar(100) DEFAULT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  PRIMARY KEY (`CompanyCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `company`
--

LOCK TABLES `company` WRITE;
/*!40000 ALTER TABLE `company` DISABLE KEYS */;
INSERT INTO `company` VALUES ('GBSI','Golden Bridge Shipping Inc.','Active'),('OJ','Ocean Fast Ferries Inc.','Active');
/*!40000 ALTER TABLE `company` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customer`
--

DROP TABLE IF EXISTS `customer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `customer` (
  `CustomerId` varchar(30) NOT NULL,
  `Fullname` varchar(100) DEFAULT NULL,
  `Address` varchar(100) DEFAULT NULL,
  `ContactNo` varchar(15) DEFAULT NULL,
  `DiscountCode` varchar(30) DEFAULT NULL,
  `Default` enum('Y','N') DEFAULT 'N',
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  `Backup` enum('Y','N') DEFAULT 'N',
  PRIMARY KEY (`CustomerId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customer`
--

LOCK TABLES `customer` WRITE;
/*!40000 ALTER TABLE `customer` DISABLE KEYS */;
/*!40000 ALTER TABLE `customer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `department`
--

DROP TABLE IF EXISTS `department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `department` (
  `DepartmentCode` varchar(30) NOT NULL,
  `DepartmentDesc` varchar(100) DEFAULT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  PRIMARY KEY (`DepartmentCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `department`
--

LOCK TABLES `department` WRITE;
/*!40000 ALTER TABLE `department` DISABLE KEYS */;
INSERT INTO `department` VALUES ('Accounting','Accounting Department','Active'),('Admin','Administration','Active'),('Audit','Audit Department','Active'),('CS','Customer Service','Active'),('IT','IT Department','Active'),('RD','Reservation Department','Active');
/*!40000 ALTER TABLE `department` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `employee` (
  `EmployeeId` varchar(30) NOT NULL,
  `EmployeeName` varchar(100) NOT NULL,
  `PositionCode` varchar(30) NOT NULL,
  `DepartmentCode` varchar(30) NOT NULL,
  `OutletCode` varchar(30) NOT NULL,
  `CompanyCode` varchar(30) NOT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  PRIMARY KEY (`EmployeeId`),
  KEY `DepartmentCode` (`DepartmentCode`),
  KEY `OutletCode` (`OutletCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
INSERT INTO `employee` VALUES ('E-00001','Jesryl A. Plarisan','IT','IT','SM','OJ','Active'),('E-00002','Prexie Mae Cuesta','Auditor','Audit','Main','OJ','Active'),('E-00003','Sir Boots','Admin','Admin','Main','GBSI','Active'),('E-00004','Yolly','FD','Admin','Main','GBSI','Active'),('E-00005','Customer Service','CS','CS','Pier','OJ','Active'),('E-00006','Rey Cuico','AccHead','Accounting','Pier','OJ','Active'),('E-00007','Berna Calinawan','RS','RD','Pier','OJ','Active'),('E-00008','Vanessa Lua','GenMan','Admin','SM','OJ','Active');
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lookuptable`
--

DROP TABLE IF EXISTS `lookuptable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `lookuptable` (
  `Key` varchar(30) NOT NULL,
  `Value` varchar(200) NOT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  PRIMARY KEY (`Key`,`Value`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lookuptable`
--

LOCK TABLES `lookuptable` WRITE;
/*!40000 ALTER TABLE `lookuptable` DISABLE KEYS */;
/*!40000 ALTER TABLE `lookuptable` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menuitem`
--

DROP TABLE IF EXISTS `menuitem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `menuitem` (
  `MenuName` varchar(50) DEFAULT NULL,
  `MenuText` varchar(50) DEFAULT NULL,
  `ItemName` varchar(50) DEFAULT NULL,
  `ItemText` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menuitem`
--

LOCK TABLES `menuitem` WRITE;
/*!40000 ALTER TABLE `menuitem` DISABLE KEYS */;
INSERT INTO `menuitem` VALUES ('MasterFileMenu','Master Files','tsmPhoneType','Phone Types'),('MasterFileMenu','Master Files','tsmServiceProvider','Service Provider'),('MasterFileMenu','Master Files','tsmCompany','Company'),('MasterFileMenu','Master Files','tsmDepartment','Department'),('MasterFileMenu','Master Files','tsmOutlet','Outlet'),('MasterFileMenu','Master Files','tsmEmployee','Employee'),('SystemMenu','System','tsmExit','Exit'),('TransactionMenu','Transactions','tsmPhoneDirectory','Phone Directory'),('SpecialPriviledge','SpecialPriviledge','CanViewList','CanViewList'),('SystemMenu','System','tsmSystemConfiguration','System Configuration'),('SystemMenu','System','tsmChangeHomeImage','Change Home Image'),('SystemMenu','System','tsmUser','User'),('SystemMenu','System','tsmUserGroup','User Group'),('SystemMenu','System','tsmChangeUserPassword','Change User Password'),('SystemMenu','System','tsmLockScreen','Lock Screen'),('MasterFileMenu','Master Files','tsmPosition','Position');
/*!40000 ALTER TABLE `menuitem` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `outlet`
--

DROP TABLE IF EXISTS `outlet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `outlet` (
  `OutletCode` varchar(30) NOT NULL,
  `OutletDesc` varchar(100) DEFAULT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `TenantName` varchar(100) DEFAULT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  PRIMARY KEY (`OutletCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `outlet`
--

LOCK TABLES `outlet` WRITE;
/*!40000 ALTER TABLE `outlet` DISABLE KEYS */;
INSERT INTO `outlet` VALUES ('Ayala','Ayala Center Cebu','Business Park Cebu City','VAL','Active'),('Main','Main Office','Labogon, Mandue City',NULL,'Active'),('Pier','CPA Bldg. Pier 1, Cebu City',NULL,NULL,'Active'),('SM','SM City Cebu','North Reclamation Area, Cebu City','VAL','Active');
/*!40000 ALTER TABLE `outlet` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `position`
--

DROP TABLE IF EXISTS `position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `position` (
  `PositionCode` varchar(30) NOT NULL,
  `PositionDesc` varchar(100) DEFAULT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  PRIMARY KEY (`PositionCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `position`
--

LOCK TABLES `position` WRITE;
/*!40000 ALTER TABLE `position` DISABLE KEYS */;
INSERT INTO `position` VALUES ('AccHead','Accounting Head','Active'),('AccStaff','Accoungting Staff','Active'),('Admin','Administration','Active'),('Auditor','Accounting Auditor','Active'),('CS','Customer Service','Active'),('FD','Front Desck Officer','Active'),('GenMan','General Manager','Active'),('IT','IT Administrator','Active'),('RS','Reservation Head','Active');
/*!40000 ALTER TABLE `position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sequence`
--

DROP TABLE IF EXISTS `sequence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sequence` (
  `Table` varchar(30) NOT NULL,
  `OutletCode` varchar(30) NOT NULL,
  `Prefix` varchar(30) DEFAULT NULL,
  `Length` int(10) DEFAULT NULL,
  `LastNumber` int(12) DEFAULT NULL,
  PRIMARY KEY (`Table`,`OutletCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sequence`
--

LOCK TABLES `sequence` WRITE;
/*!40000 ALTER TABLE `sequence` DISABLE KEYS */;
INSERT INTO `sequence` VALUES ('Employee','Main','E-',5,10),('PhoneBookDetail','Main','PBD-',5,119),('PhoneBookHeader','Main','PBH-',5,25),('UserGroup','Main','UG-',5,10);
/*!40000 ALTER TABLE `sequence` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `systemconfiguration`
--

DROP TABLE IF EXISTS `systemconfiguration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `systemconfiguration` (
  `Key` varchar(30) NOT NULL,
  `Value` varchar(200) NOT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  `OutletCode` varchar(30) NOT NULL,
  PRIMARY KEY (`Key`,`Value`,`OutletCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `systemconfiguration`
--

LOCK TABLES `systemconfiguration` WRITE;
/*!40000 ALTER TABLE `systemconfiguration` DISABLE KEYS */;
INSERT INTO `systemconfiguration` VALUES ('Address','Cebu City','Active','SM'),('ApplicationName','Vanille POS','Active','SM'),('BackupMySqlDumpAddress','C:\\\\Program Files\\\\MySQL\\\\MySQL Server 5.5\\\\bin\\\\mysqldump.exe','Active','SM'),('BackupPath','.../Main/MySqlBackUp/POSBackup','Active','SM'),('BranchCode','SMCebu','Active','SM'),('ClassCode','Class01','Active','SM'),('ContactNumber','4221951','Active','SM'),('MDITabAlignment','Top','Active','SM'),('NewTenantNo','Tenant01','Active','SM'),('OutletNumber','Outlet01','Active','SM'),('OwnerName','Vanille Passeterie','Active','SM'),('Report_AccNo','027-228099611000077','Active','SM'),('Report_CompanyName','Vanille Patisserie','Active','SM'),('Report_OperatedBy','VBL Food Chain Inc.','Active','SM'),('Report_PermitNo','0908-081-35968-001','Active','SM'),('Report_SN','A3507080905','Active','SM'),('Report_TIN','254-192-762-002 VAT','Active','SM'),('RestoreMySqlAddress','C:\\\\Program Files\\\\MySQL\\\\MySQL Server 5.5\\\\bin\\\\mysql.exe','Active','SM'),('SalesType','Food Service','Active','SM'),('ScrenSaverImageURL','','Active','SM'),('ServiceChargePercentage','10','Active','SM'),('SMCoinTextPath','.../Main/SMCoinText/','Active','SM'),('TradeCode','Trade01','Active','SM');
/*!40000 ALTER TABLE `systemconfiguration` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `systemlog`
--

DROP TABLE IF EXISTS `systemlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `systemlog` (
  `LogDesc` varchar(500) DEFAULT NULL,
  `Username` varchar(30) DEFAULT NULL,
  `Date` datetime DEFAULT NULL,
  `Hostname` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `systemlog`
--

LOCK TABLES `systemlog` WRITE;
/*!40000 ALTER TABLE `systemlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `systemlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `Username` varchar(30) NOT NULL,
  `Password` varchar(50) DEFAULT NULL,
  `Fullname` varchar(100) DEFAULT NULL,
  `EmployeeId` varchar(30) DEFAULT NULL,
  `EmployeeName` varchar(100) DEFAULT NULL,
  `UserGroupId` varchar(30) DEFAULT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  PRIMARY KEY (`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('admin','21232f297a57a5a743894a0e4a801fc3','Admin','E-00001',NULL,'UG-00004','Active'),('berna','d41d8cd98f00b204e9800998ecf8427e','Berna Calinawan','E-00007',NULL,'UG-00007','Active'),('bun','d41d8cd98f00b204e9800998ecf8427e','Vanessa Lua','E-00008',NULL,'UG-00008','Active'),('butch','d41d8cd98f00b204e9800998ecf8427e','Butch Chiong','E-00003',NULL,'UG-00002','Active'),('customerservice','d41d8cd98f00b204e9800998ecf8427e','Customer Service','E-00005',NULL,'UG-00006','Active'),('fritz','44c1dfb39a3f894345bcd6f26078f384','Prexie Mae Cuesta','E-00002',NULL,'UG-00003','Active'),('jesryl','21232f297a57a5a743894a0e4a801fc3','Jesryl A. Plarisan','E-00001',NULL,'UG-00002','Active'),('rey','d41d8cd98f00b204e9800998ecf8427e','Rey Cuico','E-00006',NULL,'UG-00003','Active'),('yolly','21232f297a57a5a743894a0e4a801fc3','Yolly','E-00004',NULL,'UG-00005','Active');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usergroup`
--

DROP TABLE IF EXISTS `usergroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usergroup` (
  `UserGroupId` varchar(30) NOT NULL,
  `UserGroupDesc` varchar(100) DEFAULT NULL,
  `Status` enum('Active','Deleted') DEFAULT 'Active',
  PRIMARY KEY (`UserGroupId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usergroup`
--

LOCK TABLES `usergroup` WRITE;
/*!40000 ALTER TABLE `usergroup` DISABLE KEYS */;
INSERT INTO `usergroup` VALUES ('UG-00001','admin','Active'),('UG-00002','IT','Active'),('UG-00003','Accounting','Active'),('UG-00005','Administration','Active'),('UG-00006','Customer Service','Active'),('UG-00007','Reservation','Active');
/*!40000 ALTER TABLE `usergroup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usergroupmenuitems`
--

DROP TABLE IF EXISTS `usergroupmenuitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usergroupmenuitems` (
  `UserGroupId` varchar(30) NOT NULL,
  `Menu` varchar(50) DEFAULT NULL,
  `Item` varchar(50) DEFAULT NULL,
  `Status` enum('Enable','Disable') DEFAULT 'Enable'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usergroupmenuitems`
--

LOCK TABLES `usergroupmenuitems` WRITE;
/*!40000 ALTER TABLE `usergroupmenuitems` DISABLE KEYS */;
INSERT INTO `usergroupmenuitems` VALUES ('UG-00002','MasterFileMenu','tsmCompany','Enable'),('UG-00002','MasterFileMenu','tsmDepartment','Enable'),('UG-00002','MasterFileMenu','tsmEmployee','Enable'),('UG-00002','MasterFileMenu','tsmOutlet','Enable');
/*!40000 ALTER TABLE `usergroupmenuitems` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-01-27 15:09:20


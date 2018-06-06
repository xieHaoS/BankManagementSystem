-- MySQL dump 10.13  Distrib 8.0.1-dmr, for Win64 (x86_64)
--
-- Host: localhost    Database: bank
-- ------------------------------------------------------
-- Server version	8.0.1-dmr-log

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
-- Table structure for table `bank_administrator`
--

DROP TABLE IF EXISTS `bank_administrator`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bank_administrator` (
  `administrator_account_number` char(6) NOT NULL,
  `administrator_name` varchar(40) NOT NULL,
  `administrator_password` char(6) NOT NULL,
  PRIMARY KEY (`administrator_account_number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bank_administrator`
--

LOCK TABLES `bank_administrator` WRITE;
/*!40000 ALTER TABLE `bank_administrator` DISABLE KEYS */;
INSERT INTO `bank_administrator` VALUES ('000001','管理员1号','111111'),('000002','管理员2号','222222');
/*!40000 ALTER TABLE `bank_administrator` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bank_user`
--

DROP TABLE IF EXISTS `bank_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bank_user` (
  `user_account_number` char(19) NOT NULL,
  `user_name` varchar(40) NOT NULL,
  `id_number` char(18) NOT NULL,
  `address` varchar(40) NOT NULL,
  `user_password` char(6) NOT NULL,
  `balance` decimal(10,2) NOT NULL,
  `user_status` char(6) NOT NULL DEFAULT 'normal',
  PRIMARY KEY (`user_account_number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bank_user`
--

LOCK TABLES `bank_user` WRITE;
/*!40000 ALTER TABLE `bank_user` DISABLE KEYS */;
INSERT INTO `bank_user` VALUES ('2222222222222222222','李四','400583199911171022','广工','808080',0.00,'cancel'),('4001002003004005006','张三','400583199911171021','小谷围','909090',0.00,'cancel'),('6620530212501754141','1111111111111111111111111111111111111111','111111111111111111','111111111111111111','111111',0.00,'normal'),('7413561753466158657','写好','333333333333333333','被干','222222',0.00,'cancel'),('7416702288858145827','444444444444444444','222222233333333333','444444','444444',0.00,'normal'),('8206086556663731512','哈喽','445201199706210324','化工','232323',0.00,'cancel'),('8603576230186146387','写好','555555555555555555','西十四','343434',999909.43,'normal');
/*!40000 ALTER TABLE `bank_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `deposit_log`
--

DROP TABLE IF EXISTS `deposit_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `deposit_log` (
  `deposit_account_number` char(19) NOT NULL,
  `deposit_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `deposit_amount` decimal(10,2) NOT NULL,
  PRIMARY KEY (`deposit_account_number`,`deposit_time`),
  CONSTRAINT `deposit_log_ibfk_1` FOREIGN KEY (`deposit_account_number`) REFERENCES `bank_user` (`user_account_number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `deposit_log`
--

LOCK TABLES `deposit_log` WRITE;
/*!40000 ALTER TABLE `deposit_log` DISABLE KEYS */;
INSERT INTO `deposit_log` VALUES ('2222222222222222222','2017-12-08 02:52:35',0.00),('2222222222222222222','2017-12-08 03:05:28',100.00),('2222222222222222222','2017-12-08 03:05:34',300.00),('2222222222222222222','2017-12-08 03:05:39',500.00),('2222222222222222222','2017-12-08 03:07:43',500.00),('4001002003004005006','2017-12-08 02:28:41',100.00),('4001002003004005006','2017-12-08 02:30:53',100.00),('6620530212501754141','2017-12-11 10:20:08',0.00),('7413561753466158657','2017-12-08 03:23:41',0.00),('7413561753466158657','2017-12-10 02:55:42',200.00),('7416702288858145827','2017-12-12 16:38:45',0.00),('8206086556663731512','2017-12-10 02:57:42',0.00),('8206086556663731512','2017-12-10 12:10:14',200.00),('8206086556663731512','2017-12-10 12:36:14',-100.00),('8603576230186146387','2017-12-10 13:59:36',0.00),('8603576230186146387','2017-12-12 16:57:05',200.00),('8603576230186146387','2017-12-12 17:04:45',300.00),('8603576230186146387','2017-12-12 17:04:56',999975.98);
/*!40000 ALTER TABLE `deposit_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `withdraw_log`
--

DROP TABLE IF EXISTS `withdraw_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `withdraw_log` (
  `withdraw_account_number` char(19) NOT NULL,
  `withdraw_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `withdraw_amount` decimal(10,2) NOT NULL,
  PRIMARY KEY (`withdraw_account_number`,`withdraw_time`),
  CONSTRAINT `withdraw_log_ibfk_1` FOREIGN KEY (`withdraw_account_number`) REFERENCES `bank_user` (`user_account_number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `withdraw_log`
--

LOCK TABLES `withdraw_log` WRITE;
/*!40000 ALTER TABLE `withdraw_log` DISABLE KEYS */;
INSERT INTO `withdraw_log` VALUES ('2222222222222222222','2017-12-08 03:06:21',200.00),('2222222222222222222','2017-12-08 03:06:23',400.00),('2222222222222222222','2017-12-08 03:06:29',400.00),('2222222222222222222','2017-12-08 03:50:29',400.00),('4001002003004005006','2017-12-08 03:47:52',100.00),('4001002003004005006','2017-12-08 03:50:06',0.00),('7413561753466158657','2017-12-10 02:56:09',100.00),('7413561753466158657','2017-12-10 02:56:45',100.00),('8206086556663731512','2017-12-10 13:05:21',-100.00),('8206086556663731512','2017-12-10 13:13:15',-100.00),('8206086556663731512','2017-12-10 13:45:19',300.00),('8603576230186146387','2017-12-12 16:59:23',100.00),('8603576230186146387','2017-12-12 17:05:07',231.77),('8603576230186146387','2017-12-12 17:05:16',234.78);
/*!40000 ALTER TABLE `withdraw_log` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-06-06 20:09:33

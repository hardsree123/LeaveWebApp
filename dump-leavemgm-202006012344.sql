-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: localhost    Database: leavemgm
-- ------------------------------------------------------
-- Server version	8.0.20

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `enumvals`
--

DROP TABLE IF EXISTS `enumvals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `enumvals` (
  `code` int NOT NULL,
  `type` int NOT NULL,
  `desc` varchar(255) NOT NULL,
  `typedesc` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `enumvals`
--

LOCK TABLES `enumvals` WRITE;
/*!40000 ALTER TABLE `enumvals` DISABLE KEYS */;
INSERT INTO `enumvals` VALUES (100003,100,'hr','designation'),(100004,100,'manager','designation'),(100005,100,'employee','designation'),(200001,200,'sick','leavetype'),(200002,200,'casual','leavetype'),(200003,200,'earn','leavetype'),(300001,300,'pending','leavestatus'),(300002,300,'approved','leavestatus'),(300003,300,'cancel','leavestatus'),(300004,300,'approvedbyhr','leavestatus'),(300005,300,'rejectedbyhr','leavestatus');
/*!40000 ALTER TABLE `enumvals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `leaverecords`
--

DROP TABLE IF EXISTS `leaverecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaverecords` (
  `id` int NOT NULL AUTO_INCREMENT,
  `reqid` varchar(8) NOT NULL,
  `empcode` varchar(8) NOT NULL,
  `reason` varchar(2500) NOT NULL,
  `leavetype` int NOT NULL,
  `leavefrom` datetime NOT NULL,
  `leaveto` datetime NOT NULL,
  `assignedto` varchar(8) NOT NULL,
  `status` int NOT NULL DEFAULT '200001',
  `lastapprover` varchar(8) DEFAULT NULL,
  `lastapprovaltime` datetime DEFAULT NULL,
  `rejectiondesc` varchar(2500) DEFAULT NULL,
  `attachmentpath` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `reqid` (`reqid`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `leaverecords`
--

LOCK TABLES `leaverecords` WRITE;
/*!40000 ALTER TABLE `leaverecords` DISABLE KEYS */;
INSERT INTO `leaverecords` VALUES (1,'PP-1000','SE-1000','HI',200001,'2020-06-04 00:00:00','2020-06-12 00:00:00','PM-1000',300002,NULL,NULL,NULL,NULL),(2,'PP-1001','SE-1000','I am sick i need a  leave',200002,'2020-06-11 00:00:00','2020-06-19 00:00:00','PM-1000',300002,NULL,NULL,NULL,NULL),(3,'PP-1002','SE-1000','Next week casual',200002,'2020-06-17 00:00:00','2020-06-19 00:00:00','PM-1000',300002,'PM-1000','2020-06-01 20:06:55',NULL,NULL),(4,'PP-1003','SE-1000','wqe',200001,'2020-06-04 00:00:00','2020-06-19 00:00:00','HR-1000',300005,'PM-1000','2020-06-01 20:47:17','rejected',NULL),(5,'PP-1004','SE-1000','asd',200002,'2020-06-12 00:00:00','2020-06-28 00:00:00','HR-1000',300004,'HR-1000','2020-06-01 20:38:55',NULL,NULL),(6,'PP-1005','SE-1000','New',200001,'2020-06-04 00:00:00','2020-06-12 00:00:00','HR-1000',300004,'HR-1000','2020-06-01 20:48:38',NULL,NULL),(8,'PP-1006','SE-1000','ASAQ',200001,'2020-06-02 00:00:00','2020-06-25 00:00:00','HR-1000',300004,'HR-1000','2020-06-01 20:48:43',NULL,NULL),(9,'PP-1007','SE-1000','qweqe',200003,'2020-06-10 00:00:00','2020-06-20 00:00:00','PM-1000',300003,NULL,NULL,'aweq',NULL),(10,'PP-1008','SE-1000','asdwqe',200001,'2020-06-05 00:00:00','2020-06-27 00:00:00','PM-1000',300003,NULL,NULL,'Rejecting this leave',NULL),(11,'PP-1009','SE-1000','aseqweqwe',200002,'2020-06-03 00:00:00','2020-06-13 00:00:00','PM-1000',300001,NULL,NULL,NULL,NULL),(12,'PP-1010','SE-1000','asewqeqwe',200001,'2020-06-02 00:00:00','2020-06-19 00:00:00','SE-1000',300002,'PM-1000','2020-06-01 20:20:20',NULL,NULL),(13,'PP-1011','SE-1000','Hi',200001,'2020-06-04 00:00:00','2020-06-27 00:00:00','PM-1000',300001,NULL,NULL,NULL,NULL),(14,'PP-1012','SE-1000','Test',200001,'2020-06-03 00:00:00','2020-06-17 00:00:00','PM-1000',300001,NULL,NULL,NULL,NULL),(15,'PP-1013','SE-1000','aseqwe',200001,'2020-06-02 00:00:00','2020-06-26 00:00:00','PM-1000',300001,NULL,NULL,NULL,NULL),(16,'PP-1014','SE-1000','New leave',200001,'2020-06-13 00:00:00','2020-06-26 00:00:00','PM-1000',300001,NULL,NULL,NULL,NULL),(17,'PP-1015','SE-1000','qwe',200001,'2020-07-01 00:00:00','2020-07-04 00:00:00','PM-1000',300001,NULL,NULL,NULL,NULL),(18,'PP-1016','SE-1000','aewq',200001,'2020-06-04 00:00:00','2020-06-28 00:00:00','PM-1000',300001,NULL,NULL,NULL,NULL),(19,'PP-1017','SE-1000','qweqwe',200001,'2020-06-20 00:00:00','2020-06-28 00:00:00','HR-1000',300002,'PM-1000','2020-06-01 22:53:41',NULL,NULL),(20,'PP-1018','SE-1000','I need a leave for my marriage',200001,'2020-06-05 00:00:00','2020-06-28 00:00:00','HR-1000',300004,'HR-1000','2020-06-01 23:30:38',NULL,NULL),(21,'PP-1019','SE-1000','New leave please',200001,'2020-06-05 00:00:00','2020-06-26 00:00:00','HR-1000',300004,'HR-1000','2020-06-01 23:29:35',NULL,NULL);
/*!40000 ALTER TABLE `leaverecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `password` varchar(10) NOT NULL,
  `empcode` varchar(8) NOT NULL,
  `empname` varchar(50) NOT NULL,
  `empcont` varchar(10) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `designation` int NOT NULL,
  `managedby` varchar(8) DEFAULT NULL,
  `teamname` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'sreejith','sreejith','SE-1000','sreejith kumar','9747359124','hardsree123@gmail.com',100005,'PM-1000','MHK'),(2,'Basheer','Basheer','SE-1001','Basheer','9747359111','hardsree123@gmail.com',100005,'PM-1000','MHK'),(3,'Preetha','Preetha','PM-1000','Preetha','9747359111','kumar.sreejith.m@gmail.com',100004,NULL,'MHK'),(4,'Vishnu','Vishnu','HR-1000','Vishnu','9747359111','kumar.sreejith.m@gmail.com',100003,NULL,'MHK');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'leavemgm'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-06-01 23:44:08

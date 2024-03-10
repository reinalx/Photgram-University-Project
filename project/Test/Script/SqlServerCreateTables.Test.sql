 DECLARE @Default_DB_Path as VARCHAR(64)  
 SET @Default_DB_Path = N'C:\SourceCode\DataBase\'
 
USE [master]


/***** Drop database if already exists  ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = 'practicamad_test')
DROP DATABASE [practicamad_test]


USE [master]


/* DataBase Creation */

	                              
DECLARE @sql nvarchar(500)

SET @sql = 
  N'CREATE DATABASE [practicamad_test] 
    ON PRIMARY ( NAME = practicamad_test, FILENAME = "' + @Default_DB_Path + N'practicamad_test.mdf")
    LOG ON ( NAME = practicamad_test_log, FILENAME = "' + @Default_DB_Path + N'practicamad_test.ldf")'

EXEC(@sql)
PRINT N'Database [PracticaMaD_Test] created.'
GO

USE [practicamad_test]

/* ********** Drop Table Comment if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Comment]') AND type in ('U'))
DROP TABLE [Comment]
GO

/* ********** Drop Table LikePub if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[LikePub]') AND type in ('U'))
DROP TABLE [LikePub]
GO

/* ********** Drop Table Follow if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Follow]') AND type in ('U'))
DROP TABLE [Follow]
GO

/* ********** Drop Table Tag if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[PostTag]') AND type in ('U')) 
DROP TABLE [PostTag]
GO

/* ********** Drop Table Tag if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Tag]') AND type in ('U')) 
DROP TABLE [Tag]
GO
/* ********** Drop Table Publication if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Post]') AND type in ('U'))
DROP TABLE [Post]
GO
/* ********** Drop Table UserProfile if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[UserProfile]') AND type in ('U'))
DROP TABLE [UserProfile]
GO


/* ********** Drop Table Category if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Category]') AND type in ('U'))
DROP TABLE [Category]
GO




/*  UserProfile */
CREATE TABLE UserProfile (
	usrId bigint IDENTITY(1,1) NOT NULL,
	loginName varchar(30) NOT NULL,
	enPassword varchar(50) NOT NULL,
	firstName varchar(30) NOT NULL,
	lastName varchar(40) NOT NULL,
	email varchar(60) NOT NULL,
	language varchar(2) NOT NULL,
	country varchar(2) NOT NULL,
	numFollows int NOT NULL DEFAULT 0,
	numFollowers int NOT NULL DEFAULT 0

	CONSTRAINT [PK_UserProfile] PRIMARY KEY (usrId),
	CONSTRAINT [UniqueKey_Login] UNIQUE (loginName)
)


CREATE NONCLUSTERED INDEX [IX_UserProfileIndexByLoginName]
ON [UserProfile] ([loginName] ASC)

PRINT N'Table UserProfile created.'
GO



/*  Category */

CREATE TABLE Category (
	categoryId bigint IDENTITY(1,1) NOT NULL,
	categoryName varchar(30) NOT NULL,

	CONSTRAINT [PK_Category] PRIMARY KEY (categoryId),
	CONSTRAINT [UK_CategoryName] UNIQUE (categoryName)
)


PRINT N'Table Category created.'
GO



/*  Post */

CREATE TABLE Post (
	postId bigint IDENTITY(1,1) NOT NULL,
	title varchar(30) NOT NULL,
	img varchar(255) NOT NULL,
	description varchar(80) ,
	date DATETIME NOT NULL,
	likes int NOT NULL DEFAULT 0,
	diaphragmOpen float,
	timeExp float,
	ISO float,
	whiteBal float ,
	categoryId BIGINT NOT NULL,
	usrId BIGINT NOT NULL,

	CONSTRAINT [PK_Post] PRIMARY KEY (postId),
	CONSTRAINT [FK_Post_Category] FOREIGN KEY (categoryId) REFERENCES Category (categoryId),
	CONSTRAINT [FK_Post_UserProfile] FOREIGN KEY (usrId) REFERENCES UserProfile (usrId)
)

PRINT N'Table Post created.'
GO

CREATE TABLE Tag (
	tagId BIGINT IDENTITY(1, 1) NOT NULL,
	tagName VARCHAR(20) NOT NULL,
	timesUsed int NOT NULL,

	CONSTRAINT [PK_Tag] PRIMARY KEY (tagId),
	CONSTRAINT [UK_TagName] UNIQUE (tagName)
)

PRINT N'Table Tag created.'
GO

/* PostTag */

CREATE TABLE PostTag (
	postId BIGINT,
	tagId BIGINT,

	CONSTRAINT [PK_PostTag] PRIMARY KEY (postId, tagId),
	CONSTRAINT [FK_PostTag_Post] FOREIGN KEY (postId) REFERENCES Post (postId),
	CONSTRAINT [FK_PostTag_Tag] FOREIGN KEY (tagId) REFERENCES Tag (tagId)
)

PRINT N'Table PostTag created.'
GO

/* Like */

CREATE TABLE LikePub (
	
	likeId bigint IDENTITY(1,1) NOT NULL,
	usrId bigint NOT NULL,
	postId bigint NOT NULL, 

	CONSTRAINT [PK_LikePhoto] PRIMARY KEY (likeId),
	CONSTRAINT [FK_LikePub_UserProfile] FOREIGN KEY (usrId) REFERENCES UserProfile (usrId),
	CONSTRAINT [FK_LikePub_Post] FOREIGN KEY (postId) REFERENCES Post (postId)

)

PRINT N'Table LikePub created.'
GO



/* Comment */

CREATE TABLE Comment(
	commentId bigint IDENTITY(1,1) NOT NULL,
	text varchar(80) NOT NULL,
	date DATETIME NOT NULL,
	usrId BIGINT NOT NULL,
	postId BIGINT NOT NULL,

	CONSTRAINT [PK_Comment] PRIMARY KEY (commentId),
	CONSTRAINT [FK_Comment_UserProfile] FOREIGN KEY (usrId) REFERENCES UserProfile (usrId),
	CONSTRAINT [FK_Comment_Post] FOREIGN KEY (postId) REFERENCES Post (postId)

)

PRINT N'Table Comment created.'
GO


CREATE TABLE Follow (
	
	followId bigint IDENTITY(1,1) NOT NULL,
	usrId1 bigint NOT NULL,
	usrId2 bigint NOT NULL, 

	CONSTRAINT [PK_Follow] PRIMARY KEY (followId),
	CONSTRAINT [FK_Follow_UserProfile1] FOREIGN KEY (usrId1) REFERENCES UserProfile (usrId),
	CONSTRAINT [FK_Follow_UserProfiel2] FOREIGN KEY (usrId2) REFERENCES UserProfile (usrId)

)

PRINT N'Table Follow created.'
GO


GO




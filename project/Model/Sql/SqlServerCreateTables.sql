/* 
 * SQL Server Script
 * 
 * This script can be directly executed to configure the test database from
 * PCs located at CECAFI Lab. The database and the corresponding users are 
 * already created in the sql server, so it will create the tables needed 
 * in the samples. 
 * 
 * In a local environment (for example, with the SQLServerExpress instance 
 * included in the VStudio installation) it will be necessary to create the 
 * database and the user required by the connection string. So, the following
 * steps are needed:
 *
 *      Configure within the CREATE DATABASE sql-sentence the path where 
 *      database and log files will be created  
 *
 * This script can be executed from MS Sql Server Management Studio Express,
 * but also it is possible to use a command Line syntax:
 *
 *    > sqlcmd.exe -U [user] -P [password] -I -i SqlServerCreateTables.sql
 *
 */

 
USE [practicamad]





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


/*
 * Create tables.
 * UserProfile table is created. Indexes required for the 
 * most common operations are also defined.
 */

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

/* Tag */

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

/* ************ Insert data into tables ************ */

INSERT INTO UserProfile(loginName, enPassword, firstName, lastName, email, language, country) VALUES ('prueba1', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'User1', 'lastName1', 'admin@admin.com', 'en', 'US');
INSERT INTO UserProfile(loginName, enPassword, firstName, lastName, email, language, country) VALUES ('prueba2', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'User2', 'lastName2', 'test@test.com', 'es', 'ES');
INSERT INTO UserProfile(loginName, enPassword, firstName, lastName, email, language, country ) VALUES ('prueba3', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'User3', 'lastName3', 'user@admin.com', 'en', 'US');
INSERT INTO UserProfile(loginName, enPassword, firstName, lastName, email, language, country ) VALUES ('nothingUser', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'nothingUser', 'nothing', 'nothing@auser.com', 'en', 'US');


INSERT INTO Category(categoryName) VALUES ('Category1');
INSERT INTO Category(categoryName) VALUES ('Category2');


/* POSTS */

/* Post Usuario 1*/
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img1',  '~/imgSources/bien.png' , 'description1', CONVERT(DATETIME, '26/10/2023 14:30:00', 103), 1, 1, 1);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img2', '~/imgSources/prueba.png', 'description2', CONVERT(DATETIME, '26/10/2023 14:30:00', 103), 1, 1, 1);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img3', '~/imgSources/esta.png', 'description3', CONVERT(DATETIME, '26/10/2023 14:30:00', 103), 1, 1, 1);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img4', '~/imgSources/prueba.png', 'description4', CONVERT(DATETIME, '26/10/2023 14:30:00', 103), 1, 1, 1);


/*Post Usuario 2*/

INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img5','~/imgSources/bien.png', 'description5', CONVERT(DATETIME, '26/10/2023 14:40:00', 103), 2, 2, 2);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img6', '~/imgSources/prueba.png', 'description6', CONVERT(DATETIME, '26/10/2023 14:53:00', 103), 2, 2, 2);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img7', '~/imgSources/esta.png', 'description7', CONVERT(DATETIME, '27/10/2023 14:40:00', 103), 2, 2, 2);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img8', '~/imgSources/prueba.png', 'description8', CONVERT(DATETIME, '29/10/2023 14:40:00', 103), 2, 2, 2);

/*Post Usuario 3*/

INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img9','~/imgSources/prueba.png', 'description9', CONVERT(DATETIME, '26/10/2023 14:30:00', 103), 2, 3, 2);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img10','~/imgSources/prueba.png', 'description10', CONVERT(DATETIME, '26/10/2023 14:55:00', 103), 2, 3, 2);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img11', '~/imgSources/prueba.png', 'description11', CONVERT(DATETIME, '27/10/2023 14:30:00', 103), 2, 3, 2);
INSERT INTO Post(title, img, description, date, likes, usrId, categoryId) VALUES('img12','~/imgSources/prueba.png', 'description12', CONVERT(DATETIME, '29/10/2023 14:50:00', 103), 2, 3, 2);


/*FOLLOWS*/

INSERT INTO Follow (usrId1, usrId2) VALUES (1, 2);
INSERT INTO Follow (usrId1, usrId2) VALUES (1, 3);

INSERT INTO Follow (usrId1, usrId2) VALUES (2, 1);
INSERT INTO Follow (usrId1, usrId2) VALUES (2, 3);

INSERT INTO Follow (usrId1, usrId2) VALUES (3, 1);
INSERT INTO Follow (usrId1, usrId2) VALUES (3, 2);


/* Comments */

/* Comentarios del Usuario1 */
INSERT INTO Comment (text, date, usrId, postId) VALUES ('usuario1 comentando en post1', CONVERT(DATETIME, '30/10/2023 14:35:00', 103), 1, 1);
INSERT INTO Comment (text, date, usrId, postId) VALUES ('usuario1 comentando en post2', CONVERT(DATETIME, '30/10/2023 14:30:00', 103), 1, 2);
INSERT INTO Comment (text, date, usrId, postId) VALUES ('usuario1 comentando en post3', CONVERT(DATETIME, '30/10/2023 14:40:00', 103), 1, 3);

/* Comentarios del Usuario2 */
INSERT INTO Comment (text, date, usrId, postId) VALUES ('usuario2 comentando en post1', CONVERT(DATETIME, '30/10/2023 14:30:00', 103), 2, 1);
INSERT INTO Comment (text, date, usrId, postId) VALUES ('usuario2 comentando en post1 2', CONVERT(DATETIME, '30/10/2023 14:25:00', 103), 2, 1);

/* Comentarios del Usuario3 */
INSERT INTO Comment (text, date, usrId, postId) VALUES ('usuario3 comentando en post1', CONVERT(DATETIME, '30/10/2023 14:29:00', 103), 3, 1);
INSERT INTO Comment (text, date, usrId, postId) VALUES ('usuario3 comentando en post2', CONVERT(DATETIME, '30/10/2023 14:35:00', 103), 3, 2);

/*LIKES*/

INSERT INTO LikePub (usrId, postId) VALUES (1, 1);
/*Likes Post 1*/
USE [master]
GO
CREATE DATABASE FrapperAuditDB;
GO
USE [FrapperAuditDB]
GO
/****** Object:  StoredProcedure [dbo].[Usp_AuditTB]    Script Date: 21-11-2020 8.35.40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Usp_AuditTB] 
	@UrlReferrer varchar(500),
	@ActionName varchar(50),
	@ControllerName varchar(50),
	@LoginStatus varchar(1),
	@LoggedInAt varchar(50),
	@LoggedOutAt varchar(50),
	@PageAccessed varchar(500),
	@IPAddress varchar(50),
	@SessionID varchar(50),
	@UserID varchar(5)
AS
BEGIN

DECLARE 
    @table varchar(15),
    @sql NVARCHAR(MAX),
	@sqlcreate NVARCHAR(MAX);
 
SET @table = (Select REPLACE(CONVERT(varchar(11), getdate(), 106),' ','_'))

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @table))
BEGIN
   
	SET @sql = CONCAT('INSERT INTO [', @table, '] (ControllerName,ActionName,LoginStatus,LoggedInAt,LoggedOutAt,PageAccessed,IPAddress,SessionID,UserID) ',
					'VALUES (''', @ControllerName, ''',''', @ActionName, ''',''', @LoginStatus, ''',''', @LoggedInAt, ''',''', @LoggedOutAt, ''',''', 
					@PageAccessed, ''',''', @IPAddress, ''',''', @SessionID, ''',''', @UserID, ''')');
	EXEC (@sql);

END
 else
 begin

 SET @sqlcreate = 'CREATE TABLE '+'['+ @table +']'+'([AuditId] [bigint] IDENTITY(1,1) NOT NULL,[ControllerName] [varchar](50) NULL,[ActionName] [varchar](50) NULL,[LoginStatus] [varchar](1) NULL,[LoggedInAt] [varchar](50) NULL,[LoggedOutAt] [varchar](50) NULL,[PageAccessed] [varchar](500) NULL,[IPAddress] [varchar](50) NULL,[SessionID] [varchar](50) NULL,[UserID] [varchar](50) NULL)'
 EXEC sp_executesql @sqlcreate;

 SET @sql =CONCAT('INSERT INTO [', @table, '] (ControllerName,ActionName,LoginStatus,LoggedInAt,LoggedOutAt,PageAccessed,IPAddress,SessionID,UserID) ',
					'VALUES (''', @ControllerName, ''',''', @ActionName, ''',''', @LoginStatus, ''',''', @LoggedInAt, ''',''', @LoggedOutAt, ''',''', 
					@PageAccessed, ''',''', @IPAddress, ''',''', @SessionID, ''',''', @UserID, ''')');
 EXEC (@sql);

 end
 
END
GO

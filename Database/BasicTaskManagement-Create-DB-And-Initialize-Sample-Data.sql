USE master
GO

DECLARE @SQL nvarchar(1000);
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'BasicTaskManagement')
BEGIN
    SET @SQL = N'USE BasicTaskManagement;

                 ALTER DATABASE BasicTaskManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                 USE master;

                 DROP DATABASE BasicTaskManagement;';
    EXEC (@SQL);
END;

CREATE DATABASE BasicTaskManagement
GO

USE BasicTaskManagement
GO

CREATE TABLE TaskGroups (
	Id				    int IDENTITY(1,1)					NOT NULL,
	[Name]				varchar(50)                         NOT NULL,
	IsFavorite			bit									NOT NULL,
	CONSTRAINT PK_TaskGroups PRIMARY KEY(Id),
	CONSTRAINT UC_TaskGroupName UNIQUE([Name]),
)
GO

CREATE TABLE TaskItems (
	Id  				int IDENTITY(1,1)					NOT NULL,
	TaskGroupId			int									NOT NULL,
	[Name]				varchar(50)                         NOT NULL,
	Notes 		        varchar(100)						NULL,
	IsImportant			bit									NOT NULL,
	IsComplete			bit									NOT NULL,
	DueDate				datetime						    NULL,
	CompletedDate		datetime							NULL,
	CreateDate			datetime						    NOT NULL,
	UpdateDate			datetime							NULL,
	CONSTRAINT PK_TaskItems PRIMARY KEY(Id),
	CONSTRAINT FK_TaskItems_TaskGroups FOREIGN KEY(TaskGroupId) REFERENCES TaskGroups(Id),
)
GO

-- optional sample data
INSERT INTO TaskGroups([Name],IsFavorite) VALUES('Family',1);
INSERT INTO TaskGroups([Name],IsFavorite) VALUES('House and home',0);
INSERT INTO TaskGroups([Name],IsFavorite) VALUES('Pastimes',0);
INSERT INTO TaskGroups([Name],IsFavorite) VALUES('Career',1);
INSERT INTO TaskGroups([Name],IsFavorite) VALUES('Holidays',0);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Family'),'Sign up for soccer camp','price goes up June 1',1,0,DATEADD(day,3,GETDATE()),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Family'),'Schedule movie night','How about Star Wars?',0,0,DATEADD(day,1,GETDATE()),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Family'),'Send card to Carol',NULL,1,1,DATEADD(day,5,GETDATE()),GETDATE(),GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Family'),'Gift for Andrew',NULL,0,0,DATEADD(day,7,GETDATE()),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Family'),'Set up photos for holiday cards','prefer to get these when it is sunny',0,0,DATEADD(day,10,GETDATE()),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'House and home'),'Clean gutters',NULL,0,0,NULL,NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'House and home'),'Mow lawn',NULL,1,1,DATEADD(day,1,GETDATE()),DATEADD(day,-3,GETDATE()),GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'House and home'),'Sweep front porch',NULL,1,0,DATEADD(day,-3,GETDATE()),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'House and home'),'Schedule lawn treatment','have a coupon for this somewhere...',1,0,DATEADD(day,-10,GETDATE()),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'House and home'),'Change furnace filter',NULL,0,1,NULL,DATEADD(day,-5,GETDATE()),GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Pastimes'),'Sell Cartoonball cards','the ones under the bed',0,0,NULL,NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Pastimes'),'Check condition of camping gear',NULL,1,1,DATEADD(day,3,GETDATE()),GETDATE(),GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Pastimes'),'Decline game night invite for next week','with regrets',1,0,GETDATE(),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Career'),'Schedule social media posts',NULL,0,0,NULL,NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Career'),'Set up time to talk to recruiter','the candidate is a really good match',1,0,DATEADD(day,-1,GETDATE()),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Career'),'Catch up on LinkedIn',NULL,0,1,NULL,DATEADD(day,-14,GETDATE()),GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Career'),'Email sales, they did a great job last quarter',NULL,1,0,GETDATE(),NULL,GETDATE(),NULL);
INSERT INTO TaskItems(TaskGroupId,[Name],Notes,IsImportant,IsComplete,DueDate,CompletedDate,CreateDate,UpdateDate) VALUES((SELECT g.Id FROM TaskGroups g WHERE g.[Name] = 'Career'),'Clear Friday schedule','time to play!',1,1,DATEADD(day,1,GETDATE()),DATEADD(day,-1,GETDATE()),GETDATE(),NULL);

USE [SKD]
IF NOT EXISTS (SELECT * FROM Patches WHERE Id = 'Test')
BEGIN
	INSERT INTO Patches (Id) VALUES ('Test')    
END
GO
IF NOT EXISTS (SELECT * FROM Patches WHERE Id = 'OrganisationUser')
BEGIN
	IF NOT EXISTS (SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE table_name = 'OrganisationUser')
	BEGIN
		CREATE TABLE OrganisationUser(
			[UID] [uniqueidentifier] NOT NULL,
			[UserUID] [uniqueidentifier] NOT NULL,
			[OrganisationUID] [uniqueidentifier] NOT NULL,
		CONSTRAINT [PK_OrganisationUser] PRIMARY KEY CLUSTERED 
		(
			[UID] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]	
		ALTER TABLE [dbo].[OrganisationUser] WITH NOCHECK ADD CONSTRAINT [FK_OrganisationUser_Organisation] FOREIGN KEY([OrganisationUid])
		REFERENCES [dbo].[Organisation] ([Uid])
		NOT FOR REPLICATION 
		ALTER TABLE [dbo].[OrganisationUser] NOCHECK CONSTRAINT [FK_OrganisationUser_Organisation]
	END
	INSERT INTO Patches (Id) VALUES ('OrganisationUser')    
END
GO
IF NOT EXISTS (SELECT * FROM Patches WHERE Id = 'AlterPatches')
BEGIN
	ALTER TABLE Patches ALTER COLUMN Id nvarchar(100) not null
	INSERT INTO Patches (Id) VALUES ('AlterPatches')    
END
GO
IF NOT EXISTS (SELECT * FROM Patches WHERE Id = 'OrganisationGuardZone')
BEGIN
	IF NOT EXISTS (SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE table_name = 'OrganisationGuardZone')
	BEGIN
		CREATE TABLE OrganisationGuardZone(
			[UID] [uniqueidentifier] NOT NULL,
			[UserUID] [uniqueidentifier] NOT NULL,
			[OrganisationUID] [uniqueidentifier] NOT NULL,
		CONSTRAINT [PK_OrganisationGuardZone] PRIMARY KEY CLUSTERED 
		(
			[UID] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]	
		ALTER TABLE [dbo].[OrganisationGuardZone] WITH NOCHECK ADD CONSTRAINT [FK_OrganisationGuardZone_Organisation] FOREIGN KEY([OrganisationUid])
		REFERENCES [dbo].[Organisation] ([Uid])
		NOT FOR REPLICATION 
		ALTER TABLE [dbo].[OrganisationGuardZone] NOCHECK CONSTRAINT [FK_OrganisationGuardZone_Organisation]
	END
	INSERT INTO Patches (Id) VALUES ('OrganisationGuardZone')    
END
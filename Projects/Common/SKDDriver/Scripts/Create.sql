USE [master]
GO
IF EXISTS(SELECT name FROM sys.databases WHERE name = 'SKD')
BEGIN
	SET NOEXEC ON
END
GO
CREATE DATABASE SKD
GO
USE SKD

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TYPE SqlTime FROM DATETIME

CREATE TYPE SqlDate FROM DATETIME
GO
CREATE RULE TimeOnlyRule AS
	datediff(dd, 0, @DateTime) = 0
GO
CREATE RULE DateOnlyRule AS
	dateAdd(dd, datediff(dd,0,@DateTime), 0) = @DateTime
GO
EXEC sp_bindrule 'TimeOnlyRule', 'SqlTime'
EXEC sp_bindrule 'DateOnlyRule', 'SqlDate'

CREATE TABLE [dbo].[Photo](
	[UID] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](MAX) NULL,
CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[AdditionalColumnType](
	[UID] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[RemovalDate] [datetime] NOT NULL,	
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[DataType] [int] NULL,
	[PersonType] [int],
	[OrganisationUID] [uniqueidentifier] NULL,
	[IsInGrid] [bit] NOT NULL, 
CONSTRAINT [PK_AdditionalColumnType] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[AdditionalColumn](
	[UID] [uniqueidentifier] NOT NULL,
	[EmployeeUID] [uniqueidentifier] NULL,
	[AdditionalColumnTypeUID] [uniqueidentifier] NULL,
	[TextData] [text] NULL,
	[PhotoUID] [uniqueidentifier] NULL,
CONSTRAINT [PK_AdditionalColumn] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[Day](
	[UID] [uniqueidentifier] NOT NULL,
	[NamedIntervalUID] [uniqueidentifier] NULL,
	[ScheduleSchemeUID] [uniqueidentifier] NOT NULL,
	[Number] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL 
 CONSTRAINT [PK_Day] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Department](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[PhotoUID] [uniqueidentifier] NULL,
	[ParentDepartmentUID] [uniqueidentifier] NULL,
	[ContactEmployeeUID] [uniqueidentifier] NULL,
	[AttendantUID] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Department_1] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Employee](
	[UID] uniqueidentifier NOT NULL,
	FirstName nvarchar(50) NULL,
	SecondName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	PhotoUID uniqueidentifier NULL, 
	PositionUID uniqueidentifier NULL,
	DepartmentUID uniqueidentifier NULL,
	ScheduleUID uniqueidentifier NULL,
	ScheduleStartDate datetime NOT NULL,
	Appointed datetime NOT NULL,
	Dismissed datetime NOT NULL,
	[Type] int NULL,
	TabelNo int NOT NULL,
	CredentialsStartDate datetime NOT NULL,
	EscortUID uniqueidentifier NULL,
	IsDeleted bit NOT NULL ,
	RemovalDate datetime NOT NULL ,
	OrganisationUID uniqueidentifier NULL,
	DocumentNumber nvarchar(50),
	BirthDate datetime NOT NULL,
	BirthPlace nvarchar(MAX),
	DocumentGivenDate datetime NOT NULL,
	DocumentGivenBy nvarchar(MAX),
	DocumentValidTo datetime NOT NULL,
	Gender int NOT NULL,
	DocumentDepartmentCode nvarchar(50),
	Citizenship nvarchar(MAX),
	DocumentType int NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[EmployeeReplacement](
	[UID] [uniqueidentifier] NOT NULL,
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[EmployeeUID] [uniqueidentifier] NULL,
	[DepartmentUID] [uniqueidentifier] NULL,
	[ScheduleUID] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_EmployeeReplacement] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Holiday](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[TransferDate] [datetime] NULL,
	[Reduction] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Holiday] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Interval](
	[BeginTime] [int] NOT NULL,
	[EndTime] [int] NOT NULL,
	[UID] [uniqueidentifier] NOT NULL,
	[NamedIntervalUID] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
CONSTRAINT [PK_Interval] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[NamedInterval](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SlideTime] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_NamedInterval_1] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Phone](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NumberString] [nvarchar](50) NULL,
	[DepartmentUID] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Phone] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Position](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
	[PhotoUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Schedule](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ScheduleSchemeUID] [uniqueidentifier] NULL,
	[IsIgnoreHoliday] [bit] NOT NULL  DEFAULT 0,
	[IsOnlyFirstEnter] [bit] NOT NULL  DEFAULT 0,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[ScheduleScheme](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ScheduleScheme] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Document](
	[UID] [uniqueidentifier] NOT NULL,
	[No] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[IssueDate] [datetime] NOT NULL,
	[LaunchDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Journal](
	[UID] [uniqueidentifier] NOT NULL,
	[SystemDate] [datetime] NOT NULL,
	[DeviceDate] [datetime] NOT NULL,
	[Subsystem] [int] NOT NULL,
	[Name] [int] NOT NULL,
	[Description] [int] NOT NULL,
	[NameText] [nvarchar](50) NULL,
	[DescriptionText] [nvarchar](max) NULL,
	[State] [int] NOT NULL, 
	[ObjectType] [int] NOT NULL,
	[ObjectName] [nvarchar](50) NULL, 
	[ObjectUID] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NULL, 
	[CardNo] int NOT NULL, 
CONSTRAINT [PK_Journal] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Card](
	[UID] [uniqueidentifier] NOT NULL,
	[Number] [int] NOT NULL,
	[EmployeeUID] [uniqueidentifier] NULL,
	[AccessTemplateUID] [uniqueidentifier] NULL,
	[CardType] [int] NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[IsInStopList] [bit] NOT NULL,
	[StopReason] [text] NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	CardTemplateUID [uniqueidentifier] NULL,
CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[PendingCard](
	[UID] [uniqueidentifier] NOT NULL,
	[CardUID] [uniqueidentifier] NOT NULL,
	[ControllerUID] [uniqueidentifier] NOT NULL,
	[Action] [int] NOT NULL,
CONSTRAINT [PK_PendingCard] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[CardDoor](
	[UID] [uniqueidentifier] NOT NULL,
	[DoorUID] [uniqueidentifier] NOT NULL,
	[ParentUID] [uniqueidentifier] NULL,
	[ParentType] [int] NULL,
	[IsWithEscort] [bit] NOT NULL,
	[IsAntipass] [bit] NOT NULL,
	[EnterIntervalID] [int] NOT NULL,
	[EnterIntervalType] [int] NULL,
	[ExitIntervalID] [int] NOT NULL,
	[ExitIntervalType] [int] NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
 CONSTRAINT [PK_CardDoor] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[AccessTemplate](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
	[OrganisationUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AccessTemplate] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[ScheduleZone](
	[UID] [uniqueidentifier] NOT NULL,
	[ZoneUID] [uniqueidentifier] NOT NULL,
	[ScheduleUID] [uniqueidentifier] NOT NULL,
	[IsControl] [bit] NOT NULL DEFAULT 0,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
 CONSTRAINT [PK_ScheduleZone] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Organisation](
	[UID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[PhotoUID] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NOT NULL ,
	[RemovalDate] [datetime] NOT NULL ,
 CONSTRAINT [PK_Organisation] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[OrganisationUser](
	[UID] [uniqueidentifier] NOT NULL,
	[UserUID] [uniqueidentifier] NOT NULL,
	[OrganisationUID] [uniqueidentifier] NOT NULL,
CONSTRAINT [PK_OrganisationUser] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[OrganisationDoor](
	[UID] [uniqueidentifier] NOT NULL,
	[DoorUID] [uniqueidentifier] NOT NULL,
	[OrganisationUID] [uniqueidentifier] NOT NULL,
CONSTRAINT [PK_OrganisationDoor] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[OrganisationZone](
	[UID] [uniqueidentifier] NOT NULL,
	[ZoneUID] [uniqueidentifier] NOT NULL,
	[OrganisationUID] [uniqueidentifier] NOT NULL,
CONSTRAINT [PK_OrganisationZone] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[OrganisationCardTemplate](
	[UID] [uniqueidentifier] NOT NULL,
	[CardTemplateUID] [uniqueidentifier] NOT NULL,
	[OrganisationUID] [uniqueidentifier] NOT NULL,
CONSTRAINT [PK_OrganisationCardTemplate] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[GuardZone](
	[UID] [uniqueidentifier] NOT NULL,
	[ZoneUID] [uniqueidentifier] NOT NULL,
	[ParentUID] [uniqueidentifier] NOT NULL,
	[CanSet] [bit] NOT NULL,
	[CanReset] [bit] NOT NULL,
CONSTRAINT [PK_GuardZone] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[PassJournal](
	[UID] [uniqueidentifier] NOT NULL,
	[EmployeeUID] [uniqueidentifier] NOT NULL,
	[ZoneUID] [uniqueidentifier] NOT NULL,
	[EnterTime] [datetime] NOT NULL,
	[ExitTime] [datetime] NOT NULL,
 CONSTRAINT [PK_PassJournal] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE Patches (Id nvarchar(100) not null)
CREATE TABLE EventNames (EventName int not null)
CREATE TABLE EventDescriptions (EventDescription int not null)

CREATE INDEX IntervalUIDIndex ON [dbo].[Interval]([UID])
CREATE INDEX NamedIntervalUIDIndex ON NamedInterval([UID])
CREATE INDEX DayUIDIndex ON [Day]([UID])
CREATE INDEX ScheduleSchemeUIDIndex ON ScheduleScheme([UID])
CREATE INDEX ScheduleUIDIndex ON Schedule([UID])
CREATE INDEX AdditionalColumnUIDIndex ON AdditionalColumn([UID])
CREATE INDEX AdditionalColumnTypeUIDIndex ON AdditionalColumnType([UID])
CREATE INDEX PositionUIDIndex ON Position([UID])
CREATE INDEX EmployeeUIDIndex ON Employee([UID])
CREATE INDEX EmployeeReplacementUIDIndex ON EmployeeReplacement([UID])
CREATE INDEX PhoneUIDIndex ON Phone([UID])
CREATE INDEX DepartmentUIDIndex ON Department([UID])
CREATE INDEX DocumentUIDIndex ON [Document]([UID])
CREATE INDEX HolidayUIDIndex ON Holiday([UID])
CREATE INDEX JournalUIDIndex ON Journal([UID])
CREATE INDEX CardUIDIndex ON Card([UID])
CREATE INDEX CardDoorUIDIndex ON CardDoor([UID])
CREATE INDEX ScheduleZoneUIDIndex ON ScheduleZone([UID])
CREATE INDEX OrganisationUIDIndex ON Organisation([UID])
CREATE INDEX PhotoUIDIndex ON Photo([UID])
CREATE INDEX PassJournalUIDIndex ON PassJournal([UID])

ALTER TABLE [dbo].[AdditionalColumn]  WITH NOCHECK ADD  CONSTRAINT [FK_AdditionalColumn_Employee] FOREIGN KEY([EmployeeUID])
REFERENCES [dbo].[Employee] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[AdditionalColumn] NOCHECK CONSTRAINT [FK_AdditionalColumn_Employee]

ALTER TABLE [dbo].[AdditionalColumn]  WITH NOCHECK ADD  CONSTRAINT [FK_AdditionalColumn_AdditionalColumnType] FOREIGN KEY([AdditionalColumnTypeUID])
REFERENCES [dbo].[AdditionalColumnType] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[AdditionalColumn] NOCHECK CONSTRAINT [FK_AdditionalColumn_AdditionalColumnType] 

ALTER TABLE [dbo].[Day]  WITH NOCHECK ADD  CONSTRAINT [FK_Day_NamedInterval] FOREIGN KEY([NamedIntervalUid])
REFERENCES [dbo].[NamedInterval] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Day] NOCHECK CONSTRAINT [FK_Day_NamedInterval]

ALTER TABLE [dbo].[Day]  WITH NOCHECK ADD  CONSTRAINT [FK_Day_ScheduleScheme] FOREIGN KEY([ScheduleSchemeUid])
REFERENCES [dbo].[ScheduleScheme] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[Day] NOCHECK CONSTRAINT [FK_Day_ScheduleScheme]

ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD  CONSTRAINT [FK_Department_Department1] FOREIGN KEY([ParentDepartmentUid])
REFERENCES [dbo].[Department] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[Department] NOCHECK CONSTRAINT [FK_Department_Department1]

ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Department1] FOREIGN KEY([DepartmentUid])
REFERENCES [dbo].[Department] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Department1]

ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD  CONSTRAINT [FK_Department_Photo] FOREIGN KEY([PhotoUID])
REFERENCES [dbo].[Photo] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Department] NOCHECK CONSTRAINT [FK_Department_Photo]

ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Position] FOREIGN KEY([PositionUid])
REFERENCES [dbo].[Position] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Position]

ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Employee] FOREIGN KEY([EscortUID])
REFERENCES [dbo].Employee([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Employee]

ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Schedule] FOREIGN KEY([ScheduleUid])
REFERENCES [dbo].[Schedule] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Schedule]

ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Photo] FOREIGN KEY([PhotoUID])
REFERENCES [dbo].[Photo] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Photo]

ALTER TABLE [dbo].[Position]  WITH NOCHECK ADD  CONSTRAINT [FK_Position_Photo] FOREIGN KEY([PhotoUID])
REFERENCES [dbo].[Photo] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Position] NOCHECK CONSTRAINT [FK_Position_Photo]

ALTER TABLE [dbo].[EmployeeReplacement]  WITH NOCHECK ADD  CONSTRAINT [FK_EmployeeReplacement_Department] FOREIGN KEY([DepartmentUid])
REFERENCES [dbo].[Department] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[EmployeeReplacement] NOCHECK CONSTRAINT [FK_EmployeeReplacement_Department]

ALTER TABLE [dbo].[EmployeeReplacement]  WITH NOCHECK ADD  CONSTRAINT [FK_EmployeeReplacement_Employee] FOREIGN KEY([EmployeeUid])
REFERENCES [dbo].[Employee] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[EmployeeReplacement] NOCHECK CONSTRAINT [FK_EmployeeReplacement_Employee]

ALTER TABLE [dbo].[EmployeeReplacement]  WITH NOCHECK ADD CONSTRAINT [FK_EmployeeReplacement_Schedule] FOREIGN KEY([ScheduleUid])
REFERENCES [dbo].[Schedule] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[EmployeeReplacement] NOCHECK CONSTRAINT [FK_EmployeeReplacement_Schedule]

ALTER TABLE [dbo].[Interval]  WITH NOCHECK ADD  CONSTRAINT [FK_Interval_NamedInterval1] FOREIGN KEY([NamedIntervalUid])
REFERENCES [dbo].[NamedInterval] ([Uid])
ON UPDATE CASCADE
ON DELETE CASCADE
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Interval] NOCHECK CONSTRAINT [FK_Interval_NamedInterval1]

ALTER TABLE [dbo].[Phone]  WITH NOCHECK ADD  CONSTRAINT [FK_Phone_Department] FOREIGN KEY([DepartmentUid])
REFERENCES [dbo].[Department] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[Phone] NOCHECK CONSTRAINT [FK_Phone_Department]

ALTER TABLE [dbo].[ScheduleZone]  WITH NOCHECK ADD  CONSTRAINT [FK_ScheduleZone_Schedule] FOREIGN KEY([ScheduleUid])
REFERENCES [dbo].[Schedule] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[ScheduleZone] NOCHECK CONSTRAINT [FK_ScheduleZone_Schedule]

ALTER TABLE [dbo].[Schedule]  WITH NOCHECK ADD  CONSTRAINT [FK_Schedule_ScheduleScheme] FOREIGN KEY([ScheduleSchemeUid])
REFERENCES [dbo].[ScheduleScheme] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Schedule] NOCHECK CONSTRAINT [FK_Schedule_ScheduleScheme]

ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD  CONSTRAINT [FK_Department_Employee1] FOREIGN KEY([ContactEmployeeUid])
REFERENCES [dbo].[Employee] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[Department] NOCHECK CONSTRAINT [FK_Department_Employee1]

ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD  CONSTRAINT [FK_Department_Employee2] FOREIGN KEY([AttendantUid])
REFERENCES [dbo].[Employee] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[Department] NOCHECK CONSTRAINT [FK_Department_Employee2]

ALTER TABLE [dbo].[Journal]  WITH NOCHECK ADD  CONSTRAINT [FK_Journal_Card] FOREIGN KEY([ObjectUID])
REFERENCES [dbo].[Card] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Journal] NOCHECK CONSTRAINT [FK_Journal_Card]

ALTER TABLE [dbo].[Card]  WITH NOCHECK ADD  CONSTRAINT [FK_Card_Employee] FOREIGN KEY([EmployeeUid])
REFERENCES [dbo].[Employee] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Card] NOCHECK CONSTRAINT [FK_Card_Employee]

ALTER TABLE [dbo].[PendingCard]  WITH NOCHECK ADD  CONSTRAINT [FK_PendingCard_Card] FOREIGN KEY([CardUid])
REFERENCES [dbo].[Card] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[PendingCard] NOCHECK CONSTRAINT [FK_PendingCard_Card]

ALTER TABLE [dbo].[Card]  WITH NOCHECK ADD CONSTRAINT [FK_Card_AccessTemplate] FOREIGN KEY([AccessTemplateUid])
REFERENCES [dbo].[AccessTemplate] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Card] NOCHECK CONSTRAINT [FK_Card_AccessTemplate]

ALTER TABLE [dbo].[CardDoor]  WITH NOCHECK ADD  CONSTRAINT [FK_CardDoor_Card] FOREIGN KEY([ParentUid])
REFERENCES [dbo].[Card] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[CardDoor] NOCHECK CONSTRAINT [FK_CardDoor_Card]

ALTER TABLE [dbo].[CardDoor] WITH NOCHECK ADD  CONSTRAINT [FK_CardDoor_AccessTemplate] FOREIGN KEY([ParentUid])
REFERENCES [dbo].[AccessTemplate] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[CardDoor] NOCHECK CONSTRAINT [FK_CardDoor_AccessTemplate]

ALTER TABLE [dbo].[AdditionalColumnType]  WITH NOCHECK ADD CONSTRAINT [FK_AdditionalColumnType_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION
ALTER TABLE [dbo].[AdditionalColumnType] NOCHECK CONSTRAINT [FK_AdditionalColumnType_Organisation]

ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD CONSTRAINT [FK_Department_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Department] NOCHECK CONSTRAINT [FK_Department_Organisation]

ALTER TABLE [dbo].[Schedule]  WITH NOCHECK ADD CONSTRAINT [FK_Schedule_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Schedule] NOCHECK CONSTRAINT [FK_Schedule_Organisation]

ALTER TABLE [dbo].[Organisation]  WITH NOCHECK ADD  CONSTRAINT [FK_Organisation_Photo] FOREIGN KEY([PhotoUID])
REFERENCES [dbo].[Photo] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Organisation] NOCHECK CONSTRAINT [FK_Organisation_Photo]

ALTER TABLE [dbo].[ScheduleScheme]  WITH NOCHECK ADD CONSTRAINT [FK_ScheduleScheme_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[ScheduleScheme] NOCHECK CONSTRAINT [FK_ScheduleScheme_Organisation]

ALTER TABLE [dbo].[Position]  WITH NOCHECK ADD CONSTRAINT [FK_Position_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Position] NOCHECK CONSTRAINT [FK_Position_Organisation]

ALTER TABLE [dbo].[Phone]  WITH NOCHECK ADD CONSTRAINT [FK_Phone_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Phone] NOCHECK CONSTRAINT [FK_Phone_Organisation]

ALTER TABLE [dbo].[NamedInterval]  WITH NOCHECK ADD CONSTRAINT [FK_NamedInterval_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[NamedInterval] NOCHECK CONSTRAINT [FK_NamedInterval_Organisation]

ALTER TABLE [dbo].[Holiday]  WITH NOCHECK ADD CONSTRAINT [FK_Holiday_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Holiday] NOCHECK CONSTRAINT [FK_Holiday_Organisation]

ALTER TABLE [dbo].[EmployeeReplacement]  WITH NOCHECK ADD CONSTRAINT [FK_EmployeeReplacement_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[EmployeeReplacement] NOCHECK CONSTRAINT [FK_EmployeeReplacement_Organisation]

ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD CONSTRAINT [FK_Employee_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Organisation]

ALTER TABLE [dbo].[Document]  WITH NOCHECK ADD CONSTRAINT [FK_Document_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[Document] NOCHECK CONSTRAINT [FK_Document_Organisation]

ALTER TABLE [dbo].[AccessTemplate] WITH NOCHECK ADD CONSTRAINT [FK_AccessTemplate_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[AccessTemplate] NOCHECK CONSTRAINT [FK_AccessTemplate_Organisation]

ALTER TABLE [dbo].[OrganisationDoor] WITH NOCHECK ADD CONSTRAINT [FK_OrganisationDoor_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[OrganisationDoor] NOCHECK CONSTRAINT [FK_OrganisationDoor_Organisation]

ALTER TABLE [dbo].[OrganisationZone] WITH NOCHECK ADD CONSTRAINT [FK_OrganisationZone_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[OrganisationZone] NOCHECK CONSTRAINT [FK_OrganisationZone_Organisation]

ALTER TABLE [dbo].[OrganisationCardTemplate] WITH NOCHECK ADD CONSTRAINT [FK_OrganisationCardTemplate_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[OrganisationCardTemplate] NOCHECK CONSTRAINT [FK_OrganisationCardTemplate_Organisation]

ALTER TABLE [dbo].[GuardZone] WITH NOCHECK ADD CONSTRAINT [FK_GuardZone_Organisation] FOREIGN KEY([ParentUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[GuardZone] NOCHECK CONSTRAINT [FK_GuardZone_Organisation]

ALTER TABLE [dbo].[GuardZone] WITH NOCHECK ADD CONSTRAINT [FK_GuardZone_Employee] FOREIGN KEY([ParentUid])
REFERENCES [dbo].[Employee] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[GuardZone] NOCHECK CONSTRAINT [FK_GuardZone_Employee]

ALTER TABLE [dbo].[GuardZone] WITH NOCHECK ADD CONSTRAINT [FK_GuardZone_AccessTemplate] FOREIGN KEY([ParentUid])
REFERENCES [dbo].[AccessTemplate] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[GuardZone] NOCHECK CONSTRAINT [FK_GuardZone_AccessTemplate]

ALTER TABLE [dbo].[OrganisationUser] WITH NOCHECK ADD CONSTRAINT [FK_OrganisationUser_Organisation] FOREIGN KEY([OrganisationUid])
REFERENCES [dbo].[Organisation] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[OrganisationUser] NOCHECK CONSTRAINT [FK_OrganisationUser_Organisation]

ALTER TABLE [dbo].[AdditionalColumn]  WITH NOCHECK ADD  CONSTRAINT [FK_AdditionalColumn_Photo] FOREIGN KEY([PhotoUID])
REFERENCES [dbo].[Photo] ([Uid])
NOT FOR REPLICATION 
ALTER TABLE [dbo].[AdditionalColumn] NOCHECK CONSTRAINT [FK_AdditionalColumn_Photo]

ALTER TABLE [dbo].[PassJournal]  WITH NOCHECK ADD  CONSTRAINT [FK_PassJournal_Employee] FOREIGN KEY([EmployeeUID])
REFERENCES [dbo].[Employee] ([UID])
ON UPDATE CASCADE
ON DELETE CASCADE
NOT FOR REPLICATION 
ALTER TABLE [dbo].[PassJournal] CHECK CONSTRAINT [FK_PassJournal_Employee]

INSERT INTO Patches (Id) VALUES
	('OrganisationUser')
INSERT INTO Patches (Id) VALUES
	('AlterPatches')
INSERT INTO Patches (Id) VALUES
	('GuardZone')
INSERT INTO Patches (Id) VALUES
	('Doors')
INSERT INTO Patches (Id) VALUES
	('OrganisationZone')
INSERT INTO Patches (Id) VALUES
	('DoorsEnterExit')
INSERT INTO Patches (Id) VALUES
	('PendingCard')
INSERT INTO Patches (Id) VALUES
	('CommonJournal')
INSERT INTO Patches (Id) VALUES
	('PassJournal')
INSERT INTO Patches (Id) VALUES
	('PendingCardControllerUID')
INSERT INTO Patches (Id) VALUES
	('OrganisationCardTemplate')
INSERT INTO Patches (Id) VALUES
	('RemoveCardSeries')
INSERT INTO Patches (Id) VALUES
	('RemoveJournalCardSeries')
INSERT INTO Patches (Id) VALUES
	('EventNamesDescription')
INSERT INTO Patches (Id) VALUES
	('DoorsEnterExitUIDtoID')
	INSERT INTO Patches (Id) VALUES
	('DoorsExitUIDtoID')

DECLARE @OrgUid uniqueidentifier;
SET @OrgUid = NEWID();
INSERT INTO Organisation ([Uid],[Name],IsDeleted,RemovalDate) VALUES (@OrgUid,'Организация',0,'01/01/1900')

DECLARE @Uid uniqueidentifier;
SET @Uid = NEWID();
INSERT INTO OrganisationUser ([Uid],[UserUID],[OrganisationUID]) VALUES (@Uid,'10e591fb-e017-442d-b176-f05756d984bb',@OrgUid)

SET NOEXEC OFF

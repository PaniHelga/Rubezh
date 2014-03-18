USE [SKUD]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[AdditionalColumn]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalColumn_Employee] FOREIGN KEY([EmployeeUID])
REFERENCES [dbo].[Employee] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[AdditionalColumn] CHECK CONSTRAINT [FK_AdditionalColumn_Employee]

GO
ALTER TABLE [dbo].[AdditionalColumn]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalColumn_AdditionalColumnType] FOREIGN KEY([AdditionalColumnTypeUID])
REFERENCES [dbo].[AdditionalColumnType] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[AdditionalColumn] CHECK CONSTRAINT [FK_AdditionalColumn_AdditionalColumnType] 

GO
ALTER TABLE [dbo].[Day]  WITH NOCHECK ADD  CONSTRAINT [FK_Day_NamedInterval] FOREIGN KEY([NamedIntervalUid])
REFERENCES [dbo].[NamedInterval] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Day] NOCHECK CONSTRAINT [FK_Day_NamedInterval]

GO
ALTER TABLE [dbo].[Day]  WITH CHECK ADD  CONSTRAINT [FK_Day_ScheduleScheme] FOREIGN KEY([ScheduleSchemeUid])
REFERENCES [dbo].[ScheduleScheme] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[Day] CHECK CONSTRAINT [FK_Day_ScheduleScheme]
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_Department_Department1] FOREIGN KEY([ParentDepartmentUid])
REFERENCES [dbo].[Department] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Department1]
GO
ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Department1] FOREIGN KEY([DepartmentUid])
REFERENCES [dbo].[Department] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD  CONSTRAINT [FK_Department_Photo] FOREIGN KEY([PhotoUID])
REFERENCES [dbo].[Photo] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Photo]
GO
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Department1]
GO
ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Position] FOREIGN KEY([PositionUid])
REFERENCES [dbo].[Position] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Position]
GO
ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Schedule] FOREIGN KEY([ScheduleUid])
REFERENCES [dbo].[Schedule] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Schedule]
GO
ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Photo] FOREIGN KEY([PhotoUID])
REFERENCES [dbo].[Photo] ([Uid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Photo]
GO
ALTER TABLE [dbo].[EmployeeReplacement]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeReplacement_Department] FOREIGN KEY([DepartmentUid])
REFERENCES [dbo].[Department] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[EmployeeReplacement] CHECK CONSTRAINT [FK_EmployeeReplacement_Department]

GO
ALTER TABLE [dbo].[EmployeeReplacement]  WITH NOCHECK ADD  CONSTRAINT [FK_EmployeeReplacement_Employee] FOREIGN KEY([EmployeeUid])
REFERENCES [dbo].[Employee] ([Uid])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[EmployeeReplacement] NOCHECK CONSTRAINT [FK_EmployeeReplacement_Employee]

GO
ALTER TABLE [dbo].[EmployeeReplacement]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeReplacement_Schedule] FOREIGN KEY([ScheduleUid])
REFERENCES [dbo].[Schedule] ([Uid])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[EmployeeReplacement] CHECK CONSTRAINT [FK_EmployeeReplacement_Schedule]
GO
ALTER TABLE [dbo].[Interval]  WITH NOCHECK ADD  CONSTRAINT [FK_Interval_NamedInterval1] FOREIGN KEY([NamedIntervalUid])
REFERENCES [dbo].[NamedInterval] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Interval] NOCHECK CONSTRAINT [FK_Interval_NamedInterval1]
GO
ALTER TABLE [dbo].[Phone]  WITH CHECK ADD  CONSTRAINT [FK_Phone_Department] FOREIGN KEY([DepartmentUid])
REFERENCES [dbo].[Department] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[Phone] CHECK CONSTRAINT [FK_Phone_Department]
GO
ALTER TABLE [dbo].[ScheduleZoneLink]  WITH NOCHECK ADD  CONSTRAINT [FK_ScheduleZoneLink_Schedule] FOREIGN KEY([ScheduleUid])
REFERENCES [dbo].[Schedule] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ScheduleZoneLink] NOCHECK CONSTRAINT [FK_ScheduleZoneLink_Schedule]
GO
ALTER TABLE [dbo].[Schedule]  WITH NOCHECK ADD  CONSTRAINT [FK_Schedule_ScheduleScheme] FOREIGN KEY([ScheduleSchemeUid])
REFERENCES [dbo].[ScheduleScheme] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Schedule] NOCHECK CONSTRAINT [FK_Schedule_ScheduleScheme]
GO
ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD  CONSTRAINT [FK_Department_Employee1] FOREIGN KEY([ContactEmployeeUid])
REFERENCES [dbo].[Employee] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[Department] NOCHECK CONSTRAINT [FK_Department_Employee1]
GO
ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD  CONSTRAINT [FK_Department_Employee2] FOREIGN KEY([AttendantUid])
REFERENCES [dbo].[Employee] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[Department] NOCHECK CONSTRAINT [FK_Department_Employee2]
GO
ALTER TABLE [dbo].[Journal]  WITH NOCHECK ADD  CONSTRAINT [FK_Journal_Card] FOREIGN KEY([CardUid])
REFERENCES [dbo].[Card] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Journal] NOCHECK CONSTRAINT [FK_Journal_Card]
GO
ALTER TABLE [dbo].[Card]  WITH NOCHECK ADD  CONSTRAINT [FK_Card_Employee] FOREIGN KEY([EmployeeUid])
REFERENCES [dbo].[Employee] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Card] NOCHECK CONSTRAINT [FK_Card_Employee]
GO
ALTER TABLE [dbo].[Card]  WITH NOCHECK ADD  CONSTRAINT [FK_Card_GUD] FOREIGN KEY([GUDUid])
REFERENCES [dbo].[GUD] ([Uid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Card] NOCHECK CONSTRAINT [FK_Card_GUD]
GO
ALTER TABLE [dbo].[CardZoneLink]  WITH NOCHECK ADD  CONSTRAINT [FK_CardZoneLink_Card] FOREIGN KEY([ParentUid])
REFERENCES [dbo].[Card] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[CardZoneLink] NOCHECK CONSTRAINT [FK_CardZoneLink_Card]
GO
ALTER TABLE [dbo].[CardZoneLink]  WITH NOCHECK ADD  CONSTRAINT [FK_CardZoneLink_GUD] FOREIGN KEY([ParentUid])
REFERENCES [dbo].[GUD] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[CardZoneLink] NOCHECK CONSTRAINT [FK_CardZoneLink_GUD]
GO
ALTER TABLE [dbo].[AdditionalColumnType]  WITH NOCHECK ADD CONSTRAINT [FK_AdditionalColumnType_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[AdditionalColumnType] NOCHECK CONSTRAINT [FK_AdditionalColumnType_Organization]
GO
ALTER TABLE [dbo].[Day]  WITH NOCHECK ADD CONSTRAINT [FK_Day_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Day] NOCHECK CONSTRAINT [FK_Day_Organization]
GO
ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD CONSTRAINT [FK_Department_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Department] NOCHECK CONSTRAINT [FK_Department_Organization]
GO
ALTER TABLE [dbo].[Schedule]  WITH NOCHECK ADD CONSTRAINT [FK_Schedule_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Schedule] NOCHECK CONSTRAINT [FK_Schedule_Organization]
GO
ALTER TABLE [dbo].[Organization]  WITH NOCHECK ADD  CONSTRAINT [FK_Organization_Photo] FOREIGN KEY([PhotoUID])
REFERENCES [dbo].[Photo] ([Uid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Organization] CHECK CONSTRAINT [FK_Organization_Photo]
GO
ALTER TABLE [dbo].[ScheduleScheme]  WITH NOCHECK ADD CONSTRAINT [FK_ScheduleScheme_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ScheduleScheme] NOCHECK CONSTRAINT [FK_ScheduleScheme_Organization]
GO
ALTER TABLE [dbo].[Position]  WITH NOCHECK ADD CONSTRAINT [FK_Position_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Position] NOCHECK CONSTRAINT [FK_Position_Organization]
GO
ALTER TABLE [dbo].[Phone]  WITH NOCHECK ADD CONSTRAINT [FK_Phone_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Phone] NOCHECK CONSTRAINT [FK_Phone_Organization]
GO
ALTER TABLE [dbo].[NamedInterval]  WITH NOCHECK ADD CONSTRAINT [FK_NamedInterval_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[NamedInterval] NOCHECK CONSTRAINT [FK_NamedInterval_Organization]
GO
ALTER TABLE [dbo].[Holiday]  WITH NOCHECK ADD CONSTRAINT [FK_Holiday_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Holiday] NOCHECK CONSTRAINT [FK_Holiday_Organization]
GO
ALTER TABLE [dbo].[EmployeeReplacement]  WITH NOCHECK ADD CONSTRAINT [FK_EmployeeReplacement_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmployeeReplacement] NOCHECK CONSTRAINT [FK_EmployeeReplacement_Organization]
GO
ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD CONSTRAINT [FK_Employee_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE NO ACTION
ON DELETE NO ACTION
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Organization]
GO
ALTER TABLE [dbo].[Document]  WITH NOCHECK ADD CONSTRAINT [FK_Document_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Document] NOCHECK CONSTRAINT [FK_Document_Organization]
GO
ALTER TABLE [dbo].[GUD] WITH NOCHECK ADD CONSTRAINT [FK_GUD_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
ON UPDATE SET NULL
ON DELETE SET NULL
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GUD] NOCHECK CONSTRAINT [FK_GUD_Organization]
GO
ALTER TABLE [dbo].[OrganizationZone] WITH NOCHECK ADD CONSTRAINT [FK_OrganizationZone_Organization] FOREIGN KEY([OrganizationUid])
REFERENCES [dbo].[Organization] ([Uid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[OrganizationZone] NOCHECK CONSTRAINT [FK_OrganizationZone_Organization]

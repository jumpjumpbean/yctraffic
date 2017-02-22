use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzZdGroup') drop table ZdtzZdGroup
go

create table ZdtzZdGroup(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Title nvarchar(31),
	GroupName nvarchar(255),
	PersonId1 int,
	PersonName1 nvarchar(31),
	PersonId2 int,
	PersonName2 nvarchar(31),
	PersonId3 int,
	PersonName3 nvarchar(31),
	PersonId4 int,
	PersonName4 nvarchar(31),
	BackupId1 int,
	BackupName1 nvarchar(31),
	BackupId2 int,
	BackupName2 nvarchar(31),
	BackupId3 int,
	BackupName3 nvarchar(31),
	BackupId4 int,
	BackupName4 nvarchar(31),
	Memo nvarchar(255),
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(255),
	StrSpare2 nvarchar(255),
	IsDeleted int NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateTime datetime
	)







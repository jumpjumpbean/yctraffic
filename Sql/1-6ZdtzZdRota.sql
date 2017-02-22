use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzZdRota') drop table ZdtzZdRota
go

create table ZdtzZdRota(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Title nvarchar(31),
	DutyDate datetime,
	DutyPerson1Id int,
	DutyPerson1Name nvarchar(31),
	DutyPerson2Id int,
	DutyPerson2Name nvarchar(31),
	DutyPerson3Id int,
	DutyPerson3Name nvarchar(31),
	DutyPerson4Id int,
	DutyPerson4Name nvarchar(31),
	DutyPerson5Id int,
	DutyPerson5Name nvarchar(31),
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








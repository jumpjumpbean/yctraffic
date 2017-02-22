use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwSectionAssign') drop table ZdtzYwSectionAssign
go

create table ZdtzYwSectionAssign(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Title nvarchar(31),
	LeaderId int,
	LeaderName nvarchar(31),
	PoliceId int,
	PoliceName nvarchar(31),
	RoadSection nvarchar(255),
	Village nvarchar(255),
	School nvarchar(255),
	Company nvarchar(255),
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

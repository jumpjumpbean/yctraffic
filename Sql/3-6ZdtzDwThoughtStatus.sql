use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzDwThoughtStatus') drop table ZdtzDwThoughtStatus
go

create table ZdtzDwThoughtStatus(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Members nvarchar(100),
	Issues nvarchar(200),
	Action nvarchar(200),
	Leader nvarchar(31),
	Recorder nvarchar(31),
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

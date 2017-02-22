use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzZdPatrolRange') drop table ZdtzZdPatrolRange
go

create table ZdtzZdPatrolRange(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Title nvarchar(31),
	PatrolRange nvarchar(2000),
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

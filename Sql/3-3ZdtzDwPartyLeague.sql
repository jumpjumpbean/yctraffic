use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzDwPartyLeague') drop table ZdtzDwPartyLeague
go

create table ZdtzDwPartyLeague(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Location nvarchar(31),
	Members nvarchar(100),
	Content nvarchar(300),
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


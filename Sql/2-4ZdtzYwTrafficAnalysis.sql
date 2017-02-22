use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwTrafficAnalysis') drop table ZdtzYwTrafficAnalysis
go

create table ZdtzYwTrafficAnalysis(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordTime datetime,
	AccidentCount int,
	IllegalActCount int,
	AccidentDescription nvarchar(255),
	IllegalActDescription nvarchar(255),
	Prevention nvarchar(300),
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

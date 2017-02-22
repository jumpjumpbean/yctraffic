use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwDriver') drop table ZdtzYwDriver
go

create table ZdtzYwDriver(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordDate datetime,
	PlateCategory nvarchar(31),
	PlateNumber nvarchar(31),
	Owner nvarchar(31),
	Address nvarchar(63),
	CarState nvarchar(31),
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

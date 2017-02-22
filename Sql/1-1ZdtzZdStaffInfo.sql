use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzZdStaffInfo') drop table ZdtzZdStaffInfo
go

create table ZdtzZdStaffInfo(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Title nvarchar(255),
	Name nvarchar(31),
	PoliceNo nvarchar(31),
	Telephone nvarchar(15),
	Address nvarchar(255),
	Degree nvarchar(31),
	IdNo nvarchar(31),
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


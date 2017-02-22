use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzZdRoster') drop table ZdtzZdRoster
go

create table ZdtzZdRoster(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Title nvarchar(255),
	CheckAttendanceDate datetime,
	StaffId int,
	Name nvarchar(31),
	Memo nvarchar(255),
	IsAttended int,
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





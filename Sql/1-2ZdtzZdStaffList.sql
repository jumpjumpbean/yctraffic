use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzZdStaffList') drop table ZdtzZdStaffList
go

create table ZdtzZdStaffList(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Title nvarchar(255),
	No int,
	Name nvarchar(31),
	Position nvarchar(31),
	Job nvarchar(255),
	Memo nvarchar(255),
	IsDeleted int NOT NULL DEFAULT 0,
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(255),
	StrSpare2 nvarchar(255),
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateTime datetime
	)




use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwSchoolCar') drop table ZdtzYwSchoolCar
go

create table ZdtzYwSchoolCar(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordTime datetime,
	SchoolName nvarchar(31),
	OwnerName nvarchar(31),
	CarNo nvarchar(31),
	DriverName nvarchar(31),
	LicenseNumber nvarchar(31),
	CarryNumber int,
	CarFlag int,
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




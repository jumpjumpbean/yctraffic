use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzZdCar') drop table ZdtzZdCar
go

create table ZdtzZdCar(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	CarNo nvarchar(255),
	Miles int,
	MaintenanceDate  datetime,
	NextMaintenanceDate  datetime,
	OwnerId int,
	OwnerName nvarchar(31),
	Memo nvarchar(255),
	CarRules image,
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






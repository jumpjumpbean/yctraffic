use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwKeyVehicle') drop table ZdtzYwKeyVehicle
go

create table ZdtzYwKeyVehicle(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordTime datetime,
	VehicleNumber nvarchar(31),
	RegisterDate datetime,
	VehicleType nvarchar(31),
	Brand nvarchar(31),
	Color nvarchar(31),
	CarryNumber int,
	OperatingInterval nvarchar(31),
	Picture image,
	CheckRecord1 nvarchar(31),
	CheckRecord2 nvarchar(31),
	CheckRecord3 nvarchar(31),
	CheckRecord4 nvarchar(31),
	CheckRecord5 nvarchar(31),
	CheckRecord6 nvarchar(31),
	CheckRecord7 nvarchar(31),
	CheckRecord8 nvarchar(31),
	CheckRecord9 nvarchar(31),
	CheckRecord10 nvarchar(31),
	CheckRecord11 nvarchar(31),
	CheckRecord12 nvarchar(31),
	CheckRecord13 nvarchar(31),
	DriverName nvarchar(31),
	QuasiDrivingType nvarchar(31),
	IDNo nvarchar(31),
	IDReceivedDate datetime,
	PhoneNumber nvarchar(31),
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



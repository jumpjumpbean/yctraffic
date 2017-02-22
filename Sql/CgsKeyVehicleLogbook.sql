use yctrafficdb
go

if exists(select * from sysobjects where name='CgsKeyVehicleLogbook') drop table CgsKeyVehicleLogbook
go

create table CgsKeyVehicleLogbook(
	Id int not null primary key identity(1,1),
	PlateType nvarchar(20),
	PlateNumber nvarchar(20),
	VehicleBrand nvarchar(20),
	VehicleType nvarchar(30),
	InitialRegistration datetime,
	ValidityDate datetime,
	ForceEliminateDate datetime,
	OperationType nvarchar(20),
	VehicleStatus nvarchar(20),
	IsAttached   nvarchar(5),
	DriverName    nvarchar(20),
	ServicedSchool nvarchar(30),
	Changes  nvarchar(30),
	OwnerDepartment   nvarchar(20),
	Remark    nvarchar(50),
	IsDeleted bit NOT NULL DEFAULT 0,
	IntSpare1 int,
	StrSpare1 nvarchar(255),
	StrSpare2 nvarchar(255),
	ApprovalName nvarchar(20),
	ApprovalTime datetime,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime
)


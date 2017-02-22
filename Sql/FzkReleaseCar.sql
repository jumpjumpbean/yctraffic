use yctrafficdb
go

if exists(select * from sysobjects where name='FzkReleaseCar') drop table FzkReleaseCar
go

create table FzkReleaseCar(
	Id int not null primary key identity(1,1),
	IdNumber nvarchar(20),
	PlateNumber nvarchar(20),
	VehicleBrand nvarchar(20),
	VehicleColor nvarchar(10),
	VehicleType nvarchar(30),
	ParkTime datetime,
	ReleaseTime datetime,
	ForceEliminateDate datetime,
	SendCarDepartment nvarchar(20),
	SendCarPerson nvarchar(20),
	PersonSignature nvarchar(20),
	ChargeId nvarchar(20),
	ChargeSignature nvarchar(20),
	IsPaidFee   nvarchar(5),
	DriverName    nvarchar(20),
	Changes  nvarchar(30),
	ApproveResult nvarchar(30),
	OwnerDepartment   nvarchar(20),
	Remark    nvarchar(50),
	IsDeleted bit NOT NULL DEFAULT 0,
	IsChargeSigned bit NOT NULL DEFAULT 0,
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


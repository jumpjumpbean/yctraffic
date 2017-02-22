use yctrafficdb
go

if exists(select * from sysobjects where name='CgsYellowMarkCar') drop table CgsYellowMarkCar
go

create table CgsYellowMarkCar(
	Id int not null primary key identity(1,1),
	PlateType nvarchar(20),
	PlateNumber nvarchar(20),
	VehicleStatus nvarchar(20),
	VehicleIDCode nvarchar(30),
	VehicleType nvarchar(30),
	FuelType nvarchar(20),
	InitialRegistration datetime,
	OwnerName    nvarchar(20),
	Division    nvarchar(30),
	Address   nvarchar(50),
	Telephone    nvarchar(20),
	Cellphone    nvarchar(50),
	OwnershipOfLand    nvarchar(30),
	UsageType   nvarchar(30),
	ValidityDate datetime,
	IsPoliticsCar   nvarchar(10),
	IsOtherGovCar   nvarchar(10),
	OverdueCnt		int,
	EliminateType   nvarchar(20),
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


use yctrafficdb
go

if exists(select * from sysobjects where name='SgkReleaseCar') drop table SgkReleaseCar
go

create table SgkReleaseCar(
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
	Node1Comment nvarchar(20),
	Node2Comment nvarchar(20),
	Node3Comment nvarchar(20),
	Node4Comment nvarchar(20),
	IsPaidFee   nvarchar(5),
	DriverName    nvarchar(20),
	Changes  nvarchar(30),
	ApproveResult nvarchar(30),
	OwnerDepartment   nvarchar(20),
	Remark    nvarchar(50),
	SubLeader1Id int NOT NULL DEFAULT 0,
	SubLeader1Name nvarchar(31),
	IsSubLeader1Signed bit NOT NULL DEFAULT 0,
	SubLeader2Id int NOT NULL DEFAULT 0,
	SubLeader2Name nvarchar(31),
	IsSubLeader2Signed bit NOT NULL DEFAULT 0,
	SubLeader3Id int NOT NULL DEFAULT 0,
	SubLeader3Name nvarchar(31),
	IsSubLeader3Signed bit NOT NULL DEFAULT 0,
	IsDeleted bit NOT NULL DEFAULT 0,
	IsChargeSigned bit NOT NULL DEFAULT 0,
	IntSpare1 int NOT NULL DEFAULT 0,
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


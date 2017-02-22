use yctrafficdb 
go 

if exists(select * from sysobjects where name='FzkChangeMeasure') drop table FzkChangeMeasure
go

create table FzkChangeMeasure(
	Id int not null primary key identity(1,1),
	Name nvarchar(10),
	Address nvarchar(205),
	Telephone nvarchar(20),
	OtherPhone nvarchar(20),
	DriverLicenseNo nvarchar(50),
	DriverLicenseArchive nvarchar(50),
	VehicleNo nvarchar(15),	
	VehicleType nvarchar(15),
	PunishTime datetime,
	PunishLocation nvarchar(105),
	PunishReason nvarchar(205),
	IsDetainVehicle		int,
	IsDetainNonVehicle		int,
	IsDetainDriverLicense		int,
	IsDetainIllegalDevice int,
	ReportPolice nvarchar(10),
	Remake nvarchar(75),
	ApproveResult nvarchar(30),
	ApprovalName nvarchar(20),
	ApprovalTime datetime,
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(51),
	StrSpare2 nvarchar(51),
	StrSpare3 nvarchar(51),
	StrSpare4 nvarchar(51),
	IsDeleted bit NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime
	)


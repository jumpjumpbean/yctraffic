use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwDUIRectification') drop table ZdtzYwDUIRectification
go

create table ZdtzYwDUIRectification(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RequsetDate date,
	ReaqustDepartment nvarchar(63),
	Action int,
	SendPolice int,
	CrossCheckTimes int,
	PropagandaTimes int,
	NoDriveLicense int,
	CarryOversize int,
	NoPlate int,
	ForgedPlate int,
	OtherPlate int,
	OtherPlateIllegal int,
	Changeable int,
	BusCheck int,
	DangerousCheck int,
	LargeCarCheck int,
	SchoolCarCheck int,
	ThreeCheck int,
	ThreeForbidden int,
	ThreeIllegal int,
	RecorderId int,
	RecorderName nvarchar(31),
	RecorderPhone nvarchar(31),
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

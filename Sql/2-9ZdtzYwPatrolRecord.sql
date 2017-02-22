use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwPatrolRecord') drop table ZdtzYwPatrolRecord
go

create table ZdtzYwPatrolRecord(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordDate datetime,
	ChargePoliceId int,
	ChargePoliceName nvarchar(31),
	DayShiftCar nvarchar(31),
	NightShiftCar nvarchar(31),
	DutyPoliceId int,
	DutyPoliceName nvarchar(31),
	RoadSection nvarchar(31),
	WorkRecord nvarchar(500),
	SquadronOwnerId int,
	SquadronOwnerName nvarchar(31),
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

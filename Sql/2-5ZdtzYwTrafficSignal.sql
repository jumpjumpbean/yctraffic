use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwTrafficSignal') drop table ZdtzYwTrafficSignal
go

create table ZdtzYwTrafficSignal(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordTime datetime,
	Number int,
	Location nvarchar(50),
	EquipmentType nvarchar(50),
	Status nvarchar(31),
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


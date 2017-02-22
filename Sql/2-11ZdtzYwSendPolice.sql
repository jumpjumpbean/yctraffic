use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwSendPolice') drop table ZdtzYwSendPolice
go

create table ZdtzYwSendPolice(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordDate datetime,
	CallPoliceTime datetime,
	ReceivePoliceId int,
	ReceivePoliceName nvarchar(31),
	Informant nvarchar(31),
	Address nvarchar(63),
	CaseSummary nvarchar(500),
	SendPolice nvarchar(31),
	SendCar nvarchar(31),
	Result nvarchar(255),
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

use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwOrder') drop table ZdtzYwOrder
go

create table ZdtzYwOrder(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordDate datetime,
	OrderTime datetime,
	Name nvarchar(31),
	PhoneNumber nvarchar(31),
	Address nvarchar(63),
	Reason nvarchar(63),
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

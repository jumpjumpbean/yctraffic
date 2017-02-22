use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZdtzCyPunish') drop table ZdtzCyPunish
go

create table ZdtzCyPunish(
	Id int not null primary key identity(1,1),
	ConfigId int,
	Title nvarchar(50),
	PatrolDate datetime,
	OwnDepartmentId int,
	OwnDepartmentName nvarchar(75),	
	CommonProcedure int,
	SimpleProcedure int,
	Remark nvarchar(250),
	PunishFile nvarchar(75),
	PunishFileName nvarchar(100),
	IsDeleted int,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime,
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(31),
	StrSpare2 nvarchar(31)
	);

use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZdtzCyPatrol') drop table ZdtzCyPatrol
go

create table ZdtzCyPatrol(
	Id int not null primary key identity(1,1),
	ConfigId int,
	Title nvarchar(50),
	Content nvarchar(500),
	PatrolDate datetime,
	OwnDepartmentId int,
	OwnDepartmentName nvarchar(75),
	PatrolMiles int,
	PatrolFile nvarchar(75),
	PatrolFileName nvarchar(100),
	Comments nvarchar(255),
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

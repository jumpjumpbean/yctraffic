use yctrafficdb 
go 

if exists(select * from sysobjects where name='HealthArchiveTable') drop table HealthArchiveTable
go 

create table HealthArchiveTable
(
	Id  					int not null primary key identity(1,1),	
	Name					nvarchar(20),
	PoliceNumber			nvarchar(50),
	IdCardNumber			nvarchar(50),	
	CheckTime				datetime,
	CheckResult				nvarchar(305),
	CheckFile				nvarchar(75),
	CheckFileThumb			nvarchar(75),
	CheckFileName			nvarchar(100),
	DepartmentId			int,
	DepartmentCode			nvarchar(20),
	DepartmentName			nvarchar(20),
	CreateUserId			int,
	CreateUserName			nvarchar(20),
	CreateTime				datetime, 
	UpdaterId				int,
	UpdaterName				nvarchar(20),
	UpdateTime				datetime,
	IntSpare1				int,
	IntSpare2				int,
	StrSpare1				nvarchar(31),
	StrSpare2				nvarchar(31),
	IsDeleted				bit NOT NULL DEFAULT 0
)

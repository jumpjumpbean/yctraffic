use yctrafficdb 
go 

if exists(select * from sysobjects where name='CgsVehicleAdminCase') drop table CgsVehicleAdminCase
go

create table CgsVehicleAdminCase(
	Id int not null primary key identity(1,1),
	CaseIndex nvarchar(50),
	CaseTime datetime,
	CaseName nvarchar(75),
	Title nvarchar(30),
	FineNumber int NOT NULL DEFAULT 0,
	PunishedName nvarchar(20),
	PunishedType nvarchar(20),
	AttachmentName1 nvarchar(40),
	AttachmentPath1 nvarchar(75),
	AttachmentName2 nvarchar(40),
	AttachmentPath2 nvarchar(75),
	Comments nvarchar(200),
	OwnDepartmentId int,
	OwnDepartmentName nvarchar(75),
	FilePath1 nvarchar(75),
	FileName1 nvarchar(100),	
	FilePath2 nvarchar(75),
	FileName2 nvarchar(100),	
	Status int NOT NULL DEFAULT 0,
	StatusName nvarchar(50),
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(75),
	StrSpare2 nvarchar(75),
	IsDeleted bit NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime
	)

use yctrafficdb 
go 

if exists(select * from sysobjects where name='FzkConsultation') drop table FzkConsultation
go

create table FzkConsultation(
	Id int not null primary key identity(1,1),
	ConfigId int,
	ConsultTitle nvarchar(105),
	ConsultDate datetime,
	ConsultLocation nvarchar(105),
	LeaderDepartment nvarchar(105),
	ConsultHosts nvarchar(30),
	RelatedDepartment nvarchar(105),
	ConsultComments nvarchar(250),
	ConsultResolution nvarchar(250),
	Signatures nvarchar(100),
	UploadFileName nvarchar(255),
	OwnDepartmentId int,
	OwnDepartmentName nvarchar(75),	
	Remake nvarchar(75),
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(31),
	StrSpare2 nvarchar(31),
	IsDeleted bit NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime
	)


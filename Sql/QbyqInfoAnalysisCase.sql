use yctrafficdb 
go 

if exists(select * from sysobjects where name='QbyqInfoAnalysisCase') drop table QbyqInfoAnalysisCase
go

create table QbyqInfoAnalysisCase(
	Id int not null primary key identity(1,1),
	CaseIndex nvarchar(50),
	CaseTime datetime,
	CaseName nvarchar(75),
	AttachmentName nvarchar(40),
	AttachmentPath nvarchar(75),
	Comments nvarchar(200),
	OwnDepartmentId int,
	OwnDepartmentName nvarchar(75),
	FilePath nvarchar(75),
	FileName nvarchar(100),	
	Status int NOT NULL DEFAULT 0,
	StatusName nvarchar(50),
	IsDeleted bit NOT NULL DEFAULT 0,
	IsRead1 bit NOT NULL DEFAULT 0,
	IsRead2 bit NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime,
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(75),
	StrSpare2 nvarchar(75)
	)

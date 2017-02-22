use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZgxcAssistantCheckin') drop table ZgxcAssistantCheckin
go

create table ZgxcAssistantCheckin(
	Id int not null primary key identity(1,1),
	ConfigId int,
	Title nvarchar(105),
	RecordDate date,
	Content nvarchar(75),
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


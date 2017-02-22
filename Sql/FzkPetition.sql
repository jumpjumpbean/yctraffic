use yctrafficdb 
go 

if exists(select * from sysobjects where name='FzkPetition') drop table FzkPetition
go

create table FzkPetition(
	Id int not null primary key identity(1,1),
	PetitionName nvarchar(30),
	PetitionTime datetime,
	Gender nvarchar(10),
	FamilyAddress nvarchar(150),
	PetitionType nvarchar(30),
	ContactInfo nvarchar(50),
	Content nvarchar(300),
	Results nvarchar(100),
	AttachmentName nvarchar(40),
	AttachmentPath nvarchar(75),
	FilePath nvarchar(75),
	FileName nvarchar(100),	
	Remark nvarchar(50),
	OwnDepartmentId int,
	OwnDepartmentName nvarchar(75),
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(75),
	StrSpare2 nvarchar(75),
	IsDeleted bit NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CheckId int,
	CheckName nvarchar(31),
	CreateTime datetime
	)

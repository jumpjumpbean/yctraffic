use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZgxcPersonnelChange') drop table ZgxcPersonnelChange
go

create table ZgxcPersonnelChange(
	Id int not null primary key identity(1,1),
	PersonName nvarchar(20),
	PersonDepartmentId int,
	PersonDepartmentName nvarchar(75),
	PersonStatus nvarchar(10),
	ChangeReason nvarchar(30),
	ApprovalComments nvarchar(100),
    ApplyPerson nvarchar(20),
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime,
	RecordStatus nvarchar(20),
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
	)


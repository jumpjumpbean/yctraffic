use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZhzxTotalViolation') drop table ZhzxTotalViolation
go

create table ZhzxTotalViolation(
	Id int not null primary key identity(1,1),
	LicensePlateNumber nvarchar(31),
	CheckpointName nvarchar(63),
	ViolationCount int,
	EarliestViolation datetime,
	LatestViolation datetime,
	Remark nvarchar(50),	
	IsDeleted bit NOT NULL DEFAULT 0,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime,
	IntSpare1 int,
	StrSpare1 nvarchar(75)
	)

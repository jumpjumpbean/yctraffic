use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZhzxElectronMonitor') drop table ZhzxElectronMonitor
go

create table ZhzxElectronMonitor(
	Id int not null primary key identity(1,1),
	CheckpointName nvarchar(63),
	SerialNumber nvarchar(30),
	Status nvarchar(30),
	Remark nvarchar(100),	
	IsDeleted bit NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateName nvarchar(31),
	UpdateTime datetime,
	IntSpare1 int,
	StrSpare1 nvarchar(75)
	)

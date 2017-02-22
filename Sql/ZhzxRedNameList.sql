use yctrafficdb
go

if exists(select * from sysobjects where name='ZhzxRedNameList') drop table ZhzxRedNameList
go

create table ZhzxRedNameList(
	Id int not null primary key identity(1,1),
	LicensePlateNumber  nvarchar(31),
	Comment nvarchar(255),
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(255),
	StrSpare2 nvarchar(255),
	IsDeleted int NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateTime datetime
)

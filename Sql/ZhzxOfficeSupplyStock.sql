use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZhzxOfficeSupplyStock') drop table ZhzxOfficeSupplyStock
go

create table ZhzxOfficeSupplyStock(
	Id int not null primary key identity(1,1),
	RecordTime datetime,
	ItemName nvarchar(15),
	Specification nvarchar(10),
	Unit nvarchar(10),
	ImportAmount nvarchar(10),
	ExportAmount nvarchar(10),
	ExistingAmount nvarchar(10),
	Remark nvarchar(100),	
	IsDeleted bit NOT NULL DEFAULT 0,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime,
	UpdaterId int,
	UpdateName nvarchar(31),
	UpdateTime datetime,
	IntSpare1 int,
	StrSpare1 nvarchar(75)
	)

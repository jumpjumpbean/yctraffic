use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZhzxFixedAssetsRegister') drop table ZhzxFixedAssetsRegister
go

create table ZhzxFixedAssetsRegister(
	Id int not null primary key identity(1,1),
	RegisteTime datetime,
	AssetName nvarchar(15),
	Specification nvarchar(10),
	Unit nvarchar(10),
	Amount nvarchar(10),
	UseStatus nvarchar(20),
	ScrapAmount nvarchar(10),
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

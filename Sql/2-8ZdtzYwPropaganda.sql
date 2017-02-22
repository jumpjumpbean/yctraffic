use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwPropaganda') drop table ZdtzYwPropaganda
go

create table ZdtzYwPropaganda(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	ActDate datetime,
	CostTime nvarchar(31),
	PropagandaWay nvarchar(63),
	PeopleNumber nvarchar(63),
	Location nvarchar(63),
	AudienceNumber nvarchar(63),
	OwnerId int,
	OwnerName nvarchar(31),
	Content nvarchar(500),
	PictureName1 nvarchar(31),
	PictureContent1 image,
	PictureName2 nvarchar(31),
	PictureContent2 image,
	Memo nvarchar(255),
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

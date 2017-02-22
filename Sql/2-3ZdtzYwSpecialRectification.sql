use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwSpecialRectification') drop table ZdtzYwSpecialRectification
go

create table ZdtzYwSpecialRectification(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	RecordTime datetime,
	PictureName1 nvarchar(31),
	PictureContent1 image,
	PictureName2 nvarchar(31),
	PictureContent2 image,
	PictureName3 nvarchar(31),
	PictureContent3 image,
	PictureName4 nvarchar(31),
	PictureContent4 image,
	PictureName5 nvarchar(31),
	PictureContent5 image,
	PictureName6 nvarchar(31),
	PictureContent6 image,
	PictureName7 nvarchar(31),
	PictureContent7 image,
	PictureName8 nvarchar(31),
	PictureContent8 image,
	Description nvarchar(800),
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

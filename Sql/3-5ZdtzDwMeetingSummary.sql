use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzDwMeetingSummary') drop table ZdtzDwMeetingSummary
go

create table ZdtzDwMeetingSummary(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RecordTime datetime,
	Host nvarchar(31),
	Location nvarchar(31),
	Recorder nvarchar(31),
	Members nvarchar(100),
	Content nvarchar(500),
	PictureName nvarchar(31),
	PictureContent image,
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

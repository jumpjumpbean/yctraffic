use yctrafficdb
go

if exists(select * from sysobjects where name='ZhzxTrafficViolation') drop table ZhzxTrafficViolation
go

create table ZhzxTrafficViolation(
	Id int not null primary key identity(1,1),
	CheckpointName nvarchar(63),
	CaptureLocation nvarchar(63),
	LicensePlateNumber nvarchar(31),
	OwnershipOfLand nvarchar(31),
	Speed nvarchar(31),
	ViolationType nvarchar(31),
	VehicleType nvarchar(31),
	LicensePlateColor nvarchar(31),
	VehicleColor nvarchar(31),
	CaptureTime datetime,
	DataStatus nvarchar(31),
	ThumbnailCode nvarchar(63),
	PictureCode nvarchar(63),
	ExcelName nvarchar(31),
	Memo nvarchar(255),
	WorkflowStatus int NOT NULL DEFAULT 0,
	IntSpare1 int,
	StrSpare1 nvarchar(255),
	IsFakeNumber bit NOT NULL DEFAULT 0,
	UploadName nvarchar(20),
	UploadTime datetime,
	ApprovalName nvarchar(20),
	ApprovalTime datetime,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateTime datetime
)


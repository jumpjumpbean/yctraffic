use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZdtzCyDangerDeal') drop table ZdtzCyDangerDeal
go

create table ZdtzCyDangerDeal(
	Id int not null primary key identity(1,1),
	ConfigId int,
	Title nvarchar(20),
	Content nvarchar(300),
	ContentImg nvarchar(75),
	ContentThumb nvarchar(75),
	ContentImgName nvarchar(50),
	HappenDate datetime,
	Location nvarchar(150),
	RoadType nvarchar(30),
	DangerDescription nvarchar(300),
	ReportDepartment nvarchar(75),
	IsSent bit NOT NULL DEFAULT 0,
	RectifyProgress nvarchar(75),
	Rectification nvarchar(75),
	RectificationThumb nvarchar(75),
	RectificationName nvarchar(50),
	DealDate datetime,
	ReviewImg nvarchar(75),
	ReviewThumb nvarchar(75),
	ReviewImgName nvarchar(50),
	OwnDepartmentId int,
	OwnDepartmentName nvarchar(75),
	Status int NOT NULL DEFAULT 0,
	StatusName nvarchar(50),
	SubLeaderId int NOT NULL DEFAULT 0,
	SubLeaderName nvarchar(31),
	IsSubLeaderSigned bit NOT NULL DEFAULT 0,
	SubLeaderComment nvarchar(127),
	SjzxAcceptComment nvarchar(127),
	LogbookType int NOT NULL DEFAULT 0,
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(31),
	StrSpare2 nvarchar(31),
	IsDeleted int NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime
	)

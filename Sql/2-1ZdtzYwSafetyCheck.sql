use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwSafetyCheck') drop table ZdtzYwSafetyCheck
go

create table ZdtzYwSafetyCheck(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	RoadNo nvarchar(150),
	Location nvarchar(150),
	RoadSurface nvarchar(31),
	LineType nvarchar(31),
	IssueReason nvarchar(300),
	IssueOwnDepartmentId nvarchar(255),
	RequiredFinishTime datetime,
	IssueComments nvarchar(300),
	IssuePicture1 image,
	IssuePicture2 image,
	IssuePicture3 image,
	IssuePicture4 image,
	Solution nvarchar(300),
	ActualFinishTime datetime,
	SolutionPicture1 image,
	SolutionPicture2 image,
	SolutionPicture3 image,
	SolutionPicture4 image,
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

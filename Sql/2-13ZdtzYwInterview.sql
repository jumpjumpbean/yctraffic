use yctrafficdb
go

if exists(select * from sysobjects where name='ZdtzYwInterview') drop table ZdtzYwInterview
go

create table ZdtzYwInterview(
	Id int not null primary key identity(1,1),
	ConfigId int,
	OwnDepartmentId int,
	Title nvarchar(31),
	InterviewDate datetime,
	Location nvarchar(31),
	InterviewerId int,
	InterviewerName nvarchar(31),
	InterviewType nvarchar(31),
	IntervieweeName nvarchar(31),
	IntervieweePlateNumber nvarchar(31),
	IntervieweeLicenseNumber nvarchar(31),
	IntervieweeAddress nvarchar(63),
	InterviewContent nvarchar(255),
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


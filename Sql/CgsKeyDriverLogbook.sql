use yctrafficdb
go

if exists(select * from sysobjects where name='CgsKeyDriverLogbook') drop table CgsKeyDriverLogbook
go

create table CgsKeyDriverLogbook(
	Id int not null primary key identity(1,1),
	DriverName    nvarchar(20),
	LicenseNo     nvarchar(50),
	PermitDriveType nvarchar(20),
	Status        nvarchar(20),
	TotalScore    int,
	NxtExamDate  datetime,
	ValidityDate datetime,
	ExaminationDate datetime,
	OperationType nvarchar(20),
	EmploymentDate datetime,
	Cellphone    nvarchar(50),
	Violations    nvarchar(50),
	OwnerDepartment   nvarchar(20),
	Remark    nvarchar(50),
	IsDeleted bit NOT NULL DEFAULT 0,
	IntSpare1 int,
	StrSpare1 nvarchar(255),
	StrSpare2 nvarchar(255),
	ApprovalName nvarchar(20),
	ApprovalTime datetime,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime
)


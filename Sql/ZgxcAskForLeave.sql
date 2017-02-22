use yctrafficdb 
go 

if exists(select * from sysobjects where name='ZgxcAskForLeave') drop table ZgxcAskForLeave
go

create table ZgxcAskForLeave(
	Id int not null primary key identity(1,1),
	PersonName nvarchar(20),
	PersonDepartmentId int,
	PersonDepartmentName nvarchar(75),
	PersonGender nvarchar(10),
	PersonJob nvarchar(10),
	EmploymentTime datetime,
	YearsOfWorking  int,
	LeaveDays       int,
	LeaveReason nvarchar(30),
	LeaveDateFrom   datetime,
	LeaveDateTo   datetime,
	Applicant nvarchar(20),
	ApplicationDate datetime,
	CheckComments  nvarchar(50),
	CheckDate datetime,
	ApproveComments  nvarchar(50),
	ApproveDate datetime,
	ApproverId int,
	ApproverName  nvarchar(20),
	BackFormLeaveDate datetime,
	CreateId int,
	CreateName nvarchar(31),
	CreateTime datetime,
	OwnDepartmentId int,
	OwnDepartmentName nvarchar(75),	
	Remake nvarchar(75),
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(31),
	StrSpare2 nvarchar(31),
	StrSpare3 nvarchar(31),
	IsDeleted bit NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime
	)


use yctrafficdb 
go 

if exists(select * from sysobjects where name='MaterialDeclareTable') drop table MaterialDeclareTable
go 

create table MaterialDeclareTable
(
	Id  				int not null primary key identity(1,1),	
	CreateUserId			int,
	CreateUserName			nvarchar(20),
	SubmitTime			datetime, 
	DepartmentId			int,
	DepartmentCode			nvarchar(20),
	DepartmentName			nvarchar(20),
	UpdaterId			int,
	UpdateTime			datetime, 
	MaterialDeclareTime		datetime,
	MaterialIssueTime		datetime, 
	MaterialTitle			nvarchar(50),
	Author				nvarchar(20),
	ApprovalUserId			int,
	ApprovalName			nvarchar(20),
	ApprovalTime			datetime, 
	Score				int,
	Comments			nvarchar(50),
	IsDeleted			int not null
)

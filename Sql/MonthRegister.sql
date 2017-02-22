use yctrafficdb 
go 

if exists(select * from sysobjects where name='MonthRegisterTable') drop table MonthRegisterTable
go 

create table MonthRegisterTable
(
	Id  					int not null primary key identity(1,1),	
	UserId					int,
	UserName				nvarchar(20),
	PoliceNumber			nvarchar(20),
	
	DepartmentId			int,
	DepartmentCode			nvarchar(20),
	DepartmentName			nvarchar(20),
	
	WhichMonth				datetime,
	WorkSummary				nvarchar(2000),
	OverTime				int,
	ApproveUserId			int,
	ApproveUserName			nvarchar(20),
	ApproveTime				datetime, 
	ApproveResult			nvarchar(20),
	StatusId				int,
	StatusName				nvarchar(20),
	
	CreateUserId			int,
	CreateUserName			nvarchar(20),
	CreateTime				datetime, 
	
	UpdaterId				int,
	UpdaterName				nvarchar(20),	
	UpdateTime				datetime, 
	IsDeleted				bit
)

use yctrafficdb 
go 

if exists(select * from sysobjects where name='MapMarkersTable') drop table MapMarkersTable
go 

create table MapMarkersTable
(
	Id  					int not null primary key identity(1,1),	
	Title					nvarchar(20),
	DepartmentId			int,
	DepartmentCode			nvarchar(20),
	DepartmentName			nvarchar(20),
	StyleTypeId				int,
	StyleType				nvarchar(20),
	lat						nvarchar(50),
	lng						nvarchar(50),	
    ClassId					int,
	AccidentDate			datetime,
	CommentMark				nvarchar(500),	
	CreateUserId			int,
	CreateUserName			nvarchar(20),
	CreateTime				datetime, 	
	UpdaterId				int,
	UpdaterName				nvarchar(20),	
	UpdateTime				datetime, 
	IsDeleted				bit
)

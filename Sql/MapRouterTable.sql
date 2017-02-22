use yctrafficdb 
go 

if exists(select * from sysobjects where name='MapRouterTable') drop table MapRouterTable
go 

create table MapRouterTable
(
	Id  					int not null primary key identity(1,1),	
	Title					nvarchar(20),
	DepartmentId			int,
	DepartmentCode			nvarchar(20),
	DepartmentName			nvarchar(20),
	StyleTypeId				int,
	StyleType				nvarchar(20),
	latStart				nvarchar(50),
	lngStart				nvarchar(50),
	latEnd					nvarchar(50),
	lngEnd					nvarchar(50),		
	CommentMark				nvarchar(500),	
	CreateUserId			int,
	CreateUserName			nvarchar(20),
	CreateTime				datetime, 	
	UpdaterId				int,
	UpdaterName				nvarchar(20),	
	UpdateTime				datetime, 
	IsDeleted				bit,
	extfield1					nvarchar(50),
	extfield2					nvarchar(50),
	extfield3					nvarchar(50),
	extfield4					nvarchar(50),
	extfield5					nvarchar(50),
	extfield6					nvarchar(50)
)

		

use yctrafficdb 
go 

if exists(select * from sysobjects where name='MapRouterPointsTable') drop table MapRouterPointsTable
go 

create table MapRouterPointsTable
(
Id  	 int not null primary key identity(1,1),	
RouterId	 int,   --Ö÷±íµÄ ID	
lat	 nvarchar(50),
lng	 nvarchar(50),	
CommentMark	 nvarchar(500),	
CreateUserId	 int,
CreateUserName	 nvarchar(20),
CreateTime	 datetime, 	
UpdaterId	 int,
UpdaterName	 nvarchar(20),	
UpdateTime	 datetime, 
IsDeleted	 bit
)
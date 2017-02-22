use yctrafficdb 
go 

if exists(select * from sysobjects where name='MayorHotlineTaskTable') drop table MayorHotlineTaskTable
go 

create table MayorHotlineTaskTable
(
	Id  					int not null primary key identity(1,1),
	CreateDate				datetime, 
	DueDate					datetime, 
	Contents				nvarchar(1205),
	ContentPicture			nvarchar(75),
	ContentThumb			nvarchar(75),
	ContentPictureName		nvarchar(100),
	OwnDepartmentId			int,
	OwnDepartmentName		nvarchar(20),
	StatusId				int,
	Status					nvarchar(20),
	Result					nvarchar(205),
	VerifyFile				nvarchar(75),
	VerifyThum				nvarchar(75),
	VerifyFileName			nvarchar(100),
	CreateUserId			int,
	CreateUserName			nvarchar(20),
	SovleUserId				int,
	SovleUserName			nvarchar(20),
	VerifyUserId			int,
	VerifyUserName			nvarchar(75),
	Suggest					nvarchar(205),
	UpdaterId				int,
	UpdaterName				nvarchar(20),
	CreateTime				datetime, 
	UpdateTime				datetime, 
	IsDeleted				bit NOT NULL DEFAULT 0,
	IsComplainPolice		int,
	IntSpare1				int,
	IntSpare2				int,
	StrSpare1				nvarchar(31),
	StrSpare2				nvarchar(31)
)

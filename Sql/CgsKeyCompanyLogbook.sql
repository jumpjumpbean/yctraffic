use yctrafficdb
go

if exists(select * from sysobjects where name='CgsKeyCompanyLogbook') drop table CgsKeyCompanyLogbook
go

create table CgsKeyCompanyLogbook(
	Id int not null primary key identity(1,1),
	CompanyName    nvarchar(30),
	KeyType       nvarchar(20),
	OperationType nvarchar(20),
	MainManagerName nvarchar(20),
	MainManagerPhone    nvarchar(50),
	SecurityManagerName nvarchar(20),
	SecurityanagerPhone    nvarchar(50),
	InChargePoliceman    nvarchar(20),
	ManagerDepartment   nvarchar(20),
	Changes  nvarchar(30),
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


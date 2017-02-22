use yctrafficdb 
go 

if exists(select * from sysobjects where name='EquipmentCheckTable') drop table EquipmentCheckTable
go 

create table EquipmentCheckTable
(
	Id  		int not null primary key identity(1,1),	
	ApplyerId	int,
	Applyer		nvarchar(20),
	DepartmentId	int,
	DepartmentCode	nvarchar(20),
	DepartmentName	nvarchar(20),	
	EquipmentName	nvarchar(200),	
	EquipmentUsed	nvarchar(200),
	MoneySource	nvarchar(200),
	ManagerId	int,
	Manager		nvarchar(50),	
	BugTypeId	int,
	BugType		nvarchar(50),	
	PerAuditerId	int,
	PerAUditer	nvarchar(50),
	PerAuditTime	datetime,
	IsAudit		bit,
	AuditResultId	int,
	AuditResult	nvarchar(50),
	AuditReason	nvarchar(500),
	AuditTime	datetime,
	AuditerId	int,
	Auditer		nvarchar(50),
	DueDate		datetime,
	DueDateLog	nvarchar(1000),
	SuperviseCount	int,
	SuperviseContent nvarchar(2000),
	SuperviseTime	datetime,
	Superviserid	int,
	Superviser	nvarchar(50),
	SuperviseLog	 nvarchar(2000),
	StatusId	int,
	Status		nvarchar(50),
	ArchiveTime	datetime,
	ArchiverId	int,
	Archiver	nvarchar(50),
	CommentMark	nvarchar(500),

	CreateUserId	int,
	CreateUserName	nvarchar(20),
	CreateTime	datetime, 
	
	UpdaterId	int,
	UpdaterName	nvarchar(20),	
	UpdateTime	datetime, 
	IsDeleted	bit

)



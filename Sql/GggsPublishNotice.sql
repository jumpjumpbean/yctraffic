use yctrafficdb
go

if exists(select * from sysobjects where name='GggsPublishNotice') drop table GggsPublishNotice
go

create table GggsPublishNotice(
	Id int not null primary key identity(1,1),
	[Category] [nvarchar](8) NULL,
	[Title] [nvarchar](40) NULL,
	[Content] [nvarchar](600) NULL,
	[Status] [nvarchar](10) NULL,
	[Remark] [nvarchar](30) NULL,
	[Topmost] [int] NOT NULL,
	[AttachmentName1] [nvarchar](40) NULL,
	[AttachmentPath1] [nvarchar](75) NULL,
	[AttachmentName2] [nvarchar](40) NULL,
	[AttachmentPath2] [nvarchar](75) NULL,
	[UpdaterId] [int] NULL,
	[UpdaterName] [nvarchar](15) NULL,
	[UpdateTime] [datetime] NULL,
	[CreateId] [int] NULL,
	[CreateName] [nvarchar](15) NULL,
	[CreateTime] [datetime] NULL,
	[AuditorId] [int] NULL,
	[AuditorName] [nvarchar](15) NULL,
	[AuditTime] [datetime] NULL,
	[DepartmentId] [int] NULL,
	[DepartmentName] [nvarchar](15) NULL,
	[IsDeleted] [int] NOT NULL
	)


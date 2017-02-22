use yctrafficdb
go

if exists(select * from sysobjects where name='ZhzxPicture') drop table ZhzxPicture
go

create table ZhzxPicture(
	[Code] [nvarchar](63) not NULL primary key,
	[ComposedPicture] [nvarchar](100) NULL,
	[SourcePicture1] [nvarchar](100) NULL,
	[SourcePicture2] [nvarchar](100) NULL,
	[SourcePicture3] [nvarchar](100) NULL,
	[IntSpare1] [int] NULL,
	[StrSpare1] [nvarchar](50) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT 0,
	[UpdaterId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[CreateId] [int] NULL,
	[CreateTime] [datetime] NULL
	)

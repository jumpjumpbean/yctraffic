use yctrafficdb
go

if exists(select * from sysobjects where name='ZhzxThumbnail') drop table ZhzxThumbnail
go

create table ZhzxThumbnail(
	[Code] [nvarchar](63) not NULL primary key,
	[ComposedThumbnail] [nvarchar](100) NULL,
	[SourceThumbnail1] [nvarchar](100) NULL,
	[SourceThumbnail2] [nvarchar](100) NULL,
	[SourceThumbnail3] [nvarchar](100) NULL,
	[IntSpare1] [int] NULL,
	[StrSpare1] [nvarchar](50) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT 0,
	[UpdaterId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[CreateId] [int] NULL,
	[CreateTime] [datetime] NULL
	)
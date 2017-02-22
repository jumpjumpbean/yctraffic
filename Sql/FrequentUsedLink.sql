use yctrafficdb 
go 

if exists(select * from sysobjects where name='FrequentUsedLink') drop table FrequentUsedLink
go

create table FrequentUsedLink(
	Id int not null primary key identity(1,1),
	InlineText1 nvarchar(20),
	NavigateUri1 nvarchar(150),
	InlineText2 nvarchar(20),
	NavigateUri2 nvarchar(150),
	InlineText3 nvarchar(20),
	NavigateUri3 nvarchar(150),
	InlineText4 nvarchar(20),
	NavigateUri4 nvarchar(150),
	InlineText5 nvarchar(20),
	NavigateUri5 nvarchar(150),
	InlineText6 nvarchar(20),
	NavigateUri6 nvarchar(150),
	InlineText7 nvarchar(20),
	NavigateUri7 nvarchar(150),
	InlineText8 nvarchar(20),
	NavigateUri8 nvarchar(150),
	InlineText9 nvarchar(20),
	NavigateUri9 nvarchar(150),
	InlineText10 nvarchar(20),
	NavigateUri10 nvarchar(150)
	)

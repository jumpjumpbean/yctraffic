﻿use yctrafficdb
go

if exists(select * from sysobjects where name='KjssEquipmentRequest') drop table KjssEquipmentRequest
go

create table KjssEquipmentRequest(
	Id int not null primary key identity(1,1),
	RequestTime datetime,
	RequestDept int NOT NULL DEFAULT 0,
	RequestDeptName nvarchar(63),
	EquipmentName nvarchar(127),
	EquipmentUse nvarchar(127),
	OutlayFrom nvarchar(63),
	Administrator nvarchar(31),
	Applicant nvarchar(31),
	RequestType int NOT NULL DEFAULT 0,
	RequestTypeName nvarchar(10),
	Node1Comment nvarchar(127),
	Node2Comment nvarchar(127),
	Node3Comment nvarchar(127),
	Node4Comment nvarchar(127),
	SuperviseCommnet nvarchar(127),
	Deadline datetime,
	Status int NOT NULL DEFAULT 0,
	StatusDesc nvarchar(10),
	SubLeaderId int NOT NULL DEFAULT 0,
	SubLeaderName nvarchar(31),
	IsSubLeaderSigned bit NOT NULL DEFAULT 0,
	IsDDZSigned bit NOT NULL DEFAULT 0,
	CompleteTime datetime,
	Item1 nvarchar(51),
	ItemUse1 nvarchar(51),
	ItemPrice1 int not null,
	ItemAmount1 int not null,
	Item2 nvarchar(51),
	ItemUse2 nvarchar(51),
	ItemPrice2 int not null,
	ItemAmount2 int not null,
	Item3 nvarchar(51),
	ItemUse3 nvarchar(51),
	ItemPrice3 int not null,
	ItemAmount3 int not null,
	Item4 nvarchar(51),
	ItemUse4 nvarchar(51),
	ItemPrice4 int not null,
	ItemAmount4 int not null,
	Item5 nvarchar(51),
	ItemUse5 nvarchar(51),
	ItemPrice5 int not null,
	ItemAmount5 int not null,
	Item6 nvarchar(51),
	ItemUse6 nvarchar(51),
	ItemPrice6 int not null,
	ItemAmount6 int not null,
	Item7 nvarchar(51),
	ItemUse7 nvarchar(51),
	ItemPrice7 int not null,
	ItemAmount7 int not null,
	Item8 nvarchar(51),
	ItemUse8 nvarchar(51),
	ItemPrice8 int not null,
	ItemAmount8 int not null,
	Item9 nvarchar(51),
	ItemUse9 nvarchar(51),
	ItemPrice9 int not null,
	ItemAmount9 int not null,
	IntSpare1 int,
	IntSpare2 int,
	StrSpare1 nvarchar(31),
	StrSpare2 nvarchar(31),
	IsDeleted int NOT NULL DEFAULT 0,
	UpdaterId int,
	UpdateTime datetime,
	CreateId int,
	CreateTime datetime
)



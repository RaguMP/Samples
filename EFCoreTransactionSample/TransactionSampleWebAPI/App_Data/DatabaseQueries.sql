create table TR_ProjectSequenceMapper
(
Id int not null Identity,
BrandId int not null,
SequenceId int not null,
IsActive bit not null,
RowVersion rowversion not null,
constraint PK_TR_ProjectSequenceMapper_Id primary key(Id)
)

create table TR_ProjectDetails
(
Id int not null Identity,
BrandId int not null,
BrandProjectId int not null,
IsActive bit not null,
constraint PK_TR_ProjectDetails_Id primary key(Id)
)

create table TR_ProjectMapper
(
Id int not null Identity,
ProjectId int not null,
IsActive bit not null,
constraint PK_TR_ProjectMapper_Id primary key(Id),
constraint FK_TR_ProjectMapper_TicketId foreign key(ProjectId) references TR_ProjectDetails(Id),
)

insert into TR_ProjectSequenceMapper(BrandId,SequenceId,IsActive) values (1,1,1)
insert into TR_ProjectSequenceMapper(BrandId,SequenceId,IsActive) values (2,1,1)
insert into TR_ProjectSequenceMapper(BrandId,SequenceId,IsActive) values (3,1,1)

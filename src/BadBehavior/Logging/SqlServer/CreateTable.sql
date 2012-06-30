if not exists (select * from sysobjects where name='BadBehavior_Log' and xtype='U')
    create table BadBehavior_Log (
        ID             bigint      not null identity(1, 1) primary key clustered,
        IP             varchar(40) not null,
        [Date]         datetime    not null default getdate(),
        RequestMethod  varchar(16) not null,
        RequestUri     ntext       not null,
        ServerProtocol varchar(16) not null,
        HttpHeaders    ntext       not null,
        UserAgent      ntext       not null,
        RequestEntity  ntext       not null,
        [Key]          varchar(8)  not null
    )

if not exists (select * from sysobjects where name='BadBehavior_Log' and xtype='U')
    create table BadBehavior_Log (
        ID             bigint      not null identity(1, 1) primary key clustered,
        IP             varchar(40) null,
        [Date]         datetime    not null default getdate(),
        RequestMethod  varchar(16) null,
        RequestUri     ntext       null,
        ServerProtocol varchar(16) null,
        HttpHeaders    ntext       null,
        UserAgent      ntext       null,
        RequestEntity  ntext       null,
        [Key]          varchar(8)  null,
        ReverseDns     ntext       null
    )

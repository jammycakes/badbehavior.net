create procedure BadBehavior_AddEntry(
    @IP             varchar(40),
    @Date           datetime,
    @RequestMethod  varchar(16),
    @RequestUri     ntext,
    @ServerProtocol varchar(16),
    @HttpHeaders    ntext,
    @UserAgent      ntext,
    @RequestEntity  ntext,
    @Key            varchar(8)
)
as
begin
    insert into BadBehavior_Log(
        IP,
        [Date],
        RequestMethod,
        RequestUri,
        ServerProtocol,
        HttpHeaders,
        UserAgent,
        RequestEntity,
        [Key]
    )
    values(
        @IP,
        @Date,
        @RequestMethod,
        @RequestUri,
        @ServerProtocol,
        @HttpHeaders,
        @UserAgent,
        @RequestEntity,
        @Key
    )
end
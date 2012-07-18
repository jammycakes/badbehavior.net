﻿insert into BadBehavior_Log(
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

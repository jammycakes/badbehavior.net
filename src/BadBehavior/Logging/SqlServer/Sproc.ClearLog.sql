create procedure BadBehavior_ClearLog
as
begin
    set nocount on
    delete from BadBehavior_Log
end
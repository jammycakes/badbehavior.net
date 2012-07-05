create procedure BadBehavior_Count(@Start  datetime, @End datetime)
as
begin
    set nocount on
    select count (*) from BadBehavior_Log
        where (@Start is null or @Start < [Date])
            and (@End is null or @End > [Date])
end
CREATE procedure procedure_ValidateDatabaseRowNumbers as

declare @LastNo int
declare @FirstNo int
declare @Amount int
declare @BelowZero int



select top 1 @LastNo = DrawNo from RawData
order by DrawNo desc
--select @LastNo

select top 1 @FirstNo = DrawNo from RawData
order by DrawNo asc
--select @FirstNo

select @BelowZero =  count(*) from RawData
where DrawNo < 0
--select @BelowZero

select @Amount = count(*) from RawData
--select @Amount


IF @LastNo = @Amount and @FirstNo = 1 and @BelowZero = 0
return 1
ELSE
return 0
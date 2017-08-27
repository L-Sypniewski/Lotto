CREATE procedure procedure_ValidateDatabaseRowNumbers as

declare @LastNo int
declare @FirstNo int
declare @Amount int
declare @BelowZero int



select top 1 @LastNo = NrLosowania from RawData
order by NrLosowania desc
--select @LastNo

select top 1 @FirstNo = NrLosowania from RawData
order by NrLosowania asc
--select @FirstNo

select @BelowZero =  count(*) from RawData
where NrLosowania < 0
--select @BelowZero

select @Amount = count(*) from RawData
--select @Amount


IF @LastNo = @Amount and @FirstNo = 1 and @BelowZero = 0
return 1
ELSE
return 0
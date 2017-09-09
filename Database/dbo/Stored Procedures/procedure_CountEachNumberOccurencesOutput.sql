CREATE procedure procedure_CountEachNumberOccurencesOutput
@number tinyint, @count_number int output
As
SET NOCOUNT ON
select  @count_number = count(*) from RawData
                    where Number1 = @number or Number2 = @number  or Number3 = @number or Number4 = @number  
					or Number5 = @number or Number6 = @number  or Number7 = @number or Number8 = @number
					or Number9 = @number or Number10 = @number or Number11 = @number or Number12 = @number
					or Number13 = @number or Number14 = @number or Number15 = @number or Number16 = @number
					or Number17 = @number or Number18 = @number or Number19 = @number or Number20 = @number
CREATE procedure procedure_CountEachNumberOccurencesOutput
@number tinyint, @count_number int output
As
SET NOCOUNT ON
select  @count_number = count(*) from RawData
                    where Liczba1 = @number or Liczba2 = @number  or Liczba3 = @number or Liczba4 = @number  
					or Liczba5 = @number or Liczba6 = @number  or Liczba7 = @number or Liczba8 = @number
					or Liczba9 = @number or Liczba10 = @number or Liczba11 = @number or Liczba12 = @number
					or Liczba13 = @number or Liczba14 = @number or Liczba15 = @number or Liczba16 = @number
					or Liczba17 = @number or Liczba18 = @number or Liczba19 = @number or Liczba20 = @number
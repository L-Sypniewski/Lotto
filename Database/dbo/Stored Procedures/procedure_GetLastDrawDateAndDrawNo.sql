CREATE procedure [procedure_GetLastDrawDateAndDrawNo] @last_draw_date Date output, @last_draw_number smallint output
AS
select top 1 @last_draw_number=DrawNo, @last_draw_date=DrawDate from RawData
order by DrawNo desc
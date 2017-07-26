CREATE procedure procedure_GetLastDrawDate @last_draw_date Date output, @last_draw_number smallint output
AS
select top 1 @last_draw_number=NrLosowania, @last_draw_date=DataLosowania from RawData
order by NrLosowania desc
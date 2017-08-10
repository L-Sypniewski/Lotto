select * from RawData
where NrLosowania between 4347 and 6453
order by DataLosowania  desc

select * from RawData
where NrLosowania in (4347,4346, 4348)
order by DataLosowania  desc

select * from RawData
WHERE CONVERT(VARCHAR(25), DataLosowania, 126) LIKE '2008-06-29%'

select count(*) from RawData
where DataLosowania between '2008/10/23' and '2012/03/26' and Plus is not Null
order by DataLosowania  desc
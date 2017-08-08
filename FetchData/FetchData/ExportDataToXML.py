from FetchData import DownloadData
import datetime

date = datetime.datetime(day=6, month=8, year=2017)
number_of_days_to_process = 2 #DownloadData.GetDaysBetweenDates(date, DownloadData.DATE_OF_FIRST_DRAW)
path =  r"F:\filename.xml"


DownloadData.SaveDrawsFromDateRangeToXML(DownloadData.DATE_TODAY, date, path)
#DownloadData.Update(path)
DownloadData.MakePrettyXML(path, '_pretty')




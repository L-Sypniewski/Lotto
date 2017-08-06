from FetchData import DownloadData
import datetime

date = datetime.datetime(month=10,day=8,year=2007)
number_of_days_to_process = 2 #DownloadData.GetDaysBetweenDates(date, DownloadData.DATE_OF_FIRST_DRAW)

for i in range(0, number_of_days_to_process):
    if i == 0:
        DownloadData.SaveDrawToXML( r"F:\filename.xml", date, True)
    else:
        DownloadData.SaveDrawToXML( r"F:\filename.xml", date, False)
    date = date + datetime.timedelta(days=-1)
    print("{0}\{1}".format(i+1, number_of_days_to_process))

DownloadData.MakePrettyXML(r"F:\filename.xml")




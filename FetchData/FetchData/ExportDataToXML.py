from FetchData import DownloadData
import datetime

dateOne = datetime.datetime(day=4, month=8, year=2017)
dateTwo = datetime.datetime(day=2, month=8, year=2017)
number_of_days_to_process = 2
path =  r"F:\filename.xml"

DownloadData.UpdateToXML(path)
DownloadData.MakePrettyXML(path, '_pretty')
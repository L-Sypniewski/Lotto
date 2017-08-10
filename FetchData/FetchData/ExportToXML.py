from FetchData import DownloadData as DD
import datetime

dateOne = datetime.datetime(day=4, month=8, year=2017)
dateTwo = datetime.datetime(day=2, month=8, year=2017)
daten = DD.DATE_OF_FIRST_DRAW
datenplus = daten + datetime.timedelta(days=1)
number_of_days_to_process = 2
path =  r"F:\filename.xml"


DD.UpdateXML(path)
DD.MakePrettyXML(path, '_pretty')
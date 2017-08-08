from xml.dom import minidom
from datetime import datetime
from operator import itemgetter
import datetime as DT
from codecs import open
import lxml.etree as etree
import xml.etree.cElementTree as ET
import re
import urllib
import re
import time
import requests


class DownloadData(object):

    DATE_TODAY = datetime.now().replace(hour=23, minute=59)
    DATE_OF_FIRST_DRAW = datetime.strptime('1996-03-17', '%Y-%m-%d')
    XSI_NAMESPACE = "http://www.w3.org/2001/XMLSchema-instance"
    XSI = "{%s}" % XSI_NAMESPACE
    NSMAP = {None : XSI_NAMESPACE} # the default namespace (no prefix)


    def GetHTML(date):
        url = 'http://www.lotto.pl/multi-multi/wyniki-i-wygrane/wyszukaj'
        datas = {'data_losowania[date]': date, 'form_id':'multimulti_wyszukaj_form'}
        r = requests.post(url, data=datas, allow_redirects=True)
        doc = r.text
        tree = etree.HTML(doc)
        raw_html = ''
        for elem in tree.xpath("//table[@class='ostatnie-wyniki-table']/tbody/tr[td/@class='archiwum_wyniki_numbers']"):
            # pretty_print ensures that it is nicely formatted.
            raw_html = raw_html + str(etree.tostring(elem, pretty_print=True))
        raw_html = re.sub('\s+', ' ', raw_html)
        raw_html=raw_html.replace('\\n', '')
        if "Brak wyników" in doc:
            raw_html = "No draws"
        return raw_html


    def SeparateDrawsToList(html):        
        return re.findall(r'<tr>(.*?)</tr>', html)


    def ExtractDrawData(html):
        regex_with_plus = r'<td>(?P<DrawNo>\d{1,5})<br.*?<br />(?P<DrawDate>\d{2}-\d{2}-\d{2}),.*?, (?P<DrawTime>\d{2}:\d{2}).*?i\">(?P<Number1>\d{1,2})</div.*?i\">(?P<Number2>\d{1,2})</div.*?i\">(?P<Number3>\d{1,2})</div.*?i\">(?P<Number4>\d{1,2})</div.*?i\">(?P<Number5>\d{1,2})</div.*?i\">(?P<Number6>\d{1,2})</div.*?i\">(?P<Number7>\d{1,2})</div.*?i\">(?P<Number8>\d{1,2})</div.*?i\">(?P<Number9>\d{1,2})</div.*?i\">(?P<Number10>\d{1,2})</div.*?i\">(?P<Number11>\d{1,2})</div.*?i\">(?P<Number12>\d{1,2})</div.*?i\">(?P<Number13>\d{1,2})</div.*?i\">(?P<Number14>\d{1,2})</div.*?i\">(?P<Number15>\d{1,2})</div.*?i\">(?P<Number16>\d{1,2})</div.*?i\">(?P<Number17>\d{1,2})</div.*?i\">(?P<Number18>\d{1,2})</div.*?i\">(?P<Number19>\d{1,2})</div.*?i\">(?P<Number20>\d{1,2})</div.*?plus\">(?P<Plus>\d{1,2})</div>'
        regex_without_plus = r'<td>(?P<DrawNo>\d{1,5})<br.*?<br />(?P<DrawDate>\d{2}-\d{2}-\d{2}),.*?, (?P<DrawTime>\d{2}:\d{2}).*?i\">(?P<Number1>\d{1,2})</div.*?i\">(?P<Number2>\d{1,2})</div.*?i\">(?P<Number3>\d{1,2})</div.*?i\">(?P<Number4>\d{1,2})</div.*?i\">(?P<Number5>\d{1,2})</div.*?i\">(?P<Number6>\d{1,2})</div.*?i\">(?P<Number7>\d{1,2})</div.*?i\">(?P<Number8>\d{1,2})</div.*?i\">(?P<Number9>\d{1,2})</div.*?i\">(?P<Number10>\d{1,2})</div.*?i\">(?P<Number11>\d{1,2})</div.*?i\">(?P<Number12>\d{1,2})</div.*?i\">(?P<Number13>\d{1,2})</div.*?i\">(?P<Number14>\d{1,2})</div.*?i\">(?P<Number15>\d{1,2})</div.*?i\">(?P<Number16>\d{1,2})</div.*?i\">(?P<Number17>\d{1,2})</div.*?i\">(?P<Number18>\d{1,2})</div.*?i\">(?P<Number19>\d{1,2})</div.*?i\">(?P<Number20>\d{1,2})</div'
        pattern_with_plus = re.compile(regex_with_plus)
        pattern_without_plus = re.compile(regex_without_plus)
        result = []
        for i in range(len(html)):
           if "plus" in html[i]:
               result.append([m.groupdict() for m in pattern_with_plus.finditer(html[i])])
               result[i][0]['DrawDate'] = DownloadData.ConvertToFullDate(result[i][0]['DrawDate'])
           else:
               result.append([m.groupdict() for m in pattern_without_plus.finditer(html[i])])
               result[i][0]['DrawDate'] = DownloadData.ConvertToFullDate(result[i][0]['DrawDate'])
        return result  # result[1][0]['DrawDate']


    def ConvertToFullDate(date : str): 
        if date[6] != '9':
            full_date = date[:6] + "20" + date[6:]
        else:
            full_date = date[:6] + "19" + date[6:]
        return full_date    

      
    def ConvertToDate(date):
        return datetime.strptime(date, '%Y-%m-%d')


    def ConvertFullDateTimeToISO(date):
        return datetime.isoformat(datetime.strptime(date, '%d-%m-%Y %H:%M'))  


    def GetDaysBetweenDates(date_from : datetime, date_to : datetime):
        date_from = date_from.replace(hour=0, minute=0)
        date_to = date_to.replace(hour=0, minute=0)
        if date_from > date_to:
            return (date_from - date_to).days
        else:
            return (date_to - date_from).days


    def AddDataToNewXML(file_path, draw_data):
        formatted_date_time =  DownloadData.ConvertFullDateTimeToISO(draw_data['DrawDate'] + ' ' + draw_data['DrawTime'])
        root = etree.Element("Draws")
        draw_elem = etree.SubElement(root, "Draw", ID=draw_data['DrawNo'])
        etree.SubElement(draw_elem, "Date").text = formatted_date_time
        if 'Plus' in draw_data:
            etree.SubElement(draw_elem, "Plus").text = draw_data['Plus']
        else:
            etree.SubElement(draw_elem, "Plus", attrib = {DownloadData.XSI + "nil" : "true"} )
        numbers_elem = etree.SubElement(draw_elem, "Numbers")
        for i in range (1, 21):
            etree.SubElement(numbers_elem, 'N').text = draw_data['Number' + str(i)]
        xmlstr = etree.tostring(root)
        string = etree.tostring(etree.fromstring(xmlstr), xml_declaration=True).decode()
        with open(file_path, "w") as f:
            f.write(string)


    def AddDataToExistingXML(file_path, draw_data, insert_as_first = False):
        try:
            xml = etree.parse(file_path)
            root = xml.getroot()            
            formatted_date_time =  DownloadData.ConvertFullDateTimeToISO(draw_data['DrawDate'] + ' ' + draw_data['DrawTime'])
            draw_elem = etree.SubElement(root, "Draw", ID=draw_data['DrawNo'])
            if insert_as_first == True:   # to append data at the beginning while updating         
                root.insert(0,draw_elem)           
            etree.SubElement(draw_elem, "Date").text = formatted_date_time
            if 'Plus' in draw_data:
                etree.SubElement(draw_elem, "Plus").text = draw_data['Plus']
            else:
                etree.SubElement(draw_elem, "Plus", attrib = {DownloadData.XSI + "nil" : "true"} )
            numbers_elem = etree.SubElement(draw_elem, "Numbers")
            for i in range (1, 21):
                etree.SubElement(numbers_elem, 'N').text = draw_data['Number' + str(i)]
            xmlstr = etree.tostring(root)
            string = etree.tostring(etree.fromstring(xmlstr), xml_declaration=True).decode()
            with open(file_path, "w") as f:
                f.write(string)
        except FileNotFoundError:
            print('File {file_path} has not been found! New file {file_path} will be created.'.format(file_path=file_path))
            DownloadData.AddDataToNewXML(file_path, draw_data)


    def MakePrettyXML(file_path, file_path_suffix='_PrettyPrinted'):
        xml = ET.parse(file_path)
        root = xml.getroot()
        xmlstr = ET.tostring(root)
        dom = minidom.parseString(xmlstr)
        pretty_xml = dom.toprettyxml().encode('utf-8')
        prety_xml_unicode = etree.tostring(etree.fromstring(pretty_xml), xml_declaration=True, encoding='utf-8').decode()
        with open(file_path[:len(file_path) - 4] + file_path_suffix + '.xml', "w", "utf-8") as f:
            f.write(prety_xml_unicode)


    def SaveDrawToXML(file_path, draw_date : datetime, create_new_file : bool):
        while True:            
            html = DownloadData.GetHTML(date=draw_date.strftime('%Y-%m-%d'))
            time.sleep(0.3)
            if html != "":
                break
            time.sleep(1) # server denies access if there are too many request in a short time
        if html == "No draws":
            return None
        draws = DownloadData.SeparateDrawsToList(html)
        extracted_data = DownloadData.ExtractDrawData(draws)        
        extracted_data_list = []
        if len(extracted_data) == 2:
            extracted_data_list = [extracted_data[0][0], extracted_data[1][0]]
        else:
             extracted_data_list = [extracted_data[0][0]] 
        extracted_data_list_sorted = sorted(extracted_data_list, key=itemgetter('DrawNo'), reverse=True)
        for i in range(0, len(extracted_data_list_sorted)):
            if create_new_file == True and i == 0:
                DownloadData.AddDataToNewXML(file_path, extracted_data_list_sorted[i])
            else:
                DownloadData.AddDataToExistingXML(file_path, extracted_data_list_sorted[i])


    def SaveDrawsFromDateRangeToXML(date_from : datetime, date_to : datetime, file_path, update_file : bool): # TODO exception if date_from > date_to
        number_of_days_to_process = DownloadData.GetDaysBetweenDates(date_from, date_to) + 1
        for i in range(0, number_of_days_to_process):
            if update_file == False and i == 0:
                DownloadData.SaveDrawToXML(file_path, date_to, True)
            else:
                DownloadData.SaveDrawToXML(file_path, date_to, False)
            date_to = date_to + DT.timedelta(days=-1)


##################################################################


    def Update(file_path, draw_date : datetime):

        xml = etree.parse(file_path)
        root = xml.getroot()  
        last_draw_number = int(xml.xpath("//Draw[1]/@ID")[0])
        last_draw_date =  datetime.strptime(str(xml.xpath("//Draw[1]/Date/text()")[0]), "%Y-%m-%dT%H:%M:%S")
        number_of_days_to_check = DownloadData.GetDaysBetweenDates(DownloadData.DATE_TODAY, last_draw_date) + 1

        while True:            
            html = DownloadData.GetHTML(date=draw_date.strftime('%Y-%m-%d'))
            time.sleep(0.3)
            if html != "":
                break
            time.sleep(1) # server denies access if there are too many request in a short time
        if html == "No draws":
            return None

        draws = DownloadData.SeparateDrawsToList(html)
        extracted_data = DownloadData.ExtractDrawData(draws)        
        extracted_data_list = []
        if len(extracted_data) == 2:
            if int(extracted_data[0][0]["DrawNo"]) > last_draw_number:
                extracted_data_list.append(extracted_data[0][0])
            if int(extracted_data[1][0]["DrawNo"]) > last_draw_number:
                extracted_data_list.append(extracted_data[1][0])
        else:
            if int(extracted_data[0][0]["DrawNo"]) > last_draw_number:
                extracted_data_list.append(extracted_data[0][0])
        if extracted_data_list != []:
            extracted_data_list_sorted = sorted(extracted_data_list, key=itemgetter('DrawNo'), reverse=True)
            for i in range(0, len(extracted_data_list_sorted)):
                DownloadData.AddDataToExistingXML(file_path, extracted_data_list_sorted[i], True)
       
    def UpdateToXML(file_path):
        xml = etree.parse(file_path)
        root = xml.getroot()
        last_draw_date =  datetime.strptime(str(xml.xpath("//Draw[1]/Date/text()")[0]), "%Y-%m-%dT%H:%M:%S")
        date_today = DownloadData.DATE_TODAY

        number_of_days_to_process = DownloadData.GetDaysBetweenDates(date_today, last_draw_date) + 1
        for i in range(0, number_of_days_to_process):
            DownloadData.Update(file_path, date_today)
        date_today = date_today + DT.timedelta(days=-1)     
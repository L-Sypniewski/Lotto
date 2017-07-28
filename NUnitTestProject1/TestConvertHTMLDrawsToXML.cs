using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

namespace Lotto
{
    [TestFixture]
    public class TestConvertHTMLDrawsToXML
    {
        static List<Losowanie> listMixed;
        static List<Losowanie> listWithPlus;
        static List<Losowanie> listWithoutPlus;


        private void runTest(List<Losowanie> dataList, int drawNumber, int [] date, byte? plus, int number1, int number2, int number3, int number4, int number5, int number6, int number7, int number8, int number9, int number10, int number11, int number12, int number13, int number14, int number15, int number16, int number17, int number18, int number19, int number20)
        {
            Losowanie draw = dataList.Select(d => d).Where(d => d.NrLosowania == drawNumber).First();
            DateTime dateTime = new DateTime(date[2], date[1], date[0], date[3], date[4], 0);
            #region Assertions
            Assert.AreEqual(drawNumber, draw.NrLosowania, "Parameter {0} does not match.", "Draw number");
            Assert.AreEqual(dateTime, draw.DataLosowania, "Parameter {0} does not match.", "Draw date");
            Assert.AreEqual(plus, draw.Plus, "Parameter {0} does not match.", "plus");
            Assert.AreEqual(number1, draw.Liczby[0], "Parameter {0} does not match.", "Number 1");
            Assert.AreEqual(number2, draw.Liczby[1], "Parameter {0} does not match.", "Number 2");
            Assert.AreEqual(number3, draw.Liczby[2], "Parameter {0} does not match.", "Number 3");
            Assert.AreEqual(number4, draw.Liczby[3], "Parameter {0} does not match.", "Number 4");
            Assert.AreEqual(number5, draw.Liczby[4], "Parameter {0} does not match.", "Number 5");
            Assert.AreEqual(number6, draw.Liczby[5], "Parameter {0} does not match.", "Number 6");
            Assert.AreEqual(number7, draw.Liczby[6], "Parameter {0} does not match.", "Number 7");
            Assert.AreEqual(number8, draw.Liczby[7], "Parameter {0} does not match.", "Number 8");
            Assert.AreEqual(number9, draw.Liczby[8], "Parameter {0} does not match.", "Number 9");
            Assert.AreEqual(number10, draw.Liczby[9], "Parameter {0} does not match.", "Number 10");
            Assert.AreEqual(number11, draw.Liczby[10], "Parameter {0} does not match.", "Number 11");
            Assert.AreEqual(number12, draw.Liczby[11], "Parameter {0} does not match.", "Number 12");
            Assert.AreEqual(number13, draw.Liczby[12], "Parameter {0} does not match.", "Number 13");
            Assert.AreEqual(number14, draw.Liczby[13], "Parameter {0} does not match.", "Number 14");
            Assert.AreEqual(number15, draw.Liczby[14], "Parameter {0} does not match.", "Number 15");
            Assert.AreEqual(number16, draw.Liczby[15], "Parameter {0} does not match.", "Number 16");
            Assert.AreEqual(number17, draw.Liczby[16], "Parameter {0} does not match.", "Number 17");
            Assert.AreEqual(number18, draw.Liczby[17], "Parameter {0} does not match.", "Number 18");
            Assert.AreEqual(number19, draw.Liczby[18], "Parameter {0} does not match.", "Number 19");
            Assert.AreEqual(number20, draw.Liczby[19], "Parameter {0} does not match.", "Number 20");
            #endregion
        }

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            string projectPath = Directory.GetParent(Path.GetDirectoryName(typeof(TestConvertHTMLDrawsToXML).Assembly.Location)).Parent.FullName + @"\Reference data";
            string mixedURL = SQLUtils.CreateLinkToSourceHTML(new DateTime(1996, 3, 18), new DateTime(2017, 7, 26, 23, 59, 59));
            string withPlusURL = SQLUtils.CreateLinkToSourceHTML(new DateTime(2009, 3, 30), new DateTime(2017, 7, 26, 23, 59, 59));
            string withoutPlusURL = SQLUtils.CreateLinkToSourceHTML(new DateTime(1996, 3, 18), new DateTime(2009, 3, 30, 23, 59, 59));
            listMixed = ConvertHTMLDrawsToXML.GetDrawsList(url: mixedURL, useLocalFile: true, localFilePath: projectPath + @"\test_mixed.html");
            listWithPlus = ConvertHTMLDrawsToXML.GetDrawsList(url: withPlusURL, useLocalFile: true, localFilePath: projectPath +@"\test_with_plus.html");
            listWithoutPlus = ConvertHTMLDrawsToXML.GetDrawsList(url: withoutPlusURL, useLocalFile: true, localFilePath: projectPath + @"\test_without_plus.html");
        }
        
        #region TestCases
        [TestCase(1, new int[] { 18, 3, 1996, 0, 0 }, null, 4, 9, 10, 16, 21, 22, 23, 26, 27, 34, 35, 41, 42, 48, 62, 66, 68, 73, 76, 78)]
        [TestCase(10350, new int[] { 26, 7, 2017, 14, 0 }, 73, 10, 11, 12, 15, 21, 24, 27, 30, 38, 39, 46, 56, 62, 66, 69, 73, 74, 76, 78, 79)]
        [TestCase(2, new int[] { 19, 3, 1996, 0, 0 }, null, 6, 12, 15, 19, 28, 33, 35, 39, 44, 48, 49, 59, 62, 63, 64, 67, 69, 71, 75, 77)]
        [TestCase(10349, new int[] { 25, 7, 2017, 21, 40 }, 34, 3, 5, 7, 15, 16, 17, 18, 19, 21, 24, 26, 28, 29, 30, 34, 47, 58, 70, 74, 76)]
        [TestCase(2517, new int[] { 26, 3, 2004, 0, 0 }, null, 6, 7, 9, 10, 16, 19, 20, 25, 26, 27, 45, 47, 50, 54, 55, 57, 61, 64, 69, 74)]
        [TestCase(5413, new int[] { 22, 10, 2010, 22, 15 }, 76, 9, 12, 14, 16, 20, 22, 24, 30, 31, 34, 38, 46, 55, 61, 63, 68, 74, 76, 79, 80)]
        [TestCase(7060, new int[] { 23, 1, 2013, 14, 0 }, 66, 4, 6, 9, 11, 13, 14, 16, 27, 34, 35, 44, 46, 48, 49, 51, 58, 61, 66, 71, 72)]
        [TestCase(3806, new int[] { 6, 10, 2007, 0, 0 }, null, 9, 15, 16, 18, 21, 26, 29, 31, 36, 40, 41, 42, 43, 46, 48, 49, 54, 57, 69, 75)]
        [TestCase(6542, new int[] { 9, 5, 2012, 14, 0 }, 42, 7, 8, 12, 13, 26, 29, 36, 42, 45, 49, 50, 51, 53, 56, 62, 66, 68, 69, 76, 78)]
        [TestCase(10234, new int[] { 29, 5, 2017, 14, 0 }, 49, 1, 9, 12, 15, 26, 28, 37, 42, 44, 49, 50, 55, 59, 61, 63, 65, 69, 76, 78, 80)]
        [TestCase(4809, new int[] { 24, 12, 2009, 22, 0 }, 5, 5, 7, 10, 17, 27, 31, 32, 37, 38, 44, 45, 46, 48, 51, 56, 58, 69, 71, 78, 80)]
        [TestCase(7928, new int[] { 2, 4, 2014, 14, 0 }, 70, 6, 7, 8, 13, 15, 18, 19, 26, 39, 44, 47, 51, 53, 61, 65, 66, 67, 69, 70, 76)]
        [TestCase(575, new int[] { 15, 10, 1998, 0, 0 }, null, 1, 10, 12, 16, 20, 32, 38, 43, 45, 50, 52, 60, 61, 63, 64, 66, 72, 73, 74, 77)]
        [TestCase(9569, new int[] { 30, 6, 2016, 21, 40 }, 20, 5, 8, 10, 11, 14, 20, 23, 30, 33, 35, 37, 38, 39, 51, 63, 70, 72, 73, 74, 79)]
        [TestCase(2616, new int[] { 3, 7, 2004, 0, 0 }, null, 3, 4, 5, 6, 7, 20, 26, 27, 29, 33, 34, 36, 43, 52, 56, 57, 58, 59, 72, 77)]
        [TestCase(4347, new int[] { 30, 3, 2009, 0, 0 }, 36, 3, 7, 24, 28, 30, 35, 36, 38, 42, 44, 59, 63, 64, 65, 71, 75, 77, 78, 79, 80)]
        [TestCase(4348, new int[] { 31, 3, 2009, 0, 0 }, 42, 2, 13, 15, 24, 25, 27, 42, 43, 45, 47, 49, 50, 52, 60, 61, 65, 69, 70, 71, 73)]
        [TestCase(4346, new int[] { 29, 3, 2009, 0, 0 }, null, 6, 7, 9, 16, 17, 19, 22, 26, 27, 35, 40, 45, 46, 47, 53, 55, 56, 59, 60, 61)]
        [TestCase(4345, new int[] { 28, 3, 2009, 0, 0 }, null, 2, 5, 9, 12, 15, 17, 19, 20, 30, 31, 32, 44, 45, 47, 54, 57, 61, 68, 70, 74)]
        [TestCase(4298, new int[] { 9, 2, 2009, 0, 0 }, null, 2, 6, 8, 9, 12, 17, 25, 39, 40, 42, 44, 49, 50, 55, 59, 61, 68, 69, 73, 76)]
        [TestCase(4416, new int[] { 7, 6, 2009, 0, 0 }, 66, 15, 18, 21, 23, 28, 30, 34, 37, 40, 41, 42, 45, 51, 52, 59, 66, 70, 71, 77, 80)]
        [TestCase(8871, new int[] { 17, 7, 2015, 21, 40 }, 8, 8, 10, 12, 16, 21, 22, 25, 28, 31, 33, 38, 39, 44, 49, 54, 60, 62, 75, 77, 78)]
        [TestCase(717, new int[] { 22, 4, 1999, 0, 0 }, null, 3, 4, 9, 10, 13, 16, 19, 25, 39, 45, 46, 49, 51, 57, 59, 62, 63, 66, 76, 80)]
        [TestCase(968, new int[] { 29, 12, 1999, 0, 0 }, null, 2, 5, 11, 12, 18, 19, 20, 30, 34, 35, 36, 37, 43, 46, 56, 58, 62, 69, 74, 75)]
        [TestCase(2814, new int[] { 17, 1, 2005, 0, 0 }, null, 1, 5, 6, 12, 13, 16, 20, 22, 25, 26, 32, 34, 39, 42, 47, 50, 56, 67, 71, 75)]
        [TestCase(1952, new int[] { 8, 9, 2002, 0, 0 }, null, 5, 8, 10, 19, 22, 24, 26, 27, 28, 32, 43, 52, 56, 60, 63, 64, 65, 69, 74, 79)]
        [TestCase(3690, new int[] { 12, 6, 2007, 0, 0 }, null, 2, 4, 8, 9, 10, 27, 36, 37, 38, 41, 42, 50, 55, 62, 65, 71, 76, 78, 79, 80)]
        [TestCase(63, new int[] { 9, 7, 1996, 0, 0 }, null, 5, 8, 9, 21, 24, 25, 29, 30, 34, 40, 45, 46, 47, 48, 53, 61, 63, 69, 73, 75)]
        [TestCase(3127, new int[] { 26, 11, 2005, 0, 0 }, null, 3, 6, 10, 12, 16, 25, 30, 33, 35, 36, 43, 51, 54, 59, 62, 63, 64, 68, 69, 74)]
        #endregion
        public void TestMixed(int drawNumber, int[] date, byte? plus, int number1, int number2, int number3, int number4, int number5, int number6, int number7, int number8, int number9, int number10, int number11, int number12, int number13, int number14, int number15, int number16, int number17, int number18, int number19, int number20)
        {
            runTest(listMixed, drawNumber, date, plus, number1, number2, number3, number4, number5, number6, number7, number8, number9, number10, number11, number12, number13, number14, number15,  number16, number17, number18, number19, number20);
        }


        #region TestCases
        [TestCase(10350, new int[] { 26, 7, 2017, 14, 0 }, 73, 10, 11, 12, 15, 21, 24, 27, 30, 38, 39, 46, 56, 62, 66, 69, 73, 74, 76, 78, 79)]
        [TestCase(10349, new int[] { 25, 7, 2017, 21, 40 }, 34, 3, 5, 7, 15, 16, 17, 18, 19, 21, 24, 26, 28, 29, 30, 34, 47, 58, 70, 74, 76)]
        [TestCase(5413, new int[] { 22, 10, 2010, 22, 15 }, 76, 9, 12, 14, 16, 20, 22, 24, 30, 31, 34, 38, 46, 55, 61, 63, 68, 74, 76, 79, 80)]
        [TestCase(7060, new int[] { 23, 1, 2013, 14, 0 }, 66, 4, 6, 9, 11, 13, 14, 16, 27, 34, 35, 44, 46, 48, 49, 51, 58, 61, 66, 71, 72)]
        [TestCase(6542, new int[] { 9, 5, 2012, 14, 0 }, 42, 7, 8, 12, 13, 26, 29, 36, 42, 45, 49, 50, 51, 53, 56, 62, 66, 68, 69, 76, 78)]
        [TestCase(10234, new int[] { 29, 5, 2017, 14, 0 }, 49, 1, 9, 12, 15, 26, 28, 37, 42, 44, 49, 50, 55, 59, 61, 63, 65, 69, 76, 78, 80)]
        [TestCase(4809, new int[] { 24, 12, 2009, 22, 0 }, 5, 5, 7, 10, 17, 27, 31, 32, 37, 38, 44, 45, 46, 48, 51, 56, 58, 69, 71, 78, 80)]
        [TestCase(7928, new int[] { 2, 4, 2014, 14, 0 }, 70, 6, 7, 8, 13, 15, 18, 19, 26, 39, 44, 47, 51, 53, 61, 65, 66, 67, 69, 70, 76)]
        [TestCase(9569, new int[] { 30, 6, 2016, 21, 40 }, 20, 5, 8, 10, 11, 14, 20, 23, 30, 33, 35, 37, 38, 39, 51, 63, 70, 72, 73, 74, 79)]
        [TestCase(4347, new int[] { 30, 3, 2009, 0, 0 }, 36, 3, 7, 24, 28, 30, 35, 36, 38, 42, 44, 59, 63, 64, 65, 71, 75, 77, 78, 79, 80)]
        [TestCase(4348, new int[] { 31, 3, 2009, 0, 0 }, 42, 2, 13, 15, 24, 25, 27, 42, 43, 45, 47, 49, 50, 52, 60, 61, 65, 69, 70, 71, 73)]
        [TestCase(4416, new int[] { 7, 6, 2009, 0, 0 }, 66, 15, 18, 21, 23, 28, 30, 34, 37, 40, 41, 42, 45, 51, 52, 59, 66, 70, 71, 77, 80)]
        [TestCase(8871, new int[] { 17, 7, 2015, 21, 40 }, 8, 8, 10, 12, 16, 21, 22, 25, 28, 31, 33, 38, 39, 44, 49, 54, 60, 62, 75, 77, 78)]
        [TestCase(5666, new int[] { 26, 2, 2011, 14, 0 }, 34, 12, 14, 15, 16 , 17, 24, 28, 32, 34, 35, 41, 42, 43, 47, 49, 50, 51, 54, 63, 72)]
        [TestCase(6153, new int[] { 27, 10, 2011, 22, 15 }, 62, 3, 4, 14, 15, 21, 27, 31, 39, 44, 47, 48, 57, 58, 62, 70, 71, 73, 77, 79, 80)]

        #endregion
        public void TestWithPlus(int drawNumber, int[] date, byte? plus, int number1, int number2, int number3, int number4, int number5, int number6, int number7, int number8, int number9, int number10, int number11, int number12, int number13, int number14, int number15, int number16, int number17, int number18, int number19, int number20)
        {
            runTest(listWithPlus, drawNumber, date, plus, number1, number2, number3, number4, number5, number6, number7, number8, number9, number10, number11, number12, number13, number14, number15, number16, number17, number18, number19, number20);
        }

        #region TestCases
        [TestCase(1, new int[] { 18, 3, 1996, 0, 0 }, null, 4, 9, 10, 16, 21, 22, 23, 26, 27, 34, 35, 41, 42, 48, 62, 66, 68, 73, 76, 78)]
        [TestCase(2, new int[] { 19, 3, 1996, 0, 0 }, null, 6, 12, 15, 19, 28, 33, 35, 39, 44, 48, 49, 59, 62, 63, 64, 67, 69, 71, 75, 77)]
        [TestCase(3806, new int[] { 6, 10, 2007, 0, 0 }, null, 9, 15, 16, 18, 21, 26, 29, 31, 36, 40, 41, 42, 43, 46, 48, 49, 54, 57, 69, 75)]
        [TestCase(575, new int[] { 15, 10, 1998, 0, 0 }, null, 1, 10, 12, 16, 20, 32, 38, 43, 45, 50, 52, 60, 61, 63, 64, 66, 72, 73, 74, 77)]
        [TestCase(2616, new int[] { 3, 7, 2004, 0, 0 }, null, 3, 4, 5, 6, 7, 20, 26, 27, 29, 33, 34, 36, 43, 52, 56, 57, 58, 59, 72, 77)]
        [TestCase(4346, new int[] { 29, 3, 2009, 0, 0 }, null, 6, 7, 9, 16, 17, 19, 22, 26, 27, 35, 40, 45, 46, 47, 53, 55, 56, 59, 60, 61)]
        [TestCase(4345, new int[] { 28, 3, 2009, 0, 0 }, null, 2, 5, 9, 12, 15, 17, 19, 20, 30, 31, 32, 44, 45, 47, 54, 57, 61, 68, 70, 74)]
        [TestCase(4298, new int[] { 9, 2, 2009, 0, 0 }, null, 2, 6, 8, 9, 12, 17, 25, 39, 40, 42, 44, 49, 50, 55, 59, 61, 68, 69, 73, 76)]
        [TestCase(717, new int[] { 22, 4, 1999, 0, 0 }, null, 3, 4, 9, 10, 13, 16, 19, 25, 39, 45, 46, 49 ,51, 57, 59, 62, 63, 66, 76, 80)]
        [TestCase(968, new int[] { 29, 12, 1999, 0, 0 }, null, 2, 5, 11, 12, 18, 19, 20, 30, 34, 35, 36, 37, 43, 46, 56, 58, 62, 69, 74, 75)]
        [TestCase(2814, new int[] { 17, 1, 2005, 0, 0 }, null, 1, 5, 6, 12, 13, 16 ,20, 22, 25, 26, 32, 34, 39, 42, 47, 50, 56, 67, 71, 75)]
        [TestCase(1952, new int[] { 8, 9, 2002, 0, 0 }, null, 5, 8, 10, 19, 22, 24, 26, 27, 28, 32, 43, 52, 56, 60, 63, 64, 65, 69, 74, 79)]
        [TestCase(3690, new int[] { 12, 6, 2007, 0, 0 }, null, 2, 4, 8, 9, 10, 27, 36, 37, 38, 41, 42, 50, 55, 62, 65, 71, 76, 78, 79, 80)]
        [TestCase(63, new int[] { 9, 7, 1996, 0, 0 }, null, 5, 8, 9, 21, 24, 25, 29, 30, 34, 40, 45, 46, 47, 48, 53, 61, 63, 69, 73, 75)]
        [TestCase(3127, new int[] { 26, 11, 2005, 0, 0 }, null, 3, 6, 10, 12, 16, 25, 30, 33, 35, 36, 43, 51, 54, 59, 62, 63, 64, 68, 69, 74)]
        #endregion
        public void TestWithoutPlus(int drawNumber, int[] date, byte? plus, int number1, int number2, int number3, int number4, int number5, int number6, int number7, int number8, int number9, int number10, int number11, int number12, int number13, int number14, int number15, int number16, int number17, int number18, int number19, int number20)
        {
            runTest(listWithoutPlus, drawNumber, date, plus, number1, number2, number3, number4, number5, number6, number7, number8, number9, number10, number11, number12, number13, number14, number15, number16, number17, number18, number19, number20);
        }
    }
}



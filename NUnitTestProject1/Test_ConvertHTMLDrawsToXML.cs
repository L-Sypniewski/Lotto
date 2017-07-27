using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Lotto
{
    [TestFixture]
    public class Test_ConvertHTMLDrawsToXML
    {
        static List<Losowanie> listAllDraws;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            listAllDraws = ConvertHTMLDrawsToXML.GetDrawsList(url: null, useLocalFile: true, localFilePath: @"C:\Users\Sypcio\Documents\Dropbox\Docs\Programming\!Visual Studio 2015\Projects\Lotto_HTML_to_XML\Lotto_HTML_to_XML\TestReference.html", saveSourceToLocalFile: false);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            // ...
        }

        [TestCase(1, new int[] { 18, 3, 1996, 0, 0 }, null, 4, 9, 10, 16, 21, 22, 23, 26, 27, 34, 35, 41, 42, 48, 62, 66, 68, 73, 76, 78)]
        [TestCase(10350, new int[] { 26, 7, 2017, 14, 0 }, 73, 10, 11, 12, 15, 21, 24, 27, 30, 38, 39, 46, 56, 62, 66, 69, 73, 74, 76, 78, 79)]
        public void TestMixed(int drawNumber, int[] date, byte? plus, int number1, int number2, int number3, int number4, int number5, int number6, int number7, int number8, int number9, int number10, int number11, int number12, int number13, int number14, int number15, int number16, int number17, int number18, int number19, int number20)
        {            
            Losowanie draw = listAllDraws.Select(d => d).Where(d => d.NrLosowania == drawNumber).First();
            DateTime dateTime = new DateTime(date[2], date[1], date[0], date[3], date[4], 0);

            #region Asserts
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
    }
}



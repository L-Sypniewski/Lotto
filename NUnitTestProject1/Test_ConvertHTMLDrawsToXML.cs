using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Lotto;

namespace Lotto
{
    [TestFixture]
    public class Test_ConvertHTMLDrawsToXML
    {
        [TestCase(10349, 3, 34)]
        [TestCase(10339, 6, 16)]
        [TestCase(5097, 1, 35)]
        [TestCase(1977, 4, null)]
        public void DivideTest(int DrawNumber, int number1, byte? plus)
        {
            List<Losowanie> listAllDraws = ConvertHTMLDrawsToXML.GetDrawsList(null, true, @"C:\Users\Sypcio\Documents\Dropbox\Docs\Programming\!Visual Studio 2015\Projects\Lotto_HTML_to_XML\Lotto_HTML_to_XML\TestReference.html", false);
            Losowanie draw = listAllDraws.Select(d => d).Where(d => d.NrLosowania == DrawNumber).First();

            Assert.AreEqual(DrawNumber, draw.NrLosowania);
            Assert.AreEqual(number1, draw.Liczby[0]);
            Assert.AreEqual(plus, draw.Plus);
        }
    }
}



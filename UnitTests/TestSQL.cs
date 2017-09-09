using NUnit.Framework;
using System.Collections.Generic;

namespace Lotto
{
    [TestFixture]
    public class TestSQL
    {
        static List<Draw> list;
        static List<Draw> list_sorted;
        static Lotto.SQLUtils.ConnectionString connectionString = new SQLUtils.ConnectionString(@"(localdb)\MSSQLLocalDB", "Lotto", true, "RawData");
       
        [OneTimeSetUp]
        public void setUp()
        {
            list = LoadXML.DeserializeXML();
            list_sorted = LoadXML.DeserializeXML();
            foreach (var draw in list_sorted)
            {
                draw.Numbers.Sort();
            }
        }

        [Test]
        public void TestSQLConnection()
        {
            Lotto.ExportDrawsToSQL exportDraws = new ExportDrawsToSQL(connectionString);
            Assert.IsTrue(SQLUtils.IsServerConnected(connectionString));
        }

        [Test]
        public void TestExportDrawsListToSQL()
        {
            Lotto.ExportDrawsToSQL exportDraws = new ExportDrawsToSQL(connectionString);
            exportDraws.ExportDrawsListToSQL(list);
        }        

        [Test]
        public void Testprocedure_ValidateDatabaseRowNumbers()
        {
            Assert.IsTrue(SQLQueries.procedure_ValidateDatabaseRowNumbers(connectionString.ToString()));
        }
    }
}
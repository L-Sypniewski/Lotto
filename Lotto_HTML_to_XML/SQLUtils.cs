using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotto
{
    public static class SQLUtils
    {
        public const string databaseFolderPath = @"C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\";

        //Creates new database. Returns 0 if succeded, throws an exception failed
        public static int CreateNewDatabase(ConnectionString connectionString, string folder)
        {
            string newDatabaseName = connectionString.databaseName;
            connectionString.databaseName = "master";
            string str = string.Format(@"CREATE DATABASE {0}", newDatabaseName, folder);
            
            using (SqlConnection con = new SqlConnection(connectionString.ToString()))
            {
                SqlCommand myCommand = new SqlCommand(str, con);
                try
                {
                    con.Open();
                    Console.WriteLine("");
                    myCommand.ExecuteNonQuery();
                    return 0;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw new Exception(ex.Message); //   ErrorCode	-2146232060	if database with a given name already exists
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // Checks if connection succeded
        public static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        public static bool IsServerConnected(ConnectionString connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        // Creates link which allows to get html source of draws within a chosen data range
        public static string CreateLinkToSourceHTML(DateTime from, DateTime to)
        {
            return string.Format("http://megalotto.pl/wyniki/multi-multi/losowania-od-{0}-do-{1}", ConvertDateToLinkString(from), ConvertDateToLinkString(to));
        }

        public static string ConvertDateToLinkString(DateTime dt)
        {
            string monthName = "";
            switch (dt.Month)
            {
                #region Months
                case 1:
                    monthName = "Stycznia";
                    break;
                case 2:
                    monthName = "Lutego";
                    break;
                case 3:
                    monthName = "Marca";
                    break;
                case 4:
                    monthName = "Kwietnia";
                    break;
                case 5:
                    monthName = "Maja";
                    break;
                case 6:
                    monthName = "Czerwca";
                    break;
                case 7:
                    monthName = "Lipca";
                    break;
                case 8:
                    monthName = "Sierpnia";
                    break;
                case 9:
                    monthName = "Września";
                    break;
                case 10:
                    monthName = "Października";
                    break;
                case 11:
                    monthName = "Listopada";
                    break;
                case 12:
                    monthName = "Grudnia";
                    break;
                    #endregion
            }
            return string.Format("{0}-{1}-{2}", dt.Day, monthName.ToString(), dt.Year);
        }

        public struct ConnectionString
        {
            public string serverName, databaseName, tableName;
            public bool trustedConnection;

            public ConnectionString(string serverName, string databaseName, bool trustedConnection, string tableName = null)
            {
                this.serverName = serverName;
                this.databaseName = databaseName;
                this.trustedConnection = trustedConnection;
                this.tableName = tableName;
            }
            public override string ToString()
            {
                if (this.tableName == null)
                    return string.Format(@"Server={0};Database={1};Trusted_Connection={2}", this.serverName, this.databaseName, this.trustedConnection.ToString());
                else
                    return string.Format(@"Server={0};Database={1};Database={2};Trusted_Connection={3}", this.serverName, this.databaseName, this.tableName, this.trustedConnection.ToString());

            }
        }
    }
}
